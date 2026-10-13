using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200168F RID: 5775
	public class PsychicRitualDef_Brainwipe : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x0600881F RID: 34847 RVA: 0x0028A4A8 File Offset: 0x002886A8
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_Brainwipe(this.InvokerRole, this.TargetRole));
			list.Add(new PsychicRitualToil_TargetCleanup(this.InvokerRole, this.TargetRole));
			return list;
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x0028A4E0 File Offset: 0x002886E0
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Mathf.FloorToInt(this.comaDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays("F1"));
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x0028A518 File Offset: 0x00288718
		public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			if (pawn.guest != null && !pawn.guest.Recruitable)
			{
				yield return "NonRecruitable".Translate();
			}
			yield break;
		}

		// Token: 0x040058C6 RID: 22726
		public SimpleCurve comaDurationDaysFromQualityCurve;
	}
}
