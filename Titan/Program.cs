using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Titan
{
	internal static class Program
	{
		private static Mutex singleton = new Mutex(initiallyOwned: true, "Azwyn");

		[STAThread]
		private static void Main()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (!singleton.WaitOne(TimeSpan.Zero, exitContext: true))
			{
				MessageBox.Show("This Software is Already running", "[ERROR]", (MessageBoxButtons)0, (MessageBoxIcon)16);
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				Application.Run((Form)(object)new Form1());
			}
		}
	}
}
