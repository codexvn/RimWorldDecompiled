using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityStubs.Internal;
using Waylith.Rendering;

namespace UnityEngine;

public class MaterialPropertyBlock : IDrawPropertyProvider
{
	private readonly Dictionary<int, ComputeBuffer> _buffers = new Dictionary<int, ComputeBuffer>();

	private readonly Dictionary<int, Color> _colors = new Dictionary<int, Color>();

	private readonly Dictionary<int, float> _floats = new Dictionary<int, float>();

	private readonly Dictionary<int, int> _ints = new Dictionary<int, int>();

	private readonly Dictionary<int, Matrix4x4> _matrices = new Dictionary<int, Matrix4x4>();

	private readonly Dictionary<int, Texture> _textures = new Dictionary<int, Texture>();

	private readonly Dictionary<int, Vector4> _vectors = new Dictionary<int, Vector4>();

	private readonly Dictionary<int, float[]> _floatArrays = new Dictionary<int, float[]>();

	private readonly Dictionary<int, Matrix4x4[]> _matrixArrays = new Dictionary<int, Matrix4x4[]>();

	private readonly Dictionary<int, Vector4[]> _vectorArrays = new Dictionary<int, Vector4[]>();

	public bool HasStorageBindings
	{
		get
		{
			if (_buffers.Count <= 0 && _floatArrays.Count <= 0 && _vectorArrays.Count <= 0)
			{
				return _matrixArrays.Count > 0;
			}
			return true;
		}
	}

	public int PropertyCount => _colors.Count + _floats.Count + _ints.Count + _matrices.Count + _textures.Count + _vectors.Count;

	public MaterialPropertyBlock()
	{
	}

	internal MaterialPropertyBlock(MaterialPropertyBlock source)
	{
		int key;
		foreach (KeyValuePair<int, ComputeBuffer> buffer in source._buffers)
		{
			buffer.Deconstruct(out key, out var value);
			int key2 = key;
			ComputeBuffer value2 = value;
			_buffers[key2] = value2;
		}
		foreach (KeyValuePair<int, Color> color in source._colors)
		{
			color.Deconstruct(out key, out var value3);
			int key3 = key;
			Color value4 = value3;
			_colors[key3] = value4;
		}
		foreach (KeyValuePair<int, float> @float in source._floats)
		{
			@float.Deconstruct(out key, out var value5);
			int key4 = key;
			float value6 = value5;
			_floats[key4] = value6;
		}
		int key5;
		foreach (KeyValuePair<int, int> @int in source._ints)
		{
			@int.Deconstruct(out key, out key5);
			int key6 = key;
			int value7 = key5;
			_ints[key6] = value7;
		}
		foreach (KeyValuePair<int, Matrix4x4> matrix in source._matrices)
		{
			matrix.Deconstruct(out key5, out var value8);
			int key7 = key5;
			Matrix4x4 value9 = value8;
			_matrices[key7] = value9;
		}
		foreach (KeyValuePair<int, Texture> texture in source._textures)
		{
			texture.Deconstruct(out key5, out var value10);
			int key8 = key5;
			Texture value11 = value10;
			_textures[key8] = value11;
		}
		foreach (KeyValuePair<int, Vector4> vector in source._vectors)
		{
			vector.Deconstruct(out key5, out var value12);
			int key9 = key5;
			Vector4 value13 = value12;
			_vectors[key9] = value13;
		}
		foreach (KeyValuePair<int, float[]> floatArray in source._floatArrays)
		{
			floatArray.Deconstruct(out key5, out var value14);
			int key10 = key5;
			float[] array = value14;
			_floatArrays[key10] = (float[])array.Clone();
		}
		foreach (KeyValuePair<int, Matrix4x4[]> matrixArray in source._matrixArrays)
		{
			matrixArray.Deconstruct(out key5, out var value15);
			int key11 = key5;
			Matrix4x4[] array2 = value15;
			_matrixArrays[key11] = (Matrix4x4[])array2.Clone();
		}
		foreach (KeyValuePair<int, Vector4[]> vectorArray in source._vectorArrays)
		{
			vectorArray.Deconstruct(out key5, out var value16);
			int key12 = key5;
			Vector4[] array3 = value16;
			_vectorArrays[key12] = (Vector4[])array3.Clone();
		}
	}

