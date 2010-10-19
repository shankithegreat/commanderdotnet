namespace Nomad.Commons
{
    using System;

    public interface IRenameFilter
    {
        string CreateNewName(string sourceName);
    }
}

