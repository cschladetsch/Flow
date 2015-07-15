// CJS: Nabbed from decompiled 4.5 .Net

using System.Runtime.InteropServices;

namespace Flow
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct VolatileBool
	{
		public volatile bool m_value;

		public VolatileBool(bool value)
		{
			m_value = value;
		}
	}
}
