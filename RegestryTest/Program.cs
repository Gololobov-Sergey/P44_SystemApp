using Microsoft.Win32;

namespace RegestryTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RegistryKey registryKey = Registry.CurrentUser;
            //RegistryKey soft = registryKey.OpenSubKey("Software", true);
            //RegistryKey myApp = soft.CreateSubKey("MyApp");
            //myApp.SetValue("AppName", "RegestryTest");
            //myApp.SetValue("user", "admin");
            //myApp.SetValue("password", "12345");
            //myApp.Close();



            //RegistryKey registryKey = Registry.CurrentUser;
            //RegistryKey soft = registryKey.OpenSubKey("Software\\MyApp", true);
            //RegistryKey myApp = soft.CreateSubKey("Settings", true);
            //myApp.SetValue("Color", "Red");
            //myApp.Close();



            //RegistryKey registryKey = Registry.CurrentUser;
            //RegistryKey soft = registryKey.OpenSubKey("Software\\MyApp\\Settings", true);
            //string color = soft.GetValue("Color").ToString();
            //Console.BackgroundColor = color switch
            //{
            //    "Red" => ConsoleColor.Red,
            //    "Green" => ConsoleColor.Green,
            //    "Blue" => ConsoleColor.Blue,
            //    _ => ConsoleColor.Black // Default case
            //};
            //Console.Clear();


            //RegistryKey registryKey = Registry.CurrentUser;
            //RegistryKey key = registryKey.OpenSubKey("Software\\MyApp\\Settings", true);
            //key.DeleteValue("Color");
            //key.Close();


            RegistryKey registryKey = Registry.CurrentUser;
            RegistryKey key = registryKey.OpenSubKey("Software\\MyApp", true);
            key.DeleteSubKey("Settings");
            key.Close();



            //Напишите программу, которая будет подсчитывать количество собственных
            //запусков, и когда параметр реестра достигнет 0, программа перестанет запускаться.Так
            //же добавьте в программу возможность самостоятельно установить параметр, в реестре
            //отвечающий за количество запусков.


            Console.ReadLine();
        }
    }
}
