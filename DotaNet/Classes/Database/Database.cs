﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using DotaNet.Classes.Gamers;

namespace DotaNet.Classes.Database
{
    [DataContract]
    public class Database
    {
        private static Database instance;
        public static Database GetInstance()
        {
            if (instance == null)
                instance = new Database();
            return instance;
        }
        [DataMember]
        public List<Match> Matches = new List<Match>();
        public List<Gamer> Gamers = new List<Gamer>();
        public List<Team> Teams = new List<Team>();
        [DataMember]
        public List<MatchResult> Results = new List<MatchResult>();
        const string matchDataPath = "matches.json";
        public void AddMatchResult(MatchResult result)
        {
            Results.Add(result);
        }
        /// <summary>
        /// Добавляет в базу данных инфу о:
        /// прошедшем матче,
        /// участвоваших в нем командах,
        /// игроках, входящих в эти команды
        /// </summary>
        /// <param name="match">Прошедший матч</param>
        public void AddMatch(Match match)
        {
            Matches.Add(match);

            Team foundTeam = Teams.Find(u => u.Name == match.Left.Name);
            if (foundTeam == null)
                Teams.Add(match.Left);

            foundTeam = Teams.Find(u => u.Name == match.Right.Name);
            if (foundTeam == null)
                Teams.Add(match.Right);

            foreach (Gamer gamer in match.Right.Gamers)
            {
                Gamer foundGamer = Gamers.Find(u => u.Name == gamer.Name);
                if (foundGamer == null)
                    Gamers.Add(gamer);
                else
                {
                    foundGamer.AddWin(gamer.Wins);
                    foundGamer.AddLoose(gamer.Looses);
                }
                    

            }

            foreach (Gamer gamer in match.Left.Gamers) {
                Gamer foundAnotherGamer = Gamers.Find(u => u.Name == gamer.Name);
                if (foundAnotherGamer == null)
                    Gamers.Add(gamer);
                else
                {
                    foundAnotherGamer.AddWin(gamer.Wins);
                    foundAnotherGamer.AddLoose(gamer.Looses);
                }
            }
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
        public void LoadData()
        {
            var streamReader = new StreamReader(matchDataPath);
            string json = streamReader.ReadToEnd();
            var data = Serialaizer.GetInstance().DeserializeData(json);
            streamReader.Close();
            if (data != null)
                instance = data;
        }
        public Database()
        {
        }
    }
}
