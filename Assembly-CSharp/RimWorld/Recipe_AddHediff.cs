using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022F3 RID: 8947
	public class Recipe_AddHediff : Recipe_Surgery
	{
		// Token: 0x0600C285 RID: 49797 RVA: 0x003869AB File Offset: 0x00384BAB
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.IsValidNow(thing, part, false);
		}

		// Token: 0x0600C286 RID: 49798 RVA: 0x003869B8 File Offset: 0x00384BB8
		public bool IsValidNow(Thing thing, BodyPartRecord part = null, bool ignoreBills = false)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn != null && (part == null || (!pawn.health.WouldDieAfterAddingHediff(this.recipe.addsHediff, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(this.recipe.addsHediff, part, 1f))) && !pawn.health.hediffSet.HasHediff(this.recipe.addsHediff, false) && (ignoreBills || !pawn.BillStack.Bills.Any((Bill b) => b.recipe == this.recipe)) && !pawn.health.hediffSet.hediffs.Any((Hediff x) => !this.recipe.CompatibleWithHediff(x.def));
		}

		// Token: 0x0600C287 RID: 49799 RVA: 0x00386A88 File Offset: 0x00384C88
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
			this.OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayerSilentFail))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
		}
	}
}
