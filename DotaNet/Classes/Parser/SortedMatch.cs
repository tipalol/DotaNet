using System;
using System.Collections.Generic;
using System.Text;
using DotaNet.Classes.Gamers;

namespace DotaNet.Classes.Parser
{
    class SortedMatch
    {
        #region Сортировка

        public static Match[] SortedMatches(MatchResult[] matchResults)
        {

            Team[] allTeams = GetAllTeam(matchResults).ToArray();
            List<Gamer> allGamers = GetAllGamers(allTeams);
            List<Gamer> gamerUnique = UniqueGamers(allGamers);
            TeamUniqueGamers(allTeams, gamerUnique);

            List<Match> result = new List<Match>();
            foreach(MatchResult matchResult in matchResults)
            {
                result.Add(new Match(matchResult));
            }

            return result.ToArray();
        }

        private static void UpdateMatches(MatchResult[] matches)
        {
            foreach(MatchResult match in matches)
            {
                if(match.ResultOfLeft>match.ResultOfRight)
                {

                    AddWinLeft(match.Left, match.Right);
                }
                else if (match.ResultOfRight > match.ResultOfLeft)
                {
                    AddWinLeft(match.Right, match.Left);
                }
            }
        }

        private static void AddWinLeft(Team left, Team right)
        {
            left.AddWin();
            right.AddLooses();
            foreach (Gamer gamer in left.Gamers)
            {
                foreach (Gamer gamerTwo in left.Gamers)
                {
                    if (gamerTwo == gamer)
                        continue;
                    gamer.Addbody(gamerTwo, 1, 0);
                }
                foreach (Gamer gamerTwo in right.Gamers)
                {
                    gamer.AddEnemy(gamerTwo, 1, 0);
                }
            }
            foreach (Gamer gamer in right.Gamers)
            {
                foreach (Gamer gamerTwo in right.Gamers)
                {
                    if (gamerTwo == gamer)
                        continue;
                    gamer.Addbody(gamerTwo, 0, 1);
                }
                foreach (Gamer gamerTwo in left.Gamers)
                {
                    gamer.AddEnemy(gamerTwo, 0, 1);
                }
            }
        }

        private static void TeamUniqueGamers(Team[] teams, List<Gamer> gamers)
        {
            foreach (Team team in teams)
            {
                var gamersTeam = team.Gamers;
                List<Gamer> GamersNewTeam = new List<Gamer>();
                foreach (Gamer gamer in gamersTeam)
                {
                    var gamerForAdd = gamers.Find(g => g.Name == gamer.Name);
                    GamersNewTeam.Add(gamerForAdd);
                }
                team.Gamers = GamersNewTeam;
            }
        }

        private static List<Gamer> GetAllGamers(Team[] team)
        {
            List<Gamer> gamers = new List<Gamer>();
            foreach (Team t in team)
            {
                gamers.AddRange(t.Gamers);
            }
            return gamers;
        }

        private static List<Team> GetAllTeam(MatchResult[] matches)
        {
            List<Team> teams = new List<Team>();
            foreach (MatchResult match in matches)
            {
                teams.Add(match.Left);
                teams.Add(match.Right);
            }
            return teams;
        }

        private static List<Gamer> UniqueGamers(List<Gamer> gamers)
        {
            List<Gamer> result = new List<Gamer>();
            foreach (Gamer gamer in gamers)
            {
                Gamer gamerUniq = result.Find(g => g.Name == gamer.Name);
                if (gamerUniq == null)
                    result.Add(gamerUniq);
            }
            return result;
        }

        #endregion
    }
}
