import {Lexer} from "chevrotain";
import {allTokens} from "./token.js";

//   Identifier, Integer, Real,
export const lexer = new Lexer(allTokens);
