using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse;

public class Hediff_Shambler : HediffWithComps
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__36 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_Shambler _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__36(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Hediff_Shambler CS_0024_003C_003E8__locals4 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffWithComps)CS_0024_003C_003E8__locals4).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals4.pawn.Downed && !CS_0024_003C_003E8__locals4.IsRising)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "Self Raise",
							action = delegate
							{
								CS_0024_003C_003E8__locals4.StartRising();
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__36(0)
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

	private static readonly FloatRange ResurrectionSecondsRange = new FloatRange(5f, 10f);

	private static readonly FloatRange StunSecondsRange = new FloatRange(1f, 3f);

	private static readonly FloatRange AlertSecondsRange = new FloatRange(0.5f, 3f);

	private static readonly IntRange SelfRaiseHoursRange = new IntRange(3, 4);

	private const float ParticleSpawnMTBSeconds = 1f;

	private static readonly IntRange CheckForTargetTicksInterval = new IntRange(900, 1800);

	private const float ExtinguishFireMTB = 45f;

	private const float BioferriteOnDeathChance = 0.04f;

	private const int BioferriteAmountOnDeath = 10;

	public float headRotation;

	private float resurrectTimer;

	private float selfRaiseTimer;

	private float alertTimer;

	private int nextTargetCheckTick = -99999;

	private Thing alertedTarget;

	private Effecter riseEffecter;

	private Sustainer riseSustainer;

	private float corpseDamagePct = 1f;

	public bool IsRising => pawn.health.hediffSet.HasHediff(HediffDefOf.Rising);

	public override void PostMake()
	{
		base.PostMake();
		headRotation = Rand.RangeSeeded(-20f, 20f, pawn.thingIDNumber);
		if (!pawn.Dead)
		{
			pawn.timesRaisedAsShambler++;
		}
	}

	public override void PostAdd(DamageInfo? dinfo)
	{
		if (!ModLister.CheckAnomaly("Shambler"))
		{
			pawn.health.RemoveHediff(this);
		}
		else
		{
			base.PostAdd(dinfo);
		}
	}

	public override void Notify_Spawned()
	{
		base.Notify_Spawned();
		pawn.Map.mapPawns.RegisterShambler(pawn);
	}

	public override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (IsRising)
		{
			if ((float)Find.TickManager.TicksGame > resurrectTimer)
			{
				FinishRising();
			}
			if (!pawn.Spawned)
			{
				return;
			}
			if ((float)Find.TickManager.TicksGame > resurrectTimer - 15f)
			{
				riseSustainer?.End();
			}
			else if (IsRising)
			{
				if (riseSustainer == null || riseSustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(pawn, MaintenanceType.PerTick);
					riseSustainer = SoundDefOf.Pawn_Shambler_Rise.TrySpawnSustainer(info);
				}
				if (riseEffecter == null)
				{
					riseEffecter = EffecterDefOf.ShamblerRaise.Spawn(pawn, pawn.Map);
				}
				if (pawn.Drawer.renderer.CurAnimation != AnimationDefOf.ShamblerRise)
				{
					pawn.Drawer.renderer.SetAnimation(AnimationDefOf.ShamblerRise);
				}
				riseSustainer.Maintain();
				riseEffecter.EffectTick(pawn, TargetInfo.Invalid);
			}
			return;
		}
		if (Rand.MTBEventOccurs(1f, 60f, 1f))
		{
			FleckMaker.ThrowShamblerParticles(pawn);
		}
		if (pawn.IsBurning() && !pawn.Downed && Rand.MTBEventOccurs(45f, 60f, 1f))
		{
			((Fire)pawn.GetAttachment(ThingDefOf.Fire)).Destroy();
			pawn.records.Increment(RecordDefOf.FiresExtinguished);
		}
		if (pawn.Spawned && !pawn.mutant.IsPassive && !pawn.Drafted)
		{
			if (Find.TickManager.TicksGame > nextTargetCheckTick)
			{
				nextTargetCheckTick = Find.TickManager.TicksGame + CheckForTargetTicksInterval.RandomInRange;
				Thing thing = MutantUtility.FindShamblerTarget(pawn);
				if (thing != null)
				{
					Notify_DelayedAlert(thing);
					MutantUtility.ActivateNearbyShamblers(pawn, thing);
				}
			}
			if (alertedTarget != null && (float)Find.TickManager.TicksGame > alertTimer)
			{
				pawn.mindState.enemyTarget = alertedTarget;
				pawn.mindState.Notify_EngagedTarget();
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
				alertedTarget = null;
				if (DebugViewSettings.drawShamblerAlertMote)
				{
					MoteMaker.MakeColonistActionOverlay(pawn, ThingDefOf.Mote_ShamblerAlert);
				}
				SoundDefOf.Pawn_Shambler_Alert.PlayOneShot(pawn);
			}
		}
		if (ShouldSelfRaise())
		{
			StartRising();
		}
	}

	private bool ShouldSelfRaise()
	{
		if (pawn.DevelopmentalStage == DevelopmentalStage.Baby)
		{
			return false;
		}
		if (pawn.Downed && pawn.CarriedBy == null)
		{
			return (float)Find.TickManager.TicksGame > selfRaiseTimer;
		}
		return false;
	}

	public void StartRising(int lifespanTicks = -1)
	{
		if (!pawn.Dead && !pawn.Downed)
		{
			Log.Error("Tried to raise non dead/downed pawn as shambler");
			pawn.mutant.Turn(clearLord: true);
			return;
		}
		MutantUtility.RestoreBodyParts(pawn);
		pawn.Notify_DisabledWorkTypesChanged();
		if (!pawn.Dead || ResurrectionUtility.TryResurrect(pawn, new ResurrectionParams
		{
			noLord = true,
			restoreMissingParts = false,
			removeDiedThoughts = false
		}))
		{
			pawn.jobs?.EndCurrentJob(JobCondition.InterruptForced);
			resurrectTimer = Find.TickManager.TicksGame + ResurrectionSecondsRange.RandomInRange.SecondsToTicks();
			pawn.health.AddHediff(HediffDefOf.Rising, null, null);
		}
	}

	private void CancelRising()
	{
		riseSustainer?.End();
		resurrectTimer = -99999f;
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out var hediff))
		{
			pawn.health.RemoveHediff(hediff);
		}
		if (!pawn.Dead)
		{
			pawn.Kill(null);
		}
	}

	private void FinishRising(bool stun = true)
	{
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.Rising, out var hediff))
		{
			pawn.health.RemoveHediff(hediff);
		}
		if (pawn.ParentHolder is Pawn_CarryTracker pawn_CarryTracker)
		{
			pawn_CarryTracker.TryDropCarriedThing(pawn_CarryTracker.pawn.Position, ThingPlaceMode.Near, out var _);
			pawn_CarryTracker.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
		}
		if (!pawn.mutant.HasTurned)
		{
			pawn.mutant.Turn();
		}
		pawn.timesRaisedAsShambler++;
		MutantUtility.RestoreUntilNotDowned(pawn);
		if (pawn.Spawned && stun)
		{
			pawn.Rotation = Rot4.South;
			pawn.stances.stunner.StunFor(StunSecondsRange.RandomInRange.SecondsToTicks(), pawn, addBattleLog: false, showMote: false);
		}
		pawn.Drawer.renderer.SetAnimation(null);
		StartSelfRaiseTimer();
	}

	private void StartSelfRaiseTimer()
	{
		selfRaiseTimer = Find.TickManager.TicksGame + 2500 * SelfRaiseHoursRange.RandomInRange;
	}

	public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
		if (dinfo.Instigator == null || !pawn.HostileTo(dinfo.Instigator))
		{
			return;
		}
		if (pawn.Spawned && dinfo.Instigator != null && dinfo.Instigator is IAttackTarget && dinfo.Instigator.Spawned && pawn.HostileTo(dinfo.Instigator) && pawn.CanSee(dinfo.Instigator))
		{
			bool num = pawn.mindState.enemyTarget == dinfo.Instigator;
			pawn.mindState.enemyTarget = dinfo.Instigator;
			pawn.mindState.Notify_EngagedTarget();
			if (!num)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptOptional);
			}
			pawn.GetLord()?.Notify_PawnAcquiredTarget(pawn, dinfo.Instigator);
		}
		MutantUtility.ActivateNearbyShamblers(pawn, dinfo.Instigator);
	}

	public void Notify_DelayedAlert(Thing target)
	{
		if (!pawn.mutant.IsPassive && !pawn.Drafted && pawn.mindState.enemyTarget == null)
		{
			alertTimer = Find.TickManager.TicksGame + AlertSecondsRange.RandomInRange.SecondsToTicks();
			alertedTarget = target;
			pawn.GetLord()?.Notify_PawnAcquiredTarget(pawn, target);
		}
	}

	public override void Notify_Downed()
	{
		StartSelfRaiseTimer();
	}

	public override void Notify_PawnKilled()
	{
		corpseDamagePct = pawn.health.summaryHealth.SummaryHealthPercent;
		base.Notify_PawnKilled();
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		if (IsRising)
		{
			CancelRising();
		}
		if (pawn.timesRaisedAsShambler == 1 && Rand.Chance(0.04f) && pawn.SpawnedOrAnyParentSpawned)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite);
			thing.stackCount = 10;
			GenPlace.TryPlaceThing(thing, pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, out var lastResultingThing, null, null, null);
			lastResultingThing.SetForbidden(value: true);
		}
		if (pawn.Corpse != null)
		{
			pawn.Corpse.HitPoints = Mathf.Max(Mathf.RoundToInt((float)pawn.Corpse.MaxHitPoints * corpseDamagePct), 10);
		}
		pawn.health.AddHediff(HediffDefOf.ShamblerCorpse, null, null);
		base.Notify_PawnDied(dinfo, culprit);
	}

	public override void PreRemoved()
	{
		pawn.MapHeld?.mapPawns.DeregisterShambler(pawn);
		base.PreRemoved();
	}

	public override void PostRemoved()
	{
		base.PostRemoved();
		if (pawn.Dead)
		{
			return;
		}
		if (IsRising)
		{
			CancelRising();
		}
		if (pawn.IsMutant)
		{
			if (pawn.mutant.HasTurned)
			{
				pawn.mutant.Revert();
			}
			else
			{
				pawn.mutant = null;
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__36))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		if (ShouldSelfRaise())
		{
			return "ShamblerRegenerating".Translate();
		}
		if (IsRising)
		{
			return "ShamblerRising".Translate();
		}
		if (pawn.CurJobDef == JobDefOf.Wait_Wander || pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
		{
			return "ShamblerStanding".Translate();
		}
		if (pawn.CurJobDef == JobDefOf.GotoWander || pawn.CurJobDef == JobDefOf.Goto)
		{
			return "ShamblerShuffling".Translate();
		}
		return "";
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref alertTimer, "alertTimer", 0f);
		Scribe_Values.Look(ref nextTargetCheckTick, "nextTargetCheckTick", 0);
		Scribe_References.Look(ref alertedTarget, "alertedTarget");
		Scribe_Values.Look(ref resurrectTimer, "resurrectTimer", 0f);
		Scribe_Values.Look(ref selfRaiseTimer, "selfRaiseTimer", 0f);
		Scribe_Values.Look(ref headRotation, "headRotation", 0f);
	}
}
