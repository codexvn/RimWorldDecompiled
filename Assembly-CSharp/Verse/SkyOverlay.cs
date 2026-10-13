using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000544 RID: 1348
	public abstract class SkyOverlay
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000B66D1 File Offset: 0x000B48D1
		// (set) Token: 0x060022FB RID: 8955 RVA: 0x000B66D9 File Offset: 0x000B48D9
		public Color? ForcedOverlayColor { get; protected set; }

		// Token: 0x060022FC RID: 8956 RVA: 0x000B66E2 File Offset: 0x000B48E2
		public static void DrawWorldOverlay(Map map, Material mat, int layer = 0)
		{
			SkyOverlay.DrawWorldOverlay(map, mat, AltitudeLayer.Weather.AltitudeFor(), layer);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000B66F4 File Offset: 0x000B48F4
		public static void DrawWorldOverlay(Map map, Material mat, float altitude, int layer = 0)
		{
			Vector3 position = map.Center.ToVector3Shifted().WithY(altitude);
			Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, mat, layer);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000B6728 File Offset: 0x000B4928
		public static void DrawScreenOverlay(Material mat, int layer = 0, Camera camera = null)
		{
			SkyOverlay.DrawScreenOverlay(mat, AltitudeLayer.Weather.AltitudeFor() + 0.03658537f, layer, camera);
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Reset()
		{
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000B6740 File Offset: 0x000B4940
		public static void DrawScreenOverlay(Material mat, float altitude, int layer = 0, Camera camera = null)
		{
			if (!camera)
			{
				camera = Find.Camera;
			}
			float num = camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * camera.aspect, 1f, num);
			Vector3 position = camera.transform.position;
			position.y = altitude;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(position, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, layer);
		}

		// Token: 0x06002301 RID: 8961
		public abstract void TickOverlay(Map map, float lerpFactor);

		// Token: 0x06002302 RID: 8962
		public abstract void DrawOverlay(Map map);

		// Token: 0x06002303 RID: 8963
		public abstract void SetOverlayColor(Color color);

		// Token: 0x06002304 RID: 8964 RVA: 0x000B67B5 File Offset: 0x000B49B5
		public override string ToString()
		{
			return "NoOverlayOverlay";
		}
	}
}
