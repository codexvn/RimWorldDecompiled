using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002690 RID: 9872
	public class RitualAttachableOutcomeEffectWorker_RandomRecruit : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8E3 RID: 55523 RVA: 0x00404500 File Offset: 0x00402700
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			if (Rand.Chance(0.5f))
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", jobRitual.Map, false);
				Slate slate2 = slate;
				string name = "overridePawnGenParams";
				PawnKindDef villager = PawnKindDefOf.Villager;
				Faction faction = null;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				Ideo ideo = jobRitual.Ritual.ideo;
				slate2.Set<PawnGenerationRequest>(name, new PawnGenerationRequest(villager, faction, context, null, true, false, false, true, false, 20f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false), false);
				QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.WandererJoins, slate);
				extraOutcomeDesc = this.def.letterInfoText;
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x04009307 RID: 37639
		public const float RecruitChance = 0.5f;
	}
}
