namespace Flow
{
	/// <summary>
	/// TODO
	/// </summary>
	internal class TimeFrame : ITimeFrame
	{
		public System.DateTime Last { get; internal set; }

		public System.DateTime Now { get; internal set; }

		public System.TimeSpan Delta { get; internal set; }
	}
}