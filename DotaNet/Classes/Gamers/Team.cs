using System;
using System.Collections.Generic;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс реализует команду
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Название команды
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Список игроков, играющих в этой команде
        /// </summary>
        List<Gamer> Gamers { get; set; }
        public Team(string name)
        {
            Name = name;
        }
    }
}
