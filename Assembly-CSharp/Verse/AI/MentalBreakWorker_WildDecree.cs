using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F6A RID: 3946
	public class MentalBreakWorker_WildDecree : MentalBreakWorker
	{
		// Token: 0x06006188 RID: 24968 RVA: 0x001EDE30 File Offset: 0x001EC030
		public override bool BreakCanOccur(Pawn pawn)
		{
			return base.BreakCanOccur(pawn) && pawn.IsColonist && !pawn.IsPrisoner && pawn.royalty != null && pawn.royalty.PossibleDecreeQuests.Any<QuestScriptDef>();
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x001EDE68 File Offset: 0x001EC068
		public override float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			if (pawn.royalty == null)
			{
				return 0f;
			}
			float num = 0f;
			List<RoyalTitle> allTitlesInEffectForReading = pawn.royalty.AllTitlesInEffectForReading;
			for (int i = 0; i < allTitlesInEffectForReading.Count; i++)
			{
				num = Mathf.Max(num, allTitlesInEffectForReading[i].def.decreeMentalBreakCommonality);
			}
			return num;
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x001EDEC0 File Offset: 0x001EC0C0
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.royalty.IssueDecree(true, reason);
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_OwnRoom, null, false, false, causedByMood, null, true, false, false);
			}
			else if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, causedByMood, null, true, false, false);
			}
			return true;
		}
	}
}
