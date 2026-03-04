using System.Runtime.InteropServices;
using Titan.Struct;

namespace Titan.Callback
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void DeviceRestoreNotificationCallback(ref AMRecoveryDevice callback_info);
}
