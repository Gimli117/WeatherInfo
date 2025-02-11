using System.Net.Quic;

namespace WeatherInfo
{
    internal class Program
    {
        public static string[]? listAllValues;
        static void Main()
        {
            bool quit = false;

            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            listAllValues = File.ReadAllLines("../../../../WeatherInfo/Tempdata.txt");

            while (!quit)
            {
                Console.Clear();

                Console.WriteLine("Welcome to the Weather App!\n");
                Console.WriteLine("Please select a command:\n\n");
                Console.WriteLine("1 - Indoor Info");
                Console.WriteLine("2 - Outdoor Info");

                ConsoleKeyInfo key = Console.ReadKey();

                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        IndoorInfo.Run();
                        break;

                    case ConsoleKey.D2:
                        OutdoorInfo.Run();
                        break;
                }
            }
        }
    }
}