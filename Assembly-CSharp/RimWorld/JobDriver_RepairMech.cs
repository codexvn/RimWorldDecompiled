using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RepairMech : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RepairMech _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
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
			JobDriver_RepairMech CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckBiotech("Mech repair"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals24.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals24.FailOnForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals24.FailOn(() => CS_0024_003C_003E8__locals24.Mech.IsAttacking());
				if (!CS_0024_003C_003E8__locals24.Remote)
				{
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0080;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0080;
			case 2:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0080:
				toil = (CS_0024_003C_003E8__locals24.Remote ? Toils_General.Wait(int.MaxValue) : Toils_General.WaitWith(TargetIndex.A, int.MaxValue, useProgressBar: false, maintainPosture: true, maintainSleep: true));
				toil.WithEffect(EffecterDefOf.MechRepairing, TargetIndex.A, null);
				toil.PlaySustainerOrSound(CS_0024_003C_003E8__locals24.Remote ? SoundDefOf.RepairMech_Remote : SoundDefOf.RepairMech_Touch);
				toil.AddPreInitAction(delegate
				{
					CS_0024_003C_003E8__locals24.ticksToNextRepair = CS_0024_003C_003E8__locals24.TicksPerHeal;
				});
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals24.ticksToNextRepair -= delta;
					if (CS_0024_003C_003E8__locals24.ticksToNextRepair <= 0)
					{
						CS_0024_003C_003E8__locals24.Mech.needs.energy.CurLevel -= CS_0024_003C_003E8__locals24.Mech.GetStatValue(StatDefOf.MechEnergyLossPerHP);
						MechRepairUtility.RepairTick(CS_0024_003C_003E8__locals24.Mech);
						CS_0024_003C_003E8__locals24.ticksToNextRepair = CS_0024_003C_003E8__locals24.TicksPerHeal;
					}
					CS_0024_003C_003E8__locals24.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals24.Mech);
					if (CS_0024_003C_003E8__locals24.pawn.skills != null)
					{
						CS_0024_003C_003E8__locals24.pawn.skills.Learn(SkillDefOf.Crafting, 0.05f * (float)delta);
					}
				};
				toil.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals24.Mech.jobs?.curJob != null)
					{
						CS_0024_003C_003E8__locals24.Mech.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}
				});
				toil.AddEndCondition(() => MechRepairUtility.CanRepair(CS_0024_003C_003E8__locals24.Mech) ? JobCondition.Ongoing : JobCondition.Succeeded);
				if (!CS_0024_003C_003E8__locals24.Remote)
				{
					toil.activeSkill = () => SkillDefOf.Crafting;
				}
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
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
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private const TargetIndex MechInd = TargetIndex.A;

	private const int DefaultTicksPerHeal = 120;

	protected int ticksToNextRepair;

	protected Pawn Mech => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	protected virtual bool Remote => false;

	protected int TicksPerHeal => Mathf.RoundToInt(1f / pawn.GetStatValue(StatDefOf.MechRepairSpeed) * 120f);

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Mech, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksToNextRepair, "ticksToNextRepair", 0);
	}
}
