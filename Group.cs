// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A flow group contains a collection of other transients, and fires events when the contents of the group changes.
	/// </summary>
	internal class Group : Generator<bool>, IGroup
	{
		/// <inheritdoc />
		public event GroupHandler Added;

		/// <inheritdoc />
		public event GroupHandler Removed;
		
		/// <inheritdoc />
		public IEnumerable<ITransient> Contents { get { return _contents; } }
		
		/// <inheritdoc />
		public IEnumerable<IGenerator> Generators 
		{
			get 
			{
				foreach (var elem in Contents) 
				{
					var gen = elem as IGenerator;
					if (gen == null)
						continue;
					yield return gen;
				}
			}
		}

		internal Group()
		{
			Deleted += tr => Clear();
			Suspended += tr => ForEachGenerator(g => g.Suspend());
			Resumed += tr => ForEachGenerator(g => g.Resume());
		}

		void ForEachGenerator(Action<IGenerator> act)
		{
			foreach (var gen in Generators) 
				act(gen);
		}

		/// <inheritdoc />
		public void Clear()
		{
			_pendingAdds.Clear();

			foreach (var tr in Contents)
				_pendingRemoves.Add(tr);

			PerformPending();
		}

		/// <inheritdoc />
		public override bool Step()
		{
			return true;
		}

		/// <inheritdoc />
		public override void Post()
		{
			PerformPending();
		}

		/// <inheritdoc />
		public void Add(ITransient trans)
		{
			if (trans == null || !trans.Exists)
				return;

			if (Contents.ContainsRef(trans) || _pendingAdds.ContainsRef(trans))
				return;

			_pendingAdds.Add(trans);
		}

		/// <inheritdoc />
		public void Remove(ITransient trans)
		{
			if (trans == null || !trans.Exists)
				return;

			if (!Contents.ContainsRef(trans) || _pendingRemoves.ContainsRef(trans))
				return;

			_pendingRemoves.Add(trans);
		}

		protected bool PerformPending()
		{
			PerformAdds();

			PerformRemoves();

			return true;
		}

		void PerformRemoves()
		{
			foreach (var tr in _pendingRemoves) 
			{
				_contents.RemoveRef(tr);
				tr.Deleted -= Remove;
				if (Removed != null)
					Removed(this, tr);
			}

			_pendingRemoves.Clear();
		}

		void PerformAdds()
		{
			foreach (var tr in _pendingAdds) 
			{
				_contents.Add(tr);
				tr.Deleted += Remove;
				if (Added != null)
					Added(this, tr);
			}

			_pendingAdds.Clear();
		}

		protected readonly List<ITransient> _pendingAdds = new List<ITransient>();
		
		protected readonly List<ITransient> _pendingRemoves = new List<ITransient>();

		private readonly List<ITransient> _contents = new List<ITransient>();
	}
}