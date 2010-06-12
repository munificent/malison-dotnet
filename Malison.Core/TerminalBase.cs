using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public abstract class TerminalBase : ITerminal
    {
        public TerminalBase() : this(TermColor.White, TermColor.Black)
        {
        }

        public TerminalBase(TermColor foreColor, TermColor backColor)
        {
            ForeColor = foreColor;
            BackColor = backColor;
        }

        #region IReadableTerminal Members

        public event EventHandler<CharacterEventArgs> CharacterChanged;

        public abstract Vec Size { get; }

        public TermColor ForeColor { get; private set; }
        public TermColor BackColor { get; private set; }

        public Character Get(Vec pos)
        {
            return GetValueCore(FlipNegativePosition(pos));
        }

        public Character Get(int x, int y)
        {
            return Get(new Vec(x, y));
        }

        #endregion

        #region ITerminal Members

        public void Set(Vec pos, Character value)
        {
            SetInternal(FlipNegativePosition(pos), value);
        }

        public void Set(int x, int y, Character value)
        {
            Set(new Vec(x, y), value);
        }

        public ITerminal this[Vec pos]
        {
            // if we aren't given a size, go all the way to the bottom-right corner of the terminal
            get { return this[pos, Size - pos]; }
        }

        public ITerminal this[int x, int y]
        {
            get { return this[new Vec(x, y)]; }
        }

        public ITerminal this[Rect rect]
        {
            get
            {
                return CreateWindowCore(ForeColor, BackColor, new Rect(FlipNegativePosition(rect.Position), rect.Size));
            }
        }

        public ITerminal this[Vec pos, Vec size]
        {
            get { return this[new Rect(pos, size)]; }
        }

        public ITerminal this[int x, int y, int width, int height]
        {
            get { return this[new Rect(x, y, width, height)]; }
        }

        public ITerminal this[TermColor foreColor, TermColor backColor]
        {
            get
            {
                return CreateWindowCore(foreColor, backColor, new Rect(Size));
            }
        }

        public ITerminal this[ColorPair color]
        {
            get { return this[color.Fore, color.Back]; }
        }

        public ITerminal this[TermColor foreColor]
        {
            get { return this[foreColor, BackColor]; }
        }

        public void Write(char ascii)
        {
            Write(new Character(ascii, ForeColor, BackColor));
        }

        public void Write(Glyph glyph)
        {
            Write(new Character(glyph, ForeColor, BackColor));
        }

        public void Write(Character character)
        {
            Set(Vec.Zero, character);
        }

        public void Write(string text)
        {
            Write(new CharacterString(text, ForeColor, BackColor));
        }

        public void Write(CharacterString text)
        {
            Vec pos = Vec.Zero;

            CheckBounds(pos.X, pos.Y);

            foreach (Character c in text)
            {
                Set(pos, c);
                pos += new Vec(1, 0);

                // don't run past edge
                if (pos.X >= Size.X) break;
            }
        }

        public void Scroll(Vec offset, Func<Vec, Character> scrollOnCallback)
        {
            int xStart = 0;
            int xEnd = Size.X;
            int xStep = 1;

            int yStart = 0;
            int yEnd = Size.Y;
            int yStep = 1;

            if (offset.X > 0)
            {
                xStep = -1;

                Obj.Swap(ref xStart, ref xEnd);

                // shift the bounds back. the loops below
                // are half-inclusive, so when we flip the
                // range, we need to make it inclusive on the
                // other end of the range.
                // in other words, if the bounds are 10-50,
                // when we flip, we need to iterate from 9-49
                xStart--;
                xEnd--;
            }

            if (offset.Y > 0)
            {
                yStep = -1;

                Obj.Swap(ref yStart, ref yEnd);

                // shift the bounds back. the loops below
                // are half-inclusive, so when we flip the
                // range, we need to make it inclusive on the
                // other end of the range.
                // in other words, if the bounds are 10-50,
                // when we flip, we need to iterate from 9-49
                yStart--;
                yEnd--;
            }

            Rect bounds = new Rect(Size);

            for (int y = yStart; y != yEnd; y += yStep)
            {
                for (int x = xStart; x != xEnd; x += xStep)
                {
                    Vec to = new Vec(x, y);
                    Vec from = to - offset;

                    if (bounds.Contains(from))
                    {
                        // can be scrolled from
                        Set(to, Get(from));
                    }
                    else
                    {
                        // nothing to scroll onto this char, so clear it
                        Set(to, scrollOnCallback(to));
                    }
                }
            }
        }

        public void Scroll(int x, int y, Func<Vec, Character> scrollOnCallback)
        {
            Scroll(new Vec(x, y), scrollOnCallback);
        }

        public void Clear()
        {
            Fill(Glyph.Space);
        }

        public void Fill(Glyph glyph)
        {
            Character character = new Character(glyph, ForeColor, BackColor);
            foreach (Vec pos in new Rect(Size))
            {
                Set(pos, character);
            }
        }

        public void DrawBox(bool isDouble, bool isContinue)
        {
            Vec pos = Vec.Zero;

            if (Size.X == 1)
            {
                DrawVerticalLine(pos, Size.Y, isDouble, isContinue);
            }
            else if (Size.Y == 1)
            {
                DrawHorizontalLine(pos, Size.X, isDouble, isContinue);
            }
            else
            {
                // figure out which glyphs to use
                Glyph topLeft;
                Glyph topRight;
                Glyph bottomLeft;
                Glyph bottomRight;
                Glyph horizontal;
                Glyph vertical;

                if (isDouble)
                {
                    topLeft = Glyph.BarDoubleDownRight;
                    topRight = Glyph.BarDoubleDownLeft;
                    bottomLeft = Glyph.BarDoubleUpRight;
                    bottomRight = Glyph.BarDoubleUpLeft;
                    horizontal = Glyph.BarDoubleLeftRight;
                    vertical = Glyph.BarDoubleUpDown;
                }
                else
                {
                    topLeft = Glyph.BarDownRight;
                    topRight = Glyph.BarDownLeft;
                    bottomLeft = Glyph.BarUpRight;
                    bottomRight = Glyph.BarUpLeft;
                    horizontal = Glyph.BarLeftRight;
                    vertical = Glyph.BarUpDown;
                }

                // top left corner
                WriteLineChar(pos, topLeft);

                // top right corner
                WriteLineChar(pos.OffsetX(Size.X - 1), topRight);

                // bottom left corner
                WriteLineChar(pos.OffsetY(Size.Y - 1), bottomLeft);

                // bottom right corner
                WriteLineChar(pos + Size - 1, bottomRight);

                // top and bottom edges
                foreach (Vec iter in Rect.Row(pos.X + 1, pos.Y, Size.X - 2))
                {
                    WriteLineChar(iter, horizontal);
                    WriteLineChar(iter.OffsetY(Size.Y - 1), horizontal);
                }

                // left and right edges
                foreach (Vec iter in Rect.Column(pos.X, pos.Y + 1, Size.Y - 2))
                {
                    WriteLineChar(iter, vertical);
                    WriteLineChar(iter.OffsetX(Size.X - 1), vertical);
                }
            }
        }

        #endregion

        internal bool SetInternal(Vec pos, Character value)
        {
            if (SetValueCore(pos, value))
            {
                if (CharacterChanged != null) CharacterChanged(this, new CharacterEventArgs(value, pos));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Override this to get the <see cref="Character"/> at the given position in the terminal.
        /// </summary>
        /// <param name="pos">The position of the character to retrieve. Must be in bounds.</param>
        /// <returns>The character at that position.</returns>
        protected abstract Character GetValueCore(Vec pos);

        /// <summary>
        /// Override this to set the <see cref="Character"/> at the given position in the terminal.
        /// </summary>
        /// <param name="pos">The position of the character to write.</param>
        /// <param name="value">The character to write to the terminal.</param>
        /// <returns><c>true</c> if the character is different from what was already there.</returns>
        protected abstract bool SetValueCore(Vec pos, Character value);

        internal abstract ITerminal CreateWindowCore(TermColor foreColor, TermColor backColor, Rect bounds);

        private Vec FlipNegativePosition(Vec pos)
        {
            // negative coordinates mean from the right/bottom edge
            if (pos.X < 0) pos.X = Size.X + pos.X;
            if (pos.Y < 0) pos.Y = Size.Y + pos.Y;

            return pos;
        }

        private void DrawHorizontalLine(Vec pos, int length, bool isDouble, bool isContinue)
        {
            // figure out which glyphs to use
            Glyph left = Glyph.BarRight;
            Glyph middle = Glyph.BarLeftRight;
            Glyph right = Glyph.BarLeft;

            if (isDouble)
            {
                middle = Glyph.BarDoubleLeftRight;

                if (isContinue)
                {
                    left = Glyph.BarDoubleLeftRight;
                    right = Glyph.BarDoubleLeftRight;
                }
                else
                {
                    left = Glyph.BarDoubleRight;
                    right = Glyph.BarDoubleLeft;
                }
            }
            else
            {
                if (isContinue)
                {
                    left = Glyph.BarLeftRight;
                    right = Glyph.BarLeftRight;
                }
            }

            // left edge
            WriteLineChar(pos, left);

            // right edge
            WriteLineChar(pos.OffsetX(length - 1), right);

            // middle
            foreach (Vec iter in Rect.Row(pos.X + 1, pos.Y, length - 2))
            {
                WriteLineChar(iter, middle);
            }
        }

        private void DrawVerticalLine(Vec pos, int length, bool isDouble, bool isContinue)
        {
            // figure out which glyphs to use
            Glyph top = Glyph.BarDown;
            Glyph middle = Glyph.BarUpDown;
            Glyph bottom = Glyph.BarUp;

            if (isDouble)
            {
                middle = Glyph.BarDoubleUpDown;

                if (isContinue)
                {
                    top = Glyph.BarDoubleUpDown;
                    bottom = Glyph.BarDoubleUpDown;
                }
                else
                {
                    top = Glyph.BarDoubleDown;
                    bottom = Glyph.BarDoubleUp;
                }
            }
            else
            {
                if (isContinue)
                {
                    top = Glyph.BarUpDown;
                    bottom = Glyph.BarUpDown;
                }
            }

            // top edge
            WriteLineChar(pos, top);

            // bottom edge
            WriteLineChar(pos.OffsetY(length - 1), bottom);

            // middle
            foreach (Vec iter in Rect.Column(pos.X, pos.Y + 1, length - 2))
            {
                WriteLineChar(iter, middle);
            }
        }

        private void WriteLineChar(Vec pos, Glyph glyph)
        {
            this[pos][ForeColor, BackColor].Write(glyph);
        }

        private void CheckBounds(int x, int y)
        {
            if (x >= Size.X) throw new ArgumentOutOfRangeException("x");
            if (y >= Size.Y) throw new ArgumentOutOfRangeException("y");

            // negative values are valid and mean "from the right or bottom", so apply and check range
            if ((x < 0) && (Size.X + x >= Size.X)) throw new ArgumentOutOfRangeException("x");
            if ((y < 0) && (Size.Y + y >= Size.Y)) throw new ArgumentOutOfRangeException("y");
        }

        private void CheckBounds(int x, int y, int width, int height)
        {
            //### bob: need to handle negative coords
            if (x < 0) throw new ArgumentOutOfRangeException("x");
            if (x >= Size.X) throw new ArgumentOutOfRangeException("x");
            if (y < 0) throw new ArgumentOutOfRangeException("y");
            if (y >= Size.Y) throw new ArgumentOutOfRangeException("y");
            if (width <= 0) throw new ArgumentException("width");
            if (x + width > Size.X) throw new ArgumentOutOfRangeException("width");
            if (height <= 0) throw new ArgumentException("height");
            if (y + height > Size.Y) throw new ArgumentOutOfRangeException("height");
        }

        private void CheckBounds(Vec pos, Vec size)
        {
            CheckBounds(pos.X, pos.Y, size.X, size.Y);
        }
    }
}
