using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class VsFile : Component
    {
        //private const int BUFFER_MAX_LEN = 65535;
        private MString _exName;
        private byte[] _buffer = new byte[0];

        public VsFile(MString name, MString exName) : base(name)
        {
            _exName = exName;
        }

        public override void Dispose()
        {
            base.Dispose();
            //释放Buffer
        }

        public void Read(string path)
        {
            FileStream fs = File.OpenRead(path);
            var arr = new byte[_buffer.Length + fs.Length];
            Array.Copy(_buffer, arr, _buffer.Length);
            fs.Read(arr, _buffer.Length, (int)fs.Length);
            _buffer = arr;
            fs.Close();
        }

        public void Write()
        {

        }

        public override void Remove()
        {
            if (_parent != null)
                _parent.childs.Remove(this);

            Dispose();
        }

        public override bool IsDirectory()
        {
            return false;
        }

        public override string GetName()
        {
            return _name + _exName;
        }

        public string GetFileSizeString()
        {
            return GetCommaNumber(_buffer.Length).PadLeft(18) + " ";
        }

        public static string GetCommaNumber(int number)
        {
            string numStr = number.ToString();
            int temp3 = 0;
            int endIndex = (number >= 0 ? 0 : 1);
            for (int i = numStr.Length - 1; i > endIndex; i--)
            {
                if (++temp3 == 3)
                {
                    temp3 = 0;
                    numStr = numStr.Insert(i, ",");
                }
            }
            return numStr;
        }

        public byte[] Buffer
        { get { return _buffer; } }
    }
}
