using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using st2c.PLCSymbolAndScope.PLCSymbolTables;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    

    public class PLCVariable : PLCSymbol
    {
        /*
        * variable中的几个属性都需要手动设置
        * */

        public string GetUniqueName()
        {
            StringBuilder sb = new StringBuilder();
            string typeName = PLCTotalSymbolTable.GetTypeByTypeID(this.TypeId).Name;
            sb.Append("(*::PLC::RFM->getSymbolByID<").Append(typeName).Append("*>(").Append(this.SymbolId).Append("))");
            return sb.ToString();
        }

        public string GetUniqueNameOfInstance(int instanceSymbolId)
        {
            StringBuilder sb = new StringBuilder();
            string typeName = PLCTotalSymbolTable.GetTypeByTypeID(this.TypeId).Name;
            sb.Append("(*::PLC::RFM->getSymbolByID<").Append(typeName).Append("*>(").Append(instanceSymbolId).Append(", ").Append(this.SymbolId).Append("))");
            return sb.ToString();
        }

        // 变量段类型
        public PLCModifierEnum.VarSections VarSections { get; set; } = PLCModifierEnum.VarSections.VAR;

        // 访问权限
        public PLCModifierEnum.AccessModifier AccessModifier { get; set; } = PLCModifierEnum.AccessModifier.NOT_DECLARED;

        // 是否为保留变量
        public PLCModifierEnum.RetainModifier RetainQualifiers { get; set; } = PLCModifierEnum.RetainModifier.NON_RETAIN;

        // 是否是常量
        public bool IfConst { get; set; } = false;

        // 在声明时变量被赋予的初始值
        // 或者是expr产生的临时变量的值
        // 默认为empty
        private string AssignVar { get; set; } = "";

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string location = "";

        public void SetIfConst(bool ifConst)
        {
            IfConst = ifConst;
        }

        public bool GetIfConst()
        {
            return IfConst;
        }

        public void SetVarSections(PLCModifierEnum.VarSections varSections)
        {
            VarSections = varSections;
        }

        public PLCModifierEnum.VarSections GetVarSections()
        {
            return VarSections;
        }

        public void SetRetainQualifiers(PLCModifierEnum.RetainModifier retainQualifiers)
        {
            RetainQualifiers = retainQualifiers;
        }

        public void SetRetainQualifiers(string retainQualifiers)
        {
            RetainQualifiers = (PLCModifierEnum.RetainModifier)Enum.Parse(typeof(PLCModifierEnum.RetainModifier), retainQualifiers);
        }

        public PLCModifierEnum.RetainModifier GetRetainQualifiers()
        {
            return RetainQualifiers;
        }

        public void SetAccessModifier(PLCModifierEnum.AccessModifier accessModifier)
        {
            AccessModifier = accessModifier;
        }

        public PLCModifierEnum.AccessModifier GetAccessModifier()
        {
            return AccessModifier;
        }

        public PLCTypeDeclSymbol DeclSymbol { get; set; }

        public PLCVariable()
        {
        }

        public PLCVariable(PLCVariable another)
        {
            TypeId = another.TypeId;
            Sort = another.Sort;
            VarSections = another.VarSections;
            AccessModifier = another.AccessModifier;
            RetainQualifiers = another.RetainQualifiers;
            IfConst = another.IfConst;
            AssignVar = another.AssignVar;
            Location = another.Location;
            DeclSymbol = another.DeclSymbol;
        }

        public string GetAssignVar()
        {
            return AssignVar;
        }

        public void SetAssignVar(string assignVar)
        {
            AssignVar = assignVar;
        }

        public override string ToString()
        {
            return $"PLCVariable{{" +
                   $"VarSections={VarSections}," +
                   $"AccessModifier={AccessModifier}," +
                   $"RetainQualifiers={RetainQualifiers}," +
                   $"IfConst={IfConst}," +
                   $"AssignVar='{AssignVar}'," +
                   $"Location='{Location}'," +
                   $"SymbolId={SymbolId}," +
                   $"TypeId={TypeId}," +
                   $"Name='{Name}'," +
                   $"RowNum={RowNum}," +
                   $"ColumnNum={ColumnNum}," +
                   $"Sort={Sort}," +
                   $"RuntimeName='{RuntimeName}'," +
                   $"RuntimeTypeName='{RuntimeTypeName}'" +
                   $"}}";
        }

        public JToken ToStringJson()
        {
            JObject jsonObject = new JObject();

            jsonObject["varSections"] = VarSections.ToString();
            jsonObject["accessModifier"] = AccessModifier.ToString();
            jsonObject["retainQualifiers"] = RetainQualifiers.ToString();
            jsonObject["ifConst"] = IfConst;
            jsonObject["assignVar"] = AssignVar;
            jsonObject["location"] = Location;
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;
            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCVariable"] = jsonObject;
            return jsonSymbol;
        }

        internal string GetName()
        {
            return Name;
        }
    }

   

    
}
