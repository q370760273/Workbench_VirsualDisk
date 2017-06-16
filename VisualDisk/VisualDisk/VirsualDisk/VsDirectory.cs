using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class VsDirectory : Component
    {
        public VsDirectory(MString name) : base(name)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Add(Component child)
        {
            child.SetParent(this);
            _childs.Add(child);
            RefreshTime();
        }

        public override void Remove(Component child)
        {
            if (_childs.Remove(child))
            {
                RefreshTime();
            }
        }

        public override void Remove()
        {
            if (_parent != null)
                _parent.Remove(this);

            foreach (Component child in _childs)
            {
                child.Dispose();
            }
            
            Dispose();
        }

        public override bool IsDirectory()
        {
            return true;
        }
    }
}
