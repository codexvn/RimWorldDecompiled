using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001691 RID: 5777
	public class PsychicRitualDef_Chronophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600882B RID: 34859 RVA: 0x0028A603 File Offset: 0x00288803
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Chronophagy(this.InvokerRole, this.TargetRole, this.yearsTransferredFromQualityRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x0600882C RID: 34860 RVA: 0x0028A641 File Offset: 0x00288841
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			foreach (string text in base.BlockingIssues(assignments, map))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (assignments.FirstAssignedPawn(this.invokerRole).ageTracker.AgeBiologicalYears < 13)
			{
				yield return "InvokerTooYoung".Translate(13);
			}
			if (assignments.FirstAssignedPawn(this.targetRole).ageTracker.AgeBiologicalYears < 13)
			{
				yield return "PsychicRitualTargetTooYoung".Translate(13);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600882D RID: 34861 RVA: 0x0028A660 File Offset: 0x00288860
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			float num = this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min);
			Pawn pawn = assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = assignments.FirstAssignedPawn(this.targetRole);
			if (pawn != null)
			{
				float num2 = Mathf.Max(pawn.ageTracker.AgeBiologicalYearsFloat - num, 13f);
				float num3 = pawn.ageTracker.AgeBiologicalYearsFloat - num2;
				TaggedString taggedString = this.outcomeDescription.Formatted(num3, this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
				if (pawn2 != null)
				{
					taggedString += "\n\n" + "CronophagyAgeChange".Translate(pawn.Named("INVOKER"), pawn.ageTracker.AgeBiologicalYearsFloat - num3, pawn2.Named("TARGET"), pawn2.ageTracker.AgeBiologicalYearsFloat + this.yearsTransferredFromQualityRange.LerpThroughRange(qualityRange.min));
				}
				return taggedString;
			}
			return this.outcomeDescription.Formatted(num, num);
		}

		// Token: 0x0600882E RID: 34862 RVA: 0x0028A775 File Offset: 0x00288975
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			yield return "Stat_Age_Label".Translate() + ": " + pawn.ageTracker.AgeBiologicalYears.ToString();
			yield break;
		}

		// Token: 0x040058CC RID: 22732
		public FloatRange yearsTransferredFromQualityRange;

		// Token: 0x040058CD RID: 22733
		public SimpleCurve brainDamageFromAgeCurve;

		// Token: 0x040058CE RID: 22734
		public const int MinimumAge = 13;
	}
}
