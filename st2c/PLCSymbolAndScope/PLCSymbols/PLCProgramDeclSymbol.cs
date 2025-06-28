using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static st2c.PLCSymbolAndScope.PLCSymbols.PLCModifierEnum;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCProgramDeclSymbol: PLCImportScopeTypeDeclType
    {
        public PLCProgramDeclSymbol():base()
        {
            Sort = PLCModifierEnum.Sort.PROGRAM_DECL;
            VarSort = PLCModifierEnum.Sort.PROGRAM;
        }

        public PLCProgramDeclSymbol(String name, int rowNum):base(name, rowNum)
        {
            Sort = PLCModifierEnum.Sort.PROGRAM_DECL;
            VarSort = PLCModifierEnum.Sort.PROGRAM;
        }

        public PLCProgramDeclSymbol(PLCProgramDeclSymbol resource):base(resource)
        {

        }

    }
}
