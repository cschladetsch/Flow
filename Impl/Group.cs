// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Impl
{
    /// <summary>
    ///     A flow Group contains a collection of other Transients, and fires events when the contents
    ///     of the group changes.
    ///     Suspending a Group suspends all contained Generators, and Resuming a Group
    ///     Resumes all contained Generators.
    ///		Stepping a group does nothing.
    /// </summary>
    internal class Group : Generator<bool>, IGroup
    {
        public event GroupHandler Added;
        public event GroupHandler Removed;

        public bool Empty => _contents.Count == 0;

        public IEnumerable<ITransient> Contents => _contents;

        public IEnumerable<IGenerator> Generators => Contents.OfType<IGenerator>();

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

            // TODO: do we really need to copy?
            foreach (var gen in Generators.ToArray())
            {
                gen.Pre();
            }
        }

        public override void Post()
        {
            base.Post();

            // TODO: do we really need to copy?
            foreach (var gen in Generators.ToArray())
            {
                gen.Post();
            }
        }

        public void Add(params ITransient[] others)
        {
            foreach (var other in others)
            {
                DeferAdd(other);
            }
        }

        protected void DeferAdd(ITransient other)
        {
            if (other == null)
                return;

            Deletions.RemoveRef(other);
            Additions.Add(other);
        }

        public void Remove(ITransient other)
        {
            if (other == null)
                return;

            Additions.RemoveRef(other);
            Deletions.Add(other);
        }

        public void Clear()
        {
            Additions.Clear();

            foreach (var tr in Contents)
            {
                Deletions.Add(tr);
            }

            PerformRemoves();
        }

        private void ForEachGenerator(Action<IGenerator> act)
        {
            foreach (var gen in Generators)
            {
                act(gen);
            }
        }

        protected void PerformPending()
        {
            PerformAdds();
            PerformRemoves();
        }

        private void PerformRemoves()
        {
            if (Deletions.Count == 0)
                return;

            foreach (var tr in Deletions.ToList())
            {
                _contents.RemoveRef(tr);
                if (tr == null)
                    continue;

                Kernel.Trace.Log("Removing {0} from Node {1}", tr, Name);

                tr.Completed -= Remove;

                Removed?.Invoke(this, tr);
            }

            Deletions.Clear();
        }

        private void PerformAdds()
        {
            foreach (var tr in Additions)
            {
                _contents.Add(tr);
                Kernel.Trace.Log("Adding {0} to Node {1}", tr, Name);
                tr.Completed += Remove;
                Added?.Invoke(this, tr);
            }

            Additions.Clear();
        }

        protected readonly List<ITransient> Additions = new List<ITransient>();
        protected readonly List<ITransient> Deletions = new List<ITransient>();
        protected readonly List<ITransient> _contents = new List<ITransient>();
    }
}
