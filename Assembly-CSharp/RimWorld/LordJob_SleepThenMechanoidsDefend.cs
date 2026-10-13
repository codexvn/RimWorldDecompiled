using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001679 RID: 5753
	public class LordJob_SleepThenMechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x06008795 RID: 34709 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008796 RID: 34710 RVA: 0x00285C04 File Offset: 0x00283E04
		public LordJob_SleepThenMechanoidsDefend()
		{
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x00287CE7 File Offset: 0x00285EE7
		public LordJob_SleepThenMechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			if (things != null)
			{
				this.things.AddRange(things);
			}
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x00287D24 File Offset: 0x00285F24
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_MechanoidsDefend(this.things, this.faction, this.defendRadius, this.defSpot, this.canAssaultColony, this.isMechCluster).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			transition.AddTrigger(new Trigger_OnHumanlikeHarmAnyThing(this.things));
			transition.AddTrigger(new Trigger_OnPlayerMechHarmAnything(this.things));
			transition.AddTrigger(new Trigger_PawnHarmed(1f, true, null, null, null));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Find.SignalManager.SendSignal(new Signal("CompCanBeDormant.WakeUp", this.things.First<Thing>().Named("SUBJECT"), Faction.OfMechanoids.Named("FACTION")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.defSpot, base.Map, false));
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x00287B93 File Offset: 0x00285D93
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			if (!p.Dead)
			{
				CompCanBeDormant compCanBeDormant = p.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null && !compCanBeDormant.Awake)
				{
					return false;
				}
			}
			return base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x0600879A RID: 34714 RVA: 0x00287E4A File Offset: 0x0028604A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
		}

		// Token: 0x04005884 RID: 22660
		public bool awakeOnClamor;
	}
}
