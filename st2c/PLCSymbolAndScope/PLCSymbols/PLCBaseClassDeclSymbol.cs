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
        private readonly Dictionary<string, PLCVariable> _variableMap = new Dictionary<string, PLCVariable>();
        private readonly List<PLCMethodDeclSymbol> _abstractMethods = new List<PLCMethodDeclSymbol>();
        private readonly List<PLCMethodDeclSymbol> _methodMap = new List<PLCMethodDeclSymbol>();
        protected PLCBaseClassDeclSymbol _baseClass;

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
            return _baseClass;
        }

        public void SetBaseClass(PLCBaseClassDeclSymbol baseClass)
        {
            _baseClass = baseClass;
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
            foreach (var methodDeclSymbol in _methodMap)
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
            return _variableMap;
        }

        public PLCVariable GetVariable(string name)
        {
            return _variableMap.ContainsKey(name) ? _variableMap[name] : null;
        }

        public void AddVariable(PLCVariable var)
        {
            _variableMap[var.Name] = var;
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
            _abstractMethods.Add(method);
        }

        public void AddAllAbsMethods(List<PLCMethodDeclSymbol> methods)
        {
            _abstractMethods.AddRange(methods);
        }

        public List<PLCMethodDeclSymbol> GetAbstractMethods()
        {
            return _abstractMethods;
        }

        public void AddMethod(PLCMethodDeclSymbol method)
        {
            _methodMap.Add(method);
        }

        public void AddAllMethods(List<PLCMethodDeclSymbol> methods)
        {
            _methodMap.AddRange(methods);
        }

        public List<PLCMethodDeclSymbol> GetMethods()
        {
            return _methodMap;
        }

        public override string ToString()
        {
            return $"PLCBaseClassDeclSymbol{{Interfaces={Interfaces}, Namespaces={Namespaces}, AbstractMethods={_abstractMethods}, " +
                   $"BaseClass={_baseClass}, ClassModifier={ClassModifier}, InitVar='{InitVar}', VarSort={VarSort}, " +
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
            foreach (var plcMethodDeclSymbol in _abstractMethods)
            {
                methodDeclArray.Add(plcMethodDeclSymbol.ToStringJson());
            }
            jsonObject["abstractMethods"] = methodDeclArray;

            jsonObject["baseClass"] = _baseClass != null ? _baseClass.ToStringJson() : null;
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
