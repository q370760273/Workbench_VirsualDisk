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
        Error_Commond,
        Error_Commond_Format,
        Error_Path_Format,
        Error_Path_Not_Found,
        Error_Disk_Not_Found,
        Error_IO,
        Error_Write_Disable
    }
    public class Logger
    {
        public static void Log(Status status, params string[] datas)
        {
            switch (status)
            {
                case Status.Error_Commond:
                    Console.WriteLine("'{0}' 不是内部或外面命令，也不是可运行的程序\n或批处理文件。", datas[0]);
                    break;
                case Status.Error_Commond_Format:
                    Console.WriteLine("命令语法不正确。");
                    break;
                case Status.Error_Path_Format:
                    Console.WriteLine("文件名、目录名或券标语法不正确。");
                    break;
                case Status.Error_Path_Not_Found:
                    Console.WriteLine("系统找不到指定的路径。");
                    break;
                case Status.Error_Disk_Not_Found:
                    Console.WriteLine("系统找不到指定的驱动器。");
                    break;
                case Status.Error_IO:
                    Console.WriteLine("另一个程序正在使用此文件，进程无法访问。");
                    break;
                case Status.Error_Write_Disable:
                    Console.WriteLine("只允许向V盘写入数据。");
                    break;
            }
        }

        public static bool ChooseDialog(ref string result, string content, params string[] datas)
        {
            if (!result.Equals("a", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Write(content + " (Yes/No/All):", datas);
                result = Console.ReadLine();
                if (!result.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    return false;
            }

            return true;
        }
    }
}
