import {Lexer} from "chevrotain";
import {allTokens} from "./token.js";
import { stParser } from "../parser/paser.js";



//   Identifier, Integer, Real,
export const lexer = new Lexer(allTokens);
