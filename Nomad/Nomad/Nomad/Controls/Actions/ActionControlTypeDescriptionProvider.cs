namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;

    internal sealed class ActionControlTypeDescriptionProvider : TypeDescriptionProvider
    {
        public ActionControlTypeDescriptionProvider(object instance) : base(TypeDescriptor.GetProvider(instance))
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new ActionControlTypeDescriptor(base.GetTypeDescriptor(objectType, instance));
        }
    }
}

