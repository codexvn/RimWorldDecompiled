using UnityStubs.Internal;

namespace UnityEngine;

public class Animator : Behaviour
{
	public RuntimeAnimatorController runtimeAnimatorController { get; set; }

	public bool applyRootMotion { get; set; }

	public AnimatorCullingMode cullingMode { get; set; }

	public float speed { get; set; } = 1f;

	public void Play(string stateName)
	{
		Play(stateName, -1, float.NegativeInfinity);
	}

	public void Play(string stateName, int layer)
	{
		Play(stateName, layer, float.NegativeInfinity);
	}

	public void Play(string stateName, int layer, float normalizedTime)
	{
		StubWarner.LogStub("Play", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Animator.cs", 19);
	}

	public void Update(float deltaTime)
	{
		StubWarner.LogStub("Update", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Animator.cs", 20);
	}
}
