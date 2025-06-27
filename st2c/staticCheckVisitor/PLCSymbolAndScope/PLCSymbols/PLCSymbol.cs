using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    

    public class PLCSymbol
    {
        // 符号唯一ID
        public int SymbolId { get; set; }

        // 类型标识符
        public int TypeId { get; set; }

        // 符号名称
        public string Name { get; set; }

        // 符号位置信息
        public int RowNum { get; set; }
        public int ColumnNum { get; set; }

        // 作用域相关
        public PLCScope LocalScope { get; set; }
        public PLCSymbolTable LocalSymbolTable { get; set; }

        // 符号分类
        protected PLCModifierEnum.Sort Sort { get; set; }

        // 运行时信息
        protected string RuntimeName { get; set; }
        protected string RuntimeTypeName { get; set; }

        public PLCSymbol(string name, int rowNum)
        {
            this.Name = name;
            this.RowNum = rowNum;
            this.LocalSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.LocalScope = PLCScopeStack.CurrentScope;
        }

        public PLCSymbol()
        {
            this.LocalSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.LocalScope = PLCScopeStack.CurrentScope;
        }

        public void SetLocal(PLCScope localScope)
        {
            this.LocalSymbolTable = localScope.GetScopeSymbolTable();
            this.LocalScope = localScope;
        }

        public void SetLocal(PLCSymbolTable localTable)
        {
            this.LocalSymbolTable = localTable;
            this.LocalScope = localTable.GetTableScope();
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort?.ToString() ?? "null";
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
