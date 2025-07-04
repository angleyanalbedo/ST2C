using Antlr4.Runtime.Misc;
using st2c.PLCException;
using st2c.PLCSymbolAndScope;
using st2c.PLCSymbolAndScope.PLCScopes;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace st2c.staticCheckVisitor
{
    using System;
    using System.Collections.Generic;

    public class VisitorTool
    {
        private static readonly VisitorTool visitorTool = new VisitorTool();

        private VisitorTool()
        {
            // 注册可以进行加减乘除乘方取模运算的符号id
            RegisterCalculableType(IDGenerator.INTID);
            RegisterCalculableType(IDGenerator.SINTID);
            RegisterCalculableType(IDGenerator.DINTID);
            RegisterCalculableType(IDGenerator.LINTID);
        }

        // 可以进行计算的类型
        private readonly HashSet<int> calculableTypeIdSet = new HashSet<int>();

        [Obsolete]
        private void RegisterCalculableType(int typeId)
        {
            this.calculableTypeIdSet.Add(typeId);
        }

        /**
         * 使用PLCTypeDeclSymbol类下的检查方法
         */
        [Obsolete]
        public bool CheckIfCalculable(int type)
        {
            return !this.calculableTypeIdSet.Contains(type);
        }

        /**
         * 检查两个类型是否兼容((比较、赋值、表达式))(不处理类)
         * 使用PLCTypeDeclSymbol类下的检查方法
         */
        [Obsolete]
        public bool CheckCompatibility(int leftType, int rightType)
        {
            if (leftType == rightType)
            {
                return false;
            }
            else
            {
                return CheckIfCalculable(leftType) || CheckIfCalculable(rightType);
            }
        }

        /*-----------变量信息整合-------------------------------------------*/

        /**
         * @describe 将resource中收集到的变量信息(VarSections、AccessModifie、IfConst、RetainQualifiers)赋给target
         */
        public void SettleVarAttrs(PLCVariable resource, PLCVariable target)
        {
            target.SetVarSections(resource.GetVarSections());
            target.SetAccessModifier(resource.GetAccessModifier());
            target.SetIfConst(resource.GetIfConst());
            target.SetRetainQualifiers(resource.GetRetainQualifiers());
            // target.SetLocation(resource.GetLocation());
        }

        /**
         * @describe  在当前符号表内查询是否存在重名符号，若存在则抛出错误
         */
        public void CheckNameOnly(PLCSymbolTable checkSymbolTable, string symbolName)
        {
            try
            {
                PLCSymbol foundSymbol = checkSymbolTable.FindSymbol(symbolName);
                if (foundSymbol != null)
                {
                    throw new PLCSemanticException("duplication of name:" + symbolName +
                            " duplicate symbol info : " + foundSymbol);
                }
            }
            catch (PLCSemanticException e)
            {
                Console.Error.WriteLine(e.Message);
                throw;
            }
        }

        // 变量声明处理
        /**
         * @describe symbolTable唯一名称检查->将varinfo中的变量信息整合到var->为varList中的变量设置id->添加到符号表symbolTable以及总表
         */
        public void ProcessVariable(List<PLCSymbol> varList, PLCVariable infoVar, PLCSymbolTable symbolTable)
        {
            foreach (PLCSymbol symbol in varList)
            {
                // 名称检查
                string symbolName = symbol.Name;
                CheckNameOnly(symbolTable, symbolName);
                // 组装信息
                PLCVariable targetSymbol = (PLCVariable)symbol;
                SettleVarAttrs(infoVar, targetSymbol);
                // 分配符号id
                targetSymbol.SetSymbolId(IDGenerator.GetIDGenerator().NewSymbolId());
                // 加入当前符号表
                symbolTable.AddSymbol(targetSymbol);
                // 加入总表
                PLCTotalSymbolTable.AddSymbol(targetSymbol);
            }
        }

        /**
         * @describe 获得工具对象
         */
        public static VisitorTool GetTool()
        {
            return visitorTool;
        }

        /**
         * @describe 根据字符串获得访问类型
         */
        public PLCModifierEnum.AccessModifier GetAccessType(string accessContext)
        {
            switch (accessContext)
            {
                case "PRIVATE":
                    return PLCModifierEnum.AccessModifier.PRIVATE;
                case "PUBLIC":
                    return PLCModifierEnum.AccessModifier.PUBLIC;
                case "INTERNAL":
                    return PLCModifierEnum.AccessModifier.INTERNAL;
                case "PROTECTED":
                    return PLCModifierEnum.AccessModifier.PROTECTED;
                default:
                    throw new PLCSemanticException("Unknown access modifier: " + accessContext);
            }
        }

        /**
         * @describe 根据字符串获得保留类型
         */
        public PLCModifierEnum.RetainModifier GetRetain(string retainText)
        {
            switch (retainText)
            {
                case "RETAIN":
                    return PLCModifierEnum.RetainModifier.RETAIN;
                case "NON_RETAIN":
                    return PLCModifierEnum.RetainModifier.NON_RETAIN;
                default:
                    throw new PLCSemanticException("Unknown retain modifier: " + retainText);
            }
        }

        /**
         * @describe 依据名称序列寻找对应的命名空间作用域
         * 传入只有名称信息的namespace符号
         */
        public PLCScope FindNestedNameSpaceScope(List<PLCSymbol> nestNamespaceList)
        {
            // nestList为空，则返回null
            if (nestNamespaceList.Count == 0)
            {
                return null;
            }
            PLCScope searchingScope = PLCScopeStack.CurrentScope;
            PLCSymbol targetSymbol;
            foreach (PLCSymbol namespaceSymbol in nestNamespaceList)
            {
                targetSymbol = searchingScope.DeepFindSymbol(namespaceSymbol.Name, PLCModifierEnum.Sort.NAMESPACE_DECL);
                // 查找不到，返回null
                if (targetSymbol == null)
                {
                    return null;
                }
                PLCImportScopeTypeDeclType tempSymbol = (PLCImportScopeTypeDeclType)targetSymbol;
                searchingScope = tempSymbol.GetImportScope();
            }
            return searchingScope;
        }

        public PLCScope FindNameSpaceScopeByNames(List<string> nameSpaceNames)
        {
            if (nameSpaceNames.Count == 0)
            {
                return null;
            }
            PLCScope searchingScope = PLCScopeStack.CurrentScope;
            PLCSymbol targetSymbol;
            foreach (string npName in nameSpaceNames)
            {
                targetSymbol = searchingScope.DeepFindSymbol(npName, PLCModifierEnum.Sort.NAMESPACE_DECL);
                // 查找不到，返回null
                if (targetSymbol == null)
                {
                    return null;
                }
                PLCImportScopeTypeDeclType tempSymbol = (PLCImportScopeTypeDeclType)targetSymbol;
                searchingScope = tempSymbol.GetImportScope();
            }
            return searchingScope;
        }
    }
}
