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
    : expr '=' expr ';'
    | expr '+=' expr ';'
    | expr '-=' expr ';'
    | expr '*=' expr ';'
    | expr '/=' expr ';'
    | expr '%=' expr ';'
    | expr '^=' expr ';'
    ;

stmt
    : varDecl                                             # variableDeclaration
    | varAssign                                           # variableAsignment
    | functionCall ';'                                     # functionCallStatement
    | 'return' expr ';'                                    # returnStatement
    | 'break' ';'                                          # breakStatement
    | 'continue' ';'                                       # continueStatement
    | 'while' expr stmtBlock                               # whileStatement
    | 'do' stmtBlock 'while' expr                          # doWhileStatement
    | 'if' expr stmtBlock ('else' stmtBlock)?              # ifStatement
    | 'for' ID '=' expr ('to'|'downto') expr stmtBlock     # forStatement
    ;

stmtBlock
    : ';'
    | stmt
    | '{' stmt* '}'
    ;

expr
    : ID                                      # variableReference
    | BOOL                                    # boolLiteral
    | NUMBER                                  # numberLiteral
    | STRING                                  # stringLiteral
    | '(' expr ')'                            # parenthesizedExpression
    | ID '[' expr ']'                         # arrayReference
    | functionCall                            # functionCallExpression
    | op='-' expr                             # unaryExpression
    | op='+' expr                             # unaryExpression
    | op='!' expr                             # unaryExpression
    |<assoc=right> expr '^' expr              # binaryExpression
    | expr op='%' expr                        # binaryExpression
    | expr op='/' expr                        # binaryExpression
    | expr op='*' expr                        # binaryExpression
    | expr op='+' expr                        # binaryExpression
    | expr op='-' expr                        # binaryExpression
    | expr op='<=' expr                       # binaryExpression
    | expr op='>=' expr                       # binaryExpression
    | expr op='<' expr                        # binaryExpression
    | expr op='>' expr                        # binaryExpression
    | expr op='==' expr                       # binaryExpression
    | expr op='!=' expr                       # binaryExpression
    | expr op='&&' expr                       # binaryExpression
    | expr op='||' expr                       # binaryExpression
    | expr '?' expr ':' expr                  # ternaryExpression
    ;

exprList : expr (',' expr)* ;

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

STRING : '"' ('\\"'|.)*? '"' ;

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