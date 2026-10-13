using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Brutal.VulkanApi;
using Unity.Collections;
using UnityEngine.Rendering;
using UnityStubs.Gui;
using UnityStubs.Internal;
using UnityStubs.Rendering;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace UnityEngine;

public class Graphics
{
	private sealed class ImmediateMaterialProperties : IDrawPropertyProvider
	{
		public DrawProperty[] Snapshot;

		public int PropertyCount => Snapshot.Length;

		public int CopyTo(Span<DrawProperty> destination)
		{
			Snapshot.CopyTo(destination);
			return Snapshot.Length;
		}
	}

	private static readonly HashSet<string> UnsupportedWarnings = new HashSet<string>(StringComparer.Ordinal);

	private static readonly int InstanceTransformsPropertyId = Shader.PropertyToID("_InstanceTransforms");

	private static Mesh _blitQuad;

	private static readonly CameraView BlitView = MakeBlitView();

	[ThreadStatic]
	private static ImmediateMaterialProperties _immediateProperties;

	[ThreadStatic]
	internal static List<MeshDrawCommand> ImmediateCapture;

	public static RenderBuffer activeColorBuffer => RenderTexture.active?.colorBuffer ?? default(RenderBuffer);

	public static RenderBuffer activeDepthBuffer => RenderTexture.active?.depthBuffer ?? default(RenderBuffer);

	public static void Blit(Texture source, RenderTexture dest, Material mat)
	{
		Blit(source, dest, mat, -1);
	}

	public static void Blit(Texture source, RenderTexture dest)
	{
		Blit(source, dest, null, 0);
	}

