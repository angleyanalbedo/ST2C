using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.staticCheckVisitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.StaticCheckVisitor.strategys
{
    public interface IStrategy
    {
        ArrayList<PLCSymbol> Invoke(ParserRuleContext parserCtx, PLCVisitor visitor);
    }
}
