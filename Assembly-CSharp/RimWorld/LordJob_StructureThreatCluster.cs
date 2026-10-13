using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200167D RID: 5757
	public class LordJob_StructureThreatCluster : LordJob
	{
		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x060087A9 RID: 34729 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060087AA RID: 34730 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_StructureThreatCluster()
		{
		}

		// Token: 0x060087AB RID: 34731 RVA: 0x002881EF File Offset: 0x002863EF
		public LordJob_StructureThreatCluster(Faction faction, IntVec3 position, float wanderRadius, bool sendWokenUpMessage = true, bool awakeOnClamor = false)
		{
			this.faction = faction;
			this.sendWokenUpMessage = sendWokenUpMessage;
			this.position = position;
			this.wanderRadius = wanderRadius;
			this.awakeOnClamor = awakeOnClamor;
		}

		// Token: 0x060087AC RID: 34732 RVA: 0x0028821C File Offset: 0x0028641C
		protected virtual LordToil GetIdleToil()
		{
			return new LordToil_Sleep();
		}

		// Token: 0x060087AD RID: 34733 RVA: 0x00288224 File Offset: 0x00286424
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil idleToil = this.GetIdleToil();
			stateGraph.StartingToil = idleToil;
			IntVec3 defendPoint = this.position;
			float? num = new float?(this.wanderRadius);
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(defendPoint, new float?(this.wanderRadius), num);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_HuntDownColonists lordToil_HuntDownColonists = new LordToil_HuntDownColonists();
			stateGraph.AddToil(lordToil_HuntDownColonists);
			Transition transition = new Transition(idleToil, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			if (this.sendWokenUpMessage)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(this.WakeUpPostAction());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(idleToil, lordToil_HuntDownColonists, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition2.AddTrigger(new Trigger_AcquiredTarget(Faction.OfPlayer));
			if (this.sendWokenUpMessage)
			{
				transition2.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition2.AddPostAction(new TransitionAction_WakeAll());
			transition2.AddPostAction(this.WakeUpPostAction());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendPoint, lordToil_HuntDownColonists, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition3.AddTrigger(new Trigger_AcquiredTarget(Faction.OfPlayer));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_HuntDownColonists, lordToil_DefendPoint, false, true);
			transition4.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x060087AE RID: 34734 RVA: 0x0028843E File Offset: 0x0028663E
		private TransitionAction_Custom WakeUpPostAction()
		{
			return new TransitionAction_Custom(delegate()
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
			});
		}

		// Token: 0x060087AF RID: 34735 RVA: 0x00288454 File Offset: 0x00286654
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

		// Token: 0x060087B0 RID: 34736 RVA: 0x00287B93 File Offset: 0x00285D93
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

		// Token: 0x060087B1 RID: 34737 RVA: 0x002884C8 File Offset: 0x002866C8
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.sendWokenUpMessage, "sendWokenUpMessage", true, false);
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.wanderRadius, "wanderRadius", 0f, false);
		}

		// Token: 0x0400588F RID: 22671
		private Faction faction;

		// Token: 0x04005890 RID: 22672
		private bool sendWokenUpMessage;

		// Token: 0x04005891 RID: 22673
		public bool awakeOnClamor;

		// Token: 0x04005892 RID: 22674
		public IntVec3 position;

		// Token: 0x04005893 RID: 22675
		public float wanderRadius;
	}
}
