package staticCheckVisitor.strategy.type_access;

import PLCException.PLCSemanticException;
import PLCSymbolAndScope.PLCScope.PLCScope;
import PLCSymbolAndScope.PLCScopeStack;
import PLCSymbolAndScope.PLCSymbols.PLCSymbol;
import PLCSymbolAndScope.PLCSymbols.PLCTypeDeclSymbol;
import antlr4.PLCSTPARSERParser;
import org.antlr.v4.runtime.ParserRuleContext;
import staticCheckVisitor.PLCVisitor;
import staticCheckVisitor.register.StrategyForVisit;
import staticCheckVisitor.strategy.Strategy;

import java.util.ArrayList;

import static antlr4.PLCSTPARSERParser.RULE_instance_name;
@StrategyForVisit(ruleIndex = RULE_instance_name)
public class VisitInstance_name implements Strategy {
    @Override
    public ArrayList<PLCSymbol> invoke(ParserRuleContext parserCtx, PLCVisitor visitor) {
        PLCSTPARSERParser.Instance_nameContext ctx = (PLCSTPARSERParser.Instance_nameContext) parserCtx;
        //��ȡ�����ռ�������
        ArrayList<String> nameList = new ArrayList<>();
        for (PLCSTPARSERParser.Namespace_nameContext nameContext : ctx.namespace_name()) {
            nameList.add(nameContext.getText());
        }

        PLCScope npScope;
        if (nameList.isEmpty()) {//��ǰ������
            npScope = PLCScopeStack.currentScope;
        } else {
            npScope = visitor.visitorTool.findNameSpaceScopeByNames(nameList);
            try {
                if (npScope == null) {
                    throw new PLCSemanticException("can not find scope: " + ctx.getText());
                }
            } catch (PLCSemanticException e) {
                System.err.println(e.getMessage());
                System.exit(-1);
            }
        }

        //��ȡ��������
        String typeName = ctx.class_name().getText();
        PLCTypeDeclSymbol basicType = (PLCTypeDeclSymbol) npScope.deepFindSymbol(typeName);
        if (basicType == null) {
            throw new PLCSemanticException("\ncan not find type : " + typeName);
        }
        //������󣬽�����װ
        PLCTypeDeclSymbol targetVar = new PLCTypeDeclSymbol(basicType);
        //����runtimeName
        StringBuilder runtimeName = new StringBuilder();
        for (String name : nameList) {
            runtimeName.append(name).append(".");
        }
        runtimeName.append(basicType.getRuntimeName());
        targetVar.setRuntimeTypeName(new String(runtimeName));

        //�������
        return visitor.packSymbols(targetVar);
    }
}
