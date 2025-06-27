using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope
{
    // 生成唯一ID标记符号、符号表、作用域对象
    public sealed class IDGenerator
    {
        private IDGenerator() { }

        private static readonly IDGenerator idGenerator = new IDGenerator();

        public static IDGenerator Instance => idGenerator;

        // 内置类型id
        public const int SINTID = 0;
        public const int INTID = 1;
        public const int DINTID = 2;
        public const int LINTID = 3;
        public const int SSTRING = 4;
        public const int BOOL = 5;
        public const int REAL = 6;
        public const int TIME = 7;
        public const int BITSTR = 8;

        private const int count = 100;
        private static int symbolId = count;
        private static int typeId = count;
        private static int tableId = 0;
        private static int scopeId = 0;
        private static int tempVarId = 0;

        public int NewSymbolId() => symbolId++;
        public int NewTableId() => tableId++;
        public int NewScopeId() => scopeId++;
        public int NewTypeId() => typeId++;
        public int NewTempId() => tempVarId++;

        internal static IDGenerator GetIDGenerator()
        {
            return Instance;
        }
    }
}
