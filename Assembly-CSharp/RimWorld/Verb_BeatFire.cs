using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003778 RID: 14200
	public class Verb_BeatFire : Verb
	{
		// Token: 0x06013ACF RID: 80591 RVA: 0x005D8955 File Offset: 0x005D6B55
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x06013AD0 RID: 80592 RVA: 0x005D896C File Offset: 0x005D6B6C
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn.stances.FullBodyBusy || fire.TicksSinceSpawn == 0)
			{
				return false;
			}
			fire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32f, 0f, -1f, this.caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}

		// Token: 0x0400DA02 RID: 55810
		private const int DamageAmount = 32;
	}
}
