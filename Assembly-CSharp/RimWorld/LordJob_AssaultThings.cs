using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001634 RID: 5684
	public class LordJob_AssaultThings : LordJob
	{
		// Token: 0x060085FC RID: 34300 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_AssaultThings()
		{
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x00280192 File Offset: 0x0027E392
		public LordJob_AssaultThings(Faction assaulterFaction, List<Thing> things, float damageFraction = 1f, bool useAvoidGridSmart = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.things = things;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060085FE RID: 34302 RVA: 0x002801B8 File Offset: 0x0027E3B8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = new LordToil_AssaultThings(this.things);
			if (this.useAvoidGridSmart)
			{
				lordToil.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			lordToil_ExitMapAndDefendSelf.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			Transition transition = new Transition(lordToil, lordToil_ExitMapAndDefendSelf, false, true);
			transition.AddTrigger(new Trigger_ThingsDamageTaken(this.things, this.damageFraction));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x0028022C File Offset: 0x0027E42C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<float>(ref this.damageFraction, "damageFraction", 0f, false);
		}

		// Token: 0x0400578A RID: 22410
		private Faction assaulterFaction;

		// Token: 0x0400578B RID: 22411
		private List<Thing> things;

		// Token: 0x0400578C RID: 22412
		private bool useAvoidGridSmart;

		// Token: 0x0400578D RID: 22413
		private float damageFraction;
	}
}
