using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCException
{
    // PLC异常类继承RuntimeException
    public class PLCException : Exception
    {
        public PLCException() : base()
        {
        }

        // 带详细描述信息的构造器
        public PLCException(string message) : base(message)
        {
        }
    }
}
