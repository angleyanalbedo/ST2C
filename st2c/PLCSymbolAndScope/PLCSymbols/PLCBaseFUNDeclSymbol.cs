using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{


    public class PLCBaseFUNDeclSymbol : PLCImportScopeTypeDeclType, UsingNamespace, DeclareVariable
        {
            public PLCBaseFUNDeclSymbol(PLCBaseFUNDeclSymbol resource) : base(resource)
            {
                IfReturned = resource.IfReturned;
                ReturnTypeId = resource.ReturnTypeId;
                ReturnVarSort = resource.ReturnVarSort;
                foreach (var variable in resource.VariableMap)
                {
                    VariableMap[variable.Key] = variable.Value;
                }
                foreach (var accessVar in resource.AccessVars)
                {
                    AccessVars.Add(accessVar);
                }
                foreach (var namespaceSymbol in resource.Namespaces)
                {
                    Namespaces.Add(namespaceSymbol);
                }
            }

            public bool IfReturned { get; set; } = false;

            public int ReturnTypeId { get; set; } = -1;

            public PLCModifierEnum.Sort? ReturnVarSort { get; set; }

            public Dictionary<string, PLCVariable> VariableMap { get; set; } = new Dictionary<string, PLCVariable>();

            public List<PLCVariable> AccessVars { get; set; } = new List<PLCVariable>();

            public List<PLCNamespaceDeclSymbol> Namespaces { get; set; } = new List<PLCNamespaceDeclSymbol>();

            public PLCBaseFUNDeclSymbol()
            {
            }

            public PLCBaseFUNDeclSymbol(string name, int rowNum) : base(name, rowNum)
            {
            }

            public void AddAccessVar(PLCVariable var)
            {
                AccessVars.Add(var);
            }

            public PLCVariable GetAccessVar(string varName)
            {
                foreach (var accessVar in AccessVars)
                {
                    if (varName == accessVar.Name)
                    {
                        return accessVar;
                    }
                }
                return null;
            }

            public List<PLCVariable> GetAccessVars()
            {
                return AccessVars;
            }

            public int GetLessParamAmount()
            {
                int count = 0;
                foreach (var accessVar in AccessVars)
                {
                    if (string.IsNullOrEmpty(accessVar.GetAssignVar()))
                    {
                        count++;
                    }
                }
                return count;
            }

            public int GetMaxParamAmount()
            {
                return AccessVars.Count;
            }

            public bool CheckOverlap(PLCBaseFUNDeclSymbol another)
            {
                var anotherAccessVars = another.GetAccessVars();
                if (AccessVars.Count != anotherAccessVars.Count)
                {
                    return false;
                }

                var thisMethodAccessType = new List<int>();
                foreach (var accessVar in AccessVars)
                {
                    thisMethodAccessType.Add(accessVar.TypeId);
                }

                foreach (var accessVar in anotherAccessVars)
                {
                    var varTypeId = accessVar.TypeId;
                    if (!thisMethodAccessType.Contains(varTypeId))
                    {
                        return false;
                    }
                    thisMethodAccessType.Remove(varTypeId);
                }

                return thisMethodAccessType.Count == 0;
            }

            public void AddNameSpace(PLCNamespaceDeclSymbol namespaceDeclSymbol)
            {
                Namespaces.Add(namespaceDeclSymbol);
            }

            public List<PLCNamespaceDeclSymbol> GetNamespaces()
            {
                return Namespaces;
            }

            public string GetStdFunction()
            {
                var funcCall = new StringBuilder();
                funcCall.Append("::PLC::RFM->getSymbolByID<").Append(Name).Append("*>(").Append(SymbolId).Append(")->callFunc");
                return funcCall.ToString();
            }

            public override string ToString()
            {
                return $"PLCBaseFUNDeclSymbol{{" +
                       $"ReturnTypeId={ReturnTypeId}," +
                       $"ReturnVarSort={ReturnVarSort}," +
                       $"AccessVars={AccessVars}," +
                       $"Namespaces={Namespaces}," +
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

            public JToken ToStringJson()
            {
                var jsonObject = new JObject();
                jsonObject["returnTypeId"] = ReturnTypeId;
                jsonObject["returnVarSort"] = ReturnVarSort?.ToString() ?? "null";
                var accessVarArray = new JArray();
                foreach (var plcVariable in AccessVars)
                {
                    accessVarArray.Add(plcVariable.ToStringJson());
                }
                jsonObject["accessVars"] = accessVarArray;
                var namespaceDeclArray = new JArray();
                foreach (var plcNamespaceDeclSymbol in Namespaces)
                {
                    namespaceDeclArray.Add(plcNamespaceDeclSymbol.ToStringJson());
                }
                jsonObject["namespaces"] = namespaceDeclArray;
                jsonObject["initVar"] = InitVar;
                jsonObject["varSort"] = VarSort.ToString() ?? "null";
                jsonObject["symbolId"] = SymbolId;
                jsonObject["typeId"] = TypeId;
                jsonObject["name"] = Name;
                jsonObject["rowNum"] = RowNum;
                jsonObject["columnNum"] = ColumnNum;
                jsonObject["sort"] = Sort.ToString() ?? "null";
                jsonObject["runtimeName"] = RuntimeName;
                jsonObject["runtimeTypeName"] = RuntimeTypeName;
                var jsonSymbol = new JObject();
                jsonSymbol["PLCBaseFUNDeclSymbol"] = jsonObject;
                return jsonSymbol;
            }

            public Dictionary<string, PLCVariable> GetVariableMap()
            {
                return VariableMap;
            }

            public PLCVariable GetVariable(string name)
            {
                return VariableMap.TryGetValue(name, out var variable) ? variable : null;
            }

            public void AddVariable(PLCVariable var)
            {
                VariableMap[var.Name] = var;
            }

            public void AddAllVariable(Collection<PLCVariable> vars)
            {
                foreach (var var in vars)
                {
                    AddVariable(var);
                }
            }
        }

   
}
