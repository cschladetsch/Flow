// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// A transient can be Delete()'d, after which its Exists property will always return false.
	/// </summary>
	internal class Transient : ITransient
	{
		public event TransientHandler Deleted;

		public string Name 
		{ 
			get 
			{
				return _name;
			}
			set 
			{
				if (_name == value)
					return;

				if (NewName != null)
					NewName(this, _name, value);
				_name = value;
			}
		}
		
		public IKernel Kernel { get; /*internal*/ set; }

		public IFactory Factory  { get { return Kernel.Factory; } }

		public event NamedHandler NewName;

		public bool Exists { get; private set; }

		public Transient()
		{
			Exists = true;
		}

		public void Delete()
		{
			if (!Exists)
				return;

			if (Deleted != null)
				Deleted(this);

			Exists = false;
		}

		public void DeleteAfter(ITransient other)
		{
			if (!Exists)
				return;

			if (other == null)
				return;

			if (!other.Exists) 
			{
				Delete();
				return;
			}

			other.Deleted += tr => Delete();
		}

		private string _name;
	}
}