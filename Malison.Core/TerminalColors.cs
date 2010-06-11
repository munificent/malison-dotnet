using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

//### bob: rename file

namespace Malison.Core
{
    public enum TermColor
    {
        Black,
        White,
        
        LightGray,
        Gray,
        DarkGray,

        LightRed,
        Red,
        DarkRed,

        LightOrange,
        Orange,
        DarkOrange,

        LightGold,
        Gold,
        DarkGold,

        LightYellow,
        Yellow,
        DarkYellow,

        LightGreen,
        Green,
        DarkGreen,

        LightCyan,
        Cyan,
        DarkCyan,

        LightBlue,
        Blue,
        DarkBlue,

        LightPurple,
        Purple,
        DarkPurple,

        LightBrown,
        Brown,
        DarkBrown,

        Flesh,
        Pink
    }

    public static class TerminalColors
    {
        /*
        Black { get { return Color.Black; } }
        White { get { return Color.White; } }

        LightGray,
        Gray,
        DarkGray,

        LightRed,
        Red,
        DarkRed,

        LightOrange,
        Orange,
        DarkOrange,

        LightGold,
        Gold,
        DarkGold,

        LightYellow,
        Yellow,
        DarkYellow,

        LightGreen,
        Green,
        DarkGreen,

        LightCyan,
        Cyan,
        DarkCyan,

        LightBlue,
        Blue,
        DarkBlue,

        LightPurple,
        Purple,
        DarkPurple,

        LightBrown,
        Brown,
        DarkBrown,

        Flesh { get { return LightOrange; } }
        Pink { get { return LightRed; } }
        
        FromName(string name)
        {
            // use reflection to go through the properties
            foreach (PropertyInfo property in typeof(TerminalColors).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (property.PropertyType.Equals(typeof(Color)) && property.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    // found it
                    return (Color)property.GetValue(null, new object[0]);
                }
            }

            throw new ArgumentException("Could not find a color named \"" + name + "\" in TerminalColors.");
        }
        */

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
