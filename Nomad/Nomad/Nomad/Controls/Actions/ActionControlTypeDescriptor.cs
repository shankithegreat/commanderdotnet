namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;

    internal class ActionControlTypeDescriptor : CustomTypeDescriptor
    {
        internal ActionControlTypeDescriptor(ICustomTypeDescriptor descr) : base(descr)
        {
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(null);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection descriptors = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor descriptor in base.GetProperties(attributes))
            {
                PropertyDescriptor descriptor2;
                string name = descriptor.Name;
                if ((name != null) && ((name == "Text") || (name == "ShortcutKeys")))
                {
                    descriptor2 = new NonSerializablePropertyDescriptor(descriptor);
                }
                else
                {
                    descriptor2 = descriptor;
                }
                descriptors.Add(descriptor2);
            }
            return descriptors;
        }
    }
}

