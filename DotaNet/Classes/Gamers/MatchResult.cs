using System;
namespace DotaNet.Classes.Gamers
{
    public struct MatchResult
    {
        public Team Left { get; set; }
        public Team Right { get; set; }
        public int ResultOfLeft { get; set; }
        public int ResultOfRight { get; set; }
        public MatchResult(Team left, Team right, int resultOfLeft, int resultOfRight)
        {
            Left = left;
            Right = right;
            ResultOfLeft = resultOfLeft;
            ResultOfRight = resultOfRight;
        }
    }
}
