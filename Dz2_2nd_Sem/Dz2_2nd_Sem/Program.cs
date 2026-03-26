using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics.Eventing.Reader;

namespace Dz2_2nd_Sem
{
    internal class Program
    {
        const int S = 7;
        static string[] 
            shelfA = new string[S];
        static string[] shelfB = new string[S];

        static Journal<PlacedEvent> placedJournal = new Journal<PlacedEvent>();
        static Journal<TakenEvent> takenJournal = new Journal<TakenEvent>();
        static Journal<MovedEvent> movedJournal = new Journal<MovedEvent>();
        static Journal<FailedAttemptEvent> failedJournal = new Journal<FailedAttemptEvent>();

        static void Main(string[] args)
        {
            LoadAllFiles();
            while (true)
            {
                ShowInfo();
                Console.WriteLine("1 - Положить товар\r\n2 - Забрать товар\r\n3 - Перенести товар\r\n4 - Показать журналы\r\n5 - Выход\r\n");
                string choice = Console.ReadLine();

                if (choice == "5")
                {
                    SavePlaced();
                    SaveMoved();
                    SaveTaken();
                    SaveFailed();
                    Console.WriteLine("Программа завершена");
                    break;
                }

                switch (choice)
                {
                    case "1": PlaceItem(); break;
                    case "2": TakeItem(); break;
                    case "3": MoveItem(); break;
                    case "4": ShowJournals(); break;
                    default:
                        Console.WriteLine("Ошибка ввода");
                        break;
                }
            }
        }

