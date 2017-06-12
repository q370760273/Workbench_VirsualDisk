using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class RemoveDirCommand : Command
    {
        private string _path;
        private Component _tempTarget;
        Regex nameRegex = new Regex("[/?*:\"<>|]");
        Regex fullSpaceNameRegex = new Regex(@"\\[\s]+\\");
        public RemoveDirCommand(string path)
        {
            _path = path;
        }
        public override void Excute()
        {
            if (_path == "")
            {
                Logger.Log(Status.Error_Commond_Format);
                return;
            }

            _path = _path.Replace("\"", "");
            string[] paths = new Regex(@"[\\]+").Split(_path);
            Status status = CheckRoot(_path, paths[0], out _tempTarget);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
                return;
            }

            for (int i = 1; i < paths.Length; i++)
            {
                if (paths[i] == "")
                {
                    continue;
                }
                else if (paths[i] == ".")
                {
                    continue;
                }
                else if (paths[i] == "..")
                {
                    if (_tempTarget.parent != null)
                        _tempTarget = _tempTarget.parent;

                    continue;
                }

                _tempTarget = EnterDirectory(_tempTarget, paths[i]);
                if (_tempTarget == null)
                {
                    Logger.Log(Status.Error_Path_Not_Found);
                    return;
                }
            }

            if (_tempTarget != null)
            {
                Component cursorTemp = VsDiskMoniter.Instance.Cursor;
                while (cursorTemp != null)
                {
                    if ((cursorTemp != _tempTarget))
                    {
                        cursorTemp = cursorTemp.parent;
                    }
                    else
                    {
                        Logger.Log(Status.Error_IO);
                        return;
                    }
                }

                _tempTarget.Remove();
            }
        }
    }
}
