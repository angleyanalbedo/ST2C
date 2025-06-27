using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    

    public class PLCArrayDeclSymbol : PLCTypeDeclSymbol
    {
        // 内部数据类型
        public int ElementTypeId { get; set; }

        // 维度数
        public int Dimension { get; set; }

        public PLCArrayDeclSymbol() : base()
        {
            Sort = PLCModifierEnum.Sort.ARRAY_DECL;
            VarSort = PLCModifierEnum.Sort.ARRAY;
        }

        public PLCArrayDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            Sort = PLCModifierEnum.Sort.ARRAY_DECL;
            VarSort = PLCModifierEnum.Sort.ARRAY;
        }

        public PLCArrayDeclSymbol(PLCArrayDeclSymbol resource) : base(resource)
        {
            Dimension = resource.Dimension;
            ElementTypeId = resource.TypeId;
        }

        public  void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            RuntimeName = $"PLC_Array_Value<{typeId}>";
        }

        public override string ToString()
        {
            return $"PLCArrayDeclSymbol{{elementTypeId={ElementTypeId}, dimension={Dimension}, " +
                   $"initVar='{InitVar}', varSort={VarSort}, symbolId={SymbolId}, " +
                   $"typeId={TypeId}, name='{Name}', rowNum={RowNum}, columnNum={ColumnNum}, " +
                   $"sort={Sort}, runtimeName='{RuntimeName}', runtimeTypeName='{RuntimeTypeName}'}}";
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject["elementTypeId"] = ElementTypeId;
            jsonObject["dimension"] = Dimension;
            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort.ToString();
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCArrayDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
