using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB7 RID: 11703
	public class CompPorcupine : ThingComp
	{
		// Token: 0x0601073E RID: 67390 RVA: 0x004BEE58 File Offset: 0x004BD058
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn == null || !pawn.RaceProps.IsFlesh)
			{
				return;
			}
			if (dinfo.WeaponBodyPartGroup == null && dinfo.Weapon != null && !dinfo.Weapon.IsMeleeWeapon)
			{
				return;
			}
			BodyPartRecord corePart = pawn.RaceProps.body.corePart;
			pawn.health.AddHediff(HediffDefOf.PorcupineQuill, corePart, null, null);
		}
	}
}
