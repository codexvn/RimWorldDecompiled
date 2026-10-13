using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000677 RID: 1655
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x000DAD74 File Offset: 0x000D8F74
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			if (fire == null || fire.Destroyed)
			{
				Thing thing = (victim != null) ? victim.GetAttachment(ThingDefOf.Fire) : null;
				if (thing != null)
				{
					fire = (Fire)thing;
				}
			}
			if (fire != null && !fire.Destroyed)
			{
				base.Apply(dinfo, victim);
				fire.fireSize -= dinfo.Amount * 0.01f;
				if (fire.fireSize < 0.1f)
				{
					fire.Destroy(DestroyMode.Vanish);
				}
			}
			Pawn pawn = victim as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return result;
		}

		// Token: 0x040021CA RID: 8650
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
