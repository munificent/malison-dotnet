using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Bramble.Core;

using Malison.Core;

namespace Malison.WinForms
{
    public static class TermColorExtensions
    {
        public static Color ToSystemColor(this TermColor color)
        {
            switch (color)
            {
                case TermColor.Black: return Color.Black;
                case TermColor.White: return Color.White;

                case TermColor.LightGray: return Color.FromArgb(192, 192, 192);
                case TermColor.Gray: return Color.FromArgb(128, 128, 128);
                case TermColor.DarkGray: return Color.FromArgb(48, 48, 48);

                case TermColor.LightRed:
                case TermColor.Pink:
                    return Color.FromArgb(255, 160, 160);

                case TermColor.Red: return Color.FromArgb(220, 0, 0);
                case TermColor.DarkRed: return Color.FromArgb(100, 0, 0);

                case TermColor.LightOrange:
                case TermColor.Flesh:
                    return Color.FromArgb(255, 200, 170);

                case TermColor.Orange: return Color.FromArgb(255, 128, 0);
                case TermColor.DarkOrange: return Color.FromArgb(128, 64, 0);

                case TermColor.LightGold: return Color.FromArgb(255, 230, 150);
                case TermColor.Gold: return Color.FromArgb(255, 192, 0);
                case TermColor.DarkGold: return Color.FromArgb(128, 96, 0);

                case TermColor.LightYellow: return Color.FromArgb(255, 255, 150);
                case TermColor.Yellow: return Color.FromArgb(255, 255, 0);
                case TermColor.DarkYellow: return Color.FromArgb(128, 128, 0);

                case TermColor.LightGreen: return Color.FromArgb(130, 255, 90);
                case TermColor.Green: return Color.FromArgb(0, 200, 0);
                case TermColor.DarkGreen: return Color.FromArgb(0, 100, 0);

                case TermColor.LightCyan: return Color.FromArgb(200, 255, 255);
                case TermColor.Cyan: return Color.FromArgb(0, 255, 255);
                case TermColor.DarkCyan: return Color.FromArgb(0, 128, 128);

                case TermColor.LightBlue: return Color.FromArgb(128, 160, 255);
                case TermColor.Blue: return Color.FromArgb(0, 64, 255);
                case TermColor.DarkBlue: return Color.FromArgb(0, 37, 168);

                case TermColor.LightPurple: return Color.FromArgb(200, 140, 255);
                case TermColor.Purple: return Color.FromArgb(128, 0, 255);
                case TermColor.DarkPurple: return Color.FromArgb(64, 0, 128);

                case TermColor.LightBrown: return Color.FromArgb(190, 150, 100);
                case TermColor.Brown: return Color.FromArgb(160, 110, 60);
                case TermColor.DarkBrown: return Color.FromArgb(100, 64, 32);

                default: throw new UnexpectedEnumValueException(color);
            }
        }
    }
}
