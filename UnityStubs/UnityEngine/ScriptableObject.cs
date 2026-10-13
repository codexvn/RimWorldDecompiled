using System;

namespace UnityEngine;

public class ScriptableObject : Object
{
	public static T0 CreateInstance<T0>() where T0 : ScriptableObject, new()
	{
		return new T0();
	}

	public static ScriptableObject CreateInstance(Type type)
	{
		if (type == null || !typeof(ScriptableObject).IsAssignableFrom(type))
		{
			throw new ArgumentException($"{type} is not a ScriptableObject");
		}
		return (ScriptableObject)Activator.CreateInstance(type, nonPublic: true);
	}

	public static ScriptableObject CreateInstance(string className)
	{
		return CreateInstance(Type.GetType(className, throwOnError: false));
	}
}
