using System;
using UnityStubs.Internal;

namespace UnityEngine;

public class SkinnedMeshRenderer : Renderer
{
	private Mesh _sharedMesh;

	private Transform[] _bones = Array.Empty<Transform>();

	private float[] _blendShapeWeights = Array.Empty<float>();

	private Mesh _skinned;

	private Mesh _skinnedSource;

	private int _skinnedSourceVersion;

	private Matrix4x4[] _skin = Array.Empty<Matrix4x4>();

	private Matrix4x4[] _lastSkin = Array.Empty<Matrix4x4>();

	private Vector3[] _positions = Array.Empty<Vector3>();

	private Vector3[] _normals = Array.Empty<Vector3>();

	private Vector4[] _tangents = Array.Empty<Vector4>();

	public SkinQuality quality { get; set; }

	public bool updateWhenOffscreen { get; set; }

	public bool forceMatrixRecalculationPerRender { get; set; }

	public bool skinnedMotionVectors { get; set; } = true;

	public GraphicsBuffer.Target vertexBufferTarget { get; set; }

	public Transform rootBone { get; set; }

	public Transform[] bones
	{
		get
		{
			return (Transform[])_bones.Clone();
		}
		set
		{
			_bones = ((value == null) ? Array.Empty<Transform>() : ((Transform[])value.Clone()));
		}
	}

	public Mesh sharedMesh
	{
		get
		{
			return _sharedMesh;
		}
		set
		{
			_sharedMesh = value;
		}
	}

	public float GetBlendShapeWeight(int index)
	{
		if ((uint)index >= (uint)_blendShapeWeights.Length)
		{
			return 0f;
		}
		return _blendShapeWeights[index];
	}

	public void SetBlendShapeWeight(int index, float value)
	{
		if (index >= 0)
		{
			StubWarner.LogStub("SetBlendShapeWeight", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SkinnedMeshRenderer.cs", 45);
			if (index >= _blendShapeWeights.Length)
			{
				Array.Resize(ref _blendShapeWeights, index + 1);
			}
			_blendShapeWeights[index] = value;
		}
	}

	public void BakeMesh(Mesh mesh)
	{
		BakeMesh(mesh, useScale: false);
	}

	public void BakeMesh(Mesh mesh, bool useScale)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		Mesh mesh2 = _sharedMesh;
		if (mesh2 == null)
		{
			return;
		}
		mesh.Clear();
		Transform transform = base.transform;
		Matrix4x4 value = (useScale ? transform.worldToLocalMatrix : Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse);
		if (CanSkin(mesh2))
		{
			Skin(mesh2, value);
			mesh.vertices = _positions;
			if (_normals.Length == _positions.Length)
			{
				mesh.normals = _normals;
			}
			if (_tangents.Length == _positions.Length)
			{
				mesh.tangents = _tangents;
			}
			_lastSkin = Array.Empty<Matrix4x4>();
		}
		else
		{
			mesh.vertices = mesh2.verticesForReading;
			mesh.normals = mesh2.normalsForReading;
			mesh.tangents = mesh2.tangentsForReading;
		}
		mesh.uv = mesh2.uv;
		mesh.colors32 = mesh2.colors32;
		mesh.subMeshCount = mesh2.subMeshCount;
		for (int i = 0; i < mesh2.subMeshCount; i++)
		{
			mesh.SetTriangles(mesh2.GetIndices(i), i);
		}
	}

	public GraphicsBuffer GetVertexBuffer()
	{
		StubWarner.LogStub("GetVertexBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SkinnedMeshRenderer.cs", 86);
		return null;
	}

	public GraphicsBuffer GetPreviousVertexBuffer()
	{
		StubWarner.LogStub("GetPreviousVertexBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SkinnedMeshRenderer.cs", 87);
		return null;
	}

	internal void ReleaseRendering()
	{
		_skinned?.Dispose();
		_skinned = null;
		_skinnedSource = null;
	}

