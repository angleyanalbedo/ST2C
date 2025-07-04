using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
  
    
    public class PLCMethodDeclSymbol : PLCBaseFUNDeclSymbol
    {
        // 方法的修饰符
        public PLCModifierEnum.ClassModifier MethodModifier { get; set; } = PLCModifierEnum.ClassModifier.NONE;

        public void SetMethodModifier(string methodModifier)
        {
            MethodModifier = Enum.Parse<PLCModifierEnum.ClassModifier>(methodModifier);
        }

        public PLCModifierEnum.ClassModifier GetMethodModifier()
        {
            return MethodModifier;
        }

        public bool GetIfAbstract()
        {
            return MethodModifier == PLCModifierEnum.ClassModifier.ABSTRACT;
        }

        // 访问修饰符
        public PLCModifierEnum.AccessModifier AccessModifier { get; set; } = PLCModifierEnum.AccessModifier.PROTECTED;

        public void SetAccessModifier(string accessModifier)
        {
            AccessModifier = Enum.Parse<PLCModifierEnum.AccessModifier>(accessModifier);
        }

        public bool IsIfOverride()
        {
            return IfOverride;
        }

        public void SetIfOverride(bool ifOverride)
        {
            IfOverride = ifOverride;
        }

        // 是否重写
        private bool IfOverride { get; set; }

        // 重写的符号 如果实现困难就不做这个了
        public PLCMethodDeclSymbol OverrideMethod { get; set; }

        public PLCMethodDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.METHOD_DECL;
        }

        public PLCMethodDeclSymbol(string name, int rowNum) : base(name, rowNum)
        {
            base.Sort = PLCModifierEnum.Sort.METHOD_DECL;
        }

        public PLCMethodDeclSymbol(PLCMethodDeclSymbol resource) : base(resource)
        {
            MethodModifier = resource.MethodModifier;
            AccessModifier = resource.AccessModifier;
            IfOverride = resource.IfOverride;
            OverrideMethod = resource.OverrideMethod;
        }

        public override string ToString()
        {
            return $"PLCMethodDeclSymbol{{" +
                   $"MethodModifier={MethodModifier}," +
                   $"AccessModifier={AccessModifier}," +
                   $"IfOverride={IfOverride}," +
                   $"OverrideMethod={OverrideMethod}," +
                   $"ReturnTypeId={ReturnTypeId}," +
                   $"ReturnVarSort={ReturnVarSort}," +
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
                   $"RuntimeTypeName='{RuntimeTypeName}'," +
                   $"AccessVars='{AccessVars}'" +
                   $"}}";
        }

        public new JToken ToStringJson()
        {
            JObject jsonObject = new JObject();
            jsonObject["methodModifier"] = MethodModifier.ToString();
            jsonObject["accessModifier"] = AccessModifier.ToString();
            jsonObject["ifOverride"] = IfOverride;
            jsonObject["overrideMethod"] = OverrideMethod != null ? OverrideMethod.ToStringJson() : null;
            jsonObject["returnTypeId"] = ReturnTypeId;
            jsonObject["returnVarSort"] = ReturnVarSort.ToString();
            JArray namespaceDeclArray = new JArray();
            foreach (PLCNamespaceDeclSymbol plcNamespaceDeclSymbol in Namespaces)
            {
                namespaceDeclArray.Add(plcNamespaceDeclSymbol.ToStringJson());
            }
            jsonObject["namespaces"] = namespaceDeclArray;
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
            JArray accessVarArray = new JArray();
            foreach (PLCVariable plcVariable in AccessVars)
            {
                accessVarArray.Add(plcVariable.ToStringJson());
            }
            jsonObject["accessVars"] = accessVarArray;
            JObject jsonSymbol = new JObject();
            jsonSymbol["PLCMethodDeclSymbol"] = jsonObject;
            return jsonSymbol;
        }

       
    }

    
}
