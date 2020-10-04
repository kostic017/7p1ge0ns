using System;

namespace Kostic017.Pigeon
{
    class InternalErrorException : Exception
    {
        public InternalErrorException(string message) : base(message)
        {
        }
    }
}
