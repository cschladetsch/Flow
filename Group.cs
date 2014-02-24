// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System.Collections.Generic;
using System.Linq;

namespace Flow
{
	/// <summary>
	/// A flow Group contains a collection of other Transients, and fires events when the contents 
	/// of the group changes.
	/// 
	/// Suspending a Group suspends all contained Generators, and Resuming a Group
	/// Resumes all contained Generators.
	/// </summary>
	internal class Group : TypedGenerator<bool>, IGroup
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
				return Contents.OfType<IGenerator>();
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
			Additions.Clear();

			// add all contents as pending deletions
			foreach (var tr in Contents)
				Deletions.Add(tr);

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
			if (IsNullOrEmpty(other))
				return;

			if (Contents.ContainsRef(other) || Additions.ContainsRef(other))
				return;

			Deletions.RemoveRef(other);
			Additions.Add(other);
		}

		/// <inheritdoc />
		public void Remove(ITransient other)
		{
			if (other == null)
				return;

			//if (!Contents.ContainsRef(other) || Deletions.ContainsRef(other))
			//	return;

			Additions.RemoveRef(other);
			Deletions.Add(other);
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
			foreach (var tr in Deletions.ToList()) 
			{
				_contents.RemoveRef(tr);
				if (tr == null)
					continue;
				//tr.Completed -= Remove;
				if (Removed != null)
					Removed(this, tr);
			}

			Deletions.Clear();
		}

		void PerformAdds()
		{
			foreach (var tr in Additions) 
			{
				_contents.Add(tr);
				//tr.Completed += Remove;
				if (Added != null)
					Added(this, tr);
			}

			Additions.Clear();
		}

		protected readonly List<ITransient> Additions = new List<ITransient>();
		
		protected readonly List<ITransient> Deletions = new List<ITransient>();

		private readonly List<ITransient> _contents = new List<ITransient>();
	}
}