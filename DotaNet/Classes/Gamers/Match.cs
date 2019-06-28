﻿using System;
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
        Team Left { get; set; }
        /// <summary>
        /// Правая команда участник
        /// </summary>
        [DataMember]
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
        public Match() { }
    }
}