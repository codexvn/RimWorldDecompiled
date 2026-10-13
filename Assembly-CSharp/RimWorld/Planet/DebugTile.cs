using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C70 RID: 15472
	public class DebugTile
	{
		// Token: 0x17003430 RID: 13360
		// (get) Token: 0x06015034 RID: 86068 RVA: 0x0064AB5C File Offset: 0x00648D5C
		private Vector2 ScreenPos
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(Find.WorldGrid.GetTileCenter(this.tile));
			}
		}

		// Token: 0x17003431 RID: 13361
		// (get) Token: 0x06015035 RID: 86069 RVA: 0x0064AB74 File Offset: 0x00648D74
		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		// Token: 0x17003432 RID: 13362
		// (get) Token: 0x06015036 RID: 86070 RVA: 0x0064ABAC File Offset: 0x00648DAC
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x06015037 RID: 86071 RVA: 0x0064ABE0 File Offset: 0x00648DE0
		public void Draw()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			if (this.mesh == null)
			{
				Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
				for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
				{
					Vector3 a = DebugTile.tmpVerts[i];
					DebugTile.tmpVerts[i] = a + a.normalized * 0.02f;
				}
				this.mesh = new Mesh();
				this.mesh.name = "DebugTile";
				this.mesh.SetVertices(DebugTile.tmpVerts);
				DebugTile.tmpIndices.Clear();
				for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
				{
					DebugTile.tmpIndices.Add(j + 2);
					DebugTile.tmpIndices.Add(j + 1);
					DebugTile.tmpIndices.Add(0);
				}
				this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
			}
			Material material;
			if (this.customMat != null)
			{
				material = this.customMat;
			}
			else
			{
				material = WorldDebugMatsSpectrum.Mat(Mathf.RoundToInt(this.colorPct * 100f) % 100);
			}
			Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x06015038 RID: 86072 RVA: 0x0064AD30 File Offset: 0x00648F30
		public void OnGUI()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			Vector2 screenPos = this.ScreenPos;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 20f, 40f, 40f);
			if (this.displayString != null)
			{
				Widgets.Label(rect, this.displayString);
			}
		}

		// Token: 0x0400EA7A RID: 60026
		public PlanetTile tile;

		// Token: 0x0400EA7B RID: 60027
		public string displayString;

		// Token: 0x0400EA7C RID: 60028
		public float colorPct;

		// Token: 0x0400EA7D RID: 60029
		public int ticksLeft;

		// Token: 0x0400EA7E RID: 60030
		public Material customMat;

		// Token: 0x0400EA7F RID: 60031
		private Mesh mesh;

		// Token: 0x0400EA80 RID: 60032
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x0400EA81 RID: 60033
		private static List<int> tmpIndices = new List<int>();
	}
}
