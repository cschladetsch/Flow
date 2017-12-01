using System;
using System.Collections;

using UnityEngine;

namespace Flow.Impl.Unity
{
    public class Ref<T>
    {
        public Action<T> set;
        public Func<T> get;

        public Ref(Action<T> set, Func<T> get)
        {
            this.get = get;
            this.set = set;
        }
    }

    public interface IUnityFactory
    {
        IGenerator MoveTo(Ref<Vector3> src, Vector3 pos, float seconds);
    }

    public class UnityFactory : Factory, IUnityFactory
    {
        public IGenerator OverTime(Action<float> action, float seconds)
        {
            return Prepare(Coroutine(OverTimeCoro, action, seconds));
        }

        IEnumerator OverTimeCoro(IGenerator self, Action<float> action, float seconds)
        {
            float total = seconds;
            while (seconds > 0)
            {
                var alpha = 1f - seconds/total;

                action(alpha);

                yield return 0;

                seconds -= (float)self.Kernel.Time.Delta.TotalSeconds;
            }
        }

        public IGenerator MoveTo(Ref<Vector3> src, Vector3 target, float seconds)
        {
            return Prepare(OverTime((t) => src.set((target - src.get())*t), seconds));
        }
    }
}
