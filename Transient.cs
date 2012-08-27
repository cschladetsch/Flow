// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	internal class Transient : ITransient
	{
		/// <inheritdoc />
		public event TransientHandler Deleted;

		/// <inheritdoc />
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
		
		/// <inheritdoc />
		public IKernel Kernel { get; /*internal*/ set; }

		/// <inheritdoc />
		public IFactory Factory  { get { return Kernel.Factory; } }

		/// <inheritdoc />
		public event NamedHandler NewName;

		/// <inheritdoc />
		public bool Exists { get; private set; }

		internal Transient()
		{
			Exists = true;
		}

		/// <inheritdoc />
		public void Delete()
		{
			if (!Exists)
				return;

			if (Deleted != null)
				Deleted(this);

			Exists = false;
		}

		/// <inheritdoc />
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