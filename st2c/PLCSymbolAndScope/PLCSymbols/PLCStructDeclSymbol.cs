using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class PLCStructDeclSymbol : PLCImportScopeTypeDeclType
    {
        public bool IfOverlap { get; set; } = false;

        public PLCStructDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.STRUCT_DECL;
            base.VarSort = PLCModifierEnum.Sort.STRUCT;
            this.RuntimeName = $"PLC_Struct_Decl<{this.TypeId}>";
        }

        public PLCStructDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            base.Sort = PLCModifierEnum.Sort.STRUCT_DECL;
            base.VarSort = PLCModifierEnum.Sort.STRUCT;
        }

        public PLCStructDeclSymbol(PLCStructDeclSymbol resource) : base(resource)
        {
            base.Sort = PLCModifierEnum.Sort.STRUCT_DECL;
            base.VarSort = PLCModifierEnum.Sort.STRUCT;
        }

        [Obsolete]
        public List<PLCStructDeclSymbol> StructElements { get; set; } = new List<PLCStructDeclSymbol>();

        [Obsolete]
        public void AddPlcStructDeclSymbols(PLCStructDeclSymbol plcStructDeclSymbol)
        {
            this.StructElements.Add(plcStructDeclSymbol);
        }

        [Obsolete]
        public void SetStructElements(List<PLCStructDeclSymbol> plcStructDeclSymbolList)
        {
            this.StructElements = plcStructDeclSymbolList;
        }

        public bool IfConst { get; set; } = false;

        public new void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            this.RuntimeName = $"PLC_Struct_Value<{typeId}>";
        }

        public override string ToString()
        {
            return $"PLCStructDeclSymbol{{" +
                   $"IfOverlap={IfOverlap}, " +
                   $"StructElements={StructElements}, " +
                   $"IfConst={IfConst}, " +
                   $"InitVar='{InitVar}', " +
                   $"VarSort={VarSort}, " +
                   $"SymbolId={SymbolId}, " +
                   $"TypeId={TypeId}, " +
                   $"Name='{Name}', " +
                   $"RowNum={RowNum}, " +
                   $"ColumnNum={ColumnNum}, " +
                   $"Sort={Sort}, " +
                   $"RuntimeName='{RuntimeName}', " +
                   $"RuntimeTypeName='{RuntimeTypeName}'" +
                   $"}}";
        }

        public JToken ToStringJson()
        {
            JObject jsonObject = new JObject();

            jsonObject["IfOverlap"] = IfOverlap;
            jsonObject["IfConst"] = IfConst;
            jsonObject["InitVar"] = InitVar;
            string varSortName = VarSort != null ? VarSort.ToString() : "null";
            jsonObject["VarSort"] = varSortName;
            jsonObject["SymbolId"] = SymbolId;
            jsonObject["TypeId"] = TypeId;
            jsonObject["Name"] = Name;
            jsonObject["RowNum"] = RowNum;
            jsonObject["ColumnNum"] = ColumnNum;
            string sortName = Sort != null ? Sort.ToString() : "null";
            jsonObject["Sort"] = sortName;
            jsonObject["RuntimeName"] = RuntimeName;
            jsonObject["RuntimeTypeName"] = RuntimeTypeName;
            JArray jsonArray = new JArray();
            foreach (PLCStructDeclSymbol plcStructDeclSymbol in StructElements)
            {
                jsonArray.Add(plcStructDeclSymbol.ToStringJson());
            }
            jsonObject["PLCStructDeclSymbolList"] = jsonArray;
            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCStructDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }

        private readonly List<PLCVariable> Variables = new List<PLCVariable>();

        public List<PLCVariable> GetVariables()
        {
            return Variables;
        }

        public void AddVariable(PLCVariable var)
        {
            this.Variables.Add(var);
        }

        public void AddAllVariable(List<PLCVariable> vars)
        {
            this.Variables.AddRange(vars);
        }
    }
}
