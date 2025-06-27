using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static st2c.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    // 枚举量定义时也会引入一个作用域
    public class PLCEnumDeclSymbol : PLCImportScopeTypeDeclType
    {
        // 枚举常量的类型id
        private int enumConstTypeId;

        public int GetEnumConstTypeId()
        {
            return enumConstTypeId;
        }

        public void SetEnumConstTypeId(int enumConstTypeId)
        {
            this.enumConstTypeId = enumConstTypeId;
        }

        // 枚举类型的内部常量
        private List<PLCVariable> enumValues = new List<PLCVariable>();

        public List<PLCVariable> GetEnumValues()
        {
            return enumValues;
        }

        public void SetEnumValues(List<PLCVariable> enumValues)
        {
            this.enumValues = enumValues;
        }

        public void AddEnumValue(PLCVariable enumValue)
        {
            enumValues.Add(enumValue);
        }

        // 返回枚举类型内的枚举常量
        public PLCVariable FindEnumValue(string name)
        {
            foreach (PLCVariable enumValue in enumValues)
            {
                if (name.Equals(enumValue.GetName()))
                {
                    return enumValue;
                }
            }
            return null;
        }

        // 默认的枚举常量
        private PLCVariable initEnumVar;

        public PLCVariable GetInitEnumVar()
        {
            return initEnumVar;
        }

        public void SetInitEnumVar(PLCVariable initEnumVar)
        {
            this.initEnumVar = initEnumVar;
        }

        public List<string> GetEnumElemName()
        {
            return enumElemName;
        }

        public void SetEnumElemName(List<string> enumElemName)
        {
            this.enumElemName = enumElemName;
        }

        public void AddEnumElemName(string name)
        {
            enumElemName.Add(name);
        }

        // 枚举常量列表
        public List<string> enumElemName = new List<string>();

        // 枚举常量的sort
        public Sort enumConstSort;

        public Sort GetEnumConstSort()
        {
            return enumConstSort;
        }

        public PLCEnumDeclSymbol() : base()
        {
            Sort = Sort.ENUM_DECL;
            VarSort = Sort.ENUM;
        }

        public PLCEnumDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            Sort = Sort.ENUM_DECL;
            VarSort = Sort.ENUM;
        }

        public PLCEnumDeclSymbol(PLCEnumDeclSymbol resource) : base(resource)
        {
        }

        public  void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            this.SetRuntimeName("PLC_Enum_Value<" + typeId + ">");
        }

        private void SetRuntimeName(string v)
        {
            this.RuntimeName = v;
        }

        public override string ToString()
        {
            return $"PLCEnumDeclSymbol{{" +
                   $"enumConstTypeId={enumConstTypeId}, " +
                   $"enumValues={string.Join(", ", enumValues)}, " +
                   $"initEnumVar={initEnumVar}, " +
                   $"enumElemName={string.Join(", ", enumElemName)}, " +
                   $"enumConstSort={enumConstSort}, " +
                   $"initVar='{InitVar}', " +
                   $"varSort={VarSort}, " +
                   $"symbolId={SymbolId}, " +
                   $"typeId={TypeId}, " +
                   $"name='{Name}', " +
                   $"rowNum={RowNum}, " +
                   $"columnNum={ColumnNum}, " +
                   $"sort={Sort}, " +
                   $"runtimeName='{RuntimeName}', " +
                   $"runtimeTypeName='{RuntimeTypeName}'" +
                   $"}}";
        }

        public JToken ToStringJson()
        {
            JObject jsonObject = new JObject();
            jsonObject["enumConstTypeId"] = enumConstTypeId;

            JArray jsonArray = new JArray();
            foreach (PLCVariable plcVariable in enumValues)
            {
                jsonArray.Add(plcVariable.ToStringJson());
            }
            jsonObject["enumValues"] = jsonArray;

            jsonArray = new JArray();
            jsonObject["initEnumVar"] = initEnumVar != null ? initEnumVar.ToStringJson() : null;

            jsonArray = new JArray();
            foreach (string enumName in enumElemName)
            {
                jsonArray.Add(enumName);
            }
            jsonObject["enumElemName"] = jsonArray;

            jsonObject["enumConstSort"] = enumConstSort != null ? enumConstSort.ToString() : null;
            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort != null ? VarSort.ToString() : null;
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort != null ? Sort.ToString() : null;
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCEnumDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
