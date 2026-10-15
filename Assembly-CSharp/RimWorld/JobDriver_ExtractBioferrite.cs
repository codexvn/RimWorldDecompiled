using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ExtractBioferrite : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExtractBioferrite _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
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
			JobDriver_ExtractBioferrite CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals7.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals7.FailOn(delegate
				{
					Pawn innerPawn2 = CS_0024_003C_003E8__locals7.InnerPawn;
					if (innerPawn2 == null || innerPawn2.Destroyed)
					{
						return true;
					}
					CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn2.TryGetComp<CompHoldingPlatformTarget>();
					return (compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || !compHoldingPlatformTarget.extractBioferrite)) ? true : false;
				});
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				int ticks = (int)(1f / CS_0024_003C_003E8__locals7.pawn.GetStatValue(StatDefOf.MedicalTendSpeed) * 2000f);
				Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: true, maintainPosture: false, maintainSleep: false, TargetIndex.A);
				toil.activeSkill = () => SkillDefOf.Medicine;
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A).PlaySustainerOrSound(SoundDefOf.Recipe_Surgery);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					Pawn innerPawn = CS_0024_003C_003E8__locals7.InnerPawn;
					int num2 = Mathf.FloorToInt(CompProducesBioferrite.BioferritePerDay(innerPawn) * 4f);
					if (num2 > 0)
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite);
						thing.stackCount = num2;
						GenPlace.TryPlaceThing(thing, CS_0024_003C_003E8__locals7.pawn.Position, CS_0024_003C_003E8__locals7.pawn.Map, ThingPlaceMode.Near, null, null, null);
					}
					innerPawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 4f));
					innerPawn.health.AddHediff(HediffDefOf.BioferriteExtracted, null, null);
				});
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const TargetIndex PlatformIndex = TargetIndex.A;

	private const int ExtractDuration = 2000;

	private const int CutDamage = 4;

	private const int BioferriteModifier = 4;

	private Thing Platform => base.TargetThingA;

	private Pawn InnerPawn => (Platform as Building_HoldingPlatform)?.HeldPawn;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		return JobUtility.GetResolvedJobReport(job.def.reportString, InnerPawn, job.targetB, job.targetC);
	}
}
