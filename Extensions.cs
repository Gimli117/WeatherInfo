using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherInfo
{
    internal static class Extensions
    {
        public static string TernaryConversion(this string inOrOut)
        {
            string text = inOrOut == "Outside" ? "Ute" : "Inne";

            return text;
        }

        public static string DateConversion(this string date)
        {
            if (int.TryParse(date, out int num))
            {
                return num.ToString("00");
            }
            return date;
        }
    }
}