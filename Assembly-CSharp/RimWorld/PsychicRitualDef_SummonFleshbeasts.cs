using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A7 RID: 5799
	public class PsychicRitualDef_SummonFleshbeasts : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088B6 RID: 34998 RVA: 0x0028C8DB File Offset: 0x0028AADB
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonFleshbeastsCultist(this.InvokerRole));
			return list;
		}

		// Token: 0x060088B7 RID: 34999 RVA: 0x0028C8F8 File Offset: 0x0028AAF8
		public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			Pawn pawn = assignments.FirstAssignedPawn(this.InvokerRole);
			object obj;
			if (pawn == null)
			{
				obj = null;
			}
			else
			{
				Lord lord = pawn.GetLord();
				obj = ((lord != null) ? lord.LordJob : null);
			}
			LordJob_PsychicRitual lordJob_PsychicRitual = obj as LordJob_PsychicRitual;
			if (lordJob_PsychicRitual != null)
			{
				power = Mathf.Max(power, lordJob_PsychicRitual.points);
			}
		}

		// Token: 0x0400592F RID: 22831
		public SimpleCurve fleshbeastPointsFromThreatPointsCurve;
	}
}
