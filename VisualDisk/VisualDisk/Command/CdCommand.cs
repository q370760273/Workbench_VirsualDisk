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

            string fileName;
            Status status = CheckPath(_path, out _target, out fileName, false);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
                return;
            }
            else if (fileName != "*")
            {
                Logger.Log(Status.Error_Path_Not_Found);
                return;
            }

            if (_target != null)
            {
                VsDiskMoniter.Instance.ResetCursor(_target);
            }
        }
    }
}
