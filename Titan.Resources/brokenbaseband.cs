using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Titan.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class brokenbaseband
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager temp = (resourceMan = new ResourceManager("Titan.Resources.brokenbaseband", typeof(brokenbaseband).Assembly));
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static byte[] i12reset
		{
			get
			{
				object obj = ResourceManager.GetObject("i12reset", resourceCulture);
				return (byte[])obj;
			}
		}

		internal static byte[] i12update
		{
			get
			{
				object obj = ResourceManager.GetObject("i12update", resourceCulture);
				return (byte[])obj;
			}
		}

		internal static byte[] i13reset
		{
			get
			{
				object obj = ResourceManager.GetObject("i13reset", resourceCulture);
				return (byte[])obj;
			}
		}

		internal static byte[] i13update
		{
			get
			{
				object obj = ResourceManager.GetObject("i13update", resourceCulture);
				return (byte[])obj;
			}
		}

		internal static byte[] purple
		{
			get
			{
				object obj = ResourceManager.GetObject("purple", resourceCulture);
				return (byte[])obj;
			}
		}

		internal static byte[] SystemStatusServer
		{
			get
			{
				object obj = ResourceManager.GetObject("SystemStatusServer", resourceCulture);
				return (byte[])obj;
			}
		}

		internal brokenbaseband()
		{
		}
	}
}
