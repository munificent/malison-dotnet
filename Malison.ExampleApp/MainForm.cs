using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Malison.Core;
using Malison.WinForms;

namespace Malison.ExampleApp
{
    public partial class MainForm : TerminalForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Terminal[2, 2].Write("Hi, this is an example app.");

            Terminal[2, 4].Write("Here are some lines and boxes:");
            Terminal[4, 6, 6, 3][TermColor.Red].DrawBox();
            Terminal[12, 6, 6, 3][TermColor.Green].DrawBox(DrawBoxOptions.DoubleLines);

            ITerminal blueTerm = Terminal[TermColor.Blue];
            blueTerm[20, 6, 1, 3].DrawBox(DrawBoxOptions.None);
            blueTerm[21, 6, 1, 3].DrawBox(DrawBoxOptions.ContinueLines);
            blueTerm[22, 6, 1, 3].DrawBox(DrawBoxOptions.DoubleLines);
            blueTerm[23, 6, 1, 3].DrawBox(DrawBoxOptions.DoubleLines | DrawBoxOptions.ContinueLines);

            Terminal[2, 10].Write("Because this is tailored for games, there's some fun glyphs in here:");
            Glyph[] glyphs = new Glyph[]
            {
                Glyph.ArrowDown,
                Glyph.ArrowLeft,
                Glyph.ArrowRight,
                Glyph.ArrowUp,
                Glyph.Box,
                Glyph.Bullet,
                Glyph.Dark,
                Glyph.DarkFill,
                Glyph.Dashes,
                Glyph.Door,
                Glyph.Face,
                Glyph.Grass,
                Glyph.Gray,
                Glyph.GrayFill,
                Glyph.Hill,
                Glyph.HorizontalBars,
                Glyph.HorizontalBarsFill,
                Glyph.Light,
                Glyph.LightFill,
                Glyph.Mountains,
                Glyph.Solid,
                Glyph.SolidFill,
                Glyph.Tombstone,
                Glyph.TreeConical,
                Glyph.TreeDots,
                Glyph.TreeRound,
                Glyph.TriangleDown,
                Glyph.TriangleLeft,
                Glyph.TriangleRight,
                Glyph.TriangleUp,
                Glyph.TwoDots,
                Glyph.VerticalBars,
                Glyph.VerticalBarsFill
            };

            int x = 4;
            for (int i = 0; i < glyphs.Length; i++)
            {
                Terminal[x, 12][TermColor.Orange].Write(glyphs[i]);
                x += 2;
            }

            Terminal[2, 14].Write("Background and foreground colors are supported:");

            TermColor[] colors = (TermColor[])Enum.GetValues(typeof(TermColor));
            for (int i = 0; i < colors.Length; i++)
            {
                Terminal[i + 3, 16][colors[i], TermColor.Black].Write(Glyph.Face);
                Terminal[i + 3, 17][TermColor.Black, colors[i]].Write(Glyph.Face);
            }
        }
    }
}
