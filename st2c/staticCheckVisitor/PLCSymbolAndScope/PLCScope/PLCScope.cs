using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols;
using st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbolTables;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;
using static System.Formats.Asn1.AsnWriter;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope
{
    public class PLCScope
    {
        // 作用域定位
        public string ScopeLocation { get; set; } = "";

        // 引入此作用域的符号, 在入栈时自动设置
        private PLCImportScopeTypeDeclType declSymbol;

        // 该作用域的符号表, 在入栈时自动设置
        private PLCSymbolTable scopeSymbolTable;

        // 作用域唯一ID,在构造方法内初始化
        private int scopeID;

        // 该作用域的父作用域,在构造方法内初始化
        private PLCScope parentScope;

        // 当前作用域嵌套深度,在构造方法内初始化
        private int scopeDepth;

        // 该作用域的子作用域,无需特地进行初始化，在声明子作用域时会自动添加
        public List<PLCScope> ChildScopeList = new List<PLCScope>();

        /**
         * 该作用域使用的命名空间, 需要手动调用方法进行初始化
         * ----------子作用域会自动继承父作用域的using namespace------------------------
         */
        protected List<PLCNamespaceDeclSymbol> usingNSList = new List<PLCNamespaceDeclSymbol>();

        /**
         * 由使用命名空间引入的有效搜索域 = 使用的命名空间本身 + 命名空间的validScopeFromNamespace
         * */
        protected List<PLCScope> validScopeFromNamespace = new List<PLCScope>();

        /**
         * 由父作用域引入的有效搜索域 = 父作用域本身 + 父作用域的validScopeFromParents + 父作用域的validScopeFromNamespace
         * */
        protected List<PLCScope> validScopeFromParents = new List<PLCScope>();

        // 去除scope list中的重复元素
        private List<PLCScope> RemoveDuplication(List<PLCScope> scopeList)
        {
            return new List<PLCScope>(new HashSet<PLCScope>(scopeList));
        }

        public PLCScope()
        {
            this.scopeID = IDGenerator.GetIDGenerator().NewScopeId();
            PLCScope tempParentScope = PLCScopeStack.CurrentScope;

            // 全局作用域没有父作用域, 与普通作用域区分对待
            if (tempParentScope != null)
            {
                scopeDepth = tempParentScope.GetScopeDepth() + 1;

                // 设置父子关系
                this.parentScope = PLCScopeStack.CurrentScope;
                PLCScopeStack.CurrentScope.AddChildScope(this);

                // 拷贝父作用域的命名空间
                this.CopyUsingNSList(this.parentScope);

                // 添加有效作用域,并去除重复
                this.validScopeFromParents.Add(this.parentScope);
                this.validScopeFromParents.AddRange(this.parentScope.validScopeFromParents);
                this.validScopeFromParents.AddRange(this.parentScope.validScopeFromNamespace);
                this.validScopeFromParents = RemoveDuplication(this.validScopeFromParents);
            }
            else
            {
                scopeDepth = 0;
            }
        }

        // 添加子作用域
        private void AddChildScope(PLCScope scope)
        {
            this.ChildScopeList.Add(scope);
        }

        // 添加命名空间
        public void AddUsingNamespace(PLCNamespaceDeclSymbol namespaceSymbol)
        {
            this.validScopeFromNamespace.Add(namespaceSymbol.GetImportScope());
            this.validScopeFromNamespace.AddRange(namespaceSymbol.GetImportScope().validScopeFromNamespace);
            this.validScopeFromNamespace = RemoveDuplication(this.validScopeFromNamespace);
            this.usingNSList.Add(namespaceSymbol);
        }

        // **********************************搜素方法***********************************************

        /**
         * 浅搜索
         * 仅仅在此作用域的符号表搜索符号
         * */
        public PLCSymbol ShallowFindSymbol(string name)
        {
            return this.scopeSymbolTable.FindSymbol(name);
        }

        public PLCSymbol ShallowFindSymbol(string name, Sort sort)
        {
            return this.scopeSymbolTable.FindSymbol(name, sort);
        }

        /**
         * 名称：深搜索
         * 浅搜索 -> 搜索validScopeFromParents -> 搜索validScopeFromNamespace
         * */
        public PLCSymbol DeepFindSymbol(string name)
        {
            // 浅搜索
            PLCSymbol result = this.ShallowFindSymbol(name);
            if (result != null)
            {
                return result;
            }

            // 搜索validScopeFromParents
            foreach (PLCScope scope in this.validScopeFromParents)
            {
                result = scope.ShallowFindSymbol(name);
                if (result != null)
                {
                    return result;
                }
            }

            // 搜索搜索validScopeFromNamespace
            foreach (PLCScope scope in this.validScopeFromNamespace)
            {
                result = scope.ShallowFindSymbol(name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /**
         * 返回所有同名的符号的数组，按照查找到的顺序排放
         */
        public List<PLCSymbol> DeepFindAllSymbols(string name)
        {
            List<PLCSymbol> symbolList = new List<PLCSymbol>();
            // 浅搜索
            PLCSymbol result = this.ShallowFindSymbol(name);
            if (result != null)
            {
                symbolList.Add(result);
            }

            // 搜索validScopeFromParents
            foreach (PLCScope scope in this.validScopeFromParents)
            {
                result = scope.ShallowFindSymbol(name);
                if (result != null)
                {
                    symbolList.Add(result);
                }
            }

            // 搜索搜索validScopeFromNamespace
            foreach (PLCScope scope in this.validScopeFromNamespace)
            {
                result = scope.ShallowFindSymbol(name);
                if (result != null)
                {
                    symbolList.Add(result);
                }
            }
            return symbolList;
        }

        /**
         * 由特定条件搜索符号
         * */
        public PLCSymbol DeepFindSymbol(string name, Sort sort)
        {
            List<PLCSymbol> symbolList = this.DeepFindAllSymbols(name);
            foreach (PLCSymbol symbol in symbolList)
            {
                if (symbol.GetSort() == sort)
                {
                    return symbol;
                }
            }
            return null;
        }

        // *********************************以下为set、get方法**************************************

        // 引入此作用域的符号的获得和设置方法
        public PLCImportScopeTypeDeclType GetDeclSymbol()
        {
            return declSymbol;
        }

        public void SetDeclSymbol(PLCImportScopeTypeDeclType declSymbol)
        {
            this.declSymbol = declSymbol;
        }

        // 该作用域符号表的设置和获得方法
        public PLCSymbolTable GetScopeSymbolTable()
        {
            return this.scopeSymbolTable;
        }

        public void SetScopeSymbolTable(PLCSymbolTable scopeSymbolTable)
        {
            this.scopeSymbolTable = scopeSymbolTable;
        }

        // 嵌套深度的设置、获取方法
        public int GetScopeDepth()
        {
            return scopeDepth;
        }

        public void SetScopeDepth(int scopeDepth)
        {
            this.scopeDepth = scopeDepth;
        }

        // 作用域唯一ID的设置、获取方法
        public int GetScopeID()
        {
            return scopeID;
        }

        public void SetScopeID(int scopeID)
        {
            this.scopeID = scopeID;
        }

        // 父作用域
        public PLCScope GetParentScope()
        {
            return parentScope;
        }

        public void SetParentScope(PLCScope parentScope)
        {
            this.parentScope = parentScope;
        }

        // 拷贝父作用域的命名空间
        private void CopyUsingNSList(PLCScope another)
        {
            this.usingNSList.AddRange(another.usingNSList);
        }

        public override string ToString()
        {
            return $"Declare symbol: {this.declSymbol.Name}\n" +
                   $"Scope ID: {this.scopeID}\n" +
                   $"Scope depth: {this.scopeDepth}\n" +
                   $"Scope location: {this.ScopeLocation}\n";
        }
    }
}