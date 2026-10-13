using UnityStubs.Internal;

namespace UnityEngine;

public struct RaycastHit
{
	public Collider collider { get; internal set; }

	public Vector3 point { get; internal set; }

	public Vector3 normal { get; internal set; }

	public float distance { get; internal set; }

	public int triangleIndex { get; internal set; }

	public Vector2 textureCoord
	{
		get
		{
			StubWarner.LogStub("textureCoord", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RaycastHit.cs", 12);
			return default(Vector2);
		}
	}
}
