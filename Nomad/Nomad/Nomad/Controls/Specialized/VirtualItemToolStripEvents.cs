namespace Nomad.Controls.Specialized
{
    using Microsoft.IO;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.Threading;
    using Nomad.Controls;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public static class VirtualItemToolStripEvents
    {
        private static WeakReference ContextMenuVirtualItem;

        public static void ChangeImage(ToolStripItem item, Image itemImage)
        {
            MethodInvoker method = null;
            if (item.Image != itemImage)
            {
                if ((item.Owner != null) && item.Owner.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            lock (itemImage)
                            {
                                item.Image = itemImage;
                            }
                        };
                    }
                    item.Owner.BeginInvoke(method);
                }
                else
                {
                    lock (itemImage)
                    {
                        item.Image = itemImage;
                    }
                }
            }
        }

        private static void Default_ExecuteVerbs(object sender, ExecuteVerbEventArgs e)
        {
            IVirtualItem target = null;
            if (ContextMenuVirtualItem.IsAlive)
            {
                target = ContextMenuVirtualItem.Target as IVirtualItem;
            }
            if (string.Equals(e.Verb, "eject", StringComparison.OrdinalIgnoreCase))
            {
                VolumeEvents.RaiseRemovingEvent(target.FullName);
            }
            ContextMenuVirtualItem = null;
        }

        private static void DelayedGetIcon(Tuple<WeakReference, Size, WeakReference> state)
        {
            if (state.Item1.IsAlive)
            {
                IVirtualItem target = state.Item1.Target as IVirtualItem;
                if (target != null)
                {
                    Image icon = VirtualIcon.GetIcon(target, state.Item2);
                    if (state.Item3.IsAlive)
                    {
                        ToolStripItem item = state.Item3.Target as ToolStripItem;
                        if ((item != null) && !item.IsDisposed)
                        {
                            ChangeImage(item, icon);
                        }
                    }
                }
            }
        }

        public static void Disposed(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            item.Tag = null;
            item.Disposed -= new EventHandler(VirtualItemToolStripEvents.Disposed);
            item.Paint -= new PaintEventHandler(VirtualItemToolStripEvents.PaintImage);
            item.Paint -= new PaintEventHandler(VirtualItemToolStripEvents.PaintForeColor);
            item.MouseUp -= new MouseEventHandler(VirtualItemToolStripEvents.MouseUp);
            item.MouseHover -= new EventHandler(VirtualItemToolStripEvents.MouseHover);
            item.MouseLeave -= new EventHandler(VirtualItemToolStripEvents.MouseLeave);
        }

        private static Form FindForm(ToolStripItem item)
        {
            ToolStripDropDown owner = item.Owner as ToolStripDropDown;
            while ((item != null) && (owner != null))
            {
                item = owner.OwnerItem;
                if (item != null)
                {
                    owner = item.Owner as ToolStripDropDown;
                }
            }
            Form form = null;
            if ((item != null) && (item.Owner != null))
            {
                form = item.Owner.FindForm();
            }
            return (form ?? Form.ActiveForm);
        }

        public static void MouseEnter(object sender, EventArgs e)
        {
            if (Settings.Default.ShowItemToolTips)
            {
                ToolStripItem item = (ToolStripItem) sender;
                IVirtualItemUI tag = item.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    item.ToolTipText = tag.ToolTip;
                }
            }
        }

        public static void MouseHover(object sender, EventArgs e)
        {
            Form activeForm = Form.ActiveForm;
            if (Settings.Default.ShowItemToolTips && (activeForm != null))
            {
                ToolStripItem item = (ToolStripItem) sender;
                IVirtualItemUI tag = item.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    Form form2 = FindForm(item);
                    if ((form2 != null) && (activeForm == form2))
                    {
                        if (item.AutoSize)
                        {
                            VirtualToolTip.Default.ShowTooltip(tag);
                        }
                        else
                        {
                            StringBuilder builder = new StringBuilder(tag.Name);
                            if (!string.IsNullOrEmpty(tag.ToolTip))
                            {
                                builder.AppendLine();
                                builder.Append(tag.ToolTip);
                            }
                            VirtualToolTip.Default.ShowTooltip(tag, builder.ToString());
                        }
                    }
                }
            }
        }

        public static void MouseLeave(object sender, EventArgs e)
        {
            VirtualToolTip.Default.HideTooltip();
        }

        public static void MouseUp(object sender, MouseEventArgs e)
        {
            IVirtualItem tag = ((ToolStripItem) sender).Tag as IVirtualItem;
            if (tag != null)
            {
                ContextMenuVirtualItem = new WeakReference(tag);
                MouseUp(sender, e, new EventHandler<ExecuteVerbEventArgs>(VirtualItemToolStripEvents.Default_ExecuteVerbs));
            }
        }

        public static void MouseUp(object sender, MouseEventArgs e, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            ToolStripDropDownClosedEventHandler handler2 = null;
            ToolStripItem StripItem;
            if (e.Button == MouseButtons.Right)
            {
                StripItem = (ToolStripItem) sender;
                IVirtualItemUI tag = StripItem.Tag as IVirtualItemUI;
                if (tag != null)
                {
                    Form owner = FindForm(StripItem);
                    if (owner != null)
                    {
                        ContextMenuStrip strip = tag.CreateContextMenuStrip(owner, 0, onExecuteVerb);
                        if (strip != null)
                        {
                            ToolStripDropDownClosedEventHandler handler = null;
                            List<ToolStripDropDown> OwnerDropDownList = null;
                            ToolStripDropDown item = StripItem.Owner as ToolStripDropDown;
                            if (item == null)
                            {
                                StripItem.Paint += new PaintEventHandler(VirtualItemToolStripEvents.PaintBorder);
                                StripItem.Invalidate();
                                if (handler2 == null)
                                {
                                    handler2 = delegate (object dummy1, ToolStripDropDownClosedEventArgs dummy2) {
                                        StripItem.Paint -= new PaintEventHandler(VirtualItemToolStripEvents.PaintBorder);
                                        StripItem.Invalidate();
                                    };
                                }
                                strip.Closed += handler2;
                            }
                            else
                            {
                                OwnerDropDownList = new List<ToolStripDropDown> {
                                    item
                                };
                                while (item.OwnerItem != null)
                                {
                                    item = item.OwnerItem.Owner as ToolStripDropDown;
                                    if (item == null)
                                    {
                                        break;
                                    }
                                    OwnerDropDownList.Add(item);
                                }
                                foreach (ToolStripDropDown down2 in OwnerDropDownList)
                                {
                                    down2.AutoClose = false;
                                    down2.Enabled = false;
                                }
                                if (handler == null)
                                {
                                    handler = delegate (object dummy1, ToolStripDropDownClosedEventArgs dummy2) {
                                        foreach (ToolStripDropDown down in OwnerDropDownList)
                                        {
                                            down.Enabled = true;
                                            down.Close();
                                        }
                                    };
                                }
                                strip.Closed += handler;
                            }
                            VirtualToolTip.Default.HideTooltip();
                            Point location = e.Location;
                            location.Offset(StripItem.Bounds.Location);
                            strip.Show(StripItem.Owner, location);
                            foreach (ToolStripDropDown down2 in OwnerDropDownList.AsEnumerable<ToolStripDropDown>())
                            {
                                down2.AutoClose = true;
                            }
                        }
                    }
                }
            }
        }

        public static void PaintBorder(object sender, PaintEventArgs e)
        {
            Color buttonPressedBorder;
            ToolStripItem item = (ToolStripItem) sender;
            Rectangle rect = new Rectangle(0, 0, item.Width - 1, item.Height - 1);
            if (item is ToolStripSeparator)
            {
                rect.Height--;
            }
            ToolStripProfessionalRenderer renderer = ToolStripManager.Renderer as ToolStripProfessionalRenderer;
            if (renderer != null)
            {
                buttonPressedBorder = renderer.ColorTable.ButtonPressedBorder;
            }
            else
            {
                buttonPressedBorder = SystemColors.ControlDarkDark;
            }
            using (Pen pen = new Pen(buttonPressedBorder))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        public static void PaintForeColor(object sender, PaintEventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem) sender;
            if (tsi.IsForeColorSet())
            {
                Color foreColor = tsi.ForeColor;
                Color backColor = tsi.BackColor;
                if (backColor.IsEmpty || (backColor.A == 0))
                {
                    backColor = tsi.Owner.BackColor;
                }
                if (backColor.IsEmpty || (backColor.A == 0))
                {
                    ToolStripProfessionalRenderer baseRenderer = tsi.Owner.Renderer as ToolStripProfessionalRenderer;
                    if (baseRenderer == null)
                    {
                        ToolStripWrapperRenderer renderer2 = tsi.Owner.Renderer as ToolStripWrapperRenderer;
                        if (renderer2 != null)
                        {
                            baseRenderer = renderer2.BaseRenderer as ToolStripProfessionalRenderer;
                        }
                    }
                    if (baseRenderer != null)
                    {
                        if (tsi.Owner is ToolStripDropDown)
                        {
                            backColor = baseRenderer.ColorTable.ToolStripDropDownBackground;
                        }
                        else
                        {
                            backColor = baseRenderer.ColorTable.ToolStripGradientMiddle;
                        }
                    }
                }
                if ((backColor.IsEmpty || (backColor.A == 0)) || ImageHelper.IsCloseColors(foreColor, backColor))
                {
                    tsi.ResetForeColor();
                }
            }
        }

        public static void PaintImage(object sender, PaintEventArgs e)
        {
            ToolStripItem senderItem = (ToolStripItem) sender;
            IVirtualItem tag = (IVirtualItem) senderItem.Tag;
            if (tag != null)
            {
                UpdateItemImage(senderItem, tag);
            }
        }

        public static void UpdateItemImage(ToolStripItem senderItem, IVirtualItem item)
        {
            Debug.Assert(senderItem != null);
            Debug.Assert(item != null);
            if (!Settings.Default.IsShowIcons)
            {
                if (senderItem.Image != null)
                {
                    senderItem.Image = null;
                }
                senderItem.MergeAction = MergeAction.Append;
            }
            else
            {
                switch (senderItem.DisplayStyle)
                {
                    case ToolStripItemDisplayStyle.Image:
                    case ToolStripItemDisplayStyle.ImageAndText:
                        if (((senderItem.Image == null) || (senderItem.MergeAction == MergeAction.Replace)) || (senderItem.MergeAction == MergeAction.Remove))
                        {
                            Size size = (senderItem.Owner != null) ? senderItem.Owner.ImageScalingSize : ImageHelper.DefaultSmallIconSize;
                            if ((VirtualIcon.DelayedExtractMode == DelayedExtractMode.Never) || (!VirtualIcon.CheckIconOption(IconOptions.ExtractIcons) && !VirtualIcon.CheckIconOption(IconOptions.ShowOverlayIcons)))
                            {
                                ChangeImage(senderItem, VirtualIcon.GetIcon(item, size));
                            }
                            else
                            {
                                if ((senderItem.Image == null) || (senderItem.MergeAction == MergeAction.Remove))
                                {
                                    ChangeImage(senderItem, VirtualIcon.GetIcon(item, size, IconStyle.DefaultIcon));
                                }
                                VirtualIcon.ExtractIconQueue.Value.QueueWeakWorkItem<Tuple<WeakReference, Size, WeakReference>>(new Action<Tuple<WeakReference, Size, WeakReference>>(VirtualItemToolStripEvents.DelayedGetIcon), Tuple.Create<WeakReference, Size, WeakReference>(new WeakReference(item), size, new WeakReference(senderItem)));
                            }
                            senderItem.MergeAction = MergeAction.Append;
                        }
                        return;
                }
            }
        }
    }
}

