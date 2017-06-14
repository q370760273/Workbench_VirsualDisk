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
        private string _exName;
        private byte[] buffers = new byte[0];

        public VsFile(string name, string exName) : base(name)
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
            var arr = new byte[buffers.Length + fs.Length];
            Array.Copy(buffers, arr, buffers.Length);
            fs.Read(arr, buffers.Length, (int)fs.Length);
            buffers = arr;
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
    }
}
