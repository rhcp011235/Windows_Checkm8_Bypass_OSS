using System;

namespace Titan
{
	public static class iOSDevice2
	{
		public static bool debugMode = false;

		public static string ActivationState = "Unactivated";

		public static string SIMStatus = "";

		public static string MyType = "";

		public static string progress = "";

		public static bool isSIMInserted = false;

		public static string IMEI = "";

		public static string BuildVersion = "";

		public static string DeviceName = "";

		public static string MEID = "";

		public static string SerialNumber = "";

		public static string UDID = "";

		public static string IOSVersion = "13.";

		public static string ProductType = "";

		public static string register = "";

		public static string Model = "iDevice Information";

		public static string ModelServer = "";

		public static string stat = "";

		public static string stat2 = "";

		public static string skip = "";

		public static string wall = "";

		public static string ota = "";

		public static bool isMEID = false;

		public static bool isWarranty = false;

		public static void setProductType(string AproductType)
		{
			ProductType = AproductType;
			try
			{
				Model = DetermineModel(AproductType);
			}
			catch (Exception)
			{
			}
		}

		public static bool clasifyGSMorMEID()
		{
			if (Model.Contains("GSM"))
			{
				return false;
			}
			if (Model.Contains("iPod") || Model.Contains("iPad"))
			{
				return true;
			}
			if (Model.Contains("CDMA") || Model.Contains("iPhone 6") || Model.Contains("iPhone SE") || Model.Contains("Global") || Model.Contains("iPod") || Model.Contains("iPad"))
			{
				return true;
			}
			if (MEID == "")
			{
				return false;
			}
			return true;
		}

		internal static uint CalculateModelNumber(string productType)
		{
			uint num = 0u;
			if (productType != null)
			{
				num = 2166136261u;
				for (int i = 0; i < productType.Length; i++)
				{
					num = (productType[i] ^ num) * 16777619;
				}
			}
			return num;
		}

