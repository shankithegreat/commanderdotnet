namespace Nomad.Controls
{
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;

    [ProvideProperty("AdvancedToolTip", typeof(Control)), ToolboxItemFilter("System.Windows.Forms"), DesignerCategory("Code")]
    public class AdvancedToolTip : Component, IExtenderProvider
    {
        private System.Windows.Forms.ToolTip ToolTip;
        private Dictionary<Control, PictureBox> ToolTipMap;

        public AdvancedToolTip()
        {
        }

        public AdvancedToolTip(IContainer container)
        {
            container.Add(this);
        }

        private void AdvancedToolTip_MouseHover(object sender, EventArgs e)
        {
            if (this.ToolTip == null)
            {
                this.ToolTip = new System.Windows.Forms.ToolTip();
            }
            Control window = (Control) sender;
            Point point = window.PointToClient(Cursor.Position);
            this.ToolTip.Show(window.Text, window, point.X, point.Y + window.Cursor.GetPrefferedHeight());
        }

        private void AdvancedToolTip_MouseLeave(object sender, EventArgs e)
        {
            if (this.ToolTip != null)
            {
                this.ToolTip.Hide((Control) sender);
            }
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            Control key = (Control) sender;
            PictureBox box = this.ToolTipMap[key];
            this.ToolTipMap.Remove(key);
            box.Dispose();
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            PictureBox toolTipBox = this.ToolTipMap[control];
            UpdateToolTipBoxLocation(toolTipBox, control);
            toolTipBox.Visible = control.Visible;
        }

        private void Control_ParentChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            PictureBox toolTipBox = this.ToolTipMap[control];
            UpdateToolTipBoxLocation(toolTipBox, control);
        }

        private void Control_VisibleChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            PictureBox toolTipBox = this.ToolTipMap[control];
            if (control.Visible)
            {
                UpdateToolTipBoxLocation(toolTipBox, control);
            }
            toolTipBox.Visible = control.Visible;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ToolTipMap != null)
                {
                    foreach (KeyValuePair<Control, PictureBox> pair in this.ToolTipMap)
                    {
                        this.Unsibscribe(pair.Key);
                        pair.Value.Dispose();
                    }
                    this.ToolTipMap = null;
                }
                if (this.ToolTip != null)
                {
                    this.ToolTip.Dispose();
                    this.ToolTip = null;
                }
            }
            base.Dispose(disposing);
        }

        [Localizable(true), Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor)), DefaultValue("")]
        public string GetAdvancedToolTip(Control control)
        {
            PictureBox box;
            if ((this.ToolTipMap != null) && this.ToolTipMap.TryGetValue(control, out box))
            {
                return box.Text;
            }
            return string.Empty;
        }

        private static Point PointToParent(Control c, Point p, Control parent)
        {
            if (c.IsHandleCreated && parent.IsHandleCreated)
            {
                return parent.PointToClient(c.PointToScreen(p));
            }
            Point point = p;
            for (Control control = c; (control != null) && (control != parent); control = control.Parent)
            {
                point.Offset(control.Location);
            }
            return point;
        }

        public void SetAdvancedToolTip(Control control, string toolTip)
        {
            if (this.ToolTipMap == null)
            {
                this.ToolTipMap = new Dictionary<Control, PictureBox>();
            }
            PictureBox box = null;
            bool flag = this.ToolTipMap.TryGetValue(control, out box);
            if (string.IsNullOrEmpty(toolTip))
            {
                if (!base.DesignMode)
                {
                    this.Unsibscribe(control);
                }
                if (flag)
                {
                    this.ToolTipMap.Remove(control);
                    box.Dispose();
                }
            }
            else
            {
                if (!flag)
                {
                    box = new PictureBox {
                        Image = Resources.InfoTip,
                        SizeMode = PictureBoxSizeMode.AutoSize
                    };
                    box.MouseHover += new EventHandler(this.AdvancedToolTip_MouseHover);
                    box.MouseLeave += new EventHandler(this.AdvancedToolTip_MouseLeave);
                    this.ToolTipMap.Add(control, box);
                    if (!base.DesignMode)
                    {
                        control.LocationChanged += new EventHandler(this.Control_LocationChanged);
                        control.ParentChanged += new EventHandler(this.Control_ParentChanged);
                        control.SizeChanged += new EventHandler(this.Control_LocationChanged);
                        control.VisibleChanged += new EventHandler(this.Control_VisibleChanged);
                        control.Disposed += new EventHandler(this.Control_Disposed);
                        UpdateToolTipBoxLocation(box, control);
                    }
                }
                box.Text = toolTip;
                box.Tag = null;
            }
        }

        bool IExtenderProvider.CanExtend(object extendee)
        {
            return (extendee is Control);
        }

        private void Unsibscribe(Control control)
        {
            control.LocationChanged -= new EventHandler(this.Control_LocationChanged);
            control.ParentChanged -= new EventHandler(this.Control_ParentChanged);
            control.SizeChanged -= new EventHandler(this.Control_LocationChanged);
            control.VisibleChanged -= new EventHandler(this.Control_VisibleChanged);
            control.Disposed -= new EventHandler(this.Control_Disposed);
        }

        public void Update(Control control)
        {
            PictureBox box;
            if ((this.ToolTipMap != null) && this.ToolTipMap.TryGetValue(control, out box))
            {
                UpdateToolTipBoxLocation(box, control);
                box.Visible = control.Visible;
            }
        }

        public void UpdateAll()
        {
            if (this.ToolTipMap != null)
            {
                foreach (KeyValuePair<Control, PictureBox> pair in this.ToolTipMap)
                {
                    UpdateToolTipBoxLocation(pair.Value, pair.Key);
                    pair.Value.Visible = pair.Key.Visible;
                }
            }
        }

        private static void UpdateToolTipBoxLocation(PictureBox toolTipBox, Control control)
        {
            Control parent = control.Parent;
            while ((parent != null) && ((!(parent is Form) && !(parent is UserControl)) && !(parent is TabPage)))
            {
                parent = parent.Parent;
            }
            if (parent != toolTipBox.Parent)
            {
                toolTipBox.Parent = parent;
            }
            if (parent != null)
            {
                Point p = new Point(control.Width, (control.Height - toolTipBox.Height) / 2);
                if (!((control is Label) || (control is CheckBox)))
                {
                    p.X += control.Margin.Right;
                }
                p = PointToParent(control, p, parent);
                if ((p.X > 0) && (p.Y > 0))
                {
                    toolTipBox.Location = p;
                    toolTipBox.BringToFront();
                }
            }
        }
    }
}

