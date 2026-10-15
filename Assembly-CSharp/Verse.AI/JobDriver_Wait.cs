using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_Wait : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public LocalTargetInfo focusLocal;

		public JobDriver_Wait _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__3()
		{
			_003C_003E4__this.pawn.rotationTracker.FaceTarget(focusLocal);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Wait _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
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
			JobDriver_Wait CS_0024_003C_003E8__locals27 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil = (CS_0024_003C_003E8__locals27.job.forceSleep ? Toils_LayDown.LayDown(TargetIndex.A, hasBed: false, lookForOtherJobs: false) : ToilMaker.MakeToil("MakeNewToils"));
				toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
				{
					CS_0024_003C_003E8__locals27.Map.pawnDestinationReservationManager.Reserve(CS_0024_003C_003E8__locals27.pawn, CS_0024_003C_003E8__locals27.job, CS_0024_003C_003E8__locals27.pawn.Position);
					CS_0024_003C_003E8__locals27.pawn.pather?.StopDead();
					CS_0024_003C_003E8__locals27.CheckForAutoAttack();
				});
				toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals27.job.expiryInterval == -1 && CS_0024_003C_003E8__locals27.job.def == JobDefOf.Wait_Combat && !CS_0024_003C_003E8__locals27.pawn.Drafted)
					{
						Log.Error(CS_0024_003C_003E8__locals27.pawn?.ToString() + " in eternal WaitCombat without being drafted.");
						CS_0024_003C_003E8__locals27.ReadyForNextToil();
					}
					else
					{
						if (CS_0024_003C_003E8__locals27.job.forceSleep)
						{
							CS_0024_003C_003E8__locals27.asleep = true;
						}
						if (GenTicks.IsTickIntervalDelta(CS_0024_003C_003E8__locals27.pawn.thingIDNumber, 4, delta))
						{
							CS_0024_003C_003E8__locals27.CheckForAutoAttack();
						}
					}
				});
				CS_0024_003C_003E8__locals27.DecorateWaitToil(toil);
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				if (CS_0024_003C_003E8__locals27.job.overrideFacing != Rot4.Invalid)
				{
					toil.handlingFacing = true;
					toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
					{
						CS_0024_003C_003E8__locals27.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals27.pawn.Position + CS_0024_003C_003E8__locals27.job.overrideFacing.FacingCell);
					});
				}
				else if (CS_0024_003C_003E8__locals27.pawn.mindState != null && CS_0024_003C_003E8__locals27.pawn.mindState.duty != null && CS_0024_003C_003E8__locals27.pawn.mindState.duty.focus != null && CS_0024_003C_003E8__locals27.job.def != JobDefOf.Wait_Combat)
				{
					_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals30 = new _003C_003Ec__DisplayClass3_0
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals27,
						focusLocal = CS_0024_003C_003E8__locals27.pawn.mindState.duty.focus
					};
					toil.handlingFacing = true;
					toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
					{
						CS_0024_003C_003E8__locals30._003C_003E4__this.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals30.focusLocal);
					});
				}
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private const int TargetSearchInterval = 4;

	public override string GetReport()
	{
		if (job.def == JobDefOf.Wait_Combat && pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
		{
			return "ReportStanding".Translate();
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void DecorateWaitToil(Toil wait)
	{
	}

	public override void Notify_StanceChanged()
	{
		if (pawn.stances.curStance is Stance_Mobile)
		{
			CheckForAutoAttack();
		}
	}

	private void CheckForAutoAttack()
	{
		if (!base.pawn.kindDef.canMeleeAttack || base.pawn.Downed || base.pawn.stances.FullBodyBusy || base.pawn.IsCarryingPawn() || (!base.pawn.IsPlayerControlled && base.pawn.IsPsychologicallyInvisible()) || base.pawn.IsShambler)
		{
			return;
		}
		collideWithPawns = false;
		bool flag = !base.pawn.WorkTagIsDisabled(WorkTags.Violent);
		bool flag2 = base.pawn.RaceProps.ToolUser && base.pawn.Faction == Faction.OfPlayer && !base.pawn.WorkTagIsDisabled(WorkTags.Firefighting);
		if (!(flag || flag2))
		{
			return;
		}
		Fire fire = null;
		for (int i = 0; i < 9; i++)
		{
			IntVec3 c = base.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
			if (!c.InBounds(base.pawn.Map))
			{
				continue;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int j = 0; j < thingList.Count; j++)
			{
				if (flag && base.pawn.kindDef.canMeleeAttack && thingList[j] is Pawn pawn && !pawn.ThreatDisabled(base.pawn) && base.pawn.HostileTo(pawn))
				{
					CompActivity comp = pawn.GetComp<CompActivity>();
					if ((comp == null || comp.IsActive) && !base.pawn.ThreatDisabledBecauseNonAggressiveRoamer(pawn) && GenHostility.IsActiveThreatTo(pawn, base.pawn.Faction, ignoreHives: false))
					{
						base.pawn.meleeVerbs.TryMeleeAttack(pawn);
						collideWithPawns = true;
						return;
					}
				}
				if (flag2 && thingList[j] is Fire fire2 && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != base.pawn))
				{
					fire = fire2;
				}
			}
		}
		if (fire != null && (!base.pawn.InMentalState || base.pawn.MentalState.def.allowBeatfire))
		{
			base.pawn.natives.TryBeatFire(fire);
		}
		else
		{
			if (!flag || !job.canUseRangedWeapon || job.def != JobDefOf.Wait_Combat || (base.pawn.drafter != null && !base.pawn.drafter.FireAtWill))
			{
				return;
			}
			Verb currentEffectiveVerb = base.pawn.CurrentEffectiveVerb;
			if (currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack)
			{
				TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToAll | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
				if (currentEffectiveVerb.IsIncendiary_Ranged())
				{
					targetScanFlags |= TargetScanFlags.NeedNonBurning;
				}
				Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(base.pawn, targetScanFlags);
				if (thing != null)
				{
					base.pawn.TryStartAttack(thing);
					collideWithPawns = true;
				}
			}
		}
	}
}
