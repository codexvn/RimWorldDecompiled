namespace UnityEngine;

public struct ParticleCollisionEvent
{
	public Vector3 intersection { get; internal set; }

	public Vector3 normal { get; internal set; }

	public Vector3 velocity { get; internal set; }

	public Component colliderComponent { get; internal set; }
}
