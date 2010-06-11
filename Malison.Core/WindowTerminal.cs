using System;
using System.Collections.Generic;
using System.Text;

using Bramble.Core;

namespace Malison.Core
{
    public class WindowTerminal : TerminalBase
    {
        public WindowTerminal(TerminalBase parent, TermColor foreColor, TermColor backColor, Rect bounds)
            : base(foreColor, backColor)
        {
            mParent = parent;
            mBounds = bounds;
        }

        public override Vec Size { get { return mBounds.Size; } }

        protected override Character GetValue(Vec pos)
        {
            return mParent.Get(pos + mBounds.Position);
        }

        protected override bool SetValue(Vec pos, Character value)
        {
            if (!mBounds.Size.Contains(pos)) return false;

            return mParent.SetInternal(pos + mBounds.Position, value);
        }

        private TerminalBase mParent;
        private Rect mBounds;
    }
}
