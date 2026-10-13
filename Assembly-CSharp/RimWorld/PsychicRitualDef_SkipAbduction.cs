using System;
using System.Collections.Generic;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A3 RID: 5795
	public class PsychicRitualDef_SkipAbduction : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x060088A2 RID: 34978 RVA: 0x0028C5E6 File Offset: 0x0028A7E6
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SkipAbduction(this.InvokerRole));
			return list;
		}
	}
}
