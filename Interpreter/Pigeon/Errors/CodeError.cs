﻿namespace Kostic017.Pigeon.Errors
{
    public class CodeError
    {
        public string Message { get; }
        public TextSpan TextSpan { get; }

        internal CodeError(string message, TextSpan textSpan)
        {
            Message = message;
            TextSpan = textSpan;
        }

        public override string ToString() => $"{TextSpan.Line}:{TextSpan.Column} {Message}";
    }
}
