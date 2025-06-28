using Newtonsoft.Json.Linq;
using st2c.PLCSymbolAndScope.PLCScopes;
using st2c.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCImportScopeTypeDeclType : PLCTypeDeclSymbol
    {
        public PLCImportScopeTypeDeclType(PLCImportScopeTypeDeclType resource) : base(resource)
        {
            ImportScope = resource.ImportScope;
            ImportSymbolTable = resource.ImportSymbolTable;
        }

        public PLCImportScopeTypeDeclType() : base()
        {
        }

        public PLCImportScopeTypeDeclType(string name, int rowNum) : base(name, rowNum)
        {
        }

        // 该符号引入的符号表，在入栈时自动设置
        public PLCSymbolTable ImportSymbolTable { get; set; }

        // 该符号引入的作用域，在入栈时自动设置
        public PLCScope ImportScope { get; set; }

        public override string ToString()
        {
            return $"PLCImportScopeTypeDeclType{{" +
                   $"initVar='{InitVar}', " +
                   $"varSort={VarSort}, " +
                   $"symbolId={SymbolId}, " +
                   $"typeId={TypeId}, " +
                   $"name='{Name}', " +
                   $"rowNum={RowNum}, " +
                   $"columnNum={ColumnNum}, " +
                   $"sort={Sort}, " +
                   $"runtimeName='{RuntimeName}', " +
                   $"runtimeTypeName='{RuntimeTypeName}'}}";
        }

        public new JToken ToStringJson()
        {
            var jsonObject = new JObject();
            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort.ToString();
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCImportScopeTypeDeclType"] = jsonObject;
            return jsonSymbol;
        }

        internal PLCScope GetImportScope()
        {
            return ImportScope;
        }

        internal PLCSymbolTable GetImportSymbolTable()
        {
            return ImportSymbolTable;
        }

        

        internal void SetImportSymbolTable(PLCSymbolTable table)
        {
            this.ImportSymbolTable = table;
        }

        internal void SetImportScope(PLCScope scope)
        {
            this.ImportScope = scope;
        }

       
    }
}
