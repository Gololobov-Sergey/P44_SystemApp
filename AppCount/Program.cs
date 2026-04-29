using Microsoft.Win32;

namespace AppCount
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string subKey = @"Software\RunCounter";
            const string valueName = "RunCount";
            const int freeRunsLimit = 18;

            using var key = Registry.CurrentUser.CreateSubKey(subKey, writable: true);
            object? raw = key.GetValue(valueName, 0);

            int runCount;
            if (raw is int i) runCount = i;
            else if (raw is string s && int.TryParse(s, out var p)) runCount = p;
            else
            {
                try { runCount = Convert.ToInt32(raw); }
                catch { runCount = 0; }
            }


            if (args.Length > 0 && int.TryParse(args[0], out int manualSetCount))
            {
                manualSetCount = Math.Max(manualSetCount, 0);
                runCount = manualSetCount;
                key.SetValue(valueName, runCount, RegistryValueKind.DWord);
                Console.WriteLine($"Счётчик вручную установлен: {runCount}");
                return;
            }


            if (runCount < freeRunsLimit)
            {
                runCount = runCount + 1;
                key.SetValue(valueName, runCount, RegistryValueKind.DWord);
                int remaining = Math.Max(freeRunsLimit - runCount, 0);
                Console.WriteLine($"Программа запущена. Всего запусков: {runCount}. Бесплатных запусков осталось: {remaining}.");
                return;
            }

            // Бесплатные запуски исчерпаны
            Console.WriteLine($"Бесплатные запуски исчерпаны (лимит {freeRunsLimit}). Таки платить надо чтоб запускать)) .");
            Console.WriteLine($"Всего запусков: {runCount}.");
        }
    }
}
