using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002774 RID: 10100
	public class RitualOutcomeEffectWorker_Blinding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DD85 RID: 56709 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Blinding()
		{
		}

		// Token: 0x0600DD86 RID: 56710 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Blinding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD87 RID: 56711 RVA: 0x00413680 File Offset: 0x00411880
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (ModsConfig.RoyaltyActive && outcome.Positive && (outcome.BestPositiveOutcome(jobRitual) || Rand.Chance(0.5f)))
			{
				Pawn pawn = ((LordJob_Ritual_Mutilation)jobRitual).mutilatedPawns[0];
				extraOutcomeDesc = "RitualOutcomeExtraDesc_BlindingPsylink".Translate(pawn.Named("PAWN"));
				List<Ability> existingAbils = pawn.abilities.AllAbilitiesForReading.ToList<Ability>();
				pawn.ChangePsylinkLevel(1, true);
				Ability ability = pawn.abilities.AllAbilitiesForReading.FirstOrDefault((Ability a) => !existingAbils.Contains(a));
				if (ability != null)
				{
					extraOutcomeDesc += " " + "RitualOutcomeExtraDesc_BlindingPsylinkAbility".Translate(ability.def.LabelCap, pawn.Named("PAWN"));
				}
			}
		}

		// Token: 0x0400954C RID: 38220
		public const float PsylinkGainChance = 0.5f;
	}
}
