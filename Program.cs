﻿using System.Net.Quic;

namespace WeatherInfo
{
    internal class Program
    {
        public static string[] listAllValues = File.ReadAllLines("../../../../WeatherInfo/Tempdata.txt");
        public delegate void MyDelegate(); 
        static void Main()
        {
            bool quit = false;
            MyDelegate myDelegate = new MyDelegate(Info.Run);
            

            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            while (!quit)
            {
                Console.Clear();

                Console.WriteLine("Welcome to the Weather App!\n");
                Console.WriteLine("Please select a command:\n\n");
                Console.WriteLine("1 - Info");
                Console.WriteLine("2 - Create Text File");
                Console.WriteLine("3 - Prints ALL data");
                Console.WriteLine("\nESC - Exits the Program");

                ConsoleKeyInfo key = Console.ReadKey(true);

                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        myDelegate();
                        break;

                    case ConsoleKey.D2:
                        CreateTextFile.Run();
                        break;

                    case ConsoleKey.D3:
                        int index = 1;
                        foreach (var line in listAllValues)
                        {
                            Console.WriteLine($"{index}: {line}");
                            index++;
                        }
                        Console.WriteLine("\n\nEnter to go back");
                        Console.ReadLine();
                        break;

                    case ConsoleKey.Escape:
                        quit = true;
                        break;
                }
            }
        }
    }
}