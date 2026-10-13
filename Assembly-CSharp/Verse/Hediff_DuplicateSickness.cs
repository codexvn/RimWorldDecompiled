using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000745 RID: 1861
	public class Hediff_DuplicateSickness : HediffWithComps
	{
		// Token: 0x06002D6E RID: 11630 RVA: 0x000EA3AF File Offset: 0x000E85AF
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Duplicate sickness"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000EA3D8 File Offset: 0x000E85D8
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.Severity >= 0.2f && !Find.History.duplicateSicknessDiscovered)
			{
				Find.LetterStack.ReceiveLetter("DuplicateSicknessLabel".Translate(), "DuplicateSicknessText".Translate(this.pawn), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null, 0, true);
				Find.History.Notify_DuplicateSicknessDiscovered();
			}
			if (this.pawn.IsWorldPawn() && this.Severity >= 0.7f && !this.pawn.Dead)
			{
				this.pawn.Kill(null, null);
			}
		}

		// Token: 0x04002402 RID: 9218
		private const float LetterSeverityThreshold = 0.2f;

		// Token: 0x04002403 RID: 9219
		private const float WorldPawnKillSeverityThreshold = 0.7f;
	}
}
