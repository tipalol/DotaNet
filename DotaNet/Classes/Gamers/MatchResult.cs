using System;
using System.Runtime.Serialization;

namespace DotaNet.Classes.Gamers
{
    [DataContract]
    public struct MatchResult
    {
        public bool isEmpty()
        {
            if (Left == null && Right == null)
                return true;
            return false;
        }
        [DataMember]
        public Team Left { get; set; }
        [DataMember]
        public Team Right { get; set; }
        [DataMember]
        public int ResultOfLeft { get; set; }
        [DataMember]
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
