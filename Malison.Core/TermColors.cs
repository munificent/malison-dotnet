using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Malison.Core
{
    /// <summary>
    /// Static class containing helper functions for dealing with <see cref="TermColor"/> values.
    /// </summary>
    public static class TermColors
    {
        public static TermColor FromName(string name)
        {
            return (TermColor)Enum.Parse(typeof(TermColor), name);
        }

        public static TermColor FromEscapeChar(char c)
        {
            switch (c)
            {
                case 'k': return TermColor.DarkGray;
                case 'K': return TermColor.Black;

                case 'm': return TermColor.Gray; // "m"edium

                case 'w': return TermColor.White;
                case 'W': return TermColor.LightGray;

                case 'r': return TermColor.Red;
                case 'R': return TermColor.DarkRed;

                case 'o': return TermColor.Orange;
                case 'O': return TermColor.DarkOrange;

                case 'l': return TermColor.Gold;
                case 'L': return TermColor.DarkGold;

                case 'y': return TermColor.Yellow;
                case 'Y': return TermColor.DarkYellow;

                case 'g': return TermColor.Green;
                case 'G': return TermColor.DarkGreen;

                case 'c': return TermColor.Cyan;
                case 'C': return TermColor.DarkCyan;

                case 'b': return TermColor.Blue;
                case 'B': return TermColor.DarkBlue;

                case 'p': return TermColor.Purple;
                case 'P': return TermColor.DarkPurple;

                case 'f': return TermColor.Flesh;
                case 'F': return TermColor.Brown;

                default: return TermColor.White;
            }
        }
    }
}
