using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CommandParser
    {
        public ICommand _cmd;
        public void Parse(MString cmdInfo)
        {
            cmdInfo = cmdInfo.Trim();
            if (!CheckValid(cmdInfo))
                return;

            if (!CreateCommand(cmdInfo))
                return;

            ExcuteCommand();
        }

        protected bool CheckValid(MString cmdInfo)
        {
            if (cmdInfo == "")
                return false;

            return true;
        }

        public void ExcuteCommand()
        {
            _cmd.Excute();
        }

        protected virtual bool CreateCommand(MString cmdInfo)
        {
            return false;
        }
    }
}
