using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_Hack : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Hack _003C_003E4__this;

		private PathEndMode _003CpathEndMode_003E5__2;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Hack CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals24.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals24.FailOn(() => CS_0024_003C_003E8__locals24.CompHacking.Props.intellectualSkillPrerequisite > 0 && CS_0024_003C_003E8__locals24.pawn.skills.GetSkill(SkillDefOf.Intellectual).Level < CS_0024_003C_003E8__locals24.CompHacking.Props.intellectualSkillPrerequisite);
				_003CpathEndMode_003E5__2 = (CS_0024_003C_003E8__locals24.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.ClosestTouch);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, _003CpathEndMode_003E5__2);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.handlingFacing = true;
				toil.tickAction = delegate
				{
					float statValue = CS_0024_003C_003E8__locals24.pawn.GetStatValue(StatDefOf.HackingSpeed);
					CS_0024_003C_003E8__locals24.CompHacking.Hack(statValue, CS_0024_003C_003E8__locals24.pawn);
					CS_0024_003C_003E8__locals24.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f);
					CS_0024_003C_003E8__locals24.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals24.HackTarget);
				};
				toil.WithEffect(EffecterDefOf.Hacking, TargetIndex.A, null);
				if (CS_0024_003C_003E8__locals24.CompHacking.Props.effectHacking != null)
				{
					toil.WithEffect(() => CS_0024_003C_003E8__locals24.CompHacking.Props.effectHacking, () => CS_0024_003C_003E8__locals24.HackTarget.OccupiedRect().ClosestCellTo(CS_0024_003C_003E8__locals24.pawn.Position), null);
				}
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals24.CompHacking.ProgressPercent, interpolateBetweenActorAndTarget: false, -0.5f, alwaysShow: true);
				toil.PlaySoundAtStart(SoundDefOf.Hacking_Started);
				toil.PlaySustainerOrSound(SoundDefOf.Hacking_InProgress);
				toil.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals24.CompHacking.IsHacked)
					{
						SoundDefOf.Hacking_Completed.PlayOneShot(CS_0024_003C_003E8__locals24.HackTarget);
						CS_0024_003C_003E8__locals24.CompHacking.Props.hackingCompletedSound?.PlayOneShot(CS_0024_003C_003E8__locals24.HackTarget);
					}
					else
					{
						SoundDefOf.Hacking_Suspended.PlayOneShot(CS_0024_003C_003E8__locals24.HackTarget);
					}
				});
				toil.FailOnCannotTouch(TargetIndex.A, _003CpathEndMode_003E5__2);
				toil.FailOn(() => CS_0024_003C_003E8__locals24.CompHacking.IsHacked || CS_0024_003C_003E8__locals24.CompHacking.LockedOut);
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.activeSkill = () => SkillDefOf.Intellectual;
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
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

	private Thing HackTarget => base.TargetThingA;

	private CompHackable CompHacking => HackTarget.TryGetComp<CompHackable>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(HackTarget, job, 1, -1, null, errorOnFailed);
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
