using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malison.Core
{
    /// <summary>
    /// Identifies a color that can be used in a terminal. An enumeration of
    /// fixed colors is used here to avoid making Malison.Core dependent on
    /// System.Drawing or some other assembly that provides a color type.
    /// </summary>
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
}
