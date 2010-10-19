namespace Nomad.Workers
{
    using Microsoft.Win32;
    using Nomad;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class SetAttributesWorker : EventBackgroundWorker
    {
        private bool CanRaiseProgress;
        private IVirtualFolder FContent;
        private DateTime? FCreationTime;
        private DateTime? FLastAccessTime;
        private DateTime? FLastWriteTime;
        private int FProcessedCount;
        private FileAttributes FResetAttributes;
        private SearchFolderOptions FSearchOptions;
        private FileAttributes FSetAttributes;
        private int FStoredProgress = -1;
        private int FTotalCount;

        public event EventHandler<VirtualItemEventArgs> OnAfterSetAttributes;

        public event EventHandler<VirtualItemEventArgs> OnBeforeSetAttributes;

        public event EventHandler<ChangeItemErrorEventArgs> OnSetAttributesError;

        public SetAttributesWorker(IEnumerable<IVirtualItem> items, bool includeSubfolders, FileAttributes setAttributes, FileAttributes resetAttributes, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime)
        {
            if (!((((setAttributes != 0) || (resetAttributes != 0)) || (creationTime.HasValue || lastAccessTime.HasValue)) || lastWriteTime.HasValue))
            {
                throw new ArgumentException();
            }
            this.FContent = new AggregatedVirtualFolder(items);
            this.FSearchOptions = includeSubfolders ? SearchFolderOptions.ProcessSubfolders : ((SearchFolderOptions) 0);
            this.FSetAttributes = setAttributes;
            this.FResetAttributes = resetAttributes;
            this.FCreationTime = creationTime;
            this.FLastAccessTime = lastAccessTime;
            this.FLastWriteTime = lastWriteTime;
        }

        protected override void DoWork()
        {
            using (new ThreadExecutionStateLock(true, false))
            {
                using (VirtualSearchFolder folder = new VirtualSearchFolder(this.FContent, null, this.FSearchOptions))
                {
                    folder.OnChanged += new EventHandler<VirtualItemChangedEventArgs>(this.SearchFolderChanged);
                    folder.Completed += new AsyncCompletedEventHandler(this.SearchFolderCompleted);
                    foreach (IVirtualItem item in folder.GetContent())
                    {
                        base.CheckSuspendingPending();
                        if (base.CancellationPending)
                        {
                            return;
                        }
                        if (this.OnBeforeSetAttributes != null)
                        {
                            this.OnBeforeSetAttributes(this, new VirtualItemEventArgs(item));
                        }
                        this.SetItemAttributes(item);
                        if (this.OnAfterSetAttributes != null)
                        {
                            this.OnAfterSetAttributes(this, new VirtualItemEventArgs(item));
                        }
                        this.FProcessedCount++;
                        this.OnProgress();
                    }
                }
            }
        }

        private void OnProgress()
        {
            if (this.CanRaiseProgress)
            {
                int progressPercent = (this.FTotalCount != 0) ? ((this.FProcessedCount * 100) / this.FTotalCount) : 0;
                if ((progressPercent <= 100) && (this.FStoredProgress != progressPercent))
                {
                    base.RaiseProgressChanged(progressPercent, null);
                    this.FStoredProgress = progressPercent;
                }
            }
        }

        private void SearchFolderChanged(object sender, VirtualItemChangedEventArgs e)
        {
            this.FTotalCount++;
        }

        private void SearchFolderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.CanRaiseProgress = true;
            this.OnProgress();
        }

        private ChangeItemAction SetAttributesError(IVirtualItem item, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            if (this.OnSetAttributesError != null)
            {
                ChangeItemErrorEventArgs e = new ChangeItemErrorEventArgs(item, available, error);
                this.OnSetAttributesError(this, e);
                none = e.Action;
            }
            return none;
        }

        private void SetItemAttributes(IVirtualItem item)
        {
            ChangeItemAction action;
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
        Label_0003:;
            try
            {
                IChangeVirtualItem item2 = item as IChangeVirtualItem;
                if (item2 == null)
                {
                    throw new ItemChangeNotSupportedException();
                }
                FileAttributes attributes = item2.Attributes;
                VirtualItemHelper.ResetSystemAttributes(item);
                IUpdateVirtualProperty property = item2 as IUpdateVirtualProperty;
                if (property != null)
                {
                    property.BeginUpdate();
                }
                if (this.FCreationTime.HasValue && item2.CanSetProperty(7))
                {
                    item2[7] = this.FCreationTime;
                }
                if (this.FLastAccessTime.HasValue && item2.CanSetProperty(9))
                {
                    item2[9] = this.FLastAccessTime;
                }
                if (this.FLastWriteTime.HasValue && item2.CanSetProperty(8))
                {
                    item2[8] = this.FLastWriteTime;
                }
                if (property != null)
                {
                    property.EndUpdate();
                }
                if (((this.FSetAttributes > 0) || (this.FResetAttributes > 0)) && item2.CanSetProperty(6))
                {
                    FileAttributes attributes2 = (attributes | this.FSetAttributes) & ~this.FResetAttributes;
                    if (attributes2 != attributes)
                    {
                        item2[6] = attributes2;
                    }
                }
                return;
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.SetAttributesError(item, canRetryOrElevate, exception);
                canRetryOrElevate &= ~AvailableItemActions.CanElevate;
            }
            catch (IOException exception2)
            {
                action = this.SetAttributesError(item, canRetryOrElevate, exception2);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_0003;

                case ChangeItemAction.Skip:
                    return;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return;
            }
            throw new InvalidEnumArgumentException();
        }

        public override string Name
        {
            get
            {
                return "Change Attributes";
            }
        }
    }
}

