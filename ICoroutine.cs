namespace Flow
{
	/// <summary>
	/// A coroutine is a process that can resume after a yield.
	/// </summary>
    public interface ICoroutine<TR> : ITypedGenerator<TR>
    {
    }
}