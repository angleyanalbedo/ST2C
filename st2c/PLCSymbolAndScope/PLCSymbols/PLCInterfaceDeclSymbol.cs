namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class PLCInterfaceDeclSymbol : PLCImportScopeTypeDeclType, AbstractMethod, UsingNamespace
    {
        //接口继承的接口
        public List<PLCInterfaceDeclSymbol> BaseInterfaces { get; } = new List<PLCInterfaceDeclSymbol>();

        //接口使用的命名空间
        private List<PLCNamespaceDeclSymbol> _namespaceList = new List<PLCNamespaceDeclSymbol>();

        //待实现的方法 = 父类的abstract方法和实现的接口的所有方法
        private List<PLCMethodDeclSymbol> _abstractMethods = new List<PLCMethodDeclSymbol>();

        public PLCInterfaceDeclSymbol() : base()
        {
            Sort = PLCModifierEnum.Sort.INTERFACE_DECL;
        }

        public PLCInterfaceDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            Sort = PLCModifierEnum.Sort.INTERFACE_DECL;
        }

        public PLCInterfaceDeclSymbol(PLCInterfaceDeclSymbol resource) : base(resource)
        {
        }

        private List<string> GetBaseInterfacesName()
        {
            var names = new List<string>();
            foreach (var baseInterface in BaseInterfaces)
            {
                names.Add(baseInterface.Name);
            }
            return names;
        }

        public void AddInterface(PLCInterfaceDeclSymbol interfaceDeclSymbol)
        {
            BaseInterfaces.Add(interfaceDeclSymbol);
        }

        public void AddBaseInterfaces(PLCInterfaceDeclSymbol interfaceDeclSymbol)
        {
            BaseInterfaces.Add(interfaceDeclSymbol);
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

        public List<PLCNamespaceDeclSymbol> GetNamespaces()
        {
            return _namespaceList;
        }

        public void AddNameSpace(PLCNamespaceDeclSymbol namespaceDeclSymbol)
        {
            _namespaceList.Add(namespaceDeclSymbol);
        }

        public override string ToString()
        {
            return $"PLCInterfaceDeclSymbol{{NamespaceList={_namespaceList}, AbstractMethods={_abstractMethods}, " +
                   $"InitVar='{InitVar}', VarSort={VarSort}, SymbolId={SymbolId}, TypeId={TypeId}, " +
                   $"Name='{Name}', RowNum={RowNum}, ColumnNum={ColumnNum}, Sort={Sort}, " +
                   $"RuntimeName='{RuntimeName}', RuntimeTypeName='{RuntimeTypeName}'}}";
        }

        public new JObject ToStringJson()
        {
            var jsonObject = new JObject();

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

            var namespaceDeclArray = new JArray();
            foreach (var plcNamespaceDeclSymbol in _namespaceList)
            {
                namespaceDeclArray.Add(plcNamespaceDeclSymbol.ToStringJson());
            }
            jsonObject["namespaceList"] = namespaceDeclArray;

            var methodDeclArray = new JArray();
            foreach (var plcMethodDeclSymbol in _abstractMethods)
            {
                methodDeclArray.Add(plcMethodDeclSymbol.ToStringJson());
            }
            jsonObject["abstractMethods"] = methodDeclArray;

            var jsonSymbol = new JObject();
            jsonSymbol["PLCInterfaceDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }
    }
}