using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class VsDiskMoniter
    {
        private static VsDiskMoniter _instance;
        public static VsDiskMoniter Instance
        {
            get {
                if (_instance == null)
                    _instance = new VsDiskMoniter();

                return _instance;
            }
        }


        private Component _root;
        private Component _curCursor;

        public VsDiskMoniter()
        {
            _curCursor = _root = new VsDirectory("V:");
        }

        public void ResetCursor(Component target)
        {
            _curCursor = target;
        }

        public Component Root
        { get { return _root; } }

        public Component Cursor
        { get { return _curCursor; } }
    }
}
