<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

>>>>>>> 2156678... Updated to .Net4.5
using System.Linq;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Node
        : Group
        , INode
    {
        private bool _stepping;
        protected bool _StepOne;
=======
    internal class Node : Group, INode
    {
        //public void Add(params ITransient[] trans)
        //{
        //    base.Add(trans);
        //}
>>>>>>> 2156678... Updated to .Net4.5

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
<<<<<<< HEAD
                if (Kernel.DebugLevel > EDebugLevel.Medium)
                    Kernel.Log.Info($"Stepping Node {Name}");

                if (_stepping)
                {
                    Kernel.Log.Error(
                        $"Node {Name} is re-entrant. Nodes cannot directly or indirectly invoke their Step methods when stepping.");
=======
                if (Kernel.DebugLevel > EDebugLevel.High)
                {
                    Kernel.Log.Info($"Stepping Node {Name}");
                }

                if (_stepping)
                {
                    Kernel.Log.Error($"Node {Name} is re-entrant");
>>>>>>> 2156678... Updated to .Net4.5
                    throw new ReentrancyException();
                }

                _stepping = true;

                base.Step();

<<<<<<< HEAD
                // TODO: do we really need to copy the contents? Maybe use some double-buffering if required to avoid copying.
                // TODO: that said, it's only creating a new list of references...
                // TODO: the underlying issue is that the contents of the node may be altered while stepping children of the node.
                foreach (var tr in Contents.ToList())
                {
                    if (tr is IGenerator gen)
=======
                foreach (var tr in Contents.ToList())
                {
                    var gen = tr as IGenerator;
                    if (gen != null)
>>>>>>> 2156678... Updated to .Net4.5
                    {
                        if (Kernel.Break)
                            goto end;

                        if (!gen.Active)
                        {
                            Remove(gen);
<<<<<<< HEAD
                            continue;
                        }

                        if (gen.Running)
                            gen.Step();
                    }

                    if (_StepOne)
                        break;
                }
            }
            catch (Exception e)
            {
                Error($"Exception: {e.Message} when stepping {Name}. Completing this Node.");
                Complete();
            }
=======
                            break;
                        }

                        if (!gen.Running)
                        {
                            break;
                        }

                        gen.Step();
                        //gen = gen.Value as IGenerator;

                        //Kernel.StepTime();
                    }

                    if (_stepOne)
                        break;
                }
            }
>>>>>>> 2156678... Updated to .Net4.5
            finally
            {
                _stepping = false;
            }

        end:
            Post();
        }
<<<<<<< HEAD
=======

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
>>>>>>> 2156678... Updated to .Net4.5
    }
}
