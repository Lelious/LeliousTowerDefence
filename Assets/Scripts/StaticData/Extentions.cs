using System;
using System.Collections.Generic;

public static class Extentions
{
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T item in source)
		{
			action(item);
		}
		return source;
	}

	public static T With<T>(this T self, Action<T> set)
	{
		set.Invoke(self);
		return self;
	}

	public static T With<T>(this T self, Action<T> apply, Func<bool> when)
	{
		if (when())
		{
			apply?.Invoke(self);
		}
		return self;
	}

	public static T With<T>(this T self, Action<T> apply, bool when)
	{
		if (when)
		{
			apply?.Invoke(self);
		}
		return self;
	}
}
