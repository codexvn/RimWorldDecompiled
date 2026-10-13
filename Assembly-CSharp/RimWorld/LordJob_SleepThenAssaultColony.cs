using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001678 RID: 5752
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x0600878C RID: 34700 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600878D RID: 34701 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x0600878E RID: 34702 RVA: 0x00287A04 File Offset: 0x00285C04
		public LordJob_SleepThenAssaultColony(Faction faction, bool sendWokenUpMessage = true)
		{
			this.faction = faction;
			this.sendWokenUpMessage = sendWokenUpMessage;
		}

		// Token: 0x0600878F RID: 34703 RVA: 0x00287A1C File Offset: 0x00285C1C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			if (this.sendWokenUpMessage)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					vector += this.lord.ownedPawns[i].Position.ToVector3();
				}
				vector /= (float)this.lord.ownedPawns.Count;
				if (this.faction == Faction.OfMechanoids)
				{
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
					return;
				}
				if (ModsConfig.IdeologyActive && this.faction == Faction.OfInsects)
				{
					SoundDefOf.InsectsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
				}
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06008790 RID: 34704 RVA: 0x00287B20 File Offset: 0x00285D20
		private bool AnyAsleep()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].Spawned && !this.lord.ownedPawns[i].Dead && !this.lord.ownedPawns[i].Awake())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008791 RID: 34705 RVA: 0x00287B93 File Offset: 0x00285D93
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

		// Token: 0x06008792 RID: 34706 RVA: 0x00287BBE File Offset: 0x00285DBE
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.sendWokenUpMessage, "sendWokenUpMessage", true, false);
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
		}

		// Token: 0x04005881 RID: 22657
		private Faction faction;

		// Token: 0x04005882 RID: 22658
		private bool sendWokenUpMessage;

		// Token: 0x04005883 RID: 22659
		public bool awakeOnClamor;
	}
}
