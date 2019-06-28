using System;
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
        string Name { get; set; }
        /// <summary>
        /// Список игроков, играющих в этой команде
        /// </summary>
        [DataMember]
        List<Gamer> Gamers { get; set; } = new List<Gamer>();
        /// <summary>
        /// Добавляет игрока в команду
        /// </summary>
        public void AddGamer(Gamer gamer)
        {
            Gamers.Add(gamer);
        }
        public Team(string name)
        {
            Name = name;
        }
    }
}
