using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс реализует игрока
    /// </summary>
    [DataContract]
    public class Gamer
    {
        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string Name { get; set; }
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
        /// Винрейт
        /// </summary>
        double Winrate
        {
            get
            {
                return (double)Wins / (double)Looses;
            }
        }
        /// <summary>
        /// Информация о совместных играх с игроком
        /// </summary>
        [DataMember]
        List<Relative> Bodies { get; set; }
        /// <summary>
        /// Информация об играх против игрока
        /// </summary>
        [DataMember]
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
        /// <summary>
        /// Добавляет победу(ы) игроку
        /// </summary>
        /// <param name="wins">Победы</param>
        public void AddWin(int wins = 1)
        {
            Wins += wins;
        }
        /// <summary>
        /// Добавляет поражение(я) игроку
        /// </summary>
        /// <param name="looses">Поражения</param>
        public void AddLoose(int looses = 1)
        {
            Looses += looses;
        }
        public Gamer(string name, int wins = 0, int looses = 0)
        {
            Name = name;
            Wins = wins;
            Looses = looses;
        }
    }
}