using st2c.PLCSymbolAndScope;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor
{
    /**
    * 为新类型新建1——15维对应的数组类型
    * */
    public class GenerateArrayTypes
    {
        public const int MAX_DIMENSION = 15;

        public void Generate(PLCTypeDeclSymbol source)
        {
            List<PLCArrayDeclSymbol> arrays = new List<PLCArrayDeclSymbol>();
            PLCTotalSymbolTable.ArraySymbolMap[source.SymbolId] = arrays;

            for (int i = 1; i <= MAX_DIMENSION; i++)
            {
                // 创建新的数组类符号
                PLCArrayDeclSymbol arrayDeclSymbol = new PLCArrayDeclSymbol();
                // 设置类型id，符号id
                arrayDeclSymbol.TypeId = IDGenerator.GetIDGenerator().NewTypeId();
                arrayDeclSymbol.SymbolId = IDGenerator.GetIDGenerator().NewSymbolId();
                // 设置内部元素变量
                arrayDeclSymbol.ElementTypeId = source.TypeId;
                // 设置维度
                arrayDeclSymbol.Dimension = i;
                // 设置名称 _ARRAY_{ElementTypeId}_{Dimension}
                arrayDeclSymbol.Name = $"_ARRAY_{source.TypeId}_{i}";

                // 加入符号表
                PLCTotalSymbolTable.TotalSymbolMap[arrayDeclSymbol.SymbolId] = arrayDeclSymbol;
                PLCTotalSymbolTable.TotalTypeMap[arrayDeclSymbol.TypeId] = arrayDeclSymbol;
                arrays.Add(arrayDeclSymbol);
            }
        }
    }

}
