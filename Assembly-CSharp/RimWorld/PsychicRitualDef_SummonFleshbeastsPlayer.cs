using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A8 RID: 5800
	public class PsychicRitualDef_SummonFleshbeastsPlayer : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088B9 RID: 35001 RVA: 0x0028C948 File Offset: 0x0028AB48
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
			list.Add(new PsychicRitualToil_SummonFleshbeastsPlayer(this.InvokerRole));
			return list;
		}

		// Token: 0x04005930 RID: 22832
		public SimpleCurve fleshbeastCombatPointsFromQualityCurve;
	}
}
