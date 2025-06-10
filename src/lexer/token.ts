import { createToken, Lexer } from 'chevrotain';

// 关键字
export const IF = createToken({ name: "IF", pattern: /IF/i });
export const THEN = createToken({ name: "THEN", pattern: /THEN/i });
export const ELSE = createToken({ name: "ELSE", pattern: /ELSE/i });
export const END_IF = createToken({ name: "END_IF", pattern: /END_IF/i });
export const VAR = createToken({ name: "VAR", pattern: /VAR/i });
export const END_VAR = createToken({ name: "END_VAR", pattern: /END_VAR/i });
export const FOR = createToken({ name: "FOR", pattern: /FOR/i });
export const TO = createToken({ name: "TO", pattern: /TO/i });
export const DO = createToken({ name: "DO", pattern: /DO/i });
export const END_FOR = createToken({ name: "END_FOR", pattern: /END_FOR/i });
export const WHILE = createToken({ name: "WHILE", pattern: /WHILE/i });
export const END_WHILE = createToken({ name: "END_WHILE", pattern: /END_WHILE/i });
export const REPEAT = createToken({ name: "REPEAT", pattern: /REPEAT/i });
export const UNTIL = createToken({ name: "UNTIL", pattern: /UNTIL/i });
export const END_REPEAT = createToken({ name: "END_REPEAT", pattern: /END_REPEAT/i });
export const RETURN = createToken({ name: "RETURN", pattern: /RETURN/i });

// 运算符
export const Assign = createToken({ name: "Assign", pattern: /:=/ });
export const Plus = createToken({ name: "Plus", pattern: /\+/ });
export const Minus = createToken({ name: "Minus", pattern: /-/ });
export const Mult = createToken({ name: "Mult", pattern: /\*/ });
export const Div = createToken({ name: "Div", pattern: /\// });
export const Eq = createToken({ name: "Eq", pattern: /=/ });
export const Neq = createToken({ name: "Neq", pattern: /<>/ });
export const Leq = createToken({ name: "Leq", pattern: /<=/ });
export const Lt = createToken({ name: "Lt", pattern: /</ });
export const Geq = createToken({ name: "Geq", pattern: />=/ });
export const Gt = createToken({ name: "Gt", pattern: />/ });



// 标识符和常量
export const Identifier = createToken({ name: "Identifier", pattern: /[a-zA-Z_][a-zA-Z0-9_]*/ });
export const Integer = createToken({ name: "Integer", pattern: /\d+/ });
export const Real = createToken({ name: "Real", pattern: /\d+\.\d+/ });

// 符号
export const LParen = createToken({ name: "LParen", pattern: /\(/ });
export const RParen = createToken({ name: "RParen", pattern: /\)/ });
export const Colon = createToken({ name: "Colon", pattern: /:/ });
export const Semicolon = createToken({ name: "Semicolon", pattern: /;/ });
export const Comma = createToken({ name: "Comma", pattern: /,/ });

// 空白
export const WhiteSpace = createToken({
  name: "WhiteSpace",
  pattern: /\s+/,
  group: Lexer.SKIPPED
});

// 导出 token 列表
export const allTokens = [
  WhiteSpace,
  IF, THEN, ELSE, END_IF, VAR, END_VAR,
  FOR, TO, DO, END_FOR, WHILE, END_WHILE,
  REPEAT, UNTIL, END_REPEAT, RETURN,
  Assign, Plus, Minus, Mult, Div,
  Eq, Neq, Leq, Geq, Lt, Gt,
  LParen, RParen, Colon, Semicolon, Comma,
  Real, Integer, Identifier
];