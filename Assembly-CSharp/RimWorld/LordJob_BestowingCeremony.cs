using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200163F RID: 5695
	public class LordJob_BestowingCeremony : LordJob_Ritual
	{
		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06008630 RID: 34352 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06008631 RID: 34353 RVA: 0x002810A8 File Offset: 0x0027F2A8
		public override IntVec3 Spot
		{
			get
			{
				return this.targetSpot.Cell;
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x06008632 RID: 34354 RVA: 0x002810B8 File Offset: 0x0027F2B8
		public override string RitualLabel
		{
			get
			{
				return "BestowingCeremonyLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x06008633 RID: 34355 RVA: 0x002810DC File Offset: 0x0027F2DC
		public override bool AllowStartNewGatherings
		{
			get
			{
				return this.lord.CurLordToil != this.ceremonyToil;
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x06008634 RID: 34356 RVA: 0x002810F4 File Offset: 0x0027F2F4
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
				}
				return this.icon;
			}
		}

		// Token: 0x06008635 RID: 34357 RVA: 0x0028111B File Offset: 0x0027F31B
		public LordJob_BestowingCeremony()
		{
		}

		// Token: 0x06008636 RID: 34358 RVA: 0x0028113C File Offset: 0x0027F33C
		public LordJob_BestowingCeremony(Pawn bestower, Pawn target, LocalTargetInfo targetSpot, IntVec3 spotCell, Thing shuttle = null, string questEndedSignal = null)
		{
			this.bestower = bestower;
			this.target = target;
			this.targetSpot = targetSpot;
			this.spotCell = spotCell;
			this.shuttle = shuttle;
			this.questEndedSignal = questEndedSignal;
		}

		// Token: 0x06008637 RID: 34359 RVA: 0x00281194 File Offset: 0x0027F394
		public override AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.lord.CurLordToil == this.ceremonyToil)
			{
				return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, this.RitualLabel));
			}
			return true;
		}

		// Token: 0x06008638 RID: 34360 RVA: 0x002811E0 File Offset: 0x0027F3E0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			if (!ModLister.CheckRoyalty("Bestowing ceremony"))
			{
				return stateGraph;
			}
			this.outcome = (RitualOutcomeEffectWorker_Bestowing)RitualOutcomeEffectDefOf.BestowingCeremony.GetInstance();
			LordToil_Wait lordToil_Wait = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait);
			LordToil_Wait lordToil_Wait2 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait2);
			LordToil_Wait lordToil_Wait3 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait3);
			LordToil_Wait lordToil_Wait4 = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait4);
			LordToil_BestowingCeremony_MoveInPlace lordToil_BestowingCeremony_MoveInPlace = new LordToil_BestowingCeremony_MoveInPlace(this.spotCell, this.target);
			stateGraph.AddToil(lordToil_BestowingCeremony_MoveInPlace);
			LordToil_BestowingCeremony_Wait lordToil_BestowingCeremony_Wait = new LordToil_BestowingCeremony_Wait(this.target, this.bestower);
			stateGraph.AddToil(lordToil_BestowingCeremony_Wait);
			this.ceremonyToil = new LordToil_BestowingCeremony_Perform(this.target, this.bestower);
			stateGraph.AddToil(this.ceremonyToil);
			this.exitToil = ((this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.Jog, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Jog, true, true));
			stateGraph.AddToil(this.exitToil);
			TransitionAction_Custom action = new TransitionAction_Custom(delegate()
			{
				this.lord.RemovePawns(this.colonistParticipants);
			});
			Transition transition = new Transition(lordToil_Wait, lordToil_Wait2, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Wait2, lordToil_BestowingCeremony_MoveInPlace, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_BestowingCeremony_MoveInPlace, lordToil_BestowingCeremony_Wait, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Position == this.spotCell));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_BestowingCeremony_Wait, this.ceremonyToil, false, true);
			transition4.AddTrigger(new Trigger_Memo(LordJob_BestowingCeremony.MemoCeremonyStarted));
			transition4.postActions.Add(new TransitionAction_Custom(delegate()
			{
				this.ceremonyStarted = true;
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(this.ceremonyToil, lordToil_Wait4, false, true);
			transition5.AddTrigger(new Trigger_Custom((TriggerSignal s) => s.type == TriggerSignalType.Tick && this.bestower.InMentalState));
			transition5.AddPreAction(action);
			transition5.postActions.Add(new TransitionAction_Custom(delegate()
			{
				this.ceremonyStarted = false;
				this.lord.RemovePawn(this.target);
			}));
			transition5.AddPreAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingInterrupted".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
			}));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_Wait4, lordToil_BestowingCeremony_Wait, false, true);
			transition6.AddTrigger(new Trigger_Custom((TriggerSignal s) => s.type == TriggerSignalType.Tick && !this.bestower.InMentalState));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_BestowingCeremony_Wait, this.exitToil, false, true);
			transition7.AddTrigger(new Trigger_TicksPassed(30000));
			transition7.AddPreAction(action);
			transition7.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyExpired", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(this.ceremonyToil, this.exitToil, false, true);
			transition8.AddTrigger(new Trigger_Signal(this.questEndedSignal));
			transition8.AddPreAction(action);
			transition8.AddPreAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingInterrupted".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
			}));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = new Transition(this.ceremonyToil, lordToil_Wait3, false, true);
			transition9.AddTrigger(new Trigger_Memo("CeremonyFinished"));
			transition9.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyDone", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = new Transition(lordToil_Wait3, this.exitToil, false, true);
			transition10.AddPreAction(action);
			transition10.AddTrigger(new Trigger_TicksPassed(600));
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition11.AddSource(lordToil_BestowingCeremony_Wait);
			transition11.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition11.AddPreAction(action);
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition12.AddSource(lordToil_BestowingCeremony_Wait);
			transition12.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.bestower.Spawned && !this.bestower.CanReach(this.spotCell, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)));
			transition12.AddPreAction(action);
			transition12.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("MessageBestowingSpotUnreachable".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition12, false);
			if (!this.questEndedSignal.NullOrEmpty())
			{
				Transition transition13 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
				transition13.AddSource(lordToil_BestowingCeremony_Wait);
				transition13.AddSource(lordToil_Wait);
				transition13.AddSource(lordToil_Wait2);
				transition13.AddTrigger(new Trigger_Signal(this.questEndedSignal));
				transition13.AddPreAction(action);
				stateGraph.AddTransition(transition13, false);
			}
			Transition transition14 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition14.AddSource(lordToil_BestowingCeremony_Wait);
			transition14.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick && !this.bestower.Dead)
				{
					Hediff firstHediffOfDef = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.35f)
					{
						return true;
					}
					Hediff firstHediffOfDef2 = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
					if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.35f)
					{
						return true;
					}
				}
				return false;
			}));
			transition14.AddPreAction(action);
			Func<Pawn, bool> <>9__15;
			transition14.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__15) == null)
					{
						pawnToLeaveBehind = (<>9__15 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				this.bestower.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, true, true, null, null);
				Messages.Message("MessageBestowingDangerTemperature".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition14, false);
			Transition transition15 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition15.AddSource(lordToil_BestowingCeremony_Wait);
			transition15.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick && !this.bestower.Dead)
				{
					if (ModsConfig.BiotechActive)
					{
						Hediff firstHediffOfDef = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxGasExposure, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.9f)
						{
							return true;
						}
					}
					Hediff firstHediffOfDef2 = this.bestower.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.LungRotExposure, false);
					if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.9f)
					{
						return true;
					}
				}
				return false;
			}));
			transition15.AddPreAction(action);
			Func<Pawn, bool> <>9__18;
			transition15.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__18) == null)
					{
						pawnToLeaveBehind = (<>9__18 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				this.bestower.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, true, true, null, null);
				Messages.Message("MessageBestowingDangerGas".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition15, false);
			Transition transition16 = new Transition(lordToil_BestowingCeremony_MoveInPlace, this.exitToil, false, true);
			transition16.AddSource(lordToil_BestowingCeremony_Wait);
			transition16.AddTrigger(new Trigger_PawnHarmed(1f, true, base.Map.ParentFaction, null, null));
			transition16.AddPreAction(action);
			Func<Pawn, bool> <>9__20;
			transition16.AddPostAction(new TransitionAction_Custom(delegate()
			{
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__20) == null)
					{
						pawnToLeaveBehind = (<>9__20 = ((Pawn p) => p != this.bestower));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				Messages.Message("MessageBestowingDanger".Translate(), this.bestower, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition16, false);
			return stateGraph;
		}

		// Token: 0x06008639 RID: 34361 RVA: 0x0028179C File Offset: 0x0027F99C
		public override void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
			if (stateDef != MentalStateDefOf.SocialFighting)
			{
				this.lord.Notify_PawnLost(pawn, PawnLostCondition.InMentalState, null);
			}
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x002817C8 File Offset: 0x0027F9C8
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.bestower || p == this.target)
			{
				this.MakeCeremonyFail();
			}
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x002817E2 File Offset: 0x0027F9E2
		public void MakeCeremonyFail()
		{
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
			this.outcome.ResetCompDatas();
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x0600863C RID: 34364 RVA: 0x00281814 File Offset: 0x0027FA14
		public override IEnumerable<Pawn> PawnsToCountTowardsPresence
		{
			get
			{
				return from p in this.lord.ownedPawns
				where p != this.bestower && p != this.target && p.IsColonist
				select p;
			}
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x00281834 File Offset: 0x0027FA34
		public override void LordJobTick()
		{
			if (this.ritual != null && this.ritual.behavior != null)
			{
				this.ritual.behavior.Tick(this);
			}
			if (this.ceremonyStarted)
			{
				RitualOutcomeEffectWorker_Bestowing ritualOutcomeEffectWorker_Bestowing = this.outcome;
				if (ritualOutcomeEffectWorker_Bestowing != null)
				{
					ritualOutcomeEffectWorker_Bestowing.Tick(this, 1f);
				}
			}
			if (this.lord.ownedPawns.Count == 0)
			{
				base.Map.lordManager.RemoveLord(this.lord);
			}
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x002818B0 File Offset: 0x0027FAB0
		private static bool CanUseSpot(Pawn bestower, IntVec3 spot)
		{
			return spot.InBounds(bestower.Map) && spot.Standable(bestower.Map) && GenSight.LineOfSight(spot, bestower.Position, bestower.Map) && bestower.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x0600863F RID: 34367 RVA: 0x0028190C File Offset: 0x0027FB0C
		public static IntVec3 TryGetUsableSpotAdjacentToBestower(Pawn bestower)
		{
			foreach (int num in Enumerable.Range(1, 4).InRandomOrder(null))
			{
				IntVec3 intVec = bestower.Position + GenRadial.ManualRadialPattern[num];
				if (LordJob_BestowingCeremony.CanUseSpot(bestower, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06008640 RID: 34368 RVA: 0x00281984 File Offset: 0x0027FB84
		public IntVec3 GetSpot()
		{
			IntVec3 result = IntVec3.Invalid;
			if (this.targetSpot.Thing != null)
			{
				IntVec3 interactionCell = this.targetSpot.Thing.InteractionCell;
				IntVec3 intVec = this.spotCell;
				foreach (IntVec3 intVec2 in GenSight.PointsOnLineOfSight(interactionCell, intVec))
				{
					if (!(intVec2 == interactionCell) && !(intVec2 == intVec) && LordJob_BestowingCeremony.CanUseSpot(this.bestower, intVec2))
					{
						result = intVec2;
						break;
					}
				}
			}
			if (result.IsValid)
			{
				return result;
			}
			return LordJob_BestowingCeremony.TryGetUsableSpotAdjacentToBestower(this.bestower);
		}

		// Token: 0x06008641 RID: 34369 RVA: 0x00281A38 File Offset: 0x0027FC38
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttending".Translate("BestowingCeremonyLabel".Translate());
		}

		// Token: 0x06008642 RID: 34370 RVA: 0x00281A58 File Offset: 0x0027FC58
		public void FinishCeremony(Pawn pawn)
		{
			this.lord.ReceiveMemo("CeremonyFinished");
			this.totalPresenceTmp.Clear();
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.ceremonyToil.Data.presentForTicks)
			{
				if (keyValuePair.Key != null && !keyValuePair.Key.Dead)
				{
					if (!this.totalPresenceTmp.ContainsKey(keyValuePair.Key))
					{
						this.totalPresenceTmp.Add(keyValuePair.Key, keyValuePair.Value);
					}
					else
					{
						Dictionary<Pawn, int> dictionary = this.totalPresenceTmp;
						Pawn key = keyValuePair.Key;
						dictionary[key] += keyValuePair.Value;
					}
				}
			}
			this.totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => tp.Value < 2500);
			this.outcome.Apply(1f, this.totalPresenceTmp, this);
			this.outcome.ResetCompDatas();
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x00281B84 File Offset: 0x0027FD84
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			if (p != this.bestower && p != this.target)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandLeaveBestowingCeremony".Translate(),
					defaultDesc = "CommandLeaveBestowingCeremonyDesc".Translate(),
					icon = this.Icon,
					action = delegate()
					{
						this.lord.Notify_PawnLost(p, PawnLostCondition.ForcedByPlayerAction, null);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					},
					hotKey = KeyBindingDefOf.Misc5
				};
			}
			else if (this.ceremonyStarted)
			{
				Action <>9__2;
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelBestowingCeremony".Translate(),
					defaultDesc = "CommandCancelBestowingCeremonyDesc".Translate(),
					icon = this.Icon,
					action = delegate()
					{
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "CommandCancelBestowingCeremonyConfirm".Translate();
						Action confirmedAct;
						if ((confirmedAct = <>9__2) == null)
						{
							confirmedAct = (<>9__2 = delegate()
							{
								QuestUtility.SendQuestTargetSignals(this.lord.questTags, "CeremonyFailed", this.lord.Named("SUBJECT"));
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					},
					hotKey = KeyBindingDefOf.Misc6
				};
			}
			yield break;
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x00281B9C File Offset: 0x0027FD9C
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_TargetInfo.Look(ref this.targetSpot, "targetSpot");
			Scribe_Values.Look<string>(ref this.questEndedSignal, "questEndedSignal", null, false);
			Scribe_Values.Look<IntVec3>(ref this.spotCell, "spotCell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.ceremonyStarted, "ceremonyStarted", false, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistParticipants, "colonistParticipants", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040057B1 RID: 22449
		public const int ExpirationTicks = 30000;

		// Token: 0x040057B2 RID: 22450
		public static readonly string MemoCeremonyStarted = "CeremonyStarted";

		// Token: 0x040057B3 RID: 22451
		private const string MemoCeremonyFinished = "CeremonyFinished";

		// Token: 0x040057B4 RID: 22452
		public const int WaitTimeTicks = 600;

		// Token: 0x040057B5 RID: 22453
		public Pawn bestower;

		// Token: 0x040057B6 RID: 22454
		public Pawn target;

		// Token: 0x040057B7 RID: 22455
		public LocalTargetInfo targetSpot;

		// Token: 0x040057B8 RID: 22456
		public IntVec3 spotCell;

		// Token: 0x040057B9 RID: 22457
		public Thing shuttle;

		// Token: 0x040057BA RID: 22458
		public string questEndedSignal;

		// Token: 0x040057BB RID: 22459
		public List<Pawn> colonistParticipants = new List<Pawn>();

		// Token: 0x040057BC RID: 22460
		public bool ceremonyStarted;

		// Token: 0x040057BD RID: 22461
		private LordToil_BestowingCeremony_Perform ceremonyToil;

		// Token: 0x040057BE RID: 22462
		private LordToil exitToil;

		// Token: 0x040057BF RID: 22463
		private RitualOutcomeEffectWorker_Bestowing outcome;

		// Token: 0x040057C0 RID: 22464
		private const float HeatstrokeHypothermiaMinSeverityForLeaving = 0.35f;

		// Token: 0x040057C1 RID: 22465
		private const int HeatstrokeHypothermiaGoodwillOffset = -50;

		// Token: 0x040057C2 RID: 22466
		private const float GasExposireSeverityForLeaving = 0.9f;

		// Token: 0x040057C3 RID: 22467
		private Texture2D icon;

		// Token: 0x040057C4 RID: 22468
		private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();
	}
}
