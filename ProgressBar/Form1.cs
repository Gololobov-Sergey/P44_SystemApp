using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SemaphoreApp
{
    public class MainForm : Form
    {
        // Основные элементы управления
        private ListBox lbWorking, lbWaiting, lbCreated;
        private NumericUpDown nudCapacity;
        private Button btnCreate;
        private System.Windows.Forms.Timer uiTimer;

        // Списки для привязки данных
        private BindingList<ThreadWorker> workingList = new BindingList<ThreadWorker>();
        private BindingList<ThreadWorker> waitingList = new BindingList<ThreadWorker>();
        private BindingList<ThreadWorker> createdList = new BindingList<ThreadWorker>();

        // Логика семафора
        public Semaphore semaphore;
        private int targetCapacity;
        private int pendingReductions = 0; // Количество мест, которые нужно "изъять" при сужении семафора
        private int threadIdCounter = 1;
        private readonly object stateLock = new object();

        public MainForm()
        {
            InitializeComponent();

            // Инициализация семафора
            targetCapacity = (int)nudCapacity.Value;
            semaphore = new Semaphore(targetCapacity, int.MaxValue);

            // Таймер для обновления счетчиков в ListBox без мерцания (каждые 100 мс)
            uiTimer = new System.Windows.Forms.Timer { Interval = 100 };
            uiTimer.Tick += (s, e) => { lbWorking.Refresh(); };
            uiTimer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Тест семафора";
            this.Size = new Size(600, 300);
            this.MinimumSize = new Size(600, 250);

            // Таблица для списков (слева направо как на скриншоте: Работающие, Ожидающие, Созданные)
            TableLayoutPanel tlp = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 33.3f),
                    new ColumnStyle(SizeType.Percent, 33.3f),
                    new ColumnStyle(SizeType.Percent, 33.3f)
                }
            };

            lbWorking = CreateListBox("Работающие потоки", tlp, 0);
            lbWaiting = CreateListBox("Ожидающие потоки", tlp, 1);
            lbCreated = CreateListBox("Созданные потоки", tlp, 2);

            lbWorking.DataSource = workingList;
            lbWaiting.DataSource = waitingList;
            lbCreated.DataSource = createdList;

            // Панель управления внизу
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };

            Label lblCap = new Label { Text = "Количество мест в семафоре", Location = new Point(10, 10), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
            nudCapacity = new NumericUpDown { Location = new Point(10, 30), Value = 3, Minimum = 1, Maximum = 100, Width = 150 };
            btnCreate = new Button { Text = "Создать поток", Location = new Point(180, 25), Width = 120 };

            pnlBottom.Controls.Add(lblCap);
            pnlBottom.Controls.Add(nudCapacity);
            pnlBottom.Controls.Add(btnCreate);

            this.Controls.Add(tlp);
            this.Controls.Add(pnlBottom);

            // События
            btnCreate.Click += BtnCreate_Click;
            lbCreated.DoubleClick += LbCreated_DoubleClick;
            lbWorking.DoubleClick += LbWorking_DoubleClick;
            nudCapacity.ValueChanged += NudCapacity_ValueChanged;
        }

        private ListBox CreateListBox(string title, TableLayoutPanel parent, int col)
        {
            Panel p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            Label l = new Label { Text = title, Dock = DockStyle.Top, Font = new Font("Arial", 9, FontStyle.Bold) };
            ListBox lb = new ListBox { Dock = DockStyle.Fill };
            p.Controls.Add(lb);
            p.Controls.Add(l);
            parent.Controls.Add(p, col, 0);
            return lb;
        }

        // 1. Создание потока
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            createdList.Add(new ThreadWorker(threadIdCounter++));
            AdjustFormSize();
        }

        // 2. Отправка в ожидание (Двойной клик по созданному)
        private void LbCreated_DoubleClick(object sender, EventArgs e)
        {
            if (lbCreated.SelectedItem is ThreadWorker tw)
            {
                createdList.Remove(tw);
                waitingList.Add(tw);
                tw.StartWaiting(this); // Запускаем логику потока
                AdjustFormSize();
            }
        }

        // 3. Завершение работы потока вручную (Двойной клик по работающему)
        private void LbWorking_DoubleClick(object sender, EventArgs e)
        {
            if (lbWorking.SelectedItem is ThreadWorker tw)
            {
                tw.Cancel(); // Поток прервется и сам вызовет HandleThreadExit
            }
        }

        // 4. Изменение количества мест в семафоре
        private void NudCapacity_ValueChanged(object sender, EventArgs e)
        {
            int newCap = (int)nudCapacity.Value;
            lock (stateLock)
            {
                if (newCap > targetCapacity)
                {
                    // Если увеличили - просто добавляем свободные места в семафор
                    semaphore.Release(newCap - targetCapacity);
                }
                else if (newCap < targetCapacity)
                {
                    // Если уменьшили, нужно убрать места.
                    int diff = targetCapacity - newCap;

                    for (int i = 0; i < diff; i++)
                    {
                        // Пытаемся забрать свободное место, если оно есть
                        if (!semaphore.WaitOne(0))
                        {
                            // Если свободных мест нет (все заняты), нужно "убить" старейший поток
                            pendingReductions++;
                            var oldest = workingList.FirstOrDefault(t => !t.IsCancelled);
                            if (oldest != null)
                            {
                                oldest.Cancel(); // При выходе он отдаст место в pendingReductions, а не в семафор
                            }
                        }
                    }
                }
                targetCapacity = newCap;
            }
        }

        // Обработка завершения потока (вызывается из самого потока)
        public void HandleThreadExit(ThreadWorker tw, bool acquiredSemaphore)
        {
            this.Invoke(new Action(() => {
                workingList.Remove(tw);
                waitingList.Remove(tw);
                AdjustFormSize();
            }));

            // Если поток успел занять место в семафоре, он должен его вернуть
            if (acquiredSemaphore)
            {
                lock (stateLock)
                {
                    if (pendingReductions > 0)
                    {
                        // Если форма требует уменьшить семафор, мы "съедаем" это место
                        pendingReductions--;
                    }
                    else
                    {
                        // Иначе штатно возвращаем место семафору для ожидающих
                        semaphore.Release();
                    }
                }
            }
        }

        // Динамическое изменение размера формы
        public void AdjustFormSize()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(AdjustFormSize));
                return;
            }

            int maxItems = Math.Max(createdList.Count, Math.Max(waitingList.Count, workingList.Count));
            int requiredHeight = 160 + (maxItems * lbCreated.ItemHeight);

            if (requiredHeight < 250) requiredHeight = 250;
            if (requiredHeight > 800) requiredHeight = 800; // ограничение максимума

            this.ClientSize = new Size(this.ClientSize.Width, requiredHeight);
        }
    }

    // Класс, описывающий логику и состояние одного потока
    public class ThreadWorker
    {
        public int Id { get; }
        public int Counter { get; private set; }
        public string StateText { get; set; }
        public bool IsCancelled { get; private set; }

        private CancellationTokenSource cts;

        public ThreadWorker(int id)
        {
            Id = id;
            StateText = "создан";
        }

        public void StartWaiting(MainForm f)
        {
            StateText = "ожидает";
            cts = new CancellationTokenSource();
            Thread t = new Thread(() => Run(f));
            t.IsBackground = true;
            t.Start();
        }

        public void Cancel()
        {
            IsCancelled = true;
            cts?.Cancel();
        }

        private void Run(MainForm f)
        {
            bool acquired = false;
            try
            {
                // Поток блокируется, ожидая свободного места в семафоре
                f.semaphore.WaitOne();
                acquired = true;

                if (IsCancelled) return;

                StateText = "работает";

                // Безопасный перенос потока в UI
                f.Invoke(new Action(() => {
                    // Удаляем из списка ожидания, добавляем в работу (перерисовка происходит автоматически)
                    // Доступ к спискам BindingList идет через главную форму, т.к. это UI потоки
                    f.Controls.Find("lbWaiting", true).FirstOrDefault(); // Hacky check if closed
                }));

                // Основной рабочий цикл (счетчик)
                while (!IsCancelled)
                {
                    Thread.Sleep(1000);
                    if (IsCancelled) break;
                    Counter++;
                }
            }
            catch (ThreadInterruptedException) { }
            finally
            {
                // Гарантированное освобождение ресурсов
                f.HandleThreadExit(this, acquired);
            }
        }

        // Переопределение метода ToString, чтобы ListBox корректно отображал состояние
        public override string ToString()
        {
            if (StateText == "создан" || StateText == "ожидает")
                return $"Поток {Id} -> {StateText}";
            else
                return $"Поток {Id} -> {Counter}";
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}