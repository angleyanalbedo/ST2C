using Newtonsoft.Json.Linq;
using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbolTables
{
    // 总符号表（依靠ID索引）
    public static class PLCTotalSymbolTable
    {
        // 类型id-类型声明符号表
        public static Dictionary<int, PLCTypeDeclSymbol> TotalTypeMap = new Dictionary<int, PLCTypeDeclSymbol>();

        public static PLCTypeDeclSymbol GetTypeByTypeID(int typeID)
        {
            return TotalTypeMap.TryGetValue(typeID, out var type) ? type : null;
        }

        // 符号id-符号表
        public static Dictionary<int, PLCSymbol> TotalSymbolMap = new Dictionary<int, PLCSymbol>();

        // 作用域ID-作用域表
        public static Dictionary<int, PLCScope> TotalScopeMap = new Dictionary<int, PLCScope>();

        // 符号表ID--符号表
        public static Dictionary<int, PLCSymbolTable> TotalTableMap = new Dictionary<int, PLCSymbolTable>();

        // 保存数组类型的表 键为类型的symbolid，值为类型的数组符号
        public static Dictionary<int, List<PLCArrayDeclSymbol>> ArraySymbolMap = new Dictionary<int, List<PLCArrayDeclSymbol>>();

        // 保存引用类型的表 键为类型的symbolid，值为类型的引用类型符号
        public static Dictionary<int, PLCRefDeclSymbol> RefSymbolMap = new Dictionary<int, PLCRefDeclSymbol>();

        // 将符号加入总表
        /// <summary>
        /// 仅仅加入一个符号
        /// </summary>
        public static void AddSymbol(PLCSymbol symbol)
        {
            TotalSymbolMap[symbol.GetSymbolId()] = symbol;
        }

        /// <summary>
        /// 仅仅加入一个作用域
        /// </summary>
        public static void AddScope(PLCScope scope)
        {
            TotalScopeMap[scope.GetScopeID()] = scope;
        }

        /// <summary>
        /// 仅仅加入一个符号表
        /// </summary>
        public static void AddTable(PLCSymbolTable table)
        {
            TotalTableMap[table.GetTableId()] = table;
        }

        /// <summary>
        /// 添加一种类型（符号声明对应的类型，不添加符号）
        /// </summary>
        public static void AddType(PLCTypeDeclSymbol declSymbol)
        {
            TotalTypeMap[declSymbol.TypeId] = declSymbol;
            new GenerateArrayTypes().Generate(declSymbol);
            new GenerateRefTypes().Generate(declSymbol);
        }

        /// <summary>
        /// 集合添加: 一个作用域、符号、表
        /// </summary>
        public static void AddBlock(PLCImportScopeTypeDeclType symbol)
        {
            AddSymbol(symbol);
            AddScope(symbol.GetImportScope());
            AddTable(symbol.GetImportSymbolTable());
        }

        public static void ShowTotalMap()
        {
            StringBuilder stringBuilder = new StringBuilder();

            PLCSymbolTable basicTypeTable = PLCScopeStack.BasicTypeTable;
            JArray jsonArray = new JArray();
            Dictionary<int, PLCSymbol> symbolIDDictionary = basicTypeTable.GetSymbolIDDictionary();
            foreach (PLCSymbol value in symbolIDDictionary.Values)
            {
                jsonArray.Add(value.ToStringJson());
            }
            JObject basicTableJson = new JObject();
            basicTableJson["Basic Table"] = jsonArray;
            JArray entirety = new JArray();
            entirety.Add(basicTableJson);

            foreach (PLCSymbolTable symbolTable in TotalTableMap.Values)
            {
                entirety.Add(symbolTable.ToJsonString());
            }

            stringBuilder.Append(Format.ToPrettyString(entirety));
            Console.WriteLine(stringBuilder.ToString());
            OutputToFile(stringBuilder);
        }

        public static void OutputToFile(StringBuilder stringBuilder)
        {
            string path = "output/symbolTable.json";

            try
            {
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // Write all text to file (automatically creates file if doesn't exist)
                File.WriteAllText(path, stringBuilder.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
