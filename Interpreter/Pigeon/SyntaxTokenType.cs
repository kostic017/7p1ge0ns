using System.ComponentModel;

namespace Kostic017.Pigeon
{
    public enum SyntaxTokenType
    {
        EOF,
        Illegal,
        Comment,
        BlockComment,

        [Description("if")]
        If,

        [Description("else")]
        Else,

        [Description("for")]
        For,

        [Description("to")]
        To,

        [Description("step")]
        Step,

        [Description("do")]
        Do,

        [Description("while")]
        While,

        [Description("break")]
        Break,

        [Description("continue")]
        Continue,

        [Description("return")]
        Return,

        [Description("int")]
        Int,

        [Description("float")]
        Float,

        [Description("bool")]
        Bool,

        [Description("string")]
        String,
        
        [Description("void")]
        Void,

        [Description("(")]
        LPar,

        [Description(")")]
        RPar,

        [Description("{")]
        LBrace,
        
        [Description("}")]
        RBrace,
        
        [Description("+")]
        Plus,

        [Description("+=")]
        PlusEq,

        [Description("++")]
        Inc,

        [Description("-")]
        Minus,

        [Description("-=")]
        MinusEq,

        [Description("--")]
        Dec,

        [Description("*")]
        Mul,

        [Description("*=")]
        MulEq,

        [Description("/")]
        Div,

        [Description("/=")]
        DivEq,

        [Description("%")]
        Mod,

        [Description("%=")]
        ModEq,

        [Description("^")]
        Power,

        [Description("^=")]
        PowerEq,

        [Description("!")]
        Not,

        [Description("&&")]
        And,

        [Description("||")]
        Or,

        [Description("<")]
        Lt,

        [Description(">")]
        Gt,

        [Description("<=")]
        Leq,

        [Description(">=")]
        Geq,

        [Description("==")]
        Eq,

        [Description("!=")]
        Neq,

        [Description("=")]
        Assign,

        [Description("?")]
        QMark,

        [Description(":")]
        Colon,

        [Description(",")]
        Comma,

        [Description(";")]
        Semicolon,

        [Description("identifier")]
        ID,

        [Description("integer literal")]
        IntLiteral,

        [Description("float literal")]
        FloatLiteral,

        [Description("bool literal")]
        BoolLiteral,

        [Description("string literal")]
        StringLiteral,
    }
}
