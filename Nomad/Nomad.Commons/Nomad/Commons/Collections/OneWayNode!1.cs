namespace Nomad.Commons.Collections
{
    using System;

    public sealed class OneWayNode<T>
    {
        internal OneWayNode<T> _Next;
        private T _Value;

        public OneWayNode<T> Next
        {
            get
            {
                return this._Next;
            }
        }

        public T Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }
    }
}

