using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal static class ParticleSystemDriver
{
	private static readonly Lock Gate = new Lock();

	private static readonly List<ParticleSystem> Systems = new List<ParticleSystem>();

	private static readonly List<ParticleSystem> Scratch = new List<ParticleSystem>();

	private static bool _subscribed;

	public static void Register(ParticleSystem system)
	{
		using (Gate.EnterScope())
		{
			Systems.Add(system);
			if (_subscribed)
			{
				return;
			}
			UnityWorld unityWorld = UnityRuntime.Current?.World;
			if (unityWorld != null)
			{
				unityWorld.PostLateUpdate += delegate
				{
					Tick();
				};
				_subscribed = true;
			}
		}
	}

	private static void Tick()
	{
		using (Gate.EnterScope())
		{
			for (int num = Systems.Count - 1; num >= 0; num--)
			{
				if (!(Systems[num] != null))
				{
					Systems[num].ReleaseRendering();
					Systems.RemoveAt(num);
				}
			}
			Scratch.AddRange(Systems);
		}
		if (Scratch.Count == 0)
		{
			return;
		}
		Camera main = Camera.main;
		Quaternion viewRotation = ((main != null) ? main.transform.rotation : Quaternion.Euler(90f, 0f, 0f));
		foreach (ParticleSystem item in Scratch)
		{
			item.FrameUpdate(viewRotation);
		}
		Scratch.Clear();
	}
}