	private bool CanSkin(Mesh mesh)
	{
		if (_bones.Length != 0 && mesh.bindposeCount > 0)
		{
			return mesh.boneWeightsForReading.Length == mesh.vertexCount;
		}
		return false;
	}

	internal void FrameUpdate()
	{
		Mesh mesh = _sharedMesh;
		if (mesh == null || !base.enabled || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!CanSkin(mesh))
		{
			DrawSubMeshes(mesh, base.transform.localToWorldMatrix);
			return;
		}
		Transform transform = ((rootBone != null) ? rootBone : base.transform);
		Matrix4x4 matrix4x = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		if (_skinned == null || (object)_skinnedSource != mesh || _skinnedSourceVersion != mesh.SkinDataVersion || _skinned.vertexCount != mesh.vertexCount)
		{
			_skinned?.Dispose();
			_skinned = mesh.CloneForSkinning();
			_skinnedSource = mesh;
			_skinnedSourceVersion = mesh.SkinDataVersion;
			_lastSkin = Array.Empty<Matrix4x4>();
		}
		ComputeRootSkinMatrices(mesh, transform, matrix4x);
		if (PoseChanged())
		{
			Skin(mesh, null);
			_skinned.WriteSkinned(_positions, _normals, _tangents);
			if (_lastSkin.Length != _skin.Length)
			{
				_lastSkin = new Matrix4x4[_skin.Length];
			}
			_skin.CopyTo(_lastSkin, 0);
		}
		DrawSubMeshes(_skinned, matrix4x);
	}

	private bool PoseChanged()
	{
		if (_skin.Length != _lastSkin.Length)
		{
			return true;
		}
		for (int i = 0; i < _skin.Length; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				float num = _skin[i][j];
				if (MathF.Abs(num - _lastSkin[i][j]) > 1E-05f * MathF.Max(1f, MathF.Abs(num)))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void ComputeRootSkinMatrices(Mesh source, Transform root, Matrix4x4 rootMatrix)
	{
		Matrix4x4[] bindposesForReading = source.bindposesForReading;
		int num = Math.Min(_bones.Length, bindposesForReading.Length);
		if (_skin.Length != num)
		{
			_skin = new Matrix4x4[num];
		}
		Matrix4x4 localToWorldMatrix = root.localToWorldMatrix;
		localToWorldMatrix.m03 = (localToWorldMatrix.m13 = (localToWorldMatrix.m23 = 0f));
		localToWorldMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Inverse(root.rotation), Vector3.one) * localToWorldMatrix;
		Matrix4x4? matrix4x = null;
		for (int i = 0; i < num; i++)
		{
			Transform transform = _bones[i];
			if (transform == null)
			{
				_skin[i] = Matrix4x4.identity;
				continue;
			}
			if (TryRelative(transform, root, out var relative))
			{
				_skin[i] = localToWorldMatrix * relative * bindposesForReading[i];
				continue;
			}
			Matrix4x4[] skin = _skin;
			int num2 = i;
			Matrix4x4 valueOrDefault = matrix4x.GetValueOrDefault();
			Matrix4x4 matrix4x2;
			if (!matrix4x.HasValue)
			{
				valueOrDefault = rootMatrix.inverse;
				matrix4x = valueOrDefault;
				matrix4x2 = valueOrDefault;
			}
			else
			{
				matrix4x2 = valueOrDefault;
			}
			skin[num2] = matrix4x2 * transform.localToWorldMatrix * bindposesForReading[i];
		}
	}

	private static bool TryRelative(Transform bone, Transform root, out Matrix4x4 relative)
	{
		relative = Matrix4x4.identity;
		Transform transform = bone;
		while (transform != null)
		{
			if ((object)transform == root)
			{
				return true;
			}
			relative = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale) * relative;
			transform = transform.parent;
		}
		return false;
	}

