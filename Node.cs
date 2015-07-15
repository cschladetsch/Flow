// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Linq;

namespace Flow
{
	/// <summary>
	///     A flow Node contains a collection of other Transients. When the Node is stepped, it steps all referenced
	///     Generators.
	/// </summary>
	internal class Node : Group, INode
	{
		private bool _stepping;

		/// <inheritdoc />
		public override void Step()
		{
			try
			{
				if (_stepping)
				{
					//Utils.LogError("Node.Step: Name=" + Name);
					throw new ReentrancyException();
				}

				_stepping = true;

				base.Step();

				foreach (IGenerator gen in Generators)
				{
					if (!gen.Active)
						Remove(gen);
					else
						gen.Step();
				}
			}
			finally
			{
				_stepping = false;
			}
		}

		/// <inheritdoc />
		public override void Post()
		{
			base.Post();

			// make a copy so that contents can be changed during iteration
			IGenerator[] list = Generators.ToArray();

			// do post for all contained generators
			foreach (IGenerator gen in list)
			{
				gen.Post();
			}
		}
	}
}
