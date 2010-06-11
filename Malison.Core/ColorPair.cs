using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malison.Core
{
    //### bob: get rid of this?
    public class ColorPair
    {
        public TermColor Fore;
        public TermColor Back;

        public ColorPair(TermColor fore, TermColor back)
        {
            Fore = fore;
            Back = back;
        }
    }
}
