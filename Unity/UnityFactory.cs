using System;
using System.Collections;

using UnityEngine;

namespace Flow.Impl.Unity
{
    public class Ref<T>
    {
        public Action<T> Set;
        public Func<T> Get;

        public Ref(Action<T> set, Func<T> get)
        {
            Get = get;
            Set = set;
        }
    }

    public interface IUnityFactory
    {
        IGenerator MoveTo<T>(Ref<T> src, T target, float seconds);
        //IGenerator Seek(Ref<Transform> src, Transform pos, float seconds);
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

     //   public IGenerator MoveTo<T>(Ref<T> src, T target, float seconds)
     //   {
	    //    dynamic s = src.Get();
	    //    dynamic tgt = target;

	    //    return Prepare(OverTime((t) => src.Set(Interp<T>(s, tgt, t)), seconds));
     //   }

	    //T Interp<T>(dynamic a, dynamic b, float t)
	    //{
		   // return a + (b - a) * t;
	    //}
	    public IGenerator MoveTo<T>(Ref<T> src, T target, float seconds)
	    {
		    throw new NotImplementedException();
	    }
    }
}

