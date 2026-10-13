using System;

namespace Waylith.Profiling;

public sealed class ProfilerWindowResult
{
	public ProfilerWindowRequest? request;

	public int newestOrdinal = -1;

	public int oldestOrdinal = -1;

	public PathAccumulator[] byPathId = Array.Empty<PathAccumulator>();

	public int[] touchedPathIds = Array.Empty<int>();

	public int touchedCount;

	public bool hasBaseline;

	public float[] baselineTotalByPathId = Array.Empty<float>();

	public float[] baselineSelfByPathId = Array.Empty<float>();

	internal bool[] listed = Array.Empty<bool>();

	internal int epoch = -1;

	internal int wholeFrames;

	internal int frames;

	internal bool torn;

	public int FramesIncluded
	{
		get
		{
			ProfilerWindowRequest profilerWindowRequest = request;
			if (profilerWindowRequest == null || !profilerWindowRequest.ClipActive)
			{
				return frames;
			}
			return System.Math.Max(1, wholeFrames);
		}
	}

	public bool IsListed(int pathId)
	{
		if ((uint)pathId < (uint)listed.Length)
		{
			return listed[pathId];
		}
		return false;
	}

	internal void Reset(ProfilerWindowRequest newRequest, int tableCount)
	{
		for (int i = 0; i < touchedCount; i++)
		{
			byPathId[touchedPathIds[i]] = default(PathAccumulator);
			listed[touchedPathIds[i]] = false;
		}
		touchedCount = 0;
		EnsureCapacity(tableCount);
		request = newRequest;
		newestOrdinal = -1;
		oldestOrdinal = -1;
		wholeFrames = 0;
		frames = 0;
		torn = false;
		hasBaseline = false;
	}

	internal void EnsureCapacity(int tableCount)
	{
		if (byPathId.Length < tableCount)
		{
			int newSize = System.Math.Max(tableCount, byPathId.Length * 2);
			Array.Resize(ref byPathId, newSize);
			Array.Resize(ref listed, newSize);
		}
	}

	internal ref PathAccumulator Touch(int pathId)
	{
		if (!listed[pathId])
		{
			listed[pathId] = true;
			if (touchedCount == touchedPathIds.Length)
			{
				Array.Resize(ref touchedPathIds, System.Math.Max(256, touchedPathIds.Length * 2));
			}
			touchedPathIds[touchedCount++] = pathId;
		}
		return ref byPathId[pathId];
	}
}
