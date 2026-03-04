using System.Runtime.InteropServices;

namespace Titan.Struct
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct AMDFUModeDevice
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] data;
	}
}
