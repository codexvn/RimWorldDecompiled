using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BFE RID: 7166
	public class QuestPart_WaitForEscort : QuestPart_MakeLord
	{
		// Token: 0x06009F82 RID: 40834 RVA: 0x002E0B2B File Offset: 0x002DED2B
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForEscort(this.point, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06009F83 RID: 40835 RVA: 0x002E0B50 File Offset: 0x002DED50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04006E59 RID: 28249
		public IntVec3 point;

		// Token: 0x04006E5A RID: 28250
		public bool addFleeToil = true;
	}
}
