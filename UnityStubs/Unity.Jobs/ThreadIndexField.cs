using System.Reflection;
using Unity.Collections;
using Waylith.Jobs;

namespace Unity.Jobs;

internal static class ThreadIndexField<T> where T : struct
{
	private static readonly FieldInfo Field = Find();

	private static FieldInfo Find()
	{
		FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType == typeof(int) && fieldInfo.IsDefined(typeof(NativeSetThreadIndexAttribute), inherit: false))
			{
				return fieldInfo;
			}
		}
		return null;
	}

	public static T Inject(T job)
	{
		if (Field == null)
		{
			return job;
		}
		object obj = job;
		Field.SetValue(obj, WaylithJobRuntime.CurrentThreadIndex);
		return (T)obj;
	}
}
