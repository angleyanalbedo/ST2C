import { createToken, Lexer, CstParser } from 'chevrotain';
import { allTokens } from './lexer/token.js';
import { stParser } from './parser/paser.js';
// 定义语法规则

function parseInput(text) {
  const lexer = new Lexer(allTokens);
  const lexingResult = lexer.tokenize(text);
  // "input" is a setter which will reset the parser's state.
  stParser.input = lexingResult.tokens;
  stParser.selectStatement();

  if (parser.errors.length > 0) {
    throw new Error("sad sad panda, Parsing errors detected");
  }
}

const inputText = "VAR x := 10; END_VAR FOR i := 1 TO 10 DO x := x + i; END_FOR RETURN x; END_VAR";
parseInput(inputText);
