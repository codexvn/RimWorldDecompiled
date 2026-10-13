using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF3 RID: 7155
	public class QuestPart_DefendPoint : QuestPart_MakeLord
	{
		// Token: 0x06009F54 RID: 40788 RVA: 0x002E00C8 File Offset: 0x002DE2C8
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_DefendPoint(this.point, this.wanderRadius, this.defendRadius, this.isCaravanSendable, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06009F55 RID: 40789 RVA: 0x002E0100 File Offset: 0x002DE300
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<float?>(ref this.defendRadius, "defendRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04006E2C RID: 28204
		public IntVec3 point;

		// Token: 0x04006E2D RID: 28205
		public float? wanderRadius;

		// Token: 0x04006E2E RID: 28206
		public float? defendRadius;

		// Token: 0x04006E2F RID: 28207
		public bool isCaravanSendable;

		// Token: 0x04006E30 RID: 28208
		public bool addFleeToil = true;
	}
}
