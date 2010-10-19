namespace Nomad.Controls.Specialized
{
    using Nomad.Configuration;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FilterComboBox : ComboBox
    {
        private int LastSelectedIndex = -1;

        public FilterComboBox()
        {
            this.Text = "*";
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                base.OnDrawItem(e);
            }
            else
            {
                object listItem = this.Items[e.Index];
                ListControlConvertEventArgs args = new ListControlConvertEventArgs(listItem.ToString(), typeof(string), listItem);
                this.OnFormat(args);
                string str = (string) args.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    e.DrawBackground();
                    TextRenderer.DrawText(e.Graphics, str, e.Font, e.Bounds, e.ForeColor, TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
                    e.DrawFocusRectangle();
                }
                else
                {
                    int num = e.Bounds.Top + (e.Bounds.Height / 2);
                    e.Graphics.DrawLine(SystemPens.ControlText, e.Bounds.Left, num, e.Bounds.Right, num);
                }
            }
        }

        protected override void OnFormat(ListControlConvertEventArgs e)
        {
            if ((e.ListItem is NamedFilter) && (e.DesiredType == typeof(string)))
            {
                e.Value = ((NamedFilter) e.ListItem).Name;
            }
            base.OnFormat(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keys keyData = e.KeyData;
            if (((keyData == Keys.Back) || (keyData == Keys.Delete)) && (base.SelectedItem is NamedFilter))
            {
                this.SelectedIndex = -1;
                this.Text = "";
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.LastSelectedIndex = this.SelectedIndex;
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            int selectedIndex = this.SelectedIndex;
            if ((selectedIndex >= 0) && string.IsNullOrEmpty(base.SelectedItem.ToString()))
            {
                selectedIndex += (this.SelectedIndex > this.LastSelectedIndex) ? 1 : -1;
            }
            if (selectedIndex == (this.Items.Count - 1))
            {
                using (FilterDialog dialog = new FilterDialog())
                {
                    IVirtualItemFilter customFilter = ((NamedFilter) this.Items[this.Items.Count - 1]).Filter;
                    dialog.Filter = customFilter;
                    dialog.RememberFilterEnabled = false;
                    if (dialog.Execute(base.FindForm()))
                    {
                        IVirtualItemFilter filter = dialog.Filter;
                        this.PopulateFilters(filter);
                        for (int i = 0; i < this.Items.Count; i++)
                        {
                            NamedFilter filter3 = this.Items[i] as NamedFilter;
                            if (((filter3 != null) && (filter3.Filter != null)) && filter3.Filter.Equals(filter))
                            {
                                selectedIndex = i;
                                goto Label_0144;
                            }
                        }
                    }
                    else
                    {
                        this.PopulateFilters(customFilter);
                        selectedIndex = this.LastSelectedIndex;
                    }
                }
            }
        Label_0144:
            if (((selectedIndex >= this.Items.Count) || (selectedIndex < 0)) || string.IsNullOrEmpty(this.Items[selectedIndex].ToString()))
            {
                selectedIndex = -1;
            }
            this.SelectedIndex = selectedIndex;
            this.LastSelectedIndex = selectedIndex;
            base.OnSelectionChangeCommitted(e);
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            if (base.SelectedItem is NamedFilter)
            {
                base.SelectedItem = null;
            }
            base.OnTextUpdate(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!(!base.Visible || base.DesignMode))
            {
                this.PopulateFilters(null);
            }
            base.OnVisibleChanged(e);
        }

        private void PopulateFilters(IVirtualItemFilter CustomFilter)
        {
            base.BeginUpdate();
            try
            {
                this.Items.Clear();
                string[] copyFilter = HistorySettings.Default.CopyFilter;
                if ((copyFilter != null) && (copyFilter.Length > 0))
                {
                    this.Items.AddRange(copyFilter);
                    this.Items.Add(string.Empty);
                }
                NamedFilter[] filters = Settings.Default.Filters;
                if ((filters != null) && (filters.Length > 0))
                {
                    this.Items.AddRange(filters);
                    this.Items.Add(string.Empty);
                }
                this.Items.Add(new NamedFilter(Resources.sCustomFilter, CustomFilter));
            }
            finally
            {
                base.EndUpdate();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Windows.Forms.DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            set
            {
                base.DrawMode = value;
            }
        }

        public IVirtualItemFilter Filter
        {
            get
            {
                if (base.Enabled)
                {
                    if (base.SelectedItem is NamedFilter)
                    {
                        return ((NamedFilter) base.SelectedItem).Filter;
                    }
                    string filterString = this.FilterString;
                    if (!(string.IsNullOrEmpty(filterString) || filterString.Equals("*", StringComparison.Ordinal)))
                    {
                        return new AggregatedVirtualItemFilter(AggregatedFilterCondition.Any, new VirtualItemNameFilter(filterString), new VirtualItemAttributeFilter(FileAttributes.Directory));
                    }
                }
                return null;
            }
        }

        public string FilterString
        {
            get
            {
                if (!(base.Enabled && !(base.SelectedItem is NamedFilter)))
                {
                    return null;
                }
                return this.Text.Trim();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ComboBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [DefaultValue("*")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}

