using System;
using System.Collections.Generic;
using System.IO;
using DotaNet.Classes.Gamers;
namespace DotaNet.Classes.Database
{
    public class Database
    {
        public static List<Match> matches = new List<Match>();
        public static List<Gamer> gamers = new List<Gamer>();
        public static List<Team> teams = new List<Team>();
        const string matchDataPath = "matches.json";
        public void AddMatch(Match match)
        {
            string json;
            var streamWriter = new StreamWriter(matchDataPath);
            matches.Add(match);

            json = Serialaizer.Serialize(match);

            streamWriter.Write(json);
            streamWriter.Close();
        }
        public Match GetMatch()
        {
            string json;
            var streamReader = new StreamReader(matchDataPath);
            json = streamReader.ReadToEnd();
            var match = Serialaizer.Deserialize(json);
            streamReader.Close();

            return match;
        }
        public Database()
        {
        }
    }
}
