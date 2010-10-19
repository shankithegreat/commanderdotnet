namespace Nomad.Controls.Actions
{
    using System;

    public interface IAction
    {
        bool Execute(object source, object target);
        ActionState Update(object source, object target);
    }
}

