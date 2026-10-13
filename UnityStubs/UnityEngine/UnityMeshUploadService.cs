using System;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;

namespace UnityEngine;

public static class UnityMeshUploadService
{
	private static readonly object Sync = new object();

	private static GraphicsDevice _graphicsDevice;

	public static void Configure(GraphicsDevice graphicsDevice)
	{
		ArgumentNullException.ThrowIfNull(graphicsDevice, "graphicsDevice");
		lock (Sync)
		{
			_graphicsDevice = graphicsDevice;
		}
	}

	public static void Shutdown()
	{
		lock (Sync)
		{
			_graphicsDevice = null;
		}
	}

	public static void Upload(Waylith.Graphics.Meshes.Mesh mesh)
	{
		ArgumentNullException.ThrowIfNull(mesh, "mesh");
		lock (Sync)
		{
			_graphicsDevice?.MeshUploads.Enqueue(mesh);
		}
	}
}
