using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace WeatherInfo
{
    internal class IndoorInfo
    {
        public static void Run()
        {
            Console.WriteLine("Indoor Info.\n\n");
            Console.WriteLine("1 - Search for Date and see Average Temp & Humidity");
            Console.WriteLine("2 - Sort Hottest to Coldest day (Average Temp per day)");
            Console.WriteLine("3 - Sort Dryest to Dampest day (Average Humidity per day");
            Console.WriteLine("4 - Sort Least to Most risk for Mold");

            ConsoleKeyInfo key = Console.ReadKey();

            Console.Clear();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    WeatherStationHelper.DateTemp("Inne");
                    break;

                case ConsoleKey.D2:
                    WeatherStationHelper.SortHotToCold("Inne");
                    break;

                case ConsoleKey.D3:
                    WeatherStationHelper.SortDryToDamp("Inne");
                    break;
            }
        }
    }
}