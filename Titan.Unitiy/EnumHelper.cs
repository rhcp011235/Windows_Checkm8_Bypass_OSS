using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Titan.Unitiy
{
	public class EnumHelper
	{
		public static string GetEnumDescription(Type enumType, object val)
		{
			string name = Enum.GetName(enumType, val);
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}
			FieldInfo field = enumType.GetField(name);
			object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
			if (customAttributes.Length != 0 && customAttributes[0] is DescriptionAttribute descriptionAttribute)
			{
				return descriptionAttribute.Description;
			}
			return name;
		}

		public static IList<EnumInfo> GetEnumList<T>()
		{
			IList<EnumInfo> list = new List<EnumInfo>();
			Type typeFromHandle = typeof(T);
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				int num = (int)obj;
				EnumInfo enumInfo = new EnumInfo();
				enumInfo.Text = GetEnumDescription(typeFromHandle, num);
				EnumInfo enumInfo2 = enumInfo;
				enumInfo2.Value = GetModel<T>(num).ToString();
				enumInfo.Value2 = num;
				list.Add(enumInfo);
			}
			return list;
		}

		public static T GetModel<T>(int value)
		{
			return (T)Enum.Parse(typeof(T), value.ToString(), ignoreCase: true);
		}

		public static T GetModel<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value.ToString(), ignoreCase: true);
		}
	}
}
