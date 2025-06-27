using st2c.PLCException;
using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols;
using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope
{
    public static class PLCScopeStack
    {
        // 全局作用域,在创建实例后初始化
        public static readonly PLCScope GlobalScope = new PLCScope();

        // 全局的符号表
        public static readonly PLCSymbolTable GlobalSymbolTable = new PLCSymbolTable();

        // 当前作用域的引用，初始为null
        public static PLCScope CurrentScope = null;

        // 当前符号表的引用，初始为null
        public static PLCSymbolTable CurrentSymbolTable = null;

        // 基本类型符号表 查询基本类型用
        public static PLCSymbolTable BasicTypeTable = new PLCSymbolTable();

        // 作用域栈
        private static readonly Stack<PLCScope> _scopeStack = new Stack<PLCScope>();

        // 初始化全局变量
        public static void StackInit()
        {
            // 全局符号
            PLCImportScopeTypeDeclType globalSymbol = new PLCImportScopeTypeDeclType();
            globalSymbol.SetName("GLOBAL");

            // 压栈&设置引用关系
            Push(globalSymbol, GlobalSymbolTable, GlobalScope);
            PLCTotalSymbolTable.AddScope(GlobalScope);
            PLCTotalSymbolTable.AddTable(GlobalSymbolTable);
        }

        // 设置符号、符号表、作用域之间的引用关系
        private static void SetReference(PLCImportScopeTypeDeclType symbol, PLCSymbolTable table, PLCScope scope)
        {
            scope.SetDeclSymbol(symbol);
            scope.SetScopeSymbolTable(table);
            table.SetSrcSymbol(symbol);
            table.SetTableScope(scope);
            symbol.SetImportSymbolTable(table);
            symbol.SetImportScope(scope);
        }

        // 根据符号更新栈的内容
        public static void Push(PLCImportScopeTypeDeclType symbol)
        {
            PLCScope newScope;
            PLCSymbolTable newTable;

            if (symbol.GetImportScope() == null)
            {
                // 作用域和符号表在函数内创建,并设置引用关系
                newScope = new PLCScope();
                newTable = new PLCSymbolTable();
                SetReference(symbol, newTable, newScope);

                // 将符号添加到栈顶作用域符号表中
                symbol.LocalScope.GetScopeSymbolTable().AddSymbol(symbol);
            }
            else
            {
                newScope = symbol.GetImportScope();
                newTable = symbol.GetImportSymbolTable();
            }

            if (newScope.GetScopeDepth() >= 1)
            {
                string instanceName;
                string name = symbol.GetName();
                PLCModifierEnum.Sort sort = symbol.GetSort();

                switch (sort)
                {
                    case PLCModifierEnum.Sort.METHOD_DECL:
                        instanceName = "meth" + name;
                        break;
                    case PLCModifierEnum.Sort.FC_DECL:
                        instanceName = "func" + name;
                        break;
                    default:
                        instanceName = name;
                        break;
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(newScope.GetParentScope().GetScopeLocation())
                            .Append("::")
                            .Append(instanceName);
                newScope.SetScopeLocation(stringBuilder.ToString());
            }

            _scopeStack.Push(newScope);
            // 修改当前作用域
            CurrentScope = newScope;
            CurrentSymbolTable = newTable;
        }

        // 使用指定的符号、作用域和符号表向栈内添加新的符号
        public static void Push(PLCImportScopeTypeDeclType symbol, PLCSymbolTable table, PLCScope scope)
        {
            // 设置引用关系
            SetReference(symbol, table, scope);
            _scopeStack.Push(scope);
            // 修改当前作用域
            CurrentScope = scope;
            CurrentSymbolTable = table;
        }

        // 弹出栈顶元素并修改CurrentScope
        public static PLCScope Pop()
        {
            try
            {
                if (CurrentScope == GlobalScope)
                {
                    throw new PLCSemanticException("pop global scope");
                }
            }
            catch (PLCSemanticException e)
            {
                Console.WriteLine(e.Message);
            }

            PLCScope tempScope = _scopeStack.Pop();
            // 修改CurrentScope
            CurrentScope = _scopeStack.Peek();
            CurrentSymbolTable = CurrentScope.GetScopeSymbolTable();
            return tempScope;
        }

        // 返回栈顶元素
        public static PLCScope Top()
        {
            return _scopeStack.Peek();
        }
    }
}
