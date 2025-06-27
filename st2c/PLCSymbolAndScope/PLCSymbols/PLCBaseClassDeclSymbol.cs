using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
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
            this.enumValues.Add(enumValue);
        }

        // 返回枚举类型内的枚举常量
        public PLCVariable FindEnumValue(string name)
        {
            foreach (PLCVariable enumValue in this.enumValues)
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
            this.enumElemName.Add(name);
        }

        // 枚举常量列表
        public List<string> enumElemName = new List<string>();

        // 枚举常量的sort
        public PLCModifierEnum.Sort enumConstSort;

        public PLCModifierEnum.Sort GetEnumConstSort()
        {
            return enumConstSort;
        }

        public PLCEnumDeclSymbol() : base()
        {
            sort = PLCModifierEnum.Sort.ENUM_DECL;
            varSort = PLCModifierEnum.Sort.ENUM;
        }

        public PLCEnumDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            sort = PLCModifierEnum.Sort.ENUM_DECL;
            varSort = PLCModifierEnum.Sort.ENUM;
        }

        public PLCEnumDeclSymbol(PLCEnumDeclSymbol resource) : base(resource)
        {
        }

        public override void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            this.SetRuntimeName("PLC_Enum_Value<" + typeId + ">");
        }

        public override string ToString()
        {
            return $"PLCEnumDeclSymbol{{" +
                   $"enumConstTypeId={enumConstTypeId}, " +
                   $"enumValues={string.Join(", ", enumValues)}, " +
                   $"initEnumVar={initEnumVar}, " +
                   $"enumElemName={string.Join(", ", enumElemName)}, " +
                   $"enumConstSort={enumConstSort}, " +
                   $"initVar='{initVar}', " +
                   $"varSort={varSort}, " +
                   $"symbolId={symbolId}, " +
                   $"typeId={typeId}, " +
                   $"name='{name}', " +
                   $"rowNum={rowNum}, " +
                   $"columnNum={columnNum}, " +
                   $"sort={sort}, " +
                   $"runtimeName='{runtimeName}', " +
                   $"runtimeTypeName='{runtimeTypeName}'" +
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
            jsonObject["initVar"] = initVar;
            jsonObject["varSort"] = varSort != null ? varSort.ToString() : null;
            jsonObject["symbolId"] = symbolId;
            jsonObject["typeId"] = typeId;
            jsonObject["name"] = name;
            jsonObject["rowNum"] = rowNum;
            jsonObject["columnNum"] = columnNum;
            jsonObject["sort"] = sort != null ? sort.ToString() : null;
            jsonObject["runtimeName"] = runtimeName;
            jsonObject["runtimeTypeName"] = runtimeTypeName;

            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCEnumDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
