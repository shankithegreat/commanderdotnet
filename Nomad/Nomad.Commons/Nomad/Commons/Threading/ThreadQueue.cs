namespace Nomad.Commons.Threading
{
    using Nomad.Commons;
    using System;
    using System.Threading;

    public class ThreadQueue : CustomThreadQueue
    {
        private ApartmentState Apartment;

        public ThreadQueue() : base(false)
        {
            this.Apartment = ApartmentState.Unknown;
        }

        public ThreadQueue(ApartmentState apartment) : base(false)
        {
            this.Apartment = apartment;
        }

        public ThreadQueue(ApartmentState apartment, bool persistent) : base(persistent)
        {
            this.Apartment = apartment;
        }

        protected override void StartWorkThread()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(this.DoWork));
            if (this.Apartment != ApartmentState.Unknown)
            {
                thread.SetApartmentState(this.Apartment);
            }
            thread.IsBackground = true;
            ErrorReport.RegisterThread(thread);
            thread.Start();
        }
    }
}

