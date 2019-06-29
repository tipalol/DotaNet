using System;
using DotaNet.Classes.Gamers;
using DotaNet.Classes.Database;
using DotaNet.Classes.Parser;
using System.Collections.Generic;

namespace DotaNet
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Match> matches = Parser.ParseThread();

            foreach (Match match in matches)
                Database.GetInstance().AddMatch(match);

            Database.GetInstance().SaveData();

        }
    }
}
