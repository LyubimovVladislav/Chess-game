using System;

namespace Chess.Extensions
{
	public static class ClampExtension
	{
		public static T Clamp<T>(this T val, T min) where T : IComparable<T>
		{
			return val.CompareTo(min) < 0 ? min : val;
		}
	}
}