using System;
using System.Collections.Generic;
using System.Linq;
using Titan.Callback;
using Titan.CoreFundation;
using Titan.Enumerates;
using Titan.Event;
using Titan.Struct;

namespace Titan
{
	public class iOSDeviceManager
	{
		private bool iSStartListen;

		private DeviceNotificationCallback deviceNotificationCallback;

		private DeviceRestoreNotificationCallback deviceRecoveryConnectedNotificationCallback;

		private DeviceRestoreNotificationCallback deviceRecoveryDisConnectedNotificationCallback;

		private DeviceDFUNotificationCallback deviceDFUConnectedNotificationCallback;

		private DeviceDFUNotificationCallback deviceDFUDisConnectedNotificationCallback;

		private List<iOSDevice> currentConnectedDevice = new List<iOSDevice>();

		public List<iOSDevice> CurrentConnectedDevice => currentConnectedDevice;

		public event DeviceCommonConnectEventHandler CommonConnectEvent;

		public event DeviceRecoveryConnectEventHandler RecoveryConnectEvent;

		public event EventHandler<ListenErrorEventHandlerEventArgs> ListenErrorEvent;

		private void NotificationCallback(ref AMDeviceNotificationCallbackInfo callback)
		{
			try
			{
				switch (callback.Msg)
				{
				case ConnectNotificationMessage.Connected:
				{
					iOSDevice iOSDevice4 = FindConnectedDevice(callback.DevicePtr);
					if (iOSDevice4 == null)
					{
						iOSDevice4 = new iOSDevice(callback.DevicePtr);
						currentConnectedDevice.Add(iOSDevice4);
					}
					this.CommonConnectEvent?.Invoke(this, new DeviceCommonConnectEventArgs(iOSDevice4, ConnectNotificationMessage.Connected));
					break;
				}
				case ConnectNotificationMessage.Disconnected:
				{
					iOSDevice iOSDevice5 = FindConnectedDevice(callback.DevicePtr);
					if (iOSDevice5 != null)
					{
						currentConnectedDevice.Remove(iOSDevice5);
					}
					this.CommonConnectEvent?.Invoke(this, new DeviceCommonConnectEventArgs(iOSDevice5, ConnectNotificationMessage.Disconnected));
					break;
				}
				}
			}
			catch (Exception ex)
			{
				this.ListenErrorEvent?.Invoke(this, new ListenErrorEventHandlerEventArgs(ex.Message, ListenErrorEventType.Connect));
			}
		}

		private iOSDevice FindConnectedDevice(IntPtr devicePtr)
		{
			return Enumerable.FirstOrDefault(currentConnectedDevice, (iOSDevice p) => p.DevicePtr == devicePtr);
		}

		private void DfuConnectCallback(ref AMDFUModeDevice callback)
		{
		}

		private void DfuDisconnectCallback(ref AMDFUModeDevice callback)
		{
		}

		private void RecoveryConnectCallback(ref AMRecoveryDevice callback)
		{
			this.RecoveryConnectEvent?.Invoke(this, new DeviceRecoveryConnectEventArgs(callback.devicePtr, ConnectNotificationMessage.Connected));
		}

		private void RecoveryDisconnectCallback(ref AMRecoveryDevice callback)
		{
			this.RecoveryConnectEvent?.Invoke(this, new DeviceRecoveryConnectEventArgs(callback.devicePtr, ConnectNotificationMessage.Disconnected));
		}

		public void StartListen()
		{
			if (iSStartListen)
			{
				return;
			}
			iSStartListen = true;
			try
			{
				deviceNotificationCallback = NotificationCallback;
				deviceDFUConnectedNotificationCallback = DfuConnectCallback;
				deviceRecoveryConnectedNotificationCallback = RecoveryConnectCallback;
				deviceDFUDisConnectedNotificationCallback = DfuDisconnectCallback;
				deviceRecoveryDisConnectedNotificationCallback = RecoveryDisconnectCallback;
				IntPtr zero = IntPtr.Zero;
				kAMDError kAMDError = (kAMDError)MobileDevice.AMDeviceNotificationSubscribe(deviceNotificationCallback, 0u, 1u, 0u, ref zero);
				if (kAMDError > kAMDError.kAMDSuccess)
				{
					this.ListenErrorEvent?.Invoke(this, new ListenErrorEventHandlerEventArgs("AMDeviceNotificationSubscribe failed with error : " + kAMDError, ListenErrorEventType.StartListen));
				}
				IntPtr zero2 = IntPtr.Zero;
				kAMDError = (kAMDError)MobileDevice.AMRestoreRegisterForDeviceNotifications(deviceDFUConnectedNotificationCallback, deviceRecoveryConnectedNotificationCallback, deviceDFUDisConnectedNotificationCallback, deviceRecoveryDisConnectedNotificationCallback, 0u, ref zero2);
				if (kAMDError > kAMDError.kAMDSuccess)
				{
					this.ListenErrorEvent?.Invoke(this, new ListenErrorEventHandlerEventArgs("AMRestoreRegisterForDeviceNotifications failed with error : " + kAMDError, ListenErrorEventType.StartListen));
				}
				CoreFoundation.CFRunLoopRun();
			}
			catch (Exception ex)
			{
				if (this.ListenErrorEvent != null)
				{
					this.ListenErrorEvent(this, new ListenErrorEventHandlerEventArgs(ex.Message, ListenErrorEventType.StartListen));
				}
			}
		}
	}
}
