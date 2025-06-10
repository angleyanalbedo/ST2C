import { lexer } from "./lexer/lexer.js";

const result = lexer.tokenize("IF THEN VAR x := 10; END_VAR END_IF");

console.log(result.tokens);