using System;
using System.Text;

namespace VetClinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                Console.WriteLine("=== Ветеринарна клініка ===");
                Console.WriteLine("1 — довідка про застосунок");
                Console.WriteLine("2 — вартість прийому залежно від ваги тварини");
                Console.WriteLine("3 — прийоми за видами тварин і днями тижня");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");

                // Вибір читаємо як рядок: порожній Enter чи літера не зламають програму
                string choice = (Console.ReadLine() ?? "").Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        ShowVisitsTable();
                        Pause();
                        break;
                    case "0":
                        running = false; // єдиний спосіб вийти з циклу
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        // Пауза після дії: щоб результат не зник разом з очищенням екрана
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Заготовка: буде реалізовано в наступному коміті
        static void ShowAbout()
        {
            Console.WriteLine("Довідка буде додана пізніше.");
        }

        // Заготовка: буде реалізовано в наступному коміті
        static void ShowCostTable()
        {
            Console.WriteLine("Табулювання буде додано пізніше.");
        }

        // Заготовка: буде реалізовано в наступному коміті
        static void ShowVisitsTable()
        {
            Console.WriteLine("Таблицю буде додано пізніше.");
        }
    }
}