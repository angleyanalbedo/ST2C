﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // 用于 JSON 操作
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using st2c.PLCSymbolAndScope;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCTypeDeclSymbol : PLCSymbol
    {
        public PLCTypeDeclSymbol() : base()
        {
        }

        public PLCTypeDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
        }

        public PLCTypeDeclSymbol(PLCTypeDeclSymbol resource) : base()
        {
            Sort = resource.Sort;
            VarSort = resource.VarSort;

            TypeId = resource.TypeId;
            AssignableSet.Add(TypeId);

            SymbolId = resource.SymbolId;
            InitVar = resource.InitVar;
            Name = resource.Name;
            RuntimeName = resource.RuntimeName;
        }

        // 内置类型初始化专供,其他情况不应当调用
        public PLCTypeDeclSymbol(int symbolId, int typeId, string name)
        {
            SymbolId = symbolId;
            TypeId = typeId;
            Name = name;
            RowNum = -1;
            LocalScope = PLCScopeStack.GlobalScope;
            LocalSymbolTable = PLCScopeStack.GlobalSymbolTable;
        }

        public new void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            AssignableSet.Add(typeId);
        }

        // 类型的初始值
        public string InitVar { get; set; } = "";

        public string GetInitVar()
        {
            return InitVar;
        }
        public void SetInitVar(string initVar)
        {
            InitVar = initVar;
        }

        // 类型对应的变量的分类,默认确定
        public PLCModifierEnum.Sort VarSort { get; set; }

        public PLCModifierEnum.Sort GetVarSort()
        {
            return VarSort;
        }
        public void SetVarSort(PLCModifierEnum.Sort varSort)
        {
            VarSort = varSort;
        }

        // 存储可进行数学运算的类型
        private HashSet<int> CalculableSet { get; set; } = new HashSet<int>();

        public HashSet<int> GetCalculableSet()
        {
            return CalculableSet;
        }
        public void SetCalculableSet(HashSet<int> calculableSet)
        {
            CalculableSet = calculableSet;
        }

        // 检查是否可进行数字运算(+-*/)
        public bool CheckCanMathCalcWith(int typeId)
        {
            return CalculableSet.Contains(typeId);
        }

        // 添加可进行数学运算的类型id
        public void AddCalculableType(int typeId)
        {
            CalculableSet.Add(typeId);
        }

        // 存储可进行大小比较的类型id
        private HashSet<int> ComparableSet { get; set; } = new HashSet<int>();

        public HashSet<int> GetComparableSet()
        {
            return ComparableSet;
        }
        public void SetComparableSet(HashSet<int> comparableSet)
        {
            ComparableSet = comparableSet;
        }

        // 检查是否可进行大小比较
        public bool CheckCanCompareWith(int typeId)
        {
            return ComparableSet.Contains(typeId);
        }

        // 添加可进行大小比较的类型
        public void AddComparableType(int typeId)
        {
            ComparableSet.Add(typeId);
        }

        // 存储可进行判等的类型id
        private HashSet<int> EqualitySet { get; set; } = new HashSet<int>();
        public HashSet<int> GetEqualitySet()
        {
            return EqualitySet;
        }
        public void SetEqualitySet(HashSet<int> equalitySet)
        {
            EqualitySet = equalitySet;
        }

        // 检查是否可进行判等
        public bool CheckCanEqualWith(int typeId)
        {
            return EqualitySet.Contains(typeId);
        }

        // 添加可进行判等的类型
        public void AddEqualType(int typeId)
        {
            EqualitySet.Add(typeId);
        }

        // 存储可进行赋值的类型id
        private HashSet<int> AssignableSet { get; set; } = new HashSet<int>();
        public HashSet<int> GetAssignableSet()
        {
            return AssignableSet;
        }
        public void SetAssignableSet(HashSet<int> assignableSet)
        {
            AssignableSet = assignableSet;
        }

        // 检查是否可进行赋值
        public bool CheckCanAssignWith(int typeId)
        {
            return AssignableSet.Contains(typeId);
        }

        // 添加可进行赋值的类型id
        public void AddAssignableType(int typeId)
        {
            AssignableSet.Add(typeId);
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append($"PLCTypeDeclSymbol{{initVar='{InitVar}', ").Append($"varSort={VarSort}, ").Append($"calculableSet={string.Join(",", CalculableSet)}, ")
               .Append($"comparableSet={string.Join(",", ComparableSet)}, ")
               .Append($"equalitySet={string.Join(",", EqualitySet)}, ")
               .Append($"assignableSet={string.Join(",", AssignableSet)}, ")
               .Append($"symbolId={SymbolId}, ")
               .Append($"typeId={TypeId}, ")
               .Append($"name='{Name}', ")
               .Append($"rowNum={RowNum}, ")
               .Append($"columnNum={ColumnNum}, ")
               .Append($"sort={Sort}, ")
               .Append($"runtimeName='{RuntimeName}', ")
               .Append($"runtimeTypeName='{RuntimeTypeName}'}}");
            return str.ToString();
        }

        public new JToken ToStringJson()
        {
            var jsonObject = new JObject();

            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort.ToString() ?? "null";

            jsonObject["calculableSet"] = new JArray(CalculableSet);
            jsonObject["comparableSet"] = new JArray(ComparableSet);
            jsonObject["equalitySet"] = new JArray(EqualitySet);
            jsonObject["assignableSet"] = new JArray(AssignableSet);

            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCTypeDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
