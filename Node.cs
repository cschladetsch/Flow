// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Linq;
using System.Runtime.InteropServices;

namespace Flow.Impl
{
	internal class Node : Group, INode
	{
		private bool _stepping;

		public override void Step()
		{
			Pre();

			try
			{
				if (Kernel.DebugLevel >= EDebugLevel.High)
				{
					Kernel.Trace.Log("Stepping Node {0}", Name);	
				}

				if (_stepping)
				{
					Kernel.Trace.Error("Node {0} is re-entrant", Name);
					throw new ReentrancyException();
				}

				_stepping = true;

				base.Step();

				foreach (var tr in Contents.ToList())
				{
					var gen = tr as IGenerator;
					while (gen != null)
					{
						if (!gen.Active)
						{
							Remove(gen);
							break;
						}

						gen.Step();
						gen = gen.Value as IGenerator;
					}
				}
			}
			finally
			{
				_stepping = false;
			}

			Post();
		}

		public override void Pre()
		{
			base.Pre();
		}

		public override void Post()
		{
			base.Post();
		}
	}
}
