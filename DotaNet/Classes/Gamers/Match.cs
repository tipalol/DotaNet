using System;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс содержит информацию о прошедшем матче
    /// </summary>
    [DataContract]
    public class Match
    {
        [DataMember]
        public string URL;
        /// <summary>
        /// Левая команда-участник
        /// </summary>
        [DataMember]
        public Team Left { get; set; }
        /// <summary>
        /// Правая команда участник
        /// </summary>
        [DataMember]
        public Team Right { get; set; }
        /// <summary>
        /// Результат матча
        /// </summary>
        [DataMember]
        public MatchResult Result { get; set; }
        public Match(Team left, Team right)
        {
            Left = left;
            Right = right;
        }
        public Match(string URL)
        {
            this.URL = URL;
            //Получение результата матча
            MatchResult matchResult = Parser.Parser.GetMatchResult(URL);

            Left = matchResult.Left;
            Right = matchResult.Right;
            if (matchResult.ResultOfLeft > matchResult.ResultOfRight)
            {
                Left.AddWin();
                foreach (Gamer gamer in Left.Gamers)
                {
                    foreach (Gamer enemy in Right.Gamers)
                    {
                        gamer.AddEnemy(enemy, 1, 0);
                        enemy.AddEnemy(gamer, 0, 1);
                    }

                    foreach (Gamer body in Left.Gamers)
                    {
                        if (gamer != body)
                        {
                            gamer.Addbody(body, 1, 0);
                            body.Addbody(gamer, 1, 0);
                        }

                    }
                }

                Right.AddLooses();
            }
            if (matchResult.ResultOfRight > matchResult.ResultOfLeft)
            {
                Right.AddWin();

                foreach (Gamer gamer in Right.Gamers)
                {
                    foreach (Gamer enemy in Left.Gamers)
                    {
                        gamer.AddEnemy(enemy, 1, 0);
                        enemy.AddEnemy(gamer, 0, 1);
                    }

                    foreach (Gamer body in Right.Gamers)
                        if (gamer != body)
                        {
                            gamer.Addbody(body, 1, 0);
                            body.Addbody(gamer, 1, 0);
                        }
                }

                Left.AddLooses();
            }
        }
        public Match() { }
    }
}
