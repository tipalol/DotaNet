using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;
using DotaNet.Classes.Parser;
using System.Collections.Generic;
using DotaNet.Classes.Magic;

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
            try
            {
                Database.GetInstance().LoadData();
            } catch (Exception)
            {
                Console.WriteLine("Файл базы данных не найден. Создайте новый");
            }
            int input = -1;
            while (input != 0)
            {
                switch (input)
                {
                    //Загрузка базы данных из файла
                    case 1:

                        Database.GetInstance().LoadData();

                        foreach (MatchResult result in Database.GetInstance().Results)
                        {
                            Console.WriteLine("Матч:");
                            Console.WriteLine($"Результат: {result.ResultOfLeft} : {result.ResultOfRight}");
                            Console.WriteLine($"Левая команда: {result.Left.Name}");
                            foreach (Gamer gamer in result.Left.Gamers)
                                Console.WriteLine($"Игрок: {gamer.Name}");
                            Console.WriteLine($"Правая команда: {result.Right.Name}");
                            foreach (Gamer gamer in result.Right.Gamers)
                                Console.WriteLine($"Игрок: {gamer.Name}");
                        }
                        break;
                        //Сохранение базы данных в файл 
                    case 2:
                        Database.GetInstance().SaveData();
                        break;
                    case 3:
                        List<MatchResult> matches = Parser.ParseThread();
                        foreach (MatchResult match in matches)
                            Database.GetInstance().AddMatchResult(match);

                        Database.GetInstance().SaveData();
                        break;
                    case 4:
                        List<MatchResult> testMatches = Parser.ParseTest();
                        foreach (MatchResult match in testMatches)
                            Database.GetInstance().AddMatchResult(match);

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
                    case 8:


                        foreach (Team team in Database.GetInstance().Teams)
                        {
                            Console.WriteLine($"Название команды: {team.Name}");
                            foreach (Gamer gamer in team.Gamers)
                                Console.WriteLine(Serialaizer.GetInstance().Serialize(gamer));
                        }
                        break;
                    case 9:
                        List<Match> matchesList = new List<Match>(SortedMatch.SortedMatches(Database.GetInstance().Results.ToArray()));
                        Database.GetInstance().Matches = matchesList;
                        
                        break;
                    case 10:

                        Console.WriteLine();
                        break;
                }
                Console.WriteLine("1 - Загрузить базу данных из файла, 2 - Сохранить базу данных в файл, 3 - Запустить парсинг, 4 - Запустить тестовый парсинг, 5 - Распечатать сериализованных игроков, 6 - Распечатать сериализованные команды, 7 - Распечатать сериализованные матчи, 8 - Вывести инфу о командах, 9 - отсортировать матчи");
                try
                {
                    input = getInt();
                } catch (Exception)
                {
                    Console.WriteLine("Ты ебан, это не число");
                }
            }

        }
    }
}
