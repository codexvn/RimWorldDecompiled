using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using UnityStubs;
using UnityStubs.Internal;
using Waylith.Rendering;

namespace UnityEngine;

public class Material : Object
{
	private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

	private static readonly object DeferredResolveLock = new object();

	private static readonly List<Material> PendingDeferredMaterials = new List<Material>();

	private readonly Waylith.Rendering.Material _engineMaterial;

	private readonly HashSet<string> _keywords = new HashSet<string>();

	private readonly Dictionary<string, string> _overrideTags = new Dictionary<string, string>();

	private readonly Dictionary<int, Texture> _textures = new Dictionary<int, Texture>();

	private Shader _shader;

	private Waylith.Rendering.Shader[] _passShaders = Array.Empty<Waylith.Rendering.Shader>();

	private Waylith.Rendering.Material[] _passMaterials = Array.Empty<Waylith.Rendering.Material>();

	private int _passMaterialsVersion = -1;

	private Dictionary<int, TextureWrapMode> _inheritedWraps;

	private Dictionary<int, ComputeBuffer> _buffers;

	private volatile Func<Material> _deferredResolver;

	private bool _resolving;

	private bool _renderQueueOverridden;

	[ThreadStatic]
	private static Waylith.Rendering.Material _immediatePassMaterial;

	[ThreadStatic]
	private static Material _immediatePassSource;

	public bool IsDeferredMaterial => _deferredResolver != null;

	public Waylith.Rendering.Material engineMaterial
	{
		get
		{
			EnsureResolved();
			return _engineMaterial;
		}
	}

	public Color color
	{
		get
		{
			return GetColor("_Color");
		}
		set
		{
			SetColor("_Color", value);
		}
	}

	public Texture mainTexture
	{
		get
		{
			return GetTexture("_MainTex");
		}
		set
		{
			SetTexture("_MainTex", value);
		}
	}

	public int passCount
	{
		get
		{
			Shader obj = shader;
			return 1 + (((object)obj != null) ? obj.ExtraPasses.Length : 0);
		}
	}

	internal Waylith.Rendering.Material[] ExtraPassMaterials
	{
		get
		{
			Waylith.Rendering.Shader[] array = shader?.ExtraPasses;
			if (array == null || array.Length == 0)
			{
				return Array.Empty<Waylith.Rendering.Material>();
			}
			if (_passShaders != array)
			{
				_passShaders = array;
				_passMaterials = new Waylith.Rendering.Material[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					_passMaterials[i] = new Waylith.Rendering.Material(array[i], base.name);
				}
				_passMaterialsVersion = -1;
			}
			Waylith.Rendering.Material material = engineMaterial;
			if (_passMaterialsVersion != material.Version)
			{
				Waylith.Rendering.Material[] passMaterials = _passMaterials;
				foreach (Waylith.Rendering.Material obj in passMaterials)
				{
					obj.CopyPropertiesFrom(material);
					obj.RenderQueue = material.RenderQueue;
					obj.WrapModeOverride = material.WrapModeOverride;
				}
				_passMaterialsVersion = material.Version;
			}
			return _passMaterials;
		}
	}

	public bool enableInstancing { get; set; }

	public int renderQueue
	{
		get
		{
			EnsureResolved();
			return _engineMaterial.RenderQueue;
		}
		set
		{
			_renderQueueOverridden = true;
			_engineMaterial.RenderQueue = value;
		}
	}

	public Shader shader
	{
		get
		{
			EnsureResolved();
			return _shader;
		}
		set
		{
			if ((object)_shader != value)
			{
				InheritDeclaredWraps(_shader);
				_shader = value;
				SyncEngineShader();
				ApplyInheritedWraps();
			}
		}
	}

	public Vector2 mainTextureOffset
	{
		get
		{
			return GetTextureOffset(Shader.PropertyToID("_MainTex"));
		}
		set
		{
			SetTextureOffset(Shader.PropertyToID("_MainTex"), value);
		}
	}

	public Vector2 mainTextureScale
	{
		get
		{
			return GetTextureScale(Shader.PropertyToID("_MainTex"));
		}
		set
		{
			SetTextureScale(Shader.PropertyToID("_MainTex"), value);
		}
	}

	public MaterialGlobalIlluminationFlags globalIlluminationFlags { get; set; }

