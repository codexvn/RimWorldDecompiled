using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using Waylith.Rendering.Culling;

namespace Waylith.Rendering;

public sealed class MeshDrawFrame : IDisposable
{
	private static readonly ConcurrentQueue<List<FinalDrawItem>> listPool = new ConcurrentQueue<List<FinalDrawItem>>();

	private bool _disposed;

	public CameraView View { get; }

	public int FrameNumber { get; }

	public List<FinalDrawItem> Draws { get; }

	public List<FinalDrawItem> PersistentDraws { get; }

	public Vector3 SunDirection { get; }

	public float CameraYaw { get; init; }

	public float CameraPitch { get; init; }

	public float SwayHead { get; init; }

	public float WindSpeed { get; init; }

	public float BillboardPitchDelta { get; init; }

	public IReadOnlyList<SceneGrabPoint>? SceneGrabs { get; init; }

	public bool OutlinesEnabled { get; init; }

	public float OutlineSampleRadiusFactor { get; init; }

	public float OutlineOpacityFactor { get; init; }

	public Vector4 BackgroundColor { get; init; }

	public bool PreserveSceneColor { get; init; }

	public OcclusionQuery.BoxSnapshot? OcclusionBoxes { get; init; }

	public CameraView ShadowView { get; init; }

	public MeshDrawFrame(int frameNumber, CameraView view, List<FinalDrawItem> draws, List<FinalDrawItem> persistentDraws, Vector3 sunDirection)
	{
		View = view;
		FrameNumber = frameNumber;
		Draws = draws;
		PersistentDraws = persistentDraws;
		SunDirection = sunDirection;
		base._002Ector();
	}

	public static List<FinalDrawItem> RentList()
	{
		if (!listPool.TryDequeue(out List<FinalDrawItem> result))
		{
			return new List<FinalDrawItem>();
		}
		return result;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			OcclusionQuery.Recycle(OcclusionBoxes);
			List<FinalDrawItem> draws = Draws;
			for (int i = 0; i < draws.Count; i++)
			{
				FinalDrawItem.Return(draws[i]);
			}
			draws.Clear();
			listPool.Enqueue(draws);
			PersistentDraws.Clear();
			listPool.Enqueue(PersistentDraws);
		}
	}
}
