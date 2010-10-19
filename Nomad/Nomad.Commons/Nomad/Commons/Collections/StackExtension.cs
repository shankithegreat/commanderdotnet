namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class StackExtension
    {
        public static void PushCollection<TSource>(this IStack<TSource> stack, IEnumerable<TSource> collection)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            foreach (TSource local in collection)
            {
                stack.Push(local);
            }
        }
    }
}

