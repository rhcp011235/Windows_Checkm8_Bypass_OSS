using System;
using System.IO;
using System.Runtime.InteropServices;
using Titan.Helper;

namespace Titan.CoreFundation
{
	public class CFBoolean
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		public static IntPtr GetCFBoolean(bool flag)
		{
			string lpProcName = (flag ? "kCFBooleanTrue" : "kCFBooleanFalse");
			IntPtr intPtr = GetModuleHandle("CoreFoundation.dll");
			if (intPtr == IntPtr.Zero)
			{
				string appleApplicationSupportFolder = DLLHelper.GetAppleApplicationSupportFolder();
				if (!string.IsNullOrWhiteSpace(appleApplicationSupportFolder))
				{
					intPtr = LoadLibrary(Path.Combine(appleApplicationSupportFolder, "CoreFoundation.dll"));
				}
			}
			IntPtr ptr = IntPtr.Zero;
			if (intPtr != IntPtr.Zero)
			{
				ptr = GetProcAddress(intPtr, lpProcName);
			}
			return Marshal.ReadIntPtr(ptr, 0);
		}
	}
}
