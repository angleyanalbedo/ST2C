using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    using System;
    using Newtonsoft.Json.Linq;

    public class PLCSubtypeDeclSymbol : PLCTypeDeclSymbol
    {
        public PLCSubtypeDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.SUBTYPE_DECL;
            base.VarSort = PLCModifierEnum.Sort.SUBTYPE;
        }

        public PLCSubtypeDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            base.Sort = PLCModifierEnum.Sort.SUBTYPE_DECL;
            base.VarSort = PLCModifierEnum.Sort.SUBTYPE;
        }

        public PLCSubtypeDeclSymbol(PLCSubtypeDeclSymbol resource) : base(resource)
        {
            base.Sort = PLCModifierEnum.Sort.SUBTYPE_DECL;
            base.VarSort = PLCModifierEnum.Sort.SUBTYPE;
        }

        public PLCTypeDeclSymbol ParentType
        {
            get { return parentType; }
            private set { parentType = value; }
        }

        public void SetParentType(PLCTypeDeclSymbol plcTypeDeclSymbol)
        {
            this.ParentType = plcTypeDeclSymbol;
            this.ParentType.SetAssignableSet(plcTypeDeclSymbol.GetAssignableSet());
            this.ParentType.SetCalculableSet(plcTypeDeclSymbol.GetCalculableSet());
            this.ParentType.SetComparableSet(plcTypeDeclSymbol.GetComparableSet());
            this.ParentType.SetEqualitySet(plcTypeDeclSymbol.GetEqualitySet());
        }

        private PLCTypeDeclSymbol parentType;

        public override string ToString()
        {
            return $"PLCSubtypeDeclSymbol{{" +
                   $"ParentType={ParentType}, " +
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

        public new JToken ToStringJson()
        {
            JObject jsonObject = new JObject();

            jsonObject["ParentType"] = ParentType.ToStringJson();
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
            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCSubtypeDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
