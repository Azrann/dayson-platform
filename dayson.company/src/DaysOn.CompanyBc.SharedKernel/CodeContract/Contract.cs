using System;

namespace DaysOn.CompanyBc.SharedKernel.CodeContract
{
    /// <summary>
    /// This is duplication of microsoft code contract concept in System.Diagnostics.Contracts namespace.
    /// At the moment we are only catering for pre-condition checks.
    /// http://devjourney.com/blog/code-contracts-part-2-preconditions/.
    /// </summary>
    public class Contract
    {
        public static void Requires<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        public static void Requires<TException>(Func<bool> assertion, string message) where TException : Exception
        {

            if (assertion())
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        public static void RequiresNotNull<T>(T arg, string message) where T : class
        {
            Requires<ArgumentNullException>(arg == null, message);
        }

        public static void Requires<TException>(bool assertion, params object[] args) where TException : Exception
        {
            if (assertion)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), args);
            }
        }


        public static void RequiresNonZero(int arg, string message)
        {
            Requires<ArgumentException>(arg == 0, message);
        }
    }
}