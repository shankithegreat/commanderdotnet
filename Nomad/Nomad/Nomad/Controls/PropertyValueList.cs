namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;

    [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
    public class PropertyValueList : List<PropertyValue>
    {
        public void AddRange(PropertyValue[] values)
        {
            base.AddRange((IEnumerable<PropertyValue>) values);
        }
    }
}

