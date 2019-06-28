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
        public double Winrate
        {
            get
            {
                if (Looses == 0)
                    return 1;
                if ((double)Wins / Looses > 1)
                    return 1;
                return (double)Wins / Looses;
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
            var findBody = Bodies.Find(e => e.Gamer.Name == body.Name);
            if (findBody.isEmpty())
                Bodies.Add(new Relative(body, wins, looses));
            else
            {
                findBody.Wins += wins;
                findBody.Looses += looses;
            }
                
        }
        /// <summary>
        /// Добавляет информацию об играх против игрока
        /// </summary>
        /// <param name="enemy">Противник</param>
        /// <param name="wins">Победы</param>
        /// <param name="looses">Поражения</param>
        public void AddEnemy(Gamer enemy, int wins, int looses)
        {
            var findEnemy = Enemies.Find(e => e.Gamer.Name == enemy.Name);
            if (findEnemy.isEmpty())
                Enemies.Add(new Relative(enemy, wins, looses));
            else
            {
                findEnemy.Wins += wins;
                findEnemy.Looses += looses;
            }
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