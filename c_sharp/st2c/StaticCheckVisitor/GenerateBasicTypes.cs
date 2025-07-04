using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using st2c.PLCSymbolAndScope;
using st2c.PLCSymbolAndScope.PLCSymbols;
using st2c.PLCSymbolAndScope.PLCSymbolTables;

namespace st2c.staticCheckVisitor
{
    public class GenerateBasicTypes
    {
        public void Generate()
        {
            AddBasicSymbol(IDGenerator.BOOL, "BOOL", PLCModifierEnum.Sort.BOOL, "(*(new BOOL(false)))", "BOOL",
                new int[] { },
                new int[] { },
                new int[] { IDGenerator.BOOL },
                new int[] { IDGenerator.BOOL });

            AddBasicSymbol(IDGenerator.SINTID, "SINT", PLCModifierEnum.Sort.INT, "(*(new SINT(0)))", "SINT",
                new int[] { IDGenerator.SINTID, IDGenerator.INTID, IDGenerator.DINTID, IDGenerator.LINTID, IDGenerator.REAL },
                new int[] { IDGenerator.SINTID, IDGenerator.INTID, IDGenerator.DINTID, IDGenerator.LINTID, IDGenerator.REAL },
                new int[] { IDGenerator.SINTID, IDGenerator.INTID, IDGenerator.DINTID, IDGenerator.LINTID, IDGenerator.REAL },
                new int[] { IDGenerator.SINTID, IDGenerator.INTID, IDGenerator.DINTID, IDGenerator.LINTID, IDGenerator.REAL });

            // ... 其他AddBasicSymbol调用保持不变 ...

            AddBasicSymbol(IDGenerator.SSTRING, "STRING", PLCModifierEnum.Sort.STRING, "(*(new STRING(\"\")))", "STRING",
                new int[] { },
                new int[] { },
                new int[] { IDGenerator.SSTRING },
                new int[] { IDGenerator.SSTRING });
        }

        private void AddBasicSymbol(int id,
                                  string name,
                                  PLCModifierEnum.Sort sort,
                                  string initVar,
                                  string runtimeName,
                                  int[] calcList,
                                  int[] compareList,
                                  int[] equalList,
                                  int[] assignList)
        {
            PLCTypeDeclSymbol basicSymbol = new PLCTypeDeclSymbol(id, id, name);
            basicSymbol.SetSort(sort);
            basicSymbol.SetVarSort(sort);
            basicSymbol.SetInitVar(initVar);
            basicSymbol.SetRuntimeName(runtimeName);

            foreach (int typeId in calcList)
            {
                basicSymbol.AddCalculableType(typeId);
            }

            foreach (int typeId in compareList)
            {
                basicSymbol.AddComparableType(typeId);
            }

            foreach (int typeId in equalList)
            {
                basicSymbol.AddEqualType(typeId);
            }

            foreach (int typeId in assignList)
            {
                basicSymbol.AddAssignableType(typeId);
            }

            // 加入总符号表
            PLCTotalSymbolTable.AddSymbol(basicSymbol);
            PLCTotalSymbolTable.AddType(basicSymbol);

            // 加入基本符号表
            PLCScopeStack.BasicTypeTable.AddSymbol(basicSymbol);
        }
    }
    
}
