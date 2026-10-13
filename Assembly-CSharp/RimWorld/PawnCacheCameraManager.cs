using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020023B3 RID: 9139
	public static class PawnCacheCameraManager
	{
		// Token: 0x17001E18 RID: 7704
		// (get) Token: 0x0600C658 RID: 50776 RVA: 0x0039BFC7 File Offset: 0x0039A1C7
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheCameraInt;
			}
		}

		// Token: 0x17001E19 RID: 7705
		// (get) Token: 0x0600C659 RID: 50777 RVA: 0x0039BFCE File Offset: 0x0039A1CE
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheRendererInt;
			}
		}

		// Token: 0x0600C65B RID: 50779 RVA: 0x0039BFF0 File Offset: 0x0039A1F0
		private static Camera CreatePawnCacheCamera()
		{
			GameObject gameObject = new GameObject("PortraitCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<PawnCacheRenderer>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.position = new Vector3(0f, 10f, 0f);
			component.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 5f;
			component.farClipPlane = 12f;
			return component;
		}

		// Token: 0x0400871C RID: 34588
		private static Camera pawnCacheCameraInt = PawnCacheCameraManager.CreatePawnCacheCamera();

		// Token: 0x0400871D RID: 34589
		private static PawnCacheRenderer pawnCacheRendererInt = PawnCacheCameraManager.pawnCacheCameraInt.GetComponent<PawnCacheRenderer>();
	}
}
