﻿using System;
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
        private MString _path;
        private Component _target;

        public DirCommand(MString path, bool formatAD, bool formatS)
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
                MString fileName;
                Status status = CheckPath(ref _path, out _target, out fileName, false);
                if (status != Status.Succeed)
                {
                    Logger.Log(status);
                    return;
                }
                else if (fileName != "*")
                {
                    var file = _target.GetFile(fileName);
                    if (file == null)
                    {
                        Logger.Log(Status.Error_Path_Not_Found);
                        return;
                    }

                    _target = file;
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
                    Console.WriteLine(_target.GetChangeTime() + (_target as VsFile).GetFileSizeString() + _target.GetName());
                }
            }
        }

        public void ShowDirectoryInfos(Component target)
        {
            Console.WriteLine();
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
            Console.WriteLine(file.GetChangeTime() + (file as VsFile).GetFileSizeString() + file.GetName());
        }
    }
}
