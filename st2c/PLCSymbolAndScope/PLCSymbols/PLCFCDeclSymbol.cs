using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static st2c.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCFCDeclSymbol: PLCBaseFUNDeclSymbol
    {
        public PLCFCDeclSymbol()
        {
            
            Sort = PLCModifierEnum.Sort.FC_DECL;
        }
        public PLCFCDeclSymbol(String name, int rowNum): base(name, rowNum)
        {
            
            Sort = PLCModifierEnum.Sort.FC_DECL;
        }

        public PLCFCDeclSymbol(PLCFCDeclSymbol resource): base(resource)
        {
            
        }
    }
}
