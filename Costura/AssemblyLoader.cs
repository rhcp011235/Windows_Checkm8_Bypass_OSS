using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura
{
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		private static object nullCacheLock = new object();

		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		private static int isAttached;

		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return string.Empty;
			}
			return culture.Name;
		}

		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		private static string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
		{
			string name = requestedAssemblyName.Name!.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo!.Name))
			{
				name = (CultureToString(requestedAssemblyName.CultureInfo) + "." + name).ToLowerInvariant();
			}
			return name;
		}

		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream stream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using DeflateStream source = new DeflateStream(stream, CompressionMode.Decompress);
					MemoryStream memoryStream = new MemoryStream();
					CopyTo(source, memoryStream);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			if (resourceNames.TryGetValue(name, out var value))
			{
				return LoadStream(value);
			}
			return null;
		}

		private static byte[] ReadStream(Stream stream)
		{
			byte[] data = new byte[stream.Length];
			stream.Read(data, 0, data.Length);
			return data;
		}

		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string name = GetAssemblyResourceName(requestedAssemblyName);
			byte[] assemblyData;
			using (Stream stream = LoadStream(assemblyNames, name))
			{
				if (stream == null)
				{
					return null;
				}
				assemblyData = ReadStream(stream);
			}
			using (Stream stream2 = LoadStream(symbolNames, name))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = ReadStream(stream2);
					return Assembly.Load(assemblyData, rawSymbolStore);
				}
			}
			return Assembly.Load(assemblyData);
		}

		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			string assemblyNameAsString = e.Name;
			AssemblyName assemblyName = new AssemblyName(assemblyNameAsString);
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(assemblyNameAsString))
				{
					return null;
				}
			}
			Assembly assembly = ReadExistingAssembly(assemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, assemblyName);
			if ((object)assembly == null)
			{
				lock (nullCacheLock)
				{
					nullCache[assemblyNameAsString] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		static AssemblyLoader()
		{
			assemblyNames.Add("bunifu.licensing", "costura.bunifu.licensing.dll.compressed");
			assemblyNames.Add("bunifu.ui.winforms", "costura.bunifu.ui.winforms.dll.compressed");
			assemblyNames.Add("costura", "costura.costura.dll.compressed");
			symbolNames.Add("costura", "costura.costura.pdb.compressed");
			assemblyNames.Add("downloader", "costura.downloader.dll.compressed");
			assemblyNames.Add("endianbitconverter", "costura.endianbitconverter.dll.compressed");
			assemblyNames.Add("guna.ui2", "costura.guna.ui2.dll.compressed");
			assemblyNames.Add("ionic.zip", "costura.ionic.zip.dll.compressed");
			assemblyNames.Add("jsonfx", "costura.jsonfx.dll.compressed");
			assemblyNames.Add("libusbdotnet.libusbdotnet", "costura.libusbdotnet.libusbdotnet.dll.compressed");
			symbolNames.Add("libusbdotnet.libusbdotnet", "costura.libusbdotnet.libusbdotnet.pdb.compressed");
			assemblyNames.Add("metroframework.design", "costura.metroframework.design.dll.compressed");
			assemblyNames.Add("metroframework", "costura.metroframework.dll.compressed");
			assemblyNames.Add("metroframework.fonts", "costura.metroframework.fonts.dll.compressed");
			assemblyNames.Add("microsoft.bcl.asyncinterfaces", "costura.microsoft.bcl.asyncinterfaces.dll.compressed");
			assemblyNames.Add("microsoft.toolkit.uwp.notifications", "costura.microsoft.toolkit.uwp.notifications.dll.compressed");
			symbolNames.Add("microsoft.toolkit.uwp.notifications", "costura.microsoft.toolkit.uwp.notifications.pdb.compressed");
			assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			assemblyNames.Add("plist-cil", "costura.plist-cil.dll.compressed");
			symbolNames.Add("plist-cil", "costura.plist-cil.pdb.compressed");
			assemblyNames.Add("plistnet", "costura.plistnet.dll.compressed");
			assemblyNames.Add("siticone.desktop.ui", "costura.siticone.desktop.ui.dll.compressed");
			assemblyNames.Add("system.buffers", "costura.system.buffers.dll.compressed");
			assemblyNames.Add("system.codedom", "costura.system.codedom.dll.compressed");
			assemblyNames.Add("system.diagnostics.diagnosticsource", "costura.system.diagnostics.diagnosticsource.dll.compressed");
			assemblyNames.Add("system.memory", "costura.system.memory.dll.compressed");
			assemblyNames.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
			assemblyNames.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
			assemblyNames.Add("system.threading.tasks.extensions", "costura.system.threading.tasks.extensions.dll.compressed");
			assemblyNames.Add("system.valuetuple", "costura.system.valuetuple.dll.compressed");
			assemblyNames.Add("tinyhome.renci.sshnet", "costura.tinyhome.renci.sshnet.dll.compressed");
		}

		public static void Attach(bool subscribe)
		{
			if (Interlocked.Exchange(ref isAttached, 1) == 1 || !subscribe)
			{
				return;
			}
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				string ResolveAssemblyassemblyNameAsString = e.Name;
				AssemblyName ResolveAssemblyassemblyName = new AssemblyName(ResolveAssemblyassemblyNameAsString);
				lock (nullCacheLock)
				{
					if (nullCache.ContainsKey(ResolveAssemblyassemblyNameAsString))
					{
						return null;
					}
				}
				Assembly ResolveAssemblyassembly = ReadExistingAssembly(ResolveAssemblyassemblyName);
				if ((object)ResolveAssemblyassembly != null)
				{
					return ResolveAssemblyassembly;
				}
				ResolveAssemblyassembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, ResolveAssemblyassemblyName);
				if ((object)ResolveAssemblyassembly == null)
				{
					lock (nullCacheLock)
					{
						nullCache[ResolveAssemblyassemblyNameAsString] = true;
					}
					if ((ResolveAssemblyassemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
					{
						ResolveAssemblyassembly = Assembly.Load(ResolveAssemblyassemblyName);
					}
				}
				return ResolveAssemblyassembly;
			};
		}
	}
}
