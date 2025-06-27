using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    public interface AbstractMethod
    {
        void AddAbstractMethod(PLCMethodDeclSymbol method);

        void AddAllAbsMethods(ArrayList<PLCMethodDeclSymbol> methods);

        ArrayList<PLCMethodDeclSymbol> GetAbstractMethods();
    }

}
