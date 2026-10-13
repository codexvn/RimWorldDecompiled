using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A1 RID: 5793
	public class PsychicRitualDef_PleasurePulse : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600889C RID: 34972 RVA: 0x0028C4AF File Offset: 0x0028A6AF
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_PleasurePulse(this.InvokerRole));
			return list;
		}

		// Token: 0x0600889D RID: 34973 RVA: 0x0028C4CA File Offset: 0x0028A6CA
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x04005920 RID: 22816
		public SimpleCurve durationDaysFromQualityCurve;
	}
}
