using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382D RID: 14381
	public class PlaceWorker_GlowRadius : PlaceWorker
	{
		// Token: 0x06013E10 RID: 81424 RVA: 0x005E6BA4 File Offset: 0x005E4DA4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing != null)
			{
				return;
			}
			CompProperties_Glower compProperties = def.GetCompProperties<CompProperties_Glower>();
			if (compProperties != null)
			{
				float num = compProperties.glowRadius + -2.1f;
				if (num > 0f)
				{
					GenDraw.DrawRadiusRing(center, num, PlaceWorker_GlowRadius.RingColor, null);
				}
			}
		}

		// Token: 0x0400DBD4 RID: 56276
		private static readonly Color RingColor = new Color(0.8f, 0.8f, 0.4f);

		// Token: 0x0400DBD5 RID: 56277
		private const float RadiusOffset = -2.1f;
	}
}
