using System;

namespace Titan.CoreFundation
{
	public struct PROCESS_INFORMATION
	{
		public IntPtr hProcess;

		public IntPtr hThread;

		public int dwProcessId;

		public int dwThreadId;

		public bool IsEmpty
		{
			get
			{
				if (hProcess == IntPtr.Zero && hThread == IntPtr.Zero && dwProcessId == 0)
				{
					return dwThreadId == 0;
				}
				return false;
			}
		}
	}
}
