// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Flow.Impl
{
	internal class Node : Group, INode
	{
		private bool _stepping;

		public override void Step()
		{
			try
			{
				if (_stepping)
				{
					throw new ReentrancyException();
				}

				_stepping = true;

				base.Step();

				foreach (var tr in Contents)
				{
					if (!tr.Active)
					{
						Remove(tr);
						continue;
					}

					var gen = tr as IGenerator;
					if (gen != null)
					{
						gen.Step();
					}
				}
			}
			finally
			{
				_stepping = false;
			}
		}

		public override void Post()
		{
			base.Post();

			// make a copy so that contents can be changed during iteration
			var list = Generators.ToArray();

			// do post for all contained generators
			foreach (var gen in list)
			{
				gen.Post();
			}
		}
	}

	class Conditional : Group
	{
		public Conditional(IGenerator test, IGenerator body)
		{
			Add(test, body);
			PerformPending();
		}

		public override void Step()
		{
			base.Step();

			if (_contents.Count != 2)
				return;

			var test = _contents[0];
			var body = _contents[1];

			var gen = test as IGenerator;
			if (gen == null)
				return;

			gen.Step();
			var s = (bool) gen.Value;
			if (!s)
				return;

			var action = body as IGenerator;
			if (action == null)
				return;

			action.Step();
		}
	}
}
