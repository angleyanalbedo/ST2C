import { createToken,CstParser } from "chevrotain";
import { allTokens } from "../lexer/token.js";

class StParser extends CstParser{
    constructor() {
        super(allTokens);
        
        const $ = this;
        // 定义语法规则
        $.RULE("program", () => {
            $.MANY(() => {
                $.SUBRULE($.statement);
            });
        });
        $.RULE("statement", () => {
            $.OR([
                { ALT: () => $.SUBRULE($.ifStatement) },
                { ALT: () => $.SUBRULE($.forStatement) },
                { ALT: () => $.SUBRULE($.whileStatement) },
                { ALT: () => $.SUBRULE($.repeatStatement) },
                { ALT: () => $.SUBRULE($.returnStatement) },
                { ALT: () => $.SUBRULE($.variableDeclaration) }
            ]);
        });
        $.RULE("ifStatement", () => {
            $.CONSUME(allTokens.IF);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.THEN);
            $.MANY(() => {
                $.SUBRULE($.statement);
            });
            $.CONSUME(allTokens.END_IF);
        });
        $.RULE("forStatement", () => {
            $.CONSUME(allTokens.FOR);
            $.CONSUME(allTokens.Identifier);
            $.CONSUME(allTokens.Assign);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.TO);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.DO);
            $.MANY(() => {
                $.SUBRULE($.statement);
            });
            $.CONSUME(allTokens.END_FOR);
        });
        $.RULE("whileStatement", () => {
            $.CONSUME(allTokens.WHILE);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.DO);
            $.MANY(() => {
                $.SUBRULE($.statement);
            });
            $.CONSUME(allTokens.END_WHILE);
        });
        $.RULE("repeatStatement", () => {
            $.CONSUME(allTokens.REPEAT);
            $.MANY(() => {
                $.SUBRULE($.statement);
            });
            $.CONSUME(allTokens.UNTIL);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.END_REPEAT);
        });
        $.RULE("returnStatement", () => {
            $.CONSUME(allTokens.RETURN);
            $.SUBRULE($.expression);
        });
        $.RULE("variableDeclaration", () => {
            $.CONSUME(allTokens.VAR);
            $.CONSUME(allTokens.Identifier);
            $.CONSUME(allTokens.Assign);
            $.SUBRULE($.expression);
            $.CONSUME(allTokens.END_VAR);
        });
        $.RULE("expression", () => {
            // 这里可以定义更复杂的表达式解析规则
            $.OR([
                { ALT: () => $.CONSUME(allTokens.Identifier) },
                { ALT: () => $.CONSUME(allTokens.Integer) },
                { ALT: () => $.CONSUME(allTokens.Real) },
                { ALT: () => $.CONSUME(allTokens.StringLiteral) }
            ]);
        });

        this.performSelfAnalysis();
    }
}
export const stParser = new StParser();