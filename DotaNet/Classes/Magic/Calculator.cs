using System;
using DotaNet.Classes.Gamers;
using System.Linq;
using System.Collections.Generic;

namespace DotaNet.Classes.Magic
{
    public class Calculator
    {
        public Match[] Matches { get; private set; }
        public Team[] Teams { get { return GetTeams(Matches); } }
        public Gamer[] Gamers { get { return GetGamers(Teams); } }

        public Calculator(Match[] matches)
        {
            Matches = matches;
        }

        public Team[] GetTeams(Match[] matches)
        {
            List<Team> teams = new List<Team>();
            foreach(Match match in matches)
            {
                teams.Add(match.Left);
                teams.Add(match.Right);
            }

            return teams.ToArray();
        }
        public Gamer[] GetGamers(Team[] teams)
        {
            var gamers = from team in teams
                         from gamer in team.Gamers
                         select gamer;
            gamers = gamers.Distinct().ToArray();
            return gamers.ToArray();
        }

        public Gamer FindGamer(string Name)
        {
            Gamer gamer = Gamers.ToList().Find(g => g.Name == Name);
            return gamer;
        }

        public (double left, double right) ResultFight(string[] Left, string[] Right)
        {
            Gamer[] gamersLeft = (from gamer in Left
                                  select FindGamer(gamer)).ToArray();
            Gamer[] gamersRight = (from gamer in Right
                                   select FindGamer(gamer)).ToArray();
            double scoresLeft = 0;
            double scoresRight = 0;

            foreach(Gamer gamer in gamersLeft)
            {
                scoresLeft += gamer.Winrate;
                foreach(Gamer body in gamersLeft)
                    if (body != gamer)
                        scoresLeft += gamer.BodyWinrate(body.Name);
                foreach (Gamer enemy in gamersRight)
                    scoresLeft += gamer.EnemyWinrate(enemy.Name);
            }
            foreach (Gamer gamer in gamersRight)
            {
                scoresRight += gamer.Winrate;
                foreach (Gamer body in gamersRight)
                    if (body != gamer)
                        scoresRight += gamer.BodyWinrate(body.Name);
                foreach (Gamer enemy in gamersLeft)
                    scoresRight += gamer.EnemyWinrate(enemy.Name);
            }

            return (scoresLeft, scoresRight);
        }
    }
}
