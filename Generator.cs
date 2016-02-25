using System;

namespace Flow
{
	internal abstract class Generator : Transient, IGenerator
	{
		internal Generator()
		{
			Completed += tr => Suspend();
		}

		/// <inheritdoc />
		public event GeneratorHandler Suspended;

		/// <inheritdoc />
		public event GeneratorHandler Resumed;

		/// <inheritdoc />
		public event GeneratorHandler Stepped;

		/// <inheritdoc />
		public bool Running { get; private set; }

		/// <inheritdoc />
		public int StepNumber { get; protected set; }

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
		public void SuspendAfter(ITransient other)
		{
			if (IsNullOrEmpty(other))
			{
				Suspend();
				return;
			}

			Resume();

			// thanks to https://github.com/innostory for reporting an issue
			// where a dangling reference to 'other' resulted in memory leaks.
			TransientHandler action = null;
			action = tr =>
			{
				other.Completed -= action;
				Suspend();
			};

			other.Completed += action;
		}

		/// <inheritdoc />
		public bool ResumeAfter(ITransient other)
		{
			if (IsNullOrEmpty(other))
			{
				Resume();
				return true;
			}

			Suspend();

			// thanks to https://github.com/innostory for reporting an issue
			// where a dangling reference to 'other' resulted in memory leaks.
			TransientHandler onCompleted = null;
			onCompleted = tr =>
			{
				other.Completed -= onCompleted;
				Resume();
			};

			other.Completed += onCompleted;

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
	}
}
