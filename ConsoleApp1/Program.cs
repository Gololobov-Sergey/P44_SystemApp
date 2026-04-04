using Student;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Student.Student student = new Student.Student
            //{
            //    Id = 1,
            //    Name = "John",
            //    BirthDay = new DateOnly(2000, 1, 1)
            //};

            //Console.WriteLine(student);

            //Console.WriteLine(Ext.MessageBox(IntPtr.Zero, "Hello, World!", "Message", (int)MessageBoxType.MB_ABORTRETRYIGNORE | (int)MessageBoxIcon.MB_ICONASTERISK));

            //IntPtr hWnd = Ext.FindWindowByTitle(IntPtr.Zero, "Калькулятор");

            //if (hWnd == IntPtr.Zero)
            //{
            //    Console.WriteLine("Калькулятор не найден.");
            //}
            //else
            //{
            //    //Console.WriteLine(hWnd);
            //    //Console.WriteLine("Калькулятор найден.");
            //    //Console.ReadLine();
            //    //Ext.ShowWindow(hWnd, (int)ShowWindowCommands.SW_MAXIMIZE); // SW_SHOW

            //    //Console.ReadLine();
            //    //Ext.ShowWindow(hWnd, (int)ShowWindowCommands.SW_HIDE); // SW_SHOW

            //    //Console.ReadLine();
            //    //Ext.ShowWindow(hWnd, (int)ShowWindowCommands.SW_NORMAL); // SW_SHOW
            //}


            while (true)
            {
                IntPtr hWnd = Ext.FindWindowByTitle(IntPtr.Zero, "Калькулятор");
                if (hWnd != IntPtr.Zero)
                {
                    Ext.SendMessage(hWnd, (uint)WindowsMessages.WM_CLOSE, 0, 0);
                }

                Thread.Sleep(3000);
            }

        }
    }
}
