// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using System.Linq;

namespace Flow.Impl
{
    internal class Node
        : Group
        , INode
    {
        private bool _stepping;
        protected bool _StepOne;

        public void Add(params IGenerator[] gens)
        {
            foreach (var gen in gens)
                DeferAdd(gen);
        }

        public new INode AddTo(IGroup group) => this.AddToGroup<INode>(group);
        public new INode Named(string name) => this.SetName<INode>(name);

        public override void Step()
        {
            Pre();

            try
            {
                if (Kernel.DebugLevel > EDebugLevel.Medium)
                    Kernel.Log.Info($"Stepping Node {Name}");

                if (_stepping)
                {
                    Kernel.Log.Error(
                        $"Node {Name} is re-entrant. Nodes cannot directly or indirectly invoke their Step methods when stepping.");
                    throw new ReEntranceException();
                }

                _stepping = true;

                base.Step();

                // TODO: do we really need to copy the contents? Maybe use some double-buffering if required to avoid copying.
                // that said, it's only creating a new list of references...
                // the underlying issue is that the contents of the node may be altered while stepping children of the node.
                foreach (var tr in Contents.ToList())
                {
                    if (tr is IGenerator gen)
                    {
                        if (Kernel.Break)
                            goto end;

                        try
                        {
                            if (!gen.Active)
                            {
                                Remove(gen);
                                continue;
                            }

                            if (gen.Running)
                                gen.Step();
                        }
                        catch (Exception e)
                        {
                            gen.Complete();
                            Error($"Exception: {e.Message} when stepping {gen.Name}. Completing this generator.");
                            Error($"   StackTrace: {e.StackTrace}");
                        }
                    }

                    if (_StepOne)
                        break;
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
    }
}
