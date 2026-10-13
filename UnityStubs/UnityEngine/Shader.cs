using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using UnityStubs.Internal;
using Waylith.Rendering;

namespace UnityEngine;

public class Shader : Object
{
	private static readonly HashSet<string> GlobalKeywords = new HashSet<string>(StringComparer.Ordinal);

	private static string GlobalKeywordKeyCache = string.Empty;

	private static bool GlobalKeywordsDirty = true;

	private volatile int[] _declaredPropertyIds = Array.Empty<int>();

	private volatile MaterialLayout _declaredFrom;

	private string _engineShaderPath = string.Empty;

	private Waylith.Rendering.Shader _engineShader;

	private static Shader _default;

	private static readonly Dictionary<string, Shader> ByDefName = new Dictionary<string, Shader>(StringComparer.Ordinal);

	private bool _unsupported;

	private static readonly Dictionary<int, float[]> GlobalFloatArrays = new Dictionary<int, float[]>();

	private static readonly Dictionary<int, Vector4[]> GlobalVectorArrays = new Dictionary<int, Vector4[]>();

	internal Waylith.Rendering.Shader[] ExtraPasses { get; private set; } = Array.Empty<Waylith.Rendering.Shader>();

	public static bool EngineRegistryReady { get; private set; }

	internal bool WaitingForEngineRegistry
	{
		get
		{
			Waylith.Rendering.Shader shader;
			if (_engineShader == null && !EngineRegistryReady && !string.IsNullOrEmpty(_engineShaderPath))
			{
				return !ShaderRegistry.TryResolve(_engineShaderPath, out shader);
			}
			return false;
		}
	}

	internal Waylith.Rendering.Shader EngineShader
	{
		get
		{
			if (_engineShader != null)
			{
				return _engineShader;
			}
			return ShaderRegistry.Resolve(_engineShaderPath);
		}
	}

	public static string GlobalKeywordKey => BuildGlobalKeywordKey();

	public static Shader Default => _default ?? (_default = FromEngineShader("Default", ShaderRegistry.Default));

	public bool isSupported => !_unsupported;

	public int passCount => 1 + ExtraPasses.Length;

	public int renderQueue => EngineShader.DefaultRenderQueue;

	public static void SetGlobalInt(int nameID, int value)
	{
		GlobalShaderState.SetFloat(nameID, value);
	}

	public static void MarkEngineRegistryReady()
	{
		EngineRegistryReady = true;
	}

