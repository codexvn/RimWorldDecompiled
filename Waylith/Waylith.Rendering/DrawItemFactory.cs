using System;
using System.Collections.Generic;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public sealed class DrawItemFactory : IDisposable
{
	private readonly MaterialBindingCache bindingCache = new MaterialBindingCache();

	private readonly Dictionary<(int meshId, int submeshIndex), MeshDrawGeometry> _meshGeometryCache = new Dictionary<(int, int), MeshDrawGeometry>();

	public DrawItemFactory(GraphicsDevice graphicsDevice)
	{
		Mesh.ReleaseRequested += OnMeshReleased;
	}

	private void OnMeshReleased(Mesh mesh)
	{
		int id = mesh.Id;
		List<(int, int)> list = new List<(int, int)>();
		foreach (var key in _meshGeometryCache.Keys)
		{
			if (key.meshId == id)
			{
				list.Add(key);
			}
		}
		foreach (var item in list)
		{
			_meshGeometryCache.Remove(item);
		}
	}

	public FinalDrawItem? TryMake(in MeshDrawCommand command, DrawPropertyArena propertyArena, string globalKeywordKey, int queueOrder = 0, string? diagnosticPrefix = null, ushort hostOrder = 0)
	{
		if (!TryResolveMesh(command.Mesh, command.SubmeshIndex, out MeshDrawGeometry meshDraw, out string failureReason))
		{
			LogDiagnostic(diagnosticPrefix, $"draw command rejected while resolving mesh: {failureReason}; {FormatCommand(in command)}.");
			return null;
		}
		if (!bindingCache.TryGetOrCreate(command.Material, globalKeywordKey, out MaterialBinding binding))
		{
			LogDiagnostic(diagnosticPrefix, "draw command rejected while resolving material binding; " + FormatCommand(in command) + ".");
			return null;
		}
		int renderQueue = ((command.RenderQueueOverride > 0) ? command.RenderQueueOverride : binding.RenderQueue);
		if (command.PropertyRange.IsEmpty)
		{
			return FinalDrawItem.Rent(command.Matrix, command.PassId, meshDraw, binding, hasOverride: false, default(MaterialUniformBytes), default(MaterialTextureResourceSet<Texture>), queueOrder, renderQueue, command.Instancing, command.Layer, hostOrder, command.Storage);
		}
		MaterialBindingResolver.ResolveMaterialData(command.Material, command.PropertyRange, propertyArena, out MaterialUniformBytes uniforms, out MaterialTextureResourceSet<Texture> resources, out bool _);
		bool hasOverride = !uniforms.Equals(in binding.Uniforms) || !resources.Equals(in binding.Resources);
		return FinalDrawItem.Rent(command.Matrix, command.PassId, meshDraw, binding, hasOverride, in uniforms, in resources, queueOrder, renderQueue, command.Instancing, command.Layer, hostOrder, command.Storage);
	}

	private bool TryResolveMesh(Mesh mesh, int submeshIndex, out MeshDrawGeometry meshDraw, out string failureReason)
	{
		(int, int) key = (mesh.Id, submeshIndex);
		GpuMeshResource gpuResource = mesh.GpuResource;
		if (_meshGeometryCache.TryGetValue(key, out MeshDrawGeometry value) && ReferencesCurrentBuffers(value, gpuResource))
		{
			meshDraw = value;
			failureReason = string.Empty;
			return true;
		}
		meshDraw = null;
		if (!gpuResource.HasBuffers)
		{
			failureReason = "mesh has no GPU buffers";
			return false;
		}
		SubMesh[] subMeshes = gpuResource.SubMeshes;
		if ((uint)submeshIndex >= (uint)subMeshes.Length)
		{
			failureReason = $"submesh index {submeshIndex} is outside submesh count {subMeshes.Length}";
			return false;
		}
		SubMesh subMesh = subMeshes[submeshIndex];
		if (subMesh.Topology != 0 || subMesh.IndexCount <= 0)
		{
			failureReason = $"submesh topology/index count is not drawable: topology={subMesh.Topology}, indexCount={subMesh.IndexCount}";
			return false;
		}
		if (!gpuResource.Layout.TryGetAttribute(VertexAttributeSemantic.Position, out var _))
		{
			failureReason = "mesh layout has no position attribute";
			return false;
		}
		ReadOnlySpan<GpuMeshBuffer> vertexStreams = gpuResource.VertexStreams;
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			if (!vertexStreams[i].IsAllocated)
			{
				failureReason = $"vertex stream {i} is not allocated";
				return false;
			}
		}
		GpuMeshBuffer[] array = new GpuMeshBuffer[vertexStreams.Length];
		for (int j = 0; j < vertexStreams.Length; j++)
		{
			array[j] = vertexStreams[j];
		}
		MeshBounds bounds = (subMesh.Bounds.IsEmpty ? gpuResource.Bounds : subMesh.Bounds);
		meshDraw = new MeshDrawGeometry(mesh.Id, submeshIndex, array, string.IsNullOrWhiteSpace(mesh.DebugName) ? $"Mesh#{mesh.Id}" : mesh.DebugName, gpuResource.Layout, gpuResource.IndexBuffer, (gpuResource.IndexFormat == MeshIndexFormat.UInt32) ? VkIndexType.UInt32 : VkIndexType.UInt16, subMesh.IndexCount, subMesh.IndexStart, subMesh.BaseVertex, subMesh.Topology, bounds);
		_meshGeometryCache[key] = meshDraw;
		failureReason = string.Empty;
		return true;
	}

	private static bool ReferencesCurrentBuffers(MeshDrawGeometry meshDraw, GpuMeshResource gpu)
	{
		if (meshDraw.IndexStream != gpu.IndexBuffer)
		{
			return false;
		}
		GpuMeshBuffer[] vertexStreams = meshDraw.VertexStreams;
		ReadOnlySpan<GpuMeshBuffer> vertexStreams2 = gpu.VertexStreams;
		if (vertexStreams.Length != vertexStreams2.Length)
		{
			return false;
		}
		for (int i = 0; i < vertexStreams.Length; i++)
		{
			if (vertexStreams[i] != vertexStreams2[i])
			{
				return false;
			}
		}
		return true;
	}

	public void Dispose()
	{
		Mesh.ReleaseRequested -= OnMeshReleased;
		bindingCache.Dispose();
		_meshGeometryCache.Clear();
	}

	private static void LogDiagnostic(string? diagnosticPrefix, string message)
	{
		if (diagnosticPrefix != null)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message2 = new LogString<Warning>(1, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendFormatted(diagnosticPrefix);
				message2.AppendLiteral(" ");
				message2.AppendFormatted(message);
			}
			log.Warning(message2, "LogDiagnostic", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\DrawCallCore\\DrawItemFactory.cs", 159);
		}
	}

	private static string FormatCommand(in MeshDrawCommand command)
	{
		return $"mesh='{command.Mesh.DebugName}' meshId={command.Mesh.Id} submesh={command.SubmeshIndex} material='{command.Material.DebugName}' materialId={command.Material.Id} passId={command.PassId}";
	}
}
