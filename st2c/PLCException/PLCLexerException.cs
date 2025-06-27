using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCException
{
    // PLC词法分析异常类继承自System.Exception
    public class PLCLexerException : Exception
    {
        public PLCLexerException() : base()
        {
        }

        // 带详细描述信息的构造器
        public PLCLexerException(string message) : base(message)
        {
        }
    }
}
