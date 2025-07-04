using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using st2c.PLCSymbolAndScope;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.StaticCheckVisitor.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor
{
    public class PLCVisitor : PLCSTPARSERBaseVisitor<List<PLCSymbol>>
    {
        
        public VisitorTool visitorTool;
        public ParseTreeProperty<List<PLCSymbol>> properties;
        private Factory factory;
        public PLCVisitor(ParseTreeProperty<List<PLCSymbol>> properties) : base()
        {

            PLCScopeStack.StackInit();
            new GenerateBasicTypes().Generate();
            visitorTool = VisitorTool.GetTool();
            this.properties = properties;
            this.factory = Factory.GetFactory();
        }
        //-----------------------工具方法-------------------------------------

        /// <summary>
        /// 返回一串拥有名称信息的符号
        /// 用于namespace(.namespace)*,获得namespace list
        /// </summary>
        public List<PLCSymbol> GetNameSpaceList(List<PLCSTPARSERParser.Namespace_nameContext> nameList)
        {
            List<PLCSymbol> nameSpaceList = new List<PLCSymbol>();
            //获得命名空间列表nameSpaceList
            foreach (var nameContext in nameList)
            {
                PLCNamespaceDeclSymbol namespaceDeclSymbol = new PLCNamespaceDeclSymbol();
                namespaceDeclSymbol.SetName(nameContext.GetText());
                nameSpaceList.Add(namespaceDeclSymbol);
            }
            return nameSpaceList;
        }

        //将类型打包返回
        public List<PLCSymbol> PackSymbols(params PLCSymbol[] symbols)
        {
            return new List<PLCSymbol>(symbols);
        }

        
    }
}
