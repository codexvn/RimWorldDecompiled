using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E0 RID: 992
	public static class CellRenderer
	{
		// Token: 0x06001958 RID: 6488 RVA: 0x0007FF80 File Offset: 0x0007E180
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0007FFA7 File Offset: 0x0007E1A7
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0007FFC2 File Offset: 0x0007E1C2
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0007FFD4 File Offset: 0x0007E1D4
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(c))
			{
				return;
			}
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00080010 File Offset: 0x0007E210
		public static void RenderSpot(IntVec3 c, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), colorPct, scale);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00080022 File Offset: 0x0007E222
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), scale);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00080034 File Offset: 0x0007E234
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				return;
			}
			loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 s = new Vector3(scale, 1f, scale);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
		}

		// Token: 0x04001666 RID: 5734
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04001667 RID: 5735
		private static CellRect viewRect;
	}
}
