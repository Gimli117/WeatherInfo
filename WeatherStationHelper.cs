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
        public static void SortHotToCold(string inOrOut)
        {
            Console.Clear();

            string text = inOrOut == "Ute" ? "Outside" : "Inside";                                                                      // Ternary Showoff

            Console.WriteLine($"Now showing each day average temperature {text}, sorted from coldest to hottest.\n\n");

            string pattern = $@"^2016-(0[6-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([\d]+:[\d]+:[\d]+),({inOrOut}),(-?[\d.]+),([\d]+)$";
            Regex regex = new Regex(pattern);

            List<double> avgTemp = new();
            var avgTempPerDay = new List<double>();

            var oldTempDay = "01";

            foreach (var line in Program.listAllValues)
            {
                Match match = regex.Match(line);

                var tempDay = match.Groups[2].Value;

                if (match.Success)
                {
                    if (tempDay != oldTempDay)
                    {
                        double avgTempDay = avgTemp.Average();

                        avgTempPerDay.Add(avgTempDay);

                        avgTemp.Clear();

                        oldTempDay = match.Groups[2].Value;
                    }
                    else if ((double.TryParse(match.Groups[5].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp)))
                    {
                        avgTemp.Add(temp);
                    }
                }
            }

            avgTempPerDay.Sort();
            avgTempPerDay.Reverse();

            int index = 0;

            foreach (var temp in avgTempPerDay)
            {
                Console.WriteLine($"{index+1}: {temp:F1}");
                index++;
            }

            Console.WriteLine($"\n\nHottest Day: {avgTempPerDay[0]:F1}");
            Console.WriteLine($"\nColdest Day: {avgTempPerDay[index-1]:F1}");
            Console.WriteLine("\n\nEnter to go back.");
            Console.ReadLine();
        }

        public static void SortDryToDamp(string inOrOut)
        {

        }

        public static void Mold(string inOrOut)
        {

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

            string datePattern = $@"^({year}-{month}-{day} [\d]+:[\d]+:[\d]+),({inOrOut}),([\d.]+),([\d]+)$";
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