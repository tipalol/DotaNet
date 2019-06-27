using System;
namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Структура содержит данные об совместном винрейте с кем либо
    /// (или против кого либо)
    /// </summary>
    public struct Relative
    {
        public Gamer Gamer { get; set; }
        public int Wins { get; set; }
        public int Looses { get; set; }
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
