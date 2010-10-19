namespace Nomad.Workers
{
    using Microsoft.Win32;
    using Nomad;
    using Nomad.Commons;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DeleteWorker : EventBackgroundWorker, ISetOwnerWindow
    {
        private bool CanRaiseProgress;
        private IVirtualFolder FContent;
        private int FProcessedItems;
        private bool FSendToBin;
        private int FStoredProgress = -1;
        private int FTotalFiles;

        public event EventHandler<VirtualItemEventArgs> OnAfterDelete;

        public event EventHandler<BeforeDeleteItemEventArgs> OnBeforeDelete;

        public event EventHandler<ChangeItemErrorEventArgs> OnDeleteError;

        public DeleteWorker(IEnumerable<IVirtualItem> items, bool sendToBin)
        {
            this.FContent = new AggregatedVirtualFolder(items);
            this.FSendToBin = sendToBin;
        }

        private void AfterDelete(IVirtualItem item)
        {
            if (this.OnAfterDelete != null)
            {
                this.OnAfterDelete(this, new VirtualItemEventArgs(item));
            }
        }

        private DialogResult BeforeDelete(IVirtualItem item)
        {
            DialogResult yes = DialogResult.Yes;
            if (this.OnBeforeDelete != null)
            {
                BeforeDeleteItemEventArgs e = new BeforeDeleteItemEventArgs(item);
                this.OnBeforeDelete(this, e);
                yes = e.Action;
            }
            return yes;
        }

        private ChangeItemAction DeleteError(IVirtualItem item, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            if (this.OnDeleteError != null)
            {
                ChangeItemErrorEventArgs e = new ChangeItemErrorEventArgs(item, available, error);
                this.OnDeleteError(this, e);
                none = e.Action;
            }
            return none;
        }

        private bool DeleteItem(IVirtualItem item, bool sendToBin)
        {
            ChangeItemAction action;
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
        Label_0003:;
            try
            {
                IChangeVirtualItem item2 = item as IChangeVirtualItem;
                if (item2 == null)
                {
                    throw new ItemChangeNotSupportedException(string.Format(Resources.sErrorDeleteNonChangeableItem, item.FullName));
                }
                item2.Delete(sendToBin);
                this.AfterDelete(item);
                return true;
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.DeleteError(item, canRetryOrElevate, exception);
                canRetryOrElevate &= ~AvailableItemActions.CanElevate;
            }
            catch (Exception exception2)
            {
                action = this.DeleteError(item, canRetryOrElevate, exception2);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_0003;

                case ChangeItemAction.Skip:
                    return false;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return false;
            }
            throw new InvalidEnumArgumentException();
        }

        protected override void DoWork()
        {
            using (new ThreadExecutionStateLock(true, false))
            {
                using (VirtualSearchFolder folder = new VirtualSearchFolder(this.FContent, null, SearchFolderOptions.SkipReparsePoints | SearchFolderOptions.ProcessSubfolders))
                {
                    IVirtualFolder current;
                    folder.OnChanged += new EventHandler<VirtualItemChangedEventArgs>(this.SearchFolderChanged);
                    folder.Completed += new AsyncCompletedEventHandler(this.SearchFolderCompleted);
                    Dictionary<ISequenceContext, ISequenceProcessor> dictionary = new Dictionary<ISequenceContext, ISequenceProcessor>();
                    Stack<IVirtualFolder> stack = new Stack<IVirtualFolder>();
                    List<IVirtualFolder> list = new List<IVirtualFolder>();
                    List<IVirtualItem> list2 = new List<IVirtualItem>();
                    foreach (IVirtualItem item in folder.GetContent())
                    {
                        ISequenceableItem sequenceableItem;
                        base.CheckSuspendingPending();
                        if (base.CancellationPending)
                        {
                            return;
                        }
                        bool flag = false;
                        using (List<IVirtualFolder>.Enumerator enumerator2 = list.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                current = enumerator2.Current;
                                if (current.IsChild(item))
                                {
                                    flag = true;
                                    goto Label_00DF;
                                }
                            }
                        }
                    Label_00DF:
                        sequenceableItem = null;
                        if (flag)
                        {
                            goto Label_01ED;
                        }
                        IVirtualFolder folder3 = item as IVirtualFolder;
                        sequenceableItem = item as ISequenceableItem;
                        if ((sequenceableItem == null) && (item is ISequenceable))
                        {
                            sequenceableItem = ((ISequenceable) item).SequenceableItem;
                        }
                        DialogResult result = this.BeforeDelete(item);
                        if (result != DialogResult.Cancel)
                        {
                            if (result != DialogResult.No)
                            {
                                goto Label_0174;
                            }
                            if (folder3 != null)
                            {
                                list.Add(folder3);
                            }
                            else
                            {
                                list2.Add(item);
                            }
                            flag = true;
                            goto Label_01ED;
                        }
                        base.CancelAsync();
                        return;
                    Label_0174:
                        if ((sequenceableItem == null) && (folder3 != null))
                        {
                            IChangeVirtualItem item3 = folder3 as IChangeVirtualItem;
                            if ((this.FSendToBin && (item3 != null)) && item3.CanSendToBin)
                            {
                                this.DeleteItem(item, this.FSendToBin);
                                list.Add(folder3);
                            }
                            else if (!(folder3 is ArchiveFolder))
                            {
                                stack.Push(folder3);
                            }
                            continue;
                        }
                    Label_01ED:
                        if (!flag)
                        {
                            if (sequenceableItem != null)
                            {
                                ISequenceContext sequenceContext = sequenceableItem.SequenceContext;
                                if (sequenceContext != null)
                                {
                                    ISequenceProcessor processor;
                                    if (!dictionary.TryGetValue(sequenceContext, out processor))
                                    {
                                        processor = sequenceContext.CreateProcessor(SequenseProcessorType.Delete);
                                        if (processor != null)
                                        {
                                            dictionary.Add(sequenceContext, processor);
                                        }
                                    }
                                    if (processor != null)
                                    {
                                        processor.Add(sequenceableItem, item);
                                        continue;
                                    }
                                }
                            }
                            if (!this.DeleteItem(item, this.FSendToBin))
                            {
                                list2.Add(item);
                            }
                        }
                        this.FProcessedItems++;
                        if (!flag)
                        {
                            this.OnProgress();
                        }
                    }
                    if (dictionary.Count > 0)
                    {
                        foreach (ISequenceProcessor processor in dictionary.Values)
                        {
                            base.CheckSuspendingPending();
                            if (base.CancellationPending)
                            {
                                return;
                            }
                            this.ProcessDelete(processor);
                            this.OnProgress();
                        }
                    }
                    while (stack.Count > 0)
                    {
                        base.CheckSuspendingPending();
                        if (base.CancellationPending)
                        {
                            return;
                        }
                        bool flag2 = false;
                        current = stack.Pop();
                        foreach (IVirtualItem item4 in list2)
                        {
                            if (current.IsChild(item4))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2 && !this.DeleteItem(current, false))
                        {
                            list2.Add(current);
                        }
                        this.FProcessedItems++;
                        if (!flag2)
                        {
                            this.OnProgress();
                        }
                    }
                }
            }
        }

        private void OnProgress()
        {
            if (this.CanRaiseProgress)
            {
                int progressPercent = (this.FTotalFiles != 0) ? ((this.FProcessedItems * 100) / this.FTotalFiles) : 0;
                if ((progressPercent <= 100) && (this.FStoredProgress != progressPercent))
                {
                    base.RaiseProgressChanged(progressPercent, null);
                    this.FStoredProgress = progressPercent;
                }
            }
        }

        private void ProcessDelete(ISequenceProcessor processor)
        {
            ISetOwnerWindow window = processor as ISetOwnerWindow;
            if (window != null)
            {
                window.Owner = this.Owner;
            }
            try
            {
                List<IVirtualItem> DeletedItems = new List<IVirtualItem>();
                processor.Process(delegate (ProcessItemEventArgs e) {
                    DeletedItems.Add((IVirtualItem) e.UserState);
                });
                foreach (IVirtualItem item in DeletedItems)
                {
                    this.AfterDelete(item);
                    this.FProcessedItems++;
                }
            }
            catch (Exception exception)
            {
                switch (this.DeleteError(null, AvailableItemActions.None, exception))
                {
                    case ChangeItemAction.Skip:
                        return;

                    case ChangeItemAction.Cancel:
                        base.CancelAsync();
                        return;
                }
                throw;
            }
        }

        private void SearchFolderChanged(object sender, VirtualItemChangedEventArgs e)
        {
            this.FTotalFiles++;
        }

        private void SearchFolderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.CanRaiseProgress = true;
            this.OnProgress();
        }

        public override string Name
        {
            get
            {
                return "Delete";
            }
        }

        public IWin32Window Owner { get; set; }
    }
}

