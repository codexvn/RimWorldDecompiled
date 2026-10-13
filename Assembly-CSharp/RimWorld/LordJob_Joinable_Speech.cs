using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D3 RID: 5843
	public class LordJob_Joinable_Speech : LordJob_Ritual
	{
		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x060089FE RID: 35326 RVA: 0x002930CE File Offset: 0x002912CE
		protected override int MinTicksToFinish
		{
			get
			{
				return base.DurationTicks / 2;
			}
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x060089FF RID: 35327 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06008A00 RID: 35328 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008A01 RID: 35329 RVA: 0x0028E3E3 File Offset: 0x0028C5E3
		public LordJob_Joinable_Speech()
		{
		}

		// Token: 0x06008A02 RID: 35330 RVA: 0x002930D8 File Offset: 0x002912D8
		public LordJob_Joinable_Speech(TargetInfo spot, Pawn organizer, Precept_Ritual ritual, List<RitualStage> stages, RitualRoleAssignments assignments, bool titleSpeech) : base(spot, ritual, null, stages, assignments, organizer, null)
		{
			Building_Throne firstThing = spot.Cell.GetFirstThing(organizer.Map);
			if (firstThing != null)
			{
				this.selectedTarget = firstThing;
			}
			this.titleSpeech = titleSpeech;
		}

		// Token: 0x06008A03 RID: 35331 RVA: 0x00293126 File Offset: 0x00291326
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (stage == null)
			{
				return new LordToil_Speech(this.spot, this.ritual, this, this.organizer);
			}
			return new LordToil_Ritual(this.spot, this, stage, this.organizer);
		}

		// Token: 0x06008A04 RID: 35332 RVA: 0x00293158 File Offset: 0x00291358
		public override string GetReport(Pawn pawn)
		{
			return ((pawn != this.organizer) ? "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER")) : "LordReportGivingSpeech".Translate()) + base.TimeLeftPostfix;
		}

		// Token: 0x06008A05 RID: 35333 RVA: 0x002931A4 File Offset: 0x002913A4
		public override void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			if (this.ticksPassed >= this.MinTicksToFinish && !cancelled)
			{
				base.ApplyOutcome(progress, false, true, false);
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelSpeechCancelled".Translate(), "LetterSpeechCancelled".Translate(this.titleSpeech ? GrammarResolverSimple.PawnResolveBestRoyalTitle(this.organizer) : this.organizer.LabelShort).CapitalizeFirst(), LetterDefOf.NegativeEvent, this.organizer, null, null, null, null, 0, true);
			RitualOutcomeEffectWorker outcomeEffect = this.ritual.outcomeEffect;
			if (outcomeEffect == null)
			{
				return;
			}
			outcomeEffect.ResetCompDatas();
		}

		// Token: 0x04005A0E RID: 23054
		private bool titleSpeech;
	}
}
