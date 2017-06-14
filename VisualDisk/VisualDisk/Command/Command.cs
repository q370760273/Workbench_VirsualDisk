using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class Command : ICommand
    {
        protected Regex nameRegex = new Regex("[/?*:\"<>|]");
        protected Regex fullSpaceNameRegex = new Regex(@"\\[\s]+\\");

        public virtual void Excute()
        {
            throw new NotImplementedException();
        }

        public virtual Status CheckPath(string path, out Component targetDir, out string fileName, bool usePattern)
        {
            targetDir = null;
            fileName = "*";
            path = path.Replace("\"", "").Replace("\\\\", "\\");
            string[] paths = path.Split('\\');

            string rootName = paths[0];
            bool isEnding = paths.Length < 2;
            Status status = CheckRootPath(ref targetDir, ref fileName, rootName, usePattern, isEnding);
            if(status != Status.Succeed || isEnding)
            {
                return status;
            }

            for (int i = 1; i < paths.Length - 1; i++)
            {
                if (paths[i].Trim() == "")
                    return Status.Error_Path_Format;

                if (nameRegex.IsMatch(paths[i]))
                    return Status.Error_Path_Format;

                if (paths[i] == ".")
                {
                    continue;
                }
                else if (paths[i] == "..")
                {
                    if (targetDir.parent != null)
                        targetDir = targetDir.parent;

                    continue;
                }

                targetDir = EnterDirectory(targetDir, paths[i]);
                if (targetDir == null)
                {
                    return Status.Error_Path_Not_Found;
                }
            }


            string endName = paths[paths.Length - 1];
            return CheckEndPath(ref targetDir, ref fileName, endName, usePattern);
        }

        private Status CheckRootPath(ref Component targetDir, ref string fileName, string rootName, bool usePattern, bool isEnding)
        {
            if (rootName == "")
            {
                targetDir = VsDiskMoniter.Instance.Root;
            }
            else if (rootName == ".")
            {
                targetDir = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName == "..")
            {
                if (VsDiskMoniter.Instance.Cursor.parent != null)
                    targetDir = VsDiskMoniter.Instance.Cursor.parent;
                else
                    targetDir = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName.Last() == ':')
            {
                if (rootName.ToLower() != "v:")
                    return Status.Error_Disk_Not_Found;

                targetDir = VsDiskMoniter.Instance.Root;
            }
            else
            {
                targetDir = VsDiskMoniter.Instance.Cursor;

                if (isEnding)
                    return CheckEndPath(ref targetDir, ref fileName, rootName, usePattern);

                if (nameRegex.IsMatch(rootName))
                    return Status.Error_Path_Format;

                VsDirectory dir = EnterDirectory(targetDir, rootName);
                if (dir != null)
                    targetDir = dir;
                else
                    return Status.Error_Path_Not_Found;
            }

            return Status.Succeed;
        }

        private Status CheckEndPath(ref Component targetDir, ref string fileName, string endName, bool usePattern)
        {
            if (endName == "")
            {
                fileName = "*";
                return Status.Succeed;
            }

            fileName = endName;

            if (usePattern)
            {
                if (new Regex("[/:\"<>|]").IsMatch(endName))
                    return Status.Error_Path_Format;

                VsDirectory dir = EnterDirectory(targetDir, endName);
                if (dir != null)
                {
                    targetDir = dir;
                    fileName = "*";
                }
            }
            else
            {
                if (nameRegex.IsMatch(endName))
                    return Status.Error_Path_Format;

                VsDirectory dir = EnterDirectory(targetDir, endName);
                if (dir != null)
                {
                    targetDir = dir;
                    fileName = "*";
                }
            }

            return Status.Succeed;
        }

        protected virtual VsDirectory EnterDirectory(Component source, string name)
        {
            return source.GetDirectory(name);
        }
    }
}
