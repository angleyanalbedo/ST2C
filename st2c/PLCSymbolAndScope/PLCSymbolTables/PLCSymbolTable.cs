using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;
using st2c.PLCSymbolAndScope;
using st2c.PLCSymbolAndScope.PLCScope;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.PLCSymbolAndScope.PLCScopes;

namespace st2c.PLCSymbolAndScope.PLCSymbolTables
{
    

    public class PLCSymbolTable
    {
        // 符号表ID
        public int TableId { get; set; }

        // 引入此表的符号对象
        public PLCImportScopeTypeDeclType SrcSymbol { get; set; }

        // 符号表所属作用域
        public PLCScope TableScope { get; set; }

        // 符号存储字典
        public Dictionary<string, PLCSymbol> SymbolNameDict { get; } = new Dictionary<string, PLCSymbol>();
        public Dictionary<int, PLCSymbol> SymbolIdDict { get; } = new Dictionary<int, PLCSymbol>();

        public PLCSymbolTable()
        {
            TableId = IDGenerator.Instance.NewTableId();
        }

        // 查找符号方法
        public PLCSymbol FindSymbol(string name)
        {
            return SymbolNameDict.TryGetValue(name, out var symbol) ? symbol : null;
        }

        public PLCSymbol FindSymbol(string name, PLCModifierEnum.Sort sort)
        {
            var sameNamedSymbols = FindSameNamedSymbol(name);
            foreach (var symbol in sameNamedSymbols)
            {
                if (symbol.GetSort() == sort)
                    return symbol;
            }
            return null;
        }

        public List<PLCSymbol> FindSameNamedSymbol(string name)
        {
            var sameNameSymbols = new List<PLCSymbol>();
            foreach (var symbol in SymbolIdDict.Values)
            {
                if (name == symbol.Name)
                    sameNameSymbols.Add(symbol);
            }
            return sameNameSymbols;
        }

        // 添加符号方法
        public void AddSymbol(PLCSymbol symbol, int symbolId, string symbolName)
        {
            SymbolIdDict[symbolId] = symbol;
            SymbolNameDict[symbolName] = symbol;
        }

        public void AddSymbol(PLCSymbol plcSymbol)
        {
            SymbolNameDict[plcSymbol.Name] = plcSymbol;
            SymbolIdDict[plcSymbol.SymbolId] = plcSymbol;
        }

        // JSON序列化
        public JObject ToJsonString()
        {
            var jsonObject = new JObject();
            jsonObject["Table Id"] = TableId;
            jsonObject["Symbol name"] = SrcSymbol?.Name;
            jsonObject["Symbol list"] = "";
            jsonObject["Symbol Sort"] = SrcSymbol?.GetSort().ToString() ?? "null";

            var jsonArray = new JArray();
            foreach (var symbol in SymbolIdDict.Values)
            {
                jsonArray.Add(symbol.ToStringJson());
            }
            jsonObject["PLCSymbolMap"] = jsonArray;

            return jsonObject;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"Table Id:{TableId}")
               .AppendLine($"Symbol name:{SrcSymbol?.Name}")
               .AppendLine("Symbol list :")
               .AppendLine($"Symbol Sort : {SrcSymbol?.GetSort()}");

            foreach (var symbol in SymbolIdDict.Values)
            {
                str.AppendLine(symbol.ToStringJson().ToString());
            }
            str.AppendLine("*******************************");
            return str.ToString();
        }

        public Dictionary<int, PLCSymbol> GetSymbolIDDictionary()
        {
            return SymbolIdDict;
        }

        public int GetTableId()
        {
            return TableId;
        }

        public void SetSrcSymbol(PLCImportScopeTypeDeclType symbol)
        {
            SrcSymbol = symbol;
        }

        public void SetTableScope(PLCScope scope)
        {
            TableScope = scope;
        }

        internal PLCScope GetTableScope()
        {
            return TableScope;
        }
    }
}
