using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CdCommand : Command
    {
        private string _path;
        private Component _target;

        public CdCommand(string path)
        {
            _path = path;
        }

        public override void Excute()
        {
            if (_path == "")
                return;

            _path = _path.Replace("\"", "");
            string[] paths = new Regex(@"[\\]+").Split(_path);
            Status status = CheckRoot(_path, paths[0], out _target);
            if (status != Status.Succeed)
            {
                if(status != Status.Error_Path_Not_Found)
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
                    if (_target.parent != null)
                        _target = _target.parent;

                    continue;
                }

                _target = EnterDirectory(_target, paths[i]);
                if (_target == null)
                    return;
            }

            if (_target != null)
            {
                VsDiskMoniter.Instance.ResetCursor(_target);
            }
        }
    }
}
