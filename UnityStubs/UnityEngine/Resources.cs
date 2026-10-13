using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityStubs.Internal;
using Waylith;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;
using Waylith.UnityCompat;
using Waylith.Utils;

namespace UnityEngine;

public class Resources
{
	private static readonly ConditionalWeakTable<object, Object> EngineWrappers = new ConditionalWeakTable<object, Object>();

	private static readonly ConcurrentDictionary<string, Lazy<Object>> StubResourceCache = new ConcurrentDictionary<string, Lazy<Object>>(StringComparer.OrdinalIgnoreCase);

	private static readonly string[] TextAssetExtensions = new string[6] { ".txt", ".xml", ".json", ".csv", ".bytes", ".glb" };

	private static readonly string[] AudioClipExtensions = new string[6] { ".mp3", ".ogg", ".aac", ".wav", ".flac", ".m4a" };

	private static readonly string[] GameFontNames = new string[3] { "Calibri_tiny", "Arial_small", "Arial_medium" };

	public static T0[] FindObjectsOfTypeAll<T0>()
	{
		return (T0[])FindObjectsOfTypeAll(typeof(T0), typeof(T0));
	}

	public static Object[] FindObjectsOfTypeAll(Type type)
	{
		return (Object[])FindObjectsOfTypeAll(type, typeof(Object));
	}

	private static Array FindObjectsOfTypeAll(Type type, Type elementType)
	{
		List<object> list = new List<object>();
		if (UnityRuntime.Current != null)
		{
			list.AddRange((IEnumerable<object>)UnityRuntime.Current.World.FindObjectsByType(type, includeInactive: true));
			if (type != typeof(GameObject) && type.IsAssignableFrom(typeof(GameObject)))
			{
				list.AddRange((IEnumerable<object>)UnityRuntime.Current.World.FindObjectsByType(typeof(GameObject), includeInactive: true));
			}
		}
		AssetRegistry.Collect(type, list);
		Array array = Array.CreateInstance(elementType, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			array.SetValue(list[i], i);
		}
		return array;
	}

	public static T0 Load<T0>(string path)
	{
		if (typeof(T0) == typeof(Font))
		{
			return (T0)(object)LoadFont(path);
		}
		return (T0)(object)Load(path, typeof(T0));
	}

	private static Font LoadFont(string path)
	{
		int num = Array.FindIndex(GameFontNames, (string n) => string.Equals(n, Path.GetFileName(path ?? ""), StringComparison.OrdinalIgnoreCase));
		if (num >= 0)
		{
			return new Font
			{
				GameFontIndex = num
			};
		}
		return null;
	}

	public static Object Load(string path)
	{
		return Load(path, typeof(Object));
	}

	public static Object Load(string path, Type systemTypeInstance)
	{
		if (systemTypeInstance == typeof(Font))
		{
			return LoadFont(path);
		}
		if (UnityRuntime.Current == null)
		{
			return null;
		}
		return ((Object)UnityRuntime.Current.World.LoadResource(path, systemTypeInstance)) ?? TryLoadFileBackedResource(path, systemTypeInstance);
	}

	public static T[] LoadAll<T>(string path) where T : Object
	{
		return LoadAll(path, typeof(T)).Cast<T>().ToArray();
	}

