using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002776 RID: 10102
	public class RitualOutcomeEffectWorker_Consumable : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DD8A RID: 56714 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Consumable()
		{
		}

		// Token: 0x0600DD8B RID: 56715 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Consumable(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD8C RID: 56716 RVA: 0x00413784 File Offset: 0x00411984
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(outcome.BestPositiveOutcome(jobRitual) ? 1f : 0.5f))
			{
				Pawn inspiredPawn = (from p in totalPresence.Keys
				where !p.Inspired && DefDatabase<InspirationDef>.AllDefsListForReading.Any((InspirationDef i) => i.Worker.InspirationCanOccur(p))
				select p).RandomElementWithFallback(null);
				if (inspiredPawn == null)
				{
					return;
				}
				InspirationDef inspirationDef = (from i in DefDatabase<InspirationDef>.AllDefsListForReading
				where i.Worker.InspirationCanOccur(inspiredPawn)
				select i).RandomElementWithFallback(null);
				if (inspirationDef == null)
				{
					Log.Error("Could not find inspiration for pawn " + inspiredPawn.Name.ToStringFull);
					return;
				}
				if (!inspiredPawn.mindState.inspirationHandler.TryStartInspiration(inspirationDef, null, false))
				{
					Log.Error("Inspiring " + inspiredPawn.Name.ToStringFull + " failed, but the inspiration worker claimed it can occur!");
					return;
				}
				extraOutcomeDesc = "RitualOutcomeExtraDesc_ConsumableInspiration".Translate(inspiredPawn.Named("PAWN"), inspirationDef.LabelCap.Named("INSPIRATION"), jobRitual.Ritual.Label.Named("RITUAL")).CapitalizeFirst() + " " + inspiredPawn.Inspiration.LetterText;
			}
		}

		// Token: 0x0400954E RID: 38222
		public const float InspirationGainChance = 0.5f;

		// Token: 0x0400954F RID: 38223
		public const float InspirationGainChanceBestOutcome = 1f;
	}
}
