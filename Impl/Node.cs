// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System.Linq;

namespace Flow.Impl
{
    internal class Node : Group, INode
    {
        //public void Add(params ITransient[] trans)
        //{
        //    base.Add(trans);
        //}

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
                    Kernel.Log.Info($"Stepping Node {Name}");
                }

                if (_stepping)
                {
                    Kernel.Log.Error($"Node {Name} is re-entrant");
                    throw new ReentrancyException();
                }

                _stepping = true;

                base.Step();

                foreach (var tr in Contents.ToList())
                {
                    if (tr is IGenerator gen)
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
                            continue;
                        }

                        gen.Step();
                    }

                    if (_stepOne)
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

        private bool _stepping;
        protected bool _stepOne;
    }
}
