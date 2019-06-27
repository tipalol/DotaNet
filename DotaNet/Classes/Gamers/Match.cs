using System;
namespace DotaNet.Classes.Gamers
{
    public class Match
    {
        Team left;
        Team right;
        public Match(Team left, Team right)
        {
            this.left = left;
            this.right = right;
        }
    }
}
