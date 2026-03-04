using System;
using Titan.Enumerates;

namespace Titan.Event
{
	public class DeviceCommonConnectEventArgs : EventArgs
	{
		private readonly iOSDevice device;

		private readonly ConnectNotificationMessage message;

		public iOSDevice Device => device;

		public ConnectNotificationMessage Message => message;

		internal DeviceCommonConnectEventArgs(iOSDevice device, ConnectNotificationMessage message)
		{
			this.device = device;
			this.message = message;
		}
	}
}
