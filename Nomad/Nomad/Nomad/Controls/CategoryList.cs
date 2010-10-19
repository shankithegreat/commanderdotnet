namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;

    [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
    public class CategoryList : List<Category>
    {
        public void AddRange(Category[] actions)
        {
            base.AddRange((IEnumerable<Category>) actions);
        }
    }
}

