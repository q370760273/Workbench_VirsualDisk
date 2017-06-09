using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class CommandParser
    {
        public Command _cmd;
        public void Parse(string cmdInfo)
        {
            cmdInfo = cmdInfo.Trim();
            if (!CheckValid(cmdInfo))
                return;

            if (!CreateCommand(cmdInfo))
                return;

            ExcuteCommand();
        }

        protected bool CheckValid(string cmdInfo)
        {
            if (cmdInfo == "")
                return false;

            return true;
        }

        public void ExcuteCommand()
        {
            _cmd.Excute();
        }

        protected virtual bool CreateCommand(string cmdInfo)
        {
            return false;
        }
    }
}
