// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc cref="Generator" />
    /// <summary>
    /// A flow Group contains a collection of other Transients, and fires events when the contents
    /// of the group changes.
    /// Suspending a Group suspends all contained Generators, and Resuming a Group
    /// Resumes all contained Generators.
    /// Stepping a group does nothing.
    /// </summary>
    internal class Group
        : Generator<bool>
        , IGroup
    {
        public event GroupHandler OnAdded;
        public event GroupHandler OnRemoved;

        public IList<ITransient> Contents => _Contents;
        public IEnumerable<IGenerator> Generators => Contents.OfType<IGenerator>();
        public bool Empty => !_Contents.Any();

        protected readonly List<ITransient> _Additions = new List<ITransient>();
        protected readonly List<ITransient> _Deletions = new List<ITransient>();
        protected readonly List<ITransient> _Contents = new List<ITransient>();

        internal Group()
        {
            Resumed += tr => ForEachGenerator(g => g.Resume());
            Suspended += tr => ForEachGenerator(g => g.Suspend());
            Completed += tr => Clear();
        }

        public override void Pre()
        {
            base.Pre();

            PerformPending();

            // Do we really need to copy? Answer: yes (?) because the
            // Pre() may change contents of this.
            foreach (var gen in Generators.ToArray())
                gen.Pre();
        }

        public override void Post()
        {
            base.Post();

            foreach (var gen in Generators.ToArray())
                gen.Post();
        }

        public void Add(IEnumerable<ITransient> others)
        {
            foreach (var other in others)
            {
                if (IsNullOrInactive(other))
                {
                    Warn($"Attempt to add null or inactive transient to Group {this}");
                    continue;
                }

                DeferAdd(other);
            }
        }

        public void Add(params ITransient[] others)
            => Add(others.ToList());

        protected void DeferAdd(ITransient other)
        {
            if (other == null)
                return;

            _Deletions.RemoveRef(other);
            _Additions.Add(other);
        }

        public void Remove(ITransient other)
        {
            if (other == null)
                return;

            _Additions.RemoveRef(other);
            _Deletions.Add(other);
        }

        public void Clear()
        {
            _Additions.Clear();

            foreach (var tr in Contents)
                _Deletions.Add(tr);

            PerformRemoves();
        }

        private void ForEachGenerator(Action<IGenerator> act)
        {
            foreach (var gen in Generators)
                act(gen);
        }

        protected void PerformPending()
        {
            PerformAdds();
            PerformRemoves();
        }

        private void PerformRemoves()
        {
            if (_Deletions.Count == 0)
                return;

            foreach (var tr in _Deletions.ToList())
            {
                _Contents.RemoveRef(tr);
                if (tr == null)
                    continue;

                tr.Completed -= Remove;

                OnRemoved?.Invoke(this, tr);
            }

            _Deletions.Clear();
        }

        private void PerformAdds()
        {
            foreach (var tr in _Additions)
            {
                _Contents.Add(tr);
                tr.Completed += Remove;
                OnAdded?.Invoke(this, tr);
            }

            _Additions.Clear();
        }
    }
}
