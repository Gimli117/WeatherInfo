using System.Net.Quic;

namespace WeatherInfo
{
    internal class Program
    {
        public static string[] listAllValues = File.ReadAllLines("../../../../WeatherInfo/Tempdata.txt");
        static void Main()
        {
            bool quit = false;

            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            while (!quit)
            {
                Console.Clear();

                Console.WriteLine("Welcome to the Weather App!\n");
                Console.WriteLine("Please select a command:\n\n");
                Console.WriteLine("1 - Info");
                Console.WriteLine("2 - Show meteorological Fall");
                Console.WriteLine("3 - Show meteorological Winter");
                Console.WriteLine("4 - Create Text File");
                Console.WriteLine("5 - Prints ALL data");
                Console.WriteLine("ESC - Exits the Program");

                ConsoleKeyInfo key = Console.ReadKey(true);

                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Info.Run();
                        break;

                    case ConsoleKey.D2:

                        break;

                    case ConsoleKey.D3:

                        break;

                    case ConsoleKey.D4:
                        CreateTextFile.Run();
                        break;

                    case ConsoleKey.D5:
                        int index = 1;
                        foreach (var line in listAllValues)
                        {
                            Console.WriteLine($"{index}: {line}");
                            index++;
                        }
                        Console.WriteLine("\n\nEnter to go back");
                        Console.ReadLine();
                        break;

                    case ConsoleKey.Escape:
                        quit = true;
                        break;
                }
            }
        }
    }
}