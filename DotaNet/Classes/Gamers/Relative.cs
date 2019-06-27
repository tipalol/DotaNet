using System;
namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Структура содержит данные об совместном винрейте с кем либо
    /// (или против кого либо)
    /// </summary>
    public struct Relative
    {
        /// <summary>
        /// Игрок, с которым существует такая статистика
        /// </summary>
        public Gamer Gamer { get; set; }
        /// <summary>
        /// Победы
        /// </summary>
        public int Wins { get; set; }
        /// <summary>
        /// Поражения
        /// </summary>
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
