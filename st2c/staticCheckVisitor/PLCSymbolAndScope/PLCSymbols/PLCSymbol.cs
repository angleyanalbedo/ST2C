using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbolTables;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{


    public class PLCSymbol
    {
        // 每个符号唯一的ID 在构造方法中自动生成
        public int SymbolId { get; set; }

        // 符号的类型标识符
        public int TypeId { get; set; }

        // 符号名称
        public string Name { get; set; }

        // 符号所在行数
        public int RowNum { get; set; }

        // 符号所在列数
        public int ColumnNum { get; set; }

        // 符号所属作用域
        public PLCScope LocalScope { get; set; }

        // 指向所属符号表
        public PLCSymbolTable LocalSymbolTable { get; set; }

        // 符号的分类
        protected PLCModifierEnum.Sort Sort { get; set; }

        // 符号在runtime里的名称
        protected string RuntimeName { get; set; }

        // runtime值类型名称
        protected string RuntimeTypeName { get; set; }

        // 具体参数构造方法
        public PLCSymbol(string name, int rowNum)
        {
            this.Name = name;
            this.RowNum = rowNum;
            this.LocalSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.LocalScope = PLCScopeStack.CurrentScope;
        }

        // 默认构造方法
        public PLCSymbol()
        {
            this.LocalSymbolTable = PLCScopeStack.CurrentSymbolTable;
            this.LocalScope = PLCScopeStack.CurrentScope;
        }

        // 根据符号表或作用域设置所属
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

        public void SetSort(PLCModifierEnum.Sort sort)
        {
            this.Sort = sort;
        }

        public PLCModifierEnum.Sort GetSort()
        {
            return this.Sort;
        }

        public JToken ToStringJson()
        {
            JObject jsonObject = new JObject();
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString() ?? "null";
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCSymbol"] = jsonObject;
            return jsonSymbol;
        }

        public override string ToString()
        {
            return $"PLCSymbol{{symbolId={SymbolId}, typeId={TypeId}, name='{Name}', " +
                   $"rowNum={RowNum}, columnNum={ColumnNum}, sort={Sort}, " +
                   $"runtimeName='{RuntimeName}', runtimeTypeName='{RuntimeTypeName}'}}";
        }

        public void SetTypeId(int typeId)
        {
            this.TypeId = typeId;
        }

        internal int GetSymbolId()
        {
            return SymbolId;
        }
    }
}
