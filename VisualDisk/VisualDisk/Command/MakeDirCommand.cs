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
        private string _path;
        private Component _targetTemp;
        Regex nameRegex = new Regex("[/?*:\"<>|]");
        Regex fullSpaceNameRegex = new Regex(@"\\[\s]+\\");
        public MakeDirCommand(string path)
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

            string filename;
            Status status = CheckPath(_path, out _targetTemp, out filename, false);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
            }
        }

        protected override VsDirectory EnterDirectory(Component source, string name)
        {
            VsDirectory child = source.GetDirectory(name);

            if (child == null)
            {
                child = new VsDirectory(name);
                source.Add(child);
            }

            return child;
        }
    }
}
