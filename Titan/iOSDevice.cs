using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Titan.CoreFundation;
using Titan.Enumerates;
using Titan.Unitiy;

namespace Titan
{
	public class iOSDevice
	{
		private bool isConnected;

		private bool isSessionOpen;

		private string activationState = string.Empty;

		private string basebandBootloaderVersion = string.Empty;

		private string basebandSerialNumber = string.Empty;

		private string basebandVersion = string.Empty;

		private string bluetoothAddress = string.Empty;

		private string buildVersion = string.Empty;

		private string cpuArchitecture = string.Empty;

		private string deviceColor = string.Empty;

		private string deviceName = string.Empty;

		private string firmwareVersion = string.Empty;

		private string hardwareModel = string.Empty;

		private string modelNumber = string.Empty;

		private string phoneNumber = string.Empty;

		private string productType = string.Empty;

		private string integratedCircuitCardIdentity = string.Empty;

		private string internationalMobileEquipmentIdentity = string.Empty;

		private string serialNumber = string.Empty;

		private string simStatus = string.Empty;

		private string uniqueDeviceID = string.Empty;

		private string uniqueChipID = string.Empty;

		private string regionInfo = string.Empty;

		private string wiFiAddress = string.Empty;

		private string productVersion = string.Empty;

		private int versionNumber;

		private string BBStatus = string.Empty;

		private string deviceClass = string.Empty;

		private string hardwarePlatform = string.Empty;

		private Dictionary<object, object> activationXml;

		private string deviceColorString = string.Empty;

		private string deviceColorBgString = string.Empty;

		private string internationalMobileEquipmentIdentity2 = string.Empty;

		private string model = string.Empty;

		private string mobileEquipmentIdentifier = string.Empty;

		public IntPtr DevicePtr;

		public IntPtr SocketContext;

		public IntPtr DeviceSecureIOContext;

		public bool IsConnected => isConnected;

		public string MobileEquipmentIdentifier
		{
			get
			{
				if (mobileEquipmentIdentifier.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.MobileEquipmentIdentifier);
					if (deviceValue != null)
					{
						mobileEquipmentIdentifier = deviceValue.ToString();
					}
				}
				return mobileEquipmentIdentifier;
			}
			set
			{
				mobileEquipmentIdentifier = MobileEquipmentIdentifier;
			}
		}

