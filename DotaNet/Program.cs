using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;
using DotaNet.Classes.Parser;

namespace DotaNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parser.Parse();

            Gamer gamer = new Gamer("Вадим", 0, 100);
            Team team = new Team("Лохи");
            team.AddGamer(gamer);

            Gamer gamer2 = new Gamer("Лох", 100, 0);
            Team team2 = new Team("Кеки");
            team2.AddGamer(gamer2);

            Match match = new Match(team, team2);

            Database.GetInstance().AddMatch(match);

            Database.GetInstance().SaveData();
        }
    }
}
