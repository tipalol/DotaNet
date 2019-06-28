using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using DotaNet.Classes.Gamers;
namespace DotaNet.Classes.Database
{
    [DataContract]
    public class Database
    {
        static Database instance;
        public static Database GetInstance()
        {
            if (instance == null)
                instance = new Database();
            return instance;
        }
        [DataMember]
        public List<Match> Matches = new List<Match>();
        [DataMember]
        public List<Gamer> Gamers = new List<Gamer>();
        [DataMember]
        public List<Team> Teams = new List<Team>();
        const string matchDataPath = "matches.json";
        /// <summary>
        /// Добавляет в базу данных инфу о:
        /// прошедшем матче,
        /// участвоваших в нем командах
        /// игроках, входящих в эти команды
        /// </summary>
        /// <param name="match">Прошедший матч</param>
        public void AddMatch(Match match)
        {
            Matches.Add(match);
            if (Teams.Contains(match.Left))
            {

            }
            Teams.Add(match.Left);
            Teams.Add(match.Right);
            foreach (Gamer gamer in match.Right.Gamers)
                Gamers.Add(gamer);
            foreach (Gamer gamer in match.Left.Gamers)
                Gamers.Add(gamer);
        }
        public Match GetMatch()
        {
            string json;
            var streamReader = new StreamReader(matchDataPath);
            json = streamReader.ReadToEnd();
            var match = Serialaizer.GetInstance().Deserialize(json);
            streamReader.Close();

            return match;
        }
        public void AddMatchTest(Match match)
        {
            Matches.Add(match);
        }
        public void SaveData()
        {
            string json = Serialaizer.GetInstance().Serialize(instance);
            var streamWriter = new StreamWriter(matchDataPath);

            streamWriter.Write(json);
            streamWriter.Close();
        }
        public Database()
        {
        }
    }
}
