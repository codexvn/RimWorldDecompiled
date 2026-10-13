using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Waylith.Math;

namespace Waylith.Rendering;

public class MaterialPropertyBlock : IDrawPropertyProvider
{
	private Dictionary<int, ComputeBuffer>? _buffers;

	private Dictionary<int, Color>? _colors;

	private Dictionary<int, float>? _floats;

	private Dictionary<int, int>? _ints;

	private Dictionary<int, Matrix4x4>? _matrices;

	private Dictionary<int, Texture?>? _textures;

	private Dictionary<int, Vector4>? _vectors;

	private Dictionary<int, (float[] array, int count)>? _floatArrays;

	private Dictionary<int, (Matrix4x4[] array, int count)>? _matrixArrays;

	private Dictionary<int, (Vector4[] array, int count)>? _vectorArrays;

	public bool HasStorageBindings
	{
		get
		{
			Dictionary<int, ComputeBuffer> buffers = _buffers;
			if ((buffers == null || buffers.Count <= 0) && !AnyLiveArray(_floatArrays) && !AnyLiveArray(_vectorArrays))
			{
				return AnyLiveArray(_matrixArrays);
			}
			return true;
		}
	}

	public int PropertyCount => (_colors?.Count ?? 0) + (_floats?.Count ?? 0) + (_ints?.Count ?? 0) + (_matrices?.Count ?? 0) + (_textures?.Count ?? 0) + (_vectors?.Count ?? 0);

	public MaterialPropertyBlock()
	{
	}

	internal MaterialPropertyBlock(MaterialPropertyBlock source)
	{
		Dictionary<int, ComputeBuffer> buffers = source._buffers;
		if (buffers != null && buffers.Count > 0)
		{
			_buffers = new Dictionary<int, ComputeBuffer>(source._buffers);
		}
		Dictionary<int, Color> colors = source._colors;
		if (colors != null && colors.Count > 0)
		{
			_colors = new Dictionary<int, Color>(source._colors);
		}
		Dictionary<int, float> floats = source._floats;
		if (floats != null && floats.Count > 0)
		{
			_floats = new Dictionary<int, float>(source._floats);
		}
		Dictionary<int, int> ints = source._ints;
		if (ints != null && ints.Count > 0)
		{
			_ints = new Dictionary<int, int>(source._ints);
		}
		Dictionary<int, Matrix4x4> matrices = source._matrices;
		if (matrices != null && matrices.Count > 0)
		{
			_matrices = new Dictionary<int, Matrix4x4>(source._matrices);
		}
		Dictionary<int, Texture> textures = source._textures;
		if (textures != null && textures.Count > 0)
		{
			_textures = new Dictionary<int, Texture>(source._textures);
		}
		Dictionary<int, Vector4> vectors = source._vectors;
		if (vectors != null && vectors.Count > 0)
		{
			_vectors = new Dictionary<int, Vector4>(source._vectors);
		}
		Dictionary<int, (float[], int)> floatArrays = source._floatArrays;
		int key;
		if (floatArrays != null && floatArrays.Count > 0)
		{
			_floatArrays = new Dictionary<int, (float[], int)>();
			foreach (KeyValuePair<int, (float[], int)> floatArray in source._floatArrays)
			{
				floatArray.Deconstruct(out key, out var value);
				int key2 = key;
				(float[], int) tuple = value;
				if (tuple.Item2 > 0)
				{
					_floatArrays[key2] = (tuple.Item1.AsSpan(0, tuple.Item2).ToArray(), tuple.Item2);
				}
			}
		}
		Dictionary<int, (Matrix4x4[], int)> matrixArrays = source._matrixArrays;
		if (matrixArrays != null && matrixArrays.Count > 0)
		{
			_matrixArrays = new Dictionary<int, (Matrix4x4[], int)>();
			foreach (KeyValuePair<int, (Matrix4x4[], int)> matrixArray in source._matrixArrays)
			{
				matrixArray.Deconstruct(out key, out var value2);
				int key3 = key;
				(Matrix4x4[], int) tuple2 = value2;
				if (tuple2.Item2 > 0)
				{
					_matrixArrays[key3] = (tuple2.Item1.AsSpan(0, tuple2.Item2).ToArray(), tuple2.Item2);
				}
			}
		}
		Dictionary<int, (Vector4[], int)> vectorArrays = source._vectorArrays;
		if (vectorArrays == null || vectorArrays.Count <= 0)
		{
			return;
		}
		_vectorArrays = new Dictionary<int, (Vector4[], int)>();
		foreach (KeyValuePair<int, (Vector4[], int)> vectorArray in source._vectorArrays)
		{
			vectorArray.Deconstruct(out key, out var value3);
			int key4 = key;
			(Vector4[], int) tuple3 = value3;
			if (tuple3.Item2 > 0)
			{
				_vectorArrays[key4] = (tuple3.Item1.AsSpan(0, tuple3.Item2).ToArray(), tuple3.Item2);
			}
		}
	}

