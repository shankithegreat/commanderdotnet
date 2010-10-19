namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.Drawing;
    using System;

    public abstract class IconCustomizeIni : BasicCustomizeIni
    {
        public IconCustomizeIni(string iniPath) : base(iniPath)
        {
        }

        protected abstract string GeneralSectionName { get; }

        public virtual IconLocation Icon
        {
            get
            {
                int num;
                string iconFileName = base.Get(this.GeneralSectionName, "IconFile");
                if (iconFileName == null)
                {
                    return null;
                }
                if (!int.TryParse(base.Get(this.GeneralSectionName, "IconIndex"), out num))
                {
                    num = 0;
                }
                return new IconLocation(iconFileName, num);
            }
            set
            {
                if (value == null)
                {
                    base.Set(this.GeneralSectionName, "IconFile", null);
                    base.Set(this.GeneralSectionName, "IconIndex", null);
                }
                else
                {
                    base.Set(this.GeneralSectionName, "IconFile", value.IconFileName);
                    base.Set(this.GeneralSectionName, "IconIndex", value.IconIndex.ToString());
                }
            }
        }
    }
}

