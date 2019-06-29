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
        [DataMember]
        public int scoreLeft;
        [DataMember]
        public int scoreRight;
        public Match(string URL)
        {
            this.URL = URL;
            //Получение результата матча
            MatchResult matchResult = Parser.Parser.GetMatchResult(URL);

            Left = matchResult.Left;
            Right = matchResult.Right;
            scoreLeft = matchResult.ResultOfLeft;
            scoreRight = matchResult.ResultOfRight;
        }
        public Match() { }
    }
}