	public void Clear()
	{
		_buffers?.Clear();
		_colors?.Clear();
		_floats?.Clear();
		_ints?.Clear();
		_matrices?.Clear();
		_textures?.Clear();
		_vectors?.Clear();
		ClearArrayCounts(_floatArrays);
		ClearArrayCounts(_matrixArrays);
		ClearArrayCounts(_vectorArrays);
	}

	private static void ClearArrayCounts<T>(Dictionary<int, (T[] array, int count)>? dict)
	{
		if (dict == null)
		{
			return;
		}
		foreach (int key in dict.Keys)
		{
			dict[key] = (dict[key].array, 0);
		}
	}

	public void SetBuffer(string name, ComputeBuffer value)
	{
		SetBuffer(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetBuffer(int nameID, ComputeBuffer value)
	{
		(_buffers ?? (_buffers = new Dictionary<int, ComputeBuffer>()))[nameID] = value;
	}

	public void SetColor(string name, Color value)
	{
		SetColor(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetColor(int nameID, Color value)
	{
		(_colors ?? (_colors = new Dictionary<int, Color>()))[nameID] = value;
	}

	public void SetFloat(int nameID, float value)
	{
		(_floats ?? (_floats = new Dictionary<int, float>()))[nameID] = value;
	}

	public void SetFloat(string name, float value)
	{
		SetFloat(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetFloatArray(int nameID, float[]? values)
	{
		Dictionary<int, (float[] array, int count)>? obj = _floatArrays ?? (_floatArrays = new Dictionary<int, (float[], int)>());
		int num = ((values != null) ? values.Length : 0);
		float[] array = EnsureArrayCapacity(obj, nameID, num);
		if (values != null)
		{
			Array.Copy(values, array, num);
		}
		obj[nameID] = (array, num);
	}

	public void SetFloatArray(int nameID, List<float>? values)
	{
		Dictionary<int, (float[] array, int count)>? obj = _floatArrays ?? (_floatArrays = new Dictionary<int, (float[], int)>());
		int num = values?.Count ?? 0;
		float[] array = EnsureArrayCapacity(obj, nameID, num);
		values?.CopyTo(array);
		obj[nameID] = (array, num);
	}

	public void SetInt(string name, int value)
	{
		SetInt(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetInt(int nameID, int value)
	{
		(_ints ?? (_ints = new Dictionary<int, int>()))[nameID] = value;
	}

	public void SetInteger(int nameID, int value)
	{
		(_ints ?? (_ints = new Dictionary<int, int>()))[nameID] = value;
	}

	public void SetMatrix(int nameID, Matrix4x4 value)
	{
		(_matrices ?? (_matrices = new Dictionary<int, Matrix4x4>()))[nameID] = value;
	}

	public void SetMatrixArray(int nameID, Matrix4x4[]? values)
	{
		Dictionary<int, (Matrix4x4[] array, int count)>? obj = _matrixArrays ?? (_matrixArrays = new Dictionary<int, (Matrix4x4[], int)>());
		int num = ((values != null) ? values.Length : 0);
		Matrix4x4[] array = EnsureArrayCapacity(obj, nameID, num);
		if (values != null)
		{
			Array.Copy(values, array, num);
		}
		obj[nameID] = (array, num);
	}

	public void SetMatrixArray(int nameID, List<Matrix4x4>? values)
	{
		Dictionary<int, (Matrix4x4[] array, int count)>? obj = _matrixArrays ?? (_matrixArrays = new Dictionary<int, (Matrix4x4[], int)>());
		int num = values?.Count ?? 0;
		Matrix4x4[] array = EnsureArrayCapacity(obj, nameID, num);
		values?.CopyTo(array);
		obj[nameID] = (array, num);
	}

	public void SetTexture(int nameID, Texture? value)
	{
		Texture.EnsureSampledReady(value);
		(_textures ?? (_textures = new Dictionary<int, Texture>()))[nameID] = value;
	}

	public void SetTexture(string name, Texture? value)
	{
		SetTexture(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetVector(int nameID, Vector4 value)
	{
		(_vectors ?? (_vectors = new Dictionary<int, Vector4>()))[nameID] = value;
	}

	public void SetVector(string name, Vector4 value)
	{
		SetVector(ShaderPropertyId.GetOrCreate(name), value);
	}

	public void SetVectorArray(int nameID, Vector4[]? values)
	{
		Dictionary<int, (Vector4[] array, int count)>? obj = _vectorArrays ?? (_vectorArrays = new Dictionary<int, (Vector4[], int)>());
		int num = ((values != null) ? values.Length : 0);
		Vector4[] array = EnsureArrayCapacity(obj, nameID, num);
		if (values != null)
		{
			Array.Copy(values, array, num);
		}
		obj[nameID] = (array, num);
	}

	public void SetVectorArray(string name, Vector4[]? values)
	{
		SetVectorArray(ShaderPropertyId.GetOrCreate(name), values);
	}

	public void SetVectorArray(int nameID, List<Vector4>? values)
	{
		Dictionary<int, (Vector4[] array, int count)>? obj = _vectorArrays ?? (_vectorArrays = new Dictionary<int, (Vector4[], int)>());
		int num = values?.Count ?? 0;
		Vector4[] array = EnsureArrayCapacity(obj, nameID, num);
		values?.CopyTo(array);
		obj[nameID] = (array, num);
	}

	private static T[] EnsureArrayCapacity<T>(Dictionary<int, (T[] array, int count)> dict, int nameID, int count)
	{
		if (dict.TryGetValue(nameID, out (T[], int) value) && value.Item1.Length >= count)
		{
			return value.Item1;
		}
		if (count != 0)
		{
			return new T[BitOperations.RoundUpToPowerOf2((uint)count)];
		}
		return Array.Empty<T>();
	}

	public MaterialPropertyBlock Clone()
	{
		return new MaterialPropertyBlock(this);
	}

	private static bool AnyLiveArray<T>(Dictionary<int, (T[] array, int count)>? dict)
	{
		if (dict == null)
		{
			return false;
		}
		foreach (var value in dict.Values)
		{
			if (value.count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void CollectStorageBindings(List<InstanceStorageBinding> output)
	{
		int key;
		if (_buffers != null)
		{
			foreach (KeyValuePair<int, ComputeBuffer> buffer in _buffers)
			{
				buffer.Deconstruct(out key, out var value);
				int propertyId = key;
				ComputeBuffer computeBuffer = value;
				if (computeBuffer != null)
				{
					output.Add(new InstanceStorageBinding(propertyId, computeBuffer.Data, computeBuffer.LastGeneration));
				}
			}
		}
		if (_floatArrays != null)
		{
			foreach (KeyValuePair<int, (float[], int)> floatArray in _floatArrays)
			{
				floatArray.Deconstruct(out key, out var value2);
				int id = key;
				(float[], int) tuple = value2;
				AddArrayBinding(output, id, MemoryMarshal.AsBytes(tuple.Item1.AsSpan(0, tuple.Item2)), 4, tuple.Item2);
			}
		}
		if (_vectorArrays != null)
		{
			foreach (KeyValuePair<int, (Vector4[], int)> vectorArray in _vectorArrays)
			{
				vectorArray.Deconstruct(out key, out var value3);
				int id2 = key;
				(Vector4[], int) tuple2 = value3;
				AddArrayBinding(output, id2, MemoryMarshal.AsBytes(tuple2.Item1.AsSpan(0, tuple2.Item2)), 16, tuple2.Item2);
			}
		}
		if (_matrixArrays == null)
		{
			return;
		}
		foreach (KeyValuePair<int, (Matrix4x4[], int)> matrixArray in _matrixArrays)
		{
			matrixArray.Deconstruct(out key, out var value4);
			int id3 = key;
			(Matrix4x4[], int) tuple3 = value4;
			AddArrayBinding(output, id3, MemoryMarshal.AsBytes(tuple3.Item1.AsSpan(0, tuple3.Item2)), 64, tuple3.Item2);
		}
	}

	private static void AddArrayBinding(List<InstanceStorageBinding> output, int id, ReadOnlySpan<byte> bytes, int stride, int count)
	{
		if (count != 0)
		{
			InstanceDataBuffer instanceDataBuffer = MeshDrawSubmission.RentStorageBuffer();
			output.Add(new InstanceStorageBinding(id, instanceDataBuffer, instanceDataBuffer.Write(bytes, stride, count)));
		}
	}

	public int CopyTo(Span<DrawProperty> destination)
	{
		int result = 0;
		int key;
		if (_colors != null)
		{
			foreach (KeyValuePair<int, Color> color2 in _colors)
			{
				color2.Deconstruct(out key, out var value);
				int propertyId = key;
				Color color = value;
				destination[result++] = DrawProperty.Color(propertyId, new Vector4(color.r, color.g, color.b, color.a));
			}
		}
		if (_floats != null)
		{
			foreach (KeyValuePair<int, float> @float in _floats)
			{
				@float.Deconstruct(out key, out var value2);
				int propertyId2 = key;
				float value3 = value2;
				destination[result++] = DrawProperty.Float(propertyId2, value3);
			}
		}
		int key2;
		if (_ints != null)
		{
			foreach (KeyValuePair<int, int> @int in _ints)
			{
				@int.Deconstruct(out key, out key2);
				int propertyId3 = key;
				int value4 = key2;
				destination[result++] = DrawProperty.Int(propertyId3, value4);
			}
		}
		if (_matrices != null)
		{
			foreach (KeyValuePair<int, Matrix4x4> matrix in _matrices)
			{
				matrix.Deconstruct(out key2, out var value5);
				int propertyId4 = key2;
				Matrix4x4 value6 = value5;
				destination[result++] = DrawProperty.Matrix(propertyId4, value6);
			}
		}
		if (_textures != null)
		{
			foreach (KeyValuePair<int, Texture> texture in _textures)
			{
				texture.Deconstruct(out key2, out var value7);
				int propertyId5 = key2;
				Texture value8 = value7;
				destination[result++] = DrawProperty.Texture(propertyId5, value8);
			}
		}
		if (_vectors != null)
		{
			foreach (KeyValuePair<int, Vector4> vector2 in _vectors)
			{
				vector2.Deconstruct(out key2, out var value9);
				int propertyId6 = key2;
				Vector4 vector = value9;
				destination[result++] = DrawProperty.Vector(propertyId6, new Vector4(vector.X, vector.Y, vector.Z, vector.W));
			}
		}
		return result;
	}

	public bool TryGetColor(int nameID, out Color value)
	{
		value = default(Color);
		return _colors?.TryGetValue(nameID, out value) ?? false;
	}

	public bool TryGetFloat(int nameID, out float value)
	{
		value = 0f;
		return _floats?.TryGetValue(nameID, out value) ?? false;
	}

	public bool TryGetInt(int nameID, out int value)
	{
		value = 0;
		return _ints?.TryGetValue(nameID, out value) ?? false;
	}

	public bool TryGetMatrix(int nameID, out Matrix4x4 value)
	{
		value = default(Matrix4x4);
		return _matrices?.TryGetValue(nameID, out value) ?? false;
	}

	public bool TryGetTexture(int nameID, out Texture? value)
	{
		value = null;
		return _textures?.TryGetValue(nameID, out value) ?? false;
	}

	public bool TryGetVector(int nameID, out Vector4 value)
	{
		value = default(Vector4);
		return _vectors?.TryGetValue(nameID, out value) ?? false;
	}
}
