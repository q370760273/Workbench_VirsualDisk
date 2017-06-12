using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class DirCommand : Command
    {
        private bool _formatAD, _formatS;
        private string _path;
        private Component _target;

        public DirCommand(string path, bool formatAD, bool formatS)
        {
            _path = path;
            _formatAD = formatAD;
            _formatS = formatS;
        }

        public override void Excute()
        {
            if (_path == "")
            {
                _target = VsDiskMoniter.Instance.Cursor;
            }
            else
            {
                _path = _path.Replace("\"", "");
                string[] paths = new Regex(@"[\\]+").Split(_path);
                Status status = CheckRoot(_path, paths[0], out _target);
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
                        if (_target.parent != null)
                            _target = _target.parent;

                        continue;
                    }

                    _target = EnterDirectory(_target, paths[i]);
                    if (_target == null)
                    {
                        Logger.Log(Status.Error_Path);
                        return;
                    }
                }
            }

            if (_target != null)
            {
                if (_target.IsDirectory())
                {
                    ShowDirectoryInfos(_target);
                }
                else
                {
                    Console.WriteLine(" " + _target.parent.GetPath() + " 的目录\n");
                    Console.WriteLine(_target.GetChangeTime() + "                   " + _target.GetName());
                }
            }
        }

        public void ShowDirectoryInfos(Component target)
        {
            ShowDirectoryTitle(target);

            if (target.parent != null)
            {
                Console.WriteLine(target.GetChangeTime() + "    <DIR>          .");
                Console.WriteLine(target.parent.GetChangeTime() + "    <DIR>          ..");
            }

            foreach (Component child in target.childs)
            {
                if (child.IsDirectory())
                {
                    ShowDirectory(child);
                }
                else
                {
                    if (_formatAD)
                        continue;

                    ShowFile(child);
                }
            }

            Console.WriteLine();

            if (_formatS)
            {
                foreach (Component child in target.childs)
                {
                    if (child.IsDirectory())
                    {
                        ShowDirectoryInfos(child);
                    }
                }
            }
        }

        public void ShowDirectoryTitle(Component dir)
        {
            Console.WriteLine(" " + dir.GetPath() + " 的目录\n");
        }

        public void ShowDirectory(Component dir)
        {
            Console.WriteLine(dir.GetChangeTime() + "    <DIR>          " + dir.name);
        }

        public void ShowFile(Component file)
        {
            Console.WriteLine(file.GetChangeTime() + "                   " + file.GetName());
        }
    }
}
