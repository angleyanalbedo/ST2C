using Newtonsoft.Json.Linq;
using st2c.PLCException;
using st2c.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static st2c.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{

   

    public class PLCBaseClassDeclSymbol : PLCImportScopeTypeDeclType, AbstractMethod, UsingNamespace, DeclareVariable, DeclareMethod
    {
        public List<PLCInterfaceDeclSymbol> Interfaces { get; } = new List<PLCInterfaceDeclSymbol>();
        public List<PLCNamespaceDeclSymbol> Namespaces { get; } = new List<PLCNamespaceDeclSymbol>();
        private readonly Dictionary<string, PLCVariable> VariableMap = new Dictionary<string, PLCVariable>();
        private readonly List<PLCMethodDeclSymbol> AbstractMethods = new List<PLCMethodDeclSymbol>();
        private readonly List<PLCMethodDeclSymbol> MethodMap = new List<PLCMethodDeclSymbol>();
        protected PLCBaseClassDeclSymbol BaseClass;

        public PLCModifierEnum.ClassModifier ClassModifier { get; set; }

        public PLCBaseClassDeclSymbol(PLCBaseClassDeclSymbol resource) : base(resource)
        {
        }

        public PLCBaseClassDeclSymbol() : base()
        {
        }

        public PLCBaseClassDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
        }

        public PLCBaseClassDeclSymbol GetBaseClass()
        {
            return BaseClass;
        }

        public void SetBaseClass(PLCBaseClassDeclSymbol baseClass)
        {
            BaseClass = baseClass;
        }

        public void SetClassModifier(string classModifier)
        {
            ClassModifier = (PLCModifierEnum.ClassModifier)Enum.Parse(typeof(PLCModifierEnum.ClassModifier), classModifier);
        }

        public void AddInterface(PLCInterfaceDeclSymbol interfaz)
        {
            Interfaces.Add(interfaz);
        }

        public PLCBaseFUNDeclSymbol FindMethod(string name)
        {
            foreach (var methodDeclSymbol in MethodMap)
            {
                if (name.Equals(methodDeclSymbol.Name))
                {
                    return methodDeclSymbol;
                }
            }
            return null;
        }

        public PLCBaseFUNDeclSymbol FindMethod(string name, List<PLCVariable> parameters)
        {
            var paramsNameList = new List<string>();
            foreach (var param in parameters)
            {
                paramsNameList.Add(param.Name);
            }

            var sameNamedMethods = ImportSymbolTable.FindSameNamedSymbol(name);
            foreach (var method in sameNamedMethods)
            {
                if (method.GetSort() != PLCModifierEnum.Sort.METHOD_DECL)
                {
                    continue;
                }

                var function = (PLCBaseFUNDeclSymbol)method;

                bool flag = false;
                foreach (var param in parameters)
                {
                    string paramName = param.Name;
                    if (paramName == null)
                    {
                        throw new PLCSemanticException("不支持不正规调用");
                    }

                    var accessVar = function.GetAccessVar(paramName);
                    if (accessVar == null)
                    {
                        flag = true;
                        break;
                    }

                    var paramSection = param.VarSections;
                    if (accessVar.VarSections != paramSection)
                    {
                        flag = true;
                        break;
                    }

                    var varType = PLCTotalSymbolTable.GetTypeByTypeID(accessVar.TypeId);
                    if (!varType.CheckCanAssignWith(param.TypeId))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    continue;
                }

                flag = true;
                var accessVars = function.GetAccessVars();
                foreach (var accessVar in accessVars)
                {
                    string accessVarName = accessVar.Name;
                    if (!paramsNameList.Contains(accessVarName) && accessVar.GetAssignVar() == null)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    return function;
                }
            }
            return null;
        }

        public Dictionary<string, PLCVariable> GetVariableMap()
        {
            return VariableMap;
        }

        public PLCVariable GetVariable(string name)
        {
            return VariableMap.ContainsKey(name) ? VariableMap[name] : null;
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

        public void AddNameSpace(PLCNamespaceDeclSymbol namespaceDeclSymbol)
        {
            Namespaces.Add(namespaceDeclSymbol);
        }

        public void AddAbstractMethod(PLCMethodDeclSymbol method)
        {
            AbstractMethods.Add(method);
        }

        public void AddAllAbsMethods(List<PLCMethodDeclSymbol> methods)
        {
            AbstractMethods.AddRange(methods);
        }

        public List<PLCMethodDeclSymbol> GetAbstractMethods()
        {
            return AbstractMethods;
        }

        public void AddMethod(PLCMethodDeclSymbol method)
        {
            MethodMap.Add(method);
        }

        public void AddAllMethods(List<PLCMethodDeclSymbol> methods)
        {
            MethodMap.AddRange(methods);
        }

        public List<PLCMethodDeclSymbol> GetMethods()
        {
            return MethodMap;
        }

        public override string ToString()
        {
            return $"PLCBaseClassDeclSymbol{{Interfaces={Interfaces}, Namespaces={Namespaces}, AbstractMethods={AbstractMethods}, " +
                   $"BaseClass={BaseClass}, ClassModifier={ClassModifier}, InitVar='{InitVar}', VarSort={VarSort}, " +
                   $"SymbolId={SymbolId}, TypeId={TypeId}, Name='{Name}', RowNum={RowNum}, ColumnNum={ColumnNum}, " +
                   $"Sort={Sort}, RuntimeName='{RuntimeName}', RuntimeTypeName='{RuntimeTypeName}'}}";
        }

        public new JObject ToStringJson()
        {
            var jsonObject = new JObject();

            var interfaceArray = new JArray();
            foreach (var plcInterfaceDeclSymbol in Interfaces)
            {
                interfaceArray.Add(plcInterfaceDeclSymbol.ToStringJson());
            }
            jsonObject["interfaces"] = interfaceArray;

            var namespaceDeclArray = new JArray();
            foreach (var plcNamespaceDeclSymbol in Namespaces)
            {
                namespaceDeclArray.Add(plcNamespaceDeclSymbol.ToStringJson());
            }
            jsonObject["namespaces"] = namespaceDeclArray;

            var methodDeclArray = new JArray();
            foreach (var plcMethodDeclSymbol in AbstractMethods)
            {
                methodDeclArray.Add(plcMethodDeclSymbol.ToStringJson());
            }
            jsonObject["abstractMethods"] = methodDeclArray;

            jsonObject["baseClass"] = BaseClass != null ? BaseClass.ToStringJson() : null;
            jsonObject["classModifier"] = ClassModifier != null ? ClassModifier.ToString() : null;
            jsonObject["initVar"] = InitVar;
            jsonObject["varSort"] = VarSort != null ? VarSort.ToString() : null;
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort != null ? Sort.ToString() : null;
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCBaseClassDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }

        public List<PLCNamespaceDeclSymbol> GetNamespaces()
        {
            return Namespaces;
        }
    }

}
