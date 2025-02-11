using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInfo
{
    internal class OutdoorInfo
    {
        public static void Run()
        {
            Console.WriteLine("Outdoor Info.\n\n");
            Console.WriteLine("1 - Search for Date and see Average Temp & Humidity");
            Console.WriteLine("2 - Sort Hottest to Coldest day (Average Temp per day)");
            Console.WriteLine("3 - Sort Dryest to Dampest day (Average Humidity per day");
            Console.WriteLine("4 - Sort Least to Most risk for Mold");

            ConsoleKeyInfo key = Console.ReadKey();

            Console.Clear();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    WeatherStationHelper.DateTemp("Ute");
                    break;

                case ConsoleKey.D2:
                    WeatherStationHelper.SortHotToCold("Ute");
                    break;

                case ConsoleKey.D3:
                    WeatherStationHelper.SortDryToDamp("Ute");
                    break;
            }
        }
    }
}