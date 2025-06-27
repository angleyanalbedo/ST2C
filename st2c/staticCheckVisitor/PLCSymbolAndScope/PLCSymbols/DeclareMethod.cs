using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    internal interface DeclareMethod
    {
        void AddMethod(PLCMethodDeclSymbol method);

        void AddAllMethods(ArrayList<PLCMethodDeclSymbol> methods);

        ArrayList<PLCMethodDeclSymbol> GetMethods();
    }

}
