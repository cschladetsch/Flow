<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

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

<<<<<<< HEAD
=======
        public static IKernel Kernel<TF>() where TF : class, IFactory, new()
        {
            return NewFactory<TF>().Kernel;
        }

>>>>>>> 2156678... Updated to .Net4.5
        public static IFactory NewFactory<TF>() where TF : class, IFactory, new()
        {
            var kernel = new Kernel();
            var factory = new TF();

            kernel.Factory = factory;
            kernel.Kernel = kernel;
            factory.Kernel = kernel;

            kernel.Root = new Node { Kernel = kernel, Name = "Root" };

            return factory;
        }
    }
}
<<<<<<< HEAD
=======

#if REQUIRE_EXTENSION_ATTRIBUTE
namespace System.Runtime.CompilerServices
{
	public class ExtensionAttribute : Attribute { }
}
#endif
>>>>>>> 2156678... Updated to .Net4.5
