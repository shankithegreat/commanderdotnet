namespace Nomad.FileSystem.Virtual.Filter
{
    using System;

    public class NamedFilter : FilterContainer
    {
        public string Name;

        public NamedFilter()
        {
        }

        public NamedFilter(string name, IVirtualItemFilter filter) : base(filter)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

