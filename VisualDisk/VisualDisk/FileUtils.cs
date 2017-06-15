using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class FileUtils
    {
        public static byte[] GetFileBuffer(string path)
        {
            FileStream fs = File.OpenRead(path);
            var buffers = new byte[fs.Length];
            fs.Read(buffers, 0, (int)fs.Length);
            fs.Close();
            return buffers;
        }

        public static bool CheckIsTextFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            bool isTextFile = true;
            try
            {
                int i = 0;
                int length = (int)fs.Length;
                byte data;
                while (i < length && isTextFile)
                {
                    data = (byte)fs.ReadByte();
                    isTextFile = (data != 0);
                    i++;
                }
                return isTextFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        public static bool CheckIsTextFile(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                    return false;
            }

            return true;
        }
    }
}
