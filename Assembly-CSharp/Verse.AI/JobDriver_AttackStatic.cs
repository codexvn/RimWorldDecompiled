using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_AttackStatic : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public JobDriver_AttackStatic _003C_003E4__this;

		public Toil init;

		internal void _003CMakeNewToils_003Eb__0(JobCondition jobCondition)
		{
			if (!_003C_003E4__this.pawn.IsPlayerControlled || !_003C_003E4__this.pawn.Drafted || _003C_003E4__this.job.playerInterruptedForced)
			{
				return;
			}
			Thing targetThingA = _003C_003E4__this.TargetThingA;
			if (targetThingA == null || !targetThingA.def.autoTargetNearbyIdenticalThings)
			{
				return;
			}
			Verb verb = _003C_003E4__this.pawn.TryGetAttackVerb(_003C_003E4__this.TargetA.Thing, !_003C_003E4__this.pawn.IsColonist);
			foreach (IntVec3 item in GenRadial.RadialCellsAround(_003C_003E4__this.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
			{
				if (!item.InBounds(_003C_003E4__this.Map))
				{
					continue;
				}
				foreach (Thing thing in item.GetThingList(_003C_003E4__this.Map))
				{
					if (thing.def == _003C_003E4__this.TargetThingA.def && verb != null && verb.CanHitTargetFrom(_003C_003E4__this.pawn.Position, thing) && _003C_003E4__this.pawn.jobs.jobQueue.Count == 0)
					{
						Job job = _003C_003E4__this.job.Clone();
						job.targetA = thing;
						job.endIfCantShootTargetFromCurPos = true;
						_003C_003E4__this.pawn.jobs.jobQueue.EnqueueFirst(job, null);
						return;
					}
				}
			}
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			if (_003C_003E4__this.TargetThingA is Pawn pawn)
			{
				_003C_003E4__this.startedIncapacitated = pawn.Downed;
			}
			_003C_003E4__this.pawn.pather.StopDead();
		}

		internal void _003CMakeNewToils_003Eb__2(int delta)
		{
			if (!_003C_003E4__this.TargetA.IsValid)
			{
				_003C_003E4__this.EndJobWith(JobCondition.Succeeded);
				return;
			}
			if (_003C_003E4__this.TargetA.HasThing)
			{
				Pawn pawn = _003C_003E4__this.TargetA.Thing as Pawn;
				if (_003C_003E4__this.TargetA.Thing.Destroyed || (pawn != null && !_003C_003E4__this.startedIncapacitated && pawn.Downed) || (pawn != null && pawn.IsPsychologicallyInvisible()))
				{
					_003C_003E4__this.EndJobWith(JobCondition.Succeeded);
					return;
				}
			}
			if (_003C_003E4__this.numAttacksMade >= _003C_003E4__this.job.maxNumStaticAttacks && !_003C_003E4__this.pawn.stances.FullBodyBusy)
			{
				_003C_003E4__this.EndJobWith(JobCondition.Succeeded);
			}
			else if (_003C_003E4__this.pawn.TryStartAttack(_003C_003E4__this.TargetA))
			{
				_003C_003E4__this.numAttacksMade++;
			}
			else
			{
				if (_003C_003E4__this.pawn.stances.FullBodyBusy)
				{
					return;
				}
				Verb verb = _003C_003E4__this.pawn.TryGetAttackVerb(_003C_003E4__this.TargetA.Thing, !_003C_003E4__this.pawn.IsColonist);
				if (_003C_003E4__this.job.endIfCantShootTargetFromCurPos && (verb == null || !verb.CanHitTargetFrom(_003C_003E4__this.pawn.Position, _003C_003E4__this.TargetA)))
				{
					_003C_003E4__this.EndJobWith(JobCondition.Incompletable);
				}
				else
				{
					if (!_003C_003E4__this.job.endIfCantShootInMelee)
					{
						return;
					}
					if (verb == null)
					{
						_003C_003E4__this.EndJobWith(JobCondition.Incompletable);
						return;
					}
					float num = verb.verbProps.EffectiveMinRange(_003C_003E4__this.TargetA, _003C_003E4__this.pawn);
					if ((float)_003C_003E4__this.pawn.Position.DistanceToSquared(_003C_003E4__this.TargetA.Cell) < num * num && _003C_003E4__this.pawn.Position.AdjacentTo8WayOrInside(_003C_003E4__this.TargetA.Cell))
					{
						_003C_003E4__this.EndJobWith(JobCondition.Incompletable);
					}
				}
			}
		}

		internal SkillDef _003CMakeNewToils_003Eb__3()
		{
			return Toils_Combat.GetActiveSkillForToil(init);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_AttackStatic _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_AttackStatic jobDriver_AttackStatic = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_AttackStatic.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.IsPlayerControlled && _003C_003E8__1._003C_003E4__this.pawn.Drafted && !_003C_003E8__1._003C_003E4__this.job.playerInterruptedForced)
					{
						Thing targetThingA = _003C_003E8__1._003C_003E4__this.TargetThingA;
						if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
						{
							Verb verb2 = _003C_003E8__1._003C_003E4__this.pawn.TryGetAttackVerb(_003C_003E8__1._003C_003E4__this.TargetA.Thing, !_003C_003E8__1._003C_003E4__this.pawn.IsColonist);
							foreach (IntVec3 item in GenRadial.RadialCellsAround(_003C_003E8__1._003C_003E4__this.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
							{
								if (item.InBounds(_003C_003E8__1._003C_003E4__this.Map))
								{
									foreach (Thing thing in item.GetThingList(_003C_003E8__1._003C_003E4__this.Map))
									{
										if (thing.def == _003C_003E8__1._003C_003E4__this.TargetThingA.def && verb2 != null && verb2.CanHitTargetFrom(_003C_003E8__1._003C_003E4__this.pawn.Position, thing) && _003C_003E8__1._003C_003E4__this.pawn.jobs.jobQueue.Count == 0)
										{
											Job job = _003C_003E8__1._003C_003E4__this.job.Clone();
											job.targetA = thing;
											job.endIfCantShootTargetFromCurPos = true;
											_003C_003E8__1._003C_003E4__this.pawn.jobs.jobQueue.EnqueueFirst(job, null);
											return;
										}
									}
								}
							}
						}
					}
				});
				_003C_003E2__current = Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.init = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.init.initAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.TargetThingA is Pawn pawn)
					{
						_003C_003E8__1._003C_003E4__this.startedIncapacitated = pawn.Downed;
					}
					_003C_003E8__1._003C_003E4__this.pawn.pather.StopDead();
				};
				_003C_003E8__1.init.tickIntervalAction = delegate
				{
					if (!_003C_003E8__1._003C_003E4__this.TargetA.IsValid)
					{
						_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						if (_003C_003E8__1._003C_003E4__this.TargetA.HasThing)
						{
							Pawn pawn2 = _003C_003E8__1._003C_003E4__this.TargetA.Thing as Pawn;
							if (_003C_003E8__1._003C_003E4__this.TargetA.Thing.Destroyed || (pawn2 != null && !_003C_003E8__1._003C_003E4__this.startedIncapacitated && pawn2.Downed) || (pawn2 != null && pawn2.IsPsychologicallyInvisible()))
							{
								_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Succeeded);
								return;
							}
						}
						if (_003C_003E8__1._003C_003E4__this.numAttacksMade >= _003C_003E8__1._003C_003E4__this.job.maxNumStaticAttacks && !_003C_003E8__1._003C_003E4__this.pawn.stances.FullBodyBusy)
						{
							_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Succeeded);
						}
						else if (_003C_003E8__1._003C_003E4__this.pawn.TryStartAttack(_003C_003E8__1._003C_003E4__this.TargetA))
						{
							_003C_003E8__1._003C_003E4__this.numAttacksMade = _003C_003E8__1._003C_003E4__this.numAttacksMade + 1;
						}
						else if (!_003C_003E8__1._003C_003E4__this.pawn.stances.FullBodyBusy)
						{
							Verb verb = _003C_003E8__1._003C_003E4__this.pawn.TryGetAttackVerb(_003C_003E8__1._003C_003E4__this.TargetA.Thing, !_003C_003E8__1._003C_003E4__this.pawn.IsColonist);
							if (_003C_003E8__1._003C_003E4__this.job.endIfCantShootTargetFromCurPos && (verb == null || !verb.CanHitTargetFrom(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1._003C_003E4__this.TargetA)))
							{
								_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Incompletable);
							}
							else if (_003C_003E8__1._003C_003E4__this.job.endIfCantShootInMelee)
							{
								if (verb == null)
								{
									_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Incompletable);
								}
								else
								{
									float num2 = verb.verbProps.EffectiveMinRange(_003C_003E8__1._003C_003E4__this.TargetA, _003C_003E8__1._003C_003E4__this.pawn);
									if ((float)_003C_003E8__1._003C_003E4__this.pawn.Position.DistanceToSquared(_003C_003E8__1._003C_003E4__this.TargetA.Cell) < num2 * num2 && _003C_003E8__1._003C_003E4__this.pawn.Position.AdjacentTo8WayOrInside(_003C_003E8__1._003C_003E4__this.TargetA.Cell))
									{
										_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Incompletable);
									}
								}
							}
						}
					}
				};
				_003C_003E8__1.init.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.init.activeSkill = () => Toils_Combat.GetActiveSkillForToil(_003C_003E8__1.init);
				_003C_003E2__current = _003C_003E8__1.init;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private bool startedIncapacitated;

	private int numAttacksMade;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref startedIncapacitated, "startedIncapacitated", defaultValue: false);
		Scribe_Values.Look(ref numAttacksMade, "numAttacksMade", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
