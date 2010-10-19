namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IStack<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        void Clear();
        T Peek();
        T Pop();
        void Push(T value);
    }
}

