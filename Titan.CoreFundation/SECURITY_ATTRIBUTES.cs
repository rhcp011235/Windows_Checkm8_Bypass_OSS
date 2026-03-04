using System.Runtime.InteropServices;

namespace Titan.CoreFundation
{
	[StructLayout(LayoutKind.Sequential)]
	public class SECURITY_ATTRIBUTES
	{
		public int nLength;

		public string lpSecurityDescriptor;

		public bool bInheritHandle;
	}
}
