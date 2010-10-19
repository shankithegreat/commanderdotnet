namespace Nomad.Commons
{
    using System;

    [Serializable]
    public abstract class BasicFilter
    {
        protected BasicFilter()
        {
        }

        public abstract bool EqualTo(object obj);
    }
}

