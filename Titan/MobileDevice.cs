using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Titan.Callback;
using Titan.CoreFundation;
using Titan.Enumerates;
using Titan.Helper;

namespace Titan
{
	internal class MobileDevice
	{
		static MobileDevice()
		{
			string text = DLLHelper.GetiTunesMobileDeviceDllPath();
			string appleApplicationSupportFolder = DLLHelper.GetAppleApplicationSupportFolder();
			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(appleApplicationSupportFolder))
			{
				Environment.SetEnvironmentVariable("Path", string.Join(";", Environment.GetEnvironmentVariable("Path"), text, appleApplicationSupportFolder));
			}
		}

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCConnectionClose(IntPtr conn);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCConnectionGetFSBlockSize(IntPtr conn);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCConnectionInvalidate(IntPtr conn);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCConnectionIsValid(IntPtr conn);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCConnectionSetSecureContext(IntPtr device, IntPtr intResult);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCConnectionOpen(IntPtr socket, uint io_timeout, ref IntPtr conn);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCDeviceInfoOpen(IntPtr conn, ref IntPtr dict);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceInstallApplication(int socket, IntPtr path, IntPtr options, IntPtr callback, IntPtr unknow1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceTransferApplication(int socket, IntPtr path, IntPtr options, DeviceInstallApplicationCallback callback, IntPtr unknow1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceUninstallApplication(IntPtr conn, IntPtr bundleIdentifier, IntPtr installOption, IntPtr unknown0, IntPtr unknown1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceArchiveApplication(IntPtr conn, IntPtr bundleIdentifier, IntPtr options, DeviceInstallApplicationCallback callback);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDPostNotification(int socket, IntPtr NoticeMessage, uint uint_0);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDObserveNotification(int socket, IntPtr NoticeMessage);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceSecureStartService(IntPtr device, IntPtr service_name, IntPtr unknown, ref IntPtr socket);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDServiceConnectionGetSocket(IntPtr socket);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMDServiceConnectionGetSecureIOContext(IntPtr socket);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceLookupApplicationArchives(IntPtr conn, IntPtr AppType, ref IntPtr result);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceLookupApplications(IntPtr conn, IntPtr AppType, ref IntPtr result);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCDirectoryClose(IntPtr conn, IntPtr dir);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCDirectoryCreate(IntPtr conn, string path);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefLock(IntPtr conn, long long_0);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCDirectoryOpen(IntPtr conn, byte[] path, ref IntPtr dir);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCDirectoryOpen(IntPtr afcHandle, IntPtr path, ref IntPtr dir);

		public static int AFCDirectoryOpen(IntPtr conn, string path, ref IntPtr dir)
		{
			return AFCDirectoryOpen(conn, Encoding.UTF8.GetBytes(path), ref dir);
		}

		public static int AFCDirectoryRead(IntPtr conn, IntPtr dir, ref string buffer)
		{
			IntPtr zero = IntPtr.Zero;
			int num = AFCDirectoryRead(conn, dir, ref zero);
			if (num == 0 && zero != IntPtr.Zero)
			{
				int num2 = 0;
				List<byte> list = new List<byte>();
				for (; Marshal.ReadByte(zero, num2) > 0; num2++)
				{
					list.Add(Marshal.ReadByte(zero, num2));
				}
				buffer = Encoding.UTF8.GetString(list.ToArray());
				return num;
			}
			buffer = null;
			return num;
		}

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCGetFileInfo(IntPtr afcHandle, IntPtr path, ref IntPtr buffer, ref uint length);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCDirectoryRead(IntPtr afcHandle, IntPtr dir, ref IntPtr dirent);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileInfoOpen(IntPtr conn, byte[] path, ref IntPtr dict);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCFileInfoOpen(IntPtr afcHandle, IntPtr path, ref IntPtr info);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefClose(IntPtr conn, long handle);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefOpen(IntPtr conn, byte[] path, int mode, ref long handle);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCFileRefOpen(IntPtr afcHandle, IntPtr path, int mode, ref long handle);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefRead(IntPtr conn, long handle, byte[] buffer, ref uint len);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefSeek(IntPtr conn, long handle, uint pos, uint origin);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCFileRefSeek(IntPtr afcHandle, long handle, long pos, uint origin);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCFileRefUnlock(IntPtr afcHandle, long handle);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefSetFileSize(IntPtr conn, long handle, uint size);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefTell(IntPtr conn, long handle, ref uint position);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFileRefWrite(IntPtr conn, long handle, byte[] buffer, uint len);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCFlushData(IntPtr conn, long handle);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCKeyValueClose(IntPtr dict);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCKeyValueRead(IntPtr dict, ref IntPtr key, ref IntPtr val);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCRemovePath(IntPtr conn, byte[] path);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AFCRemovePath(IntPtr afcHandle, IntPtr path);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AFCRenamePath(IntPtr conn, byte[] old_path, byte[] new_path);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceActivate(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceConnect(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMDeviceCopyDeviceIdentifier(IntPtr device);

		public static object AMDeviceCopyValue(IntPtr device, string domain, string name)
		{
			object result = string.Empty;
			IntPtr intPtr = CoreFoundation.StringToCFString(domain);
			IntPtr intPtr2 = CoreFoundation.StringToCFString(name);
			IntPtr intPtr3 = AMDeviceCopyValue_Int(device, intPtr, intPtr2);
			if (intPtr != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				result = RuntimeHelpers.GetObjectValue(CoreFoundation.ManagedTypeFromCFType(ref intPtr3));
				CoreFoundation.CFRelease(intPtr3);
			}
			return result;
		}

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDeviceCopyValue")]
		public static extern IntPtr AMDeviceCopyValue_Int(IntPtr device, IntPtr domain, IntPtr cfstring);

		public static bool AMDeviceRemoveValue(IntPtr device, string domain, string name)
		{
			bool result = true;
			try
			{
				IntPtr intPtr = CoreFoundation.StringToCFString(domain);
				IntPtr intPtr2 = CoreFoundation.StringToCFString(name);
				if (AMDeviceRemoveValue_UInt32(device, intPtr, intPtr2) != 0)
				{
					result = false;
				}
				if (intPtr != IntPtr.Zero)
				{
					CoreFoundation.CFRelease(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					CoreFoundation.CFRelease(intPtr2);
					return result;
				}
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDeviceRemoveValue")]
		public static extern int AMDeviceRemoveValue_UInt32(IntPtr device, IntPtr domain, IntPtr cfsKey);

		public static bool AMDeviceSetValue(IntPtr device, string domain, string name, object value)
		{
			bool result = true;
			IntPtr intPtr = CoreFoundation.StringToCFString(domain);
			IntPtr intPtr2 = CoreFoundation.StringToCFString(name);
			IntPtr intPtr3 = CoreFoundation.CFTypeFromManagedType(value);
			if (AMDeviceSetValue_Int(device, intPtr, intPtr2, intPtr3) != 0)
			{
				result = false;
			}
			if (intPtr != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr3);
			}
			return result;
		}

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDeviceSetValue")]
		public static extern int AMDeviceSetValue_Int(IntPtr device, IntPtr domain, IntPtr cfsKey, IntPtr cfsValue);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceSetValue(IntPtr device, IntPtr mbz, IntPtr cfstringkey, IntPtr cfstringname);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceDeactivate(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceDisconnect(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceEnterRecovery(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceGetConnectionID(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceIsPaired(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceNotificationSubscribe(DeviceNotificationCallback callback, uint unused1, uint unused2, uint unused3, ref IntPtr am_device_notification_ptr);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AMDeviceNotificationUnsubscribe(IntPtr am_device_notification_ptr);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceRelease(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceStartService(IntPtr device, IntPtr service_name, ref int socket, IntPtr unknown);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void AMDeviceRetain(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceStartSession(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceStopSession(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceValidatePairing(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDevicePair(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AMDevicePairWithOptions(IntPtr device, IntPtr ptrOption);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceUnpair(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError AMDListenForNotifications(int socket, DeviceEventSink callback, IntPtr userdata);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMRecoveryModeDeviceCopyIMEI(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceGetLocationID(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceGetProductID(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceGetProductType(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceGetTypeID();

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeGetSoftwareBuildVersion();

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMRecoveryModeDeviceCopySerialNumber(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMRecoveryModeDeviceCopyAuthlnstallPreflightOptions(byte[] byte_0, IntPtr intptr_0, IntPtr intptr_1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceReboot(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRecoveryModeDeviceSetAutoBoot(byte[] device, byte paramByte);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern string AMRestoreModeDeviceCopyIMEI(byte[] device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRestoreModeDeviceCreate(uint unknown0, int connection_id, uint unknown1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRestoreRegisterForDeviceNotifications(DeviceDFUNotificationCallback dfu_connect, DeviceRestoreNotificationCallback recovery_connect, DeviceDFUNotificationCallback dfu_disconnect, DeviceRestoreNotificationCallback recovery_disconnect, uint unknown0, ref IntPtr user_info);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDeviceGetInterfaceType(IntPtr device);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDServiceConnectionReceive(IntPtr socket, ref uint uint_0, int int_1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDServiceConnectionReceive")]
		public static extern int AMDServiceConnectionReceive_1(IntPtr socket, IntPtr intptr_0, int int_1);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMRestorePerformRecoveryModeRestore(byte[] device, IntPtr dicopts, DeviceInstallApplicationCallback callback, IntPtr user_info);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr AMRestoreCreateDefaultOptions(IntPtr allocator);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDServiceConnectionReceive(int inSocket, IntPtr buffer, int bufferlen);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDServiceConnectionReceive")]
		public static extern int AMDServiceConnectionReceive_UInt32(int inSocket, ref uint buffer, int bufferlen);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AMDServiceConnectionSend(IntPtr inSocket, IntPtr buffer, int bufferlen);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AMDServiceConnectionSend")]
		public static extern int AMDServiceConnectionSend_UInt32(IntPtr inSocket, ref uint buffer, int bufferlen);

		[DllImport("MobileDevice.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int USBMuxConnectByPort(int connectionID, uint iPhone_port_network_byte_order, ref int outSocket);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint htonl(uint hostlong);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint htons(uint hostshort);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ntohl(uint netlong);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int send(int inSocket, IntPtr buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int send(int inSocket, byte[] buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "send")]
		public static extern int send_UInt32(int inSocket, ref uint buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern int recv(int inSocket, IntPtr buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern int recv(int inSocket, byte[] buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "recv")]
		public static extern int recv_UInt(int inSocket, ref uint buffer, int bufferlen, int flags);

		[DllImport("Ws2_32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int closesocket(int inSocket);

		public static IntPtr ATCFMessageCreate(int sesssion, string strMessageType, Dictionary<object, object> dictParams)
		{
			IntPtr intPtr = CoreFoundation.StringToCFString(strMessageType);
			IntPtr intPtr2 = CoreFoundation.CFTypeFromManagedType(dictParams);
			IntPtr result = ATCFMessageCreate_IntPtr(sesssion, intPtr, intPtr2);
			CoreFoundation.CFRelease(intPtr);
			CoreFoundation.CFRelease(intPtr2);
			return result;
		}

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ATCFMessageCreate")]
		public static extern IntPtr ATCFMessageCreate_IntPtr(int sesssion, IntPtr hMessageType, IntPtr hParams);

		public static IntPtr ATHostConnectionCreateWithLibrary(string strPrefsValue, string strUUID)
		{
			IntPtr intPtr = CoreFoundation.StringToCFString(strPrefsValue);
			IntPtr intPtr2 = CoreFoundation.StringToCFString(strUUID);
			IntPtr result = ATHostConnectionCreateWithLibrary_IntPtr(intPtr, intPtr2, 0);
			CoreFoundation.CFRelease(intPtr2);
			CoreFoundation.CFRelease(intPtr);
			return result;
		}

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ATHostConnectionCreateWithLibrary")]
		private static extern IntPtr ATHostConnectionCreateWithLibrary_IntPtr(IntPtr hPrefsValue, IntPtr hUUID, int unkonwn);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ATHostConnectionGetCurrentSessionNumber(IntPtr hATHost);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ATHostConnectionGetGrappaSessionId(IntPtr hATHost);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ATHostConnectionReadMessage(IntPtr hATHost);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ATHostConnectionRelease(IntPtr hATHost);

		public static int ATHostConnectionSendAssetCompleted(IntPtr hATHost, string strPid, string strMediaType, string strFilePath)
		{
			IntPtr intPtr = CoreFoundation.CFStringMakeConstantString(strPid);
			IntPtr intPtr2 = CoreFoundation.CFStringMakeConstantString(strMediaType);
			IntPtr intPtr3 = CoreFoundation.CFStringMakeConstantString(strFilePath);
			IntPtr intPtr4 = ATHostConnectionSendAssetCompleted_IntPtr(hATHost, intPtr, intPtr2, intPtr3);
			CoreFoundation.CFRelease(intPtr);
			CoreFoundation.CFRelease(intPtr2);
			CoreFoundation.CFRelease(intPtr3);
			return intPtr4.ToInt32();
		}

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ATHostConnectionSendAssetCompleted")]
		private static extern IntPtr ATHostConnectionSendAssetCompleted_IntPtr(IntPtr hATHost, IntPtr hPid, IntPtr hMediaType, IntPtr hFilePath);

		public static int ATHostConnectionSendFileError(IntPtr hATHost, string strPid, string strMediaType, int intType)
		{
			IntPtr intPtr = CoreFoundation.CFStringMakeConstantString(strPid);
			IntPtr intPtr2 = CoreFoundation.CFStringMakeConstantString(strMediaType);
			IntPtr intPtr3 = (IntPtr)(int)ATHostConnectionSendFileError_IntPtr(hATHost, intPtr, intPtr2, intType);
			CoreFoundation.CFRelease(intPtr);
			CoreFoundation.CFRelease(intPtr2);
			return intPtr3.ToInt32();
		}

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ATHostConnectionSendFileError")]
		private static extern kAMDError ATHostConnectionSendFileError_IntPtr(IntPtr hATHost, IntPtr hPid, IntPtr hMediaType, int intType);

		public static int ATHostConnectionSendFileProgress(IntPtr hATHost, string strPid, string strMediaType, double fileProgress, double totalProgress)
		{
			IntPtr intPtr = CoreFoundation.CFStringMakeConstantString(strPid);
			IntPtr intPtr2 = CoreFoundation.CFStringMakeConstantString(strMediaType);
			byte[] bytes = BitConverter.GetBytes(fileProgress);
			byte[] bytes2 = BitConverter.GetBytes(totalProgress);
			int proc1L = BitConverter.ToInt32(bytes, 0);
			int proc1H = BitConverter.ToInt32(bytes, 4);
			int proc2L = BitConverter.ToInt32(bytes2, 0);
			int proc2H = BitConverter.ToInt32(bytes2, 4);
			IntPtr intPtr3 = (IntPtr)(int)ATHostConnectionSendFileProgress_IntPtr(hATHost, intPtr, intPtr2, proc1L, proc1H, proc2L, proc2H);
			CoreFoundation.CFRelease(intPtr);
			CoreFoundation.CFRelease(intPtr2);
			return intPtr3.ToInt32();
		}

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ATHostConnectionSendFileProgress")]
		private static extern kAMDError ATHostConnectionSendFileProgress_IntPtr(IntPtr hATHost, IntPtr hPid, IntPtr hMediaType, int proc1L, int proc1H, int proc2L, int proc2H);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError ATHostConnectionSendHostInfo(IntPtr hATHost, IntPtr hDictInfo);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError ATHostConnectionSendMetadataSyncFinished(IntPtr hATHost, IntPtr hKeybag, IntPtr hMedia);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError ATHostConnectionSendPowerAssertion(IntPtr hATHost, IntPtr allocator);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern kAMDError ATHostConnectionSendSyncRequest(IntPtr hATHost, IntPtr hArrDataclasses, IntPtr hDictDataclassAnchors, IntPtr hDictHostInfo);

		[DllImport("AirTrafficHost.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ATProcessLinkSendMessage(IntPtr hATHost, IntPtr hATCFMessage);

		public static IntPtr CFStringMakeConstantString(string s)
		{
			return CoreFoundation.StringToCFString(s);
		}
	}
}
