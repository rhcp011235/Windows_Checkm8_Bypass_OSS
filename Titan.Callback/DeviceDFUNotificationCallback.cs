using System.Runtime.InteropServices;
using Titan.Struct;

namespace Titan.Callback
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DeviceDFUNotificationCallback(ref AMDFUModeDevice callback_info);
}
