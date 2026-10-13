using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164E RID: 5710
	public class LordJob_DefendCerebrexCore : LordJob
	{
		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x060086A1 RID: 34465 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060086A2 RID: 34466 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendCerebrexCore()
		{
		}

		// Token: 0x060086A3 RID: 34467 RVA: 0x00283456 File Offset: 0x00281656
		public LordJob_DefendCerebrexCore(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x060086A4 RID: 34468 RVA: 0x00283468 File Offset: 0x00281668
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
		}

		// Token: 0x060086A5 RID: 34469 RVA: 0x00283490 File Offset: 0x00281690
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase startingToil = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = startingToil;
			return stateGraph;
		}

		// Token: 0x040057F3 RID: 22515
		private IntVec3 baseCenter;
	}
}
