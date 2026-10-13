using UnityStubs.Internal;

namespace UnityEngine;

public class AnimationClip : Motion
{
	public float length
	{
		get
		{
			StubWarner.LogStub("length", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AnimationClip.cs", 5);
			return 0f;
		}
	}

	public float frameRate { get; set; } = 60f;

	public WrapMode wrapMode { get; set; }
}
