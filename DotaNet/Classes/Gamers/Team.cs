using System;
using System.Collections.Generic;

namespace DotaNet.Classes.Gamers
{
    public class Team
    {
        string Name { get; set; }
        List<Gamer> Gamers { get; set; }
        public Team(string name)
        {
            Name = name;
        }
    }
}
