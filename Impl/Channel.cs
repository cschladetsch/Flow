<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System.Collections.Generic;

namespace Flow.Impl
{
<<<<<<< HEAD
    internal class Channel<TR>
        : Subroutine<bool>
        , IChannel<TR>
    {
        private readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();
        private readonly Queue<TR> _values = new Queue<TR>();

=======
    internal class Channel<TR> : Subroutine<bool>, IChannel<TR>
    {
>>>>>>> 2156678... Updated to .Net4.5
        internal Channel(IKernel kernel)
        {
            Sub = StepChannel;
            Completed += tr => Close();
        }

        internal Channel(IKernel kernel, IGenerator<TR> gen)
            : this(kernel)
        {
            gen.Stepped += g => Insert(gen.Value);
            CompleteAfter(gen);
        }

        public IFuture<TR> Extract()
        {
            var future = Factory.Future<TR>();
            _requests.Enqueue(future);
            return future;
        }

        public List<TR> ExtractAll()
        {
            Flush();

            var list = new List<TR>();
            while (_values.Count > 0)
<<<<<<< HEAD
                list.Add(_values.Dequeue());
=======
            {
                list.Add(_values.Dequeue());
            }
>>>>>>> 2156678... Updated to .Net4.5

            return list;
        }

        public void Insert(TR val)
        {
            _values.Enqueue(val);
        }

        public void Flush()
        {
            while (_values.Count > 0 && _requests.Count > 0)
<<<<<<< HEAD
                _requests.Dequeue().Value = _values.Dequeue();
=======
            {
                _requests.Dequeue().Value = _values.Dequeue();
            }
>>>>>>> 2156678... Updated to .Net4.5
        }

        internal void Close()
        {
            Flush();

            foreach (var f in _requests)
<<<<<<< HEAD
                f.Complete();
=======
            {
                f.Complete();
            }
>>>>>>> 2156678... Updated to .Net4.5
        }

        private bool StepChannel(IGenerator self)
        {
            Flush();

            return true;
        }
<<<<<<< HEAD
=======

        private readonly Queue<IFuture<TR>> _requests = new Queue<IFuture<TR>>();
        private readonly Queue<TR> _values = new Queue<TR>();
>>>>>>> 2156678... Updated to .Net4.5
    }
}
