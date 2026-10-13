using System.Collections.Generic;
using UnityStubs.Internal;

namespace UnityEngine;

public static class ParticlePhysicsExtensions
{
	public static int GetSafeCollisionEventSize(this ParticleSystem ps)
	{
		return 0;
	}

	public static int GetCollisionEvents(this ParticleSystem ps, GameObject go, List<ParticleCollisionEvent> collisionEvents)
	{
		StubWarner.LogStub("GetCollisionEvents", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticlePhysicsExtensions.cs", 10);
		collisionEvents.Clear();
		return 0;
	}
}
