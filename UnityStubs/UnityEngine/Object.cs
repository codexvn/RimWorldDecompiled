using System;
using System.Threading;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Object
{
	private static int nextFallbackInstanceId;

	private string _fallbackName = string.Empty;

	private bool _destroyed;

	private readonly int fallbackInstanceId = Interlocked.Decrement(ref nextFallbackInstanceId);

	public const string cloneDestroyedMessage = "Instantiate failed because the clone was destroyed during creation. This can happen if DestroyImmediate is called in MonoBehaviour.Awake.";

	public const int kInstanceID_None = 0;

	public const string objectIsNullMessage = "The Object you want to instantiate is null.";

	public string name
	{
		get
		{
			return UnityRuntime.Current?.World.GetName(this) ?? _fallbackName;
		}
		set
		{
			_fallbackName = value ?? string.Empty;
			UnityRuntime.Current?.World.SetName(this, _fallbackName);
			OnNameChanged(_fallbackName);
		}
	}

	public HideFlags hideFlags
	{
		set
		{
			StubWarner.LogStub("hideFlags", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Object.cs", 193);
		}
	}

	public static T[] FindObjectsOfType<T>()
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<T>();
		}
		return (T[])UnityRuntime.Current.World.FindObjectsByType(typeof(T), includeInactive: false);
	}

	public Object()
	{
		if ((!(this is Component) && !(this is GameObject)) || 1 == 0)
		{
			AssetRegistry.Register(this);
		}
	}

	internal static void MarkDestroyed(object instance)
	{
		if (instance is Object @object)
		{
			@object._destroyed = true;
		}
	}

	public static bool operator !=(Object x, Object y)
	{
		return !(x == y);
	}

	public static void Destroy(Object obj)
	{
		if (obj == null)
		{
			return;
		}
		if (UnityRuntime.Current == null)
		{
			if (obj is IDisposable disposable)
			{
				disposable.Dispose();
			}
			obj._destroyed = true;
		}
		else if (UnityRuntime.Current.World.GetInstanceId(obj) == 0)
		{
			if (obj is IDisposable disposable2)
			{
				disposable2.Dispose();
			}
			obj._destroyed = true;
		}
		else
		{
			UnityRuntime.Current.World.DestroyObject(obj, immediate: false);
		}
	}

	public static void DestroyImmediate(Object obj)
	{
		if (obj == null)
		{
			return;
		}
		if (UnityRuntime.Current == null)
		{
			if (obj is IDisposable disposable)
			{
				disposable.Dispose();
			}
			obj._destroyed = true;
		}
		else if (UnityRuntime.Current.World.GetInstanceId(obj) == 0)
		{
			if (obj is IDisposable disposable2)
			{
				disposable2.Dispose();
			}
			obj._destroyed = true;
		}
		else
		{
			UnityRuntime.Current.World.DestroyObject(obj, immediate: true);
		}
	}

	public static void DestroyImmediate(Object obj, bool allowDestroyingAssets)
	{
		DestroyImmediate(obj);
	}

	public static void DontDestroyOnLoad(Object target)
	{
		if (!(target == null) && UnityRuntime.Current != null)
		{
			UnityRuntime.Current.World.DontDestroyOnLoad(target);
		}
	}

	public static T0[] FindObjectsByType<T0>(FindObjectsSortMode sortMode)
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<T0>();
		}
		return (T0[])UnityRuntime.Current.World.FindObjectsByType(typeof(T0), includeInactive: true);
	}

	public static T0[] FindObjectsByType<T0>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode)
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<T0>();
		}
		return (T0[])UnityRuntime.Current.World.FindObjectsByType(typeof(T0), findObjectsInactive == FindObjectsInactive.Include);
	}

	public int GetInstanceID()
	{
		if (UnityRuntime.Current == null)
		{
			return fallbackInstanceId;
		}
		int instanceId = UnityRuntime.Current.World.GetInstanceId(this);
		if (instanceId != 0)
		{
			return instanceId;
		}
		return fallbackInstanceId;
	}

	public static T0 Instantiate<T0>(T0 original)
	{
		if (UnityRuntime.Current == null || original == null)
		{
			return original;
		}
		return (T0)UnityRuntime.Current.World.InstantiateObject(original);
	}

	public static T0 Instantiate<T0>(T0 original, Transform parent)
	{
		return Instantiate(original, parent, instantiateInWorldSpace: false);
	}

	public static T0 Instantiate<T0>(T0 original, Transform parent, bool instantiateInWorldSpace)
	{
		T0 val = Instantiate(original);
		Transform transform = TransformOf(val);
		if ((object)transform != null)
		{
			transform.SetParent(parent, instantiateInWorldSpace);
			return val;
		}
		return val;
	}

	public static T0 Instantiate<T0>(T0 original, Vector3 position, Quaternion rotation)
	{
		return Instantiate(original, position, rotation, null);
	}

	public static T0 Instantiate<T0>(T0 original, Vector3 position, Quaternion rotation, Transform parent)
	{
		T0 val = Instantiate(original);
		Transform transform = TransformOf(val);
		if (transform != null)
		{
			transform.SetParent(parent, worldPositionStays: true);
			transform.position = position;
			transform.rotation = rotation;
		}
		return val;
	}

	private static Transform TransformOf(object o)
	{
		if (!(o is GameObject { transform: var transform }))
		{
			if (!(o is Component { transform: var transform2 }))
			{
				return null;
			}
			return transform2;
		}
		return transform;
	}

	public static bool operator ==(Object x, Object y)
	{
		if ((object)x == y)
		{
			return true;
		}
		if ((object)y == null)
		{
			return x._destroyed;
		}
		if ((object)x == null)
		{
			return y._destroyed;
		}
		return false;
	}

	public static implicit operator bool(Object exists)
	{
		if ((object)exists == null)
		{
			return false;
		}
		return !exists._destroyed;
	}

	protected virtual void OnNameChanged(string newName)
	{
	}
}