		public static string DetermineModel(string productType)
		{
			string result = "N/A";
			switch (CalculateModelNumber(productType))
			{
			case 194068216u:
				if (productType == "iPhone5,1")
				{
					result = "iPhone 5 (AT&T/Canada)";
				}
				break;
			case 80824001u:
				if (productType == "iPad6,8")
				{
					result = "iPad PRO 12.9 Wifi + Cellular";
				}
				break;
			case 13713525u:
				if (productType == "iPad6,4")
				{
					result = "iPad PRO 9.7 Wifi + Cellular";
				}
				break;
			case 227623454u:
				if (productType == "iPhone5,3")
				{
					result = "iPhone 5c";
				}
				break;
			case 218008307u:
				if (productType == "iPad7,3")
				{
					result = "iPad PRO 10.5 Wifi";
				}
				break;
			case 201230688u:
				if (productType == "iPad7,2")
				{
					result = "iPad PRO 12.9 Wifi + Cellular";
				}
				break;
			case 244401073u:
				if (productType == "iPhone5,2")
				{
					result = "iPhone 5";
				}
				break;
			case 240921132u:
				if (productType == "iPad6,12")
				{
					result = "iPad (5th) Wifi + Cellular";
				}
				break;
			case 235638739u:
				if (productType == "iPhone4,1")
				{
					result = "iPhone 4S";
				}
				break;
			case 277956311u:
				if (productType == "iPhone5,4")
				{
					result = "iPhone 5c";
				}
				break;
			case 268341164u:
				if (productType == "iPad7,6")
				{
					result = "iPad (6th) WiFi + Cellular";
				}
				break;
			case 251563545u:
				if (productType == "iPad7,1")
				{
					result = "iPad PRO 12.9 Wifi";
				}
				break;
			case 318674021u:
				if (productType == "iPad7,5")
				{
					result = "iPad (6th) WiFi";
				}
				break;
			case 301896402u:
				if (productType == "iPad7,4")
				{
					result = "iPad PRO 10.5 Wifi + Cellular";
				}
				break;
			case 291253989u:
				if (productType == "iPad6,11")
				{
					result = "iPad (5th) Wifi";
				}
				break;
			case 383736520u:
				if (productType == "iPhone11,6")
				{
					result = "iPhone XS Max China";
				}
				break;
			case 350181282u:
				if (productType == "iPhone11,8")
				{
					result = "iPhone XR";
				}
				break;
			case 342808911u:
				if (productType == "iPad8,8")
				{
					result = "iPad PRO 12.9 1TB, WiFi + Cellular";
				}
				break;
			case 417291758u:
				if (productType == "iPhone11,4")
				{
					result = "iPhone XS Max";
				}
				break;
			case 409919387u:
				if (productType == "iPad8,4")
				{
					result = "iPad PRO 11 1TB, WiFi + Cellular";
				}
				break;
			case 393141768u:
				if (productType == "iPad8,5")
				{
					result = "iPad PRO 12.9 WiFi";
				}
				break;
			case 450846996u:
				if (productType == "iPhone11,2")
				{
					result = "iPhone XS";
				}
				break;
			case 443474625u:
				if (productType == "iPad8,6")
				{
					result = "iPad PRO 12.9 1TB, WiFi";
				}
				break;
			case 426697006u:
				if (productType == "iPad8,7")
				{
					result = "iPad PRO 12.9 WiFi + Cellular";
				}
				break;
			case 510585101u:
				if (productType == "iPad8,2")
				{
					result = "iPad PRO 11 1TB, WiFi";
				}
				break;
			case 493807482u:
				if (productType == "iPad8,3")
				{
					result = "iPad PRO 11 WiFi + Cellular";
				}
				break;
			case 460252244u:
				if (productType == "iPad8,1")
				{
					result = "iPad PRO 11 WiFi";
				}
				break;
			case 755807492u:
				if (productType == "iPhone12,1")
				{
					result = "iPhone 11";
				}
				break;
			case 688697016u:
				if (productType == "iPhone12,5")
				{
					result = "iPhone 11 Pro Max";
				}
				break;
			case 519927770u:
				if (productType == "iPod4,1")
				{
					result = "iPod Touch Fourth Generation";
				}
				break;
			case 897947417u:
				if (productType == "iPod9,1")
				{
					result = "iPod Touch 7th Generation";
				}
				break;
			case 876599987u:
				if (productType == "iPhone9,4")
				{
					result = "iPhone 7 Plus (GSM)";
				}
				break;
			case 789362730u:
				if (productType == "iPhone12,3")
				{
					result = "iPhone 11 Pro";
				}
				break;
			case 977265701u:
				if (productType == "iPhone9,2")
				{
					result = "iPhone 7 Plus (CDMA)";
				}
				break;
			case 960488082u:
				if (productType == "iPhone9,3")
				{
					result = "iPhone 7 (GSM)";
				}
				break;
			case 926932844u:
				if (productType == "iPhone9,1")
				{
					result = "iPhone 7 (CDMA)";
				}
				break;
			case 1027150186u:
				if (productType == "iPhone3,1")
				{
					result = "iPhone 4 (GSM)";
				}
				break;
			case 1010372567u:
				if (productType == "iPhone3,2")
				{
					result = "iPhone 4 (GSM Rev A)";
				}
				break;
			case 993594948u:
				if (productType == "iPhone3,3")
				{
					result = "iPhone 4 (CDMA/Verizon/Sprint)";
				}
				break;
			case 1118200753u:
				if (productType == "iPad5,3")
				{
					result = "iPad AIR 2 Wifi";
				}
				break;
			case 1101423134u:
				if (productType == "iPad5,2")
				{
					result = "iPad Mini 4 Wifi + Cellular";
				}
				break;
			case 1084645515u:
				if (productType == "iPad5,1")
				{
					result = "iPad Mini 4 Wifi";
				}
				break;
			case 1538345056u:
				if (productType == "iPad3,6")
				{
					result = "iPad 4 Wifi + Cellular";
				}
				break;
			case 1158652399u:
				if (productType == "iPod7,1")
				{
					result = "iPod Touch 6th Generation";
				}
				break;
			case 1134978372u:
				if (productType == "iPad5,4")
				{
					result = "iPad AIR 2 Wifi + Cellular";
				}
				break;
			case 1605455532u:
				if (productType == "iPad3,2")
				{
					result = "iPad 3 Wifi + Cellular";
				}
				break;
			case 1588677913u:
				if (productType == "iPad3,5")
				{
					result = "iPad 4 Wifi + Cellular";
				}
				break;
			case 1571900294u:
				if (productType == "iPad3,4")
				{
					result = "iPad 4 Wifi";
				}
				break;
			case 1655788389u:
				if (productType == "iPad3,1")
				{
					result = "iPad 3 Wifi";
				}
				break;
			case 1622233151u:
				if (productType == "iPad3,3")
				{
					result = "iPad 3 Wifi + Cellular";
				}
				break;
			case 1613858532u:
				if (productType == "iPhone1,1")
				{
					result = "iPhone 1";
				}
				break;
			case 1760014814u:
				if (productType == "iPhone7,1")
				{
					result = "iPhone 6 Plus";
				}
				break;
			case 1743237195u:
				if (productType == "iPhone7,2")
				{
					result = "iPhone 6";
				}
				break;
			case 1664191389u:
				if (productType == "iPhone1,2")
				{
					result = "iPhone 3G";
				}
				break;
			case 2081752929u:
				if (productType == "iPhone6,1")
				{
					result = "iPhone 5s";
				}
				break;
			case 2031420072u:
				if (productType == "iPhone6,2")
				{
					result = "iPhone 5s (Global)";
				}
				break;
			case 1886294147u:
				if (productType == "iPod3,1")
				{
					result = "iPod Touch Third Generation";
				}
				break;
			case 2286986705u:
				if (productType == "iPhone10,4")
				{
					result = "iPhone 8 (GSM)";
				}
				break;
			case 2270209086u:
				if (productType == "iPhone10,5")
				{
					result = "iPhone 8 Plus (GSM)";
				}
				break;
			case 2253431467u:
				if (productType == "iPhone10,6")
				{
					result = "iPhone X (GSM)";
				}
				break;
			case 2337319562u:
				if (productType == "iPhone10,1")
				{
					result = "iPhone 8 (CDMA)";
				}
				break;
			case 2320541943u:
				if (productType == "iPhone10,2")
				{
					result = "iPhone 8 Plus (CDMA)";
				}
				break;
			case 2303764324u:
				if (productType == "iPhone10,3")
				{
					result = "iPhone X (CDMA)";
				}
				break;
			case 2643280656u:
				if (productType == "iPad4,1")
				{
					result = "iPad AIR Wifi";
				}
				break;
			case 2526436330u:
				if (productType == "iPad1,2")
				{
					result = "iPad 1 Wifi + Cellular";
				}
				break;
			case 2509658711u:
				if (productType == "iPad1,1")
				{
					result = "iPad 1 Wifi)";
				}
				break;
			case 2710391132u:
				if (productType == "iPad4,5")
				{
					result = "iPad Mini 2 Wifi + Cellular";
				}
				break;
			case 2693613513u:
				if (productType == "iPad4,2")
				{
					result = "iPad AIR Wifi + Cellular";
				}
				break;
			case 2676835894u:
				if (productType == "iPad4,3")
				{
					result = "iPad AIR Wifi + Cellular";
				}
				break;
			case 2760723989u:
				if (productType == "iPad4,6")
				{
					result = "iPad Mini 2 Wifi + Cellular";
				}
				break;
			case 2743946370u:
				if (productType == "iPad4,7")
				{
					result = "iPad Mini 3 Wifi";
				}
				break;
			case 2727168751u:
				if (productType == "iPad4,4")
				{
					result = "iPad Mini 2 Wifi";
				}
				break;
			case 2900469187u:
				if (productType == "iPad11,4")
				{
					result = "iPad Air 3rd Gen Wifi  + Cellular";
				}
				break;
			case 2794279227u:
				if (productType == "iPad4,8")
				{
					result = "iPad Mini 3 Wifi + Cellular";
				}
				break;
			case 2777501608u:
				if (productType == "iPad4,9")
				{
					result = "iPad Mini 3 Wifi + Cellular";
				}
				break;
			case 2989097949u:
				if (productType == "iPod5,1")
				{
					result = "iPod Touch 5th Generation";
				}
				break;
			case 2984357282u:
				if (productType == "iPad11,3")
				{
					result = "iPad Air 3rd Gen Wifi ";
				}
				break;
			case 2950802044u:
				if (productType == "iPad11,1")
				{
					result = "iPad mini 5th Gen WiFi";
				}
				break;
			case 3317288369u:
				if (productType == "iPad7,12")
				{
					result = "iPad (7th)WiFi + Cellular";
				}
				break;
			case 3266955512u:
				if (productType == "iPad7,11")
				{
					result = "iPad (7th)WiFi";
				}
				break;
			case 3001134901u:
				if (productType == "iPad11,2")
				{
					result = "iPad mini 5th Gen Wifi  + Cellular";
				}
				break;
			case 3430040502u:
				if (productType == "iPad2,5")
				{
					result = "iPad Mini Wifi";
				}
				break;
			case 3413262883u:
				if (productType == "iPad2,6")
				{
					result = "iPad Mini Wifi + Cellular";
				}
				break;
			case 3396485264u:
				if (productType == "iPad2,7")
				{
					result = "iPad Mini Wifi + Cellular";
				}
				break;
			case 3480373359u:
				if (productType == "iPad2,2")
				{
					result = "iPad 2 GSM";
				}
				break;
			case 3463595740u:
				if (productType == "iPad2,3")
				{
					result = "iPad 2 3G";
				}
				break;
			case 3446818121u:
				if (productType == "iPad2,4")
				{
					result = "iPad 2 Wifi";
				}
				break;
			case 3579376904u:
				if (productType == "iPhone8,4")
				{
					result = "iPhone SE";
				}
				break;
			case 3506766125u:
				if (productType == "iPhone2,1")
				{
					result = "iPhone 3GS";
				}
				break;
			case 3497150978u:
				if (productType == "iPad2,1")
				{
					result = "iPad 2 Wifi";
				}
				break;
			case 3721962577u:
				if (productType == "iPod1,1")
				{
					result = "iPod Touch";
				}
				break;
			case 3680042618u:
				if (productType == "iPhone8,2")
				{
					result = "iPhone 6S Plus";
				}
				break;
			case 3663264999u:
				if (productType == "iPhone8,1")
				{
					result = "iPhone 6S";
				}
				break;
			case 4258347964u:
				if (productType == "iPad6,7")
				{
					result = "iPad PRO 12.9 Wifi";
				}
				break;
			case 4191237488u:
				if (productType == "iPad6,3")
				{
					result = "iPad PRO 9.7 Wifi";
				}
				break;
			case 3981813096u:
				if (productType == "iPod2,1")
				{
					result = "iPod Touch Second Generation";
				}
				break;
			}
			return result;
		}
	}
}
