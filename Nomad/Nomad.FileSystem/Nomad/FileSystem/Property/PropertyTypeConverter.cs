namespace Nomad.FileSystem.Property
{
    using System;
    using System.ComponentModel;

    public abstract class PropertyTypeConverter : TypeConverter
    {
        protected PropertyTypeConverter()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(PropertyMeasure)) || base.CanConvertTo(context, destinationType));
        }
    }
}

