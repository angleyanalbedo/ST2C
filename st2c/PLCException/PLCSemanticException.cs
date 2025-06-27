using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCException
{
    // PLC语义异常类
    public class PLCSemanticException : Exception
    {
        public PLCSemanticException() : base()
        {
        }

        // 带详细描述信息的构造器
        public PLCSemanticException(string message) : base(message)
        {
        }
    }
}
