using System;
using System.Collections.Generic;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс реализует игрока
    /// </summary>
    public class Gamer
    {
        /// <summary>
        /// Победы
        /// </summary>
        int Wins { get; set; }
        /// <summary>
        /// Поражения
        /// </summary>
        int Looses { get; set; }
        /// <summary>
        /// Винрейт
        /// </summary>
        double Winrate
        {
            get
            {
                return Wins / Looses;
            }
        }
        /// <summary>
        /// Информация о совместных играх с игроком
        /// </summary>
        List<Relative> Bodies { get; set; }
        /// <summary>
        /// Информация об играх против игрока
        /// </summary>
        List<Relative> Enemies { get; set; }
        /// <summary>
        /// Добавляет информацию о совместных играх с игроком
        /// </summary>
        /// <param name="body">Союзник</param>
        /// <param name="wins">Победы</param>
        /// <param name="looses">Поражения</param>
        public void Addbody(Gamer body, int wins, int looses) {
            Bodies.Add(new Relative(body, wins, looses));
        }
        /// <summary>
        /// Добавляет информацию об играх против игрока
        /// </summary>
        /// <param name="enemy">Противник</param>
        /// <param name="wins">Победы</param>
        /// <param name="looses">Поражения</param>
        public void AddEnemy(Gamer enemy, int wins, int looses)
        {
            Enemies.Add(new Relative(enemy, wins, looses));
        }
        public Gamer(int wins = 0, int looses = 0)
        {
            Wins = wins;
            Looses = looses;
        }
    }
}