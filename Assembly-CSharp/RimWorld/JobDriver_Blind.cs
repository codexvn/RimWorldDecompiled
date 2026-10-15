using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_Blind : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Pawn target;

		public JobDriver_Blind _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Blind(target, _003C_003E4__this.pawn);
			CreateHistoryEventDef(target);
			SoundDefOf.Execute_Cut.PlayOneShot(target);
			if (target.RaceProps.BloodDef == null)
			{
				return;
			}
			CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
			for (int i = 0; i < 3; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (randomCell.InBounds(_003C_003E4__this.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, _003C_003E4__this.Map))
				{
					FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, target.LabelIndefinite());
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Blind _003C_003E4__this;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private Toil _003Cscarify_003E5__2;

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
			_003C_003E8__1 = null;
			_003Cscarify_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Blind jobDriver_Blind = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckIdeology("Scarify"))
				{
					return false;
				}
				jobDriver_Blind.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.target = jobDriver_Blind.Target;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(35);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Cscarify_003E5__2 = ToilMaker.MakeToil("MakeNewToils");
				_003Cscarify_003E5__2.initAction = delegate
				{
					Blind(_003C_003E8__1.target, _003C_003E8__1._003C_003E4__this.pawn);
					CreateHistoryEventDef(_003C_003E8__1.target);
					SoundDefOf.Execute_Cut.PlayOneShot(_003C_003E8__1.target);
					if (_003C_003E8__1.target.RaceProps.BloodDef != null)
					{
						CellRect cellRect = new CellRect(_003C_003E8__1.target.PositionHeld.x - 1, _003C_003E8__1.target.PositionHeld.z - 1, 3, 3);
						for (int i = 0; i < 3; i++)
						{
							IntVec3 randomCell = cellRect.RandomCell;
							if (randomCell.InBounds(_003C_003E8__1._003C_003E4__this.Map) && GenSight.LineOfSight(randomCell, _003C_003E8__1.target.PositionHeld, _003C_003E8__1._003C_003E4__this.Map))
							{
								FilthMaker.TryMakeFilth(randomCell, _003C_003E8__1.target.MapHeld, _003C_003E8__1.target.RaceProps.BloodDef, _003C_003E8__1.target.LabelIndefinite());
							}
						}
					}
				};
				_003Cscarify_003E5__2.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = Toils_General.Wait(120);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cscarify_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
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

	private const TargetIndex TargetPawnIndex = TargetIndex.A;

	protected Pawn Target => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public static void Blind(Pawn pawn, Pawn doer)
	{
		Lord lord = pawn.GetLord();
		IEnumerable<BodyPartRecord> enumerable = from p in pawn.health.hediffSet.GetNotMissingParts()
			where p.def == BodyPartDefOf.Eye
			select p;
		if (lord != null && lord.LordJob is LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation && enumerable.Count() == 1)
		{
			lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
		}
		foreach (BodyPartRecord item in enumerable)
		{
			if (item.def == BodyPartDefOf.Eye)
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, item));
				break;
			}
		}
		if (pawn.Dead)
		{
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, doer, PawnExecutionKind.GenericBrutal);
		}
	}

	public static void CreateHistoryEventDef(Pawn pawn)
	{
		if (PawnUtility.IsBiologicallyBlind(pawn))
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotBlinded, pawn.Named(HistoryEventArgsNames.Doer)));
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
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
