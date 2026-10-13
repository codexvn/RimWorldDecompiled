using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200169C RID: 5788
	public class PsychicRitualDef_NeurosisPulse : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600887E RID: 34942 RVA: 0x0028BF9F File Offset: 0x0028A19F
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_NeurosisPulse(this.InvokerRole));
			return list;
		}

		// Token: 0x0600887F RID: 34943 RVA: 0x0028BFBA File Offset: 0x0028A1BA
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x0400590C RID: 22796
		public SimpleCurve durationDaysFromQualityCurve;
	}
}
