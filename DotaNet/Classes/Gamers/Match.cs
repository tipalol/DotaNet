using System;
namespace DotaNet.Classes.Gamers
{
    /// <summary>
    /// Класс содержит информацию о прошедшем матче
    /// </summary>
    public class Match
    {
        public string URL;
        /// <summary>
        /// Левая команда-участник
        /// </summary>
        Team Left { get; set; }
        /// <summary>
        /// Правая команда участник
        /// </summary>
        Team Right { get; set; }
        public Match(Team left, Team right)
        {
            Left = left;
            Right = right;
        }
        public Match(string URL)
        {
            this.URL = URL;
            Parser.Parser parser = new Parser.Parser();
            try
            {
                parser.GetTeams(URL);
            }
            catch
            {
                URL = null;
            }
        }
    }
}
