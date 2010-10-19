namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public class ScalablePictureBox : PictureBox
    {
        private bool FScalable = true;

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (this.Scalable)
            {
                base.ScaleControl(factor, specified);
            }
        }

        [DefaultValue(true)]
        public bool Scalable
        {
            get
            {
                return this.FScalable;
            }
            set
            {
                this.FScalable = value;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return this.Scalable;
            }
        }
    }
}

