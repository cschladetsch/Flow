namespace Flow
{
	/// <summary>
	/// A barrier is a group that deletes itself when all added ITransients have been removed from it.
	/// </summary>
	public interface IBarrier : IGroup
	{
	}
}