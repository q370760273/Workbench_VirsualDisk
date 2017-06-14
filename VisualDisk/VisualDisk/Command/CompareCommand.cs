using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CompareCommand : Command
    {
        private string[] _paths;

        public CompareCommand(string[] paths)
        {
            _paths = paths;
        }

        public override void Excute()
        {
            if (_paths.Length < 2)
            {
                Logger.Log(Status.Error_Commond_Format);
                return;
            }

            string sourcePath = _paths[0].Replace("\"", "").Replace("\\\\", "\\");
            if (!File.Exists(sourcePath))
            {
                Logger.Log(Status.Error_Path_Not_Found);
                return;
            }

            Component destDir;
            string lastDestPath;
            Status status = CheckPath(_paths[1], out destDir, out lastDestPath, false);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
                return;
            }
            if (lastDestPath == "*")
            {
                Logger.Log(Status.Error_Path_Format);
            }

            VsFile destFile = destDir.GetFile(lastDestPath);

            if (destFile == null)
                Logger.Log(Status.Error_Path_Not_Found);

            
        }
    }
}
