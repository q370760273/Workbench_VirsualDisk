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
        private MString[] _paths;

        public DeleteFileCommand(MString[] paths)
        {
            _paths = paths;
        }

        public override void Excute()
        {
            for (int i = 0; i <_paths.Length; i++)
            {
                Component target;
                MString fileName;
                Status status = CheckPath(ref _paths[i], out target, out fileName, true);
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
            if (pattern == "*") //删除目录下所有文件
            {
                for (int i = 0; i < target.childs.Count; i++)
                {
                    Component child = target.childs[i];
                    if (child.IsDirectory())
                        continue;

                    child.Remove();
                }
            }
            else if (pattern.StartsWith("*.")) //删除指定后缀名的文件
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
            else  //删除单个文件
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