	public void Clear()
	{
		_buffers.Clear();
		_colors.Clear();
		_floats.Clear();
		_ints.Clear();
		_matrices.Clear();
		_textures.Clear();
		_vectors.Clear();
		_floatArrays.Clear();
		_matrixArrays.Clear();
		_vectorArrays.Clear();
	}

	public void SetBuffer(string name, ComputeBuffer value)
	{
		SetBuffer(Shader.PropertyToID(name), value);
	}

	public void SetBuffer(int nameID, ComputeBuffer value)
	{
		_buffers[nameID] = value;
	}

	public void SetColor(string name, Color value)
	{
		SetColor(Shader.PropertyToID(name), value);
	}

	public void SetColor(int nameID, Color value)
	{
		_colors[nameID] = value;
	}

	public void SetFloat(int nameID, float value)
	{
		_floats[nameID] = value;
	}

	public void SetFloat(string name, float value)
	{
		SetFloat(Shader.PropertyToID(name), value);
	}

	public void SetFloatArray(string name, float[] values)
	{
		SetFloatArray(Shader.PropertyToID(name), values);
	}

	public void SetFloatArray(int nameID, float[] values)
	{
		ShaderArrays.Set(_floatArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetFloatArray(string name, List<float> values)
	{
		SetFloatArray(Shader.PropertyToID(name), values);
	}

	public void SetFloatArray(int nameID, List<float> values)
	{
		ShaderArrays.Set(_floatArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetInt(string name, int value)
	{
		SetInt(Shader.PropertyToID(name), value);
	}

	public void SetInt(int nameID, int value)
	{
		_ints[nameID] = value;
	}

	public void SetInteger(int nameID, int value)
	{
		_ints[nameID] = value;
	}

	public void SetMatrix(int nameID, Matrix4x4 value)
	{
		_matrices[nameID] = value;
	}

	public void SetMatrixArray(string name, Matrix4x4[] values)
	{
		SetMatrixArray(Shader.PropertyToID(name), values);
	}

	public void SetMatrixArray(int nameID, Matrix4x4[] values)
	{
		ShaderArrays.Set(_matrixArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetMatrixArray(string name, List<Matrix4x4> values)
	{
		SetMatrixArray(Shader.PropertyToID(name), values);
	}

	public void SetMatrixArray(int nameID, List<Matrix4x4> values)
	{
		ShaderArrays.Set(_matrixArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetTexture(int nameID, Texture value)
	{
		_textures[nameID] = value;
	}

	public void SetTexture(string name, Texture value)
	{
		SetTexture(Shader.PropertyToID(name), value);
	}

	public void SetVector(int nameID, Vector4 value)
	{
		_vectors[nameID] = value;
	}

	public void SetVector(string name, Vector4 value)
	{
		SetVector(Shader.PropertyToID(name), value);
	}

	public void SetVectorArray(int nameID, Vector4[] values)
	{
		ShaderArrays.Set(_vectorArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetVectorArray(string name, Vector4[] values)
	{
		SetVectorArray(Shader.PropertyToID(name), values);
	}

	public void SetVectorArray(int nameID, List<Vector4> values)
	{
		ShaderArrays.Set(_vectorArrays, nameID, ShaderArrays.Check(values));
	}

	public void SetVectorArray(string name, List<Vector4> values)
	{
		SetVectorArray(Shader.PropertyToID(name), values);
	}

	public MaterialPropertyBlock Clone()
	{
		return new MaterialPropertyBlock(this);
	}

	public void CollectStorageBindings(List<InstanceStorageBinding> output)
	{
		int key;
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
		foreach (KeyValuePair<int, float[]> floatArray in _floatArrays)
		{
			floatArray.Deconstruct(out key, out var value2);
			int id = key;
			float[] array = value2;
			AddArrayBinding(output, id, MemoryMarshal.AsBytes(array.AsSpan()), 4, array.Length);
		}
		foreach (KeyValuePair<int, Vector4[]> vectorArray in _vectorArrays)
		{
			vectorArray.Deconstruct(out key, out var value3);
			int id2 = key;
			Vector4[] array2 = value3;
			AddArrayBinding(output, id2, MemoryMarshal.AsBytes(array2.AsSpan()), 16, array2.Length);
		}
		foreach (KeyValuePair<int, Matrix4x4[]> matrixArray in _matrixArrays)
		{
			matrixArray.Deconstruct(out key, out var value4);
			int id3 = key;
			Matrix4x4[] array3 = value4;
			AddArrayBinding(output, id3, MemoryMarshal.AsBytes(array3.AsSpan()), 64, array3.Length);
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
		foreach (KeyValuePair<int, Color> color2 in _colors)
		{
			color2.Deconstruct(out key, out var value);
			int propertyId = key;
			Color color = value;
			destination[result++] = DrawProperty.Color(propertyId, TextureST.Reorder(propertyId, new System.Numerics.Vector4(color.r, color.g, color.b, color.a)));
		}
		foreach (KeyValuePair<int, float> @float in _floats)
		{
			@float.Deconstruct(out key, out var value2);
			int propertyId2 = key;
			float value3 = value2;
			destination[result++] = DrawProperty.Float(propertyId2, value3);
		}
		int key2;
		foreach (KeyValuePair<int, int> @int in _ints)
		{
			@int.Deconstruct(out key, out key2);
			int propertyId3 = key;
			int value4 = key2;
			destination[result++] = DrawProperty.Int(propertyId3, value4);
		}
		foreach (KeyValuePair<int, Matrix4x4> matrix in _matrices)
		{
			matrix.Deconstruct(out key2, out var value5);
			int propertyId4 = key2;
			Matrix4x4 m = value5;
			destination[result++] = DrawProperty.Matrix(propertyId4, Matrix4x4.ToNumerics(m));
		}
		foreach (KeyValuePair<int, Texture> texture2 in _textures)
		{
			texture2.Deconstruct(out key2, out var value6);
			int propertyId5 = key2;
			Texture texture = value6;
			Texture.EnsureSampledTextureReady(texture);
			destination[result++] = DrawProperty.Texture(propertyId5, texture?.CoreTexture);
		}
		foreach (KeyValuePair<int, Vector4> vector2 in _vectors)
		{
			vector2.Deconstruct(out key2, out var value7);
			int propertyId6 = key2;
			Vector4 vector = value7;
			destination[result++] = DrawProperty.Vector(propertyId6, TextureST.Reorder(propertyId6, new System.Numerics.Vector4(vector.x, vector.y, vector.z, vector.w)));
		}
		return result;
	}

	public bool TryGetColor(int nameID, out Color value)
	{
		return _colors.TryGetValue(nameID, out value);
	}

	public bool HasColor(string name)
	{
		return HasColor(Shader.PropertyToID(name));
	}

	public bool HasColor(int nameID)
	{
		if (!_colors.ContainsKey(nameID))
		{
			return _vectors.ContainsKey(nameID);
		}
		return true;
	}

	public bool HasFloat(string name)
	{
		return HasFloat(Shader.PropertyToID(name));
	}

	public bool HasFloat(int nameID)
	{
		if (!_floats.ContainsKey(nameID))
		{
			return _ints.ContainsKey(nameID);
		}
		return true;
	}

	public Color GetColor(int nameID)
	{
		if (!TryGetColor(nameID, out var value))
		{
			return default(Color);
		}
		return value;
	}

	public Color GetColor(string name)
	{
		return GetColor(Shader.PropertyToID(name));
	}

	public float GetFloat(int nameID)
	{
		if (!TryGetFloat(nameID, out var value))
		{
			return 0f;
		}
		return value;
	}

	public float GetFloat(string name)
	{
		return GetFloat(Shader.PropertyToID(name));
	}

	public int GetInt(int nameID)
	{
		if (!TryGetInt(nameID, out var value))
		{
			return 0;
		}
		return value;
	}

	public int GetInt(string name)
	{
		return GetInt(Shader.PropertyToID(name));
	}

	public Vector4 GetVector(int nameID)
	{
		if (!TryGetVector(nameID, out var value))
		{
			return default(Vector4);
		}
		return value;
	}

	public Vector4 GetVector(string name)
	{
		return GetVector(Shader.PropertyToID(name));
	}

	public float[] GetFloatArray(int nameID)
	{
		if (!_floatArrays.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return (float[])value.Clone();
	}

	public float[] GetFloatArray(string name)
	{
		return GetFloatArray(Shader.PropertyToID(name));
	}

	public Vector4[] GetVectorArray(int nameID)
	{
		if (!_vectorArrays.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return (Vector4[])value.Clone();
	}

	public Vector4[] GetVectorArray(string name)
	{
		return GetVectorArray(Shader.PropertyToID(name));
	}

	public Matrix4x4[] GetMatrixArray(int nameID)
	{
		if (!_matrixArrays.TryGetValue(nameID, out var value))
		{
			return null;
		}
		return (Matrix4x4[])value.Clone();
	}

	public Matrix4x4[] GetMatrixArray(string name)
	{
		return GetMatrixArray(Shader.PropertyToID(name));
	}

	public void GetFloatArray(int nameID, List<float> values)
	{
		ShaderArrays.Fill(values, GetFloatArray(nameID));
	}

	public void GetFloatArray(string name, List<float> values)
	{
		GetFloatArray(Shader.PropertyToID(name), values);
	}

	public void GetVectorArray(int nameID, List<Vector4> values)
	{
		ShaderArrays.Fill(values, GetVectorArray(nameID));
	}

	public void GetVectorArray(string name, List<Vector4> values)
	{
		GetVectorArray(Shader.PropertyToID(name), values);
	}

	public void GetMatrixArray(int nameID, List<Matrix4x4> values)
	{
		ShaderArrays.Fill(values, GetMatrixArray(nameID));
	}

	public void GetMatrixArray(string name, List<Matrix4x4> values)
	{
		GetMatrixArray(Shader.PropertyToID(name), values);
	}

	public Matrix4x4 GetMatrix(int nameID)
	{
		if (!TryGetMatrix(nameID, out var value))
		{
			return Matrix4x4.identity;
		}
		return value;
	}

	public Matrix4x4 GetMatrix(string name)
	{
		return GetMatrix(Shader.PropertyToID(name));
	}

	public Texture GetTexture(int nameID)
	{
		if (!TryGetTexture(nameID, out var value))
		{
			return null;
		}
		return value;
	}

	public Texture GetTexture(string name)
	{
		return GetTexture(Shader.PropertyToID(name));
	}

	public bool TryGetFloat(int nameID, out float value)
	{
		return _floats.TryGetValue(nameID, out value);
	}

	public bool TryGetInt(int nameID, out int value)
	{
		return _ints.TryGetValue(nameID, out value);
	}

	public bool TryGetMatrix(int nameID, out Matrix4x4 value)
	{
		return _matrices.TryGetValue(nameID, out value);
	}

	public bool TryGetTexture(int nameID, out Texture value)
	{
		return _textures.TryGetValue(nameID, out value);
	}

	public bool TryGetVector(int nameID, out Vector4 value)
	{
		return _vectors.TryGetValue(nameID, out value);
	}
}
