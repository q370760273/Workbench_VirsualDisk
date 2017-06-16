using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class MakeDirCommand : Command
    {
        private MString _path;
        private Component _targetTemp;
        public MakeDirCommand(MString path)
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

            MString filename;
            Status status = CheckPath(ref _path, out _targetTemp, out filename, false);
            if (status != Status.Succeed)
            {
                if(status == Status.Error_Path_Already_Exist)
                    Logger.Log(status, _path);
                else
                    Logger.Log(status);
            }
        }

        protected override Status CheckEndPath(ref Component targetDir, ref MString fileName, MString endName, bool usePattern)
        {
            if (targetDir.GetChild(endName) != null)
                return Status.Error_Path_Already_Exist;

            return base.CheckEndPath(ref targetDir, ref fileName, endName, usePattern);
        }

        protected override Status EnterDirectory(ref Component source, MString name)
        {
            if(source.GetFile(name) != null)
                return Status.Error_Path_Already_Exist;

            VsDirectory child = source.GetDirectory(name);

            if (child == null)
            {
                child = new VsDirectory(name);
                source.Add(child);
            }

            source = child;
            return Status.Succeed;
        }
    }
}
