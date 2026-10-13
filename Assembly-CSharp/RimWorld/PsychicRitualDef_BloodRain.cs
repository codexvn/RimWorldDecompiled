using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200168E RID: 5774
	public class PsychicRitualDef_BloodRain : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600881C RID: 34844 RVA: 0x0028A447 File Offset: 0x00288647
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_BloodRain(this.InvokerRole, this.durationHoursFromQualityRange));
			return list;
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x0028A468 File Offset: 0x00288668
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.durationHoursFromQualityRange.LerpThroughRange(qualityRange.min) * 2500f).ToStringTicksToPeriod(true, false, true, true, false));
		}

		// Token: 0x040058C5 RID: 22725
		private FloatRange durationHoursFromQualityRange;
	}
}
