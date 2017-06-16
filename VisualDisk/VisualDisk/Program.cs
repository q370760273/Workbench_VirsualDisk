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
                Console.WriteLine();
            }
            //Test2();
        }

        public static void Test2()
        {
            string a = "1234567890yyyy1234567890yyyy";
            a = a.Replace("yy", "b");
            MString str = new MString("ywyw1234567890yyyyyw1234567890yyyyyw");
            MString str11 = str.MultiReplace("yw", "b");
            MString str0 = str.MultiReplace("y", "y");
            MString str1 = str.Substring(4);
            MString str2 = str.Substring(4, 10);
            MString str3 = str.Trim();
            MString str4 = str.Trim(new char[] { 'y', 'w'});
            MString str5 = str.TrimStart(new char[] { 'y', 'w' });
            MString str6 = str.TrimEnd(new char[] { 'y', 'w' });
            bool b0 = str.StartsWith("");
            bool b1 = str.StartsWith("yw");
            bool b2 = str.StartsWith("YW");
            bool b3 = str.StartsWith("YW", true);
            bool b4 = str.StartsWith(new string[]{ "y", "w"});
            bool b5 = str.StartsWith(new string[] { "Y", "W" });
            bool b6 = str.StartsWith(new string[] { "Y", "W" }, true);
            int idx1 = str.IndexOf("y");
            int idx2 = str.IndexOf("w");
            int idx3 = str.IndexOf("Y");
            int idx4 = str.IndexOf("W");
            int idx5 = str.IndexOf("W", true);
            int idx6 = str.IndexOf("W", 10, true);
            int idx7 = str.IndexOf("W", 10, 2, true);
            int idx8 = str.IndexOf(new string[] { "W", "Y" });
            int idx9 = str.IndexOf(new string[] { "W", "Y" }, true);
            int idx0 = str.IndexOf(new string[] { "W", "Y" }, 10, true);
            MString mstr1 = str.Remove(4);
            MString mstr2 = str.Remove(4, 10);
            string[] strs0 = ((string)str).Split('y');
            MString[] strs1 = str.Split('y');
            string[] strs2 = ((string)str).Split('w');
            MString[] strs3 = str.Split('w');
            MString[] strs4 = str.MultiSplit('y');
        }
        public static void Test()
        {
            MString str1 = null;
            MString str2 = "def";
            MString str3 = "def";
            Console.WriteLine(str1 + str2);
            if (str2 == null)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");


            if (str1 == null)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");


            if (str1 == str3)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");


            if (str1 == "")
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");


            if (str3 == "")
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");


            if (str3 == str2)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");
            str3 = str1 + str2;

            if (str3 == str2)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");

            str3.Replace("e", "");

            if (str3 == str2)
                Console.WriteLine("yes");
            else
                Console.WriteLine("no");

            Console.ReadLine();
        }
    }
}