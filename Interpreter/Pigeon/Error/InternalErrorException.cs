using System;

namespace Kostic017.Pigeon.Error
{
    class InternalErrorException : Exception
    {
        internal InternalErrorException(string message) : base(message)
        {
        }
    }
}
