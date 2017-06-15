using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CompareCommand : Command
    {
        private string[] _paths;

        public CompareCommand(string[] paths)
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

            string sourcePath = _paths[0].Replace("\"", "").Replace("\\\\", "\\");
            if (!File.Exists(sourcePath))
            {
                Logger.Log(Status.Error_Path_Not_Found);
                return;
            }

            string destPath = _paths[1].Replace("\"", "").Replace("\\\\", "\\");
            Component destDir;
            string lastDestPath;
            Status status = CheckPath(destPath, out destDir, out lastDestPath, false);
            if (status != Status.Succeed)
            {
                Logger.Log(status);
                return;
            }
            if (lastDestPath == "*")
            {
                Logger.Log(Status.Error_Path_Format);
            }

            VsFile destFile = destDir.GetFile(lastDestPath);

            if (destFile == null)
                Logger.Log(Status.Error_Path_Not_Found);


            Console.WriteLine("正在比较文件 " + sourcePath + " 和 " + destPath);
            byte[] sourceBuffer = FileUtils.GetFileBuffer(sourcePath);
            byte[] destBuffer = destFile.Buffer;

            int differentIndex = -1;
            int index = 0;
            while (index < sourceBuffer.Length && index < destBuffer.Length)
            {
                if(sourceBuffer[index] != destBuffer[index])
                {
                    differentIndex = index;
                    break;
                }
                index++;
            }

            if (sourceBuffer.Length == index && destBuffer.Length == index)
            {
                Console.WriteLine("内容比较一致。");
            }
            else
            {
                Console.WriteLine("***** " + sourcePath);
                OutputFileContent(sourceBuffer, differentIndex);
                Console.WriteLine("***** " + destPath);
                OutputFileContent(destBuffer, differentIndex);
            }
        }

        public void OutputFileContent(byte[] buffer, int rightIndex)
        {
            int leftIndex = 0;

            if (rightIndex >= buffer.Length)
                rightIndex = buffer.Length - 1;

            if (rightIndex < 0)
            {
                Console.WriteLine();
                return;
            }
            else if (rightIndex > 15)
            {
                leftIndex = rightIndex - 15;
            }

            byte[] outputArray = new byte[rightIndex - leftIndex + 1];
            Array.Copy(buffer, leftIndex, outputArray, 0, outputArray.Length);
            if (FileUtils.CheckIsTextFile(buffer))
            {
                Console.WriteLine(Encoding.UTF8.GetString(outputArray));
            }
            else
            {
                string outputStr = "";
                for (int i = leftIndex; i <= rightIndex; i++)
                {
                    outputStr += buffer[i].ToString("X");
                }
                Console.WriteLine(outputStr);
            }
        }
    }
}
