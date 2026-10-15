using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_UseItem : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_UseItem _003C_003E4__this;

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
			JobDriver_UseItem CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals7.FailOnIncapable(PawnCapacityDefOf.Manipulation);
				CS_0024_003C_003E8__locals7.FailOn(() => !CS_0024_003C_003E8__locals7.TargetThingA.TryGetComp<CompUsable>().CanBeUsedBy(CS_0024_003C_003E8__locals7.pawn));
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, CS_0024_003C_003E8__locals7.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals7.PrepareToUse();
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals7.Use();
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

	private int useDuration = -1;

	private Mote warmupMote;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref useDuration, "useDuration", 0);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		useDuration = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (job.targetB.IsValid && !pawn.Reserve(job.targetB, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
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

	protected Toil PrepareToUse()
	{
		Toil toil = Toils_General.Wait(useDuration, TargetIndex.A);
		toil.WithProgressBarToilDelay(TargetIndex.A);
		toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
		toil.FailOnCannotTouch(TargetIndex.A, base.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch);
		toil.handlingFacing = true;
		toil.tickAction = delegate
		{
			if (job.GetTarget(TargetIndex.A).Thing is ThingWithComps thingWithComps)
			{
				foreach (CompUseEffect comp in thingWithComps.GetComps<CompUseEffect>())
				{
					comp?.PrepareTick();
				}
			}
			else
			{
				job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUseEffect>()?.PrepareTick();
			}
			CompUsable compUsable = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>();
			if (compUsable != null && warmupMote == null && compUsable.Props.warmupMote != null)
			{
				warmupMote = MoteMaker.MakeAttachedOverlay(job.GetTarget(TargetIndex.B).Thing, compUsable.Props.warmupMote, Vector3.zero);
			}
			warmupMote?.Maintain();
			pawn.rotationTracker.FaceTarget(base.TargetA);
		};
		if (job.targetB.IsValid)
		{
			toil.FailOnDespawnedOrNull(TargetIndex.B);
			CompTargetable compTargetable = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTargetable>();
			if (compTargetable != null && compTargetable.Props.nonDownedPawnOnly)
			{
				toil.FailOnDestroyedOrNull(TargetIndex.B);
				toil.FailOnDowned(TargetIndex.B);
			}
		}
		return toil;
	}

	protected Toil Use()
	{
		Toil use = ToilMaker.MakeToil("Use");
		use.initAction = delegate
		{
			Pawn actor = use.actor;
			CompUsable compUsable = actor.CurJob.targetA.Thing.TryGetComp<CompUsable>();
			compUsable.UsedBy(actor);
			if (compUsable.Props.finalizeMote != null)
			{
				MoteMaker.MakeAttachedOverlay(actor.CurJob.GetTarget(TargetIndex.A).Thing, compUsable.Props.finalizeMote, Vector3.zero);
			}
		};
		use.defaultCompleteMode = ToilCompleteMode.Instant;
		return use;
	}
}
