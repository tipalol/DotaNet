﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс реализует игрока
    /// </summary>
    [DataContract(IsReference = false)]
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
                double res = Wins / (double)(Wins + Looses);
                return res;
            }
        }
        /// <summary>
        /// Информация о совместных играх с игроком
        /// </summary>
        [DataMember]
        List<Relative> Bodies { get; set; } = new List<Relative>();
        /// <summary>
        /// Информация об играх против игрока
        /// </summary>
        [DataMember]
        List<Relative> Enemies { get; set; } = new List<Relative>();
        /// <summary>
        /// Добавляет информацию о совместных играх с игроком
        /// </summary>
        /// <param name="body">Союзник</param>
        /// <param name="wins">Победы</param>
        /// <param name="looses">Поражения</param>
        public void Addbody(Gamer body, int wins, int looses) {
            var findBody = Bodies.Find(e => e.GamerName == body.Name);
            if (findBody == null)
                Bodies.Add(new Relative(body.Name, wins, looses));
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
            var findEnemy = Enemies.Find(e => e.GamerName == enemy.Name);
            if (findEnemy == null)
                Enemies.Add(new Relative(enemy.Name, wins, looses));
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
        public double BodyWinrate(string Name)
        {
            foreach(var body in Bodies)
            {
                if (body.GamerName == Name)
                    return body.Winrate;
            }
            return 0;
        }
        public double EnemyWinrate(string Name)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.GamerName == Name)
                    return enemy.Winrate;
            }
            return 0;
        }
        public Gamer(string name, int wins = 0, int looses = 0)
        {
            Name = name;
            Wins = wins;
            Looses = looses;
        }
    }
}