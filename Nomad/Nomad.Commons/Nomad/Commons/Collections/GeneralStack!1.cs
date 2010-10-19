namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class GeneralStack<T> : Stack<T>, IStack<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        public GeneralStack()
        {
        }

        public GeneralStack(IEnumerable<T> collection) : base(collection)
        {
        }

        public GeneralStack(int capacity) : base(capacity)
        {
        }

        void IStack<T>.Clear()
        {
            base.Clear();
        }

        T IStack<T>.Peek()
        {
            return base.Peek();
        }

        T IStack<T>.Pop()
        {
            return base.Pop();
        }

        void IStack<T>.Push(T local1)
        {
            base.Push(local1);
        }
    }
}

