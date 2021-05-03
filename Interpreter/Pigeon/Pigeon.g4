grammar Pigeon;

program : (varDecl | functionDecl)+ EOF ;
functionDecl : TYPE ID '(' functionParams? ')' stmtBlock ;
functionParams : TYPE ID (',' TYPE ID)* ;
functionCall : ID '(' exprList? ')' ;

varDecl
    : TYPE ID ('=' expr)? ';'
    | 'const' TYPE ID '=' expr ';'
    ;

varAssign
    : variable op='=' expr ';'
    | variable op='+=' expr ';'
    | variable op='-=' expr ';'
    | variable op='*=' expr ';'
    | variable op='/=' expr ';'
    | variable op='%=' expr ';'
    | variable op='^=' expr ';'
    ;

variable
    : ID
    | ID '[' expr ']'
    ;

stmt
    : varDecl                                          # variableDeclaration
    | varAssign                                        # variableAsignment
    | functionCall ';'                                 # functionCallStatement
    | 'return' expr ';'                                # returnStatement
    | 'break' ';'                                      # breakStatement
    | 'continue' ';'                                   # continueStatement
    | 'while' expr stmtBlock                           # whileStatement
    | 'do' stmtBlock 'while' expr                      # doWhileStatement
    | 'if' expr stmtBlock ('else' stmtBlock)?          # ifStatement
    | 'for' ID '=' expr ('to'|'downto') expr stmtBlock # forStatement
    ;

stmtBlock
    : ';'
    | stmt
    | '{' stmt* '}'
    ;

expr
    : BOOL                                # boolLiteral
    | NUMBER                              # numberLiteral
    | STRING                              # stringLiteral
    | variable                            # variableExpression
    | '(' expr ')'                        # parenthesizedExpression
    | functionCall                        # functionCallExpression
    | op='-' expr                         # unaryExpression
    | op='+' expr                         # unaryExpression
    | op='!' expr                         # unaryExpression
    |<assoc=right> expr '^' expr          # binaryExpression
    | expr op='%' expr                    # binaryExpression
    | expr op='/' expr                    # binaryExpression
    | expr op='*' expr                    # binaryExpression
    | expr op='+' expr                    # binaryExpression
    | expr op='-' expr                    # binaryExpression
    | expr op='<=' expr                   # binaryExpression
    | expr op='>=' expr                   # binaryExpression
    | expr op='<' expr                    # binaryExpression
    | expr op='>' expr                    # binaryExpression
    | expr op='==' expr                   # binaryExpression
    | expr op='!=' expr                   # binaryExpression
    | expr op='&&' expr                   # binaryExpression
    | expr op='||' expr                   # binaryExpression
    |<assoc=right> expr '?' expr ':' expr # ternaryExpression
    ;

exprList : expr (',' expr)* ;

COMMENT : ('//' .*? '\r'? '\n' | '/*' .*? '*/') -> channel(HIDDEN) ;

STRING : '"' (ESCAPE|.)*? '"' ;

BOOL
    : 'true'
    | 'false'
    ;

TYPE
    : 'int'
    | 'float'
    | 'string'
    | 'bool'
    | 'void'
    ;

NUMBER
    : DIGIT+
    | '.' DIGIT+
    | DIGIT+ '.' DIGIT*
    ;

ID : ('_'|LETTER)('_'|LETTER|DIGIT)* ;
WHITESPACE : [ \r\n\t]+ -> skip ;

fragment
DIGIT : [0-9] ;

fragment
LETTER : [a-zA-Z] ;

fragment
ESCAPE
    : '\\"'
    | '\\n'
    | '\\t'
    | '\\\\'
    ;