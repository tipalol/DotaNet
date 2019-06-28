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
        public Match(Team left, Team right)
        {
            Left = left;
            Right = right;
        }
        public Match(string URL)
        {
            this.URL = URL;
            MatchResult matchResult = Parser.Parser.GetMatchResult(URL);

            Left = matchResult.Left;
            Right = matchResult.Right;
            if (matchResult.ResultOfLeft > matchResult.ResultOfRight)
                Left.AddWin();
            if (matchResult.ResultOfRight > matchResult.ResultOfLeft)
                Right.AddWin();
                
        }
        public Match() { }
    }
}
