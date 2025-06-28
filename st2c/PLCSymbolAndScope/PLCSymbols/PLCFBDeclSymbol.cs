using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{

    public class PLCFBDeclSymbol : PLCBaseClassDeclSymbol
    {
        //返回值声明标识符，根据先后顺序排队
        public List<int> ReturnVars { get; } = new List<int>();

        //入参类型id，根据先后顺序排队
        public List<int> InVars { get; } = new List<int>();

        //出入参符号，根据先后顺序排队
        public List<int> InOutVars { get; } = new List<int>();

        public PLCFBDeclSymbol() : base()
        {
            Sort = PLCModifierEnum.Sort.FB_DECL;
            VarSort = PLCModifierEnum.Sort.FB;
        }

        public PLCFBDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            Sort = PLCModifierEnum.Sort.FB_DECL;
            VarSort = PLCModifierEnum.Sort.FB;
        }

        public PLCFBDeclSymbol(PLCFBDeclSymbol resource) : base(resource)
        {
        }

        public void AddReturnVar(int returnVarId)
        {
            ReturnVars.Add(returnVarId);
        }

        public void AddInVar(int inVarId)
        {
            InVars.Add(inVarId);
        }

        public void AddInOutVar(int inOutVarId)
        {
            InOutVars.Add(inOutVarId);
        }

        public override string ToString()
        {
            return $"PLCFBDeclSymbol{{ReturnVars={ReturnVars}, InVars={InVars}, InOutVars={InOutVars}, " +
                   $"Interfaces={Interfaces}, Namespaces={Namespaces}, BaseClass={BaseClass?.Name}, " +
                   $"ClassModifier={ClassModifier}, InitVar='{InitVar}', VarSort={VarSort}, " +
                   $"SymbolId={SymbolId}, TypeId={TypeId}, Name='{Name}', RowNum={RowNum}, " +
                   $"ColumnNum={ColumnNum}, Sort={Sort}, RuntimeName='{RuntimeName}', " +
                   $"RuntimeTypeName='{RuntimeTypeName}'}}";
        }

        public new JObject ToStringJson()
        {
            var jsonObject = new JObject();

            // Return vars
            var returnVarsArray = new JArray();
            foreach (var returnVar in ReturnVars)
            {
                returnVarsArray.Add(returnVar);
            }
            jsonObject["returnVars"] = returnVarsArray;

            // In vars
            var inVarsArray = new JArray();
            foreach (var inVar in InVars)
            {
                inVarsArray.Add(inVar);
            }
            jsonObject["inVars"] = inVarsArray;

            // InOut vars
            var inOutVarsArray = new JArray();
            foreach (var inOutVar in InOutVars)
            {
                inOutVarsArray.Add(inOutVar);
            }
            jsonObject["inOutVars"] = inOutVarsArray;

            // Interfaces
            var interfacesArray = new JArray();
            foreach (var plcInterfaceDeclSymbol in Interfaces)
            {
                interfacesArray.Add(plcInterfaceDeclSymbol.ToStringJson());
            }
            jsonObject["interfaces"] = interfacesArray;

            // Namespaces
            var namespacesArray = new JArray();
            foreach (var plcNamespaceDeclSymbol in Namespaces)
            {
                namespacesArray.Add(plcNamespaceDeclSymbol.ToStringJson());
            }
            jsonObject["namespaces"] = namespacesArray;

            // Other properties
            jsonObject["baseClass"] = BaseClass?.Name;
            jsonObject["classModifier"] = ClassModifier.ToString();
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
            jsonSymbol["PLCImportScopeTypeDeclType"] = jsonObject;
            return jsonSymbol;
        }
    }
}
