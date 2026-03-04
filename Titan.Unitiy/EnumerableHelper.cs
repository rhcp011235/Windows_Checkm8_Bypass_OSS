using System;
using System.Collections.Generic;

namespace Titan.Unitiy
{
	public static class EnumerableHelper
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T obj in enumeration)
			{
				action(obj);
			}
		}
	}
}
