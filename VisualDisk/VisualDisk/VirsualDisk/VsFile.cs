using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class VsFile : Component
    {
        private string _exName;

        public VsFile(string name) : base(name)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            //释放Buffer
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
            return _name + "." + _exName;
        }

        public override string GetPath()
        {
            return base.GetPath() + "." + _exName;
        }
    }
}
