using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001660 RID: 5728
	public class LordJob_LoadAndEnterPortal : LordJob
	{
		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x0600870C RID: 34572 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x0600870D RID: 34573 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowStartNewRituals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x0600870E RID: 34574 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600870F RID: 34575 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_LoadAndEnterPortal()
		{
		}

		// Token: 0x06008710 RID: 34576 RVA: 0x002857FB File Offset: 0x002839FB
		public LordJob_LoadAndEnterPortal(MapPortal portal)
		{
			this.portal = portal;
		}

		// Token: 0x06008711 RID: 34577 RVA: 0x0028580A File Offset: 0x00283A0A
		public override void ExposeData()
		{
			Scribe_References.Look<MapPortal>(ref this.portal, "portal", false);
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x00285820 File Offset: 0x00283A20
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterPortal startingToil = new LordToil_LoadAndEnterPortal(this.portal);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x0400583D RID: 22589
		public MapPortal portal;
	}
}
