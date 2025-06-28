using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCNamespaceDeclSymbol : PLCImportScopeTypeDeclType, UsingNamespace
    {
        public PLCNamespaceDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.NAMESPACE_DECL;
            VarSort = PLCModifierEnum.Sort.NAMESPACE;
        }

        public PLCNamespaceDeclSymbol(string name, int rowNum)
        {
            base.Name = name;
            base.RowNum = rowNum;
            base.Sort = PLCModifierEnum.Sort.NAMESPACE_DECL;
            VarSort = PLCModifierEnum.Sort.NAMESPACE;
        }

        public PLCNamespaceDeclSymbol(PLCNamespaceDeclSymbol resource)
        {
            base.Name = resource.Name;
            base.RowNum = resource.RowNum;
            base.Sort = (PLCModifierEnum.Sort)resource.Sort;
            VarSort = resource.VarSort;
            InitVar = resource.InitVar;
            SymbolId = resource.SymbolId;
            TypeId = resource.TypeId;
            ColumnNum = resource.ColumnNum;
            RuntimeName = resource.RuntimeName;
            RuntimeTypeName = resource.RuntimeTypeName;
            UsingNamespaceList.AddRange(resource.UsingNamespaceList);
        }

        public List<PLCNamespaceDeclSymbol> UsingNamespaceList { get; set; } = new List<PLCNamespaceDeclSymbol>();

        public List<PLCNamespaceDeclSymbol> GetNamespaces()
        {
            return UsingNamespaceList;
        }

        public void AddNameSpace(PLCNamespaceDeclSymbol namespaceDeclSymbol)
        {
            UsingNamespaceList.Add(namespaceDeclSymbol);
        }

        public override string ToString()
        {
            return $"PLCNamespaceDeclSymbol{{" +
                   $"UsingNamespaceList={UsingNamespaceList}," +
                   $"InitVar='{InitVar}'," +
                   $"VarSort={VarSort}," +
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

        public new JToken ToStringJson()
        {
            JObject jsonObject = new JObject();
            jsonObject["initVar"] = InitVar;
            string varSortName = VarSort != null ? VarSort.ToString() : "null";
            jsonObject["varSort"] = varSortName;
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            string sortName = Sort != null ? Sort.ToString() : "null";
            jsonObject["sort"] = sortName;
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;
            JArray jsonArray = new JArray();
            foreach (PLCNamespaceDeclSymbol plcNamespaceDeclSymbol in UsingNamespaceList)
            {
                jsonArray.Add(plcNamespaceDeclSymbol.ToStringJson());
            }
            jsonObject["usingNamespaceList"] = jsonArray;
            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCNamespaceDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }

    }
}
