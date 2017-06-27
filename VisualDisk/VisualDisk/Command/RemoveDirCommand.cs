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
        private bool _formatS;
        private MString _path;
        private Component _tempTarget;

        public RemoveDirCommand(MString path, bool formatS)
        {
            _path = path;
            _formatS = formatS;
        }
        public override void Excute()
        {
            if (_path == "")
            {
                Logger.Log(Status.Error_Commond_Format);
                return;
            }

            MString fileName;
            Status status = CheckPath(ref _path, out _tempTarget, out fileName, false);
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

                if (_tempTarget.childs.Count > 0)
                {
                    if (!_formatS)
                    {
                        Logger.Log(Status.Dir_Not_Empty);
                        return;
                    }
                    else
                    {
                        if (!Logger.ChooseDialogYN("是否确认"))
                            return;
                    }
                }

                _tempTarget.Remove();
            }
        }
    }
}
