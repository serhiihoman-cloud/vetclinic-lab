using System;
using System.Globalization;
using System.Text;

namespace VetClinic
{
    internal class Program
    {
        // Сталі ціни (змінних у полях класу більше немає, дані передаються параметрами)
        const double ConsultationFee = 300.0; // вартість консультації, грн
        const double MedicinePerKg = 12.5;    // вартість ліків за 1 кг ваги, грн

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            // Дані живуть тут, у Main, і передаються в методи параметрами
            string[] species = { "Собаки", "Коти", "Птахи", "Гризуни", "Рептилії" };
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
            double[] costs = new double[100]; // вартості прийомів
            int count = 0;                    // скільки елементів масиву зайнято

            while (running)
            {
                ShowMenu();
                string choice = (Console.ReadLine() ?? "").Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowVisitsTable(species, days); Pause(); break;
                    case "4":
                        AddValue(costs, ref count, ReadDouble("Вартість прийому, грн", 0, 100000), costs.Length);
                        Console.WriteLine($"Записів у списку: {count}");
                        Pause();
                        break;
                    case "5": PrintValues(costs, count); Pause(); break;
                    case "6": ShowStatistics(costs, count); Pause(); break;
                    case "7": ShowReferenceDemo(); Pause(); break;
                    case "0": running = false; break;
                    default: ShowUnknownChoice(); Pause(); break;
                }
            }

