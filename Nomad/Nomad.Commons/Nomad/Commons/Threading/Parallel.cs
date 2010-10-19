namespace Nomad.Commons.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Parallel
    {
        private static int MaxParallelCount = Environment.ProcessorCount;
        private static Semaphore ParallelLock = new Semaphore(MaxParallelCount, MaxParallelCount);

        private static void ExecuteAndWait(TaskContext context, ContextedTask task)
        {
            if (ParallelLock.WaitOne(0, false))
            {
                task.Context.Enter();
                ThreadPool.QueueUserWorkItem(new WaitCallback(Parallel.ExecuteTask), task);
            }
            else
            {
                task.InternalRun();
            }
        }

        private static void ExecuteTask(object state)
        {
            ContextedTask task = (ContextedTask) state;
            try
            {
                task.InternalRun();
            }
            catch (Exception exception)
            {
                task.Context.RegisterException(exception);
            }
            finally
            {
                ParallelLock.Release();
                Thread.MemoryBarrier();
                task.Context.Leave();
            }
        }

        public static void For(int start, int end, Action<int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }
            int num = (end - start) + 1;
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            switch (num)
            {
                case 0:
                    break;

                case 1:
                    action(start);
                    break;

                default:
                    int num2;
                    if (MaxParallelCount == 1)
                    {
                        for (num2 = start; num2 <= end; num2++)
                        {
                            action(num2);
                        }
                    }
                    else
                    {
                        using (TaskContext context = new TaskContext())
                        {
                            for (num2 = start; num2 <= end; num2++)
                            {
                                ExecuteAndWait(context, new ContextedActionTask<int>(context, action, num2));
                            }
                        }
                    }
                    break;
            }
        }

        public static void For(int start, int end, Action<int, IParallelLoopState> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }
            int num = (end - start) + 1;
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            switch (num)
            {
                case 0:
                    break;

                case 1:
                    action(start, new SimpleLoopState());
                    break;

                default:
                    int num2;
                    if (MaxParallelCount == 1)
                    {
                        SimpleLoopState state = new SimpleLoopState();
                        for (num2 = start; (num2 <= end) && !state.IsStopped; num2++)
                        {
                            action(num2, state);
                        }
                    }
                    else
                    {
                        using (TaskContext context = new TaskContext())
                        {
                            for (num2 = start; (num2 <= end) && !context.IsStopped; num2++)
                            {
                                ExecuteAndWait(context, new ContextedAndStatedActionTask<int>(context, action, num2));
                            }
                        }
                    }
                    break;
            }
        }

        public static void ForEach<TSource>(IEnumerable<TSource> data, Action<TSource> action)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (MaxParallelCount == 1)
            {
                foreach (TSource local in data)
                {
                    action(local);
                }
            }
            else
            {
                using (TaskContext context = new TaskContext())
                {
                    foreach (TSource local in data)
                    {
                        ExecuteAndWait(context, new ContextedActionTask<TSource>(context, action, local));
                    }
                }
            }
        }

        public static void ForEach<TSource>(IEnumerable<TSource> data, Action<TSource, IParallelLoopState> action)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (MaxParallelCount == 1)
            {
                SimpleLoopState state = new SimpleLoopState();
                foreach (TSource local in data)
                {
                    if (state.IsStopped)
                    {
                        break;
                    }
                    action(local, state);
                }
            }
            else
            {
                using (TaskContext context = new TaskContext())
                {
                    foreach (TSource local in data)
                    {
                        if (context.IsStopped)
                        {
                            return;
                        }
                        ExecuteAndWait(context, new ContextedAndStatedActionTask<TSource>(context, action, local));
                    }
                }
            }
        }

        public static void Invoke(ThreadStart[] actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException();
            }
            switch (actions.Length)
            {
                case 0:
                    break;

                case 1:
                    actions[0]();
                    break;

                default:
                    if (MaxParallelCount == 1)
                    {
                        foreach (ThreadStart start in actions)
                        {
                            start();
                        }
                    }
                    else
                    {
                        using (TaskContext context = new TaskContext())
                        {
                            foreach (ThreadStart start in actions)
                            {
                                ExecuteAndWait(context, new ContextedThreadStartTask(context, start));
                            }
                        }
                    }
                    break;
            }
        }

        private class ContextedActionTask<T> : Parallel.ContextedTask
        {
            private Action<T> FAction;
            private T FState;

            public ContextedActionTask(Parallel.TaskContext context, Action<T> action, T state) : base(context)
            {
                this.FAction = action;
                this.FState = state;
            }

            protected override void Run()
            {
                this.FAction(this.FState);
            }
        }

        private class ContextedAndStatedActionTask<T> : Parallel.ContextedTask
        {
            private Action<T, IParallelLoopState> FAction;
            private T FState;

            public ContextedAndStatedActionTask(Parallel.TaskContext context, Action<T, IParallelLoopState> action, T state) : base(context)
            {
                this.FAction = action;
                this.FState = state;
            }

            protected override void Run()
            {
                this.FAction(this.FState, base.Context);
            }
        }

        private abstract class ContextedTask : SimpleTask
        {
            public readonly Parallel.TaskContext Context;

            public ContextedTask(Parallel.TaskContext context)
            {
                this.Context = context;
            }

            protected override bool OnError(Exception e)
            {
                this.Context.RegisterException(e);
                return true;
            }
        }

        private class ContextedThreadStartTask : Parallel.ContextedTask
        {
            private ThreadStart FAction;

            public ContextedThreadStartTask(Parallel.TaskContext context, ThreadStart action) : base(context)
            {
                this.FAction = action;
            }

            protected override void Run()
            {
                this.FAction();
            }
        }

        private class SimpleLoopState : IParallelLoopState
        {
            private bool Stopped;

            public void Stop()
            {
                this.Stopped = true;
            }

            public bool IsExceptional
            {
                get
                {
                    return false;
                }
            }

            public bool IsStopped
            {
                get
                {
                    return this.Stopped;
                }
            }

            public bool ShouldExitCurrentIteration
            {
                get
                {
                    return this.Stopped;
                }
            }
        }

        private class TaskContext : IParallelLoopState, IDisposable
        {
            private LinkedList<Exception> ExceptionList;
            private EventWaitHandle FinishedEvent = new ManualResetEvent(true);
            private int LockCount;
            private int StopCount;

            public void Close()
            {
                if (this.FinishedEvent == null)
                {
                    throw new ObjectDisposedException("TaskContext");
                }
                int num = 0;
                while ((Thread.VolatileRead(ref this.LockCount) > 0) && (num++ < 100))
                {
                    Thread.SpinWait(20);
                }
                this.FinishedEvent.WaitOne();
                this.FinishedEvent.Close();
                this.FinishedEvent = null;
                if (this.ExceptionList != null)
                {
                    throw new TargetInvocationException(this.ExceptionList.First.Value);
                }
            }

            public void Dispose()
            {
                if (this.FinishedEvent != null)
                {
                    this.Close();
                }
            }

            public void Enter()
            {
                if (this.FinishedEvent == null)
                {
                    throw new ObjectDisposedException("TaskContext");
                }
                Interlocked.Increment(ref this.LockCount);
                Thread.MemoryBarrier();
                this.FinishedEvent.Reset();
            }

            public void Leave()
            {
                if (this.FinishedEvent == null)
                {
                    throw new ObjectDisposedException("TaskContext");
                }
                if (Interlocked.Decrement(ref this.LockCount) == 0)
                {
                    this.FinishedEvent.Set();
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void RegisterException(Exception e)
            {
                if (this.ExceptionList == null)
                {
                    this.ExceptionList = new LinkedList<Exception>();
                }
                this.ExceptionList.AddLast(e);
            }

            public void Stop()
            {
                Interlocked.CompareExchange(ref this.StopCount, 1, 0);
            }

            public bool IsExceptional
            {
                get
                {
                    return (this.ExceptionList != null);
                }
            }

            public bool IsStopped
            {
                get
                {
                    return (this.StopCount > 0);
                }
            }

            public bool ShouldExitCurrentIteration
            {
                get
                {
                    return (this.StopCount > 0);
                }
            }
        }
    }
}

