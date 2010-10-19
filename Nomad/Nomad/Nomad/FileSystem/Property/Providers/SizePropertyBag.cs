namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.Commons.Threading;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class SizePropertyBag : CustomPropertyProvider, IGetVirtualProperty
    {
        private static VirtualPropertySet AvailableSet = new VirtualPropertySet(new int[] { 3, 5 });
        private bool CalculateNeeded = true;
        private static WorkQueue CalculateQueue = new ThreadPoolQueue();
        private long CompressedSize;
        private AsyncCompletedEventHandler FCompleted;
        private IVirtualFolder FOwner;
        private IVirtualCachedFolder FOwnerParent;
        private long Size;

        public SizePropertyBag(IVirtualFolder owner, AsyncCompletedEventHandler completed)
        {
            if (owner is ICloneable)
            {
                this.FOwner = (IVirtualFolder) ((ICloneable) owner).Clone();
            }
            else
            {
                this.FOwner = owner;
            }
            this.FOwnerParent = owner.Parent as IVirtualCachedFolder;
            this.FCompleted = completed;
        }

        private void CalculateFolderSize(object value)
        {
            Stack<IVirtualFolder> stack = new Stack<IVirtualFolder>();
            stack.Push(this.FOwner);
            Exception error = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                while (stack.Count > 0)
                {
                    using (IVirtualFolder folder = stack.Pop())
                    {
                        foreach (IVirtualItem item in folder.GetContent())
                        {
                            if (item is IVirtualFolder)
                            {
                                stack.Push((IVirtualFolder) item);
                            }
                            else
                            {
                                object obj2 = item[3];
                                if (obj2 != null)
                                {
                                    this.Size += (long) obj2;
                                }
                                obj2 = item[5];
                                if (obj2 != null)
                                {
                                    this.CompressedSize += Convert.ToInt64(obj2);
                                }
                                if ((this.FOwnerParent != null) && (stopwatch.ElapsedMilliseconds >= 500L))
                                {
                                    stopwatch.Reset();
                                    this.FOwnerParent.RaiseChanged(WatcherChangeTypes.Changed, this.FOwner);
                                    stopwatch.Start();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                error = exception2;
            }
            finally
            {
                stopwatch.Stop();
                if (this.FOwnerParent != null)
                {
                    this.FOwnerParent.RaiseChanged(WatcherChangeTypes.Changed, this.FOwner);
                }
                this.FOwner = null;
                this.FOwnerParent = null;
                if (this.FCompleted != null)
                {
                    this.FCompleted(this, new AsyncCompletedEventArgs(error, false, null));
                    this.FCompleted = null;
                }
            }
        }

        public override VirtualPropertySet AvailableProperties
        {
            get
            {
                return AvailableSet;
            }
        }

        public object this[int property]
        {
            get
            {
                if ((property == 3) || (property == 5))
                {
                    if (this.CalculateNeeded)
                    {
                        CalculateQueue.QueueWeakWorkItem(new WaitCallback(this.CalculateFolderSize));
                    }
                    return ((property == 3) ? this.Size : this.CompressedSize);
                }
                return null;
            }
        }
    }
}

