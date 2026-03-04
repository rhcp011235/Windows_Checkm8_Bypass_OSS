using System.Runtime.InteropServices;

namespace Titan.Struct
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct AMRecoveryDevice
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] devicePtr;
	}
}
