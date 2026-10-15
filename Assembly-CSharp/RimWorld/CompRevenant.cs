using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld;

public class CompRevenant : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__45 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRevenant _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompRevenant CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				if (Find.TickManager.TicksGame < CS_0024_003C_003E8__locals16.nextHypnosis)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "Reset hypnosis cooldown",
						action = delegate
						{
							CS_0024_003C_003E8__locals16.nextHypnosis = Find.TickManager.TicksGame;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0084;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0084;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00c4;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0105;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0105:
				if (CS_0024_003C_003E8__locals16.revenantState != 0 && CS_0024_003C_003E8__locals16.revenantState != RevenantState.Sleep)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "Find target",
					action = delegate
					{
						Pawn pawn = RevenantUtility.ScanForTarget(CS_0024_003C_003E8__locals16.Revenant, forced: true);
						if (pawn != null)
						{
							CS_0024_003C_003E8__locals16.Revenant.mindState.enemyTarget = pawn;
							CS_0024_003C_003E8__locals16.revenantState = RevenantState.Attack;
							CS_0024_003C_003E8__locals16.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced);
						}
					}
				};
				_003C_003E1__state = 4;
				return true;
				IL_00c4:
				if (CS_0024_003C_003E8__locals16.revenantState != RevenantState.Sleep)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "Change to sleep mode",
						action = delegate
						{
							CS_0024_003C_003E8__locals16.revenantState = RevenantState.Sleep;
							CS_0024_003C_003E8__locals16.Revenant.mindState.enemyTarget = null;
							CS_0024_003C_003E8__locals16.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced);
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0105;
				IL_0084:
				if (CS_0024_003C_003E8__locals16.revenantState != 0)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "Change to wander mode",
						action = delegate
						{
							CS_0024_003C_003E8__locals16.revenantState = RevenantState.Wander;
							CS_0024_003C_003E8__locals16.Revenant.mindState.enemyTarget = null;
							CS_0024_003C_003E8__locals16.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced);
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00c4;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__45(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private const int ForcedVisibilityMessageCooldownTicks = 2400;

	private const int HeardCheckInterval = 60;

	private const int SeenCheckInterval = 90;

	private const float HeardDistance = 7.9f;

	private const float SeenDistance = 8.9f;

	private const int HeardMessageCooldownTicks = 1200;

	private const int SeenLetterCooldownTicks = 1200;

	public const int LongerTracksUnlockIndex = 1;

	public const int CanHearUnlockIndex = 2;

	public RevenantState revenantState;

	public List<Pawn> revenantVictims = new List<Pawn>();

	public bool everRevealed;

	public bool revenantSmearNotified;

	public int revenantLastLeftSmear = -99999;

	public int becomeInvisibleTick = int.MaxValue;

	public int lastForcedVisibilityMessage = -99999;

	public int escapeSecondStageStartedTick = -99999;

	public int biosignature;

	public bool injuredWhileAttacking;

	public int nextHypnosis = -99999;

	private int lastHeardMessageTick = -99999;

	private int lastSeenLetterTick = -99999;

	private string biosignatureName;

	[Unsaved(false)]
	private HediffComp_Invisibility invisibility;

	private Pawn Revenant => (Pawn)parent;

	public string BiosignatureName => biosignatureName ?? (biosignatureName = AnomalyUtility.GetBiosignatureName(biosignature));

	public HediffComp_Invisibility Invisibility
	{
		get
		{
			if (invisibility != null)
			{
				return invisibility;
			}
			Hediff hediff = Revenant.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility);
			if (hediff == null)
			{
				hediff = Revenant.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null);
			}
			invisibility = hediff?.TryGetComp<HediffComp_Invisibility>();
			return invisibility;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref revenantState, "revenantState", RevenantState.Wander);
		Scribe_Collections.Look(ref revenantVictims, "revenantVictims", LookMode.Reference);
		Scribe_Values.Look(ref everRevealed, "everRevealed", defaultValue: false);
		Scribe_Values.Look(ref revenantSmearNotified, "revenantSmearNotified", defaultValue: false);
		Scribe_Values.Look(ref revenantLastLeftSmear, "revenantLastLeftSmear", 0);
		Scribe_Values.Look(ref becomeInvisibleTick, "becomeInvisibleTick", 0);
		Scribe_Values.Look(ref lastForcedVisibilityMessage, "lastForcedVisibilityMessage", 0);
		Scribe_Values.Look(ref escapeSecondStageStartedTick, "escapeSecondStageStartedTick", 0);
		Scribe_Values.Look(ref biosignature, "biosignature", 0);
		Scribe_Values.Look(ref injuredWhileAttacking, "injuredWhileAttacking", defaultValue: false);
		Scribe_Values.Look(ref nextHypnosis, "nextHypnosis", 0);
		Scribe_Values.Look(ref lastHeardMessageTick, "lastHeardMessageTick", 0);
		Scribe_Values.Look(ref lastSeenLetterTick, "lastSeenLetterTick", 0);
	}

	public void SetState(RevenantState state)
	{
		revenantState = state;
		Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced);
	}

	public override void PostPostMake()
	{
		biosignature = Rand.Int;
	}

	public override void CompTick()
	{
		if (Find.TickManager.TicksGame > becomeInvisibleTick)
		{
			Invisibility.BecomeInvisible();
			becomeInvisibleTick = int.MaxValue;
		}
		if (Revenant.Spawned)
		{
			if (Revenant.IsHashIntervalTick(60) && Revenant.IsPsychologicallyInvisible())
			{
				CheckIfHeard();
			}
			if (Revenant.IsHashIntervalTick(90))
			{
				CheckIfSeen();
			}
		}
	}

	private void CheckIfHeard()
	{
		if (!Find.AnalysisManager.TryGetAnalysisProgress(biosignature, out var details) || details.timesDone < 2)
		{
			return;
		}
		List<Pawn> freeColonistsSpawned = Revenant.Map.mapPawns.FreeColonistsSpawned;
		for (int i = 0; i < freeColonistsSpawned.Count; i++)
		{
			Pawn pawn = freeColonistsSpawned[i];
			if (Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 7.9f) && WanderUtility.InSameRoom(pawn.PositionHeld, Revenant.PositionHeld, Revenant.Map) && !MentalStateUtility.IsHavingMentalBreak(pawn))
			{
				MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RevenantHeard, new Vector3(0f, 0f, 1.1f));
				GrammarRequest request = default(GrammarRequest);
				request.Includes.Add(RulePackDefOf.RevenantNoises);
				if (Find.TickManager.TicksGame > lastHeardMessageTick + 1200)
				{
					Messages.Message("MessageRevenantHeard".Translate(pawn.Named("PAWN"), GrammarResolver.Resolve("verb", request, null, forceLog: false, null, null, null, capitalizeFirstSentence: false)), pawn, MessageTypeDefOf.ThreatSmall);
					lastHeardMessageTick = Find.TickManager.TicksGame;
				}
			}
		}
	}

	private void CheckIfSeen()
	{
		if (!Find.AnalysisManager.TryGetAnalysisProgress(biosignature, out var details) || !details.Satisfied)
		{
			return;
		}
		List<Pawn> freeColonistsSpawned = Revenant.Map.mapPawns.FreeColonistsSpawned;
		bool flag = false;
		for (int i = 0; i < freeColonistsSpawned.Count; i++)
		{
			Pawn pawn = freeColonistsSpawned[i];
			if (!PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn) && Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 8.9f) && GenSight.LineOfSightToThing(pawn.PositionHeld, Revenant, Revenant.Map))
			{
				if (Revenant.IsPsychologicallyInvisible() && Find.TickManager.TicksGame > lastSeenLetterTick + 1200)
				{
					Find.LetterStack.ReceiveLetter("LetterRevenantSeenLabel".Translate(), "LetterRevenantSeen".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn);
					lastSeenLetterTick = Find.TickManager.TicksGame;
				}
				flag = true;
				break;
			}
		}
		if (flag)
		{
			Invisibility.BecomeVisible();
			becomeInvisibleTick = Find.TickManager.TicksGame + 140;
		}
	}

	public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		if (!Revenant.Dead)
		{
			if (revenantState == RevenantState.Attack)
			{
				injuredWhileAttacking = true;
			}
			if (revenantState == RevenantState.Sleep)
			{
				SetState(RevenantState.Escape);
			}
		}
	}

	public override void Notify_BecameVisible()
	{
		if (Revenant.Spawned)
		{
			SoundDefOf.Pawn_Revenant_Revealed.PlayOneShotOnCamera();
			everRevealed = true;
			revenantSmearNotified = true;
		}
	}

	public override void Notify_BecameInvisible()
	{
		if (Revenant.Spawned)
		{
			SoundDefOf.Pawn_Revenant_Stealth.PlayOneShot(Revenant);
		}
	}

	public override void Notify_ForcedVisible()
	{
		if (Revenant.Spawned && !Revenant.Position.Fogged(Revenant.Map))
		{
			SoundDefOf.Pawn_Revenant_StartledScream.PlayOneShot(Revenant);
			if (Find.TickManager.TicksGame > lastForcedVisibilityMessage + 2400)
			{
				Messages.Message("MessageRevenantForcedVisibility".Translate(), Revenant, MessageTypeDefOf.ThreatSmall);
				lastForcedVisibilityMessage = Find.TickManager.TicksGame;
			}
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		ClearVictims();
	}

	public override void Notify_PassedToWorld()
	{
		ClearVictims();
	}

	private void ClearVictims()
	{
		if (revenantVictims.NullOrEmpty())
		{
			return;
		}
		foreach (Pawn revenantVictim in revenantVictims)
		{
			Hediff hediff = revenantVictim?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.RevenantHypnosis);
			if (hediff != null)
			{
				revenantVictim.health.RemoveHediff(hediff);
			}
		}
		revenantVictims.Clear();
	}

	public void Hypnotize(Pawn victim)
	{
		if (!victim.Dead)
		{
			victim.health.AddHediff(HediffDefOf.RevenantHypnosis, null, null);
			RestUtility.WakeUp(victim);
			revenantVictims.Add(victim);
			Revenant.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
			Revenant.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
			Revenant.mindState.enemyTarget = null;
			nextHypnosis = Find.TickManager.TicksGame + Mathf.FloorToInt(RevenantUtility.SearchForTargetCooldownRangeDays.RandomInRange * 60000f);
			if (PawnUtility.ShouldSendNotificationAbout(victim))
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPawnHypnotized".Translate(victim.Named("PAWN")), "LetterPawnHypnotized".Translate(victim.Named("PAWN")), LetterDefOf.NegativeEvent, victim);
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		TaggedString taggedString = "Biosignature".Translate() + ": " + BiosignatureName;
		if (DebugSettings.showHiddenInfo)
		{
			taggedString += "\nState: " + revenantState;
		}
		return taggedString;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__45))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
	}
}
