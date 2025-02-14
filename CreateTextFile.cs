using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherInfo
{
    internal class CreateTextFile
    {
        public static void Run()
        {
            // Average Temperature inside and outside, per month


            // Average Humidity inside and outside, per month

            // Average Risk of Mold inside and outside, per month

            // Meteorological Fall and Winter

            // Print the Mold Algorithm
        }

        public static void AvgTemp()
        {
            string pattern = $@"^2016-(0[6-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([\d]+:[\d]+:[\d]+),(\w),(-?[\d.]+),([\d]+)$"; 
            Regex regex = new Regex(pattern);         
            var tempHumList = new List<(double temperature, double humidity)>();
            var avgPerMonth = new Dictionary<string, List<(double temperature, double humidity, double moldRisk)>>();


            var oldMonth = "06";

            foreach (var line in Program.listAllValues)
            {
                Match match = regex.Match(line);

                var month = match.Groups[1].Value;
                string date = $"2016-{month}";

                if(match.Success)
                {

                    if(oldMonth != month)
                    {
                        double avgTemp = tempHumList.Average(entry => entry.temperature);
                        double avgHumidity = tempHumList.Average(entry => entry.humidity);

                        double moldTempFactor = Math.Max(0, (tempHumList.Average(entry => entry.temperature)) / 15);
                        double moldHumFactor = Math.Max(0, (tempHumList.Average(entry => entry.humidity)) - 78);

                        double moldRisk = (moldHumFactor * moldTempFactor) / 0.22;

 
                        avgPerMonth[date].Add((avgTemp, avgHumidity, moldRisk));

                        tempHumList.Clear();

                        oldMonth = match.Groups[1].Value;
                    }
                    else if ((double.TryParse(match.Groups[6].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double hum)) && (double.TryParse(match.Groups[5].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp)))
                    {

                        tempHumList.Add((temp, hum));
                    }
                }               

            }


            int index = 0;
            foreach (var month in avgPerMonth)
            {
                Console.WriteLine($"{index + 1}:\t{month.Key:F1} \t {month.Value}");
                index++;
            }
            Console.ReadKey(true);
        }
    }
}