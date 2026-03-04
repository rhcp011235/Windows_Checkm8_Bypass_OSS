using System;
using Titan.Enumerates;

namespace Titan.Event
{
	public class DeviceRecoveryConnectEventArgs : EventArgs
	{
		private byte[] devicePtr;

		private ConnectNotificationMessage message;

		public byte[] DevicePtr => devicePtr;

		public ConnectNotificationMessage Message => message;

		public DeviceRecoveryConnectEventArgs(byte[] devicePtr, ConnectNotificationMessage message)
		{
			this.devicePtr = devicePtr;
			this.message = message;
		}
	}
}
