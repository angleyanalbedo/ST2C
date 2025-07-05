package staticCheckVisitor.strategy.variable_access;

import PLCException.PLCSemanticException;
import PLCSymbolAndScope.IDGenerator;
import PLCSymbolAndScope.PLCScopeStack;
import PLCSymbolAndScope.PLCSymbolTables.PLCSymbolTable;
import PLCSymbolAndScope.PLCSymbolTables.PLCTotalSymbolTable;
import PLCSymbolAndScope.PLCSymbols.*;
import antlr4.PLCSTPARSERParser;
import org.antlr.v4.runtime.ParserRuleContext;
import staticCheckVisitor.PLCVisitor;
import staticCheckVisitor.register.StrategyForVisit;
import staticCheckVisitor.strategy.Strategy;

import java.util.ArrayList;
import java.util.Objects;

import static PLCSymbolAndScope.PLCScopeStack.currentScope;
import static antlr4.PLCSTPARSERParser.RULE_symbolic_variable;

@StrategyForVisit(ruleIndex = RULE_symbolic_variable,branch = 0)
public class VisitThis_symbol implements Strategy {
    /**
     * ��ʱֻ֧����ͨ���������ڵ����������á��ṹ���Լ�������δʵ��
     * */
    @Override
    public ArrayList<PLCSymbol> invoke(ParserRuleContext parserCtx, PLCVisitor visitor) {
        PLCSTPARSERParser.ThisSymbolicContext ctx = (PLCSTPARSERParser.ThisSymbolicContext) parserCtx;
        //��ȡ��������
        String varName = ctx.identifier().getText();
        PLCSymbol tempFoundSymbol;
        if (ctx.getChild(0).getText().equals("THIS")) {//THIS.A, ��ʱ�ڵ�ǰ������ķ��ű�������
            tempFoundSymbol = PLCScopeStack.currentScope.getParentScope().shallowFindSymbol(varName);
        }
        else{ //A, ��ʱ�ڵ�ǰ�������ڽ���������
            //�ȼ���ǲ��Ƿ����ķ���ֵ
            PLCImportScopeTypeDeclType declSymbol = currentScope.getDeclSymbol();
            //����Ϊ������ֱ�ӷ���ֵ
            if(declSymbol instanceof PLCBaseFUNDeclSymbol funSymbol &&
                    funSymbol.getReturnTypeId() != -1 &&
                    varName.equals(declSymbol.getName())){
                int returnTypeId = funSymbol.getReturnTypeId();
                funSymbol.setIfReturned(true);
                tempFoundSymbol = new PLCVariable();
                tempFoundSymbol.setTypeId(returnTypeId);
                tempFoundSymbol.setName("*this->returnValue ");

            }else{
                PLCSymbol symbolItself = currentScope.deepFindSymbol(varName);
                if(symbolItself instanceof PLCVariable castedSymbol){
                    tempFoundSymbol = new PLCVariable(castedSymbol);
                    tempFoundSymbol.setName(symbolItself.getLocalScope() == currentScope ? "*"+varName : castedSymbol.getUniqueName());
                }else{
                    throw new PLCSemanticException("can not find variable named " + varName + " FROM : " + ctx.getText());
                }
            }
        }

        // '^'* ((array_index*)//ArrayList|(struct_variable*)//. var_access)�ķ���δ���
        return visitor.packSymbols(tempFoundSymbol);
    }
}
