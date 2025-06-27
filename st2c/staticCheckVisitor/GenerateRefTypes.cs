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
    public class GenerateRefTypes
    {
        public void Generate(PLCTypeDeclSymbol source)
        {
            // 创建新的引用类符号
            PLCRefDeclSymbol refDeclSymbol = new PLCRefDeclSymbol();
            // 设置类型id，符号id
            refDeclSymbol.TypeId = IDGenerator.GetIDGenerator().NewTypeId();
            refDeclSymbol.SymbolId = IDGenerator.GetIDGenerator().NewSymbolId();
            // 设置内部元素id
            refDeclSymbol.ReferredTypeId = source.TypeId;

            // 设置名称 _REF_{ElementTypeId}
            refDeclSymbol.Name = $"_REF_{refDeclSymbol.ReferredTypeId}";

            // 加入符号表
            PLCTotalSymbolTable.TotalSymbolMap[refDeclSymbol.SymbolId] = refDeclSymbol;
            PLCTotalSymbolTable.TotalTypeMap[refDeclSymbol.TypeId] = refDeclSymbol;
            PLCTotalSymbolTable.RefSymbolMap[source.SymbolId] = refDeclSymbol;
        }
    }
}
