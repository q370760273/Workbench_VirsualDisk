using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class DiskCmdParser : CommandParser
    {
        protected override bool CreateCommand(string cmdInfo)
        {
            if (cmdInfo.StartsWith("cd", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new System.Text.RegularExpressions.Regex("[c|C][d|D]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new CdCommand(newDir);
            }
            else if (cmdInfo.StartsWith("mkdir", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new System.Text.RegularExpressions.Regex("[m|M][k|K][d|D][i|I][r|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new MakeDirCommand(newDir);
            }
            else if (cmdInfo.StartsWith("rmdir", StringComparison.CurrentCultureIgnoreCase))
            {
                string newDir = new System.Text.RegularExpressions.Regex("[r|R][m|M][d|D][i|I][r|R]([\\s]+|(?=[.]|[..]|$|\\\\))").Replace(cmdInfo, "");
                _cmd = new RemoveDirCommand(newDir);
            }
            else
            {
                int length = cmdInfo.IndexOf(" ");
                if (length == -1)
                {
                    length = cmdInfo.Length;
                }
                Console.WriteLine("'" + cmdInfo.Substring(0, length) + "' 不是内部或外面命令，也不是可运行的程序\n或批处理文件。");
                return false;
            }

            return true;
        }
    }
}
