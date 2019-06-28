using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;
using DotaNet.Classes.Parser;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotaNet
{
    class Program
    {
        static int getInt()
        {
            string input = Console.ReadLine();
            return Convert.ToInt32(input);
        }
        static void Main(string[] args)
        {
            int input = -1;
            while (input != 0)
            {
                switch (input)
                {
                    case 1:
                        Database.GetInstance().LoadData();
                        foreach (Gamer gamer in Database.GetInstance().Gamers)
                        {
                            Console.WriteLine($"Имя: {gamer.Name}");
                            Console.WriteLine($"Винрейт: {gamer.Winrate}");
                        }
                        break;
                    case 2:
                        Database.GetInstance().SaveData();
                        break;
                    case 3:
                        List<Match> matches = Parser.Parse();
                        foreach (Match match in matches)
                            Database.GetInstance().AddMatch(match);

                        Database.GetInstance().SaveData();
                        break;
                    case 4:
                        List<Match> testMatches = Parser.ParseTest();
                        foreach (Match match in testMatches)
                            Database.GetInstance().AddMatch(match);

                        Database.GetInstance().SaveData();
                        break;
                }
                Console.WriteLine("1 - Load, 2 - Save, 3 - Parse, 4 - Parse test");
                input = getInt();
            }

        }
    }
}
