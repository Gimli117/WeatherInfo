using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherInfo
{
    internal class WeatherStationHelper
    {
        public static void SortTempOrHum(string inOrOut, int num)
        {
            int sortNum = num == 2 ? 5 : 6;

            Console.Clear();

            string text = sortNum == 5 ? "Temperature" : "Humidity";

            Console.WriteLine($"Now showing each day average {text} {inOrOut}, sorted from highest to lowest.\n\n");

            string pattern = $@"^2016-(0[6-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([\d]+:[\d]+:[\d]+),({inOrOut.TernaryConversion()}),(-?[\d.]+),([\d]+)$";
            Regex regex = new Regex(pattern);

            List<double> avg = new();
            var avgPerDay = new List<double>();

            var oldDay = "01";

            foreach (var line in Program.listAllValues)
            {
                Match match = regex.Match(line);

                var day = match.Groups[2].Value;

                if (match.Success)
                {
                    if (day != oldDay)
                    {
                        double avgTempDay = avg.Average();

                        avgPerDay.Add(avgTempDay);

                        avg.Clear();

                        oldDay = match.Groups[2].Value;
                    }
                    else if ((double.TryParse(match.Groups[sortNum].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double data)))
                    {
                        avg.Add(data);
                    }
                }
            }

            avgPerDay.Sort();
            avgPerDay.Reverse();

            int index = 0;

            foreach (var hum in avgPerDay)
            {
                Console.WriteLine($"{index + 1}: {hum:F1}");
                index++;
            }

            Console.WriteLine($"\n\nHighest {text}: {avgPerDay[0]:F1}");
            Console.WriteLine($"\nLowest {text}: {avgPerDay[index - 1]:F1}");
            Console.WriteLine("\n\nEnter to go back.");
            Console.ReadLine();
        }

        public static void Mold(string inOrOut)
        {
            // ((luftfuktighet -78) * (Temp/15))/0,22
        }

        public static void DateFall()
        {

        }

        public static void DateWinter()
        {

        }

        public static void DateTemp(string inOrOut)
        {
            Console.Clear();

            Console.WriteLine("Enter Year (2016)");
            string? year = Console.ReadLine();
            Console.WriteLine("Enter Month (01-12)");
            string? month = Console.ReadLine();
            Console.WriteLine("Enter Day (01-31)");
            string? day = Console.ReadLine();

            if (day == "35")
            {
                Console.Clear();
                Console.WriteLine("Nice Try!");
                Thread.Sleep(1000);
                return;
            }

            month = month?.DateConversion();
            day = day?.DateConversion();

            string datePattern = $@"^({year}-{month}-{day} [\d]+:[\d]+:[\d]+),({inOrOut.TernaryConversion()}),([\d.]+),([\d]+)$";
            Regex regex = new Regex(datePattern);

            var filteredData = new List<(double temperature, double humidity)>();

            bool matchFound = false;
            foreach (var line in Program.listAllValues)
            {
                Match match = regex.Match(line);

                if (match.Success)
                {
                    if ((double.TryParse(match.Groups[3].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
                        && (double.TryParse(match.Groups[4].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double hum)))
                    {
                        filteredData.Add((temp, hum));
                        matchFound = true;
                    }
                }
            }
            if (!matchFound)
            {
                Console.Clear();
                Console.WriteLine("Ingen matchning.");
                Thread.Sleep(2000);
                return;
            }

            double avgTemp = filteredData.Average(entry => entry.temperature);
            double avgHumidity = filteredData.Average(entry => entry.humidity);

            Console.WriteLine($"\n\nNow showing info for: {year} - {month} - {day}\n\n");
            Console.WriteLine($"Average Temperature: {avgTemp:F1}\nAverage Humidity: {avgHumidity:F1}");
            Console.WriteLine("\n\nEnter to go back.");
            Console.ReadLine();
        }
    }
}