using System;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Структура содержит данные об совместном винрейте с кем либо
    /// (или против кого либо)
    /// </summary>
    [DataContract]
    public struct Relative
    {
        /// <summary>
        /// Игрок, с которым существует такая статистика
        /// </summary>
        [DataMember]
        public Gamer Gamer { get; set; }
        /// <summary>
        /// Победы
        /// </summary>
        [DataMember]
        public int Wins { get; set; }
        /// <summary>
        /// Поражения
        /// </summary>
        [DataMember]
        public int Looses { get; set; }
        /// <summary>
        /// Получает винрейт
        /// </summary>
        public double Winrate
        {
            get
            {
                return Wins / Looses;
            }
        }
        public Relative(Gamer gamer, int wins, int looses)
        {
            Gamer = gamer;
            Wins = wins;
            Looses = looses;
        }
    }
}
