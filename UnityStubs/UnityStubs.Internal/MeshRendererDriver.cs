using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal static class MeshRendererDriver
{
	private static readonly Lock Gate = new Lock();

	private static readonly List<Renderer> Renderers = new List<Renderer>();

	private static readonly List<Renderer> Scratch = new List<Renderer>();

	private static bool _subscribed;

	public static void Register(Renderer renderer)
	{
		using (Gate.EnterScope())
		{
			Renderers.Add(renderer);
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
			for (int num = Renderers.Count - 1; num >= 0; num--)
			{
				if (!(Renderers[num] != null))
				{
					(Renderers[num] as SkinnedMeshRenderer)?.ReleaseRendering();
					Renderers.RemoveAt(num);
				}
			}
			Scratch.AddRange(Renderers);
		}
		foreach (Renderer item in Scratch)
		{
			if (item is SkinnedMeshRenderer skinnedMeshRenderer)
			{
				skinnedMeshRenderer.FrameUpdate();
			}
			else if (item is MeshRenderer meshRenderer)
			{
				meshRenderer.FrameUpdate();
			}
		}
		Scratch.Clear();
	}
}
