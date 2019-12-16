using System;

namespace WarWolfWorks_Mod.Internal.Debugging.Exceptions
{
    public sealed class SingleCallException : Exception
    {
        private string actMess;
        public override string Message => actMess;

        public SingleCallException(object singlecallViolator) : base()
        {
            actMess = $"{singlecallViolator.ToString()} is meant to be set only once, but was set twice or more.";
        }

        public SingleCallException() : base()
        {
            actMess = $"An object meant to be set only once was set over the threshold";
        }
    }
}
