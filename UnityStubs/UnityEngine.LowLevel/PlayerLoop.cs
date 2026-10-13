using System;
using UnityEngine.PlayerLoop;
using UnityStubs.Internal;

namespace UnityEngine.LowLevel;

public static class PlayerLoop
{
	public static PlayerLoopSystem GetDefaultPlayerLoop()
	{
		return Build();
	}

	public static PlayerLoopSystem GetCurrentPlayerLoop()
	{
		return Build();
	}

	public static void SetPlayerLoop(PlayerLoopSystem loop)
	{
		StubWarner.LogStub("SetPlayerLoop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\PlayerLoop.cs", 25);
	}

	private static PlayerLoopSystem Build()
	{
		PlayerLoopSystem result = default(PlayerLoopSystem);
		result.type = null;
		result.subSystemList = new PlayerLoopSystem[7]
		{
			Phase(typeof(Initialization)),
			Phase(typeof(EarlyUpdate)),
			Phase(typeof(FixedUpdate)),
			Phase(typeof(PreUpdate), typeof(PreUpdate.IMGUISendQueuedEvents)),
			Phase(typeof(Update), typeof(Update.ScriptRunBehaviourUpdate)),
			Phase(typeof(PreLateUpdate), typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate)),
			Phase(typeof(PostLateUpdate))
		};
		return result;
	}

	private static PlayerLoopSystem Phase(Type type, params Type[] steps)
	{
		PlayerLoopSystem[] array = new PlayerLoopSystem[steps.Length];
		for (int i = 0; i < steps.Length; i++)
		{
			array[i] = new PlayerLoopSystem
			{
				type = steps[i],
				subSystemList = Array.Empty<PlayerLoopSystem>()
			};
		}
		PlayerLoopSystem result = default(PlayerLoopSystem);
		result.type = type;
		result.subSystemList = array;
		return result;
	}
}