            Console.WriteLine("До побачення.");
        }

        // Виводить меню. Нічого не читає й не повертає
        static void ShowMenu()
        {
            Console.WriteLine("=== Ветеринарна клініка ===");
            Console.WriteLine("1 — довідка про застосунок");
            Console.WriteLine("2 — вартість прийому залежно від ваги тварини");
            Console.WriteLine("3 — прийоми за видами тварин і днями тижня");
            Console.WriteLine("4 — додати вартість прийому");
            Console.WriteLine("5 — показати вартості прийомів");
            Console.WriteLine("6 — статистика вартостей");
            Console.WriteLine("7 — дослід: значиме і посилальне");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        // Повідомлення про неправильний пункт меню
        static void ShowUnknownChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
        }

        // Пауза після дії: щоб результат не зник разом з очищенням екрана
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Пункт 1: довідка про тему, сутність і поля
        static void ShowAbout()
        {
            Console.WriteLine("Тема проєкту: №6 «Ветеринарна клініка».");
            Console.WriteLine("Головна сутність: «Прийом» (полів: 8).");
            Console.WriteLine("Довідник: «Вид» тварини.");
            Console.WriteLine("Підсумкова величина: кількість прийомів за видами тварин.");
            Console.WriteLine();
            Console.WriteLine("Поля сутності «Прийом»:");
            Console.WriteLine("{0,-4}{1,-28}{2}", "№", "Поле", "Тип");
            Console.WriteLine("{0,-4}{1,-28}{2}", "1", "номер картки", "int");
            Console.WriteLine("{0,-4}{1,-28}{2}", "2", "кличка тварини", "string");
            Console.WriteLine("{0,-4}{1,-28}{2}", "3", "код виду (з довідника)", "int");
            Console.WriteLine("{0,-4}{1,-28}{2}", "4", "вік", "int");
            Console.WriteLine("{0,-4}{1,-28}{2}", "5", "прізвище власника", "string");
            Console.WriteLine("{0,-4}{1,-28}{2}", "6", "дата прийому", "DateTime");
            Console.WriteLine("{0,-4}{1,-28}{2}", "7", "діагноз", "string");
            Console.WriteLine("{0,-4}{1,-28}{2}", "8", "вартість", "double");
            Console.WriteLine();
            Console.WriteLine("Поля довідника «Вид»: код (int), назва (string), чи активний (bool).");
        }

        // Пункт 2: питає межі й крок, потім викликає табулювання
        static void ShowCostTable()
        {
            double start = ReadDouble("Початок проміжку ваги, кг", 0.1, 50);
            double end = ReadDouble("Кінець проміжку ваги, кг", start, 50);
            double step = ReadDouble("Крок, кг", 0.25, 10);

            Console.WriteLine();
            PrintCostTable(start, end, step);
        }

        // Табулювання: початок, кінець і крок приходять параметрами.
        // Виводить таблицю, а ціну кожного рядка бере з методів-обчислювачів
        static void PrintCostTable(double start, double end, double step)
        {
            if (step <= 0)
            {
                Console.WriteLine("Помилка: крок має бути більшим за нуль.");
                return; // рання відмова: інакше цикл ніколи б не закінчився
            }

            Console.WriteLine("Вартість прийому залежно від ваги тварини");
            Console.WriteLine();
            Console.WriteLine("{0,10}{1,14}{2,16}", "Вага, кг", "Ліки, грн", "Прийом, грн");
            Console.WriteLine(new string('-', 40));

            // + 1e-9 — запас на похибку дробових чисел, щоб не зникла остання стрічка
            for (double x = start; x <= end + 1e-9; x += step)
            {
                double medicine = CalculateMedicineCost(x, MedicinePerKg);
                double total = CalculateVisitCost(ConsultationFee, medicine);
                Console.WriteLine($"{x,10:F2}{medicine,14:F2}{total,16:F2}");
            }
        }

        // Рахує тільки вартість ліків і повертає її, нічого не виводить
        static double CalculateMedicineCost(double weightKg, double pricePerKg)
        {
            return weightKg * pricePerKg;
        }

        // Вартість прийому = консультація + ліки. Теж лише повертає значення
        static double CalculateVisitCost(double consultationFee, double medicineCost)
        {
            return consultationFee + medicineCost;
        }

        // Пункт 3: питає розміри таблиці й викликає виведення
        static void ShowVisitsTable(string[] species, string[] days)
        {
            int rows = ReadInt("Скільки видів показати", 1, species.Length);
            int cols = ReadInt("Скільки днів показати", 1, days.Length);

            Console.WriteLine();
            PrintVisitsTable(species, days, rows, cols);
        }

        // Кількість прийомів для виду з номером speciesIndex у день dayIndex.
        // рахуємо за формулою
        static int CountVisits(int speciesIndex, int dayIndex)
        {
            return (speciesIndex * 3 + dayIndex * 5) % 7 + 1;
        }

        // Виводить таблицю «вид × день» з підсумками рядків, стовпців і загальним
        static void PrintVisitsTable(string[] species, string[] days, int rows, int cols)
        {
            Console.WriteLine("Кількість прийомів за видами тварин і днями тижня");
            Console.WriteLine();

            int[] columnTotals = new int[cols]; // підсумки стовпців
            int grandTotal = 0;                 // загальний підсумок

            // Шапка: до зовнішнього циклу
            Console.Write("{0,-12}", "Вид");
            for (int c = 0; c < cols; c++)
            {
                Console.Write("{0,6}", days[c]);
            }
            Console.WriteLine("{0,8}", "Усього");
            Console.WriteLine(new string('-', 12 + 6 * cols + 8));

            for (int r = 0; r < rows; r++)
            {
                int rowTotal = 0; // лічильник підсумку рядка: свій для кожного рядка
                Console.Write("{0,-12}", species[r]);

                for (int c = 0; c < cols; c++)
                {
                    int visits = CountVisits(r, c);

                    Console.Write("{0,6}", visits);
                    rowTotal += visits;
                    columnTotals[c] += visits;
                }

                Console.WriteLine("{0,8}", rowTotal); // перехід на новий рядок — після внутрішнього циклу
                grandTotal += rowTotal;
            }

            // Рядок підсумків під таблицею
            Console.WriteLine(new string('-', 12 + 6 * cols + 8));
            Console.Write("{0,-12}", "Усього");
            for (int c = 0; c < cols; c++)
            {
                Console.Write("{0,6}", columnTotals[c]);
            }
            Console.WriteLine("{0,8}", grandTotal);
        }

        // Додає значення в кінець масиву. Масив іде без ref (він і так посилальний),
        // а лічильник — із ref, бо int інакше передався б копією
        static void AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count == capacity)
            {
                Console.WriteLine("Місця більше немає.");
                return;
            }

            values[count] = newValue;
            count++; // змінюється лічильник у Main
        }

        // Виводить перші count елементів таблицею з номерами. З клавіатури нічого не читає
        static void PrintValues(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            Console.WriteLine("Вартості прийомів");
            Console.WriteLine();
            Console.WriteLine("{0,4}{1,16}", "№", "Вартість, грн");
            Console.WriteLine(new string('-', 20));

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1,4}{values[i],16:F2}");
            }
        }

        // Шукає найбільше значення й його номер (з 1, як у таблиці).
        // Два результати віддаються через out. Повертає false для порожнього набору
        static bool TryFindMax(double[] values, int count, out double max, out int maxNumber)
        {
            if (count == 0)
            {
                max = 0; // out-параметри треба присвоїти в кожній гілці
                maxNumber = 0;
                return false;
            }

            max = values[0];
            maxNumber = 1;

            for (int i = 1; i < count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxNumber = i + 1;
                }
            }

            return true;
        }

        // Сума й середнє через out. Перевірка count > 0 захищає від ділення на нуль
        static void Statistics(double[] values, int count, out double sum, out double average)
        {
            sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }

            average = count > 0 ? sum / count : 0;
        }

        // Пункт «статистика»: використовує обидва методи вище й виводить результат
        static void ShowStatistics(double[] values, int count)
        {
            if (!TryFindMax(values, count, out double max, out int maxNumber))
            {
                Console.WriteLine("Список порожній, статистики немає.");
                return;
            }

            Statistics(values, count, out double sum, out double average);

            Console.WriteLine($"Записів:          {count}");
            Console.WriteLine($"Сума:             {sum:F2} грн");
            Console.WriteLine($"Середня вартість: {average:F2} грн");
            Console.WriteLine($"Найдорожчий:      {max:F2} грн (запис №{maxNumber})");
        }

        // Спроба змінити int без ref: змінюється лише копія всередині методу
        static void IncreaseInt(int value)
        {
            value = value + 100;
        }

        // Зміна елемента масиву: масив передається посиланням, тому зміна видна в Main
        static void IncreaseFirst(double[] prices)
        {
            prices[0] = prices[0] + 100;
        }

        // Дослід для звіту: вивід до й після виклику обох методів
        static void ShowReferenceDemo()
        {
            int price = 50;
            double[] prices = { 50, 70, 90 };

            Console.WriteLine("Дослід: значиме проти посилального");
            Console.WriteLine();
            Console.WriteLine($"До виклику:    price = {price}, prices[0] = {prices[0]}");

            IncreaseInt(price);
            IncreaseFirst(prices);

            Console.WriteLine($"Після виклику: price = {price}, prices[0] = {prices[0]}");
            Console.WriteLine();
            Console.WriteLine("int передався копією, тому зміна всередині методу зникла.");
            Console.WriteLine("Масив передався посиланням, тому змінився сам елемент масиву.");
        }

        // Питає ціле число, поки не отримає число з проміжку [min; max].
        // Нічого не друкує, крім підказки й повідомлень про помилку
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write($"{prompt} (від {min} до {max}): ");
                string text = (Console.ReadLine() ?? "").Trim();

                if (!int.TryParse(text, out int value))
                {
                    Console.WriteLine("Помилка: потрібно ввести ціле число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        // Те саме для дробового числа. Приймає і кому, і крапку (0,5 та 0.5)
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write($"{prompt} (від {min} до {max}): ");
                string text = (Console.ReadLine() ?? "").Trim();

                if (!double.TryParse(text.Replace(',', '.'), NumberStyles.Float,
                                     CultureInfo.InvariantCulture, out double value)
                    || double.IsNaN(value))
                {
                    Console.WriteLine("Помилка: потрібно ввести число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }
    }
}