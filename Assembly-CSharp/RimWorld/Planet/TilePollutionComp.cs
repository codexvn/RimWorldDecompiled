using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CC3 RID: 15555
	public class TilePollutionComp : WorldComponent
	{
		// Token: 0x170034BD RID: 13501
		// (get) Token: 0x06015256 RID: 86614 RVA: 0x006545B4 File Offset: 0x006527B4
		public float TotalWorldPollution
		{
			get
			{
				if (this.totalWorldPollution < 0f)
				{
					for (int i = 0; i < this.world.grid.TilesCount; i++)
					{
						this.totalWorldPollution += this.world.grid[i].pollution;
					}
				}
				return this.totalWorldPollution;
			}
		}

		// Token: 0x06015257 RID: 86615 RVA: 0x00654612 File Offset: 0x00652812
		public TilePollutionComp(World world) : base(world)
		{
		}

		// Token: 0x06015258 RID: 86616 RVA: 0x00654628 File Offset: 0x00652828
		public override void WorldComponentTick()
		{
			int tilesCount = this.world.grid.TilesCount;
			int num = Mathf.CeilToInt(1.6666667E-05f * (float)tilesCount);
			for (int i = 0; i < num; i++)
			{
				PlanetTile tile = new PlanetTile(this.cycleIndex);
				if (!this.world.grid.InBounds(tile))
				{
					this.totalWorldPollution = this.currentWorldPollution;
					this.cycleIndex = 0;
					this.currentWorldPollution = 0f;
					break;
				}
				this.currentWorldPollution += this.world.grid[tile].pollution;
				this.cycleIndex++;
			}
			if (Find.TickManager.TicksGame % 60000 == 0)
			{
				this.DoPollutionReduction();
			}
		}

		// Token: 0x06015259 RID: 86617 RVA: 0x006546EC File Offset: 0x006528EC
		private void DoPollutionReduction()
		{
			float num = this.TotalWorldPollution * 0.0001f;
			List<int> tiles = Find.World.tilesInRandomOrder.Tiles;
			int num2 = Rand.Range(0, tiles.Count);
			for (int i = 0; i < tiles.Count; i++)
			{
				int tileId = tiles[(i + num2) % tiles.Count];
				PlanetTile planetTile = new PlanetTile(tileId);
				if (this.world.grid[planetTile].pollution > 0f)
				{
					float num3 = Mathf.Min(num, this.world.grid[planetTile].pollution);
					this.world.grid[planetTile].pollution -= num3;
					num -= num3;
					Find.World.renderer.Notify_TilePollutionChanged(planetTile);
					if (num <= 0f)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0601525A RID: 86618 RVA: 0x006547D0 File Offset: 0x006529D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.totalWorldPollution, "totalWorldPollution", 0f, false);
			Scribe_Values.Look<float>(ref this.currentWorldPollution, "currentWorldPollution", 0f, false);
		}

		// Token: 0x0400EBF0 RID: 60400
		private const float WorldFractionCheckPerTick = 1.6666667E-05f;

		// Token: 0x0400EBF1 RID: 60401
		private const int WorldPollutionReductionInterval = 60000;

		// Token: 0x0400EBF2 RID: 60402
		private const float WorldPollutionReductionFactor = 0.0001f;

		// Token: 0x0400EBF3 RID: 60403
		private int cycleIndex;

		// Token: 0x0400EBF4 RID: 60404
		private float totalWorldPollution = -1f;

		// Token: 0x0400EBF5 RID: 60405
		private float currentWorldPollution;
	}
}
