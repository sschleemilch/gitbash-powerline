using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitbash_powerline
{

    class Program
    {
        static string currentWorkingDirectory = Environment.GetEnvironmentVariable("PWD");
        static string homeDirectory = Environment.GetEnvironmentVariable("HOME").Replace('\\', '/');

        static GitRepo repo;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            printCWD();

            repo = new GitRepo(currentWorkingDirectory);

            if (repo.isRepo)
            {
                printGitBranchInfos();
                if (!repo.isDetachedHead)
                {
                    printGitRemoteInfos();
                }
                printGitMergeState();
                printGitFilesInfos();
            }

            printPromptSymbol();
        }

        static void printGitMergeState()
        {
            if (repo.isInMerge)
            {
                string mergeSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.MERGE);
                BashColor.print(" " + mergeSymbol + " MERGE ", BashColor.COLOR.BLACK, BashColor.COLOR.YELLOW);
            }
        }

        static void printGitRemoteInfos()
        {
            
            if (!repo.branchHasUpstream)
            {
                string udArrow = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.ARROW_UP_DOWN);
                BashColor.print(" " + udArrow + "? ", BashColor.COLOR.RED, BashColor.COLOR.GREY_DARK);
                return;
            }

            if (repo.isAhead)
            {
                string uArrow = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.ARROW_UP);
                string cAhead = repo.commitsAhead.ToString();
                string space = " ";
                if (repo.isBehind)
                {
                    space = "";
                }
                BashColor.print(" " + cAhead + uArrow + space, BashColor.COLOR.GREEN, BashColor.COLOR.GREY_DARK);
            }
            
            if (repo.isAhead && repo.isBehind)
            {
                string warningSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.LIGHTNING);
                BashColor.print(warningSymbol + " ", BashColor.COLOR.YELLOW, BashColor.COLOR.GREY_DARK);
            }

            if (repo.isBehind)
            {
                string dArrow = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.ARROW_DOWN);
                string cBehind = repo.commitsBehind.ToString();
                string space = " ";
                if (repo.isAhead)
                {
                    space = "";
                }
                BashColor.print(space + dArrow + cBehind + " ", BashColor.COLOR.RED, BashColor.COLOR.GREY_DARK);
            }
        }

        static void printGitBranchInfos()
        {
            if (repo.isDetachedHead)
            {
                string anchorSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.ANCHOR);
                BashColor.print(" " + anchorSymbol + " " + repo.commitID + " ", BashColor.COLOR.BLACK, BashColor.COLOR.ORANGE);

                if (repo.hasModifiedFiles || 
                    repo.hasStagedFiles)
                {
                    string dangerSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.LIGHTNING);
                    BashColor.print(" " + dangerSymbol + " DANGER " + dangerSymbol + " ", BashColor.COLOR.BLACK, BashColor.COLOR.RED, true);
                }
            } else
            {
                BashColor.COLOR branchBGColor = BashColor.COLOR.BLACK;
                string branchPreSymbol = " ";
                if (repo.hasModifiedFiles ||
                    repo.hasStagedFiles ||
                    repo.hasUntrackedFilesNotIgnored)
                {
                    branchBGColor = BashColor.COLOR.ORANGE;
                }

                if (!repo.hasModifiedFiles &&
                    !repo.hasStagedFiles &&
                    !repo.hasUntrackedFilesNotIgnored)
                {
                    branchPreSymbol = " " + UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.SUN) + " ";
                    branchBGColor = BashColor.COLOR.GREEN;
                } else
                {
                    branchPreSymbol = " " + UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.DELTA) + " ";
                }
                BashColor.print(branchPreSymbol + repo.branch + " ", BashColor.COLOR.BLACK, branchBGColor);

            }
        }

        static void printGitFilesInfos()
        {
            if (repo.hasModifiedFiles)
            {
                string modSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.PENCIL);
                BashColor.print(" " + modSymbol + " ", BashColor.COLOR.ORANGE, BashColor.COLOR.GREY_DARK);
            }
            if (repo.hasUntrackedFilesNotIgnored)
            {
                string untrackedSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.PLUS);
                BashColor.print(" " + untrackedSymbol + " ", BashColor.COLOR.YELLOW, BashColor.COLOR.GREY_DARK);
            }
            if (repo.hasStagedFiles)
            {
                string stagedSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.FLAG);
                BashColor.print(" " + stagedSymbol + " ", BashColor.COLOR.GREEN, BashColor.COLOR.GREY_DARK);
            }
        }

        static void printPromptSymbol()
        {
            string promptSymbol = UnicodeSymbols.getString(UnicodeSymbols.SYMBOL.ARROW_RIGHT);
            BashColor.print(" " + promptSymbol + " ", BashColor.COLOR.BLUE, BashColor.COLOR.TRANSPARENT);
        }

        static void printCWD()
        {
            BashColor.print(" " + getCWDString() + " ", BashColor.COLOR.BLACK, BashColor.COLOR.BLUE);
        }

        static string getCWDString()
        {
            string pathString = currentWorkingDirectory;
            if (currentWorkingDirectory.Contains(homeDirectory))
            {
                pathString = pathString.Replace(homeDirectory, "~");
            }

            string[] pathStringSplit = pathString.Split('/');
            string[] pathStringShortened = new string[4];

            int pathStringSplitLenght = pathStringSplit.Length;

            if (pathStringSplit.Length > 4)
            {
                pathStringShortened[0] = pathStringSplit[0];
                pathStringShortened[1] = "...";
                pathStringShortened[2] = pathStringSplit[pathStringSplitLenght - 2];
                pathStringShortened[3] = pathStringSplit[pathStringSplitLenght - 1];
                pathString = String.Join("/", pathStringShortened);
            }

            return pathString;
        }
    }
}
