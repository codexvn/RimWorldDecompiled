using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016AD RID: 5805
	public class PsychicRitualDef_VoidProvocation : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088D6 RID: 35030 RVA: 0x0028CE03 File Offset: 0x0028B003
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_VoidProvocation(this.InvokerRole, this.psychicShockChanceFromQualityCurve));
			return list;
		}

		// Token: 0x060088D7 RID: 35031 RVA: 0x0028CE24 File Offset: 0x0028B024
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(this.psychicShockChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
		}

		// Token: 0x060088D8 RID: 35032 RVA: 0x0028CE4C File Offset: 0x0028B04C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04005946 RID: 22854
		public SimpleCurve psychicShockChanceFromQualityCurve;

		// Token: 0x04005947 RID: 22855
		public FloatRange darkPsychicShockDurarionHoursRange;

		// Token: 0x04005948 RID: 22856
		public FloatRange incidentDelayHoursRange;
	}
}
