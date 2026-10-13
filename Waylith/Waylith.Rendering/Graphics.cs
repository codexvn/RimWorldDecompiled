using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Waylith.Graphics.Meshes;
using Waylith.Math;

namespace Waylith.Rendering;

public static class Graphics
{
	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
	{
		DrawMesh(mesh, Matrix4x4Extensions.TRS(position, rotation, Vector3Extensions.get_one()), material, layer, null, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, MaterialPropertyBlock? properties, int layer, int renderQueue = 0)
	{
		DrawMesh(mesh, matrix, material, layer, null, 0, properties, renderQueue);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, object? camera, int submeshIndex, MaterialPropertyBlock? properties, int renderQueue)
	{
		if (mesh != null && material != null)
		{
			MeshDrawSubmission.Enqueue(new QueuedMeshDraw(mesh, material, MeshDrawSubmission.CopyProperties(properties), matrix, layer, submeshIndex, renderQueue, MeshDrawSubmission.CollectDrawStorage(properties)));
		}
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, object? camera)
	{
		DrawMesh(mesh, matrix, material, layer, camera, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
	{
		DrawMesh(mesh, matrix, material, layer, null, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, object? camera, int submeshIndex, MaterialPropertyBlock? properties)
	{
		DrawMesh(mesh, Matrix4x4Extensions.TRS(position, rotation, Vector3Extensions.get_one()), material, layer, camera, submeshIndex, properties);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, object? camera, int submeshIndex, MaterialPropertyBlock? properties)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, 0);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, object? camera)
	{
		DrawMesh(mesh, position, rotation, material, layer, camera, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, object? camera, int submeshIndex)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, object? camera, int submeshIndex)
	{
		DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock? properties, int layer)
	{
		if (mesh != null && material != null && matrices != null)
		{
			MeshDrawSubmission.EnqueueInstanced(mesh, submeshIndex, material, new ReadOnlySpan<Matrix4x4>(matrices, 0, System.Math.Min(count, matrices.Length)), properties, layer);
		}
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock? properties, int layer)
	{
		if (mesh != null && material != null && matrices != null)
		{
			MeshDrawSubmission.EnqueueInstanced(mesh, submeshIndex, material, CollectionsMarshal.AsSpan(matrices), properties, layer);
		}
	}

	public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer? bufferWithArgs, int argsOffset = 0, MaterialPropertyBlock? properties = null, int layer = 0)
	{
		if (mesh != null && material != null && bufferWithArgs != null)
		{
			MeshDrawSubmission.EnqueueInstancedIndirect(mesh, submeshIndex, material, bufferWithArgs, argsOffset, properties, layer);
		}
	}
}
