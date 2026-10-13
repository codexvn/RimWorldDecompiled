using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196E RID: 6510
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x060093D3 RID: 37843 RVA: 0x002B7883 File Offset: 0x002B5A83
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x060093D4 RID: 37844 RVA: 0x002B78BC File Offset: 0x002B5ABC
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.offsetNorth;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.offsetEast;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.offsetSouth;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.offsetWest;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.offset;
			}
		}

		// Token: 0x060093D5 RID: 37845 RVA: 0x002B796C File Offset: 0x002B5B6C
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			Graphic graphic = GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol, null);
			Vector3 loc = center.ToVector3ShiftedWithAltitude(drawAltitude) + thingDef.graphicData.DrawOffsetForRot(rot) + this.DrawOffsetForRot(rot);
			graphic.DrawFromDef(loc, rot, thingDef, 0f);
		}

		// Token: 0x04005F56 RID: 24406
		public float fireSize = 1f;

		// Token: 0x04005F57 RID: 24407
		public float finalFireSize = 1f;

		// Token: 0x04005F58 RID: 24408
		public float fireGrowthDurationTicks = -1f;

		// Token: 0x04005F59 RID: 24409
		public Vector3 offset;

		// Token: 0x04005F5A RID: 24410
		public Vector3? offsetNorth;

		// Token: 0x04005F5B RID: 24411
		public Vector3? offsetSouth;

		// Token: 0x04005F5C RID: 24412
		public Vector3? offsetWest;

		// Token: 0x04005F5D RID: 24413
		public Vector3? offsetEast;
	}
}
