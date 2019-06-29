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
            return new Match[1];

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
