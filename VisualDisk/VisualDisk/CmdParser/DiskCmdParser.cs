using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class DiskCmdParser : CommandParser
    {
        protected override bool CreateCommand(string cmdInfo)
        {
            if (cmdInfo.StartsWith("cd", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[c|C][d|D]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new CdCommand(newDir);
            }
            else if (cmdInfo.StartsWith("mkdir", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[m|M][k|K][d|D][i|I][r|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new MakeDirCommand(newDir);
            }
            else if (cmdInfo.StartsWith("rmdir", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[r|R][m|M][d|D][i|I][r|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new RemoveDirCommand(newDir);
            }
            else if (cmdInfo.StartsWith("dir", StringComparison.CurrentCultureIgnoreCase))
            {
                Regex rgxAD = new Regex(@"[\s]*[/]ad");
                Regex rgxS = new Regex(@"[\s]*[/]s");
                bool formatAD, formatS;
                if (formatAD = rgxAD.IsMatch(cmdInfo))
                {
                    cmdInfo = rgxAD.Replace(cmdInfo, "");
                }
                if (formatS = rgxS.IsMatch(cmdInfo))
                {
                    cmdInfo = rgxS.Replace(cmdInfo, "");
                }
                string newDir = new Regex("[d|D][i|I][r|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new DirCommand(newDir, formatAD, formatS);
            }
            else if (cmdInfo.StartsWith("copy", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[c|C][o|O][p|P][y|Y]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                string[] paths = new Regex(@"[\s]+").Split(newDir);
                _cmd = new CopyCommand(paths);
            }
            else if (cmdInfo.StartsWith("del", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[d|D][e|E][l|L]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                string[] paths = new Regex(@"[\s]+").Split(newDir);
                _cmd = new DeleteFileCommand(paths);
            }
            else if (cmdInfo.StartsWith("compare", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new Regex("[c|C][o|O][m|M][p|P][a|A][r|R][e|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                string[] paths = new Regex(@"[\s]+").Split(newDir);
                _cmd = new CompareCommand(paths);
            }
            else
            {
                int length = cmdInfo.IndexOf(" ");
                if (length == -1)
                {
                    length = cmdInfo.Length;
                }
                Logger.Log(Status.Error_Commond, cmdInfo.Substring(0, length));
                return false;
            }

            return true;
        }
    }
}
