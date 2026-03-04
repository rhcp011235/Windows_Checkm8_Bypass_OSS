using System;

namespace Titan.CoreFundation
{
	internal struct CFRange
	{
		internal IntPtr Location;

		internal IntPtr Length;

		internal CFRange(int location, int len)
			: this((IntPtr)location, (IntPtr)len)
		{
		}

		internal CFRange(IntPtr location, IntPtr len)
		{
			Location = location;
			Length = len;
		}
	}
}
