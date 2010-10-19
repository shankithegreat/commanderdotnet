namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.ComponentModel;

    public class ExecuteVerbEventArgs : HandledEventArgs
    {
        public readonly string Verb;

        public ExecuteVerbEventArgs(string verb)
        {
            this.Verb = verb;
        }
    }
}

