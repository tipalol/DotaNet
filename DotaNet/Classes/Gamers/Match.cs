using System;
namespace DotaNet.Classes.Gamers
{
    public class Match
    {
        Team Left { get; set; }
        Team Right { get; set; }
        public Match(Team left, Team right)
        {
            Left = left;
            Right = right;
        }
    }
}
