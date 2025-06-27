using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.staticCheckVisitor.PLCSymbolAndScope.PLCSymbols
{
    public interface DeclareVariable
    {
        Dictionary<String, PLCVariable> GetVariableMap();

        PLCVariable GetVariable(String name);

        void AddVariable(PLCVariable var);

        void AddAllVariable(Collection<PLCVariable> vars);
    }
}
