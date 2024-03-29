﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс реализует команду
    /// </summary>
    [DataContract]
    public class Team
    {
        /// <summary>
        /// Название команды
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Список игроков, играющих в этой команде
        /// </summary>
        [DataMember]
        public List<Gamer> Gamers { get; set; } = new List<Gamer>();
        /// <summary>
        /// Добавляет игрока в команду
        /// </summary>
        public void AddGamer(Gamer gamer)
        {
            Gamers.Add(gamer);
        }
        /// <summary>
        /// Добавляет победу всем участникам команды
        /// </summary>
        public void AddWin(int wins=1)
        {
            foreach (Gamer gamer in Gamers)
            {
                gamer.AddWin(wins);
            }


        }
        /// <summary>
        /// Добавляет поражение всем участникам команды
        /// </summary>
        public void AddLooses(int wins = 1)
        {
            foreach (Gamer gamer in Gamers)
            {
                gamer.AddLoose(wins);
            }
        }
        public Team(string name, Gamer[] gamers = null)
        {
            Name = name;
            if (gamers != null)
            {
                foreach (Gamer gamer in gamers)
                {
                    if (gamer != null)
                        Gamers.Add(gamer);
                }
            }
        }
    }
}
