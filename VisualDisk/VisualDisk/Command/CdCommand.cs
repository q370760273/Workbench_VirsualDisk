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

            if (!CheckRoot(paths[0]))
                return;

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

                if (!EnterDirectory(paths[i]))
                    return;
            }

            if (_target != null)
            {
                VsDiskMoniter.Instance.ResetCursor(_target);
            }
        }

        public bool CheckRoot(string rootName)
        {
            if (rootName == "")
            {
                _target = VsDiskMoniter.Instance.Root;
            }
            else if (rootName == ".")
            {
                _target = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName == "..")
            {
                if (VsDiskMoniter.Instance.Cursor.parent != null)
                    _target = VsDiskMoniter.Instance.Cursor.parent;
                else
                    _target = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName.Last() == ':')
            {
                if (rootName.ToLower() != "v:")
                {
                    Console.WriteLine("系统找不到指定的驱动器。");
                    return false;
                }
                else
                {
                    _target = VsDiskMoniter.Instance.Root;
                }
            }
            else
            {
                _target = VsDiskMoniter.Instance.Cursor;

                if (!EnterDirectory(rootName))
                    return false;
            }

            return true;
        }

        public bool EnterDirectory(string name)
        {
            Component child;
            if ((child = _target.GetChild(name)) != null)
            {
                _target = child;
                return true;
            }
            else
            {
                _target = null;
                return false;
            }
        }
    }
}
