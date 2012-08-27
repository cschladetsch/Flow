// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;
using System;

namespace Flow
{
	/// <summary>
	/// A flow Node contains a collection of other transients. When the node is stepped, it steps all referenced generators.
	/// </summary>
	internal class Node : Group, INode
	{
		public override bool Step()
		{
			if (_stepping)
				throw new ReentrancyException();

			_stepping = true;

			if (!base.Step())
				return false;

			foreach (var gen in Generators)
				gen.Step();

			_stepping = false;

			return true;
		}

		public override void Post()
		{
			base.Post();

			// make a copy so that contents can be changed during iteration
			var list = new List<IGenerator>();
			foreach (var gen in Generators)
				list.Add(gen);

			// do post for all contained generators
			foreach (var gen in list)
				gen.Post();
		}

		bool _stepping;
	}
}