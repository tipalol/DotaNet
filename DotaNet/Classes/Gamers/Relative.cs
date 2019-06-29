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
        public bool isEmpty()
        {
            if (GamerName == null)
                return true;
            return false;
        }
        /// <summary>
        /// Игрок, с которым существует такая статистика
        /// </summary>
        [DataMember]
        public string GamerName{ get; set; }
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
        public Relative(string gamer, int wins, int looses)
        {
            GamerName = gamer;
            Wins = wins;
            Looses = looses;
        }
    }
}
