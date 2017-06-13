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

            _path = _path.Replace("\"", "");
            string[] paths = new Regex(@"[\\]+").Split(_path);
            Status status = CheckRoot(_path, paths[0], out _targetTemp);
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
                    if (_targetTemp.parent != null)
                        _targetTemp = _targetTemp.parent;

                    continue;
                }

                _targetTemp = EnterDirectory(_targetTemp, paths[i]);
            }
        }

        protected override Component EnterDirectory(Component source, string name)
        {
            Component child = source.GetDirectory(name);

            if (child == null)
            {
                child = new VsDirectory(name);
                source.Add(child);
            }

            return child;
        }
    }
}
