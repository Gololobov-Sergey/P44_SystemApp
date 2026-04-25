using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DancingBarsWPF
{
    public partial class MainWindow : Window
    {
        private bool _isRunning = false; // Флаг для остановки предыдущих анимаций при перезапуске

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(CountTextBox.Text, out int count) || count <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректне число (більше нуля).");
                return;
            }

            // Останавливаем старые процессы и очищаем окно
            _isRunning = false;
            BarsContainer.Children.Clear();
            _isRunning = true;

            // Создаем прогресс-бары
            for (int i = 0; i < count; i++)
            {
                var pb = new ProgressBar
                {
                    Height = 20,
                    Margin = new Thickness(0, 0, 0, 10),
                    Minimum = 0,
                    Maximum = 100,
                    Value = 0,
                    // Убираем стандартную анимацию "блика" Windows, чтобы она не мешала нашей
                    IsIndeterminate = false
                };

                BarsContainer.Children.Add(pb);

                // Запускаем асинхронный "поток" логики для каждого бара индивидуально
                StartDancingTask(pb);
            }
        }

        private async void StartDancingTask(ProgressBar pb)
        {
            // Создаем отдельный генератор случайностей для каждой задачи,
            // чтобы потоки не конфликтовали между собой.
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            while (_isRunning)
            {
                // 1. Генерируем случайную цель (от 10% до 100%)
                double targetValue = rnd.Next(10, 101);

                // 2. Генерируем случайное время "полета" бара (от 0.5 до 1.5 секунд)
                double durationSeconds = rnd.NextDouble() * 1.0 + 0.5;

                // 3. Генерируем случайный цвет
                byte r = (byte)rnd.Next(50, 255);
                byte g = (byte)rnd.Next(50, 255);
                byte b = (byte)rnd.Next(50, 255);
                Color randomColor = Color.FromRgb(r, g, b);

                // Передаем команду интерфейсу (UI потоку) нарисовать анимацию
                pb.Dispatcher.Invoke(() =>
                {
                    // Меняем цвет заливки бара
                    pb.Foreground = new SolidColorBrush(randomColor);

                    // Настраиваем плавную анимацию значения
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        To = targetValue,
                        Duration = TimeSpan.FromSeconds(durationSeconds),
                        // QuadraticEase делает так, что бар разгоняется и тормозит плавно, а не бьется о края
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                    };

                    // Запускаем анимацию!
                    pb.BeginAnimation(ProgressBar.ValueProperty, animation);
                });

                // Асинхронно ждем, пока анимация закончится, прежде чем генерировать новую цель.
                // Это не вешает программу, так как Task.Delay освобождает поток.
                await Task.Delay(TimeSpan.FromSeconds(durationSeconds));
            }
        }
    }
}