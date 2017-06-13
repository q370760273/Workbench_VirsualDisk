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

        public Status CheckRoot(string _path, string rootName, out Component target)
        {
            target = null;

            if (fullSpaceNameRegex.IsMatch(_path))
                return Status.Error_Path_Format;

            if (nameRegex.IsMatch(_path.Substring(rootName.Length)))
                return Status.Error_Path_Format;


            if (rootName == "")
            {
                target = VsDiskMoniter.Instance.Root;
            }
            else if (rootName == ".")
            {
                target = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName == "..")
            {
                if (VsDiskMoniter.Instance.Cursor.parent != null)
                    target = VsDiskMoniter.Instance.Cursor.parent;
                else
                    target = VsDiskMoniter.Instance.Cursor;
            }
            else if (rootName.Last() == ':')
            {
                if (rootName.ToLower() != "v:")
                    return Status.Error_Disk_Not_Found;

                target = VsDiskMoniter.Instance.Root;
            }
            else
            {
                if (nameRegex.IsMatch(rootName))
                    return Status.Error_Path_Format;

                target = EnterDirectory(VsDiskMoniter.Instance.Cursor, rootName);

                if (target == null)
                    return Status.Error_Path_Not_Found;
            }

            return Status.Succeed;
        }

        protected virtual Component EnterDirectory(Component source, string name)
        {
            return source.GetDirectory(name);
        }
    }
}
