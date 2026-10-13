using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001651 RID: 5713
	public abstract class LordJob_EntitySwarm : LordJob
	{
		// Token: 0x060086AC RID: 34476 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_EntitySwarm()
		{
		}

		// Token: 0x060086AD RID: 34477 RVA: 0x0028350F File Offset: 0x0028170F
		public LordJob_EntitySwarm(IntVec3 startPos, IntVec3 destPos)
		{
			this.startPos = startPos;
			this.destPos = destPos;
		}

		// Token: 0x060086AE RID: 34478 RVA: 0x00283528 File Offset: 0x00281728
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = this.CreateTravelingToil(this.startPos, this.destPos);
			stateGraph.StartingToil = lordToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(lordToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x060086AF RID: 34479
		protected abstract LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest);

		// Token: 0x060086B0 RID: 34480 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DutyActiveWhenDown(Pawn pawn)
		{
			return true;
		}

		// Token: 0x060086B1 RID: 34481 RVA: 0x0028358C File Offset: 0x0028178C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.startPos, "startPos", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.destPos, "destPos", default(IntVec3), false);
		}

		// Token: 0x040057F5 RID: 22517
		private IntVec3 startPos;

		// Token: 0x040057F6 RID: 22518
		private IntVec3 destPos;
	}
}
