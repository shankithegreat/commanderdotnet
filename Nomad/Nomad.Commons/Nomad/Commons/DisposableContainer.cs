namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;

    public class DisposableContainer : IDisposableContainer, IDisposable
    {
        private List<IDisposable> ItemList;

        public DisposableContainer()
        {
            this.ItemList = new List<IDisposable>();
        }

        public DisposableContainer(IEnumerable<IDisposable> items)
        {
            this.ItemList = new List<IDisposable>();
            this.AddRange(items);
        }

        public void Add(IDisposable item)
        {
            this.ItemList.Add(item);
        }

        public void AddRange(IEnumerable<IDisposable> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException();
            }
            foreach (IDisposable disposable in items)
            {
                this.ItemList.Add(disposable);
            }
        }

        public void Dispose()
        {
            if (this.ItemList != null)
            {
                foreach (IDisposable disposable in this.ItemList)
                {
                    disposable.Dispose();
                }
                this.ItemList = null;
            }
        }

        public void Remove(IDisposable item)
        {
            this.ItemList.Remove(item);
        }
    }
}

