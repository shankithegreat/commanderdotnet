namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Runtime.Serialization;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Serializable]
    internal class TabStripToolboxItem : ToolboxItem
    {
        public TabStripToolboxItem()
        {
        }

        public TabStripToolboxItem(System.Type toolType) : base(toolType)
        {
        }

        private TabStripToolboxItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected override IComponent[] CreateComponentsCore(IDesignerHost host)
        {
            IComponent[] sourceArray = base.CreateComponentsCore(host);
            Control component = null;
            ControlDesigner parent = null;
            TabStrip strip = null;
            IComponentChangeService service = (IComponentChangeService) host.GetService(typeof(IComponentChangeService));
            if ((sourceArray.Length > 0) && (sourceArray[0] is TabStrip))
            {
                strip = sourceArray[0] as TabStrip;
                ITreeDesigner designer2 = host.GetDesigner(strip) as ITreeDesigner;
                parent = designer2.Parent as ControlDesigner;
                if (parent != null)
                {
                    component = parent.Control;
                }
            }
            if (host != null)
            {
                TabPageSwitcher switcher = null;
                DesignerTransaction transaction = null;
                try
                {
                    try
                    {
                        transaction = host.CreateTransaction("add tabswitcher");
                    }
                    catch (CheckoutException exception)
                    {
                        if (exception != CheckoutException.Canceled)
                        {
                            throw exception;
                        }
                        return sourceArray;
                    }
                    MemberDescriptor member = TypeDescriptor.GetProperties(parent)["Controls"];
                    switcher = host.CreateComponent(typeof(TabPageSwitcher)) as TabPageSwitcher;
                    if (service != null)
                    {
                        service.OnComponentChanging(component, member);
                        service.OnComponentChanged(component, member, null, null);
                    }
                    Dictionary<string, object> properties = new Dictionary<string, object>();
                    properties["Location"] = new Point(strip.Left, strip.Bottom + 3);
                    properties["TabStrip"] = strip;
                    this.SetProperties(switcher, properties, host);
                }
                finally
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                if (switcher != null)
                {
                    IComponent[] destinationArray = new IComponent[sourceArray.Length + 1];
                    Array.Copy(sourceArray, destinationArray, sourceArray.Length);
                    destinationArray[destinationArray.Length - 1] = switcher;
                    return destinationArray;
                }
            }
            return sourceArray;
        }

        private void SetProperties(Component component, Dictionary<string, object> properties, IDesignerHost host)
        {
            IComponentChangeService service = (IComponentChangeService) host.GetService(typeof(IComponentChangeService));
            foreach (string str in properties.Keys)
            {
                PropertyDescriptor member = TypeDescriptor.GetProperties(component)[str];
                if (service != null)
                {
                    service.OnComponentChanging(component, member);
                }
                if (member != null)
                {
                    member.SetValue(component, properties[str]);
                }
                if (service != null)
                {
                    service.OnComponentChanged(component, member, null, null);
                }
            }
        }
    }
}

