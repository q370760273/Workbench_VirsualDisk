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

        public virtual Status CheckPath(ref MString path, out Component targetDir, out MString fileName, bool usePattern)
        {
            targetDir = null;
            fileName = "*";
            path = path.Replace("\"", "").MultiReplace("\\", "\\");
            MString[] paths = path.MultiSplit('\\');

            MString rootName = paths[0];
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

                status = EnterDirectory(ref targetDir, paths[i]);
                if (status != Status.Succeed)
                {
                    return status;
                }
            }


            string endName = paths[paths.Length - 1];
            return CheckEndPath(ref targetDir, ref fileName, endName, usePattern);
        }

        protected virtual Status CheckRootPath(ref Component targetDir, ref MString fileName, MString rootName, bool usePattern, bool isEnding)
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

                return EnterDirectory(ref targetDir, rootName);
            }

            return Status.Succeed;
        }

        protected virtual Status CheckEndPath(ref Component targetDir, ref MString fileName, MString endName, bool usePattern)
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

                Status status = EnterDirectory(ref targetDir, endName);
                if (status == Status.Succeed)
                {
                    fileName = "*";
                }
            }
            else
            {
                if (nameRegex.IsMatch(endName))
                    return Status.Error_Path_Format;

                Status status = EnterDirectory(ref targetDir, endName);
                if (status == Status.Succeed)
                {
                    fileName = "*";
                }
            }

            return Status.Succeed;
        }

        protected virtual Status EnterDirectory(ref Component source, MString name)
        {
            var dir = source.GetDirectory(name);

            if (dir == null)
            {
                return Status.Error_Path_Not_Found;
            }
            else
            {
                source = dir;
                return Status.Succeed;
            }
        }
    }
}
