using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI;

public class JobDriver_AttackMelee : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_AttackMelee _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__4(int _003C_003E1__state)
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
			JobDriver_AttackMelee CS_0024_003C_003E8__locals32 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals32.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals32.pawn.IsPlayerControlled && CS_0024_003C_003E8__locals32.pawn.Drafted && !CS_0024_003C_003E8__locals32.job.playerInterruptedForced)
					{
						Thing targetThingA = CS_0024_003C_003E8__locals32.TargetThingA;
						if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
						{
							foreach (IntVec3 item in GenRadial.RadialCellsAround(CS_0024_003C_003E8__locals32.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
							{
								if (item.InBounds(CS_0024_003C_003E8__locals32.Map))
								{
									foreach (Thing thing2 in item.GetThingList(CS_0024_003C_003E8__locals32.Map))
									{
										if (thing2.def == CS_0024_003C_003E8__locals32.TargetThingA.def && CS_0024_003C_003E8__locals32.pawn.CanReach(thing2, PathEndMode.Touch, Danger.Deadly) && CS_0024_003C_003E8__locals32.pawn.jobs.jobQueue.Count == 0)
										{
											Job job = CS_0024_003C_003E8__locals32.job.Clone();
											job.targetA = thing2;
											CS_0024_003C_003E8__locals32.pawn.jobs.jobQueue.EnqueueFirst(job, null);
											return;
										}
									}
								}
							}
						}
					}
				});
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					if (CS_0024_003C_003E8__locals32.job.targetA.Thing is Pawn pawn)
					{
						bool num2 = pawn.Downed && CS_0024_003C_003E8__locals32.pawn.mindState.duty != null && CS_0024_003C_003E8__locals32.pawn.mindState.duty.attackDownedIfStarving && CS_0024_003C_003E8__locals32.pawn.Starving();
						CompActivity comp;
						bool flag = ModsConfig.AnomalyActive && pawn.TryGetComp<CompActivity>(out comp) && comp.IsDormant;
						if (num2 || flag)
						{
							CS_0024_003C_003E8__locals32.job.killIncappedTarget = true;
						}
					}
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate
				{
					Thing thing = CS_0024_003C_003E8__locals32.job.GetTarget(TargetIndex.A).Thing;
					if (CS_0024_003C_003E8__locals32.job.reactingToMeleeThreat && thing is Pawn p && !p.Awake())
					{
						CS_0024_003C_003E8__locals32.EndJobWith(JobCondition.InterruptForced);
					}
					else if (CS_0024_003C_003E8__locals32.pawn.meleeVerbs.TryMeleeAttack(thing, CS_0024_003C_003E8__locals32.job.verbToUse) && CS_0024_003C_003E8__locals32.pawn.CurJob != null && CS_0024_003C_003E8__locals32.pawn.jobs.curDriver == CS_0024_003C_003E8__locals32)
					{
						if (CS_0024_003C_003E8__locals32.pawn.GetLord()?.LordJob is LordJob_Ritual_Duel lordJob_Ritual_Duel)
						{
							lordJob_Ritual_Duel.Notify_MeleeAttack(CS_0024_003C_003E8__locals32.pawn, thing);
						}
						CS_0024_003C_003E8__locals32.numMeleeAttacksMade++;
						if (CS_0024_003C_003E8__locals32.numMeleeAttacksMade >= CS_0024_003C_003E8__locals32.job.maxNumMeleeAttacks)
						{
							CS_0024_003C_003E8__locals32.EndJobWith(JobCondition.Succeeded);
						}
					}
				}).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			_003CMakeNewToils_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__4(0)
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

	private const int AutotargetRadius = 4;

	private int numMeleeAttacksMade;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref numMeleeAttacksMade, "numMeleeAttacksMade", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (job.targetA.Thing is IAttackTarget target)
		{
			pawn.Map.attackTargetReservationManager.Reserve(pawn, job, target);
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__4))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_PatherFailed()
	{
		if (job.attackDoorIfTargetLost)
		{
			Thing thing;
			using (PawnPath pawnPath = base.Map.pathFinder.FindPathNow(pawn.Position, base.TargetA.Cell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors), null))
			{
				if (!pawnPath.Found)
				{
					return;
				}
				thing = pawnPath.FirstBlockingBuilding(out var _, pawn);
			}
			if (thing != null && thing.Position.InHorDistOf(pawn.Position, 6f))
			{
				job.targetA = thing;
				job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
				job.expiryInterval = Rand.Range(2000, 4000);
				return;
			}
		}
		base.Notify_PatherFailed();
	}

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}
}
