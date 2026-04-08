using System.Diagnostics;

namespace ProcessApp
{
    public partial class Form1 : Form
    {
        Process[] processes;

        public Form1()
        {
            InitializeComponent();
        }

        void UpdateProcessList()
        {
            listBox1.Items.Clear();

            processes = Process.GetProcesses();
            Array.Sort(processes, (x, y) => string.Compare(x.ProcessName, y.ProcessName, StringComparison.OrdinalIgnoreCase));
            foreach (Process process in processes)
            {
                listBox1.Items.Add($"{process.Id,8}   {process.ProcessName}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = (int)numericUpDown1.Value * 1000;

            UpdateProcessList();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value * 1000;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                int index = lb.SelectedIndex;
                MessageBox.Show($"Process ID: {processes[index].Id}\nProcess Name: {processes[index].ProcessName}\n{processes[index].StartTime}", "Process Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
