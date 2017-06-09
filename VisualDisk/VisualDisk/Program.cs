using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandParser _parser = new DiskCmdParser();

            while (true)
            {
                Console.Write(VsDiskMoniter.Instance.Cursor.GetPath() + ">");
                string cmdInfo = Console.ReadLine().Trim();
                _parser.Parse(cmdInfo);
            }
        }
    }
}