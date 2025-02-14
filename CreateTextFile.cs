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
            if (File.Exists("..\\..\\..\\WeatherReport.txt"))
            {
                Console.WriteLine("File already exists.");
                Thread.Sleep(2000);
                return;
            }

            int firstLoop = 0;
            string inOrOut = "";

            while (firstLoop < 2)
            {
                if (firstLoop == 0)
                {
                    inOrOut = "Inne";
                }
                else
                {
                    inOrOut = "Ute";
                }

                string pattern = $@"^(2016|2017)-(0[1-4]|0[6-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([\d]+:[\d]+:[\d]+),({inOrOut}),(-?[\d.]+),([\d]+)$";
                Regex regex = new Regex(pattern);
                var tempHumList = new List<(double temperature, double humidity)>();
                var avgPerMonth = new Dictionary<string, List<(double temperature, double humidity, double moldRisk)>>();

                var oldMonth = "06";

                foreach (var line in Program.listAllValues)
                {
                    Match match = regex.Match(line);

                    var month = match.Groups[2].Value;
                    string date = $"2016-{month}";
                    var oldDate = "";

                    if (match.Success)
                    {

                        if (oldMonth != month)
                        {
                            double avgTemp = tempHumList.Average(entry => entry.temperature);
                            double avgHumidity = tempHumList.Average(entry => entry.humidity);

                            double moldTempFactor = Math.Max(0, (tempHumList.Average(entry => entry.temperature)) / 15);
                            double moldHumFactor = Math.Max(0, (tempHumList.Average(entry => entry.humidity)) - 78);

                            double moldRisk = (moldHumFactor * moldTempFactor) / 0.22;

                            if (match.Groups[2].Value == "01")
                            {
                                oldDate = $"2016-12";
                            }
                            else
                            {
                                oldDate = $"2016-{int.Parse(month) - 1}";
                            }



                            avgPerMonth[oldDate] = new List<(double, double, double)>();

                            avgPerMonth[oldDate].Add((avgTemp, avgHumidity, moldRisk));

                            tempHumList.Clear();

                            oldMonth = match.Groups[2].Value;
                        }
                        else if ((double.TryParse(match.Groups[7].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double hum)) && (double.TryParse(match.Groups[6].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double temp)))
                        {
                            tempHumList.Add((temp, hum));
                        }
                    }
                }
                string inOrOutTxt = $"{inOrOut} Info:\n\n";

                File.AppendAllText("..\\..\\..\\WeatherReport.txt", inOrOutTxt);

                foreach (var month in avgPerMonth.Keys)
                {
                    foreach (var entry in avgPerMonth[month])
                    {
                        string monthData = $"Temperature: {entry.temperature:F1}\tHumidity: {entry.humidity:F1}\tRisk of Mold: {entry.moldRisk:F1}\n";

                        File.AppendAllText("..\\..\\..\\WeatherReport.txt", monthData);
                    }
                }
                firstLoop++;
            }
            var meteoroDates = WeatherStationHelper.SortTempOrHumOrMold("Outside", 2, true);

            File.AppendAllText("..\\..\\..\\WeatherReport.txt", meteoroDates);

            string moldAlgo = "\n\nMold Algorithm:\t((Humidity - 78) * (Temperature / 15)) / 0.22";

            File.AppendAllText("..\\..\\..\\WeatherReport.txt", moldAlgo);

            Console.WriteLine("Weather Report Successfully Created.");
            Thread.Sleep(2000);
        }
    }
}