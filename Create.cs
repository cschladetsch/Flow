// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using Flow.Impl;

#pragma warning disable 1685

namespace Flow
{
	/// <summary>
	/// Boot-strapper for the flow library using default implementations
	/// </summary>
	public static class Create
	{
		public static IKernel Kernel()
		{
			return NewFactory<Factory>().Kernel;
		}

		public static IKernel Kernel<TF>() where TF : class, IFactory, new()
		{
			return NewFactory<TF>().Kernel;
		}

		public static IFactory NewFactory<TF>() where TF : class, IFactory, new()
		{
			var kernel = new Kernel();
			var factory = new TF();

			kernel.Factory = factory;
			factory.Kernel = kernel;

			kernel.Root = new Node {Kernel = kernel, Name = "Root"};

			return factory;
		}
	}
}

#if REQUIRE_EXTENSION_ATTRIBUTE
namespace System.Runtime.CompilerServices
{
	public class ExtensionAttribute : Attribute { }
}
#endif