	private void ComputeSkinMatrices(Mesh source, Matrix4x4 toTarget)
	{
		Matrix4x4[] bindposesForReading = source.bindposesForReading;
		int num = Math.Min(_bones.Length, bindposesForReading.Length);
		if (_skin.Length != num)
		{
			_skin = new Matrix4x4[num];
		}
		for (int i = 0; i < num; i++)
		{
			_skin[i] = ((_bones[i] != null) ? (toTarget * _bones[i].localToWorldMatrix * bindposesForReading[i]) : Matrix4x4.identity);
		}
	}

	private void Skin(Mesh source, Matrix4x4? toTarget)
	{
		if (toTarget.HasValue)
		{
			ComputeSkinMatrices(source, toTarget.Value);
		}
		BoneWeight[] boneWeightsForReading = source.boneWeightsForReading;
		Vector3[] verticesForReading = source.verticesForReading;
		Vector3[] normalsForReading = source.normalsForReading;
		Vector4[] tangentsForReading = source.tangentsForReading;
		int num = verticesForReading.Length;
		bool flag = normalsForReading.Length == num;
		bool flag2 = tangentsForReading.Length == num;
		if (_positions.Length != num)
		{
			_positions = new Vector3[num];
		}
		if (_normals.Length != (flag ? num : 0))
		{
			_normals = new Vector3[flag ? num : 0];
		}
		if (_tangents.Length != (flag2 ? num : 0))
		{
			_tangents = new Vector4[flag2 ? num : 0];
		}
		Matrix4x4[] skin = _skin;
		for (int i = 0; i < num; i++)
		{
			BoneWeight boneWeight = boneWeightsForReading[i];
			Matrix4x4 m = default(Matrix4x4);
			Accumulate(ref m, skin, boneWeight.boneIndex0, boneWeight.weight0);
			Accumulate(ref m, skin, boneWeight.boneIndex1, boneWeight.weight1);
			Accumulate(ref m, skin, boneWeight.boneIndex2, boneWeight.weight2);
			Accumulate(ref m, skin, boneWeight.boneIndex3, boneWeight.weight3);
			Vector3 vector = verticesForReading[i];
			_positions[i] = new Vector3(m.m00 * vector.x + m.m01 * vector.y + m.m02 * vector.z + m.m03, m.m10 * vector.x + m.m11 * vector.y + m.m12 * vector.z + m.m13, m.m20 * vector.x + m.m21 * vector.y + m.m22 * vector.z + m.m23);
			if (flag)
			{
				_normals[i] = Rotate(in m, normalsForReading[i]).normalized;
			}
			if (flag2)
			{
				Vector4 vector2 = tangentsForReading[i];
				Vector3 normalized = Rotate(in m, new Vector3(vector2.x, vector2.y, vector2.z)).normalized;
				_tangents[i] = new Vector4(normalized.x, normalized.y, normalized.z, vector2.w);
			}
		}
	}

	private static void Accumulate(ref Matrix4x4 m, Matrix4x4[] skin, int bone, float weight)
	{
		if (weight != 0f && (uint)bone < (uint)skin.Length)
		{
			ref Matrix4x4 reference = ref skin[bone];
			m.m00 += reference.m00 * weight;
			m.m01 += reference.m01 * weight;
			m.m02 += reference.m02 * weight;
			m.m03 += reference.m03 * weight;
			m.m10 += reference.m10 * weight;
			m.m11 += reference.m11 * weight;
			m.m12 += reference.m12 * weight;
			m.m13 += reference.m13 * weight;
			m.m20 += reference.m20 * weight;
			m.m21 += reference.m21 * weight;
			m.m22 += reference.m22 * weight;
			m.m23 += reference.m23 * weight;
		}
	}

	private static Vector3 Rotate(in Matrix4x4 m, Vector3 v)
	{
		return new Vector3(m.m00 * v.x + m.m01 * v.y + m.m02 * v.z, m.m10 * v.x + m.m11 * v.y + m.m12 * v.z, m.m20 * v.x + m.m21 * v.y + m.m22 * v.z);
	}
}
