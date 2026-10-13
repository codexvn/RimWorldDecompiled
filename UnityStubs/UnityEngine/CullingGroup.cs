using System;
using UnityStubs.Internal;

namespace UnityEngine;

public class CullingGroup : IDisposable
{
	public delegate void StateChanged(CullingGroupEvent sphere);

	public StateChanged onStateChanged { get; set; }

	public bool enabled { get; set; } = true;

	public Camera targetCamera { get; set; }

	public CullingGroup()
	{
		StubWarner.LogStub(".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 18);
	}

	public void Dispose()
	{
	}

	public void SetBoundingSpheres(BoundingSphere[] array)
	{
		StubWarner.LogStub("SetBoundingSpheres", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 23);
	}

	public void SetBoundingSphereCount(int count)
	{
		StubWarner.LogStub("SetBoundingSphereCount", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 24);
	}

	public void EraseSwapBack(int index)
	{
		StubWarner.LogStub("EraseSwapBack", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 25);
	}

	public int QueryIndices(bool visible, int[] result, int firstIndex)
	{
		StubWarner.LogStub("QueryIndices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 26);
		return 0;
	}

	public int QueryIndices(int distanceIndex, int[] result, int firstIndex)
	{
		StubWarner.LogStub("QueryIndices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 27);
		return 0;
	}

	public int QueryIndices(bool visible, int distanceIndex, int[] result, int firstIndex)
	{
		StubWarner.LogStub("QueryIndices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 28);
		return 0;
	}

	public bool IsVisible(int index)
	{
		StubWarner.LogStub("IsVisible", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 29);
		return true;
	}

	public int GetDistance(int index)
	{
		StubWarner.LogStub("GetDistance", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 30);
		return 0;
	}

	public void SetBoundingDistances(float[] distances)
	{
		StubWarner.LogStub("SetBoundingDistances", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 31);
	}

	public void SetDistanceReferencePoint(Vector3 point)
	{
		StubWarner.LogStub("SetDistanceReferencePoint", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 32);
	}

	public void SetDistanceReferencePoint(Transform transform)
	{
		StubWarner.LogStub("SetDistanceReferencePoint", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CullingGroup.cs", 33);
	}
}
