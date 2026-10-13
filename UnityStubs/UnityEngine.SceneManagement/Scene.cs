using System;
using Waylith.UnityCompat;

namespace UnityEngine.SceneManagement;

public struct Scene
{
	private readonly string _name;

	public bool isLoaded => !string.IsNullOrEmpty(_name);

	public int handle
	{
		get
		{
			if (!string.IsNullOrEmpty(_name))
			{
				return _name.GetHashCode();
			}
			return 0;
		}
	}

	public int buildIndex
	{
		get
		{
			if (!IsValid())
			{
				return -1;
			}
			return (!(_name == "Entry")) ? 1 : 0;
		}
	}

	public string path
	{
		get
		{
			if (!IsValid())
			{
				return string.Empty;
			}
			return "Assets/Scenes/" + _name + ".unity";
		}
	}

	public int rootCount => GetRootGameObjects().Length;

	public bool isDirty => false;

	public string name => _name ?? string.Empty;

	internal Scene(string name)
	{
		_name = name;
	}

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(_name);
	}

	public GameObject[] GetRootGameObjects()
	{
		if (UnityRuntime.Current == null || string.IsNullOrEmpty(_name))
		{
			return Array.Empty<GameObject>();
		}
		return (GameObject[])UnityRuntime.Current.World.GetSceneRootGameObjects(_name, typeof(GameObject));
	}
}