	public bool doubleSidedGI { get; set; }

	internal static Waylith.Rendering.Material CurrentImmediateMaterial => _immediatePassMaterial;

	internal static Material CurrentImmediateSource => _immediatePassSource;

	public string[] shaderKeywords
	{
		get
		{
			EnsureResolved();
			return _keywords.ToArray();
		}
		set
		{
			EnsureResolved();
			string[] array = _keywords.ToArray();
			foreach (string text in array)
			{
				if (Array.IndexOf(value, text) < 0)
				{
					DisableKeyword(text);
				}
			}
			array = value;
			foreach (string keyword in array)
			{
				EnableKeyword(keyword);
			}
		}
	}

	public Material(Material source)
	{
		ArgumentNullException.ThrowIfNull(source, "source");
		if (source._deferredResolver != null)
		{
			_engineMaterial = Waylith.Rendering.Material.CreateUnseeded();
			_deferredResolver = () => source;
			RegisterDeferred();
			CopyBuffersFrom(source);
			return;
		}
		_engineMaterial = new Waylith.Rendering.Material(source.engineMaterial.Shader, source.name)
		{
			RenderQueue = source.engineMaterial.RenderQueue
		};
		CopyPropertiesFrom(source.engineMaterial);
		_engineMaterial.CopyArraysFrom(source.engineMaterial);
		base.name = source.name;
		_shader = source.shader;
		foreach (string keyword in source._keywords)
		{
			_keywords.Add(keyword);
			_engineMaterial.EnableKeyword(keyword);
		}
		foreach (KeyValuePair<string, string> overrideTag in source._overrideTags)
		{
			_overrideTags[overrideTag.Key] = overrideTag.Value;
		}
		foreach (var (key, value) in source._textures)
		{
			_textures[key] = value;
		}
		InheritDeclaredWraps(source);
		CopyBuffersFrom(source);
	}

	private void CopyBuffersFrom(Material source)
	{
		if (source._buffers == null)
		{
			return;
		}
		foreach (var (nameID, value) in source._buffers)
		{
			SetBuffer(nameID, value);
		}
	}

	public Material(Shader shader)
		: this(shader, allowDeferral: true)
	{
	}

	private Material(Shader shader, bool allowDeferral)
	{
		if (allowDeferral && shader != null && shader.WaitingForEngineRegistry)
		{
			_engineMaterial = Waylith.Rendering.Material.CreateUnseeded();
			_deferredResolver = () => new Material(shader, allowDeferral: false);
			RegisterDeferred();
			return;
		}
		_shader = shader;
		_engineMaterial = new Waylith.Rendering.Material(shader?.EngineShader);
		if (!TryGetAuthoredColor(shader?.EngineShader, out var _))
		{
			_engineMaterial.SetColor(ColorPropertyId, ToNumerics(Color.white));
		}
	}

	private Material(Func<Material> resolver)
	{
		_engineMaterial = Waylith.Rendering.Material.CreateUnseeded();
		_deferredResolver = resolver;
		RegisterDeferred();
	}

	public static Material CreateDeferred(Func<Material> resolver)
	{
		ArgumentNullException.ThrowIfNull(resolver, "resolver");
		return new Material(resolver);
	}

	public static void ResolveAllDeferred()
	{
		Material[] array;
		lock (DeferredResolveLock)
		{
			if (PendingDeferredMaterials.Count == 0)
			{
				return;
			}
			array = PendingDeferredMaterials.ToArray();
			PendingDeferredMaterials.Clear();
		}
		Material[] array2 = array;
		foreach (Material material in array2)
		{
			try
			{
				material.EnsureResolved();
			}
			catch (Exception value)
			{
				Debug.LogError($"Failed to warm up deferred material '{material.name}': {value}");
			}
		}
	}

	private void RegisterDeferred()
	{
		lock (DeferredResolveLock)
		{
			PendingDeferredMaterials.Add(this);
		}
	}

