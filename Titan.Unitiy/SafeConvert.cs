using System;

namespace Titan.Unitiy
{
	public static class SafeConvert
	{
		public static bool ToBoolean(string s)
		{
			bool flag;
			return bool.TryParse(s, out flag) && flag;
		}

		public static bool[] ToBoolean(string[] s)
		{
			return Array.ConvertAll(s, ToBoolean);
		}

		public static byte ToByte(string s)
		{
			if (!byte.TryParse(s, out var b))
			{
				return 0;
			}
			return b;
		}

		public static byte[] ToByte(string[] s)
		{
			return Array.ConvertAll(s, ToByte);
		}

		public static sbyte ToSbyte(string s)
		{
			if (!sbyte.TryParse(s, out var b))
			{
				return 0;
			}
			return b;
		}

		public static sbyte[] ToSbyte(string[] s)
		{
			return Array.ConvertAll(s, ToSbyte);
		}

		public static short ToInt16(string s)
		{
			return ToInt16(s, 0);
		}

		public static short ToInt16(string s, short defaultValue)
		{
			if (!short.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static short[] ToInt16(string[] s)
		{
			return Array.ConvertAll(s, ToInt16);
		}

		public static ushort ToUInt16(string s)
		{
			return ToUInt16(s, 0);
		}

		public static ushort ToUInt16(string s, ushort defaultValue)
		{
			if (!ushort.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static ushort[] ToUInt16(string[] s)
		{
			return Array.ConvertAll(s, ToUInt16);
		}

		public static int ToInt32(string s)
		{
			return ToInt32(s, 0);
		}

		public static int ToInt32(string s, int defaultValue)
		{
			if (!int.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static int[] ToInt32(string[] s)
		{
			return Array.ConvertAll(s, ToInt32);
		}

		public static uint ToUInt32(string s)
		{
			if (!uint.TryParse(s, out var num))
			{
				return 0u;
			}
			return num;
		}

		public static uint ToUInt32(string s, uint defalutValue)
		{
			if (!uint.TryParse(s, out var num))
			{
				return defalutValue;
			}
			return num;
		}

		public static uint[] ToUInt32(string[] s)
		{
			return Array.ConvertAll(s, ToUInt32);
		}

		public static long ToInt64(string s, long defalutValue)
		{
			if (!long.TryParse(s, out var num))
			{
				return defalutValue;
			}
			return num;
		}

		public static long ToInt64(string s)
		{
			return ToInt64(s, 0L);
		}

		public static long[] ToInt64(string[] s)
		{
			return Array.ConvertAll(s, ToInt64);
		}

		public static ulong ToUInt64(string s)
		{
			return ToUInt64(s, 0u);
		}

		public static ulong ToUInt64(string s, uint defaultValue)
		{
			if (!ulong.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static ulong[] ToUInt64(string[] s)
		{
			return Array.ConvertAll(s, ToUInt64);
		}

		public static float ToSingle(string s, float defaultValue)
		{
			if (!float.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static float ToSingle(string s)
		{
			return ToSingle(s, 0f);
		}

		public static float[] ToSingle(string[] s)
		{
			return Array.ConvertAll(s, ToSingle);
		}

		public static double ToDouble(string s, double defaultValue)
		{
			if (!double.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static double ToDouble(string s)
		{
			return ToDouble(s, 0.0);
		}

		public static double[] ToDouble(string[] s)
		{
			return Array.ConvertAll(s, ToDouble);
		}

		public static float ToFloat(string s, float defaultValue = 0f)
		{
			if (!float.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static decimal ToDecimal(string s, decimal defaultValue)
		{
			if (!decimal.TryParse(s, out var num))
			{
				return defaultValue;
			}
			return num;
		}

		public static decimal ToDecimal(string s)
		{
			return ToDecimal(s, 0m);
		}

		public static decimal[] ToDecimal(string[] s)
		{
			return Array.ConvertAll(s, ToDecimal);
		}

		public static DateTime ToDateTime(string s, DateTime defaultValue)
		{
			if (!DateTime.TryParse(s, out var dateTime))
			{
				return defaultValue;
			}
			return dateTime;
		}

		public static DateTime ToDateTime(string s)
		{
			return ToDateTime(s, DateTime.MinValue);
		}

		public static DateTime[] ToDateTime(string[] s)
		{
			return Array.ConvertAll(s, ToDateTime);
		}

		public static TimeSpan ToTimeSpan(string s, TimeSpan defaultValue)
		{
			if (!TimeSpan.TryParse(s, out var timeSpan))
			{
				return defaultValue;
			}
			return timeSpan;
		}

		public static TimeSpan ToTimeSpan(string s)
		{
			return ToTimeSpan(s, TimeSpan.Zero);
		}

		public static TimeSpan[] ToTimeSpan(string[] s)
		{
			return Array.ConvertAll(s, ToTimeSpan);
		}

		public static object ToEnum(object obj, Type enumType)
		{
			if (Enum.IsDefined(enumType, obj))
			{
				string[] names = Enum.GetNames(enumType);
				string text = obj.ToString();
				for (int i = 0; i < names.Length; i++)
				{
					if (text == names[i])
					{
						return Enum.Parse(enumType, text);
					}
				}
				return Enum.ToObject(enumType, obj);
			}
			return null;
		}

		public static T ToEnum<T>(object obj) where T : struct
		{
			int value = 0;
			bool flag = int.TryParse(obj.ToString(), out value);
			if (Enum.IsDefined(typeof(T), obj))
			{
				string[] names = Enum.GetNames(typeof(T));
				string text = obj.ToString();
				for (int i = 0; i < names.Length; i++)
				{
					if (text == names[i])
					{
						return (T)Enum.Parse(typeof(T), text);
					}
				}
				return (T)Enum.ToObject(typeof(T), obj);
			}
			if (flag)
			{
				return (T)Enum.ToObject(typeof(T), value);
			}
			return default(T);
		}

		public static T[] ToEnum<T>(object[] s) where T : struct
		{
			return Array.ConvertAll(s, ToEnum<T>);
		}

		public static string ToString(string p)
		{
			return p;
		}

		public static DateTime? ToDateTime(long timeStamp)
		{
			DateTime? result;
			if (timeStamp == 0)
			{
				result = null;
			}
			else
			{
				DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
				long ticks = long.Parse(timeStamp + "0000000");
				TimeSpan value = new TimeSpan(ticks);
				result = dateTime.Add(value);
			}
			return result;
		}

		public static long ToDateTimeInt(DateTime time)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return (long)(time - d).TotalSeconds;
		}
	}
}
