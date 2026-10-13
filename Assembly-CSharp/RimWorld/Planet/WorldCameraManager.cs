using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C69 RID: 15465
	[StaticConstructorOnStartup]
	public static class WorldCameraManager
	{
		// Token: 0x17003422 RID: 13346
		// (get) Token: 0x06015005 RID: 86021 RVA: 0x0064A121 File Offset: 0x00648321
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x17003423 RID: 13347
		// (get) Token: 0x06015006 RID: 86022 RVA: 0x0064A128 File Offset: 0x00648328
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x17003424 RID: 13348
		// (get) Token: 0x06015007 RID: 86023 RVA: 0x0064A12F File Offset: 0x0064832F
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x06015008 RID: 86024 RVA: 0x0064A138 File Offset: 0x00648338
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x06015009 RID: 86025 RVA: 0x0064A1E8 File Offset: 0x006483E8
		private static Camera CreateWorldCamera()
		{
			GameObject gameObject = new GameObject("WorldCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<WorldCameraDriver>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldLayerMask;
			component.clearFlags = CameraClearFlags.Depth;
			component.useOcclusionCulling = true;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 7500f;
			component.fieldOfView = 20f;
			component.depth = 1f;
			return component;
		}

		// Token: 0x0601500A RID: 86026 RVA: 0x0064A280 File Offset: 0x00648480
		private static Camera CreateWorldSkyboxCamera(Camera parent)
		{
			GameObject gameObject = new GameObject("WorldSkyboxCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(true);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.SetParent(parent.transform);
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldSkyboxLayerMask;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = WorldCameraManager.SkyColor;
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 7500f;
			component.fieldOfView = 60f;
			component.depth = 0f;
			return component;
		}

		// Token: 0x0400EA5B RID: 59995
		private static Camera worldCameraInt;

		// Token: 0x0400EA5C RID: 59996
		private static Camera worldSkyboxCameraInt;

		// Token: 0x0400EA5D RID: 59997
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x0400EA5E RID: 59998
		public static readonly string WorldLayerName = "World";

		// Token: 0x0400EA5F RID: 59999
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x0400EA60 RID: 60000
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x0400EA61 RID: 60001
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x0400EA62 RID: 60002
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x0400EA63 RID: 60003
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x0400EA64 RID: 60004
		public const int FarClipPlane = 7500;

		// Token: 0x0400EA65 RID: 60005
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.11764706f);
	}
}
