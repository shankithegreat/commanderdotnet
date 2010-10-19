namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class EmptyEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        T IEnumerator<T>.Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
    }
}

