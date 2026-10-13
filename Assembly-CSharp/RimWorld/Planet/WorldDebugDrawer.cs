using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C6F RID: 15471
	public class WorldDebugDrawer
	{
		// Token: 0x1700342F RID: 13359
		// (get) Token: 0x0601502B RID: 86059 RVA: 0x0064A8C9 File Offset: 0x00648AC9
		private static float MaxDistToCameraToDisplayLabel
		{
			get
			{
				return WorldCameraDriver.MinAltitude - 100f + 14f;
			}
		}

		// Token: 0x0601502C RID: 86060 RVA: 0x0064A8DC File Offset: 0x00648ADC
		public void FlashTile(PlanetTile tile, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x0601502D RID: 86061 RVA: 0x0064A918 File Offset: 0x00648B18
		public void FlashTile(PlanetTile tile, Material mat, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x0601502E RID: 86062 RVA: 0x0064A954 File Offset: 0x00648B54
		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false, int duration = 50)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x0601502F RID: 86063 RVA: 0x0064A980 File Offset: 0x00648B80
		public void FlashLine(PlanetTile tileA, PlanetTile tileB, int duration = 50)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x06015030 RID: 86064 RVA: 0x0064A9C0 File Offset: 0x00648BC0
		public void WorldDebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				this.debugTiles[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x06015031 RID: 86065 RVA: 0x0064AA1C File Offset: 0x00648C1C
		public void WorldDebugDrawerTick()
		{
			for (int i = this.debugTiles.Count - 1; i >= 0; i--)
			{
				DebugTile debugTile = this.debugTiles[i];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(i);
				}
			}
			for (int j = this.debugLines.Count - 1; j >= 0; j--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[j];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(j);
				}
			}
		}

		// Token: 0x06015032 RID: 86066 RVA: 0x0064AAB8 File Offset: 0x00648CB8
		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (this.debugTiles[i].DistanceToCamera <= WorldDebugDrawer.MaxDistToCameraToDisplayLabel)
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400EA77 RID: 60023
		private List<DebugTile> debugTiles = new List<DebugTile>();

		// Token: 0x0400EA78 RID: 60024
		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		// Token: 0x0400EA79 RID: 60025
		private const int DefaultLifespanTicks = 50;
	}
}
