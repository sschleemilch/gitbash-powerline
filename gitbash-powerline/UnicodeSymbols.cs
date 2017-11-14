using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitbash_powerline
{
    static class UnicodeSymbols
    {
        public enum SYMBOL
        {
            CROSS,
            ANCHOR,
            LIGHTNING,
            FLAG,
            PENCIL,
            PLUS,
            DELTA,
            ARROW_UP_DOWN,
            CHECK,
            SUN,
            HEART,
            DANGER,
            ARROW_RIGHT,
            ARROW_UP,
            ARROW_DOWN,
            MERGE,
            ARROW_RIGHT_2,
            TRIANGLE_RIGHT
        }

        public static string getString(SYMBOL s)
        {
            switch (s) {
                case SYMBOL.CROSS:
                    return "\u2715";
                case SYMBOL.ANCHOR:
                    return "\u2693";
                case SYMBOL.LIGHTNING:
                    return "\u21AF";
                case SYMBOL.FLAG:
                    return "\u2691";
                case SYMBOL.PENCIL:
                    return "\u270E";
                case SYMBOL.PLUS:
                    return "\u002B";
                case SYMBOL.DELTA:
                    return "\u0394";
                case SYMBOL.ARROW_UP_DOWN:
                    return "\u21C5";
                case SYMBOL.CHECK:
                    return "\u2713";
                case SYMBOL.SUN:
                    return "\u2600";
                case SYMBOL.HEART:
                    return "\u2665";
                case SYMBOL.DANGER:
                    return "\u26A0";
                case SYMBOL.ARROW_RIGHT:
                    return "\u2023";
                case SYMBOL.ARROW_UP:
                    return "\u2191";
                case SYMBOL.ARROW_DOWN:
                    return "\u2193";
                case SYMBOL.MERGE:
                    return "\u26D5";
                case SYMBOL.ARROW_RIGHT_2:
                    return "\u21A6";
                case SYMBOL.TRIANGLE_RIGHT:
                    return "\u22B3";
            }
            return null;
        }
    }
}
