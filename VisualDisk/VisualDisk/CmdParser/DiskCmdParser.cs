using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class DiskCmdParser : CommandParser
    {
        private const string CD = "cd";
        private const string MKDIR = "mkdir";
        private const string RMDIR = "rmdir";
        private const string DIR = "dir";
        private const string COPY = "copy";
        private const string DEL = "del";
        private const string COMPARE = "compare";

        protected override bool CreateCommand(MString cmdInfo)
        {
            if (cmdInfo.StartsWith(CD, true) && (cmdInfo.Substring(CD.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(CD.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(CD.Length).Trim();
                _cmd = new CdCommand(newDir);
            }
            else if (cmdInfo.StartsWith(MKDIR, true) && (cmdInfo.Substring(MKDIR.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(MKDIR.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(MKDIR.Length).Trim();
                _cmd = new MakeDirCommand(newDir);
            }
            else if (cmdInfo.StartsWith(RMDIR, true) && (cmdInfo.Substring(RMDIR.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(RMDIR.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(RMDIR.Length).Trim();
                _cmd = new RemoveDirCommand(newDir);
            }
            else if (cmdInfo.StartsWith(DIR, true) && (cmdInfo.Substring(DIR.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(DIR.Length).StartsWith(new string[] { " ", ".", "\\", "/" }))))
            {
                MString newDir = cmdInfo.Substring(DIR.Length).Trim();
                MString[] attrs = { "ad", "s" };
                bool[] results = new bool[2];
                if (!CheckAttribute(ref newDir, attrs, results))
                {
                    return false;
                }
                _cmd = new DirCommand(newDir.Trim(), results[0], results[1]);
            }
            else if (cmdInfo.StartsWith(COPY, true) && (cmdInfo.Substring(COPY.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(COPY.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(COPY.Length).Trim();
                MString[] paths = newDir.MultiSplit(' ');
                _cmd = new CopyCommand(paths);
            }
            else if (cmdInfo.StartsWith(DEL, true) && (cmdInfo.Substring(DEL.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(DEL.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(DEL.Length).Trim();
                MString[] paths = newDir.MultiSplit(' ');
                _cmd = new DeleteFileCommand(paths);
            }
            else if (cmdInfo.StartsWith(COMPARE, true) && (cmdInfo.Substring(COMPARE.Length).TrimEnd().Length == 0 || (cmdInfo.Substring(COMPARE.Length).StartsWith(new string[] { " ", ".", "\\" }))))
            {
                MString newDir = cmdInfo.Substring(COMPARE.Length).Trim();
                MString[] paths = newDir.MultiSplit(' ');
                _cmd = new CompareCommand(paths);
            }
            else
            {
                int length = ((string)cmdInfo).IndexOf(" ");
                if (length == -1)
                {
                    length = cmdInfo.Length;
                }
                Logger.Log(Status.Error_Commond, cmdInfo.Substring(0, length));
                return false;
            }

            return true;
        }

        private bool CheckAttribute(ref MString newDir, MString[] attrs, bool[] results)
        {
            for (int i = 0; i < newDir.Length; i++)
            {
                if (newDir[i] == '/')
                {
                    MString str = newDir.Substring(i + 1);
                    int attrEndIndex = str.IndexOf(new string[] { " ", "/" });
                    if (attrEndIndex == -1)
                    {
                        attrEndIndex = str.Length;
                    }
                    MString attr = str.Substring(0, attrEndIndex);
                    int rmStartIdx = 0, rmCount = 0;

                    bool isRightAttr = false;
                    for (int j = 0; j < attrs.Length; j++)
                    {
                        if (attr == attrs[j])
                        {
                            isRightAttr = true;
                            results[j] = true;
                            rmStartIdx = newDir.Substring(0, i).TrimEnd().Length;
                            rmCount = (i - rmStartIdx + attrs[j].Length + 1);
                            break;
                        }
                    }

                    if(!isRightAttr)
                    {
                        Logger.Log(Status.Error_Attribute_Format, attr);
                        return false;
                    }

                    newDir = newDir.Remove(rmStartIdx, rmCount);
                    i = rmStartIdx - 1;
                }
            }

            return true;
        }
    }
}
