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
        public static string SortTempOrHumOrMold(string inOrOut, int num, bool textCall)
        {
            int sortNum = num == 2 ? 5 : 6;                                                                             // If User Input is 2 (avg temp), fetch data from correct Match Group 5

            bool mold = false;
            if (num == 4)
            {
                mold = true;
            }

            string text = sortNum == 5 ? "Temperature" : num == 4 ? "Risk of Mold" : "Humidity";

            if (!textCall)
            {
                Console.Clear();
                Console.WriteLine($"List of each day average {text} {inOrOut}, sorted from highest to lowest.\n\n");
            }

            string pattern = $@"^2016-(0[6-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([\d]+:[\d]+:[\d]+),({inOrOut.TernaryConversion()}),(-?[\d.]+),([\d]+)$";        // Temp: Group 5. Hum: Group 6
            Regex regex = new Regex(pattern);

            List<double> avg = new();
            var avgPerDay = new Dictionary<string, double>();                                                           // Dictionary to store Date and Average Value
            var moldData = new List<(double temperature, double humidity)>();
            int fallCount = 0;
            int winterCount = 0;
            bool fall = false;
            bool winter = false;
            string fallDate = "";
            string winterDate = "";
            double avgDay = 0;
            var winterDay = new Dictionary<int, string>();

            var oldDay = "01";

            foreach (var line in Program.listAllValues)
            {
                Match match = regex.Match(line);

                var day = match.Groups[2].Value;
                var month = match.Groups[1].Value;
                string date = $"2016-{month}-{day}";                                                                    // The Dictionary Key in order to set the average data for this current date

                if (match.Success)
                {
                    if (day != oldDay)
                    {
                        if (mold)
                        {
                            double moldTempFactor = Math.Max(0, (moldData.Average(entry => entry.temperature)) / 15);
                            double moldHumFactor = Math.Max(0, (moldData.Average(entry => entry.humidity)) - 78);

                            double moldRisk = (moldHumFactor * moldTempFactor) / 0.22;

                            avgPerDay[date] = moldRisk;
                        }
                        else
                        {
                            avgDay = avg.Average();

                            avgPerDay[date] = avgDay;                                                               // Adds average value for current date


                        }

                        if (avgDay < 10 && !fall)           // Fall
                        {
                            fallCount++;
                            if (fallCount == 5)
                            {
                                fall = true;
                                fallDate = date;
                            }
                        }
                        else
                        {
                            fallCount = 0;
                        }

                        if (avgDay < 0 && !winter)          // Winter
                        {
                            winterCount++;
                            if (winterCount == 5)
                            {
                                winter = true;
                                winterDate = date;
                            }
                        }
                        else
                        {
                            string oldDate = $"2016-{month}-{int.Parse(day)-1}";    // If there are not 5 "winter" days in a row
                            winterDay[winterCount] = oldDate;
                            winterCount = 0;
                        }

                        avg.Clear();

                        moldData.Clear();

                        oldDay = match.Groups[2].Value;
                    }
                    else if ((double.TryParse(match.Groups[sortNum].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double data)))
                    {
                        if (mold && (double.TryParse(match.Groups[5].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double data2)))
                        {
                            moldData.Add((data2, data));
                        }
                        else
                        {
                            avg.Add(data);
                        }
                    }
                }
            }

            var sortedDays = avgPerDay.OrderByDescending(x => x.Value).ToList();

            int index = 0;

            if (textCall)
            {
                string winterString = winter == true ? $"\nMeteorological Winter: {winterDate}" : $"\nAlmost Winter:\t\t{winterDay[winterDay.Keys.Max()]} - {winterDay.Keys.Max()} days";

                return "\n\nMeteorological Fall:" + "\t" + fallDate + "\n" + winterString;
            }
            foreach (var day in sortedDays)
            {
                Console.WriteLine($"{index + 1}\t{day.Key}\t{day.Value:F1}");
                index++;
            }

            Console.WriteLine($"\n\nHighest {text}:\t{sortedDays.First().Key}, {sortedDays.First().Value:F1}");         // date is the Key, average data is the Value
            Console.WriteLine($"\nLowest {text}:\t{sortedDays.Last().Key}, {sortedDays.Last().Value:F1}");
            if (sortNum == 5 && inOrOut == "Outside")
            {
                Console.WriteLine($"\n\nMeteorological Fall: {fallDate}");

                string winterString = winter == true ? $"\nMeteorological Winter: {winterDate}" : $"\nAlmost Winter: {winterDay[winterDay.Keys.Max()]} - {winterDay.Keys.Max()} days";

                Console.WriteLine(winterString);
            }
            Console.WriteLine("\n\nEnter to go back.");
            Console.ReadKey();

            return "";
        }

        public static void DateTemp(string inOrOut)
        {
            Console.Clear();

            Console.WriteLine("Enter Year (2016)");
            string? year = Console.ReadLine();
            Console.WriteLine("\nEnter Month (01-12)");
            string? month = Console.ReadLine();
            Console.WriteLine("\nEnter Day (01-31)");
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