        static void ShowInfo()
        {
            Console.WriteLine("Полка А: ");
            foreach (string el in shelfA)
            {
                Console.Write(el + " | ");
            }
            Console.WriteLine();
            Console.WriteLine("Полка В: ");
            foreach (string el in shelfB)
            {
                Console.Write(el + " | ");
            }
            Console.WriteLine();
        }
        static void PlaceItem()
        {
            Console.Write("Введите название полки(А/В): ");
            string currentShelf = Console.ReadLine();
            if (currentShelf == "A" || currentShelf == "B")
            {
                Console.Write($"Введите слот (1-{S}): ");
                bool correctInput = int.TryParse(Console.ReadLine(), out int slot);
                if (correctInput == false)
                {
                    Console.WriteLine("Ошибка ввода");
                    failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Ошибка ввода", "Размещение"));
                    return;
                }
                else if(slot > S || slot < 1)
                {
                    Console.WriteLine("Неверный номер слота");
                    failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Неверный номер слота", "Размещение"));
                    return;
                }
                else
                {
                    Console.Write("Введите название товара: ");
                    string nameProduct = Console.ReadLine();
                    if (currentShelf == "A")
                    {
                        if (shelfA[slot - 1] == null)
                        {
                            shelfA[slot - 1] = nameProduct;
                            placedJournal.Add(new PlacedEvent(currentShelf, slot, nameProduct));
                            Console.WriteLine("Успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Слот уже занят");
                            failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Слот занят", "Размещение"));
                            return;
                        }
                    }
                    if (currentShelf == "B")
                    {
                        if (shelfB[slot - 1] == null)
                        {
                            shelfB[slot - 1] = nameProduct;
                            placedJournal.Add(new PlacedEvent(currentShelf, slot, nameProduct));
                            Console.WriteLine("Успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Слот уже занят");
                            failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Слот занят", "Размещение"));
                            return;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("такой полки не существует");
                failedJournal.Add(new FailedAttemptEvent(currentShelf, 0, "Отсутcтвие полки", "Размещение"));
                return;
            }
        }

        static void TakeItem()
        {
            Console.Write("Введите название полки(А/В): ");
            string currentShelf = Console.ReadLine();
            if (currentShelf == "A" || currentShelf == "B")
            {
                Console.Write($"Введите слот (1-{S}): ");
                bool correctInput = int.TryParse(Console.ReadLine(), out int slot);
                if (correctInput == false)
                {
                    Console.WriteLine("Ошибка ввода");
                    failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Ошибка ввода", "Изъятие"));
                    return;
                }
                else if (slot > S || slot < 1)
                {
                    Console.WriteLine("Неверный номер слота");
                    failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Неверный номер слота", "Изъятие"));
                    return;
                }
                else
                {
                    if (currentShelf == "A")
                    {
                        if (shelfA[slot - 1] != null)
                        {
                            takenJournal.Add(new TakenEvent(currentShelf, slot, shelfA[slot - 1]));
                            shelfA[slot - 1] = null;
                            Console.WriteLine("Успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Слот и так пуст");
                            failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Слот пуст", "Изъятие"));
                            return;
                        }
                    }
                    if (currentShelf == "B")
                    {
                        if (shelfB[slot - 1] != null)
                        {
                            takenJournal.Add(new TakenEvent(currentShelf, slot, shelfB[slot - 1]));
                            shelfB[slot - 1] = null;
                            Console.WriteLine("Успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Слот и так пуст");
                            failedJournal.Add(new FailedAttemptEvent(currentShelf, slot, "Слот пуст", "Изъятие"));
                            return;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("такой полки не существует");
                failedJournal.Add(new FailedAttemptEvent(currentShelf, 0, "Отсутcтвие полки", "Изъятие"));
                return;
            }
        }

        static void MoveItem()
        {
            Console.Write("Откуда переносим (A/B): ");
            string fromShelfName = Console.ReadLine();
            Console.Write("Номер слота источника: ");
            bool correctInput = int.TryParse(Console.ReadLine(), out int fromSlot);
            if(correctInput == false)
            {
                Console.WriteLine("Ошибка ввода");
                failedJournal.Add(new FailedAttemptEvent(fromShelfName, fromSlot, "Ошибка ввода", "Изъятие"));
                return;
            }
            else if (fromSlot > S || fromSlot < 1)
            {
                Console.WriteLine("Неверный номер слота");
                failedJournal.Add(new FailedAttemptEvent(fromShelfName, fromSlot, "Неверный номер слота", "Изъятие"));
                return;
            }

            Console.Write("Куда переносим (A/B): ");
            string toShelfName = Console.ReadLine();
            Console.Write("Номер слота назначения: ");
            bool correctInput1 = int.TryParse(Console.ReadLine(), out int toSlot);
            if (correctInput1 == false || toSlot > S || toSlot < 1)
            {
                Console.WriteLine("Ошибка ввода");
                failedJournal.Add(new FailedAttemptEvent(toShelfName, toSlot, "Ошибка ввода", "Изъятие"));
                return;
            }
            else if (fromSlot > S || fromSlot < 1)
            {
                Console.WriteLine("Неверный номер слота");
                failedJournal.Add(new FailedAttemptEvent(toShelfName, toSlot, "Неверный номер слота", "Изъятие"));
                return;
            }

            string[] fromShelf = (fromShelfName == "A") ? shelfA : shelfB;
            string[] toShelf = (toShelfName == "A") ? shelfA : shelfB;

            if (fromShelf[fromSlot - 1] == null)
            {
                Console.WriteLine("Ошибка: Слот-источник пуст");
                failedJournal.Add(new FailedAttemptEvent(fromShelfName, fromSlot, "Слот-источник пуст", "Изъятие"));
                return;
            }
            if (toShelf[toSlot - 1] != null)
            {
                Console.WriteLine("Ошибка: Слот-назначение уже занят");
                failedJournal.Add(new FailedAttemptEvent(toShelfName, toSlot, "Слот-назначение пуст", "Изъятие"));
                return;
            }

            string itemName = fromShelf[fromSlot - 1];
            fromShelf[fromSlot - 1] = null;
            toShelf[toSlot - 1] = itemName;

            movedJournal.Add(new MovedEvent(fromShelfName, fromSlot, toShelfName, toSlot, itemName));

            Console.WriteLine($"Успешно: '{itemName}' перемещен!");
        }


        static void ShowJournals()
        {
            Console.WriteLine("--- Журнал размещений ---");
            foreach (var el in placedJournal.GetAll()) {Console.WriteLine(el.ToScreenLine());}

            Console.WriteLine("--- Журнал изъятий ---");
            foreach (var el in takenJournal.GetAll()) {Console.WriteLine(el.ToScreenLine());}

            Console.WriteLine("--- Журнал перемещений ---");
            foreach (var el in movedJournal.GetAll()) {Console.WriteLine(el.ToScreenLine());}

            Console.WriteLine("--- Журнал неуспешных попыток ---");
            foreach (var el in failedJournal.GetAll()) { Console.WriteLine(el.ToScreenLine()); }
        }

        static void SavePlaced()
        {
            StreamWriter writer = new StreamWriter("Placed.log", false);
            var allPlaced = placedJournal.GetAll();
            foreach(var el in allPlaced)
            {
                writer.WriteLine(el.ToLogLine());
            }
            writer.Close();
        }

        static void SaveTaken()
        {
            StreamWriter writer = new StreamWriter("Taken.log", false);
            var allTaken = takenJournal.GetAll();
            foreach(var el in allTaken)
            {
                writer.WriteLine(el.ToLogLine());   
            }
            writer.Close();
        }

        static void SaveMoved()
        {
            StreamWriter writer = new StreamWriter("Moved.log", false);
            var allMoved = movedJournal.GetAll();
            foreach (var el in allMoved)
            {
                writer.WriteLine(el.ToLogLine());
            }
            writer.Close();
        }

        static void SaveFailed()
        {
            StreamWriter writer = new StreamWriter("Failed.log", false);
            var allFailed = failedJournal.GetAll();
            foreach (var el in allFailed)
            {
                writer.WriteLine(el.ToLogLine());
            }
            writer.Close();
        }

        static void LoadAllFiles()
        {
            if (File.Exists("Placed.log"))
            {
                foreach (var line in File.ReadAllLines("Placed.log"))
                {
                    var ev = PlacedEvent.FromLogLine(line);
                    placedJournal.Add(ev);
                    if (ev.Shelf == "A")
                        shelfA[ev.Slot - 1] = ev.ItemName;
                    else if (ev.Shelf == "B")
                        shelfB[ev.Slot - 1] = ev.ItemName;
                }
            }

            if (File.Exists("Taken.log"))
            {
                foreach(var line in File.ReadAllLines("Taken.log"))
                {
                    var ev = TakenEvent.FromLogLine(line);
                    takenJournal.Add(ev);
                    if (ev.Shelf == "A")
                        shelfA[ev.Slot - 1] = ev.ItemName;
                    else if(ev.Shelf == "B")
                        shelfB[ev.Slot - 1] = ev.ItemName;
                }
            }

            if (File.Exists("Moved.log"))
            {
                foreach (var line in File.ReadAllLines("Moved.log"))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    var ev = MovedEvent.FromLogLine(line);
                    movedJournal.Add(ev);
                    if (ev.FromShelf == "A")
                        shelfA[ev.FromSlot - 1] = null;
                    else
                        shelfB[ev.FromSlot - 1] = null;
                    if (ev.ToShelf == "A")
                        shelfA[ev.ToSlot - 1] = ev.ItemName;
                    else
                        shelfB[ev.ToSlot - 1] = ev.ItemName;
                }
            }
        }
    }
}

