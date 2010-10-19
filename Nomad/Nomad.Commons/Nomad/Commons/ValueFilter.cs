namespace Nomad.Commons
{
    using System;

    [Serializable]
    public abstract class ValueFilter : BasicFilter
    {
        protected ValueFilter()
        {
        }

        public abstract bool MatchValue(object value);
    }
}

