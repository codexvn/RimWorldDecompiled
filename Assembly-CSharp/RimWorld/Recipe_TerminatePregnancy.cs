using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200231D RID: 8989
	public class Recipe_TerminatePregnancy : Recipe_Surgery
	{
		// Token: 0x0600C328 RID: 49960 RVA: 0x0038934C File Offset: 0x0038754C
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn != null && PregnancyUtility.GetPregnancyHediff(pawn) != null;
		}

		// Token: 0x0600C329 RID: 49961 RVA: 0x0038937C File Offset: 0x0038757C
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (PregnancyUtility.GetPregnancyHediff(pawn) == null)
			{
				return;
			}
			if (billDoer != null && base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			if (PregnancyUtility.TryTerminatePregnancy(pawn) && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (billDoer != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
					{
						billDoer,
						pawn
					});
				}
				Messages.Message("MessagePregnancyTerminated".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayerSilentFail))
				{
					base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
				}
			}
		}
	}
}
