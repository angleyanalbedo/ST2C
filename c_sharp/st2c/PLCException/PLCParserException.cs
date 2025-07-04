using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCException
{
    public class PLCPaserException : Exception
    {
        public PLCPaserException() : base()
        {
        }

        // 带详细描述信息的构造器
        public PLCPaserException(string message) : base(message)
        {
        }
    }
}
