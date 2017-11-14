using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitbash_powerline
{
    static class BashColor
    {
        public enum COLOR
        {
            GREEN, RED, BLACK, WHITE, ORANGE, BLUE, GREY, GREY_DARK, GREY_LIGHT, YELLOW, TRANSPARENT, PINK
        };

        private static string getEscapeSequenceForFGColor(COLOR c)
        {
            string fgStart = "\\[\\033[38;5;";
            string fgEnd = "m\\]";
            switch(c)
            {
                case COLOR.GREEN:
                    return fgStart + "2" + fgEnd;
                case COLOR.RED:
                    return fgStart + "9" + fgEnd;
                case COLOR.BLACK:
                    return fgStart + "0" + fgEnd;
                case COLOR.WHITE:
                    return fgStart + "231" + fgEnd;
                case COLOR.ORANGE:
                    return fgStart + "202" + fgEnd;
                case COLOR.BLUE:
                    return fgStart + "39" + fgEnd;
                case COLOR.GREY:
                    return fgStart + "245" + fgEnd;
                case COLOR.GREY_DARK:
                    return fgStart + "239" + fgEnd;
                case COLOR.GREY_LIGHT:
                    return fgStart + "251" + fgEnd;
                case COLOR.YELLOW:
                    return fgStart + "190" + fgEnd;
                case COLOR.TRANSPARENT:
                    return fgStart + "8" + fgEnd;
                case COLOR.PINK:
                    return fgStart + "213" + fgEnd;

            }
            return null;
        }

        private static string getEscapeSequenceForBGColor(COLOR c)
        {
            string bgStart = "\\[\\033[48;5;";
            string bgEnd = "m\\]";
            switch (c)
            {
                case COLOR.GREEN:
                    return bgStart + "2" + bgEnd;
                case COLOR.RED:
                    return bgStart + "1" + bgEnd;
                case COLOR.BLACK:
                    return bgStart + "0" + bgEnd;
                case COLOR.WHITE:
                    return bgStart + "231" + bgEnd;
                case COLOR.ORANGE:
                    return bgStart + "208" + bgEnd;
                case COLOR.BLUE:
                    return bgStart + "39" + bgEnd;
                case COLOR.GREY:
                    return bgStart + "245" + bgEnd;
                case COLOR.GREY_DARK:
                    return bgStart + "239" + bgEnd;
                case COLOR.GREY_LIGHT:
                    return bgStart + "251" + bgEnd;
                case COLOR.YELLOW:
                    return bgStart + "3" + bgEnd;
                case COLOR.TRANSPARENT:
                    return bgStart + "8" + bgEnd;
                case COLOR.PINK:
                    return bgStart + "213" + bgEnd;

            }
            return null;
        }

        private static string getResetSequence()
        {
            return "\\[\\033[0m\\]";
        }

        private static string getBoldSequence()
        {
            return "\\[\\033[1m\\]";
        }

        public static void print(string s, COLOR fgC, COLOR bgC, bool bold = false)
        {
            string colorString = getEscapeSequenceForFGColor(fgC) +
                getEscapeSequenceForBGColor(bgC);

            if (bold)
            {
                colorString += getBoldSequence();
            }
            colorString += s + getResetSequence();

            Console.Write(colorString);
        }
    }
}