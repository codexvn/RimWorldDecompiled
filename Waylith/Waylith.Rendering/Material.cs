using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Waylith.Math;

namespace Waylith.Rendering;

public sealed class Material : IDisposable
{
	private sealed class KeywordState(string[] keywords, string key)
	{
		public static KeywordState Empty { get; } = new KeywordState(Array.Empty<string>(), string.Empty);

		public string[] Keywords { get; } = keywords;

		public string Key { get; } = key;
	}

	private sealed class BlockImage
	{
		public const int RingSize = 4;

		public readonly byte[][] Ring = CreateRing(size);

		public int Current = -1;

		public int Generation;

		public BlockImage(int size)
		{
		}

		private static byte[][] CreateRing(int size)
		{
			byte[][] array = new byte[4][];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new byte[size];
			}
			return array;
		}
	}

	private sealed class ComposedStorage(MaterialLayout layout)
	{
		public readonly MaterialLayout Layout = layout;

		public readonly BlockImage[] Blocks = Array.ConvertAll(layout.UniformBlocks, (MaterialUniformBlockSlot block) => new BlockImage(block.Size));

		public MaterialStorageArray[] Own = Array.Empty<MaterialStorageArray>();

		public MaterialStorageArray[] Result = Array.Empty<MaterialStorageArray>();

		public int MaterialVersion = -1;

		public long GlobalVersion = -1L;

		public readonly List<MaterialStorageArray> Scratch = new List<MaterialStorageArray>();
	}

	private sealed class MaterialArray<T>(T[] values, int generation)
	{
		public T[] Values { get; } = values;

		public int Generation { get; set; } = generation;
	}

	private static int NextId;

	private int disposed;

	private readonly object syncRoot = new object();

	private readonly Dictionary<int, DrawProperty> properties = new Dictionary<int, DrawProperty>();

	private readonly Dictionary<int, MaterialTextureTransform> textureTransforms = new Dictionary<int, MaterialTextureTransform>();

	private readonly Dictionary<int, MaterialArray<float>> floatArrays = new Dictionary<int, MaterialArray<float>>();

	private readonly Dictionary<int, MaterialArray<Vector4>> vectorArrays = new Dictionary<int, MaterialArray<Vector4>>();

	private readonly Dictionary<int, MaterialArray<Matrix4x4>> matrixArrays = new Dictionary<int, MaterialArray<Matrix4x4>>();

	private readonly Dictionary<int, MaterialArray<byte>> storageBuffers = new Dictionary<int, MaterialArray<byte>>();

	private int arrayGeneration;

	private int storageArraysGeneration;

	private MaterialStorageArray[] storageArrays = Array.Empty<MaterialStorageArray>();

	private ComposedStorage? composedStorage;

	private readonly HashSet<string> keywords = new HashSet<string>(StringComparer.Ordinal);

	private DrawProperty[] propertySnapshot = Array.Empty<DrawProperty>();

	private volatile bool propertySnapshotDirty;

	private KeywordState keywordState = KeywordState.Empty;

	private Shader shader;

	private int renderQueue = -1;

	private string debugName;

	private int version;

	private static readonly Lock DeferredResolveLock = new Lock();

	private static readonly List<Material> PendingDeferredMaterials = new List<Material>();

	private volatile Func<Material>? deferredResolver;

	private bool resolvingDeferred;

	private bool shaderSet;

	private TextureWrapMode? wrapModeOverride;

	private static readonly int ColorPropertyId = ShaderPropertyId.GetOrCreate("_Color");

	private static readonly int MainTexPropertyId = ShaderPropertyId.GetOrCreate("_MainTex");

	public bool IsDeferredMaterial => deferredResolver != null;

	public int Id { get; }

	public int Version => Volatile.Read(in version);

	public string DebugName
	{
		get
		{
			return Volatile.Read(in debugName);
		}
		set
		{
			lock (syncRoot)
			{
				string text = (string.IsNullOrWhiteSpace(value) ? $"Material#{Id}" : value);
				if (!string.Equals(debugName, text, StringComparison.Ordinal))
				{
					Volatile.Write(ref debugName, text);
				}
			}
		}
	}

	public Shader Shader
	{
		get
		{
			EnsureResolved();
			return Volatile.Read(in shader);
		}
		set
		{
			lock (syncRoot)
			{
				shaderSet = true;
				if (shader != value)
				{
					Volatile.Write(ref shader, value ?? ShaderRegistry.Default);
					IncrementVersionNoLock();
				}
			}
		}
	}

	public int RenderQueue
	{
		get
		{
			EnsureResolved();
			int num = Volatile.Read(in renderQueue);
			if (num < 0)
			{
				return Shader.DefaultRenderQueue;
			}
			return num;
		}
		set
		{
			lock (syncRoot)
			{
				if (renderQueue != value)
				{
					Volatile.Write(ref renderQueue, value);
					IncrementVersionNoLock();
				}
			}
		}
	}

	public TextureWrapMode? WrapModeOverride
	{
		get
		{
			return wrapModeOverride;
		}
		set
		{
			lock (syncRoot)
			{
				if (wrapModeOverride != value)
				{
					wrapModeOverride = value;
					IncrementVersionNoLock();
				}
			}
		}
	}

	public ReadOnlyMemory<DrawProperty> Properties => PropertiesForReading;

	public DrawProperty[] PropertiesForReading
	{
		get
		{
			EnsureResolved();
			if (propertySnapshotDirty)
			{
				lock (syncRoot)
				{
					if (propertySnapshotDirty)
					{
						PublishPropertySnapshotNoLock();
					}
				}
			}
			return Volatile.Read(in propertySnapshot);
		}
	}

	public IReadOnlyDictionary<int, MaterialTextureTransform> TextureTransforms
	{
		get
		{
			EnsureResolved();
			lock (syncRoot)
			{
				return new Dictionary<int, MaterialTextureTransform>(textureTransforms);
			}
		}
	}

	public ReadOnlySpan<string> Keywords
	{
		get
		{
			EnsureResolved();
			return Volatile.Read(in keywordState).Keywords;
		}
	}

	public string KeywordKey
	{
		get
		{
			EnsureResolved();
			return Volatile.Read(in keywordState).Key;
		}
	}

	public long KeywordKeyId
	{
		get
		{
			EnsureResolved();
			return ShaderKeywordIdMaker.GetOrCreateId(Volatile.Read(in keywordState).Key);
		}
	}

	public Color Color
	{
		get
		{
			return GetColor(ColorPropertyId);
		}
		set
		{
			SetColor(ColorPropertyId, value);
		}
	}

	public Texture? MainTexture
	{
		get
		{
			return GetTexture(MainTexPropertyId);
		}
		set
		{
			SetTexture(MainTexPropertyId, value);
		}
	}

	public Vector2 MainTextureOffset
	{
		get
		{
			return GetTextureOffset(MainTexPropertyId);
		}
		set
		{
			SetTextureOffset(MainTexPropertyId, value);
		}
	}

	public Vector2 MainTextureScale
	{
		get
		{
			return GetTextureScale(MainTexPropertyId);
		}
		set
		{
			SetTextureScale(MainTexPropertyId, value);
		}
	}

	public MaterialStorageArray[] StorageArrays
	{
		get
		{
			EnsureResolved();
			MaterialLayout materialLayout = shader.MaterialLayout;
			if (materialLayout.UniformBlocks.Length != 0 || (materialLayout.HasStorageBuffers && GlobalShaderState.HasArrays))
			{
				return ComposeStorageArrays(materialLayout);
			}
			return OwnStorageArrays();
		}
	}

	private static event Action<int>? Released;

	internal static void RegisterReleaseListener(MaterialReleaseListener listener)
	{
		Released += listener.Enqueue;
	}

	internal static void UnregisterReleaseListener(MaterialReleaseListener listener)
	{
		Released -= listener.Enqueue;
	}

	public Material(Shader? shader = null, string? debugName = null)
		: this(shader, debugName, seedDefaults: true)
	{
	}

	private Material(Shader? shader, string? debugName, bool seedDefaults)
	{
		Id = Interlocked.Increment(ref NextId);
		this.shader = shader ?? ShaderRegistry.Default;
		shaderSet = shader != null;
		this.debugName = debugName ?? $"Material#{Id}";
		if (seedDefaults)
		{
			SetColor(ColorPropertyId, Color.white);
		}
	}

	public static Material CreateUnseeded(string? debugName = null)
	{
		return new Material(null, debugName, seedDefaults: false);
	}

	public Material(Material source)
		: this(null, null, seedDefaults: false)
	{
		ArgumentNullException.ThrowIfNull(source, "source");
		if (source.deferredResolver != null)
		{
			deferredResolver = () => source;
			RegisterDeferred();
			return;
		}
		Volatile.Write(ref shader, source.Shader);
		shaderSet = source.shaderSet;
		DebugName = source.DebugName;
		Volatile.Write(ref renderQueue, source.renderQueue);
		WrapModeOverride = source.WrapModeOverride;
		CopyPropertiesFrom(source);
		ReadOnlySpan<string> readOnlySpan = source.Keywords;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			string keyword = readOnlySpan[i];
			EnableKeyword(keyword);
		}
	}

	public static Material CreateDeferred(Func<Material> resolver)
	{
		ArgumentNullException.ThrowIfNull(resolver, "resolver");
		Material material = new Material(null, null, seedDefaults: false);
		material.deferredResolver = resolver;
		material.RegisterDeferred();
		return material;
	}

	public static void ResolveAllDeferred(Action<Material, Exception>? onError = null)
	{
		Material[] array;
		using (DeferredResolveLock.EnterScope())
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
			catch (Exception arg)
			{
				onError?.Invoke(material, arg);
			}
		}
	}

	private void RegisterDeferred()
	{
		using (DeferredResolveLock.EnterScope())
		{
			PendingDeferredMaterials.Add(this);
		}
	}

	private void EnsureResolved()
	{
		if (deferredResolver != null)
		{
			ResolveDeferredSlow();
		}
	}

	private void ResolveDeferredSlow()
	{
		using (DeferredResolveLock.EnterScope())
		{
			if (deferredResolver == null || resolvingDeferred)
			{
				return;
			}
			resolvingDeferred = true;
			try
			{
				Material material = deferredResolver();
				if (material == null)
				{
					throw new InvalidOperationException("Deferred Material '" + DebugName + "' was read/bound before its MaterialDef (or clone source) was available. This almost always means a [StaticConstructorOnStartup] read a material's properties/shader before defs were loaded. Defer the read to render time, or load the material in an explicit post-def step.");
				}
				material.EnsureResolved();
				MergeFromDeferredBase(material);
				deferredResolver = null;
			}
			finally
			{
				resolvingDeferred = false;
			}
		}
	}

	private void MergeFromDeferredBase(Material baseMaterial)
	{
		if (!shaderSet)
		{
			Volatile.Write(ref shader, baseMaterial.Shader);
			shaderSet = baseMaterial.shaderSet;
		}
		MergeUnsetPropertiesFrom(baseMaterial);
		ReadOnlySpan<string> readOnlySpan = baseMaterial.Keywords;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			string keyword = readOnlySpan[i];
			EnableKeyword(keyword);
		}
		if (Volatile.Read(in renderQueue) < 0)
		{
			Volatile.Write(ref renderQueue, baseMaterial.renderQueue);
		}
		if (!WrapModeOverride.HasValue)
		{
			TextureWrapMode? textureWrapMode2 = (WrapModeOverride = baseMaterial.WrapModeOverride);
		}
	}

	public void SetFloat(int propertyId, float value)
	{
		SetProperty(DrawProperty.Float(propertyId, value));
	}

	public void SetInt(int propertyId, int value)
	{
		SetProperty(DrawProperty.Int(propertyId, value));
	}

	public void SetVector(int propertyId, Vector4 value)
	{
		SetProperty(DrawProperty.Vector(propertyId, value));
	}

	public void SetColor(int propertyId, Vector4 value)
	{
		SetProperty(DrawProperty.Color(propertyId, value));
	}

	public void SetMatrix(int propertyId, Matrix4x4 value)
	{
		SetProperty(DrawProperty.Matrix(propertyId, value));
	}

	public void SetTexture(int propertyId, object? value)
	{
		if (value is Texture texture)
		{
			Texture.EnsureSampledReady(texture);
		}
		SetProperty(DrawProperty.Texture(propertyId, value));
	}

	public Color GetColor(int propertyId)
	{
		if (TryGetColor(propertyId, out var value))
		{
			return value;
		}
		if (propertyId != ColorPropertyId)
		{
			return default(Color);
		}
		return Color.white;
	}

	public Color GetColor(string name)
	{
		return GetColor(ShaderPropertyId.GetOrCreate(name));
	}

	public float GetFloat(int propertyId)
	{
		if (!TryGetFloat(propertyId, out var value))
		{
			return 0f;
		}
		return value;
	}

	public float GetFloat(string name)
	{
		return GetFloat(ShaderPropertyId.GetOrCreate(name));
	}

	public int GetInt(int propertyId)
	{
		if (!TryGetInt(propertyId, out var value))
		{
			return 0;
		}
		return value;
	}

	public Vector4 GetVector(int propertyId)
	{
		if (!TryGetVector(propertyId, out var value))
		{
			return default(Vector4);
		}
		return value;
	}

	public Matrix4x4 GetMatrix(int propertyId)
	{
		if (!TryGetMatrix(propertyId, out var value))
		{
			return Matrix4x4.Identity;
		}
		return value;
	}

	public Texture? GetTexture(int propertyId)
	{
		if (!TryGetTexture<Texture>(propertyId, out Texture value))
		{
			return null;
		}
		return value;
	}

	public Texture? GetTexture(string name)
	{
		return GetTexture(ShaderPropertyId.GetOrCreate(name));
	}

	public bool HasTexture(int propertyId)
	{
		return GetTexture(propertyId) != null;
	}

	public void CollectSampledTextures(ISet<Texture> textures)
	{
		MaterialTextureSlot[] textureSlots = Shader.MaterialLayout.TextureSlots;
		for (int i = 0; i < textureSlots.Length; i++)
		{
			MaterialTextureSlot materialTextureSlot = textureSlots[i];
			if (materialTextureSlot.Kind != MaterialTextureKind.Sampler)
			{
				Texture texture = GetTexture(materialTextureSlot.PropertyId);
				if (texture != null)
				{
					textures.Add(texture);
				}
			}
		}
	}

	public bool HasProperty(int propertyId)
	{
		return Shader.HasProperty(propertyId);
	}

	public bool IsKeywordEnabled(string keyword)
	{
		return HasKeyword(keyword);
	}

	public void SetColor(int propertyId, Color value)
	{
		SetColor(propertyId, (Vector4)value);
	}

	public void SetColor(string name, Color value)
	{
		SetColor(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetFloat(string name, float value)
	{
		SetFloat(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetInt(string name, int value)
	{
		SetInt(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetVector(string name, Vector4 value)
	{
		SetVector(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetTexture(string name, object? value)
	{
		SetTexture(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void CopyPropertiesFrom(Material source)
	{
		DrawProperty[] propertiesForReading = source.PropertiesForReading;
		int i;
		for (i = 0; i < propertiesForReading.Length; i++)
		{
			DrawProperty property = propertiesForReading[i];
			SetProperty(in property);
		}
		foreach (KeyValuePair<int, MaterialTextureTransform> textureTransform in source.TextureTransforms)
		{
			textureTransform.Deconstruct(out i, out var value);
			int propertyId = i;
			MaterialTextureTransform transform = value;
			SetTextureTransform(propertyId, transform);
		}
		CopyArraysFrom(source);
	}

	public void MergeUnsetPropertiesFrom(Material source)
	{
		HashSet<int> hashSet = new HashSet<int>();
		DrawProperty[] propertiesForReading = PropertiesForReading;
		foreach (DrawProperty drawProperty in propertiesForReading)
		{
			hashSet.Add(drawProperty.PropertyId);
		}
		propertiesForReading = source.PropertiesForReading;
		int i;
		for (i = 0; i < propertiesForReading.Length; i++)
		{
			DrawProperty property = propertiesForReading[i];
			if (!hashSet.Contains(property.PropertyId))
			{
				SetProperty(in property);
			}
		}
		HashSet<int> hashSet2 = new HashSet<int>();
		MaterialTextureTransform value;
		foreach (KeyValuePair<int, MaterialTextureTransform> textureTransform in TextureTransforms)
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
				SetTextureTransform(num, transform);
			}
		}
		MergeUnsetArraysFrom(source);
	}

	public void CopyArraysFrom(Material source)
	{
		CopyArraysFrom(source, overwrite: true);
	}

	public void MergeUnsetArraysFrom(Material source)
	{
		CopyArraysFrom(source, overwrite: false);
	}

	private void CopyArraysFrom(Material source, bool overwrite)
	{
		if (source == this)
		{
			return;
		}
		EnsureResolved();
		source.EnsureResolved();
		Material obj = ((Id < source.Id) ? this : source);
		Material material = ((obj == this) ? source : this);
		lock (obj.syncRoot)
		{
			lock (material.syncRoot)
			{
				CopyArraysNoLock(source.floatArrays, floatArrays, overwrite);
				CopyArraysNoLock(source.vectorArrays, vectorArrays, overwrite);
				CopyArraysNoLock(source.matrixArrays, matrixArrays, overwrite);
				CopyArraysNoLock(source.storageBuffers, storageBuffers, overwrite);
			}
		}
	}

	private void CopyArraysNoLock<T>(Dictionary<int, MaterialArray<T>> source, Dictionary<int, MaterialArray<T>> destination, bool overwrite)
	{
		foreach (KeyValuePair<int, MaterialArray<T>> item in source)
		{
			item.Deconstruct(out var key, out var value);
			int key2 = key;
			MaterialArray<T> materialArray = value;
			if (overwrite || !destination.ContainsKey(key2))
			{
				T[] values = (T[])materialArray.Values.Clone();
				key = ++arrayGeneration;
				destination[key2] = new MaterialArray<T>(values, key);
			}
		}
	}

	public void SetFloatArray(int propertyId, ReadOnlySpan<float> values)
	{
		SetArray(floatArrays, propertyId, values);
	}

	public void SetVectorArray(int propertyId, ReadOnlySpan<Vector4> values)
	{
		SetArray(vectorArrays, propertyId, values);
	}

	public void SetMatrixArray(int propertyId, ReadOnlySpan<Matrix4x4> values)
	{
		SetArray(matrixArrays, propertyId, values);
	}

	public void SetStorageBuffer(int propertyId, ReadOnlySpan<byte> bytes)
	{
		lock (syncRoot)
		{
			MaterialArray<byte> value;
			if (bytes.IsEmpty)
			{
				if (storageBuffers.Remove(propertyId))
				{
					arrayGeneration++;
				}
			}
			else if (storageBuffers.TryGetValue(propertyId, out value) && value.Values.Length == bytes.Length)
			{
				bytes.CopyTo(value.Values);
				value.Generation = ++arrayGeneration;
			}
			else
			{
				storageBuffers[propertyId] = new MaterialArray<byte>(bytes.ToArray(), ++arrayGeneration);
			}
			IncrementVersionNoLock();
		}
	}

	public float[]? GetFloatArray(int propertyId)
	{
		return GetArray(floatArrays, propertyId);
	}

	public Vector4[]? GetVectorArray(int propertyId)
	{
		return GetArray(vectorArrays, propertyId);
	}

	public Matrix4x4[]? GetMatrixArray(int propertyId)
	{
		return GetArray(matrixArrays, propertyId);
	}

	private MaterialStorageArray[] OwnStorageArrays()
	{
		if (Volatile.Read(in storageArraysGeneration) == Volatile.Read(in arrayGeneration))
		{
			return Volatile.Read(in storageArrays);
		}
		lock (syncRoot)
		{
			if (storageArraysGeneration != arrayGeneration)
			{
				Volatile.Write(ref storageArrays, BuildStorageArraysNoLock());
				Volatile.Write(ref storageArraysGeneration, arrayGeneration);
			}
			return storageArrays;
		}
	}

	private MaterialStorageArray[] ComposeStorageArrays(MaterialLayout layout)
	{
		MaterialStorageArray[] array = OwnStorageArrays();
		lock (syncRoot)
		{
			ComposedStorage composedStorage = this.composedStorage;
			if (composedStorage == null || composedStorage.layout != layout)
			{
				composedStorage = (this.composedStorage = new ComposedStorage(layout));
			}
			long num = GlobalShaderState.Version;
			if (composedStorage.Own == array && composedStorage.MaterialVersion == version && composedStorage.GlobalVersion == num)
			{
				return composedStorage.Result;
			}
			List<MaterialStorageArray> scratch = composedStorage.Scratch;
			scratch.Clear();
			scratch.AddRange(array);
			for (int i = 0; i < layout.UniformBlocks.Length; i++)
			{
				MaterialUniformBlockSlot materialUniformBlockSlot = layout.UniformBlocks[i];
				BlockImage blockImage = composedStorage.Blocks[i];
				byte[] array2 = blockImage.Ring[(blockImage.Current + 1) % 4];
				MaterialBindingResolver.ResolveUniformBlock(this, materialUniformBlockSlot, array2);
				if (blockImage.Current < 0 || !((ReadOnlySpan<byte>)array2.AsSpan()).SequenceEqual((ReadOnlySpan<byte>)blockImage.Ring[blockImage.Current]))
				{
					blockImage.Current = (blockImage.Current + 1) % 4;
					blockImage.Generation++;
				}
				scratch.Add(new MaterialStorageArray(materialUniformBlockSlot.PropertyId, blockImage.Generation, blockImage.Ring[blockImage.Current]));
			}
			MaterialStorageBufferSlot[] storageBufferSlots = layout.StorageBufferSlots;
			for (int j = 0; j < storageBufferSlots.Length; j++)
			{
				MaterialStorageBufferSlot materialStorageBufferSlot = storageBufferSlots[j];
				if (!Provides(scratch, materialStorageBufferSlot.PropertyId) && GlobalShaderState.TryGetArraySnapshot(materialStorageBufferSlot.PropertyId, out byte[] bytes, out int generation))
				{
					scratch.Add(new MaterialStorageArray(materialStorageBufferSlot.PropertyId, generation, bytes));
				}
			}
			composedStorage.Own = array;
			composedStorage.MaterialVersion = version;
			composedStorage.GlobalVersion = num;
			if (!SameArrays(scratch, composedStorage.Result))
			{
				composedStorage.Result = scratch.ToArray();
			}
			return composedStorage.Result;
		}
	}

	private static bool Provides(List<MaterialStorageArray> arrays, int propertyId)
	{
		foreach (MaterialStorageArray array in arrays)
		{
			if (array.PropertyId == propertyId)
			{
				return true;
			}
		}
		return false;
	}

	private static bool SameArrays(List<MaterialStorageArray> a, MaterialStorageArray[] b)
	{
		if (a.Count != b.Length)
		{
			return false;
		}
		for (int i = 0; i < b.Length; i++)
		{
			if (a[i].PropertyId != b[i].PropertyId || a[i].Generation != b[i].Generation || a[i].Bytes != b[i].Bytes)
			{
				return false;
			}
		}
		return true;
	}

	internal bool TryCopyArray(in MaterialBlockArraySlot slot, Span<byte> destination)
	{
		lock (syncRoot)
		{
			ReadOnlySpan<byte> source;
			int sourceStride;
			MaterialArray<Vector4> value2;
			if (slot.Kind == MaterialBlockArrayKind.Float && floatArrays.TryGetValue(slot.PropertyId, out MaterialArray<float> value))
			{
				source = MemoryMarshal.AsBytes(value.Values.AsSpan());
				sourceStride = 4;
			}
			else if (slot.Kind == MaterialBlockArrayKind.Vector && vectorArrays.TryGetValue(slot.PropertyId, out value2))
			{
				source = MemoryMarshal.AsBytes(value2.Values.AsSpan());
				sourceStride = 16;
			}
			else
			{
				if (slot.Kind != MaterialBlockArrayKind.Matrix || !matrixArrays.TryGetValue(slot.PropertyId, out MaterialArray<Matrix4x4> value3))
				{
					return false;
				}
				source = MemoryMarshal.AsBytes(value3.Values.AsSpan());
				sourceStride = 64;
			}
			GlobalShaderState.CopyArrayElements(source, sourceStride, destination, slot.Stride, slot.ElementBytes, slot.Length);
			return true;
		}
	}

	private MaterialStorageArray[] BuildStorageArraysNoLock()
	{
		int num = floatArrays.Count + vectorArrays.Count + matrixArrays.Count + storageBuffers.Count;
		if (num == 0)
		{
			return Array.Empty<MaterialStorageArray>();
		}
		MaterialStorageArray[] array = new MaterialStorageArray[num];
		int num2 = 0;
		int key;
		foreach (KeyValuePair<int, MaterialArray<float>> floatArray in floatArrays)
		{
			floatArray.Deconstruct(out key, out var value);
			int propertyId = key;
			MaterialArray<float> materialArray = value;
			array[num2++] = new MaterialStorageArray(propertyId, materialArray.Generation, MemoryMarshal.AsBytes(materialArray.Values.AsSpan()).ToArray());
		}
		foreach (KeyValuePair<int, MaterialArray<Vector4>> vectorArray in vectorArrays)
		{
			vectorArray.Deconstruct(out key, out var value2);
			int propertyId2 = key;
			MaterialArray<Vector4> materialArray2 = value2;
			array[num2++] = new MaterialStorageArray(propertyId2, materialArray2.Generation, MemoryMarshal.AsBytes(materialArray2.Values.AsSpan()).ToArray());
		}
		foreach (KeyValuePair<int, MaterialArray<Matrix4x4>> matrixArray in matrixArrays)
		{
			matrixArray.Deconstruct(out key, out var value3);
			int propertyId3 = key;
			MaterialArray<Matrix4x4> materialArray3 = value3;
			array[num2++] = new MaterialStorageArray(propertyId3, materialArray3.Generation, MemoryMarshal.AsBytes(materialArray3.Values.AsSpan()).ToArray());
		}
		foreach (KeyValuePair<int, MaterialArray<byte>> storageBuffer in storageBuffers)
		{
			storageBuffer.Deconstruct(out key, out var value4);
			int propertyId4 = key;
			MaterialArray<byte> materialArray4 = value4;
			array[num2++] = new MaterialStorageArray(propertyId4, materialArray4.Generation, (byte[])materialArray4.Values.Clone());
		}
		return array;
	}

	private void SetArray<T>(Dictionary<int, MaterialArray<T>> arrays, int propertyId, ReadOnlySpan<T> values)
	{
		if (values.IsEmpty)
		{
			return;
		}
		lock (syncRoot)
		{
			if (arrays.TryGetValue(propertyId, out MaterialArray<T> value))
			{
				values.Slice(0, System.Math.Min(values.Length, value.Values.Length)).CopyTo(value.Values);
				value.Generation = ++arrayGeneration;
			}
			else
			{
				arrays[propertyId] = new MaterialArray<T>(values.ToArray(), ++arrayGeneration);
			}
		}
	}

	private T[]? GetArray<T>(Dictionary<int, MaterialArray<T>> arrays, int propertyId)
	{
		EnsureResolved();
		lock (syncRoot)
		{
			MaterialArray<T> value;
			return arrays.TryGetValue(propertyId, out value) ? ((T[])value.Values.Clone()) : null;
		}
	}

	public void EnableKeyword(string keyword)
	{
		if (string.IsNullOrWhiteSpace(keyword))
		{
			return;
		}
		lock (syncRoot)
		{
			if (keywords.Add(keyword))
			{
				PublishKeywordStateNoLock();
				IncrementVersionNoLock();
			}
		}
	}

	public void DisableKeyword(string keyword)
	{
		if (string.IsNullOrWhiteSpace(keyword))
		{
			return;
		}
		lock (syncRoot)
		{
			if (keywords.Remove(keyword))
			{
				PublishKeywordStateNoLock();
				IncrementVersionNoLock();
			}
		}
	}

	public bool HasKeyword(string keyword)
	{
		if (string.IsNullOrWhiteSpace(keyword))
		{
			return false;
		}
		string[] array = Volatile.Read(in keywordState).Keywords;
		for (int i = 0; i < array.Length; i++)
		{
			if (string.Equals(array[i], keyword, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public void SetTextureOffset(int propertyId, Vector2 value)
	{
		lock (syncRoot)
		{
			MaterialTextureTransform textureTransform = GetTextureTransform(propertyId);
			if (!(textureTransform.Offset == value))
			{
				textureTransform = textureTransform with
				{
					Offset = value
				};
				textureTransforms[propertyId] = textureTransform;
				SetTextureSTProperty(propertyId, textureTransform);
				propertySnapshotDirty = true;
				IncrementVersionNoLock();
			}
		}
	}

	public void SetTextureScale(int propertyId, Vector2 value)
	{
		lock (syncRoot)
		{
			MaterialTextureTransform textureTransform = GetTextureTransform(propertyId);
			if (!(textureTransform.Scale == value))
			{
				textureTransform = textureTransform with
				{
					Scale = value
				};
				textureTransforms[propertyId] = textureTransform;
				SetTextureSTProperty(propertyId, textureTransform);
				propertySnapshotDirty = true;
				IncrementVersionNoLock();
			}
		}
	}

	public Vector2 GetTextureOffset(int propertyId)
	{
		lock (syncRoot)
		{
			MaterialTextureTransform value;
			return textureTransforms.TryGetValue(propertyId, out value) ? value.Offset : Vector2.Zero;
		}
	}

	public Vector2 GetTextureScale(int propertyId)
	{
		lock (syncRoot)
		{
			MaterialTextureTransform value;
			return textureTransforms.TryGetValue(propertyId, out value) ? value.Scale : Vector2.One;
		}
	}

	public void SetTextureTransform(int propertyId, MaterialTextureTransform transform)
	{
		lock (syncRoot)
		{
			if (!(GetTextureTransform(propertyId) == transform))
			{
				textureTransforms[propertyId] = transform;
				SetTextureSTProperty(propertyId, transform);
				propertySnapshotDirty = true;
				IncrementVersionNoLock();
			}
		}
	}

	public bool TryGetFloat(int propertyId, out float value)
	{
		return DrawPropertySpanLookup.TryGetFloat(PropertiesForReading, propertyId, out value);
	}

	public bool TryGetInt(int propertyId, out int value)
	{
		return DrawPropertySpanLookup.TryGetInt(PropertiesForReading, propertyId, out value);
	}

	public bool TryGetVector(int propertyId, out Vector4 value)
	{
		return DrawPropertySpanLookup.TryGetVector(PropertiesForReading, propertyId, out value);
	}

	public bool TryGetColor(int propertyId, out Vector4 value)
	{
		return DrawPropertySpanLookup.TryGetColor(PropertiesForReading, propertyId, out value);
	}

	public bool TryGetMatrix(int propertyId, out Matrix4x4 value)
	{
		return DrawPropertySpanLookup.TryGetMatrix(PropertiesForReading, propertyId, out value);
	}

	public bool TryGetTexture<T>(int propertyId, out T? value) where T : class
	{
		return DrawPropertySpanLookup.TryGetTexture<T>(PropertiesForReading, propertyId, out value);
	}

	public void SetProperty(in DrawProperty property)
	{
		lock (syncRoot)
		{
			if (!properties.TryGetValue(property.PropertyId, out var value) || !DrawPropertyEquals(value, property))
			{
				properties[property.PropertyId] = property;
				propertySnapshotDirty = true;
				IncrementVersionNoLock();
			}
		}
	}

	internal void GetKeywordState(out string key, out long keyId)
	{
		EnsureResolved();
		KeywordState keywordState = Volatile.Read(in this.keywordState);
		key = keywordState.Key;
		keyId = ShaderKeywordIdMaker.GetOrCreateId(keywordState.Key);
	}

	private void PublishPropertySnapshotNoLock()
	{
		if (properties.Count == 0)
		{
			Volatile.Write(ref propertySnapshot, Array.Empty<DrawProperty>());
		}
		else
		{
			DrawProperty[] array = new DrawProperty[properties.Count];
			int num = 0;
			foreach (DrawProperty value in properties.Values)
			{
				array[num++] = value;
			}
			Volatile.Write(ref propertySnapshot, array);
		}
		propertySnapshotDirty = false;
	}

	private void PublishKeywordStateNoLock()
	{
		if (keywords.Count == 0)
		{
			Volatile.Write(ref keywordState, KeywordState.Empty);
			return;
		}
		string[] array = new string[keywords.Count];
		int num = 0;
		foreach (string keyword in keywords)
		{
			array[num++] = keyword;
		}
		Array.Sort(array, (IComparer<string>?)StringComparer.Ordinal);
		string key = string.Join(";", array);
		Volatile.Write(ref keywordState, new KeywordState(array, key));
	}

	private void IncrementVersionNoLock()
	{
		Volatile.Write(ref version, version + 1);
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref disposed, 1) == 0)
		{
			Material.Released?.Invoke(Id);
			GC.SuppressFinalize(this);
		}
	}

	~Material()
	{
		try
		{
			Dispose();
		}
		catch
		{
		}
	}

	private static bool DrawPropertyEquals(DrawProperty left, DrawProperty right)
	{
		if (left.PropertyId != right.PropertyId || left.Type != right.Type)
		{
			return false;
		}
		switch (left.Type)
		{
		case DrawPropertyType.Float:
			return left.FloatValue.Equals(right.FloatValue);
		case DrawPropertyType.Int:
			return left.IntValue == right.IntValue;
		case DrawPropertyType.Vector:
		case DrawPropertyType.Color:
			return left.VectorValue.Equals(right.VectorValue);
		case DrawPropertyType.Matrix:
			return left.MatrixValue.Equals(right.MatrixValue);
		case DrawPropertyType.Texture:
			return left.ReferenceValue == right.ReferenceValue;
		default:
			return false;
		}
	}

	private MaterialTextureTransform GetTextureTransform(int propertyId)
	{
		if (!textureTransforms.TryGetValue(propertyId, out var value))
		{
			return MaterialTextureTransform.Identity;
		}
		return value;
	}

	private void SetTextureSTProperty(int propertyId, MaterialTextureTransform transform)
	{
		if (ShaderPropertyId.TryGetName(propertyId, out string name))
		{
			int orCreate = ShaderPropertyId.GetOrCreate(name + "_ST");
			properties[orCreate] = DrawProperty.Vector(orCreate, new Vector4(transform.Offset.X, transform.Offset.Y, transform.Scale.X, transform.Scale.Y));
		}
	}
}
