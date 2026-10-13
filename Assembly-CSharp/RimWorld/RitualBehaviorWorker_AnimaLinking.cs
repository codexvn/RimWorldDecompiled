using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D2 RID: 9938
	public class RitualBehaviorWorker_AnimaLinking : RitualBehaviorWorker
	{
		// Token: 0x0600D9DB RID: 55771 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_AnimaLinking()
		{
		}

		// Token: 0x0600D9DC RID: 55772 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_AnimaLinking(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9DD RID: 55773 RVA: 0x004077CC File Offset: 0x004059CC
		public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			float value = RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			TaggedString taggedString = "AnimaLinkingExplanationBase".Translate(count, value);
			if (assignments.ExtraRequiredPawnsForReading.Any<Pawn>())
			{
				TaggedString psylinkAffectedByTraitsNegativelyWarning = RoyalTitleUtility.GetPsylinkAffectedByTraitsNegativelyWarning(assignments.ExtraRequiredPawnsForReading.FirstOrDefault<Pawn>());
				if (psylinkAffectedByTraitsNegativelyWarning.RawText != null)
				{
					taggedString += "\n\n" + psylinkAffectedByTraitsNegativelyWarning.Resolve();
				}
			}
			return taggedString;
		}

		// Token: 0x0600D9DE RID: 55774 RVA: 0x00407850 File Offset: 0x00405A50
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			int count = assignments.SpectatorsForReading.Count;
			return Mathf.RoundToInt((float)ritual.behavior.def.durationTicks.max / RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)(count + 1))).ToStringTicksToPeriod(false, false, true, true, false);
		}
	}
}