	public void DisableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			_keywords.Remove(keyword);
			_engineMaterial.DisableKeyword(keyword);
		}
	}

	public void EnableKeyword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			_keywords.Add(keyword);
			_engineMaterial.EnableKeyword(keyword);
		}
	}

	internal Waylith.Rendering.Material PassMaterial(int pass)
	{
		if (pass == 0)
		{
			return engineMaterial;
		}
		Waylith.Rendering.Material[] extraPassMaterials = ExtraPassMaterials;
		if (pass <= 0 || pass > extraPassMaterials.Length)
		{
			return null;
		}
		return extraPassMaterials[pass - 1];
	}

	public Color GetColor(string name)
	{
		return GetColor(Shader.PropertyToID(name));
	}

	public float GetFloat(string name)
	{
		return GetFloat(Shader.PropertyToID(name));
	}

	public float GetFloat(int nameID)
	{
		EnsureResolved();
		if (!_engineMaterial.TryGetFloat(nameID, out var value))
		{
			return 0f;
		}
		return value;
	}

	public int GetInt(int nameID)
	{
		EnsureResolved();
		if (!_engineMaterial.TryGetInt(nameID, out var value))
		{
			return 0;
		}
		return value;
	}

	public Matrix4x4 GetMatrix(int nameID)
	{
		EnsureResolved();
		if (!_engineMaterial.TryGetMatrix(nameID, out var value))
		{
			return Matrix4x4.identity;
		}
		return Matrix4x4.FromNumerics(value);
	}

	public Texture GetTexture(int nameID)
	{
		EnsureResolved();
		if (!_textures.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return value;
	}

	public Texture GetTexture(string name)
	{
		return GetTexture(Shader.PropertyToID(name));
	}

	public Vector2 GetTextureOffset(int nameID)
	{
		EnsureResolved();
		return ToUnityVector(_engineMaterial.GetTextureOffset(nameID));
	}

	public Vector2 GetTextureScale(int nameID)
	{
		EnsureResolved();
		return ToUnityVector(_engineMaterial.GetTextureScale(nameID));
	}

	public Vector4 GetVector(string name)
	{
		return GetVector(Shader.PropertyToID(name));
	}

	public Vector4 GetVector(int nameID)
	{
		EnsureResolved();
		if (!TryGetVectorOrColor(nameID, out var value))
		{
			return Vector4.zero;
		}
		return ToUnityVector(TextureST.Reorder(nameID, value));
	}

	public bool HasProperty(int nameID)
	{
		if (shader != null)
		{
			return shader.HasProperty(nameID);
		}
		return false;
	}

	public bool HasProperty(string name)
	{
		return HasProperty(Shader.PropertyToID(name));
	}

	public Vector2 GetTextureOffset(string name)
	{
		return GetTextureOffset(Shader.PropertyToID(name));
	}

	public Vector2 GetTextureScale(string name)
	{
		return GetTextureScale(Shader.PropertyToID(name));
	}

	public void SetTextureOffset(string name, Vector2 value)
	{
		SetTextureOffset(Shader.PropertyToID(name), value);
	}

	public void SetTextureScale(string name, Vector2 value)
	{
		SetTextureScale(Shader.PropertyToID(name), value);
	}

	public void CopyPropertiesFromMaterial(Material mat)
	{
		ArgumentNullException.ThrowIfNull(mat, "mat");
		EnsureResolved();
		mat.EnsureResolved();
		_engineMaterial.CopyPropertiesFrom(mat._engineMaterial);
		foreach (var (key, value) in mat._textures)
		{
			_textures[key] = value;
		}
		foreach (string keyword in mat._keywords)
		{
			EnableKeyword(keyword);
		}
		InheritDeclaredWraps(mat);
		if ((object)mat._shader != _shader)
		{
			InheritDeclaredWraps(mat._shader);
		}
		ApplyInheritedWraps();
	}

	public bool HasTexture(int nameID)
	{
		EnsureResolved();
		if (_textures.TryGetValue(nameID, out var value))
		{
			return value != null;
		}
		return false;
	}

	public void SetColor(int nameID, Color value)
	{
		if (!TrySetTextureST(nameID, ToNumerics(value)))
		{
			_engineMaterial.SetColor(nameID, ToNumerics(value));
		}
	}

	public void SetColor(string name, Color value)
	{
		SetColor(Shader.PropertyToID(name), value);
	}

	public void SetFloat(string name, float value)
	{
		SetFloat(Shader.PropertyToID(name), value);
	}

	public void SetBuffer(string name, ComputeBuffer value)
	{
		SetBuffer(Shader.PropertyToID(name), value);
	}

	public void SetBuffer(int nameID, ComputeBuffer value)
	{
		if (_buffers == null)
		{
			_buffers = new Dictionary<int, ComputeBuffer>();
		}
		if (_buffers.TryGetValue(nameID, out var value2))
		{
			if (value2 == value)
			{
				return;
			}
			value2.Unbind(this, nameID);
		}
		if (value == null)
		{
			_buffers.Remove(nameID);
			_engineMaterial.SetStorageBuffer(nameID, default(ReadOnlySpan<byte>));
		}
		else
		{
			_buffers[nameID] = value;
			value.Bind(this, nameID);
		}
	}

	internal void UploadBuffer(int nameID, ReadOnlySpan<byte> bytes)
	{
		_engineMaterial.SetStorageBuffer(nameID, bytes);
	}

	public void SetFloatArray(string name, float[] values)
	{
		SetFloatArray(Shader.PropertyToID(name), values);
	}

	public void SetFloatArray(int nameID, float[] values)
	{
		_engineMaterial.SetFloatArray(nameID, ShaderArrays.Check(values));
	}

	public void SetFloatArray(string name, List<float> values)
	{
		SetFloatArray(Shader.PropertyToID(name), values);
	}

	public void SetFloatArray(int nameID, List<float> values)
	{
		_engineMaterial.SetFloatArray(nameID, ShaderArrays.Check(values));
	}

	public void SetVectorArray(string name, Vector4[] values)
	{
		SetVectorArray(Shader.PropertyToID(name), values);
	}

	public void SetVectorArray(int nameID, Vector4[] values)
	{
		_engineMaterial.SetVectorArray(nameID, MemoryMarshal.Cast<Vector4, System.Numerics.Vector4>(ShaderArrays.Check(values)));
	}

	public void SetVectorArray(string name, List<Vector4> values)
	{
		SetVectorArray(Shader.PropertyToID(name), values);
	}

	public void SetVectorArray(int nameID, List<Vector4> values)
	{
		_engineMaterial.SetVectorArray(nameID, MemoryMarshal.Cast<Vector4, System.Numerics.Vector4>(ShaderArrays.Check(values)));
	}

	public void SetColorArray(string name, Color[] values)
	{
		SetColorArray(Shader.PropertyToID(name), values);
	}

	public void SetColorArray(int nameID, Color[] values)
	{
		_engineMaterial.SetVectorArray(nameID, MemoryMarshal.Cast<Color, System.Numerics.Vector4>(ShaderArrays.Check(values)));
	}

	public void SetColorArray(string name, List<Color> values)
	{
		SetColorArray(Shader.PropertyToID(name), values);
	}

	public void SetColorArray(int nameID, List<Color> values)
	{
		_engineMaterial.SetVectorArray(nameID, MemoryMarshal.Cast<Color, System.Numerics.Vector4>(ShaderArrays.Check(values)));
	}

	public void SetMatrixArray(string name, Matrix4x4[] values)
	{
		SetMatrixArray(Shader.PropertyToID(name), values);
	}

	public void SetMatrixArray(int nameID, Matrix4x4[] values)
	{
		_engineMaterial.SetMatrixArray(nameID, MemoryMarshal.Cast<Matrix4x4, System.Numerics.Matrix4x4>(ShaderArrays.Check(values)));
	}

	public void SetMatrixArray(string name, List<Matrix4x4> values)
	{
		SetMatrixArray(Shader.PropertyToID(name), values);
	}

	public void SetMatrixArray(int nameID, List<Matrix4x4> values)
	{
		_engineMaterial.SetMatrixArray(nameID, MemoryMarshal.Cast<Matrix4x4, System.Numerics.Matrix4x4>(ShaderArrays.Check(values)));
	}

	public float[] GetFloatArray(string name)
	{
		return GetFloatArray(Shader.PropertyToID(name));
	}

	public float[] GetFloatArray(int nameID)
	{
		EnsureResolved();
		return _engineMaterial.GetFloatArray(nameID);
	}

	public void GetFloatArray(string name, List<float> values)
	{
		GetFloatArray(Shader.PropertyToID(name), values);
	}

	public void GetFloatArray(int nameID, List<float> values)
	{
		ShaderArrays.Fill(values, GetFloatArray(nameID));
	}

	public Vector4[] GetVectorArray(string name)
	{
		return GetVectorArray(Shader.PropertyToID(name));
	}

	public Vector4[] GetVectorArray(int nameID)
	{
		return ConvertArray<System.Numerics.Vector4, Vector4>(GetEngineVectorArray(nameID));
	}

	public void GetVectorArray(string name, List<Vector4> values)
	{
		GetVectorArray(Shader.PropertyToID(name), values);
	}

	public void GetVectorArray(int nameID, List<Vector4> values)
	{
		ShaderArrays.Fill(values, GetVectorArray(nameID));
	}

	public Color[] GetColorArray(string name)
	{
		return GetColorArray(Shader.PropertyToID(name));
	}

	public Color[] GetColorArray(int nameID)
	{
		return ConvertArray<System.Numerics.Vector4, Color>(GetEngineVectorArray(nameID));
	}

	public void GetColorArray(string name, List<Color> values)
	{
		GetColorArray(Shader.PropertyToID(name), values);
	}

	public void GetColorArray(int nameID, List<Color> values)
	{
		ShaderArrays.Fill(values, GetColorArray(nameID));
	}

	public Matrix4x4[] GetMatrixArray(string name)
	{
		return GetMatrixArray(Shader.PropertyToID(name));
	}

	public Matrix4x4[] GetMatrixArray(int nameID)
	{
		EnsureResolved();
		return ConvertArray<System.Numerics.Matrix4x4, Matrix4x4>(_engineMaterial.GetMatrixArray(nameID));
	}

	public void GetMatrixArray(string name, List<Matrix4x4> values)
	{
		GetMatrixArray(Shader.PropertyToID(name), values);
	}

	public void GetMatrixArray(int nameID, List<Matrix4x4> values)
	{
		ShaderArrays.Fill(values, GetMatrixArray(nameID));
	}

	public void SetFloat(int nameID, float value)
	{
		_engineMaterial.SetFloat(nameID, value);
	}

	public void SetInt(string name, int value)
	{
		SetInt(Shader.PropertyToID(name), value);
	}

	public void SetInt(int nameID, int value)
	{
		_engineMaterial.SetInt(nameID, value);
	}

	public void SetInteger(string name, int value)
	{
		SetInt(name, value);
	}

	public void SetInteger(int nameID, int value)
	{
		SetInt(nameID, value);
	}

	public void SetKeyword(in LocalKeyword keyword, bool value)
	{
		if (keyword.isValid)
		{
			if (value)
			{
				EnableKeyword(keyword.Name);
			}
			else
			{
				DisableKeyword(keyword.Name);
			}
		}
	}

	public void SetOverrideTag(string tag, string val)
	{
		_overrideTags[tag] = val;
	}

	public bool SetPass(int pass)
	{
		if (pass != 0)
		{
			return false;
		}
		_immediatePassMaterial = engineMaterial;
		_immediatePassSource = this;
		return true;
	}

	public void SetTexture(int nameID, Texture value)
	{
		Texture.EnsureSampledTextureReady(value);
		_textures[nameID] = value;
		_engineMaterial.SetTexture(nameID, value?.CoreTexture);
		ApplyInheritedWrap(nameID, value);
	}

	public void SetTexture(string name, Texture value)
	{
		SetTexture(Shader.PropertyToID(name), value);
	}

	public void SetTexture(int nameID, RenderTexture value, RenderTextureSubElement element)
	{
		if ((uint)(element - 1) <= 1u)
		{
			StubWarner.LogStub("SetTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Material.cs", 463);
		}
		SetTexture(nameID, value);
	}

	public void SetTexture(string name, RenderTexture value, RenderTextureSubElement element)
	{
		SetTexture(Shader.PropertyToID(name), value, element);
	}

	public void SetTextureOffset(int nameID, Vector2 value)
	{
		_engineMaterial.SetTextureOffset(nameID, ToNumerics(value));
	}

	public void SetTextureScale(int nameID, Vector2 value)
	{
		_engineMaterial.SetTextureScale(nameID, ToNumerics(value));
	}

	public void SetMatrix(int nameID, Matrix4x4 value)
	{
		_engineMaterial.SetMatrix(nameID, Matrix4x4.ToNumerics(value));
	}

	public void SetVector(int nameID, Vector4 value)
	{
		if (!TrySetTextureST(nameID, ToNumerics(value)))
		{
			_engineMaterial.SetVector(nameID, ToNumerics(value));
		}
	}

	public void SetVector(string name, Vector4 value)
	{
		SetVector(Shader.PropertyToID(name), value);
	}

	internal bool TryGetColor(int nameID, out Color value)
	{
		if (TextureST.Is(nameID))
		{
			Vector4 value2;
			bool result = TryGetVector(nameID, out value2);
			value = value2;
			return result;
		}
		EnsureResolved();
		if (TryGetVectorOrColor(nameID, out var value3))
		{
			value = ToUnityColor(value3);
			return true;
		}
		value = default(Color);
		return false;
	}

	public bool TryGetFloat(int nameID, out float value)
	{
		EnsureResolved();
		return _engineMaterial.TryGetFloat(nameID, out value);
	}

	internal bool TryGetInt(int nameID, out int value)
	{
		EnsureResolved();
		return _engineMaterial.TryGetInt(nameID, out value);
	}

	internal bool TryGetMatrix(int nameID, out Matrix4x4 value)
	{
		EnsureResolved();
		if (_engineMaterial.TryGetMatrix(nameID, out var value2))
		{
			value = Matrix4x4.FromNumerics(value2);
			return true;
		}
		value = default(Matrix4x4);
		return false;
	}

	public bool TryGetTexture(int nameID, out Texture value)
	{
		EnsureResolved();
		return _textures.TryGetValue(nameID, out value);
	}

	internal bool TryGetVector(int nameID, out Vector4 value)
	{
		EnsureResolved();
		if (TryGetVectorOrColor(nameID, out var value2))
		{
			value = ToUnityVector(TextureST.Reorder(nameID, value2));
			return true;
		}
		value = default(Vector4);
		return false;
	}

	internal bool HasKeyword(string keyword)
	{
		EnsureResolved();
		return _keywords.Contains(keyword);
	}

	public bool IsKeywordEnabled(string keyword)
	{
		EnsureResolved();
		return _keywords.Contains(keyword);
	}

	public bool IsKeywordEnabled(in LocalKeyword keyword)
	{
		EnsureResolved();
		return _keywords.Contains(keyword.Name);
	}

	internal string GetOverrideTag(string tag)
	{
		EnsureResolved();
		return _overrideTags.GetValueOrDefault(tag, string.Empty);
	}

	protected override void OnNameChanged(string newName)
	{
		_engineMaterial.DebugName = newName;
	}

	private void SyncEngineShader()
	{
		_engineMaterial.Shader = _shader?.EngineShader ?? ShaderRegistry.Default;
	}

	private void EnsureResolved()
	{
		if (_deferredResolver == null)
		{
			return;
		}
		lock (DeferredResolveLock)
		{
			if (_deferredResolver == null || _resolving)
			{
				return;
			}
			_resolving = true;
			try
			{
				Material material = _deferredResolver();
				if (material == null)
				{
					throw new InvalidOperationException("Deferred Material '" + base.name + "' was read/bound before its MaterialDef (or clone source) was available. This almost always means a [StaticConstructorOnStartup] read a material's properties/shader before PlayDataLoader loaded defs. Defer the read to render time, or load the material in an explicit post-def step.");
				}
				material.EnsureResolved();
				MergeFrom(material);
				_deferredResolver = null;
			}
			finally
			{
				_resolving = false;
			}
		}
	}

	private void MergeFrom(Material baseMaterial)
	{
		Waylith.Rendering.Material material = baseMaterial._engineMaterial;
		if (_shader == null)
		{
			_shader = baseMaterial._shader;
			_engineMaterial.Shader = material.Shader;
		}
		MergePropertiesFrom(material);
		foreach (var (key, value) in baseMaterial._textures)
		{
			_textures.TryAdd(key, value);
		}
		foreach (string keyword in baseMaterial._keywords)
		{
			if (_keywords.Add(keyword))
			{
				_engineMaterial.EnableKeyword(keyword);
			}
		}
		foreach (var (key2, value2) in baseMaterial._overrideTags)
		{
			_overrideTags.TryAdd(key2, value2);
		}
		if (!_renderQueueOverridden)
		{
			_engineMaterial.RenderQueue = material.RenderQueue;
		}
		InheritDeclaredWraps(baseMaterial);
		if ((object)baseMaterial._shader != _shader)
		{
			InheritDeclaredWraps(baseMaterial._shader);
		}
		ApplyInheritedWraps();
	}

	private void InheritDeclaredWraps(Shader outgoing)
	{
		if (outgoing == null || outgoing.WaitingForEngineRegistry)
		{
			return;
		}
		Dictionary<string, Waylith.Rendering.TextureWrapMode> dictionary = outgoing.EngineShader?.Program.textureWrapOverrides;
		if (dictionary == null || dictionary.Count == 0)
		{
			return;
		}
		if (_inheritedWraps == null)
		{
			_inheritedWraps = new Dictionary<int, TextureWrapMode>();
		}
		foreach (var (text2, value) in dictionary)
		{
			_inheritedWraps.TryAdd(Shader.PropertyToID(text2), (TextureWrapMode)value);
		}
	}

	private void InheritDeclaredWraps(Material source)
	{
		if (source._inheritedWraps == null)
		{
			return;
		}
		if (_inheritedWraps == null)
		{
			_inheritedWraps = new Dictionary<int, TextureWrapMode>();
		}
		foreach (var (key, value) in source._inheritedWraps)
		{
			_inheritedWraps.TryAdd(key, value);
		}
	}

	private void ApplyInheritedWraps()
	{
		if (_inheritedWraps == null)
		{
			return;
		}
		foreach (var (nameID, texture2) in _textures)
		{
			ApplyInheritedWrap(nameID, texture2);
		}
	}

	private void ApplyInheritedWrap(int nameID, Texture texture)
	{
		if (_inheritedWraps != null && _inheritedWraps.TryGetValue(nameID, out var value) && texture?.CoreTexture is Waylith.Rendering.Texture2D texture2D)
		{
			string sourcePath = texture2D.SourcePath;
			if (sourcePath != null && !CurrentShaderDeclaresWrap(nameID) && ImportDefaults.IsOfficial(sourcePath))
			{
				texture.wrapMode = value;
			}
		}
	}

	private bool CurrentShaderDeclaresWrap(int nameID)
	{
		if (_shader == null || _shader.WaitingForEngineRegistry)
		{
			return false;
		}
		Dictionary<string, Waylith.Rendering.TextureWrapMode> dictionary = _shader.EngineShader?.Program.textureWrapOverrides;
		if (dictionary != null && ShaderPropertyId.TryGetName(nameID, out string key))
		{
			return dictionary.ContainsKey(key);
		}
		return false;
	}

	private void MergePropertiesFrom(Waylith.Rendering.Material source)
	{
		HashSet<int> hashSet = new HashSet<int>();
		ReadOnlyMemory<DrawProperty> properties = _engineMaterial.Properties;
		ReadOnlySpan<DrawProperty> span = properties.Span;
		int i;
		for (i = 0; i < span.Length; i++)
		{
			DrawProperty drawProperty = span[i];
			hashSet.Add(drawProperty.PropertyId);
		}
		properties = source.Properties;
		ReadOnlySpan<DrawProperty> span2 = properties.Span;
		for (i = 0; i < span2.Length; i++)
		{
			DrawProperty property = span2[i];
			if (!hashSet.Contains(property.PropertyId))
			{
				CopyProperty(property);
			}
		}
		HashSet<int> hashSet2 = new HashSet<int>();
		MaterialTextureTransform value;
		foreach (KeyValuePair<int, MaterialTextureTransform> textureTransform in _engineMaterial.TextureTransforms)
		{
			textureTransform.Deconstruct(out i, out value);
			int item = i;
			hashSet2.Add(item);
		}
		foreach (KeyValuePair<int, MaterialTextureTransform> textureTransform2 in source.TextureTransforms)
		{
			textureTransform2.Deconstruct(out i, out value);
			int num = i;
			MaterialTextureTransform transform = value;
			if (!hashSet2.Contains(num))
			{
				_engineMaterial.SetTextureTransform(num, transform);
			}
		}
		_engineMaterial.MergeUnsetArraysFrom(source);
	}

	public Color GetColor(int nameID)
	{
		if (TextureST.Is(nameID))
		{
			return GetVector(nameID);
		}
		EnsureResolved();
		if (!TryGetVectorOrColor(nameID, out var value))
		{
			if (nameID != ColorPropertyId)
			{
				return default(Color);
			}
			if (!TryGetAuthoredColor(_engineMaterial.Shader, out var value2))
			{
				return Color.white;
			}
			return ToUnityColor(value2);
		}
		return ToUnityColor(value);
	}

	private static bool TryGetAuthoredColor(Waylith.Rendering.Shader shader, out System.Numerics.Vector4 value)
	{
		MaterialUniformSlot[] array = shader?.MaterialLayout?.UniformSlots ?? Array.Empty<MaterialUniformSlot>();
		for (int i = 0; i < array.Length; i++)
		{
			MaterialUniformSlot materialUniformSlot = array[i];
			if (materialUniformSlot.PropertyId == ColorPropertyId && materialUniformSlot.HasAuthoredDefault)
			{
				value = materialUniformSlot.DefaultValue.Vector4;
				return true;
			}
		}
		value = default(System.Numerics.Vector4);
		return false;
	}

	private bool TryGetVectorOrColor(int nameID, out System.Numerics.Vector4 value)
	{
		if (!_engineMaterial.TryGetVector(nameID, out value))
		{
			return _engineMaterial.TryGetColor(nameID, out value);
		}
		return true;
	}

	private void CopyProperty(DrawProperty property)
	{
		switch (property.Type)
		{
		case DrawPropertyType.Float:
			_engineMaterial.SetFloat(property.PropertyId, property.FloatValue);
			break;
		case DrawPropertyType.Int:
			_engineMaterial.SetInt(property.PropertyId, property.IntValue);
			break;
		case DrawPropertyType.Vector:
			_engineMaterial.SetVector(property.PropertyId, property.VectorValue);
			break;
		case DrawPropertyType.Color:
			_engineMaterial.SetColor(property.PropertyId, property.VectorValue);
			break;
		case DrawPropertyType.Matrix:
			_engineMaterial.SetMatrix(property.PropertyId, property.MatrixValue);
			break;
		case DrawPropertyType.Texture:
			_engineMaterial.SetTexture(property.PropertyId, property.ReferenceValue);
			break;
		}
	}

	private void CopyPropertiesFrom(Waylith.Rendering.Material source)
	{
		ReadOnlySpan<DrawProperty> span = source.Properties.Span;
		int i;
		for (i = 0; i < span.Length; i++)
		{
			DrawProperty property = span[i];
			CopyProperty(property);
		}
		foreach (KeyValuePair<int, MaterialTextureTransform> textureTransform in source.TextureTransforms)
		{
			textureTransform.Deconstruct(out i, out var value);
			int propertyId = i;
			MaterialTextureTransform transform = value;
			_engineMaterial.SetTextureTransform(propertyId, transform);
		}
	}

	private bool TrySetTextureST(int nameID, System.Numerics.Vector4 value)
	{
		if (!TextureST.TryGetTextureId(nameID, out var textureId))
		{
			return false;
		}
		_engineMaterial.SetTextureTransform(textureId, new MaterialTextureTransform(new System.Numerics.Vector2(value.Z, value.W), new System.Numerics.Vector2(value.X, value.Y)));
		return true;
	}

	private System.Numerics.Vector4[] GetEngineVectorArray(int nameID)
	{
		EnsureResolved();
		return _engineMaterial.GetVectorArray(nameID);
	}

	private static TTo[] ConvertArray<TFrom, TTo>(TFrom[] values) where TFrom : struct where TTo : struct
	{
		if (values != null)
		{
			return MemoryMarshal.Cast<TFrom, TTo>((ReadOnlySpan<TFrom>)values).ToArray();
		}
		return null;
	}

	private static System.Numerics.Vector2 ToNumerics(Vector2 value)
	{
		return Vector2.ToNumerics(value);
	}

	private static System.Numerics.Vector4 ToNumerics(Color value)
	{
		return Color.ToNumerics(value);
	}

	private static System.Numerics.Vector4 ToNumerics(Vector4 value)
	{
		return Vector4.ToNumerics(value);
	}

	private static Color ToUnityColor(System.Numerics.Vector4 value)
	{
		return new Color(value.X, value.Y, value.Z, value.W);
	}

	private static Vector4 ToUnityVector(System.Numerics.Vector4 value)
	{
		return Vector4.FromNumerics(value);
	}

	private static Vector2 ToUnityVector(System.Numerics.Vector2 value)
	{
		return Vector2.FromNumerics(value);
	}
}