	public static void DisableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword) && GlobalKeywords.Remove(keyword))
		{
			GlobalKeywordsDirty = true;
		}
	}

	public static void EnableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword) && GlobalKeywords.Add(keyword))
		{
			GlobalKeywordsDirty = true;
		}
	}

	public static Shader Find(string name)
	{
		return (Shader)Resources.TryLoadFileBackedResource(name, typeof(Shader));
	}

	public static Shader FromEngineShader(string name, Waylith.Rendering.Shader shader)
	{
		Shader shader2 = new Shader();
		shader2.name = name ?? shader?.Name ?? string.Empty;
		shader2.SetEngineShader(shader);
		shader2.SetDeclaredProperties(shader?.MaterialLayout);
		return shader2;
	}

	internal static Shader FromEngineShader(string name, Waylith.Rendering.Shader shader, Waylith.Rendering.Shader[] extraPasses)
	{
		Shader shader2 = FromEngineShader(name, shader);
		shader2.ExtraPasses = extraPasses;
		return shader2;
	}

	public static Shader CreateLateBound(string engineShaderPath)
	{
		if (engineShaderPath == null)
		{
			engineShaderPath = string.Empty;
		}
		lock (ByDefName)
		{
			if (ByDefName.TryGetValue(engineShaderPath, out var value))
			{
				return value;
			}
			Shader shader = new Shader
			{
				name = engineShaderPath
			};
			shader.SetEngineShaderPath(engineShaderPath);
			ByDefName[engineShaderPath] = shader;
			return shader;
		}
	}

	public static Shader ForDef(string defName, Waylith.Rendering.Shader shader)
	{
		Shader shader2 = CreateLateBound(defName);
		shader2.SetEngineShader(shader);
		shader2.SetDeclaredProperties(shader?.MaterialLayout);
		return shader2;
	}

	internal static Shader Unsupported(string name)
	{
		Shader shader = FromEngineShader(name, ShaderRegistry.Default);
		shader._unsupported = true;
		return shader;
	}

	public int GetPropertyCount()
	{
		EnsureDeclaredPropertiesFromEngineShader();
		return _declaredPropertyIds.Length;
	}

	public int GetPropertyNameId(int propertyIndex)
	{
		EnsureDeclaredPropertiesFromEngineShader();
		return _declaredPropertyIds[propertyIndex];
	}

	public bool HasProperty(int propertyId)
	{
		EnsureDeclaredPropertiesFromEngineShader();
		return Array.IndexOf(_declaredPropertyIds, propertyId) >= 0;
	}

	public static int PropertyToID(string name)
	{
		return ShaderPropertyId.GetOrCreate(name);
	}

	public static Matrix4x4 GetGlobalMatrix(string name)
	{
		return GetGlobalMatrix(PropertyToID(name));
	}

	public static Matrix4x4 GetGlobalMatrix(int nameID)
	{
		if (!TryGetGlobalMatrix(nameID, out var value))
		{
			return Matrix4x4.zero;
		}
		return value;
	}

	public static void SetGlobalFloatArray(string name, float[] values)
	{
		SetGlobalFloatArray(PropertyToID(name), values);
	}

	public static void SetGlobalFloatArray(string name, List<float> values)
	{
		SetGlobalFloatArray(PropertyToID(name), values);
	}

	public static void SetGlobalFloatArray(int nameID, float[] values)
	{
		ShaderArrays.Set(GlobalFloatArrays, nameID, ShaderArrays.Check(values));
		GlobalShaderState.SetFloatArray(nameID, GlobalFloatArrays[nameID]);
	}

	public static void SetGlobalFloatArray(int nameID, List<float> values)
	{
		ShaderArrays.Set(GlobalFloatArrays, nameID, ShaderArrays.Check(values));
		GlobalShaderState.SetFloatArray(nameID, GlobalFloatArrays[nameID]);
	}

	public static void SetGlobalVectorArray(string name, Vector4[] values)
	{
		SetGlobalVectorArray(PropertyToID(name), values);
	}

	public static void SetGlobalVectorArray(string name, List<Vector4> values)
	{
		SetGlobalVectorArray(PropertyToID(name), values);
	}

	public static void SetGlobalVectorArray(int nameID, Vector4[] values)
	{
		ShaderArrays.Set(GlobalVectorArrays, nameID, ShaderArrays.Check(values));
		GlobalShaderState.SetVectorArray(nameID, MemoryMarshal.Cast<Vector4, System.Numerics.Vector4>((ReadOnlySpan<Vector4>)GlobalVectorArrays[nameID]));
	}

	public static void SetGlobalVectorArray(int nameID, List<Vector4> values)
	{
		ShaderArrays.Set(GlobalVectorArrays, nameID, ShaderArrays.Check(values));
		GlobalShaderState.SetVectorArray(nameID, MemoryMarshal.Cast<Vector4, System.Numerics.Vector4>((ReadOnlySpan<Vector4>)GlobalVectorArrays[nameID]));
	}

	public static float[] GetGlobalFloatArray(string name)
	{
		return GetGlobalFloatArray(PropertyToID(name));
	}

	public static float[] GetGlobalFloatArray(int nameID)
	{
		if (!GlobalFloatArrays.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return (float[])value.Clone();
	}

	public static Vector4[] GetGlobalVectorArray(string name)
	{
		return GetGlobalVectorArray(PropertyToID(name));
	}

	public static Vector4[] GetGlobalVectorArray(int nameID)
	{
		if (!GlobalVectorArrays.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return (Vector4[])value.Clone();
	}

	public static Texture GetGlobalTexture(string name)
	{
		return GetGlobalTexture(PropertyToID(name));
	}

	public static Texture GetGlobalTexture(int nameID)
	{
		StubWarner.LogStub("GetGlobalTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Shader.cs", 170);
		return null;
	}

	public string GetPropertyName(int propertyIndex)
	{
		StubWarner.LogStub("GetPropertyName", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Shader.cs", 172);
		return string.Empty;
	}

	public ShaderPropertyType GetPropertyType(int propertyIndex)
	{
		StubWarner.LogStub("GetPropertyType", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Shader.cs", 173);
		return ShaderPropertyType.Float;
	}

	public Vector2 GetPropertyRangeLimits(int propertyIndex)
	{
		StubWarner.LogStub("GetPropertyRangeLimits", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Shader.cs", 174);
		return Vector2.zero;
	}

	public static void SetGlobalColor(string name, Color value)
	{
		SetGlobalColor(PropertyToID(name), value);
	}

	public static void SetGlobalColor(int nameID, Color value)
	{
		GlobalShaderState.SetVector(nameID, TextureST.Reorder(nameID, ToNumerics(value)));
	}

	public static void SetGlobalFloat(string name, float value)
	{
		SetGlobalFloat(PropertyToID(name), value);
	}

	public static void SetGlobalFloat(int nameID, float value)
	{
		GlobalShaderState.SetFloat(nameID, value);
	}

	public static void SetGlobalMatrix(string name, Matrix4x4 value)
	{
		SetGlobalMatrix(PropertyToID(name), value);
	}

	public static void SetGlobalMatrix(int nameID, Matrix4x4 value)
	{
		GlobalShaderState.SetMatrix(nameID, Matrix4x4.ToNumerics(value));
	}

	public static void SetGlobalTexture(string name, Texture value)
	{
		SetGlobalTexture(PropertyToID(name), value);
	}

	public static void SetGlobalTexture(int nameID, Texture value)
	{
		Texture.EnsureSampledTextureReady(value);
		if (value?.CoreTexture != null)
		{
			GlobalShaderState.SetTexture(nameID, value.CoreTexture);
		}
	}

	public static void SetGlobalVector(string name, Vector4 value)
	{
		SetGlobalVector(PropertyToID(name), value);
	}

	public static void SetGlobalVector(int nameID, Vector4 value)
	{
		GlobalShaderState.SetVector(nameID, TextureST.Reorder(nameID, ToNumerics(value)));
	}

	private static System.Numerics.Vector4 ToNumerics(Color c)
	{
		return new System.Numerics.Vector4(c.r, c.g, c.b, c.a);
	}

	private static System.Numerics.Vector4 ToNumerics(Vector4 v)
	{
		return new System.Numerics.Vector4(v.x, v.y, v.z, v.w);
	}

	public static bool TryGetGlobalFloat(int nameID, out float value)
	{
		return GlobalShaderState.TryGetFloat(nameID, out value);
	}

	public static float GetGlobalFloat(int nameID)
	{
		if (!TryGetGlobalFloat(nameID, out var value))
		{
			return 0f;
		}
		return value;
	}

	public static float GetGlobalFloat(string name)
	{
		return GetGlobalFloat(PropertyToID(name));
	}

	public static int GetGlobalInt(int nameID)
	{
		return (int)GetGlobalFloat(nameID);
	}

	public static int GetGlobalInt(string name)
	{
		return GetGlobalInt(PropertyToID(name));
	}

	public static Vector4 GetGlobalVector(int nameID)
	{
		if (!TryGetGlobalVector(nameID, out var value))
		{
			return Vector4.zero;
		}
		return value;
	}

	public static Vector4 GetGlobalVector(string name)
	{
		return GetGlobalVector(PropertyToID(name));
	}

	public static Color GetGlobalColor(int nameID)
	{
		return GetGlobalVector(nameID);
	}

	public static Color GetGlobalColor(string name)
	{
		return GetGlobalColor(PropertyToID(name));
	}

	public static void SetGlobalInt(string name, int value)
	{
		SetGlobalInt(PropertyToID(name), value);
	}

	public static bool TryGetGlobalVector(int nameID, out Vector4 value)
	{
		if (GlobalShaderState.TryGetVector(nameID, out var value2))
		{
			value = Vector4.FromNumerics(TextureST.Reorder(nameID, value2));
			return true;
		}
		value = default(Vector4);
		return false;
	}

	public static bool TryGetGlobalColor(int nameID, out Color value)
	{
		if (GlobalShaderState.TryGetVector(nameID, out var value2))
		{
			value = Color.FromNumerics(TextureST.Reorder(nameID, value2));
			return true;
		}
		value = default(Color);
		return false;
	}

	public static bool TryGetGlobalMatrix(int nameID, out Matrix4x4 value)
	{
		if (GlobalShaderState.TryGetMatrix(nameID, out var value2))
		{
			value = Matrix4x4.FromNumerics(value2);
			return true;
		}
		value = default(Matrix4x4);
		return false;
	}

	internal void SetDeclaredProperties(MaterialLayout layout)
	{
		_declaredPropertyIds = GetDeclaredPropertyIds(layout);
		_declaredFrom = layout;
	}

	internal void SetEngineShader(Waylith.Rendering.Shader shader)
	{
		_engineShader = shader ?? ShaderRegistry.Default;
		_engineShaderPath = _engineShader.ShaderPath;
	}

	internal void SetEngineShaderPath(string shaderPath)
	{
		_engineShaderPath = shaderPath ?? string.Empty;
		_engineShader = null;
	}

	private void EnsureDeclaredPropertiesFromEngineShader()
	{
		Waylith.Rendering.Shader shader = _engineShader;
		if ((_declaredPropertyIds.Length == 0 || (shader != null && _declaredFrom != shader.MaterialLayout)) && (shader != null || ShaderRegistry.TryResolve(_engineShaderPath, out shader)))
		{
			SetDeclaredProperties(shader.MaterialLayout);
		}
	}

	private static int[] GetDeclaredPropertyIds(MaterialLayout layout)
	{
		if (layout == null)
		{
			return Array.Empty<int>();
		}
		int[] array = new int[layout.UniformSlots.Length + layout.TextureSlots.Length];
		int num = 0;
		MaterialUniformSlot[] uniformSlots = layout.UniformSlots;
		foreach (MaterialUniformSlot materialUniformSlot in uniformSlots)
		{
			array[num++] = materialUniformSlot.PropertyId;
		}
		MaterialTextureSlot[] textureSlots = layout.TextureSlots;
		foreach (MaterialTextureSlot materialTextureSlot in textureSlots)
		{
			array[num++] = materialTextureSlot.PropertyId;
		}
		return array;
	}

	private static string BuildGlobalKeywordKey()
	{
		if (!GlobalKeywordsDirty)
		{
			return GlobalKeywordKeyCache;
		}
		if (GlobalKeywords.Count == 0)
		{
			GlobalKeywordKeyCache = string.Empty;
			GlobalKeywordsDirty = false;
			return GlobalKeywordKeyCache;
		}
		string[] array = new string[GlobalKeywords.Count];
		int num = 0;
		foreach (string globalKeyword in GlobalKeywords)
		{
			array[num++] = globalKeyword;
		}
		Array.Sort(array, (IComparer<string>?)StringComparer.Ordinal);
		GlobalKeywordKeyCache = string.Join(";", array);
		GlobalKeywordsDirty = false;
		return GlobalKeywordKeyCache;
	}
}
