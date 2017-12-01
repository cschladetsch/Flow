using System;
using NUnit.Framework;
using UnityEngine;

namespace Flow.Impl.Unity.Test
{
    [TestFixture]
    public class MoveToTest
    {
        [Test]
        public void TestMoveTo()
        {
            var k = new Kernel();
            var r = new Node();
            var flow = new UnityFactory();
            k.Root = r;
            k.Factory = flow;

            Vector3 src = Vector3.zero;
            Vector3 target = new Vector3(5, 5, 5);
            var t = MakeRef(ref src);

            r.Add(flow.MoveTo(t, target, 1f));
        }

        [Test]
        public void TestRef()
        {
            int n = 42;
            var s = new Storage<int>(ref n);
            s.Set(12);

        }

        struct Storage<T>
        {
            object val;

            public Storage(ref T v)
            {
                val = v;

                var type = typeof(T);
                //type.UnderlyingSystemType.
            }

            public void Set(T x)
            {
                val = (T)x;
            }

            public T Get()
            {
                return (T)val;
            }
        }

        Ref<T> MakeRef<T>(ref T src)
        {
            Storage<T> store = new Storage<T>(ref src);

            return new Ref<T>(
                (val) => SetVal(store, val),
                () => GetVal(store));
        }

        void SetVal<T>(Storage<T> src, T val)
        {
            src.Set(val);
        }

        T GetVal<T>(Storage<T> src)
        {
            return src.Get();
        }
    }
}
