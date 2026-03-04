using System;
using System.Runtime.InteropServices;
using Titan.Enumerates;

namespace Titan.Struct
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct AMDeviceNotificationCallbackInfo
	{
		internal IntPtr DevicePtr;

		public ConnectNotificationMessage Msg;

		public uint NotificationId;
	}
}
