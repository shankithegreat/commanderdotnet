namespace Nomad.Commons.Threading
{
    using System;

    public interface IParallelLoopState
    {
        void Stop();

        bool IsExceptional { get; }

        bool IsStopped { get; }

        bool ShouldExitCurrentIteration { get; }
    }
}

