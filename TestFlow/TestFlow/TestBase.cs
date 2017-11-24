using System;
using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestBase
	{
		protected IKernel _kernel;
		protected IFactory _factory;
		protected INode _root;

		[SetUp]
		public void Pre()
		{
			_kernel = Create.Kernel();
			_factory = _kernel.Factory;
			_root = _kernel.Root;
		}

		protected void Print(object q)
		{
			PrintFmt("{0}={1}", SymbolName.Get(() => q), q);
		}

		protected void PrintFmt(string fmt, params object[] args)
		{
#if UNITY
			UnityEngine.Debug.LogFormat(fmt, args);
#else
			Console.WriteLine(fmt, args);
#endif
		}

		protected void Step(int steps = 1)
		{
			for (var n = 0; n < steps; ++n)
			{
				_kernel.Step();
			}
		}
	}
}
