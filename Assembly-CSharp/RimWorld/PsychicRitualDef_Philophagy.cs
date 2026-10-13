using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200169D RID: 5789
	public class PsychicRitualDef_Philophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06008881 RID: 34945 RVA: 0x0028BFF2 File Offset: 0x0028A1F2
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Philophagy(this.InvokerRole, this.TargetRole, this.brainDamageRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06008882 RID: 34946 RVA: 0x0028C030 File Offset: 0x0028A230
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			float num = this.xpTransferFromQualityCurve.Evaluate(qualityRange.min);
			string value = num.ToStringPercent();
			Pawn pawn = assignments.FirstAssignedPawn(this.InvokerRole);
			Pawn pawn2 = assignments.FirstAssignedPawn(this.TargetRole);
			TaggedString taggedString = this.outcomeDescription.Formatted(value);
			if (pawn == null || pawn2 == null)
			{
				return taggedString;
			}
			float value2;
			SkillDef philophagySkillAndXpTransfer = PsychicRitualUtility.GetPhilophagySkillAndXpTransfer(pawn, pawn2, num, out value2);
			if (philophagySkillAndXpTransfer != null)
			{
				taggedString += "\n\n" + "PhilophagyTransferReport".Translate(pawn.Named("INVOKER"), value2, philophagySkillAndXpTransfer.Named("SKILL"), pawn2.Named("TARGET"));
			}
			return taggedString;
		}

		// Token: 0x06008883 RID: 34947 RVA: 0x0028C0E4 File Offset: 0x0028A2E4
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			foreach (string text in base.BlockingIssues(assignments, map))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			Pawn pawn = assignments.FirstAssignedPawn(this.TargetRole);
			if (pawn != null)
			{
				if (!pawn.skills.skills.Any((SkillRecord s) => !s.TotallyDisabled))
				{
					yield return "NoSkillsToTransfer".Translate(pawn.Named("TARGET"));
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06008884 RID: 34948 RVA: 0x0028C102 File Offset: 0x0028A302
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			if (pawn.skills != null)
			{
				SkillRecord skillRecord = pawn.skills.skills.MaxBy((SkillRecord s) => s.XpTotalEarned);
				if (skillRecord.Level > 0)
				{
					yield return "HighestSkill".Translate() + ": " + skillRecord.def.skillLabel.CapitalizeFirst() + " (" + "SkillLevel".Translate() + " " + skillRecord.Level.ToString() + ")";
				}
			}
			yield break;
		}

		// Token: 0x0400590D RID: 22797
		public FloatRange brainDamageRange;

		// Token: 0x0400590E RID: 22798
		public SimpleCurve xpTransferFromQualityCurve;
	}
}
