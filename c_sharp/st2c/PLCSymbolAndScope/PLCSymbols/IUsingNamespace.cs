using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public interface IUsingNamespace
    {
        List<PLCNamespaceDeclSymbol> GetNamespaces();

        void AddNameSpace(PLCNamespaceDeclSymbol namespaceDeclSymbol);
    }
}
