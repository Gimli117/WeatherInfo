using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInfo
{
    internal class Info
    {
        public static void Run()
        {
            bool quit = false;
            bool input = false;
            string infoString = "";
            while (!input)
            {
                Console.WriteLine("Begin by selecting Outside info or Inside info:\n");
                Console.WriteLine("1 - Outside");
                Console.WriteLine("2 - Inside");
                Console.WriteLine("\nQ - Return to Main Menu");

                ConsoleKeyInfo infoKey = Console.ReadKey(true);

                Console.Clear();

                if (infoKey.Key == ConsoleKey.D1)
                {
                    infoString = "Outside";
                    input = true;
                }
                else if (infoKey.Key == ConsoleKey.D2)
                {
                    infoString = "Inside";
                    input = true;
                }
                else if (infoKey.Key == ConsoleKey.Q)
                {
                    quit = true;
                    input = true;
                }
                else
                {
                    Console.WriteLine("Try again.");
                    Thread.Sleep(2000);
                    Console.Clear();
                }
            }

            while (!quit)
            {
                Console.Clear();

                Console.WriteLine($"{infoString} Info.\n\n");
                Console.WriteLine("1 - Search for Date and see Average Temp & Humidity");
                Console.WriteLine("2 - Sort Hottest to Coldest day (Average Temp per day)");
                Console.WriteLine("3 - Sort Dryest to Dampest day (Average Humidity per day)");
                Console.WriteLine("4 - Sort Least to Most risk for Mold");
                Console.WriteLine("\nQ - Return to Main Menu");

                ConsoleKeyInfo key = Console.ReadKey(true);
                ConsoleKey key2 = key.Key;
                int selectNum = key2 - ConsoleKey.D0;

                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        WeatherStationHelper.DateTemp(infoString);
                        break;

                    case ConsoleKey.D2:                                                 // If Case 2 is selected, it will fall through to Case 3 since there is no Break
                    case ConsoleKey.D3:
                        WeatherStationHelper.SortTempOrHum(infoString, selectNum);
                        break;

                    case ConsoleKey.D4:
                        WeatherStationHelper.Mold(infoString);
                        break;

                    case ConsoleKey.Q:
                        quit = true;
                        break;
                }
            }
        }
    }
}