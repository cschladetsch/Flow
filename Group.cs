// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;

namespace Flow
{
	/// <summary>
	/// A flow Group contains a collection of other Transients, and fires events when the contents of the group changes.
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
			Resumed += tr => ForEachGenerator(g => g.Resume());
			Suspended += tr => ForEachGenerator(g => g.Suspend());
			Completed += tr => Clear();
		}

		/// <inheritdoc />
		public void Clear()
		{
			// all pending adds are aborted
			_adds.Clear();

			// add all contents as pending deletions
			foreach (var tr in Contents)
				_dels.Add(tr);

			// remove all contents
			PerformRemoves();
		}

		/// <inheritdoc />
		public override void Post()
		{
			PerformPending();
		}

		/// <inheritdoc />
		public void Add(ITransient other)
		{
			if (Transient.IsNullOrEmpty(other))
				return;

			if (Contents.ContainsRef(other) || _adds.ContainsRef(other))
				return;

			_dels.RemoveRef(other);
			_adds.Add(other);
		}

		/// <inheritdoc />
		public void Remove(ITransient other)
		{
			if (other == null)
				return;

			if (!Contents.ContainsRef(other) || _dels.ContainsRef(other))
				return;

			_adds.RemoveRef(other);
			_dels.Add(other);
		}

		void ForEachGenerator(Action<IGenerator> act)
		{
			foreach (var gen in Generators) 
				act(gen);
		}

		protected void PerformPending()
		{
			PerformAdds();
			PerformRemoves();
		}

		void PerformRemoves()
		{
			foreach (var tr in _dels) 
			{
				_contents.RemoveRef(tr);
				tr.Completed -= Remove;
				if (Removed != null)
					Removed(this, tr);
			}

			_dels.Clear();
		}

		void PerformAdds()
		{
			foreach (var tr in _adds) 
			{
				_contents.Add(tr);
				tr.Completed += Remove;
				if (Added != null)
					Added(this, tr);
			}

			_adds.Clear();
		}

		protected readonly List<ITransient> _adds = new List<ITransient>();
		
		protected readonly List<ITransient> _dels = new List<ITransient>();

		private readonly List<ITransient> _contents = new List<ITransient>();
	}
}