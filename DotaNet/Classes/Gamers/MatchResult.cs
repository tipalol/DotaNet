using System;
namespace DotaNet.Classes.Gamers
{
    public struct MatchResult
    {
        Team Left { get; set; }
        Team Right { get; set; }
        int ResultOfLeft { get; set; }
        int ResultOfRight { get; set; }
        public MatchResult(Team left, Team right, int resultOfLeft, int resultOfRight)
        {
            Left = left;
            Right = right;
            ResultOfLeft = resultOfLeft;
            ResultOfRight = resultOfRight;
        }
    }
}
