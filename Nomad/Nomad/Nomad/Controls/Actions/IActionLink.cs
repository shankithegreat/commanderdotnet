namespace Nomad.Controls.Actions
{
    using System;
    using System.ComponentModel;

    public interface IActionLink : IDisposable
    {
        CustomAction Action { get; }

        System.ComponentModel.Component Component { get; }
    }
}

