using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E7 RID: 8935
	public class PawnFootprintMaker
	{
		// Token: 0x0600C239 RID: 49721 RVA: 0x0038505F File Offset: 0x0038325F
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C23A RID: 49722 RVA: 0x00385070 File Offset: 0x00383270
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.RaceProps.makesFootprints || (this.pawn.IsMutant && !this.pawn.mutant.Def.makesFootprints))
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain == null || !terrain.takeSplashes)
				{
					return;
				}
			}
			if ((this.pawn.Drawer.DrawPos - this.lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.39942405f)
			{
				this.TryPlaceFootprint();
			}
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x00385108 File Offset: 0x00383308
		private void TryPlaceFootprint()
		{
			Vector3 drawPos = this.pawn.Drawer.DrawPos;
			Vector3 normalized = (drawPos - this.lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = (float)(this.lastFootprintRight ? 90 : -90);
			Vector3 b = normalized.RotatedBy(angle) * 0.17f * Mathf.Sqrt(this.pawn.BodySize);
			Vector3 vector = drawPos + PawnFootprintMaker.FootprintOffset + b;
			IntVec3 c = vector.ToIntVec3();
			if (c.InBounds(this.pawn.Map))
			{
				TerrainDef terrain = c.GetTerrain(this.pawn.Map);
				if (terrain != null)
				{
					if (terrain.takeSplashes)
					{
						FleckMaker.WaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						FleckMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}

		// Token: 0x0400849F RID: 33951
		private Pawn pawn;

		// Token: 0x040084A0 RID: 33952
		private Vector3 lastFootprintPlacePos;

		// Token: 0x040084A1 RID: 33953
		private bool lastFootprintRight;

		// Token: 0x040084A2 RID: 33954
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x040084A3 RID: 33955
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x040084A4 RID: 33956
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x040084A5 RID: 33957
		private const float FootprintSplashSize = 2f;
	}
}
