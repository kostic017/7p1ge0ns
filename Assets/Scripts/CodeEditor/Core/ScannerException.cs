using System;

class ScannerException : Exception
{
    public int Line { get; }
    public int Column { get; }

    public ScannerException(string message, int line, int column) : base($"{message} at {line}:{column}")
    {
        Line = line;
        Column = column;
    }
}
