<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Impl
{
<<<<<<< HEAD
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
=======
    /// <summary>
    ///     A flow Group contains a collection of other Transients, and fires events when the contents
    ///     of the group changes.
    ///     Suspending a Group suspends all contained Generators, and Resuming a Group
    ///     Resumes all contained Generators.
    ///		Stepping a group does nothing.
    /// </summary>
    internal class Group : Generator<bool>, IGroup
>>>>>>> 2156678... Updated to .Net4.5
    {
        public event GroupHandler Added;
        public event GroupHandler Removed;

<<<<<<< HEAD
        public bool Empty => _Contents.Count == 0;
        public IEnumerable<ITransient> Contents => _Contents;
        public IEnumerable<IGenerator> Generators => Contents.OfType<IGenerator>();

        protected readonly List<ITransient> Additions = new List<ITransient>();
        protected readonly List<ITransient> Deletions = new List<ITransient>();
        protected readonly List<ITransient> _Contents = new List<ITransient>();
=======
        public bool Empty => _contents.Count == 0;

        public IEnumerable<ITransient> Contents => _contents;

        public IEnumerable<IGenerator> Generators => Contents.OfType<IGenerator>();
>>>>>>> 2156678... Updated to .Net4.5

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

<<<<<<< HEAD
            // TODO: do we really need to copy? Answer: yes (?) because the Pre() may change contents of this
            foreach (var gen in Generators.ToArray())
                gen.Pre();
=======
            // TODO: do we really need to copy?
            foreach (var gen in Generators.ToArray())
            {
                gen.Pre();
            }
>>>>>>> 2156678... Updated to .Net4.5
        }

        public override void Post()
        {
            base.Post();

<<<<<<< HEAD
            // TODO: do we really need to copy? Answer: yes (?) because the Pre() may change contents of this
            foreach (var gen in Generators.ToArray())
                gen.Post();
=======
            // TODO: do we really need to copy?
            foreach (var gen in Generators.ToArray())
            {
                gen.Post();
            }
>>>>>>> 2156678... Updated to .Net4.5
        }

        public void Add(IEnumerable<ITransient> others)
        {
            foreach (var other in others)
            {
<<<<<<< HEAD
                if (other == null)
                {
                    Warn($"Attempt to add null value to Barrier {this}");
                    continue;
                }

=======
>>>>>>> 2156678... Updated to .Net4.5
                DeferAdd(other);
            }
        }

        public void Add(params ITransient[] others)
        {
            Add(others.ToList());
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
<<<<<<< HEAD
                Deletions.Add(tr);
=======
            {
                Deletions.Add(tr);
            }
>>>>>>> 2156678... Updated to .Net4.5

            PerformRemoves();
        }

        private void ForEachGenerator(Action<IGenerator> act)
        {
            foreach (var gen in Generators)
<<<<<<< HEAD
                act(gen);
=======
            {
                act(gen);
            }
>>>>>>> 2156678... Updated to .Net4.5
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
<<<<<<< HEAD
                _Contents.RemoveRef(tr);
                if (tr == null)
                    continue;

                Kernel.Log.Verbose(10, "Removing {0} from Node {1}", tr, Name);
=======
                _contents.RemoveRef(tr);
                if (tr == null)
                    continue;

                Kernel.Log.Info("Removing {0} from Node {1}", tr, Name);
>>>>>>> 2156678... Updated to .Net4.5

                tr.Completed -= Remove;

                Removed?.Invoke(this, tr);
            }

            Deletions.Clear();
        }

        private void PerformAdds()
        {
            foreach (var tr in Additions)
            {
<<<<<<< HEAD
                _Contents.Add(tr);
                Verbose(10, $"Adding {tr} to {this}");
=======
                _contents.Add(tr);
                Verbose(30, $"Adding {tr} to {this}");
>>>>>>> 2156678... Updated to .Net4.5
                tr.Completed += Remove;
                Added?.Invoke(this, tr);
            }

            Additions.Clear();
        }
<<<<<<< HEAD
=======

        protected readonly List<ITransient> Additions = new List<ITransient>();
        protected readonly List<ITransient> Deletions = new List<ITransient>();
        protected readonly List<ITransient> _contents = new List<ITransient>();
>>>>>>> 2156678... Updated to .Net4.5
    }
}
