using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using st2c.PLCSymbolAndScope.PLCSymbols;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCClassDeclSymbol : PLCBaseClassDeclSymbol
    {

        public PLCClassDeclSymbol():base()
        {
            // Removed invalid 'base()' call
            Sort = PLCModifierEnum.Sort.CLASS_DECL;
            VarSort = PLCModifierEnum.Sort.CLASS;
            this.RuntimeName = "";
        }


        public PLCClassDeclSymbol(PLCClassDeclSymbol resource) : base(resource) { }

    }
}
