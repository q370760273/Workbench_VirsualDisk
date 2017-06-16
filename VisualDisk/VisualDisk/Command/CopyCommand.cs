using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CopyCommand : Command
    {
        private MString[] _paths;

        public CopyCommand(MString[] paths)
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

            FileInfo[] sourceFileInfos = GetSourceInfo();

            Component destDir;
            MString lastDestPath;
            Status status = CheckPath(ref _paths[1], out destDir, out lastDestPath, true);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
                return;
            }

            //处理最后一个

            if (lastDestPath == "*") //复制到上一个目录下
            {
                CopyAll(sourceFileInfos, destDir);
            }
            else if (lastDestPath.StartsWith("*.")) //把复制的名字的后缀修改掉
            {
                if (nameRegex.IsMatch(lastDestPath.Substring(2)))
                {
                    Logger.Log(Status.Error_Path_Format);
                    return;
                }
                CopyAllWithChangeExName(sourceFileInfos, destDir, lastDestPath);
            }
            else  //把复制的名字和后缀全改掉
            {
                if (nameRegex.IsMatch(lastDestPath))
                {
                    Logger.Log(Status.Error_Path_Format);
                    return;
                }
                CopyWithChangeNameAndExName(sourceFileInfos, destDir, lastDestPath);
            }
        }

        public FileInfo[] GetSourceInfo()
        {
            string sourcePath = _paths[0].Replace("\"", "");
            string[] sourcePaths = new Regex(@"[\\]+").Split(sourcePath);
            sourcePaths[sourcePaths.Length - 1] = sourcePaths[sourcePaths.Length - 1].Replace("?", "*");
            FileInfo[] sourceFileInfos = null;

            if (IsRealDisk(sourcePaths[0]))
            {
                var builder = new StringBuilder(sourcePaths[0]);
                string searchPattern = "";
                if (sourcePaths[sourcePaths.Length - 1] == "*" || sourcePaths[sourcePaths.Length - 1].Contains("."))
                {
                    for (int i = 1; i < sourcePaths.Length - 1; i++)
                    {
                        builder.Append("\\");
                        builder.Append(sourcePaths[i]);
                    }
                    searchPattern = sourcePaths[sourcePaths.Length - 1];
                }
                else
                {
                    for (int i = 1; i < sourcePaths.Length - 1; i++)
                    {
                        builder.Append("\\");
                        builder.Append(sourcePaths[i]);
                    }
                    if (Directory.Exists(builder.ToString() + "\\" + sourcePaths[sourcePaths.Length - 1]))
                    {
                        builder.Append("\\" + sourcePaths[sourcePaths.Length - 1]);
                        searchPattern = "*";
                    }
                    else
                    {
                        searchPattern = sourcePaths[sourcePaths.Length - 1];
                    }
                }

                //拷贝整个目录
                if (!Directory.Exists(builder.ToString()))
                    return null;

                DirectoryInfo dirInfo = Directory.CreateDirectory(builder.ToString());
                sourceFileInfos = dirInfo.GetFiles(searchPattern);
            }

            return sourceFileInfos;
        }

        public void CopyAll(FileInfo[] sourceFileInfos, Component targetDir)
        {
            string result = "";

            foreach (FileInfo fi in sourceFileInfos)
            {
                Console.WriteLine(fi.FullName);
                Component file = targetDir.GetChild(fi.Name);
                if (file != null)
                {
                    if (file.IsDirectory())
                    {
                        Console.WriteLine("拒绝访问。");
                        continue;
                    }
                    else
                    {
                        if (Logger.ChooseDialog(ref result, "覆盖 {0} 吗?", fi.Name))
                            file.Remove();
                        else
                            continue;
                    }
                }

                file = new VsFile(fi.Name.Remove(fi.Name.Length - fi.Extension.Length, fi.Extension.Length), fi.Extension);
                (file as VsFile).Read(fi.FullName);
                targetDir.Add(file);
            }
        }

        public void CopyAllWithChangeExName(FileInfo[] sourceFileInfos, Component targetDir, string lastDestPath)
        {
            string result = "";
            string exName = lastDestPath.Substring(1);

            if (exName == ".")
                exName = "";

            foreach (FileInfo fi in sourceFileInfos)
            {
                Console.WriteLine(fi.FullName);
                string name = fi.Name.Remove(fi.Name.Length - fi.Extension.Length, fi.Extension.Length) + exName;
                Component file = targetDir.GetChild(name);
                if (file != null)
                {
                    if (file.IsDirectory())
                    {
                        Console.WriteLine("拒绝访问。");
                        continue;
                    }
                    else
                    {
                        if (Logger.ChooseDialog(ref result, "覆盖 {0} 吗?", fi.Name))
                            file.Remove();
                        else
                            continue;
                    }
                }

                string newExName = "";
                if (name.LastIndexOf('.') != -1)
                {
                    newExName = name.Substring(name.LastIndexOf('.'), name.Length - name.LastIndexOf('.'));
                }
                string newName = name.Remove(name.Length - newExName.Length, newExName.Length);
                file = new VsFile(newName, newExName);
                (file as VsFile).Read(fi.FullName);
                targetDir.Add(file);
            }
        }

        public void CopyWithChangeNameAndExName(FileInfo[] sourceFileInfos, Component targetDir, string lastDestPath)
        {
            string result = "";
            string exName = "";
            if (lastDestPath.LastIndexOf('.') != -1)
            {
                exName = lastDestPath.Substring(lastDestPath.LastIndexOf('.'), lastDestPath.Length - lastDestPath.LastIndexOf('.'));
            }
            string name = lastDestPath.Remove(lastDestPath.Length - exName.Length, exName.Length);
            Component file = targetDir.GetChild(lastDestPath);
            if (file != null)
            {
                if (file.IsDirectory())
                {
                    Console.WriteLine("拒绝访问。");
                    return;
                }
                else
                {
                    if (Logger.ChooseDialog(ref result, "覆盖 {0} 吗?", name))
                        file.Remove();
                    else
                        return;
                }
            }
            
            file = new VsFile(name, exName);
            foreach (FileInfo fi in sourceFileInfos)
            {
                Console.WriteLine(fi.FullName);
                (file as VsFile).Read(fi.FullName);
            }
            targetDir.Add(file);

        }

        private bool IsRealDisk(string rootName)
        {
            if (rootName.Last() == ':')
            {
                try
                {
                    if (new DriveInfo(rootName).DriveType == DriveType.Fixed)
                        return true;
                }
                catch (Exception) { }
            }

            return false;
        }
    }
}
