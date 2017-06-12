using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public enum Status
    {
        Succeed,
        Error_Command,
        Error_Format,
        Error_Path,
        Error_DiskPath,
        Error_IO,
    }
    public class Logger
    {
        public static void Log(Status status)
        {
            switch (status)
            {
                case Status.Error_Command:
                    Console.WriteLine("命令语法不正确。");
                    break;
                case Status.Error_Format:
                    Console.WriteLine("文件名、目录名或券标语法不正确。");
                    break;
                case Status.Error_Path:
                    Console.WriteLine("系统找不到指定的路径。");
                    break;
                case Status.Error_DiskPath:
                    Console.WriteLine("系统找不到指定的驱动器。");
                    break;
                case Status.Error_IO:
                    Console.WriteLine("另一个程序正在使用此文件，进程无法访问。");
                    break;
            }
        }
    }
}
