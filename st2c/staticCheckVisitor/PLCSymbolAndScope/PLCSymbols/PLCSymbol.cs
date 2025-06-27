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
        public int symbolId { get; set; }

        // 类型标识符
        public int typeId { get; set; }

        // 符号名称
        public string name { get; set; }

        // 符号位置信息
        public int rowNum { get; set; }
        public int columnNum { get; set; }

        // 作用域相关
        public PLCScope localScope { get; set; }
        public PLCSymbolTable localSymbolTable { get; set; }

        // 符号分类
        protected PLCModifierEnum.Sort sort { get; set; }

        // 运行时信息
        protected string runtimeName { get; set; }
        protected string runtimeTypeName { get; set; }

        public PLCSymbol(string name, int rowNum)
        {
            this.name = name;
            this.rowNum = rowNum;
            this.localSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.localScope = PLCScopeStack.CurrentScope;
        }

        public PLCSymbol()
        {
            this.localSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.localScope = PLCScopeStack.CurrentScope;
        }

        public void SetLocal(PLCScope localScope)
        {
            this.localSymbolTable = localScope.GetScopeSymbolTable();
            this.localScope = localScope;
        }

        public void SetLocal(PLCSymbolTable localTable)
        {
            this.localSymbolTable = localTable;
            this.localScope = localTable.GetTableScope();
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject["symbolId"] = symbolId;
            jsonObject["typeId"] = typeId;
            jsonObject["name"] = name;
            jsonObject["rowNum"] = rowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = sort?.ToString() ?? "null";
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = runtimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
