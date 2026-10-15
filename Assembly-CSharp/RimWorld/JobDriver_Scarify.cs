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

public class JobDriver_Scarify : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Pawn target;

		public JobDriver_Scarify _003C_003E4__this;

		public Func<BodyPartRecord, bool> _003C_003E9__1;

		internal void _003CMakeNewToils_003Eb__0()
		{
			if (!(from part in GetPartsToApplyOn(target)
				where AvailableOnNow(target, part)
				select part).TryRandomElement(out var result) && !GetPartsToApplyOn(target).TryRandomElement(out result))
			{
				_003C_003E4__this.pawn.jobs.EndCurrentJob(JobCondition.Errored);
				Log.Error("Failed to find body part to scarify");
			}
			Scarify(target, result);
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

		internal bool _003CMakeNewToils_003Eb__1(BodyPartRecord part)
		{
			return AvailableOnNow(target, part);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPartsToApplyOn_003Ed__5 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<BodyPartRecord> _003C_003E7__wrap1;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetPartsToApplyOn_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn.health.hediffSet.GetNotMissingParts().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					BodyPartRecord current = _003C_003E7__wrap1.Current;
					if (current.def.canScarify)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetPartsToApplyOn_003Ed__5 _003CGetPartsToApplyOn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPartsToApplyOn_003Ed__ = this;
			}
			else
			{
				_003CGetPartsToApplyOn_003Ed__ = new _003CGetPartsToApplyOn_003Ed__5(0);
			}
			_003CGetPartsToApplyOn_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetPartsToApplyOn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Scarify _003C_003E4__this;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Scarify jobDriver_Scarify = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckIdeology("Scarify"))
				{
					return false;
				}
				jobDriver_Scarify.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.target = jobDriver_Scarify.Target;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(35);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (!(from part in GetPartsToApplyOn(_003C_003E8__1.target)
						where AvailableOnNow(_003C_003E8__1.target, part)
						select part).TryRandomElement(out var result) && !GetPartsToApplyOn(_003C_003E8__1.target).TryRandomElement(out result))
					{
						_003C_003E8__1._003C_003E4__this.pawn.jobs.EndCurrentJob(JobCondition.Errored);
						Log.Error("Failed to find body part to scarify");
					}
					Scarify(_003C_003E8__1.target, result);
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

	private const TargetIndex TargetPawnIndex = TargetIndex.A;

	public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[2]
	{
		new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.8f),
		new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f)
	};

	protected Pawn Target => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public static bool AvailableOnNow(Pawn pawn, BodyPartRecord part = null)
	{
		if (!pawn.RaceProps.Humanlike || (Faction.OfPlayerSilentFail != null && !Faction.OfPlayer.ideos.AnyPreceptWithRequiredScars()))
		{
			return false;
		}
		if (part != null && (pawn.health.WouldDieAfterAddingHediff(HediffDefOf.Scarification, part, 1f) || pawn.health.WouldLosePartAfterAddingHediff(HediffDefOf.Scarification, part, 1f)))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetPartsToApplyOn_003Ed__5))]
	public static IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPartsToApplyOn_003Ed__5(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static void Scarify(Pawn pawn, BodyPartRecord part)
	{
		if (ModLister.CheckIdeology("Scarification"))
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation)
			{
				lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Scarification, pawn, part);
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff.TryGetComp<HediffComp_GetsPermanent>();
			hediffComp_GetsPermanent.IsPermanent = true;
			hediffComp_GetsPermanent.SetPainCategory(InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted e) => e.weight).category);
			pawn.health.AddHediff(hediff, null, null);
		}
	}

	public static void CreateHistoryEventDef(Pawn pawn)
	{
		Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotScarified, pawn.Named(HistoryEventArgsNames.Doer)));
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
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
}
