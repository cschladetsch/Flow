using System;
using System.Collections.Generic;
using UniRx;

namespace Dekuple
{
    public static class CollectionExtentions
    {
        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public static void Shuffle<T>(this IList<T> list)
        {
            Shuffle(list, Rand);
        }

        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, rnd.Next(i, list.Count));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static void Connect<TTarget>(
            this IReadOnlyReactiveCollection<TTarget> self,
            Action<CollectionAddEvent<TTarget>> add,
            Action<CollectionRemoveEvent<TTarget>> remove)
        {
            self.ObserveAdd().Subscribe(add);
            self.ObserveRemove().Subscribe(remove);
        }
    }
}
