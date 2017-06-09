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
        private Component _dirIndex;
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
                    if (_dirIndex.parent != null)
                        _dirIndex = _dirIndex.parent;

                    continue;
                }

                CreateDirectory(paths[i]);
            }
        }

        private bool CheckRoot(string rootName)
        {
            if (rootName == "")
            {
                _dirIndex = VsDiskMoniter.Instance.Root;
            }
            else if (rootName == ".")
            {
                _dirIndex = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName == "..")
            {
                if (VsDiskMoniter.Instance.Cursor.parent != null)
                    _dirIndex = VsDiskMoniter.Instance.Cursor.parent;
                else
                    _dirIndex = VsDiskMoniter.Instance.Cursor;
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
                    _dirIndex = VsDiskMoniter.Instance.Root;
                }
            }
            else
            {
                _dirIndex = VsDiskMoniter.Instance.Cursor;
                if (nameRegex.IsMatch(rootName))
                {
                    Console.WriteLine("文件名、目录名或券标语法不正确。");
                    return false;
                }
                CreateDirectory(rootName);
            }

            if (nameRegex.IsMatch(_path.Substring(rootName.Length)))
            {
                Console.WriteLine("文件名、目录名或券标语法不正确。");
                return false;
            }

            return true;
        }

        private void CreateDirectory(string name)
        {
            Component child;
            if ((child = _dirIndex.GetChild(name)) != null)
            {
                _dirIndex = child;
            }
            else
            {
                Component dir = new VsDirectory(name);
                _dirIndex.Add(dir);
                _dirIndex = dir;
            }
        }
    }
}
