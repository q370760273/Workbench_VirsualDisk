using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class DeleteFileCommand : Command
    {
        private string[] _paths;

        public DeleteFileCommand(string[] paths)
        {
            _paths = paths;
        }

        public override void Excute()
        {
            foreach (string path in _paths)
            {
                Component target;
                string fileName;
                Status status = CheckPath(path, out target, out fileName, true);
                if (status != Status.Succeed)
                {
                    Logger.Log(status);
                    return;
                }

                status = ExcuteByPattern(target, fileName);
                if (status != Status.Succeed)
                {
                    Logger.Log(status);
                    return;
                }
            }
        }

        private Status ExcuteByPattern(Component target, string pattern)
        {
            if (pattern == "*") //复制到上一个目录下
            {
                target.Remove();
            }
            else if (pattern.StartsWith("*.")) //把复制的名字的后缀修改掉
            {
                string matchStr = pattern.Substring(2);

                if (nameRegex.IsMatch(matchStr))
                    return Status.Error_Path_Format;

                for (int i = 0; i < target.childs.Count; i++)
                {
                    Component child = target.childs[i];
                    if (child.IsDirectory())
                        continue;

                    if (child.GetName().EndsWith(matchStr))
                    {
                        child.Remove();
                        i--;
                    }
                }
            }
            else  //把复制的名字和后缀全改掉
            {
                if (nameRegex.IsMatch(pattern))
                    return Status.Error_Path_Format;

                VsFile file = target.GetFile(pattern);
                if (file != null)
                {
                    file.Remove();
                }
            }

            return Status.Succeed;
        }
    }
}
