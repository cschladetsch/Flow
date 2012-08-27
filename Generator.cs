// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Text;

namespace Flow
{
    internal class Generator<TR> : Transient, ITypedGenerator<TR>
    {
        public TR Value { get; protected set; }

        public event GeneratorHandler Suspended;

        public event GeneratorHandler Resumed;

        public event GeneratorHandler Stepped;

        public bool Running { get; private set; }

		public int StepNumber { get; private set; }

        public virtual bool Step()
		{
			++StepNumber;

			if (Stepped != null)
				Stepped(this);

			return true;
		}

		public virtual void Post()
		{
		}

        public void Suspend()
        {
            if (!Running || !Exists)
                return;
            
            Running = false;
            
            if (Suspended != null)
                Suspended(this);
        }

        public void Resume()
        {
            if (Running || !Exists)
                return;
            
            Running = true;

            if (Resumed != null)
                Resumed(this);
        }

        public void SuspendAfter(ITransient transient)
        {
            Resume();

            transient.Deleted += tr => Suspend();
        }

        public bool ResumeAfter(ITransient transient)
		{
			if (transient == null || !transient.Exists) 
			{
				Resume();
				return true;
			}

			Suspend();

            transient.Deleted += tr => Resume();

			return true;
        }

		public bool ResumeAfter(TimeSpan span)
		{
			if (!Exists)
				return false;

			ResumeAfter(Factory.NewTimer(span));

			return true;
		}

		public bool SuspendAfter(TimeSpan span)
		{
			if (!Exists)
				return false;

			SuspendAfter(Factory.NewTimer(span));

			return true;
		}
		
		public IFuture<TR2> GetNext<TR2>(ITypedGenerator<TR2> gen)
		{
			var future = Kernel.Factory.NewFuture<TR2>();
			GeneratorHandler del;
			del = delegate(IGenerator tr) { GenStepped<TR2>(gen, future, del); };
			gen.Stepped += del;
			return future;
		}

		void GenStepped<TR2>(ITypedGenerator<TR2> gen, IFuture<TR2> future, GeneratorHandler del)
		{
			gen.Stepped -= del;
			future.Value = gen.Value;
		}
    }
}