	public static Object[] LoadAll(string path, Type systemTypeInstance)
	{
		if (UnityRuntime.Current == null)
		{
			return Array.Empty<Object>();
		}
		List<Object> list = new List<Object>();
		HashSet<Object> hashSet = new HashSet<Object>();
		Object[] array = (Object[])UnityRuntime.Current.World.LoadAllResources(path, systemTypeInstance);
		foreach (Object @object in array)
		{
			if (@object != null && hashSet.Add(@object))
			{
				list.Add(@object);
			}
		}
		foreach (Object item in LoadAllFileBackedResources(path, systemTypeInstance))
		{
			if (item != null && hashSet.Add(item))
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static T GetBuiltinResource<T>(string path) where T : Object
	{
		return (T)GetBuiltinResource(typeof(T), path);
	}

	public static Object GetBuiltinResource(Type type, string path)
	{
		if (type.IsAssignableFrom(typeof(Font)) && path != null && path.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase))
		{
			return new Font
			{
				name = Path.GetFileNameWithoutExtension(path),
				GameFontIndex = 1
			};
		}
		StubWarner.LogStub("GetBuiltinResource", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Resources.cs", 108);
		return null;
	}

	public static AsyncOperation UnloadUnusedAssets()
	{
		return new AsyncOperation();
	}

	internal static Object TryLoadFileBackedResource(string path, Type requestedType)
	{
		if (TryGetEngineType(requestedType, out var engineType))
		{
			return Wrap(Waylith.Resources.Load(path, engineType));
		}
		string key = requestedType.FullName + ":" + ResourceFiles.NormalizePath(path);
		return StubResourceCache.GetOrAdd(key, (string _) => new Lazy<Object>(() => LoadStubResource(path, requestedType))).Value;
	}

	private static IEnumerable<Object> LoadAllFileBackedResources(string path, Type requestedType)
	{
		Type engineType;
		string[] array = (TryGetEngineType(requestedType, out engineType) ? Waylith.Resources.LoadAllExtensionsFor(engineType) : StubExtensionsFor(requestedType));
		if (array.Length == 0)
		{
			return Array.Empty<Object>();
		}
		return from resourcePath in Waylith.Resources.EnumerateResourcePaths(path, array)
			select TryLoadFileBackedResource(resourcePath, requestedType);
	}

	private static bool TryGetEngineType(Type requestedType, out Type engineType)
	{
		engineType = ((requestedType == typeof(Texture2D)) ? typeof(Waylith.Rendering.Texture2D) : ((requestedType == typeof(Texture3D)) ? typeof(Waylith.Rendering.Texture3D) : ((requestedType == typeof(Mesh)) ? typeof(Waylith.Graphics.Meshes.Mesh) : null)));
		return engineType != null;
	}

	private static Object Wrap(object engineResource)
	{
		if (engineResource != null)
		{
			return EngineWrappers.GetValue(engineResource, CreateWrapper);
		}
		return null;
	}

	private static Object CreateWrapper(object engineResource)
	{
		if (!(engineResource is Waylith.Rendering.Texture2D texture))
		{
			if (!(engineResource is Waylith.Rendering.Texture3D core))
			{
				if (engineResource is Waylith.Graphics.Meshes.Mesh core2)
				{
					return new Mesh(core2);
				}
				throw new NotSupportedException($"No UnityEngine wrapper for resource type '{engineResource.GetType()}'.");
			}
			return new Texture3D(core);
		}
		return WrapTexture(texture);
	}

	private static Texture2D WrapTexture(Waylith.Rendering.Texture2D texture)
	{
		Texture2D obj = new Texture2D(texture)
		{
			wrapMode = TextureWrapMode.Clamp
		};
		Texture2D.CompressLikeImporter(obj);
		return obj;
	}

	private static string[] StubExtensionsFor(Type requestedType)
	{
		if (requestedType == typeof(TextAsset))
		{
			return TextAssetExtensions;
		}
		if (requestedType == typeof(AudioClip))
		{
			return AudioClipExtensions;
		}
		return Array.Empty<string>();
	}

	private static Object LoadStubResource(string path, Type requestedType)
	{
		if (requestedType == typeof(TextAsset))
		{
			string text = Waylith.Resources.FindFile(path, TextAssetExtensions, includeBareIfHasExtension: true);
			if (text == null)
			{
				return null;
			}
			return TextAsset.LoadFromFile(text);
		}
		if (requestedType == typeof(Shader))
		{
			if (!ShaderRegistry.TryResolve(path, out Waylith.Rendering.Shader shader))
			{
				return null;
			}
			return Shader.FromEngineShader(Path.GetFileNameWithoutExtension(path), shader);
		}
		if (requestedType == typeof(AudioClip))
		{
			string text2 = Waylith.Resources.FindFile(path, AudioClipExtensions);
			if (text2 == null)
			{
				return null;
			}
			return AudioClip.Load(text2, importedByUnity: true);
		}
		return null;
	}
}
