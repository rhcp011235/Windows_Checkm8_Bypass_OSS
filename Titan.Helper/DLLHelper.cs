using System;
using System.IO;
using Microsoft.Win32;

namespace Titan.Helper
{
	public class DLLHelper
	{
		public static string GetiTunesMobileDeviceDllPath()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Apple Inc.\\Apple Mobile Device Support\\Shared");
			if (registryKey != null)
			{
				string text = registryKey.GetValue("MobileDeviceDLL") as string;
				if (!string.IsNullOrWhiteSpace(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Exists)
					{
						return fileInfo.DirectoryName;
					}
				}
			}
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\Apple\\Mobile Device Support";
			if (File.Exists(text2 + "\\MobileDevice.dll"))
			{
				return text2;
			}
			text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\Apple\\Mobile Device Support";
			if (File.Exists(text2 + "\\MobileDevice.dll"))
			{
				return text2;
			}
			return string.Empty;
		}

		public static string GetAppleApplicationSupportFolder()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Apple Inc.\\Apple Mobile Device Support");
			if (registryKey != null)
			{
				string text = registryKey.GetValue("InstallDir") as string;
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\Apple\\Mobile Device Support";
			if (File.Exists(text2 + "\\CoreFoundation.dll"))
			{
				return text2;
			}
			text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\Apple\\Mobile Device Support";
			if (File.Exists(text2 + "\\CoreFoundation.dll"))
			{
				return text2;
			}
			return string.Empty;
		}
	}
}
