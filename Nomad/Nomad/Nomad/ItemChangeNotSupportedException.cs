namespace Nomad
{
    using System;

    public class ItemChangeNotSupportedException : NotSupportedException
    {
        public ItemChangeNotSupportedException() : base("Item does not support modification logic")
        {
        }

        public ItemChangeNotSupportedException(string message) : base(message)
        {
        }
    }
}