		public string InternationalMobileEquipmentIdentity2
		{
			get
			{
				if (internationalMobileEquipmentIdentity2.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.InternationalMobileEquipmentIdentity2);
					if (deviceValue != null)
					{
						internationalMobileEquipmentIdentity2 = deviceValue.ToString();
					}
				}
				return internationalMobileEquipmentIdentity2;
			}
			set
			{
				internationalMobileEquipmentIdentity2 = InternationalMobileEquipmentIdentity2;
			}
		}

		public string BasebandStatus
		{
			get
			{
				if (BBStatus.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BasebandStatus);
					if (deviceValue != null)
					{
						BBStatus = deviceValue.ToString();
					}
				}
				return BBStatus;
			}
		}

		public string HardwarePlatform
		{
			get
			{
				if (hardwarePlatform.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.HardwarePlatform);
					if (deviceValue != null)
					{
						hardwarePlatform = deviceValue.ToString();
					}
				}
				return hardwarePlatform;
			}
		}

		public string DeviceClass
		{
			get
			{
				if (deviceClass.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.DeviceClass);
					if (deviceValue != null)
					{
						deviceClass = deviceValue.ToString();
					}
				}
				return deviceClass;
			}
		}

		public string ActivationState
		{
			get
			{
				if (activationState.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.ActivationState);
					if (deviceValue != null)
					{
						activationState = deviceValue.ToString();
					}
				}
				return activationState;
			}
		}

		public string BasebandBootloaderVersion
		{
			get
			{
				if (basebandBootloaderVersion.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BasebandBootloaderVersion);
					if (deviceValue != null)
					{
						basebandBootloaderVersion = deviceValue.ToString();
					}
				}
				return basebandBootloaderVersion;
			}
		}

		public string BasebandSerialNumber
		{
			get
			{
				if (basebandSerialNumber.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BasebandSerialNumber);
					if (deviceValue != null)
					{
						basebandSerialNumber = Convert.ToBase64String((deviceValue as byte[]) ?? new byte[0]);
					}
				}
				return basebandSerialNumber;
			}
		}

		public string BasebandVersion
		{
			get
			{
				if (basebandVersion.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BasebandVersion);
					if (deviceValue != null)
					{
						basebandVersion = deviceValue.ToString();
					}
				}
				return basebandVersion;
			}
		}

		public string BluetoothAddress
		{
			get
			{
				if (bluetoothAddress.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BluetoothAddress);
					if (deviceValue != null)
					{
						bluetoothAddress = deviceValue.ToString();
					}
				}
				return bluetoothAddress;
			}
		}

		public string BuildVersion
		{
			get
			{
				if (buildVersion.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.BuildVersion);
					if (deviceValue != null)
					{
						buildVersion = deviceValue.ToString();
					}
				}
				return buildVersion;
			}
		}

		public string CPUArchitecture
		{
			get
			{
				if (cpuArchitecture.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.CPUArchitecture);
					if (deviceValue != null)
					{
						cpuArchitecture = deviceValue.ToString();
					}
				}
				return cpuArchitecture;
			}
		}

		public string DeviceColor
		{
			get
			{
				if (string.IsNullOrWhiteSpace(deviceColor))
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.DeviceColor);
					if (deviceValue != null)
					{
						deviceColor = deviceValue.ToString();
					}
				}
				return deviceColor;
			}
		}

		public string DeviceName
		{
			get
			{
				if (deviceName.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.DeviceName);
					if (deviceValue != null)
					{
						deviceName = deviceValue.ToString();
					}
				}
				return deviceName;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				if (firmwareVersion.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.FirmwareVersion);
					if (deviceValue != null)
					{
						firmwareVersion = deviceValue.ToString();
					}
				}
				return firmwareVersion;
			}
		}

		public string HardwareModel
		{
			get
			{
				if (hardwareModel.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.HardwareModel);
					if (deviceValue != null)
					{
						hardwareModel = deviceValue.ToString();
					}
				}
				return hardwareModel;
			}
		}

		public string ModelNumber
		{
			get
			{
				if (modelNumber.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.ModelNumber);
					if (deviceValue != null)
					{
						modelNumber = deviceValue.ToString();
					}
				}
				return modelNumber;
			}
		}

		public string PhoneNumber
		{
			get
			{
				if (phoneNumber.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.PhoneNumber);
					if (deviceValue != null)
					{
						phoneNumber = deviceValue.ToString();
					}
				}
				return phoneNumber;
			}
		}

		public string ProductType
		{
			get
			{
				if (productType.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.ProductType);
					if (deviceValue != null)
					{
						productType = deviceValue.ToString();
					}
				}
				return productType;
			}
		}

		public string IntegratedCircuitCardIdentity
		{
			get
			{
				if (integratedCircuitCardIdentity.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.IntegratedCircuitCardIdentity);
					if (deviceValue != null)
					{
						integratedCircuitCardIdentity = deviceValue.ToString();
					}
				}
				return integratedCircuitCardIdentity;
			}
		}

		public string InternationalMobileEquipmentIdentity
		{
			get
			{
				if (internationalMobileEquipmentIdentity.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.InternationalMobileEquipmentIdentity);
					if (deviceValue != null)
					{
						internationalMobileEquipmentIdentity = deviceValue.ToString();
					}
				}
				return internationalMobileEquipmentIdentity;
			}
		}

		public string SerialNumber
		{
			get
			{
				if (serialNumber.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.SerialNumber);
					if (deviceValue != null)
					{
						serialNumber = deviceValue.ToString();
					}
				}
				return serialNumber;
			}
		}

		public string SIMStatus
		{
			get
			{
				if (simStatus.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.SIMStatus);
					if (deviceValue != null)
					{
						simStatus = deviceValue.ToString();
					}
				}
				return simStatus;
			}
		}

		public string UniqueDeviceID
		{
			get
			{
				if (uniqueDeviceID.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.UniqueDeviceID);
					if (deviceValue != null)
					{
						uniqueDeviceID = deviceValue.ToString();
					}
				}
				return uniqueDeviceID;
			}
		}

		public string UniqueChipID
		{
			get
			{
				if (uniqueChipID.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.UniqueChipID);
					if (deviceValue != null)
					{
						uniqueChipID = deviceValue.ToString();
					}
				}
				return uniqueChipID;
			}
		}

		public string RegionInfo
		{
			get
			{
				if (regionInfo.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.RegionInfo);
					if (deviceValue != null)
					{
						regionInfo = deviceValue.ToString();
					}
				}
				return regionInfo;
			}
		}

		public string WiFiAddress
		{
			get
			{
				if (wiFiAddress.Length == 0)
				{
					object deviceValue = GetDeviceValue(DeviceInfoKey.WiFiAddress);
					if (deviceValue != null)
					{
						wiFiAddress = deviceValue.ToString();
					}
				}
				return wiFiAddress;
			}
		}

		public string ProductVersion
		{
			get
			{
				if (string.IsNullOrEmpty(productVersion))
				{
					productVersion = Convert.ToString(GetDeviceValue(DeviceInfoKey.ProductVersion)) + string.Empty;
				}
				return productVersion;
			}
		}

		public int VersionNumber
		{
			get
			{
				if (versionNumber == 0)
				{
					versionNumber = Convert.ToInt32(ProductVersion.Replace(".", string.Empty).PadRight(3, '0'));
				}
				return versionNumber;
			}
		}

		public string Identifier
		{
			get
			{
				string result = string.Empty;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = MobileDevice.AMDeviceCopyDeviceIdentifier(DevicePtr);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						result = Convert.ToString(CoreFoundation.ManagedTypeFromCFType(ref intPtr));
						CoreFoundation.CFRelease(intPtr);
					}
				}
				return result;
			}
		}

		public iOSDevice(IntPtr devicePtr)
		{
			DevicePtr = devicePtr;
			ConnectAndInitService();
		}

		public kAMDError Connect()
		{
			kAMDError kAMDError = kAMDError.kAMDSuccess;
			try
			{
				if (!isConnected)
				{
					kAMDError = (kAMDError)MobileDevice.AMDeviceConnect(DevicePtr);
					if (kAMDError == kAMDError.kAMDSuccess)
					{
						isConnected = true;
						return kAMDError;
					}
					return kAMDError;
				}
				return kAMDError;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return kAMDError;
			}
		}

		public kAMDError Disconnect()
		{
			kAMDError result = kAMDError.kAMDSuccess;
			isConnected = false;
			try
			{
				result = (kAMDError)MobileDevice.AMDeviceDisconnect(DevicePtr);
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return result;
			}
		}

		private bool ConnectAndInitService()
		{
			bool result = false;
			try
			{
				if (Connect() > kAMDError.kAMDSuccess)
				{
					return false;
				}
				if (MobileDevice.AMDeviceValidatePairing(DevicePtr) != 0)
				{
					kAMDError kAMDError = (kAMDError)MobileDevice.AMDevicePair(DevicePtr);
					if (kAMDError > kAMDError.kAMDSuccess)
					{
						Disconnect();
						return false;
					}
				}
				isSessionOpen = false;
				if (StartSession() == kAMDError.kAMDSuccess)
				{
					isConnected = true;
					StopSession();
					result = true;
					return result;
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public kAMDError StartSession(bool isRretry = false)
		{
			kAMDError kAMDError = kAMDError.kAMDSuccess;
			try
			{
				if (!isSessionOpen)
				{
					kAMDError = (kAMDError)MobileDevice.AMDeviceStartSession(DevicePtr);
					if (kAMDError != kAMDError.kAMDInvalidHostIDError)
					{
						if (kAMDError <= kAMDError.kAMDSuccess)
						{
							isSessionOpen = true;
							return kAMDError;
						}
						if (!isRretry)
						{
							Disconnect();
							Connect();
							return StartSession(isRretry: true);
						}
						return kAMDError;
					}
					if (MobileDevice.AMDeviceUnpair(DevicePtr) == 0 && MobileDevice.AMDevicePair(DevicePtr) == 0)
					{
						kAMDError = (kAMDError)MobileDevice.AMDeviceStartSession(DevicePtr);
						if (kAMDError > kAMDError.kAMDSuccess)
						{
							return kAMDError;
						}
						isSessionOpen = true;
						return kAMDError;
					}
				}
				return kAMDError;
			}
			catch
			{
				return kAMDError.kAMDUndefinedError;
			}
		}

		private kAMDError StopSession()
		{
			isSessionOpen = false;
			try
			{
				return (kAMDError)MobileDevice.AMDeviceStopSession(DevicePtr);
			}
			catch
			{
				return kAMDError.kAMDUndefinedError;
			}
		}

		private bool StartSocketService(string serviceName, ref int serviceSocket)
		{
			if (serviceSocket > 0)
			{
				return true;
			}
			if (!isConnected && Connect() > kAMDError.kAMDSuccess)
			{
				Console.WriteLine("StartService()执行Connect()失败");
				return false;
			}
			bool flag4 = false;
			if (!isSessionOpen)
			{
				if (StartSession() != 0)
				{
					return false;
				}
				flag4 = true;
			}
			bool flag5 = false;
			IntPtr zero = IntPtr.Zero;
			if (MobileDevice.AMDeviceSecureStartService(DevicePtr, CoreFoundation.StringToCFString(serviceName), IntPtr.Zero, ref zero) == 0)
			{
				serviceSocket = MobileDevice.AMDServiceConnectionGetSocket(zero);
				if (serviceSocket > 0)
				{
					SocketContext = zero;
					DeviceSecureIOContext = MobileDevice.AMDServiceConnectionGetSecureIOContext(zero);
				}
				flag5 = true;
			}
			else if (MobileDevice.AMDeviceStartService(DevicePtr, CoreFoundation.StringToCFString(serviceName), ref serviceSocket, IntPtr.Zero) == 0)
			{
				flag5 = true;
			}
			if (flag4)
			{
				StopSession();
			}
			return flag5;
		}

		private bool StopSocketService(ref int socket)
		{
			kAMDError kAMDError = kAMDError.kAMDSuccess;
			if (socket > 0)
			{
				try
				{
					kAMDError = (kAMDError)MobileDevice.closesocket(socket);
				}
				catch (Exception)
				{
					return false;
				}
			}
			socket = 0;
			return kAMDError > kAMDError.kAMDSuccess;
		}

		public bool SendMessageToSocket(int sock, IntPtr message)
		{
			if (sock < 1 || message == IntPtr.Zero)
			{
				return false;
			}
			bool flag2 = false;
			IntPtr intPtr = CoreFoundation.CFWriteStreamCreateWithAllocatedBuffers(IntPtr.Zero, IntPtr.Zero);
			if (intPtr != IntPtr.Zero)
			{
				if (!CoreFoundation.CFWriteStreamOpen(intPtr))
				{
					return false;
				}
				IntPtr zero = IntPtr.Zero;
				if (CoreFoundation.CFPropertyListWriteToStream(message, intPtr, CFPropertyListFormat.kCFPropertyListBinaryFormat_v1_0, ref zero) > 0)
				{
					IntPtr intPtr2 = CoreFoundation.CFWriteStreamCopyProperty(intPtr, CoreFoundation.kCFStreamPropertyDataWritten);
					IntPtr buffer = CoreFoundation.CFDataGetBytePtr(intPtr2);
					int num = CoreFoundation.CFDataGetLength(intPtr2);
					uint structure = MobileDevice.htonl((uint)num);
					int num2 = Marshal.SizeOf(structure);
					if (DeviceSecureIOContext == IntPtr.Zero || SocketContext == IntPtr.Zero)
					{
						if (MobileDevice.send_UInt32(sock, ref structure, num2, 0) != num2)
						{
							Console.WriteLine("could not send message size");
						}
						else if (MobileDevice.send(sock, buffer, num, 0) != num)
						{
							Console.WriteLine("Could not send message.");
						}
						else
						{
							flag2 = true;
						}
					}
					else if (MobileDevice.AMDServiceConnectionSend_UInt32(SocketContext, ref structure, num2) != num2)
					{
						Console.WriteLine("could not send message size with socket");
					}
					else if (MobileDevice.AMDServiceConnectionSend(SocketContext, buffer, num) != num)
					{
						Console.WriteLine("could not send message size");
					}
					else
					{
						flag2 = true;
					}
					CoreFoundation.CFRelease(intPtr2);
				}
				CoreFoundation.CFWriteStreamClose(intPtr);
			}
			return flag2;
		}

		public bool SendMessageToSocket(int sock, object dict)
		{
			IntPtr message = CoreFoundation.CFTypeFromManagedType(dict);
			return SendMessageToSocket(sock, message);
		}

		public object ReceiveMessageFromSocket(int sock)
		{
			if (sock < 0)
			{
				return null;
			}
			uint netlong = 0u;
			uint num = 0u;
			uint num2 = 0u;
			IntPtr intPtr = IntPtr.Zero;
			if (DeviceSecureIOContext != IntPtr.Zero && SocketContext != IntPtr.Zero)
			{
				if (MobileDevice.AMDServiceConnectionReceive(SocketContext, ref netlong, 4) == 4)
				{
					num2 = MobileDevice.ntohl(netlong);
					if (num2 == 0)
					{
						Console.WriteLine("receive size error, dataSize:" + num2);
						return null;
					}
					intPtr = Marshal.AllocCoTaskMem((int)num2);
					if (intPtr == IntPtr.Zero)
					{
						Console.WriteLine("Could not allocate message buffer.");
						return null;
					}
					IntPtr intptr_ = intPtr;
					int num4;
					for (; num < num2; num += (uint)num4)
					{
						num4 = MobileDevice.AMDServiceConnectionReceive_1(SocketContext, intptr_, (int)(num2 - num));
						if (num4 <= -1)
						{
							Console.WriteLine("Could not receive secure message: " + num4);
							num = num2 + 1;
						}
						else if (num4 == 0)
						{
							Console.WriteLine("receive size is zero. ");
							break;
						}
						intptr_ = new IntPtr(intptr_.ToInt64() + num4);
					}
				}
			}
			else if (MobileDevice.recv_UInt(sock, ref netlong, 4, 0) == 4)
			{
				num2 = MobileDevice.ntohl(netlong);
				if (num2 == 0)
				{
					Console.WriteLine("receive size error, dataSize:" + num2);
					return null;
				}
				intPtr = Marshal.AllocCoTaskMem((int)num2);
				if (intPtr == IntPtr.Zero)
				{
					Console.WriteLine("Could not allocate message buffer.");
					return null;
				}
				IntPtr buffer = intPtr;
				int num3;
				for (; num < num2; num += (uint)num3)
				{
					num3 = MobileDevice.recv(sock, buffer, (int)(num2 - num), 0);
					if (num3 <= -1)
					{
						Console.WriteLine("Could not receive secure message: " + num3);
						num = num2 + 1;
					}
					else if (num3 == 0)
					{
						Console.WriteLine("receive size is zero. ");
						break;
					}
					buffer = new IntPtr(buffer.ToInt64() + num3);
				}
			}
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			if (num == num2)
			{
				intPtr2 = CoreFoundation.CFDataCreate(CoreFoundation.kCFAllocatorDefault, intPtr, (int)num2);
				if (intPtr2 == IntPtr.Zero)
				{
					Console.WriteLine("Could not create CFData for message");
				}
				else
				{
					IntPtr zero = IntPtr.Zero;
					intPtr3 = CoreFoundation.CFPropertyListCreateFromXMLData(CoreFoundation.kCFAllocatorDefault, intPtr2, CFPropertyListMutabilityOptions.kCFPropertyListImmutable, ref zero);
					if (intPtr3 == IntPtr.Zero)
					{
						Console.WriteLine("Could not convert raw xml into a dictionary: " + Convert.ToString(CoreFoundation.ManagedTypeFromCFType(ref zero)));
						return null;
					}
				}
			}
			if (intPtr2 != IntPtr.Zero)
			{
				try
				{
					CoreFoundation.CFRelease(intPtr2);
				}
				catch
				{
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			object obj = CoreFoundation.ManagedTypeFromCFType(ref intPtr3);
			if (intPtr3 != IntPtr.Zero)
			{
				CoreFoundation.CFRelease(intPtr3);
			}
			return obj;
		}

		public bool OpenConnection(int inSocket, out IntPtr connPtr)
		{
			connPtr = IntPtr.Zero;
			try
			{
				if (MobileDevice.AFCConnectionOpen(new IntPtr(inSocket), 0u, ref connPtr) == 0)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		public void CreateUSBMuxConnect(string localServeIp = "127.0.0.1", int localPort = 2222, uint remotePort = 22u)
		{
			Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress address = IPAddress.Parse(localServeIp);
			IPEndPoint localEP = new IPEndPoint(address, localPort);
			socket2.Bind(localEP);
			socket2.Listen(0);
			int socket = 0;
			Socket cSocket = socket2.Accept();
			int num = MobileDevice.USBMuxConnectByPort(MobileDevice.AMDeviceGetConnectionID(DevicePtr), MobileDevice.htons(remotePort), ref socket);
			if (num != 0)
			{
				throw new Exception($"USBMuxConnectByPort Error.Code:{num}");
			}
			IntPtr zero = IntPtr.Zero;
			if (OpenConnection(socket, out zero) && cSocket.Connected)
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					ConnForwardingThread(cSocket.Handle.ToInt32(), socket);
				});
				thread.Start();
				Thread thread2 = new Thread((ThreadStart)delegate
				{
					ConnForwardingThread(socket, cSocket.Handle.ToInt32());
				});
				thread2.Start();
			}
		}

		private void ConnForwardingThread(int from, int to)
		{
			byte[] buffer = new byte[256];
			while (true)
			{
				int num = MobileDevice.recv(from, buffer, 256, 0);
				if (num == -1)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 10035)
					{
						MobileDevice.closesocket(from);
						MobileDevice.closesocket(to);
						return;
					}
				}
				else
				{
					int num2 = MobileDevice.send(to, buffer, num, 0);
					if (num == 0 || num == -1 || num2 == 0 || num2 == -1)
					{
						break;
					}
				}
			}
			MobileDevice.closesocket(from);
			MobileDevice.closesocket(to);
		}

		public object GetDeviceValue(DeviceInfoKey key)
		{
			return GetDeviceValue(null, key);
		}

		public object GetDeviceValue(string domain, DeviceInfoKey key)
		{
			return GetDeviceValue(domain, key.ToString());
		}

		public object GetDeviceValue(string domain, string key)
		{
			object result = null;
			try
			{
				bool flag = false;
				bool flag2 = false;
				if (!isConnected)
				{
					if (Connect() > kAMDError.kAMDSuccess)
					{
						return null;
					}
					flag = true;
				}
				if (!isSessionOpen)
				{
					if (StartSession() == kAMDError.kAMDSuccess)
					{
						flag2 = true;
					}
					else if (flag)
					{
						Disconnect();
					}
				}
				result = MobileDevice.AMDeviceCopyValue(DevicePtr, domain, key);
				if (flag2)
				{
					StopSession();
				}
				if (flag)
				{
					Disconnect();
					return result;
				}
				return result;
			}
			catch
			{
				return result;
			}
		}

		public int GetBatteryCurrentCapacity()
		{
			try
			{
				string text = Convert.ToString(GetDeviceValue("com.apple.mobile.battery", DeviceInfoKey.BatteryCurrentCapacity)) + string.Empty;
				if (text.Length > 0)
				{
					return SafeConvert.ToInt32(text);
				}
			}
			catch (Exception)
			{
			}
			return -1;
		}

		public bool GetBatteryIsCharging()
		{
			try
			{
				object deviceValue = GetDeviceValue("com.apple.mobile.battery", DeviceInfoKey.BatteryIsCharging);
				if (deviceValue != null && deviceValue is bool)
				{
					return Convert.ToBoolean(deviceValue);
				}
			}
			catch
			{
			}
			return false;
		}

		public Dictionary<object, object> GetBatteryInfoFormDiagnostics()
		{
			try
			{
				object batteryInfo = GetBatteryInfo();
				if (batteryInfo == null)
				{
					return null;
				}
				Dictionary<object, object> dictionary = batteryInfo as Dictionary<object, object>;
				if (dictionary["Status"].ToString() != "Success")
				{
					return null;
				}
				if (!(dictionary["Diagnostics"] is Dictionary<object, object> dictionary2))
				{
					return null;
				}
				return dictionary2["IORegistry"] as Dictionary<object, object>;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public object GetBatteryInfo()
		{
			int sock = 0;
			if (!StartSocketService("com.apple.mobile.diagnostics_relay", ref sock))
			{
				return null;
			}
			Dictionary<object, object> dict = new Dictionary<object, object>
			{
				{ "Request", "IORegistry" },
				{ "EntryClass", "IOPMPowerSource" }
			};
			if (SendMessageToSocket(sock, dict))
			{
				return ReceiveMessageFromSocket(sock);
			}
			return null;
		}
	}
}
