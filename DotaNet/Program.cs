using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;

namespace DotaNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Gamer gamer = new Gamer("Вадим", 0, 228);
            Team team = new Team("Лузеры");
            team.AddGamer(gamer);

            Gamer gamer2 = new Gamer("Пидор", 100, 2);
            Team team2 = new Team("Пидарасы");
            team2.AddGamer(gamer2);

            Match match = new Match(team, team2);

            Database database = new Database();
            database.AddMatch(match);

            Match match2 = database.GetMatch();
            Console.WriteLine(Serialaizer.Serialize(match2));
        }
    }
}
