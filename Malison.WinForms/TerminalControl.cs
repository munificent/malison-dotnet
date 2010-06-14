using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Bramble.Core;

using Malison.Core;

namespace Malison.WinForms
{
    public partial class TerminalControl : UserControl
    {
        public TerminalControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Opaque, true);

            mGlyphSheet = GlyphSheet.Terminal7x10;

            HideCursor = true;
        }

        /// <summary>
        /// Gets and sets whether or not the cursor should be hidden when it hovers of the control.
        /// </summary>
        [Description("Whether or not the cursor should be hidden when over this control.")]
        public bool HideCursor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ITerminal Terminal
        {
            get { return mTerminal; }
            set
            {
                if (mTerminal != value)
                {
                    if (mTerminal != null)
                    {
                        mTerminal.CharacterChanged -= Terminal_CharacterChanged;
                    }

                    mTerminal = value;

                    if (mTerminal != null)
                    {
                        mTerminal.CharacterChanged += Terminal_CharacterChanged;
                    }
                    Invalidate();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GlyphSheet GlyphSheet
        {
            get { return mGlyphSheet; }
            set
            {
                if (mGlyphSheet != value)
                {
                    mGlyphSheet = value;
                    Invalidate();
                }
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(
                (mGlyphSheet.Width * mTerminal.Size.X) + (Padding * 2),
                (mGlyphSheet.Height * mTerminal.Size.Y) + (Padding * 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            if (mTerminal != null)
            {
                // only refresh characters in the clip rect
                int left = Math.Max(0, (e.ClipRectangle.Left - Padding) / mGlyphSheet.Width);
                int top = Math.Max(0, (e.ClipRectangle.Top - Padding) / mGlyphSheet.Height);
                int right = Math.Min(mTerminal.Size.X, (e.ClipRectangle.Right - Padding) / mGlyphSheet.Width + 1);
                int bottom = Math.Min(mTerminal.Size.Y, (e.ClipRectangle.Bottom - Padding) / mGlyphSheet.Height + 1);

                for (int y = top; y < bottom; y++)
                {
                    for (int x = left; x < right; x++)
                    {
                        Character character = mTerminal.Get(x, y);

                        // fill the background if needed
                        if (!character.BackColor.Equals(Color.Black))
                        {
                            int fillLeft = (x * mGlyphSheet.Width) + Padding;
                            int fillTop = (y * mGlyphSheet.Height) + Padding;
                            int width = mGlyphSheet.Width;
                            int height = mGlyphSheet.Height;

                            // fill past the padding on the edges
                            if (x == 0)
                            {
                                fillLeft -= Padding;
                                width += Padding;
                            }
                            if (x == mTerminal.Size.X - 1)
                            {
                                width += Padding;
                            }
                            if (y == 0)
                            {
                                fillTop -= Padding;
                                height += Padding;
                            }
                            if (y == mTerminal.Size.Y - 1)
                            {
                                height += Padding;
                            }

                            e.Graphics.FillRectangle(new SolidBrush(character.BackColor.ToSystemColor()),
                                fillLeft, fillTop, width, height);
                        }

                        // draw the glyph
                        mGlyphSheet.Draw(e.Graphics,
                            (x * mGlyphSheet.Width) + Padding,
                            (y * mGlyphSheet.Height) + Padding,
                            character);
                    }
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (HideCursor)
            {
                Cursor.Hide();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (HideCursor)
            {
                Cursor.Show();
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.KeyCode == Keys.Tab) e.IsInputKey = true;
            if (e.KeyCode == Keys.Up) e.IsInputKey = true;
            if (e.KeyCode == Keys.Down) e.IsInputKey = true;
            if (e.KeyCode == Keys.Left) e.IsInputKey = true;
            if (e.KeyCode == Keys.Right) e.IsInputKey = true;
        }

        private void InvalidateCharacter(Vec pos)
        {
            int width = mGlyphSheet.Width;
            int height = mGlyphSheet.Height;
            int left = (pos.X * width) + Padding;
            int top = (pos.Y * height) + Padding;

            // fill past the padding on the edges
            if (pos.X == 0)
            {
                left -= Padding;
                width += Padding;
            }
            if (pos.X == mTerminal.Size.X - 1)
            {
                width += Padding;
            }
            if (pos.Y == 0)
            {
                top -= Padding;
                height += Padding;
            }
            if (pos.Y == mTerminal.Size.Y - 1)
            {
                height += Padding;
            }

            // invalidate the rect under the character
            Invalidate(new Rectangle(left, top, width, height));
        }

        private void Terminal_CharacterChanged(object sender, CharacterEventArgs e)
        {
            InvalidateCharacter(e.Position);
        }

        private const int Padding = 2;

        private GlyphSheet mGlyphSheet;
        private ITerminal mTerminal;
    }
}
