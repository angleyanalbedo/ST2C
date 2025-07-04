using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public interface IAbstractMethod
    {
        void AddAbstractMethod(PLCMethodDeclSymbol method);

        void AddAllAbsMethods(List<PLCMethodDeclSymbol> methods);

        List<PLCMethodDeclSymbol> GetAbstractMethods();
    }

}
