namespace Flow
{
	/// <summary>
	/// An instance of IKernel contains a top-level root node, which contains all the transients in the IKernel.
	/// <para>
	/// When the Kernel is stepped, it Step()s the top-level root node.
	/// </para>
	/// <para>
	/// After that, it calls Stepped()'d on the top-level root node.
	/// </para>
	/// </summary>
    public interface IKernel : IGenerator
    {
		/// <summary>
		/// Gets the factory to use for making objects that reside within this kernel.
		/// </summary>
		/// <value>
		/// The factory to use to add things to this IKernel.
		/// </value>
        IFactory Factory { get; }

		/// <summary>
		/// Gets or sets the root of the kernel. When the Kernel is stepped, it will first Step() every generator reachable from Root, 
		/// then call Stepped() on all nodes reachable from the Root.
		/// </summary>
		/// <value>
		/// The root group.
		/// </value>
        INode Root { get; set; }
    }
}