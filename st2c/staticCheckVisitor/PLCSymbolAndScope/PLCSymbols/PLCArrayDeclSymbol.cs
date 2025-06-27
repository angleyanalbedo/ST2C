using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    

    public class PLCArrayDeclSymbol : PLCTypeDeclSymbol
    {
        // 内部数据类型
        public int ElementTypeId { get; set; }

        // 维度数
        public int Dimension { get; set; }

        public PLCArrayDeclSymbol() : base()
        {
            sort = PLCModifierEnum.Sort.ARRAY_DECL;
            VarSort = PLCModifierEnum.Sort.ARRAY;
        }

        public PLCArrayDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            sort = PLCModifierEnum.Sort.ARRAY_DECL;
            VarSort = PLCModifierEnum.Sort.ARRAY;
        }

        public PLCArrayDeclSymbol(PLCArrayDeclSymbol resource) : base(resource)
        {
            Dimension = resource.Dimension;
            ElementTypeId = resource.typeId;
        }

        public override void SetTypeId(int typeId)
        {
            base.SetTypeId(typeId);
            RuntimeName = $"PLC_Array_Value<{typeId}>";
        }

        public override string ToString()
        {
            return $"PLCArrayDeclSymbol{{elementTypeId={ElementTypeId}, dimension={Dimension}, " +
                   $"initVar='{InitVar}', varSort={VarSort}, symbolId={symbolId}, " +
                   $"typeId={typeId}, name='{name}', rowNum={rowNum}, columnNum={ColumnNum}, " +
                   $"sort={sort}, runtimeName='{RuntimeName}', runtimeTypeName='{runtimeTypeName}'}}";
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject["elementTypeId"] = ElementTypeId;
            jsonObject["dimension"] = Dimension;
            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort?.ToString();
            jsonObject["symbolId"] = symbolId;
            jsonObject["typeId"] = typeId;
            jsonObject["name"] = name;
            jsonObject["rowNum"] = rowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = sort?.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = runtimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCArrayDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}
