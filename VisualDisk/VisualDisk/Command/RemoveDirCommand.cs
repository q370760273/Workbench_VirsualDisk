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
        private string _path;
        private Component _target;
        Regex nameRegex = new Regex("[/?*:\"<>|]");
        Regex fullSpaceNameRegex = new Regex(@"\\[\s]+\\");
        public RemoveDirCommand(string path)
        {
            _path = path;
        }
        public override void Excute()
        {
            if (_path == "")
            {
                Console.WriteLine("命令语法不正确。");
                return;
            }
            if (fullSpaceNameRegex.IsMatch(_path))
            {
                Console.WriteLine("文件名、目录名或券标语法不正确。");
                return;
            }

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

                bool succeed = EnterDirectory(paths[i]);
                if (!succeed)
                {
                    Console.WriteLine("系统找不到指定的路径。");
                    return;
                }
            }

            if (_target != null)
            {
                Component cursorTemp = VsDiskMoniter.Instance.Cursor;
                while (cursorTemp != null)
                {
                    if ((cursorTemp != _target))
                    {
                        cursorTemp = cursorTemp.parent;
                    }
                    else
                    {
                        Console.WriteLine("另一个程序正在使用此文件，进程无法访问。");
                        return;
                    }
                }

                _target.Remove();
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
                if (nameRegex.IsMatch(rootName))
                {
                    Console.WriteLine("文件名、目录名或券标语法不正确。");
                    return false;
                }
                bool succeed = EnterDirectory(rootName);
                if (!succeed)
                {
                    Console.WriteLine("系统找不到指定的路径。");
                    return false;
                }
            }

            if (nameRegex.IsMatch(_path.Substring(rootName.Length)))
            {
                Console.WriteLine("文件名、目录名或券标语法不正确。");
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
