using System;
using System.Runtime.InteropServices;

namespace Titan.Callback
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DeviceInstallApplicationCallback(IntPtr dict);
}
