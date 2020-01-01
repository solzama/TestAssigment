using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GameEngine
{
    [Serializable]
    public enum CellState
    {
        [EnumMember]
        Empty,
        [EnumMember]
        WhiteBall = 1,
        [EnumMember]
        BlackBall = 2
    }
    
    // Console.OutputEncoding = System.Text.Encoding.Unicode;
    // string whiteBall = "\u25ef"; - colors are inverted in console output
    // string blackBall = "\u2b24";
}