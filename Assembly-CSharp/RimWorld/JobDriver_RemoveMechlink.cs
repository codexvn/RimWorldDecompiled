using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RemoveMechlink : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RemoveMechlink _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
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
			JobDriver_RemoveMechlink CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckBiotech("Remove mechlink"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals6.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals6.FailOn(() => !CS_0024_003C_003E8__locals6.Corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant));
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_General.Wait(300, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A).WithEffect(() => EffecterDefOf.Surgery, TargetIndex.A, null)
					.PlaySustainerOrSound(SoundDefOf.Recipe_Surgery)
					.PlaySoundAtEnd(SoundDefOf.Mechlink_Removed);
				toil2.handlingFacing = true;
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Pawn innerPawn = CS_0024_003C_003E8__locals6.Corpse.InnerPawn;
					List<Hediff> hediffs = innerPawn.health.hediffSet.hediffs;
					for (int num2 = hediffs.Count - 1; num2 >= 0; num2--)
					{
						if (hediffs[num2].def == HediffDefOf.MechlinkImplant)
						{
							innerPawn.health.RemoveHediff(hediffs[num2]);
						}
					}
					Mechlink obj = (Mechlink)ThingMaker.MakeThing(ThingDefOf.Mechlink);
					obj.sentMechsToPlayer = innerPawn.IsColonist || innerPawn.IsPrisoner || innerPawn.IsSlave || innerPawn.IsQuestLodger();
					GenPlace.TryPlaceThing(obj, CS_0024_003C_003E8__locals6.Corpse.Position, CS_0024_003C_003E8__locals6.Map, ThingPlaceMode.Near, null, null, null);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private const TargetIndex CorpseInd = TargetIndex.A;

	public const int RemoveTicks = 300;

	protected Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Corpse, job, 1, -1, null, errorOnFailed);
	}

	public override string GetReport()
	{
		return "ReportRemovingMechlink".Translate(HediffDefOf.MechlinkImplant.label);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
