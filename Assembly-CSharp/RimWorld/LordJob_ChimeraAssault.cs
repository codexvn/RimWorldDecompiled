using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001648 RID: 5704
	public class LordJob_ChimeraAssault : LordJob
	{
		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x0600867C RID: 34428 RVA: 0x0028296F File Offset: 0x00280B6F
		private bool InAttackMode
		{
			get
			{
				return this.lord.CurLordToil is LordToil_ChimeraAttack;
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x0600867D RID: 34429 RVA: 0x00282984 File Offset: 0x00280B84
		private bool CanSwitchMode
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.currentModeStartedTick) > 7500f;
			}
		}

		// Token: 0x0600867F RID: 34431 RVA: 0x002829A0 File Offset: 0x00280BA0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ChimeraStalk lordToil_ChimeraStalk = new LordToil_ChimeraStalk();
			stateGraph.StartingToil = lordToil_ChimeraStalk;
			LordToil_ChimeraAttack lordToil_ChimeraAttack = new LordToil_ChimeraAttack();
			stateGraph.AddToil(lordToil_ChimeraAttack);
			Transition transition = new Transition(lordToil_ChimeraStalk, lordToil_ChimeraAttack, false, true);
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				this.currentModeStartedTick = Find.TickManager.TicksGame;
				this.SendAttackingLetter();
			}));
			transition.triggers.Add(new Trigger_Memo("StalkToAttack"));
			transition.triggers.Add(new LordJob_ChimeraAssault.Trigger_ChimeraHarmed(1f, true, null, DutyDefOf.ChimeraStalkFlee, new int?(2500)));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ChimeraAttack, lordToil_ChimeraStalk, false, true);
			transition2.AddPostAction(new TransitionAction_Custom(delegate()
			{
				this.currentModeStartedTick = Find.TickManager.TicksGame;
				this.SendModeChangeMessage("MessageChimeraWithdrawing".Translate());
			}));
			transition2.triggers.Add(new Trigger_Memo("AttackToStalk"));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x00282A6E File Offset: 0x00280C6E
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x00282A78 File Offset: 0x00280C78
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.CanSwitchMode)
			{
				if (this.InAttackMode && Rand.MTBEventOccurs(8f, 2500f, 1f))
				{
					this.lord.ReceiveMemo("AttackToStalk");
					return;
				}
				if (!this.InAttackMode && Rand.MTBEventOccurs(0.7f, 60000f, 1f))
				{
					this.lord.ReceiveMemo("StalkToAttack");
				}
			}
		}

		// Token: 0x06008682 RID: 34434 RVA: 0x00282AF0 File Offset: 0x00280CF0
		public void SwitchMode()
		{
			if (this.lord == null)
			{
				return;
			}
			LordToil curLordToil = this.lord.CurLordToil;
			if (!(curLordToil is LordToil_ChimeraStalk))
			{
				if (!(curLordToil is LordToil_ChimeraAttack))
				{
					Log.Error(string.Format("Chimera lord job tried switching from a toil which is not handled {0}", this.lord.CurLordToil));
				}
				else
				{
					this.lord.ReceiveMemo("AttackToStalk");
				}
			}
			else
			{
				this.lord.ReceiveMemo("StalkToAttack");
			}
			this.currentModeStartedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06008683 RID: 34435 RVA: 0x00282B72 File Offset: 0x00280D72
		public override void Notify_PawnDowned(Pawn p)
		{
			base.Notify_PawnDowned(p);
			if (this.CanSwitchMode && Rand.Chance(0.075f) && this.lord.CurLordToil is LordToil_ChimeraAttack)
			{
				this.SwitchMode();
			}
		}

		// Token: 0x06008684 RID: 34436 RVA: 0x00282BA7 File Offset: 0x00280DA7
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (this.CanSwitchMode && condition == PawnLostCondition.Killed && Rand.Chance(0.075f) && this.lord.CurLordToil is LordToil_ChimeraAttack)
			{
				this.SwitchMode();
			}
		}

		// Token: 0x06008685 RID: 34437 RVA: 0x00282BDC File Offset: 0x00280DDC
		private void SendModeChangeMessage(string verb)
		{
			if (this.NoActivePawns())
			{
				return;
			}
			Messages.Message((this.lord.ownedPawns.Count > 1) ? "MessageChimeraModeChangePlural".Translate(verb) : "MessageChimeraModeChangeSingular".Translate(verb), new LookTargets(this.lord.ownedPawns), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06008686 RID: 34438 RVA: 0x00282C48 File Offset: 0x00280E48
		private void SendAttackingLetter()
		{
			if (this.NoActivePawns())
			{
				return;
			}
			TaggedString label = "LetterChimerasAttackingLabel".Translate();
			TaggedString text = "LetterChimerasAttacking".Translate();
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, new LookTargets(this.lord.ownedPawns), null, null, null, null, 0, true);
		}

		// Token: 0x06008687 RID: 34439 RVA: 0x00282C9C File Offset: 0x00280E9C
		private bool NoActivePawns()
		{
			if (this.lord.ownedPawns.Count == 0)
			{
				return true;
			}
			bool flag = false;
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				flag = !pawn.Downed;
			}
			return !flag;
		}

		// Token: 0x06008688 RID: 34440 RVA: 0x00282D14 File Offset: 0x00280F14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentModeStartedTick, "currentModeStartedTick", 0, false);
		}

		// Token: 0x040057E1 RID: 22497
		private int currentModeStartedTick;

		// Token: 0x040057E2 RID: 22498
		private const float StalkToAttackMTBDays = 0.7f;

		// Token: 0x040057E3 RID: 22499
		private const float AttackToStalkMTBHours = 8f;

		// Token: 0x040057E4 RID: 22500
		private const float ChanceToFleeOnDown = 0.075f;

		// Token: 0x040057E5 RID: 22501
		private const float MinTimeInModeTicks = 7500f;

		// Token: 0x040057E6 RID: 22502
		private const int MinTicksFleeing = 2500;

		// Token: 0x040057E7 RID: 22503
		public const string StalkToAttackMemo = "StalkToAttack";

		// Token: 0x040057E8 RID: 22504
		private const string AttackToStalkMemo = "AttackToStalk";

		// Token: 0x02001649 RID: 5705
		private class Trigger_ChimeraHarmed : Trigger_PawnHarmed
		{
			// Token: 0x0600868B RID: 34443 RVA: 0x00282D6D File Offset: 0x00280F6D
			public Trigger_ChimeraHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null, DutyDef skipDuty = null, int? minTicks = null) : base(chance, requireInstigatorWithFaction, requireInstigatorWithSpecificFaction, skipDuty, minTicks)
			{
			}

			// Token: 0x0600868C RID: 34444 RVA: 0x00282D7C File Offset: 0x00280F7C
			public override bool ActivateOn(Lord lord, TriggerSignal signal)
			{
				LordJob_ChimeraAssault lordJob_ChimeraAssault = lord.LordJob as LordJob_ChimeraAssault;
				return (lordJob_ChimeraAssault == null || lordJob_ChimeraAssault.CanSwitchMode) && base.ActivateOn(lord, signal);
			}
		}
	}
}
