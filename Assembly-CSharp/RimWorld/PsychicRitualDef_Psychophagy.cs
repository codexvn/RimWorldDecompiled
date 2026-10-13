using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A2 RID: 5794
	public class PsychicRitualDef_Psychophagy : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600889F RID: 34975 RVA: 0x0028C502 File Offset: 0x0028A702
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Psychophagy(this.InvokerRole, this.TargetRole, this.brainDamageRange));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x060088A0 RID: 34976 RVA: 0x0028C540 File Offset: 0x0028A740
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			string value = Mathf.FloorToInt(this.effectDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1");
			IntRange disappearsAfterTicks = HediffDefOf.DarkPsychicShock.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks;
			FloatRange floatRange = new FloatRange((float)Mathf.FloorToInt(disappearsAfterTicks.min.TicksToDays()), (float)Mathf.FloorToInt(disappearsAfterTicks.max.TicksToDays()));
			return this.outcomeDescription.Formatted(value, floatRange.ToString(), this.brainDamageRange.ToString());
		}

		// Token: 0x04005921 RID: 22817
		public FloatRange brainDamageRange;

		// Token: 0x04005922 RID: 22818
		public SimpleCurve effectDurationDaysFromQualityCurve;
	}
}
