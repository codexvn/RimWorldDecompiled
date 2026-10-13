using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000196 RID: 406
	public class PawnWaterRippleMaker
	{
		// Token: 0x06000B6C RID: 2924 RVA: 0x0003625F File Offset: 0x0003445F
		public PawnWaterRippleMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00036270 File Offset: 0x00034470
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.pawn.Flying)
			{
				return;
			}
			if ((this.pawn.Swimming && GenTicks.IsTickInterval(this.pawn.GetHashCode(), 20)) || (this.pawn.pather.Moving && GenTicks.IsTickInterval(this.pawn.GetHashCode(), 25) && this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position).IsWater))
			{
				this.SpawnRippleFleck(this.pawn);
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00036308 File Offset: 0x00034508
		private void SpawnRippleFleck(Pawn pawn)
		{
			float size = Mathf.Clamp(pawn.BodySize, 0.5f, 2f) * 0.6f;
			FleckMaker.WaterRipple(pawn.DrawPos.WithY(AltitudeLayer.MoteLow.AltitudeFor()), pawn.Map, size);
		}

		// Token: 0x0400068C RID: 1676
		private Pawn pawn;

		// Token: 0x0400068D RID: 1677
		private const int RippleSpawnIntervalTicks = 25;

		// Token: 0x0400068E RID: 1678
		private const int RippleSpawnIntervalTicksSwimming = 20;

		// Token: 0x0400068F RID: 1679
		private const float RippleStartingSize = 0.6f;

		// Token: 0x04000690 RID: 1680
		private const float MinPawnSizeFactor = 0.5f;

		// Token: 0x04000691 RID: 1681
		private const float MaxPawnSizeFactor = 2f;
	}
}
