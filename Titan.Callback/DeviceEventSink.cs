using System;
using System.Runtime.InteropServices;

namespace Titan.Callback
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DeviceEventSink(IntPtr str, IntPtr user);
}
