using System;
using System.Collections.Generic;
using System.IO;
using UnityStubs.Internal;

namespace UnityEngine;

public class AssetBundle : Object
{
	private static readonly List<AssetBundle> _loaded = new List<AssetBundle>();

	private AssetBundleReader _reader;

	public bool isStreamedSceneAssetBundle => false;

	internal AssetBundle(AssetBundleReader reader, string name)
	{
		_reader = reader;
		base.name = name;
		lock (_loaded)
		{
			_loaded.Add(this);
		}
	}

	public string[] GetAllAssetNames()
	{
		return _reader?.AssetNames ?? Array.Empty<string>();
	}

	public string[] GetAllScenePaths()
	{
		return Array.Empty<string>();
	}

	public bool Contains(string name)
	{
		if (_reader != null)
		{
			return _reader.Contains(name);
		}
		return false;
	}

	public T LoadAsset<T>(string name) where T : Object
	{
		return LoadAsset(name, typeof(T)) as T;
	}

	public Object LoadAsset(string name)
	{
		return LoadAsset(name, typeof(Object));
	}

	public Object LoadAsset(string name, Type type)
	{
		if (string.IsNullOrEmpty(name) || _reader == null)
		{
			return null;
		}
		return _reader.Load(name, type ?? typeof(Object));
	}

	public Object[] LoadAssetWithSubAssets(string name)
	{
		return LoadAssetWithSubAssets(name, typeof(Object));
	}

	public T[] LoadAssetWithSubAssets<T>(string name) where T : Object
	{
		return Downcast<T>(LoadAssetWithSubAssets(name, typeof(T)));
	}

	public Object[] LoadAssetWithSubAssets(string name, Type type)
	{
		if (string.IsNullOrEmpty(name) || _reader == null)
		{
			return Array.Empty<Object>();
		}
		return _reader.LoadWithSubAssets(name, type ?? typeof(Object));
	}

	public T[] LoadAllAssets<T>() where T : Object
	{
		return Downcast<T>(LoadAllAssets(typeof(T)));
	}

	public Object[] LoadAllAssets()
	{
		return LoadAllAssets(typeof(Object));
	}

	public Object[] LoadAllAssets(Type type)
	{
		return _reader?.LoadAll(type ?? typeof(Object)) ?? Array.Empty<Object>();
	}

	public static AssetBundle LoadFromFile(string path)
	{
		return LoadFromFile(path, 0u, 0uL);
	}

	public static AssetBundle LoadFromFile(string path, uint crc)
	{
		return LoadFromFile(path, crc, 0uL);
	}

	public static AssetBundle LoadFromFile(string path, uint crc, ulong offset)
	{
		if (!string.IsNullOrEmpty(path) && Path.DirectorySeparatorChar == '/')
		{
			path = path.Replace('\\', '/');
		}
		if (string.IsNullOrEmpty(path) || !File.Exists(path))
		{
			Debug.LogError("Unable to open archive file: " + path);
			return null;
		}
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			if (offset != 0)
			{
				fileStream.Position = (long)offset;
			}
			return new AssetBundle(AssetBundleReader.Open(fileStream, path, fromFile: true), Path.GetFileName(path));
		}
		catch (Exception value)
		{
			Debug.LogError($"Failed to read AssetBundle '{path}': {value}");
			return null;
		}
	}

	public static AssetBundle LoadFromMemory(byte[] binary)
	{
		return LoadFromMemory(binary, 0u);
	}

	public static AssetBundle LoadFromMemory(byte[] binary, uint crc)
	{
		if (binary != null)
		{
			return LoadFromStream(new MemoryStream(binary, writable: false), crc, 0u);
		}
		return null;
	}

	public static AssetBundle LoadFromStream(Stream stream)
	{
		return LoadFromStream(stream, 0u, 0u);
	}

	public static AssetBundle LoadFromStream(Stream stream, uint crc)
	{
		return LoadFromStream(stream, crc, 0u);
	}

	public static AssetBundle LoadFromStream(Stream stream, uint crc, uint managedReadBufferSize)
	{
		if (stream == null)
		{
			return null;
		}
		try
		{
			string path = ((stream is FileStream fileStream) ? Path.GetFileName(fileStream.Name) : "memory");
			return new AssetBundle(AssetBundleReader.Open(stream, path), path);
		}
		catch (Exception value)
		{
			Debug.LogError($"Failed to read AssetBundle from stream: {value}");
			return null;
		}
	}

	public static AssetBundleCreateRequest LoadFromFileAsync(string path)
	{
		return new AssetBundleCreateRequest(LoadFromFile(path));
	}

	public static AssetBundleCreateRequest LoadFromFileAsync(string path, uint crc)
	{
		return new AssetBundleCreateRequest(LoadFromFile(path, crc));
	}

	public static AssetBundleCreateRequest LoadFromFileAsync(string path, uint crc, ulong offset)
	{
		return new AssetBundleCreateRequest(LoadFromFile(path, crc, offset));
	}

	public static AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary)
	{
		return new AssetBundleCreateRequest(LoadFromMemory(binary));
	}

	public static AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary, uint crc)
	{
		return new AssetBundleCreateRequest(LoadFromMemory(binary, crc));
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream)
	{
		return new AssetBundleCreateRequest(LoadFromStream(stream));
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream, uint crc)
	{
		return new AssetBundleCreateRequest(LoadFromStream(stream, crc));
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream, uint crc, uint managedReadBufferSize)
	{
		return new AssetBundleCreateRequest(LoadFromStream(stream, crc, managedReadBufferSize));
	}

	public void Unload(bool unloadAllLoadedObjects)
	{
		AssetBundleReader reader = _reader;
		_reader = null;
		if (reader == null)
		{
			return;
		}
		if (unloadAllLoadedObjects)
		{
			foreach (Object loadedObject in reader.LoadedObjects)
			{
				Object.Destroy(loadedObject);
			}
		}
		reader.Dispose();
		lock (_loaded)
		{
			_loaded.Remove(this);
		}
	}

	public static void UnloadAllAssetBundles(bool unloadAllObjects)
	{
		AssetBundle[] array;
		lock (_loaded)
		{
			array = _loaded.ToArray();
		}
		AssetBundle[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Unload(unloadAllObjects);
		}
	}

	public static IEnumerable<AssetBundle> GetAllLoadedAssetBundles()
	{
		lock (_loaded)
		{
			return _loaded.ToArray();
		}
	}

	private static T[] Downcast<T>(Object[] objects) where T : Object
	{
		List<T> list = new List<T>(objects.Length);
		for (int i = 0; i < objects.Length; i++)
		{
			if (objects[i] is T item)
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}
}
