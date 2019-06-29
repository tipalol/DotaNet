using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;
using DotaNet.Classes.Parser;
using System.Collections.Generic;

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
                        List<Match> matches = Parser.ParseThread();
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
                    case 5:
                        foreach (Gamer gamer in Database.GetInstance().Gamers)
                            Console.WriteLine(Serialaizer.GetInstance().Serialize(gamer));
                        break;
                    case 6:
                        foreach (Team team in Database.GetInstance().Teams)
                            Console.WriteLine(Serialaizer.GetInstance().Serialize(team));
                        break;
                    case 7:
                        foreach (Match match in Database.GetInstance().Matches)
                            Console.WriteLine(Serialaizer.GetInstance().Serialize(match));
                        break;
                }
                Console.WriteLine("1 - Load, 2 - Save, 3 - Parse, 4 - Parse test, 5 - Print gamers info, 6 - Print teams info, 7 - Print matches info");
                input = getInt();
            }

        }
    }
}
