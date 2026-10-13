using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Waylith.UnityCompat;

namespace UnityStubs.Internal;

internal static class LineRendererDriver
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
					Release(Renderers[num]);
					Renderers.RemoveAt(num);
				}
			}
			Scratch.AddRange(Renderers);
		}
		if (Scratch.Count == 0)
		{
			return;
		}
		Camera main = Camera.main;
		foreach (Renderer item in Scratch)
		{
			if (item is TrailRenderer trailRenderer)
			{
				trailRenderer.FrameUpdate(main);
			}
			else if (item is LineRenderer lineRenderer)
			{
				lineRenderer.FrameUpdate(main);
			}
		}
		Scratch.Clear();
	}

	private static void Release(Renderer renderer)
	{
		if (renderer is TrailRenderer trailRenderer)
		{
			trailRenderer.ReleaseRendering();
		}
		else if (renderer is LineRenderer lineRenderer)
		{
			lineRenderer.ReleaseRendering();
		}
	}
}
