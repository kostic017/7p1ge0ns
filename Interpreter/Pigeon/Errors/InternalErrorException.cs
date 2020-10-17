using System;

namespace Kostic017.Pigeon.Errors
{
    class InternalErrorException : Exception
    {
        internal InternalErrorException(string message) : base(message)
        {
        }
    }
}
