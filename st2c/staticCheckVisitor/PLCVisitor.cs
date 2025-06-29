using Antlr4.Runtime.Tree;
using st2c.PLCSymbolAndScope.PLCSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor
{
    public class PLCVisitor : PLCSTPARSERBaseVisitor<List<PLCSymbol>>
    {
        private ParseTreeProperty<List<PLCSymbol>> property;

        public PLCVisitor(ParseTreeProperty<List<PLCSymbol>> property)
        {
            this.property = property;
        }

        internal void visit(object parseTree)
        {
            throw new NotImplementedException();
        }
    }
}
