using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace gitbash_powerline
{
    class GitRepo
    {
        private static int gitProcessTimeout = 1000;

        public bool isRepo = false;

        private string repoRoot;
        private string workingDirectory;

        public bool isDetachedHead;
        public string branch;
        private string branchRaw;
        public bool branchHasUpstream;
        public string commitID;

        public bool hasUntrackedFilesNotIgnored;
        public bool hasStagedFiles;
        public bool hasModifiedFiles;

        public bool isAhead;
        public bool isBehind;
        public int commitsAhead;
        public int commitsBehind;

        public bool isInMerge;

        public GitRepo(string path)
        {
            this.workingDirectory = path;

            getPathInformation(workingDirectory);

            if (!this.isRepo)
            {
                return;
            }

            updateIndex();

            Thread tSetBranchName = new Thread(setBranchName);
            Thread tSetHasUntrackedAndNotIgnoredState = new Thread(setHasUntrackedAndNotIgnoredState);
            Thread tSetHasModFiles = new Thread(setHasModifiedFiles);
            Thread tSetHasStagedFiles = new Thread(setHasStagedFiles);
            Thread tSetBranchHasUpstream = new Thread(setBranchHasUpstream);
            Thread tIsInMerge = new Thread(setIsInMerge);

            tSetBranchName.Start();
            tSetHasUntrackedAndNotIgnoredState.Start();
            tSetHasModFiles.Start();
            tSetHasStagedFiles.Start();
            tSetBranchHasUpstream.Start();
            tIsInMerge.Start();

            tSetHasUntrackedAndNotIgnoredState.Join();
            tSetHasModFiles.Join();
            tSetHasStagedFiles.Join();
            tIsInMerge.Join();

            tSetBranchName.Join();
            if (!this.isDetachedHead)
            {
                tSetBranchHasUpstream.Join();
                if (branchHasUpstream)
                {
                    setOriginInformation();
                }
            }
        }

        private void setBranchHasUpstream()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "rev-parse --abbrev-ref --symbolic-full-name @{u}";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.ExitCode == 0)
            {
                this.branchHasUpstream = true;
            }
            else
            {
                this.branchHasUpstream = false;
            }
        }

        private void setIsInMerge()
        {
            string mergeHeadPath = repoRoot + "/.git/MERGE_HEAD";
            mergeHeadPath = mergeHeadPath.Replace("/", "\\\\");
            if (File.Exists(mergeHeadPath))
            {
                this.isInMerge = true;
            } else
            {
                this.isInMerge = false;
            }
        }

        private void setOriginInformation()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = String.Format("rev-list --left-right --count {0}...origin/{0}", this.branchRaw);
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            string stdout = p.StandardOutput.ReadToEnd();
            Regex regex = new Regex(@"(\d+)\s*(\d+)");
            Match match = regex.Match(stdout);

            this.commitsAhead = Int32.Parse(match.Groups[1].Value);
            this.commitsBehind = Int32.Parse(match.Groups[2].Value);
            
            if (commitsAhead > 0)
            {
                this.isAhead = true;
            } else
            {
                this.isAhead = false;
            }

            if (commitsBehind > 0)
            {
                this.isBehind = true;
            } else
            {
                this.isBehind = false;
            }

        }

        private void setHasModifiedFiles()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "diff-files --quiet";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.ExitCode == 0)
            {
                this.hasModifiedFiles = false;
            }
            else
            {
                this.hasModifiedFiles = true;
            }
        }

        private void setHasStagedFiles()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "diff-index --quiet --cached HEAD --";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.ExitCode == 0)
            {
                this.hasStagedFiles = false;
            } else
            {
                this.hasStagedFiles = true;
            }
        }

        private void setHasUntrackedAndNotIgnoredState()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "ls-files --exclude-standard --others";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.StandardOutput.ReadToEnd().Trim() == "")
            {
                this.hasUntrackedFilesNotIgnored = false;
            } else
            {
                this.hasUntrackedFilesNotIgnored = true;
            }
        }

        private void updateIndex()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "update-index -q --refresh";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);
        }

        private void setBranchName()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "symbolic-ref HEAD";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.ExitCode != 0)
            {
                this.isDetachedHead = true;
                setCommitID();
            } else
            {
                string commandOutput = p.StandardOutput.ReadToEnd().Trim();
                this.branchRaw = commandOutput.Replace("refs/heads/", "");
                this.branch = this.branchRaw;
                if (this.branch.Length > 25)
                {
                    this.branch = this.branch.Substring(0, 25) + "..";
                }
            }
        }

        private void setCommitID()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "rev-parse HEAD";
            p.StartInfo.WorkingDirectory = repoRoot;
            p.Start();
            p.WaitForExit(gitProcessTimeout);
            this.commitID = p.StandardOutput.ReadToEnd().Trim().Substring(0, 6);
        }

        private void getPathInformation(string path)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.Arguments = "rev-parse --show-toplevel";
            p.StartInfo.WorkingDirectory = path;
            p.Start();
            p.WaitForExit(gitProcessTimeout);

            if (p.ExitCode != 0)
            {
                this.isRepo = false;
            } else
            {
                this.isRepo = true;
                this.repoRoot = p.StandardOutput.ReadToEnd().Trim();
            }
        }
    }
}
