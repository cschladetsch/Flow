﻿using System;
using System.Collections.Generic;
using Dekuple.Model;
using UniRx;

namespace Dekuple
{
    public static class CollectionExtentions
    {
        public static void AddReactive<T>(this ICollection<T> coll, T val)
            where T : class, IHasDestroyHandler<IModel>
        {
            coll.Add(val);

            void Remove(IHasDestroyHandler<IModel> tr)
            {
                val.OnDestroyed -= Remove;  // remove dangling reference
                coll.Remove(val);
                UnityEngine.Debug.Log($"Destroyed {val}. {coll.Count} items remain");
            }

            val.OnDestroyed += Remove;
        }

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
