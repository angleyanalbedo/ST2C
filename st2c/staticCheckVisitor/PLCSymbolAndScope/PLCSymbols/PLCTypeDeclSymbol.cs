using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // 用于 JSON 操作
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    

    public class PLCTypeDeclSymbol : PLCSymbol
    {
        // 类型的初始值
        protected string initVar = "";

        public string InitVar
        {
            get { return initVar; }
            set { initVar = value; }
        }

        // 类型对应的变量的分类，默认确定
        protected PLCModifierEnum.Sort varSort;

        public PLCModifierEnum.Sort VarSort
        {
            get { return varSort; }
            set { varSort = value; }
        }

        // 存储可进行数学运算的类型
        private HashSet<int> calculableSet = new HashSet<int>();

        public HashSet<int> CalculableSet
        {
            get { return calculableSet; }
            set { calculableSet = value; }
        }

        // 存储可进行大小比较的类型
        private HashSet<int> comparableSet = new HashSet<int>();

        public HashSet<int> ComparableSet
        {
            get { return comparableSet; }
            set { comparableSet = value; }
        }

        // 存储可进行判等的类型
        private HashSet<int> equalitySet = new HashSet<int>();

        public HashSet<int> EqualitySet
        {
            get { return equalitySet; }
            set { equalitySet = value; }
        }

        // 存储可进行赋值的类型
        private HashSet<int> assignableSet = new HashSet<int>();

        public HashSet<int> AssignableSet
        {
            get { return assignableSet; }
            set { assignableSet = value; }
        }

        public PLCTypeDeclSymbol() : base()
        {
        }

        public PLCTypeDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
        }

        public PLCTypeDeclSymbol(PLCTypeDeclSymbol resource) : base()
        {
            this.sort = resource.sort;
            this.varSort = resource.varSort;

            this.typeId = resource.typeId;
            this.assignableSet.Add(typeId);

            this.symbolId = resource.symbolId;
            this.initVar = resource.initVar;
            this.name = resource.name;
            this.runtimeName = resource.runtimeName;
        }

        // 内置类型初始化专供，其他情况不应当调用
        public PLCTypeDeclSymbol(int symbolId, int typeId, string name)
        {
            this.symbolId = symbolId;
            this.typeId = typeId;
            this.name = name;
            this.rowNum = -1;
            this.localScope = PLCScopeStack.GlobalScope;
            this.localSymbolTable = PLCScopeStack.GlobalSymbolTable;
        }

        public override void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            this.assignableSet.Add(typeId);
        }

        // 检查是否可进行数字运算 (+-*/)
        public bool CheckCanMathCalcWith(int typeId)
        {
            return this.calculableSet.Contains(typeId);
        }

        // 添加可进行数学运算的类型
        public void AddCalculableType(int typeId)
        {
            this.calculableSet.Add(typeId);
        }

        // 检查是否可进行大小比较
        public bool CheckCanCompareWith(int typeId)
        {
            return this.comparableSet.Contains(typeId);
        }

        // 添加可进行大小比较的类型
        public void AddComparableType(int typeId)
        {
            this.comparableSet.Add(typeId);
        }

        // 检查是否可进行判等
        public bool CheckCanEqualWith(int typeId)
        {
            return this.equalitySet.Contains(typeId);
        }

        // 添加可进行判等的类型
        public void AddEqualType(int typeId)
        {
            this.equalitySet.Add(typeId);
        }

        // 检查是否可进行赋值
        public bool CheckCanAssignWith(int typeId)
        {
            return this.assignableSet.Contains(typeId);
        }

        // 添加可进行赋值的类型
        public void AddAssignableType(int typeId)
        {
            this.assignableSet.Add(typeId);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("PLCTypeDeclSymbol{initVar='").Append(initVar).Append('\'')
                .Append(", varSort=").Append(varSort)
                .Append(", calculableSet=").Append(string.Join(", ", calculableSet))
                .Append(", comparableSet=").Append(string.Join(", ", comparableSet))
                .Append(", equalitySet=").Append(string.Join(", ", equalitySet))
                .Append(", assignableSet=").Append(string.Join(", ", assignableSet))
                .Append(", symbolId=").Append(symbolId)
                .Append(", typeId=").Append(typeId)
                .Append(", name='").Append(name).Append('\'')
                .Append(", rowNum=").Append(rowNum)
                .Append(", columnNum=").Append(columnNum)
                .Append(", sort=").Append(sort)
                .Append(", runtimeName='").Append(runtimeName).Append('\'')
                .Append(", runtimeTypeName='").Append(runtimeTypeName).Append('\'').Append('}');
            return str.ToString();
        }

        public JToken ToStringJson()
        {
            JObject jsonObject = new JObject();

            jsonObject["initVar"] = initVar;
            jsonObject["varSort"] = varSort?.ToString() ?? "null";
            jsonObject["calculableSet"] = new JArray(calculableSet);
            jsonObject["comparableSet"] = new JArray(comparableSet);
            jsonObject["equalitySet"] = new JArray(equalitySet);
            jsonObject["assignableSet"] = new JArray(assignableSet);
            jsonObject["symbolId"] = symbolId;
            jsonObject["typeId"] = typeId;
            jsonObject["name"] = name;
            jsonObject["rowNum"] = rowNum;
            jsonObject["columnNum"] = columnNum;
            jsonObject["sort"] = sort?.ToString();
            jsonObject["runtimeName"] = runtimeName;
            jsonObject["runtimeTypeName"] = runtimeTypeName;

            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCTypeDeclSymbol"] = jsonObject;

            return jsonSymbol;
        }
    }
}
