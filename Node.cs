// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Linq;
using System.Runtime.InteropServices;

namespace Flow.Impl
{
	internal class Node : Group, INode
	{
		public new void Add(params ITransient[] trans)
		{
            base.Add(trans);
		}

		public void Add(params IGenerator[] gens)
		{
			foreach (var gen in gens)
				DeferAdd(gen);
		}

		public override void Step()
		{
			Pre();

			try
			{
				if (Kernel.DebugLevel > EDebugLevel.High)
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
						if (Kernel.Break)
							goto end;

						if (!gen.Active)
						{
							Remove(gen);
							break;
						}

						if (!gen.Running)
						{
							break;
						}

						gen.Step();
						gen = gen.Value as IGenerator;

						Kernel.StepTime();
					}
				}
			}
			finally
			{
				_stepping = false;
			}

		end:
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

		private bool _stepping;

	}
}
