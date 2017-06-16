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
        private MString _path;
        private Component _target;

        public CdCommand(MString path)
        {
            _path = path;
        }

        public override void Excute()
        {
            if (_path == "")
                return;

            MString fileName;
            Status status = CheckPath(ref _path, out _target, out fileName, false);
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
