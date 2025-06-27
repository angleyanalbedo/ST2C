using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public interface DeclareVariable
    {
        Dictionary<string, PLCVariable> GetVariableMap();

        PLCVariable GetVariable(string name);

        void AddVariable(PLCVariable var);

        void AddAllVariable(Collection<PLCVariable> vars);
    }
}
