namespace Nomad
{
    using Nomad.Configuration;
    using System;

    [Serializable]
    public class TwoPanelTab : GeneralTab
    {
        public TwoPanelLayout Layout;
        public PanelContentContainer Left;
        public PanelContentContainer Right;
    }
}

