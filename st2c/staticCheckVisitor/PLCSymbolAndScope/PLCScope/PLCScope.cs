using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope
{
    

    internal class PLCScope
    {
        // 作用域定位
        public string ScopeLocation { get; set; } = "";

        // 引入此作用域的符号, 在入栈时自动设置
        private PLCImportScopeTypeDeclType declSymbol;

        // 该作用域的符号表, 在入栈时自动设置
        private PLCSymbolTable scopeSymbolTable;

        // 作用域唯一ID, 在构造方法内初始化
        private int scopeID;

        // 该作用域的父作用域, 在构造方法内初始化
        private PLCScope parentScope;

        // 当前作用域嵌套深度, 在构造方法内初始化
        private int scopeDepth;

        // 该作用域的子作用域, 无需特地进行初始化，在声明子作用域时会自动添加
        public List<PLCScope> ChildScopeList { get; } = new List<PLCScope>();

        // 该作用域使用的命名空间, 需要手动调用方法进行初始化
        protected List<PLCNamespaceDeclSymbol> UsingNSList { get; } = new List<PLCNamespaceDeclSymbol>();

        // 由使用命名空间引入的有效搜索域 = 使用的命名空间本身 + 命名空间的validScopeFromNamespace
        protected List<PLCScope> ValidScopeFromNamespace { get; } = new List<PLCScope>();

        // 由父作用域引入的有效搜索域 = 父作用域本身 + 父作用域的validScopeFromParents + 父作用域的validScopeFromNamespace
        protected List<PLCScope> ValidScopeFromParents { get; } = new List<PLCScope>();

        // 去除 scope list 中的重复元素
        private List<PLCScope> RemoveDuplication(List<PLCScope> scopeList)
        {
            return scopeList.Distinct().ToList();
        }

        public PLCScope()
        {
            this.scopeID = IDGenerator.IDGenerator.NewScopeId();
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

                // 添加有效作用域, 并去除重复
                this.ValidScopeFromParents.Add(this.parentScope);
                this.ValidScopeFromParents.AddRange(this.parentScope.ValidScopeFromParents);
                this.ValidScopeFromParents.AddRange(this.parentScope.ValidScopeFromNamespace);
                this.ValidScopeFromParents = RemoveDuplication(this.ValidScopeFromParents);
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
        public void AddUsingNamespace(PLCNamespaceDeclSymbol namespace)
    {
        this.ValidScopeFromNamespace.Add(namespace.GetImportScope());
        this.ValidScopeFromNamespace.AddRange(namespace.GetImportScope().ValidScopeFromNamespace);
        this.ValidScopeFromNamespace = RemoveDuplication(this.ValidScopeFromNamespace);
        this.UsingNSList.Add(namespace);
    }

    //********************************** 搜索方法 ***********************************************

    // 浅搜索
    // 仅仅在此作用域的符号表搜索符号
    public PLCSymbol ShallowFindSymbol(string name)
    {
        return this.scopeSymbolTable.FindSymbol(name);
    }

    public PLCSymbol ShallowFindSymbol(string name, Sort sort)
    {
        return this.scopeSymbolTable.FindSymbol(name, sort);
    }

    // 深搜索
    // 浅搜索 -> 搜索 validScopeFromParents -> 搜索 validScopeFromNamespace
    public PLCSymbol DeepFindSymbol(string name)
    {
        // 浅搜索
        PLCSymbol result = this.ShallowFindSymbol(name);
        if (result != null)
        {
            return result;
        }

        // 搜索 validScopeFromParents
        foreach (PLCScope scope in this.ValidScopeFromParents)
        {
            result = scope.ShallowFindSymbol(name);
            if (result != null)
            {
                return result;
            }
        }

        // 搜索 validScopeFromNamespace
        foreach (PLCScope scope in this.ValidScopeFromNamespace)
        {
            result = scope.ShallowFindSymbol(name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    // 返回所有同名的符号的数组，按照查找到的顺序排列
    public List<PLCSymbol> DeepFindAllSymbols(string name)
    {
        List<PLCSymbol> symbolArrayList = new List<PLCSymbol>();
        // 浅搜索
        PLCSymbol result = this.ShallowFindSymbol(name);
        if (result != null)
        {
            symbolArrayList.Add(result);
        }

        // 搜索 validScopeFromParents
        foreach (PLCScope scope in this.ValidScopeFromParents)
        {
            result = scope.ShallowFindSymbol(name);
            if (result != null)
            {
                symbolArrayList.Add(result);
            }
        }

        // 搜索 validScopeFromNamespace
        foreach (PLCScope scope in this.ValidScopeFromNamespace)
        {
            result = scope.ShallowFindSymbol(name);
            if (result != null)
            {
                symbolArrayList.Add(result);
            }
        }
        return symbolArrayList;
    }

    // 由特定条件搜索符号
    public PLCSymbol DeepFindSymbol(string name, Sort sort)
    {
        List<PLCSymbol> symbolArrayList = this.DeepFindAllSymbols(name);
        foreach (PLCSymbol symbol in symbolArrayList)
        {
            if (symbol.GetSort() == sort)
            {
                return symbol;
            }
        }
        return null;
    }

    //********************************* 以下为 set、get 方法 **************************************

    // 引入此作用域的符号的获得和设置方法
    public void SetDeclSymbol(PLCImportScopeTypeDeclType declSymbol)
    {
        this.declSymbol = declSymbol;
    }

    public PLCImportScopeTypeDeclType GetDeclSymbol()
    {
        return declSymbol;
    }

    // 该作用域符号表的设置和获得方法
    public void SetScopeSymbolTable(PLCSymbolTable scopeSymbolTable)
    {
        this.scopeSymbolTable = scopeSymbolTable;
    }

    public PLCSymbolTable GetScopeSymbolTable()
    {
        return this.scopeSymbolTable;
    }

    // 嵌套深度的设置、获取方法
    public void SetScopeDepth(int scopeDepth)
    {
        this.scopeDepth = scopeDepth;
    }

    public int GetScopeDepth()
    {
        return scopeDepth;
    }

    // 作用域唯一ID的设置、获取方法
    public void SetScopeID(int scopeID)
    {
        this.scopeID = scopeID;
    }

    public int GetScopeID()
    {
        return scopeID;
    }

    // 父作用域
    public void SetParentScope(PLCScope parentScope)
    {
        this.parentScope = parentScope;
    }

    public PLCScope GetParentScope()
    {
        return parentScope;
    }

    // 拷贝父作用域的命名空间
    private void CopyUsingNSList(PLCScope another)
    {
        this.UsingNSList.AddRange(another.UsingNSList);
    }

    public override string ToString()
    {
        return $"Declare symbol: {this.declSymbol.Name}\n" +
               $"Scope ID: {this.scopeID}\n" +
               $"Scope depth: {this.scopeDepth}\n" +
               $"Scope location: {this.scopeLocation}\n";
    }
    }
    }
