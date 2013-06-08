// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Text;

namespace Flow
{
	internal abstract class Generator<TR> : Transient, ITypedGenerator<TR>
	{
		/// <inheritdoc />
		public TR Value { get; protected set; }

		/// <inheritdoc />
		public event GeneratorHandler Suspended;

		/// <inheritdoc />
		public event GeneratorHandler Resumed;

		/// <inheritdoc />
		public event GeneratorHandler Stepped;

		/// <inheritdoc />
		public bool Running { get; private set; }

		/// <inheritdoc />
		public int StepNumber { get; private set; }

		/// <inheritdoc />
		public virtual void Step()
		{
			++StepNumber;

			if (Stepped != null)
				Stepped(this);
		}

		/// <inheritdoc />
		public virtual void Post()
		{
		}

		/// <inheritdoc />
		public void Suspend()
		{
			if (!Running || !Active)
				return;
			
			Running = false;
			
			if (Suspended != null)
				Suspended(this);
		}

		/// <inheritdoc />
		public void Resume()
		{
			if (Running || !Active)
				return;
			
			Running = true;

			if (Resumed != null)
				Resumed(this);
		}

		/// <inheritdoc />
		public void SuspendAfter (ITransient other)
		{
			if (Transient.IsNullOrEmpty(other))
			{
				Suspend();
				return;
			}

			Resume();

			other.Completed += tr => Suspend();
		}

		/// <inheritdoc />
		public bool ResumeAfter(ITransient other)
		{
			if (Transient.IsNullOrEmpty(other))
			{
				Resume();
				return true;
			}

			Suspend();

			other.Completed += tr => Resume();

			return true;
		}

		/// <inheritdoc />
		public bool ResumeAfter(TimeSpan span)
		{
			if (!Active)
				return false;

			ResumeAfter(Factory.NewTimer(span));

			return true;
		}

		/// <inheritdoc />
		public bool SuspendAfter(TimeSpan span)
		{
			if (!Active)
				return false;

			SuspendAfter(Factory.NewTimer(span));

			return true;
		}

		internal Generator()
		{
			Completed += tr => Suspend();
		}
	}
}
