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

        protected override Character GetValueCore(Vec pos)
        {
            return mParent.Get(pos + mBounds.Position);
        }

        protected override bool SetValueCore(Vec pos, Character value)
        {
            if (!mBounds.Size.Contains(pos)) return false;

            return mParent.SetInternal(pos + mBounds.Position, value);
        }

        internal override ITerminal CreateWindowCore(TermColor foreColor, TermColor backColor, Rect bounds)
        {
            // transform by this window's bounds and then defer to the parent.
            // this flattens out a chain of windows at creation time so that
            // drawing commands don't have to burn cycles walking up the chain
            // each time. this shaves a measurable chunk of time off drawing.
            bounds += mBounds.Position;
            bounds = bounds.Intersect(mBounds);

            return mParent.CreateWindowCore(foreColor, backColor, bounds);
        }

        private TerminalBase mParent;
        private Rect mBounds;
    }
}
