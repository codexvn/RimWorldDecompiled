using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000339 RID: 825
	public static class OnPostRenderHook
	{
		// Token: 0x060014DE RID: 5342 RVA: 0x000624F8 File Offset: 0x000606F8
		static OnPostRenderHook()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(OnPostRenderHook.OnPreRender));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(OnPostRenderHook.OnPostRender));
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00062550 File Offset: 0x00060750
		public static void HookOnce(Camera camera, Action postRender)
		{
			OnPostRenderHook.hooks.Add(camera, new OnPostRenderHook.Callbacks
			{
				postRender = postRender,
				preRenderCalled = false
			});
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00062584 File Offset: 0x00060784
		private static void OnPreRender(Camera camera)
		{
			OnPostRenderHook.Callbacks callbacks;
			if (OnPostRenderHook.hooks.TryGetValue(camera, out callbacks))
			{
				OnPostRenderHook.hooks[camera] = new OnPostRenderHook.Callbacks
				{
					postRender = callbacks.postRender,
					preRenderCalled = true
				};
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x000625CC File Offset: 0x000607CC
		private static void OnPostRender(Camera camera)
		{
			OnPostRenderHook.Callbacks callbacks;
			if (OnPostRenderHook.hooks.TryGetValue(camera, out callbacks) && callbacks.preRenderCalled)
			{
				OnPostRenderHook.hooks.Remove(camera);
				callbacks.postRender();
			}
		}

		// Token: 0x04001378 RID: 4984
		private static Dictionary<Camera, OnPostRenderHook.Callbacks> hooks = new Dictionary<Camera, OnPostRenderHook.Callbacks>();

		// Token: 0x0200033A RID: 826
		private struct Callbacks
		{
			// Token: 0x04001379 RID: 4985
			public bool preRenderCalled;

			// Token: 0x0400137A RID: 4986
			public Action postRender;
		}
	}
}
