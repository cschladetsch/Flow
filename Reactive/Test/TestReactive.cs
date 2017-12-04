using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flow;
using NUnit.Framework;

using Flow.Impl;

namespace TestFlow.Editor
{
	[TestFixture()]
	public class TestReactiveProperty
	{
		[Test]
		public void TestChangingField()
		{

		}
	}

	public interface IObservable<T> : IChannel<T>
	{
		IChannel<T> Channel { get; }
	}

	internal class Observable<T> : Channel<T>, IObservable<T>
	{
		public IChannel<T> Channel { get; }
		private IFactory fact;

		public IGenerator<T> Subscribe(Action<T> fun)
		{
			var nextResult = fact.Coroutine(NextResult);
			var pull = fact.Coroutine(Next, nextResult);
			throw new NotImplementedException();
		}

		public enum EReturnType
		{
			None,
			Error,
			Value,
			Pending,
			Future,
			Complete,
		}

		struct ReturnValue<T>
		{
			public EReturnType result;
			public T Value { get { return Future.Value; } }
			public IFuture<T> Future;

			public ReturnValue(EReturnType ret)
			{
				Future = null;
				result = ret;
			}

			public ReturnValue(IFuture<T> f)
			{
				Future = f;
				result = EReturnType.Future;
			}
		}

		private IEnumerator Next<T>(IGenerator self, IGenerator<ReturnValue<T>> next)
		{
			while (true)
			{
				next.Step();
				var ret = next.Value;
				switch (ret.result)
				{
					case EReturnType.Future:
						yield return self.ResumeAfter(ret.Future);
						break;
					
				}

				yield return 0;
			}
		}

		private IEnumerator<ReturnValue<T>> NextResult(IGenerator self)
		{
			while (true)
			{
				var next = Channel.Extract();
				yield return new ReturnValue<T>(next);
				if (!Channel.Active)
					yield return new ReturnValue<T>(EReturnType.Complete);
				if (next.Available)
					yield return new ReturnValue<T>(next);
			}
		}
	}

	class Foo
	{
		public IObservable<int> num = new Observable<int>();
	}

	class FooWatchher
	{
		public FooWatchher(Foo foo)
		{
			Observable<int> n = new Observable<int>();
			n.Value = true;
			n.Subscribe(Console.WriteLine);
		}
	}
}
