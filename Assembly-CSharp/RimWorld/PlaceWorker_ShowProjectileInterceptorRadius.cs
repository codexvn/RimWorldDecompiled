using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380B RID: 14347
	public class PlaceWorker_ShowProjectileInterceptorRadius : PlaceWorker
	{
		// Token: 0x06013DAF RID: 81327 RVA: 0x005E50B0 File Offset: 0x005E32B0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_ProjectileInterceptor compProperties = def.GetCompProperties<CompProperties_ProjectileInterceptor>();
			if (compProperties != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties.radius);
			}
		}
	}
}
