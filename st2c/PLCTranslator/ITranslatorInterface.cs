using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.PLCTranslator
{
    public interface TranslatorInterface<T>
    {
        T TranslateNode(PLCSTPARSERParser ctx, PLCTranslatorNew translatorNew);

    }

}
