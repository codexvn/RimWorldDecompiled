using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001633 RID: 5683
	public class LordJob_AssaultColony : LordJob, ILordAvoidTraps
	{
		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x060085F4 RID: 34292 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x060085F5 RID: 34293 RVA: 0x0027FB34 File Offset: 0x0027DD34
		public float AvoidTrapRatio
		{
			get
			{
				if (!this.useAvoidGridSmart)
				{
					return 0f;
				}
				return 0.3f;
			}
		}

		// Token: 0x060085F6 RID: 34294 RVA: 0x0027FB49 File Offset: 0x0027DD49
		public LordJob_AssaultColony()
		{
		}

		// Token: 0x060085F7 RID: 34295 RVA: 0x0027FB68 File Offset: 0x0027DD68
		public LordJob_AssaultColony(SpawnedPawnParams parms)
		{
			this.assaulterFaction = parms.spawnerThing.Faction;
			this.canKidnap = false;
			this.canTimeoutOrFlee = false;
			this.canSteal = false;
		}

		// Token: 0x060085F8 RID: 34296 RVA: 0x0027FBB8 File Offset: 0x0027DDB8
		public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true, bool breachers = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.canKidnap = canKidnap;
			this.canTimeoutOrFlee = canTimeoutOrFlee;
			this.sappers = sappers;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.canSteal = canSteal;
			this.breachers = breachers;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x0027FC20 File Offset: 0x0027DE20
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			List<LordToil> list = new List<LordToil>();
			LordToil lordToil = null;
			if (this.sappers)
			{
				lordToil = new LordToil_AssaultColonySappers();
				if (this.useAvoidGridSmart)
				{
					lordToil.useAvoidGrid = true;
				}
				stateGraph.AddToil(lordToil);
				list.Add(lordToil);
				Transition transition = new Transition(lordToil, lordToil, true, true);
				transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition, false);
			}
			if (this.breachers)
			{
				LordToil lordToil2 = new LordToil_AssaultColonyBreaching();
				if (this.useAvoidGridSmart)
				{
					lordToil2.useAvoidGrid = this.useAvoidGridSmart;
				}
				stateGraph.AddToil(lordToil2);
				list.Add(lordToil2);
			}
			LordToil lordToil3 = new LordToil_AssaultColony(false, this.canPickUpOpportunisticWeapons);
			if (this.useAvoidGridSmart)
			{
				lordToil3.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil3);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.sappers)
			{
				Transition transition2 = new Transition(lordToil, lordToil3, false, true);
				transition2.AddTrigger(new Trigger_NoFightingSappers());
				stateGraph.AddTransition(transition2, false);
			}
			if (this.assaulterFaction != null && this.assaulterFaction.def.humanlikeFaction)
			{
				if (this.canTimeoutOrFlee)
				{
					Transition transition3 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition3.AddSources(list);
					IntRange intRange;
					if (this.sappers)
					{
						intRange = LordJob_AssaultColony.SapTimeBeforeGiveUp;
					}
					else if (this.breachers)
					{
						intRange = LordJob_AssaultColony.BreachTimeBeforeGiveUp;
					}
					else
					{
						intRange = LordJob_AssaultColony.AssaultTimeBeforeGiveUp;
					}
					Trigger_TicksPassed trigger_TicksPassed = new Trigger_TicksPassed(intRange.RandomInRange);
					trigger_TicksPassed.WithFilter(new TriggerFilter_MapExitable());
					transition3.AddTrigger(trigger_TicksPassed);
					transition3.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition3, false);
					Transition transition4 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition4.AddSources(list);
					FloatRange floatRange = new FloatRange(0.25f, 0.35f);
					Trigger_FractionColonyDamageTaken trigger_FractionColonyDamageTaken = new Trigger_FractionColonyDamageTaken(floatRange.RandomInRange, 900f);
					trigger_FractionColonyDamageTaken.WithFilter(new TriggerFilter_MapExitable());
					transition4.AddTrigger(trigger_FractionColonyDamageTaken);
					transition4.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition4, false);
				}
				if (this.canKidnap)
				{
					LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
					Transition transition5 = new Transition(lordToil3, startingToil, false, true);
					transition5.AddSources(list);
					transition5.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition5.AddTrigger(new Trigger_KidnapVictimPresent());
					stateGraph.AddTransition(transition5, false);
				}
				if (this.canSteal)
				{
					LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
					Transition transition6 = new Transition(lordToil3, startingToil2, false, true);
					transition6.AddSources(list);
					transition6.AddPreAction(new TransitionAction_Message("MessageRaidersStealing".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition6.AddTrigger(new Trigger_HighValueThingsAround());
					stateGraph.AddTransition(transition6, false);
				}
			}
			if (this.assaulterFaction != null && this.assaulterFaction == Faction.OfMechanoids)
			{
				Transition transition7 = new Transition(lordToil3, lordToil_ExitMap, false, true);
				transition7.AddSources(list);
				transition7.AddTrigger(new Trigger_GameEnding(2500));
				transition7.AddPreAction(new TransitionAction_Message("MechanoidsMovingOn".Translate(), null, 1f));
				stateGraph.AddTransition(transition7, false);
			}
			if (this.assaulterFaction != null)
			{
				Transition transition8 = new Transition(lordToil3, lordToil_ExitMap, false, true);
				transition8.AddSources(list);
				transition8.AddTrigger(new Trigger_BecameNonHostileToPlayer());
				transition8.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
				stateGraph.AddTransition(transition8, false);
			}
			return stateGraph;
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x002800B8 File Offset: 0x0027E2B8
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
			Scribe_Values.Look<bool>(ref this.breachers, "breaching", false, false);
			Scribe_Values.Look<bool>(ref this.canPickUpOpportunisticWeapons, "canPickUpOpportunisticWeapons", false, false);
		}

		// Token: 0x0400577F RID: 22399
		private Faction assaulterFaction;

		// Token: 0x04005780 RID: 22400
		private bool canKidnap = true;

		// Token: 0x04005781 RID: 22401
		private bool canTimeoutOrFlee = true;

		// Token: 0x04005782 RID: 22402
		private bool sappers;

		// Token: 0x04005783 RID: 22403
		private bool useAvoidGridSmart;

		// Token: 0x04005784 RID: 22404
		private bool canSteal = true;

		// Token: 0x04005785 RID: 22405
		private bool breachers;

		// Token: 0x04005786 RID: 22406
		private bool canPickUpOpportunisticWeapons;

		// Token: 0x04005787 RID: 22407
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

		// Token: 0x04005788 RID: 22408
		private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);

		// Token: 0x04005789 RID: 22409
		private static readonly IntRange BreachTimeBeforeGiveUp = new IntRange(33000, 38000);
	}
}
