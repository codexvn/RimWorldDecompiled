using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000735 RID: 1845
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x06002CF8 RID: 11512 RVA: 0x000E8500 File Offset: 0x000E6700
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.CurStageIndex >= 3 && this.pawn.IsHashIntervalTick(300, delta) && this.HangoverSusceptible(this.pawn))
			{
				Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (hediff != null)
				{
					hediff.Severity = 1f;
					return;
				}
				hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
				hediff.Severity = 1f;
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000028E7 File Offset: 0x00000AE7
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x040023CA RID: 9162
		private const int HangoverCheckInterval = 300;
	}
}
