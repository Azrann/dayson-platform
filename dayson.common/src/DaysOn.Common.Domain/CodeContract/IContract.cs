using System;

namespace DaysOn.Common.Domain.CodeContract
{
    public interface IContract
    {
        void Requires<TException>(bool assertion, string message) where TException : Exception;
        void Requires<TException>(Func<bool> assertion, string message) where TException : Exception;
    }
}
