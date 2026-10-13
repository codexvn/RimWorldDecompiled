using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A4 RID: 5796
	public class PsychicRitualDef_SkipAbductionPlayer : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088A4 RID: 34980 RVA: 0x0028C601 File Offset: 0x0028A801
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_SkipAbductionPlayer(this.InvokerRole));
			return list;
		}

		// Token: 0x060088A5 RID: 34981 RVA: 0x0028C61C File Offset: 0x0028A81C
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			string value = Mathf.FloorToInt(this.comaDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1");
			return this.outcomeDescription.Formatted(value);
		}

		// Token: 0x04005923 RID: 22819
		public SimpleCurve comaDurationDaysFromQualityCurve;
	}
}
