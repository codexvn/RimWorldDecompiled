using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001661 RID: 5729
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x06008713 RID: 34579 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x06008714 RID: 34580 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowStartNewRituals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x06008715 RID: 34581 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008716 RID: 34582 RVA: 0x00285852 File Offset: 0x00283A52
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x06008717 RID: 34583 RVA: 0x00285861 File Offset: 0x00283A61
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06008718 RID: 34584 RVA: 0x00285877 File Offset: 0x00283A77
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x06008719 RID: 34585 RVA: 0x0028588C File Offset: 0x00283A8C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x0400583E RID: 22590
		public int transportersGroup = -1;
	}
}
