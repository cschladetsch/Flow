namespace Flow
{
	/// <summary>
	/// Bootstrapper for the flow library using default implementations
	/// </summary>
    public static class Global
    {
		/// <summary>
		/// News the kernel.
		/// </summary>
		/// <returns>
		/// The kernel.
		/// </returns>
        public static IKernel NewKernel()
        {
            return NewFactory().Kernel;
        }

		/// <summary>
		/// News the factory.
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