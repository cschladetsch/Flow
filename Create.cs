// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

#pragma warning disable 1685

namespace Flow
{
	/// <summary>
	/// Boot-strapper for the flow library using default implementations
	/// </summary>
	public static class Create
	{
		/// <summary>
		/// Create a new Kernel and Factory
		/// </summary>
		/// <returns>
		/// The kernel.
		/// </returns>
		public static IKernel NewKernel()
		{
			return NewFactory().Kernel;
		}

		/// <summary>
		/// Create a new Kernel and Factory
		/// </summary>
		/// <returns>
		/// The factory.
		/// </returns>
		public static IFactory NewFactory()
		{
			var kernel = new Kernel();
			var factory = new Factory();

			kernel.Factory = factory;
			factory.Kernel = kernel;

			kernel.Root = new Node { Kernel = kernel };

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