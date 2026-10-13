using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000783 RID: 1923
	public class HediffGiver_Terrain : HediffGiver
	{
		// Token: 0x06002EFB RID: 12027 RVA: 0x000F168C File Offset: 0x000EF88C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (!pawn.Spawned)
			{
				return;
			}
			TerrainDef terrain = pawn.Position.GetTerrain(pawn.Map);
			if (terrain.ignitePawnsIntervalTicks > 0f && Rand.MTBEventOccurs(terrain.ignitePawnsIntervalTicks, 1f, 60f) && Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(pawn)))
			{
				pawn.TryAttachFire(Rand.Range(0.15f, 0.25f), null);
			}
			if (terrain.burnDamage > 0 && Rand.MTBEventOccurs(terrain.burnIntervalTicks, 1f, 60f))
			{
				int num = Mathf.Max(terrain.burnDamage, 3);
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				dinfo.SetBodyRegion(BodyPartHeight.Bottom, BodyPartDepth.Outside);
				pawn.TakeDamage(dinfo);
			}
		}
	}
}
