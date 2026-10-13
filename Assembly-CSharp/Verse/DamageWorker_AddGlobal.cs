using System;

namespace Verse
{
	// Token: 0x02000676 RID: 1654
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06002982 RID: 10626 RVA: 0x000DAD20 File Offset: 0x000D8F20
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}
