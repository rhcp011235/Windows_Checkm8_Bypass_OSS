using System.Runtime.InteropServices;
using Titan.Struct;

namespace Titan.Callback
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DeviceNotificationCallback(ref AMDeviceNotificationCallbackInfo callback_info);
}
