using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
    public class PLCRefDeclSymbol : PLCTypeDeclSymbol
    {
        public PLCRefDeclSymbol()
        {
            base.Sort = PLCModifierEnum.Sort.REF_DECL;
            VarSort = PLCModifierEnum.Sort.REF;
        }

        public PLCRefDeclSymbol(PLCRefDeclSymbol resource) : base(resource)
        {
            ReferredTypeId = resource.ReferredTypeId;
            base.Sort = PLCModifierEnum.Sort.REF_DECL;
            VarSort = PLCModifierEnum.Sort.REF;
        }

        public int ReferredTypeId { get; set; }

        private int referredTypeId
        {
            get { return ReferredTypeId; }
            set { ReferredTypeId = value; }
        }
    }

}
