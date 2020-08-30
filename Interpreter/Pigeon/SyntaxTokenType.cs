﻿using System.ComponentModel;

namespace Kostic017.Pigeon
{
    public enum SyntaxTokenType
    {
        EOF,
        Illegal,
        Comment,
        BlockComment,

        If,
        Else,
        For,
        To,
        Step,
        Do,
        While,
        Break,
        Continue,
        Return,
        Int,
        Float,
        Bool,
        String,
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

        ID,
        IntLiteral,
        FloatLiteral,
        BoolLiteral,
        StringLiteral,
    }
}