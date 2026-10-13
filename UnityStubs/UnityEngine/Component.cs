using System;
using System.Collections.Generic;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Component : Object
{
	public GameObject gameObject => (GameObject)UnityRuntime.Current.World.GetGameObject(this);

	public Transform transform => (Transform)UnityRuntime.Current.World.GetTransform(this);

	public T0 GetComponent<T0>()
	{
		if (UnityRuntime.Current == null)
		{
			return default(T0);
		}
		return (T0)UnityRuntime.Current.World.GetComponent(gameObject, typeof(T0));
	}

	public Component GetComponent(Type type)
	{
		return gameObject.GetComponent(type);
	}

	public T0[] GetComponents<T0>()
	{
		return gameObject.GetComponents<T0>();
	}

	public void GetComponents<T0>(List<T0> results)
	{
		ArgumentNullException.ThrowIfNull(results, "results");
		results.Clear();
		results.AddRange(GetComponents<T0>());
	}

	public Component[] GetComponents(Type type)
	{
		return gameObject.GetComponents(type);
	}

	public void GetComponents(Type type, List<Component> results)
	{
		ArgumentNullException.ThrowIfNull(results, "results");
		results.Clear();
		results.AddRange(gameObject.GetComponents(type));
	}

	public T0[] GetComponentsInChildren<T0>()
	{
		return GetComponentsInChildren<T0>(includeInactive: false);
	}

	public T0[] GetComponentsInChildren<T0>(bool includeInactive)
	{
		return gameObject.GetComponentsInChildren<T0>(includeInactive);
	}

	public T0 GetComponentInChildren<T0>(bool includeInactive)
	{
		return gameObject.GetComponentInChildren<T0>(includeInactive);
	}

	public T0 GetComponentInChildren<T0>()
	{
		if (UnityRuntime.Current == null)
		{
			return default(T0);
		}
		return (T0)UnityRuntime.Current.World.GetComponentInChildren(gameObject, typeof(T0), includeInactive: false);
	}

	public bool TryGetComponent<T0>(out T0 component)
	{
		if (UnityRuntime.Current != null && UnityRuntime.Current.World.TryGetComponent(gameObject, typeof(T0), out object component2) && component2 is T0 val)
		{
			component = val;
			return true;
		}
		component = default(T0);
		return false;
	}
}