	public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset)
	{
		Blit(source, dest, scale, offset, 0, 0);
	}

	public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
	{
		if (scale != Vector2.one || offset != Vector2.zero)
		{
			StubWarner.LogStub("Blit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphics.cs", 23);
		}
		Blit(source, dest, null, 0);
	}

	public static void DrawTexture(Rect screenRect, Texture texture)
	{
		if (RenderTexture.active != null)
		{
			DrawTexture(screenRect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, Color.white, null, -1);
		}
		else
		{
			GuiBackend.Current?.DrawTexture(screenRect, texture, ScaleMode.StretchToFill, Color.white);
		}
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
	{
		DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, Color.white, null, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color)
	{
		DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, null, -1);
	}

	public static void Blit(Texture source, RenderTexture dest, Material mat, int pass)
	{
		if ((!(dest != null) || !(mat != null) || !TryBlitWithMaterial(source, dest, mat, pass)) && source != null && dest != null)
		{
			TryBlitGpu(source, dest);
		}
		RenderTexture.active = dest;
	}

	private static bool TryBlitWithMaterial(Texture source, RenderTexture dest, Material mat, int pass)
	{
		ICameraTextureRenderer current = CameraTextureRenderer.Current;
		if (current == null)
		{
			return false;
		}
		if (_blitQuad == null)
		{
			_blitQuad = new Mesh
			{
				name = "MaterialBlitQuad"
			};
			_blitQuad.vertices = new Vector3[4]
			{
				new Vector3(-0.5f, 0f, -0.5f),
				new Vector3(-0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, 0.5f),
				new Vector3(0.5f, 0f, -0.5f)
			};
			_blitQuad.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			};
			_blitQuad.triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
		}
		if (source != null)
		{
			mat.mainTexture = source;
		}
		List<MeshDrawCommand> list = new List<MeshDrawCommand>();
		int num = ((pass >= 0) ? 1 : mat.passCount);
		for (int i = 0; i < num; i++)
		{
			Waylith.Rendering.Material material = mat.PassMaterial((pass < 0) ? i : pass);
			if (material != null)
			{
				list.Add(new MeshDrawCommand(_blitQuad.EngineMesh, material, default(DrawPropertyRange), System.Numerics.Matrix4x4.Identity, 0, 0));
			}
		}
		current.Render(dest, list, BlitView, new Rect(0f, 0f, 1f, 1f), null);
		return true;
	}

	private static CameraView MakeBlitView()
	{
		System.Numerics.Matrix4x4 matrix4x = Matrix4x4.ToNumerics(Matrix4x4.Ortho(-0.5f, 0.5f, -0.5f, 0.5f, 0.1f, 100f));
		System.Numerics.Matrix4x4 matrix4x2 = Matrix4x4.ToNumerics(Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * Matrix4x4.Rotate(Quaternion.Inverse(Quaternion.Euler(90f, 0f, 0f))) * Matrix4x4.Translate(new Vector3(0f, -10f, 0f)));
		return new CameraView(matrix4x2, matrix4x, matrix4x * matrix4x2, -1);
	}

	public static void CopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY)
	{
		if (!(src == null) && !(dst == null))
		{
			TryCopyTextureGpu(src, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstMip, dstX, dstY);
		}
	}

	public static void CopyTexture(Texture src, Texture dst)
	{
		if (!(src == null) && !(dst == null))
		{
			CopyTexture(src, 0, 0, 0, 0, src.width, src.height, dst, 0, 0, 0, 0);
		}
	}

	public static void CopyTexture(Texture src, int srcElement, int srcMip, Texture dst, int dstElement, int dstMip)
	{
		if (!(src == null) && !(dst == null))
		{
			int srcWidth = Math.Max(1, src.width >> srcMip);
			int srcHeight = Math.Max(1, src.height >> srcMip);
			CopyTexture(src, srcElement, srcMip, 0, 0, srcWidth, srcHeight, dst, dstElement, dstMip, 0, 0);
		}
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, null, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int renderQueue = 0)
	{
		if (renderQueue == 0)
		{
			Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, null, LightProbeUsage.Off, null);
		}
		else
		{
			DrawMeshCore(mesh, matrix, material, layer, submeshIndex, properties, renderQueue);
		}
	}

	internal static void Internal_DrawMesh(Mesh mesh, int submeshIndex, Matrix4x4 matrix, Material material, int layer, Camera camera, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
	{
		DrawMeshCore(mesh, matrix, material, layer, submeshIndex, properties, 0);
	}

	private static void DrawMeshCore(Mesh mesh, Matrix4x4 matrix, Material material, int layer, int submeshIndex, MaterialPropertyBlock properties, int renderQueue)
	{
		if (!(mesh == null) && !(material == null))
		{
			DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(properties);
			InstanceStorageBinding[] storage = MeshDrawSubmission.CollectDrawStorage(properties);
			byte passId = LayerPassRouter.Resolve(layer);
			System.Numerics.Matrix4x4 matrix2 = Matrix4x4.ToNumerics(matrix);
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material.engineMaterial, propertyRange, matrix2, submeshIndex, passId, null, renderQueue, layer, storage));
			Waylith.Rendering.Material[] extraPassMaterials = material.ExtraPassMaterials;
			foreach (Waylith.Rendering.Material material2 in extraPassMaterials)
			{
				MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material2, propertyRange, matrix2, submeshIndex, passId, null, renderQueue, layer, storage));
			}
		}
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, DrawPropertyBlock properties)
	{
		DrawMesh(mesh, matrix, material, layer, null, 0, properties, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, DrawPropertyBlock properties)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, DrawPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int renderQueue = 0)
	{
		if (!(mesh == null) && !(material == null))
		{
			DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(properties);
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material.engineMaterial, propertyRange, Matrix4x4.ToNumerics(matrix), submeshIndex, LayerPassRouter.Resolve(layer), null, renderQueue, layer));
		}
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Waylith.Rendering.Material material, int layer)
	{
		DrawMesh(mesh, matrix, material, layer, null, 0);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Waylith.Rendering.Material material, int layer, Camera camera, int submeshIndex)
	{
		if (!(mesh == null) && material != null)
		{
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material, default(DrawPropertyRange), Matrix4x4.ToNumerics(matrix), submeshIndex, LayerPassRouter.Resolve(layer), null, 0, layer));
		}
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Waylith.Rendering.Material material, int layer, Camera camera, int submeshIndex, DrawPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows)
	{
		if (!(mesh == null) && material != null)
		{
			DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(properties);
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material, propertyRange, Matrix4x4.ToNumerics(matrix), submeshIndex, LayerPassRouter.Resolve(layer), null, 0, layer));
		}
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera)
	{
		DrawMesh(mesh, matrix, material, layer, camera, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
	{
		DrawMesh(mesh, matrix, material, layer, null, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows: true);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, bool useLightProbes)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, bool useLightProbes)
	{
		DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows, bool useLightProbes)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera)
	{
		DrawMesh(mesh, position, rotation, material, layer, camera, 0, null);
	}

	public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex)
	{
		DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null);
	}

	public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex)
	{
		DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, null, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true, 0, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true, 0, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, null, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true, 0, null);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows: true, 0, null);
	}

	internal static void Internal_DrawMesh_Injected(Mesh mesh, int submeshIndex, ref Matrix4x4 matrix, Material material, int layer, Camera camera, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
	{
		Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, lightProbeProxyVolume);
	}

	public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
	{
		RenderTexture.active = colorBuffer.RenderTexture;
	}

	public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer)
	{
		RenderTexture.active = ((colorBuffers != null && colorBuffers.Length > 0) ? colorBuffers[0].RenderTexture : depthBuffer.RenderTexture);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer)
	{
		EnqueueInstancedDirect(mesh, submeshIndex, material, (matrices == null) ? default(ReadOnlySpan<Matrix4x4>) : new ReadOnlySpan<Matrix4x4>(matrices, 0, Math.Min(count, matrices.Length)), properties, layer);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
	{
		EnqueueInstancedDirect(mesh, submeshIndex, material, (matrices == null) ? default(ReadOnlySpan<Matrix4x4>) : new ReadOnlySpan<Matrix4x4>(matrices, 0, Math.Min(count, matrices.Length)), properties, layer);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
	{
		EnqueueInstancedDirect(mesh, submeshIndex, material, (matrices == null) ? default(Span<Matrix4x4>) : CollectionsMarshal.AsSpan(matrices), properties, layer);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera);
	}

	public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, UnityEngine.Rendering.ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume)
	{
		DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera);
	}

	public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset = 0, MaterialPropertyBlock properties = null, UnityEngine.Rendering.ShadowCastingMode castShadows = UnityEngine.Rendering.ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes)
	{
		if (!(mesh == null) && !(material == null) && bufferWithArgs != null)
		{
			List<InstanceStorageBinding> list = new List<InstanceStorageBinding>();
			properties?.CollectStorageBindings(list);
			DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(properties);
			InstancedDrawInfo instancing = InstancedDrawInfo.Indirect(bufferWithArgs.Data, bufferWithArgs.LastGeneration, argsOffset, list.ToArray(), Vector3.ToNumerics(bounds.center));
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material.engineMaterial, propertyRange, System.Numerics.Matrix4x4.Identity, submeshIndex, LayerPassRouter.Resolve(layer), instancing, 0, layer));
		}
	}

	private static void EnqueueInstancedDirect(Mesh mesh, int submeshIndex, Material material, ReadOnlySpan<Matrix4x4> matrices, MaterialPropertyBlock properties, int layer)
	{
		if (!(mesh == null) && !(material == null) && matrices.Length != 0)
		{
			int length = matrices.Length;
			InstanceDataBuffer instanceDataBuffer = MeshDrawSubmission.RentInstanceDataBuffer();
			int generation = instanceDataBuffer.Write(MemoryMarshal.AsBytes(matrices), 64, length);
			List<InstanceStorageBinding> list = new List<InstanceStorageBinding>
			{
				new InstanceStorageBinding(InstanceTransformsPropertyId, instanceDataBuffer, generation)
			};
			properties?.CollectStorageBindings(list);
			DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(properties);
			InstancedDrawInfo instancing = InstancedDrawInfo.Direct(length, list.ToArray(), InstancesSortCentre(mesh.EngineMesh.Bounds, matrices));
			MeshDrawSubmission.Enqueue(new MeshDrawCommand(mesh.EngineMesh, material.engineMaterial, propertyRange, System.Numerics.Matrix4x4.Identity, submeshIndex, LayerPassRouter.Resolve(layer), instancing, 0, layer));
		}
	}

	private static System.Numerics.Vector3 InstancesSortCentre(in MeshBounds bounds, ReadOnlySpan<Matrix4x4> matrices)
	{
		System.Numerics.Vector3 vector = (bounds.IsEmpty ? System.Numerics.Vector3.Zero : bounds.Center);
		System.Numerics.Vector3 vector2 = (bounds.IsEmpty ? System.Numerics.Vector3.Zero : bounds.Extents);
		System.Numerics.Vector3 vector3 = new System.Numerics.Vector3(float.PositiveInfinity);
		System.Numerics.Vector3 vector4 = new System.Numerics.Vector3(float.NegativeInfinity);
		ReadOnlySpan<Matrix4x4> readOnlySpan = matrices;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ref readonly Matrix4x4 reference = ref readOnlySpan[i];
			System.Numerics.Vector3 vector5 = new System.Numerics.Vector3(reference.m00 * vector.X + reference.m01 * vector.Y + reference.m02 * vector.Z + reference.m03, reference.m10 * vector.X + reference.m11 * vector.Y + reference.m12 * vector.Z + reference.m13, reference.m20 * vector.X + reference.m21 * vector.Y + reference.m22 * vector.Z + reference.m23);
			System.Numerics.Vector3 vector6 = new System.Numerics.Vector3(MathF.Abs(reference.m00) * vector2.X + MathF.Abs(reference.m01) * vector2.Y + MathF.Abs(reference.m02) * vector2.Z, MathF.Abs(reference.m10) * vector2.X + MathF.Abs(reference.m11) * vector2.Y + MathF.Abs(reference.m12) * vector2.Z, MathF.Abs(reference.m20) * vector2.X + MathF.Abs(reference.m21) * vector2.Y + MathF.Abs(reference.m22) * vector2.Z);
			vector3 = System.Numerics.Vector3.Min(vector3, vector5 - vector6);
			vector4 = System.Numerics.Vector3.Max(vector4, vector5 + vector6);
		}
		return (vector3 + vector4) * 0.5f;
	}

	public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation)
	{
		DrawMeshNow(mesh, Matrix4x4.TRS(position, rotation, Vector3.one));
	}

	public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix)
	{
		List<MeshDrawCommand> immediateCapture = ImmediateCapture;
		if (mesh == null || immediateCapture == null)
		{
			WarnUnsupportedDrawPath("DrawMeshNow");
			return;
		}
		Waylith.Rendering.Material currentImmediateMaterial = Material.CurrentImmediateMaterial;
		if (currentImmediateMaterial == null)
		{
			WarnUnsupportedDrawPath("DrawMeshNow");
			return;
		}
		if (_immediateProperties == null)
		{
			_immediateProperties = new ImmediateMaterialProperties();
		}
		_immediateProperties.Snapshot = currentImmediateMaterial.PropertiesForReading;
		DrawPropertyRange propertyRange = MeshDrawSubmission.CopyProperties(_immediateProperties);
		_immediateProperties.Snapshot = null;
		immediateCapture.Add(new MeshDrawCommand(mesh.EngineMesh, currentImmediateMaterial, propertyRange, Matrix4x4.ToNumerics(matrix), 0, LayerPassRouter.Resolve(0)));
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat, int pass)
	{
		DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, Color.white, mat, pass);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat)
	{
		DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
	{
		DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
	{
		DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, null, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
	{
		DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat, int pass)
	{
		DrawTexture(screenRect, texture, new Rect(0f, 0f, 1f, 1f), leftBorder, rightBorder, topBorder, bottomBorder, mat, pass);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Material mat)
	{
		DrawTexture(screenRect, texture, mat, -1);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Material mat, int pass)
	{
		DrawTexture(screenRect, texture, 0, 0, 0, 0, mat, pass);
	}

	internal static void Internal_DrawTexture(ref Internal_DrawTextureArguments args)
	{
		DrawTexture(args.screenRect, args.texture, args.sourceRect, args.leftBorder, args.rightBorder, args.topBorder, args.bottomBorder, args.color, args.mat, args.pass);
	}

	public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat, int pass)
	{
		if (texture == null)
		{
			return;
		}
		RenderTexture active = RenderTexture.active;
		if (active == null)
		{
			IGuiBackend current = GuiBackend.Current;
			if (current == null)
			{
				return;
			}
			if (mat == null)
			{
				current.DrawTextureWithTexCoords(screenRect, texture, sourceRect, color);
				return;
			}
			if (pass >= 0)
			{
				current.DrawTextureWithMaterial(screenRect, texture, sourceRect, color, mat, mat.PassMaterial(pass));
				return;
			}
			for (int i = 0; i < mat.passCount; i++)
			{
				current.DrawTextureWithMaterial(screenRect, texture, sourceRect, color, mat, mat.PassMaterial(i));
			}
		}
		else if (!TryDrawTextureGpu(active, texture, GL.TransformRectBounds(screenRect), sourceRect, color))
		{
			StubWarner.LogStub("DrawTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphics.cs", 434);
		}
	}

	public static void ExecuteCommandBuffer(UnityEngine.Rendering.CommandBuffer buffer)
	{
		StubWarner.LogStub("ExecuteCommandBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphics.cs", 436);
	}

	public static void ExecuteCommandBufferAsync(UnityEngine.Rendering.CommandBuffer buffer, ComputeQueueType queueType)
	{
		StubWarner.LogStub("ExecuteCommandBufferAsync", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Graphics.cs", 437);
	}

	public static void RenderMesh(in RenderParams rparams, Mesh mesh, int submeshIndex, Matrix4x4 objectToWorld, Matrix4x4? prevObjectToWorld = null)
	{
		if (!(mesh == null) && !(rparams.material == null))
		{
			DrawMesh(mesh, objectToWorld, rparams.material, rparams.layer, null, submeshIndex, rparams.matProps);
		}
	}

	public static void RenderMeshInstanced<T0>(RenderParams rparams, Mesh mesh, int submeshIndex, NativeArray<T0> instanceData, int instanceCount = -1, int startInstance = 0)
	{
		WarnUnsupportedDrawPath("RenderMeshInstanced");
	}

	public static void SetRenderTarget(RenderTexture rt)
	{
		RenderTexture.active = rt;
	}

	private static bool TryBlitGpu(Texture source, RenderTexture dest)
	{
		if (!TryGetGpuTexture(source, forSource: true, out var gpuTexture, out var width, out var height, out var surrogate))
		{
			return false;
		}
		using (surrogate)
		{
			return BlitGpu(source, gpuTexture, width, height, dest);
		}
	}

	private static bool BlitGpu(Texture source, GpuTextureAllocation sourceGpu, int sourceWidth, int sourceHeight, RenderTexture dest)
	{
		if (!dest.TryEnsureGpuReady() || dest.GpuTexture == null)
		{
			return false;
		}
		int num = ((source is Texture2D) ? BlitSourceMip(sourceGpu.MipCount, sourceWidth, sourceHeight, dest.width, dest.height) : 0);
		int num2 = Math.Max(1, sourceWidth >> num);
		int num3 = Math.Max(1, sourceHeight >> num);
		GpuTextureService.BlitTexture(sourceGpu, num2, num3, num, 0, 0, num2, num3, dest.GpuTexture, 0, 0, 0, dest.width, dest.height, ToVkFilter(source.filterMode));
		return true;
	}

	private static int BlitSourceMip(int mipCount, int sourceWidth, int sourceHeight, int destWidth, int destHeight)
	{
		float num = MathF.Max((float)sourceWidth / (float)Math.Max(1, destWidth), (float)sourceHeight / (float)Math.Max(1, destHeight));
		if (!(num <= 1f))
		{
			return Math.Clamp((int)MathF.Round(MathF.Log2(num)), 0, mipCount - 1);
		}
		return 0;
	}

	private static bool TryDrawTextureGpu(RenderTexture target, Texture source, Rect screenRect, Rect sourceRect, Color color)
	{
		if (color != Color.white)
		{
			return false;
		}
		if (!TryGetGpuTexture(source, forSource: true, out var gpuTexture, out var width, out var height, out var surrogate))
		{
			return false;
		}
		using (surrogate)
		{
			return DrawTextureGpu(target, source, gpuTexture, width, height, screenRect, sourceRect);
		}
	}

	private static bool DrawTextureGpu(RenderTexture target, Texture source, GpuTextureAllocation sourceGpu, int sourceWidth, int sourceHeight, Rect screenRect, Rect sourceRect)
	{
		if (!target.TryEnsureGpuReady(batch: true) || target.GpuTexture == null)
		{
			return false;
		}
		int num = Math.Clamp((int)MathF.Round(screenRect.x), 0, target.width);
		int num2 = Math.Clamp((int)MathF.Round(screenRect.y), 0, target.height);
		int num3 = Math.Clamp((int)MathF.Round(screenRect.width), 0, target.width - num);
		int num4 = Math.Clamp((int)MathF.Round(screenRect.height), 0, target.height - num2);
		if (num3 <= 0 || num4 <= 0)
		{
			return true;
		}
		int srcX = Math.Clamp((int)MathF.Round(sourceRect.x * (float)sourceWidth), 0, sourceWidth);
		int srcY = Math.Clamp((int)MathF.Round((1f - sourceRect.y - sourceRect.height) * (float)sourceHeight), 0, sourceHeight);
		int num5 = Math.Clamp((int)MathF.Round(sourceRect.width * (float)sourceWidth), 0, sourceWidth);
		int num6 = Math.Clamp((int)MathF.Round(sourceRect.height * (float)sourceHeight), 0, sourceHeight);
		if (num5 <= 0 || num6 <= 0)
		{
			return true;
		}
		GpuTextureService.BlitTexture(sourceGpu, sourceWidth, sourceHeight, 0, srcX, srcY, num5, num6, target.GpuTexture, 0, num, num2, num3, num4, ToVkFilter(source.filterMode), batch: true);
		return true;
	}

	private static bool TryCopyTextureGpu(Texture source, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture destination, int dstMip, int dstX, int dstY)
	{
		if (!TryGetGpuTexture(source, forSource: true, out var gpuTexture, out var _, out var _, out var surrogate))
		{
			return false;
		}
		using (surrogate)
		{
			return CopyTextureGpu(source, gpuTexture, srcMip, srcX, srcY, srcWidth, srcHeight, destination, dstMip, dstX, dstY);
		}
	}

	private static bool CopyTextureGpu(Texture source, GpuTextureAllocation sourceGpu, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture destination, int dstMip, int dstX, int dstY)
	{
		if (!TryGetGpuTexture(destination, forSource: false, out var gpuTexture, out var _, out var _, out var _))
		{
			return false;
		}
		bool batch = (object)RenderTexture.active == source || (object)RenderTexture.active == destination;
		if (sourceGpu.Format != gpuTexture.Format)
		{
			GpuTextureService.BlitTexture(sourceGpu, Math.Max(1, source.width >> srcMip), Math.Max(1, source.height >> srcMip), srcMip, srcX, srcY, srcWidth, srcHeight, gpuTexture, dstMip, dstX, dstY, srcWidth, srcHeight, VkFilter.Nearest, batch);
		}
		else
		{
			GpuTextureService.CopyTexture(sourceGpu, Math.Max(1, source.width >> srcMip), Math.Max(1, source.height >> srcMip), srcMip, srcX, srcY, srcWidth, srcHeight, gpuTexture, dstMip, dstX, dstY, batch);
		}
		return true;
	}

	private static bool TryGetGpuTexture(Texture texture, bool forSource, out GpuTextureAllocation gpuTexture, out int width, out int height, out Waylith.Rendering.Texture2D surrogate)
	{
		gpuTexture = null;
		surrogate = null;
		width = texture.width;
		height = texture.height;
		if (!(texture is Texture2D texture2D))
		{
			if (texture is RenderTexture renderTexture)
			{
				if (!renderTexture.TryEnsureGpuReady((object)RenderTexture.active == renderTexture))
				{
					return false;
				}
				if (renderTexture.GpuTexture == null)
				{
					return false;
				}
				gpuTexture = renderTexture.GpuTexture;
				return true;
			}
			return false;
		}
		if (forSource && texture2D.GpuTexture == null)
		{
			texture2D.EnsureGpuResident();
		}
		if (!forSource && texture2D.GpuTexture == null && texture2D.HasCpuMipStorage)
		{
			return false;
		}
		if (!forSource && texture2D.GpuTexture == null && !texture2D.TryEnsureGpuTextureAllocated())
		{
			return false;
		}
		if (texture2D.GpuTexture == null)
		{
			return false;
		}
		gpuTexture = texture2D.GpuTexture;
		if (forSource)
		{
			gpuTexture.MarkUsed();
		}
		else
		{
			gpuTexture.MarkWritten();
		}
		if (forSource && !gpuTexture.Swizzle.IsIdentity && TryCreateBlitSurrogate(texture2D, out surrogate))
		{
			gpuTexture = surrogate.GpuResource;
		}
		return true;
	}

	private static bool TryCreateBlitSurrogate(Texture2D texture, out Waylith.Rendering.Texture2D surrogate)
	{
		surrogate = null;
		Waylith.Rendering.Texture2D coreTexture = texture.CoreTexture;
		byte[] array = coreTexture?.TryGetCpuMip0();
		if (array == null)
		{
			return false;
		}
		bool linear = coreTexture.CpuData?.Linear ?? false;
		int mipCount = TexturePixelData.CalculateMipCount(coreTexture.width, coreTexture.height);
		surrogate = new Waylith.Rendering.Texture2D(coreTexture.width, coreTexture.height, LoadedTextureFormat.Rgba32, linear, mipCount, createUninitialized: true)
		{
			Name = coreTexture.Name + "_BlitSurrogate"
		};
		surrogate.ReplaceCpuData(coreTexture.width, coreTexture.height, LoadedTextureFormat.Rgba32, new byte[1][] { array }, mipCount, linear, upload: false);
		surrogate.Apply();
		if (surrogate.GpuResource != null)
		{
			return true;
		}
		surrogate.Dispose();
		surrogate = null;
		return false;
	}

	private static VkFilter ToVkFilter(FilterMode mode)
	{
		if (mode != 0)
		{
			return VkFilter.Linear;
		}
		return VkFilter.Nearest;
	}

	private static void WarnUnsupportedDrawPath(string pathName)
	{
		if (UnsupportedWarnings.Add(pathName))
		{
			Debug.LogWarning("Brutal Graphics." + pathName + " is not supported yet; affected draws will be skipped.");
		}
	}

	internal static Color32[] SampleDownTexture(Texture2D texture, int mipLevel, int targetWidth, int targetHeight)
	{
		return SampleTextureRect(texture, mipLevel, 0, 0, Math.Max(1, texture.width >> mipLevel), Math.Max(1, texture.height >> mipLevel), targetWidth, targetHeight);
	}

	internal static Color32[] DownsampleTextureBox(Texture2D texture, int mipLevel, int targetWidth, int targetHeight)
	{
		return DownsampleTextureRectBox(texture, mipLevel, 0, 0, Math.Max(1, texture.width >> mipLevel), Math.Max(1, texture.height >> mipLevel), targetWidth, targetHeight);
	}

	private static Color32[] SampleTexture(Texture texture, int mipLevel, int targetWidth, int targetHeight)
	{
		int srcWidth = Math.Max(1, texture.width >> mipLevel);
		int srcHeight = Math.Max(1, texture.height >> mipLevel);
		return SampleTextureRect(texture, mipLevel, 0, 0, srcWidth, srcHeight, targetWidth, targetHeight);
	}

	private static Color32[] SampleTextureRect(Texture texture, int mipLevel, int srcX, int srcY, int srcWidth, int srcHeight, int targetWidth, int targetHeight)
	{
		Color32[] array = new Color32[targetWidth * targetHeight];
		for (int i = 0; i < targetHeight; i++)
		{
			float num = (float)srcY + ((float)i + 0.5f) / (float)targetHeight * (float)srcHeight;
			for (int j = 0; j < targetWidth; j++)
			{
				float num2 = (float)srcX + ((float)j + 0.5f) / (float)targetWidth * (float)srcWidth;
				array[i * targetWidth + j] = SampleTexturePixel(texture, mipLevel, num2 / (float)Math.Max(1, texture.width >> mipLevel), num / (float)Math.Max(1, texture.height >> mipLevel));
			}
		}
		return array;
	}

	private static Color32[] DownsampleTextureRectBox(Texture texture, int mipLevel, int srcX, int srcY, int srcWidth, int srcHeight, int targetWidth, int targetHeight)
	{
		Color32[] array = new Color32[targetWidth * targetHeight];
		int num = Math.Max(1, texture.width >> mipLevel);
		int num2 = Math.Max(1, texture.height >> mipLevel);
		for (int i = 0; i < targetHeight; i++)
		{
			float num3 = (float)srcY + (float)i / (float)targetHeight * (float)srcHeight;
			float num4 = (float)srcY + ((float)i + 1f) / (float)targetHeight * (float)srcHeight;
			for (int j = 0; j < targetWidth; j++)
			{
				float num5 = (float)srcX + (float)j / (float)targetWidth * (float)srcWidth;
				float num6 = (float)srcX + ((float)j + 1f) / (float)targetWidth * (float)srcWidth;
				Color32 a = SampleTexturePixel(texture, mipLevel, num5 / (float)num, num3 / (float)num2);
				Color32 b = SampleTexturePixel(texture, mipLevel, num6 / (float)num, num3 / (float)num2);
				Color32 c = SampleTexturePixel(texture, mipLevel, num5 / (float)num, num4 / (float)num2);
				Color32 d = SampleTexturePixel(texture, mipLevel, num6 / (float)num, num4 / (float)num2);
				array[i * targetWidth + j] = AverageAlphaWeighted(a, b, c, d);
			}
		}
		return array;
	}

	private static Color32 SampleTexturePixel(Texture texture, int mipLevel, float u, float v)
	{
		if (texture is Texture2D texture2D)
		{
			return texture2D.SampleColor32(mipLevel, u, v);
		}
		return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	private static void WriteTextureRect(Texture destination, int mipLevel, int dstX, int dstY, int width, int height, Color32[] pixels)
	{
		if (destination is Texture2D texture2D)
		{
			texture2D.SetPixels32Region(mipLevel, dstX, dstY, width, height, pixels);
			texture2D.UploadChangedTextureData();
		}
	}

	private static Color32 AverageAlphaWeighted(Color32 a, Color32 b, Color32 c, Color32 d)
	{
		float num = (float)(int)a.a / 255f;
		float num2 = (float)(int)b.a / 255f;
		float num3 = (float)(int)c.a / 255f;
		float num4 = (float)(int)d.a / 255f;
		float num5 = num + num2 + num3 + num4;
		if (num5 <= 1E-06f)
		{
			return new Color32(0, 0, 0, 0);
		}
		float x = ((float)(int)a.r * num + (float)(int)b.r * num2 + (float)(int)c.r * num3 + (float)(int)d.r * num4) / num5;
		float x2 = ((float)(int)a.g * num + (float)(int)b.g * num2 + (float)(int)c.g * num3 + (float)(int)d.g * num4) / num5;
		float x3 = ((float)(int)a.b * num + (float)(int)b.b * num2 + (float)(int)c.b * num3 + (float)(int)d.b * num4) / num5;
		return new Color32(a: (byte)Math.Clamp((int)MathF.Round((float)(a.a + b.a + c.a + d.a) * 0.25f), 0, 255), r: (byte)Math.Clamp((int)MathF.Round(x), 0, 255), g: (byte)Math.Clamp((int)MathF.Round(x2), 0, 255), b: (byte)Math.Clamp((int)MathF.Round(x3), 0, 255));
	}

	private static Color32 Multiply(Color32 a, Color b)
	{
		Color color = a;
		return new Color(color.r * b.r, color.g * b.g, color.b * b.b, color.a * b.a);
	}
}
