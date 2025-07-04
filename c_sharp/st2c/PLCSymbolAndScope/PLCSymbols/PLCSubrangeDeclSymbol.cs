using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    using Newtonsoft.Json.Linq;
    using st2c.PLCSymbolAndScope.PLCSymbolTables;
    using System;

    public class PLCSubrangeDeclSymbol : PLCTypeDeclSymbol
    {
        public string UpperLimit { get; set; }
        public string LowerLimit { get; set; }
        public int SubTypeId { get; set; }
        public string GetUpperLimit()
        {
            return UpperLimit;
        }

        public void SetUpperLimit(string upperLimit)
        {
            UpperLimit = upperLimit;
        }

        public string GetLowerLimit()
        {
            return LowerLimit;
        }

        public void SetLowerLimit(string lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        public int GetSubTypeId()
        {
            return SubTypeId;
        }

        public PLCSubrangeDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.SUBRANGE_DECL;
            base.VarSort = PLCModifierEnum.Sort.SUBRANGE;
        }

        public PLCSubrangeDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            base.Sort = PLCModifierEnum.Sort.SUBRANGE_DECL;
            base.VarSort = PLCModifierEnum.Sort.SUBRANGE;
        }

        public PLCSubrangeDeclSymbol(PLCSubrangeDeclSymbol resource) : base(resource)
        {
        }

        public void SetSubTypeId(int typeId)
        {
            this.SubTypeId = typeId;
            PLCTypeDeclSymbol parentType = PLCTotalSymbolTable.GetTypeByTypeID(typeId);
            this.SetAssignableSet(parentType.GetAssignableSet());
            this.SetCalculableSet(parentType.GetCalculableSet());
            this.SetComparableSet(parentType.GetComparableSet());
            this.SetEqualitySet(parentType.GetEqualitySet());
        }

        public override string ToString()
        {
            return $"PLCSubrangeDeclSymbol{{" +
                   $"UpperLimit='{UpperLimit}', " +
                   $"LowerLimit='{LowerLimit}', " +
                   $"SubTypeId={SubTypeId}, " +
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

            jsonObject["UpperLimit"] = UpperLimit;
            jsonObject["LowerLimit"] = LowerLimit;
            jsonObject["SubTypeId"] = SubTypeId;
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
            jsonSymbol["PLCSubrangeDeclSymbol"] = jsonObject;

            return jsonSymbol;
        }
    }
}
