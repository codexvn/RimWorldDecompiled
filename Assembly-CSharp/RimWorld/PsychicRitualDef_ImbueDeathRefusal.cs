using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001694 RID: 5780
	public class PsychicRitualDef_ImbueDeathRefusal : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06008842 RID: 34882 RVA: 0x0028AABB File Offset: 0x00288CBB
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_ImbueDeathRefusal(this.TargetRole));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06008843 RID: 34883 RVA: 0x0028AAED File Offset: 0x00288CED
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.Abs(this.skillOffsetPercentFromQualityCurve.Evaluate(qualityRange.min)).ToStringPercent());
		}

		// Token: 0x040058DD RID: 22749
		public SimpleCurve skillOffsetPercentFromQualityCurve;
	}
}
