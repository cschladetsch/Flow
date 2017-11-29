// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow.Impl
{
	public class Transient : ITransient
	{
		public event NamedHandler NewName;
		public event TransientHandler Completed;
		public event TransientHandlerReason WhyCompleted;

		public static bool DebugTrace;
		public bool Active { get; private set; }
		public IKernel Kernel { get; /*internal*/ set; }
		public IFactory Factory { get { return Kernel.Factory; } }

		public virtual string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
					return;

				if (NewName != null)
					NewName(this, _name, value);

				_name = value;
			}
		}

		public Transient()
		{
			Active = true;
		}

		public ITransient Named(string name)
		{
			Name = name;
			return this;
		}

		public void Complete()
		{
			if (!Active)
				return;

			Active = false;

			if (Completed != null)
				Completed(this);
		}

		public void CompleteAfter(ITransient other)
		{
			if (!Active)
				return;

			if (other == null)
				return;

			if (!other.Active)
			{
				Complete();
				return;
			}

			other.Completed += tr => CompletedBecause(other);
		}

		public void CompleteAfter(TimeSpan span)
		{
			CompleteAfter(Factory.OneShotTimer(span));
		}

		public static bool IsNullOrInactive(ITransient other)
		{
			return other == null || !other.Active;
		}

		private void CompletedBecause(ITransient other)
		{
			if (!Active)
				return;

			if (WhyCompleted != null)
				WhyCompleted(this, other);

			Complete();
		}

		private string _name;
	}
}
