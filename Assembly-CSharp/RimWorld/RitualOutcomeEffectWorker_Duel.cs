using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200277C RID: 10108
	public class RitualOutcomeEffectWorker_Duel : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DD9B RID: 56731 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Duel()
		{
		}

		// Token: 0x0600DD9C RID: 56732 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Duel(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD9D RID: 56733 RVA: 0x00413B42 File Offset: 0x00411D42
		protected override bool OutcomePossible(RitualOutcomePossibility chance, LordJob_Ritual ritual)
		{
			if (!chance.BestPositiveOutcome(ritual))
			{
				return true;
			}
			return ((LordJob_Ritual_Duel)ritual).duelists.Any((Pawn d) => d.Dead);
		}

		// Token: 0x0600DD9E RID: 56734 RVA: 0x00413B80 File Offset: 0x00411D80
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive)
			{
				float amount = outcome.BestPositiveOutcome(jobRitual) ? 0.5f : 0.25f;
				float xp = outcome.BestPositiveOutcome(jobRitual) ? 5000f : 2500f;
				float xp2 = outcome.BestPositiveOutcome(jobRitual) ? 2000f : 1000f;
				LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)jobRitual;
				foreach (Pawn pawn in totalPresence.Keys)
				{
					if (lordJob_Ritual_Duel.duelists.Contains(pawn))
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp, false, false);
					}
					else
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp2, false, false);
						if (pawn.needs.joy != null)
						{
							pawn.needs.joy.GainJoy(amount, JoyKindDefOf.Social);
						}
					}
				}
			}
		}

		// Token: 0x04009559 RID: 38233
		public const float RecreationGainGood = 0.25f;

		// Token: 0x0400955A RID: 38234
		public const float RecreationGainBest = 0.5f;

		// Token: 0x0400955B RID: 38235
		public const float MeleeXPGainParticipantsGood = 2500f;

		// Token: 0x0400955C RID: 38236
		public const float MeleeXPGainSpectatorsGood = 1000f;

		// Token: 0x0400955D RID: 38237
		public const float MeleeXPGainParticipantsBest = 5000f;

		// Token: 0x0400955E RID: 38238
		public const float MeleeXPGainSpectatorsBest = 2000f;
	}
}
