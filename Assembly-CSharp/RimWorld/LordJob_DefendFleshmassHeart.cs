using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164F RID: 5711
	public class LordJob_DefendFleshmassHeart : LordJob
	{
		// Token: 0x060086A6 RID: 34470 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendFleshmassHeart()
		{
		}

		// Token: 0x060086A7 RID: 34471 RVA: 0x002834B5 File Offset: 0x002816B5
		public LordJob_DefendFleshmassHeart(Building_FleshmassHeart heart)
		{
			this.heart = heart;
		}

		// Token: 0x060086A8 RID: 34472 RVA: 0x002834C4 File Offset: 0x002816C4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendFleshmassHeart(this.heart));
			return stateGraph;
		}

		// Token: 0x060086A9 RID: 34473 RVA: 0x002834DC File Offset: 0x002816DC
		public override void ExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x040057F4 RID: 22516
		private Building_FleshmassHeart heart;
	}
}
