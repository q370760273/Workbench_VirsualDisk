using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class VsDirectory : Component
    {
        public VsDirectory(string name) : base(name)
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
        }

        public override void Remove()
        {
            if (_parent != null)
                _parent.childs.Remove(this);

            foreach (Component child in _childs)
            {
                child.Dispose();
            }
            
            Dispose();
        }
    }
}
