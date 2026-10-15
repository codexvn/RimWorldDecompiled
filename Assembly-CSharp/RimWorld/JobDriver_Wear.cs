using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Wear : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public JobDriver_Wear _003C_003E4__this;

		public bool usingSource;

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			_003C_003E4__this.unequipBuffer += delta;
			_003C_003E4__this.TryUnequipSomething();
			_003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E4__this.Apparel.PositionHeld);
		}

		internal void _003CMakeNewToils_003Eb__0()
		{
			Apparel apparel = _003C_003E4__this.Apparel;
			if (usingSource)
			{
				_003C_003E4__this.ApparelSource.RemoveApparel(apparel);
			}
			_003C_003E4__this.pawn.apparel.Wear(apparel);
			if (_003C_003E4__this.pawn.outfits != null && _003C_003E4__this.job.playerForced)
			{
				_003C_003E4__this.pawn.outfits.forcedHandler.SetForced(apparel, forced: true);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Wear _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
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
			JobDriver_Wear jobDriver_Wear = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_Wear.FailOnBurningImmobile(TargetIndex.A);
				_003C_003E8__1.usingSource = jobDriver_Wear.TargetIsOnApparelSource;
				if (_003C_003E8__1.usingSource)
				{
					_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.B);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00bc;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00bc;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					Apparel apparel = _003C_003E8__1._003C_003E4__this.Apparel;
					if (_003C_003E8__1.usingSource)
					{
						_003C_003E8__1._003C_003E4__this.ApparelSource.RemoveApparel(apparel);
					}
					_003C_003E8__1._003C_003E4__this.pawn.apparel.Wear(apparel);
					if (_003C_003E8__1._003C_003E4__this.pawn.outfits != null && _003C_003E8__1._003C_003E4__this.job.playerForced)
					{
						_003C_003E8__1._003C_003E4__this.pawn.outfits.forcedHandler.SetForced(apparel, forced: true);
					}
				});
				_003C_003E1__state = 4;
				return true;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00bc:
				toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					_003C_003E8__1._003C_003E4__this.unequipBuffer = _003C_003E8__1._003C_003E4__this.unequipBuffer + delta;
					_003C_003E8__1._003C_003E4__this.TryUnequipSomething();
					_003C_003E8__1._003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.Apparel.PositionHeld);
				};
				toil.WithProgressBarToilDelay((!_003C_003E8__1.usingSource) ? TargetIndex.A : TargetIndex.B);
				toil.FailOnDespawnedNullOrForbidden((!_003C_003E8__1.usingSource) ? TargetIndex.A : TargetIndex.B);
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = jobDriver_Wear.duration;
				toil.handlingFacing = true;
				toil.PlaySustainerOrSound(jobDriver_Wear.GetCurrentWearSound);
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	private int duration;

	private int unequipBuffer;

	private const TargetIndex ApparelInd = TargetIndex.A;

	private const TargetIndex ApparelSourceIndex = TargetIndex.B;

	private Apparel Apparel => (Apparel)job.GetTarget(TargetIndex.A).Thing;

	private bool TargetIsOnApparelSource
	{
		get
		{
			Apparel apparel = Apparel;
			if (apparel != null && !apparel.Spawned && apparel.ParentHolder is IApparelSource apparelSource)
			{
				return apparelSource is Thing;
			}
			return false;
		}
	}

	private IApparelSource ApparelSource => (IApparelSource)job.GetTarget(TargetIndex.B).Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref duration, "duration", 0);
		Scribe_Values.Look(ref unequipBuffer, "unequipBuffer", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (TargetIsOnApparelSource)
		{
			return pawn.Reserve((Thing)Apparel.ParentHolder, job, 1, -1, null, errorOnFailed);
		}
		return pawn.Reserve(Apparel, job, 1, -1, null, errorOnFailed);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (TargetIsOnApparelSource)
		{
			job.targetB = (Thing)Apparel.ParentHolder;
		}
		duration = (int)(Apparel.GetStatValue(StatDefOf.EquipDelay) * 60f);
		Apparel apparel = Apparel;
		List<Apparel> wornApparel = pawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, pawn.RaceProps.body))
			{
				duration += (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private SoundDef GetCurrentWearSound()
	{
		Apparel apparel = Apparel;
		List<Apparel> wornApparel = pawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, pawn.RaceProps.body))
			{
				if (unequipBuffer >= (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f))
				{
					break;
				}
				return wornApparel[num].def.apparel.soundRemove;
			}
		}
		return apparel.def.apparel.soundWear;
	}

	private void TryUnequipSomething()
	{
		Apparel apparel = Apparel;
		List<Apparel> wornApparel = pawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, pawn.RaceProps.body))
			{
				int num2 = (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f);
				if (unequipBuffer >= num2)
				{
					bool forbid = pawn.Faction != null && pawn.Faction.HostileTo(Faction.OfPlayer);
					if (!pawn.apparel.TryDrop(wornApparel[num], out var _, pawn.PositionHeld, forbid))
					{
						Log.Error(pawn?.ToString() + " could not drop " + wornApparel[num].ToStringSafe());
						EndJobWith(JobCondition.Errored);
					}
				}
				break;
			}
		}
	}
}
