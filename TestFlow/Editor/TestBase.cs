using NUnit.Framework;

namespace Flow.Test
{
	[TestFixture]
	public class TestBase
	{
		protected IKernel _kernel;
		protected INode _root;
		protected IFactory _factory;

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
            _kernel.Trace.Log(fmt, args);
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
