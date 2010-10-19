namespace Nomad.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public class OptionsSectionGroup : ConfigurationSectionGroup
    {
        public IList<OptionsSection> OrderedSectionList
        {
            get
            {
                List<OptionsSection> list = new List<OptionsSection>(base.Sections.Count);
                foreach (OptionsSection section in base.Sections)
                {
                    list.Add(section);
                }
                list.Sort(delegate (OptionsSection s1, OptionsSection s2) {
                    return s1.Order - s2.Order;
                });
                return list;
            }
        }
    }
}

