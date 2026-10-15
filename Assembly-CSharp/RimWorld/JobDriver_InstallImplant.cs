using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_InstallImplant : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_InstallImplant _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_InstallImplant @object = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.WaitWith(TargetIndex.A, 600, useProgressBar: false, maintainPosture: true, maintainSleep: false, TargetIndex.A);
				toil.WithProgressBarToilDelay(TargetIndex.A);
				toil.FailOnDespawnedOrNull(TargetIndex.A);
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(@object.Install);
				_003C_003E1__state = 5;
				return true;
			case 5:
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const TargetIndex TargetPawnInd = TargetIndex.A;

	private const TargetIndex ItemInd = TargetIndex.B;

	private const int DurationTicks = 600;

	private Pawn TargetPawn => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	private Thing Item => job.GetTarget(TargetIndex.B).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(TargetPawn, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(Item, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void Install()
	{
		CompTargetEffect_InstallImplantInOtherPawn compTargetEffect_InstallImplantInOtherPawn = Item.TryGetComp<CompTargetEffect_InstallImplantInOtherPawn>();
		if (compTargetEffect_InstallImplantInOtherPawn == null)
		{
			return;
		}
		BodyPartRecord bodyPartRecord = TargetPawn.RaceProps.body.GetPartsWithDef(compTargetEffect_InstallImplantInOtherPawn.Props.bodyPart).FirstOrFallback();
		if (bodyPartRecord != null)
		{
			Hediff firstHediffOfDef = TargetPawn.health.hediffSet.GetFirstHediffOfDef(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef);
			if (firstHediffOfDef == null && !compTargetEffect_InstallImplantInOtherPawn.Props.requiresExistingHediff)
			{
				TargetPawn.health.AddHediff(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef, bodyPartRecord, null);
			}
			else if (compTargetEffect_InstallImplantInOtherPawn.Props.canUpgrade)
			{
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
			if (TargetPawn.Map == Find.CurrentMap && compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed != null)
			{
				compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(TargetPawn));
			}
			Item.SplitOff(1).Destroy();
		}
	}
}
