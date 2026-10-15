using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ForceTargetWear : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__16 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ForceTargetWear _003C_003E4__this;

		private bool _003CusingApparelSource_003E5__2;

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
		public _003CMakeNewToils_003Ed__16(int _003C_003E1__state)
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
			JobDriver_ForceTargetWear CS_0024_003C_003E8__locals27 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals27.FailOnBurningImmobile(TargetIndex.B);
				CS_0024_003C_003E8__locals27.FailOnAggroMentalState(TargetIndex.A);
				_003CusingApparelSource_003E5__2 = CS_0024_003C_003E8__locals27.TargetIsOnApparelSource;
				if (_003CusingApparelSource_003E5__2)
				{
					_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.C);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b2;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00b2;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0118;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0118;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.A, null, storageMode: true);
				_003C_003E1__state = 7;
				return true;
			case 7:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					PawnUtility.ForceWait(CS_0024_003C_003E8__locals27.TargetPawn, CS_0024_003C_003E8__locals27.duration + 360, null, maintainPosture: true, maintainSleep: true);
				};
				toil2.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals27.unequipBuffer += delta;
					CS_0024_003C_003E8__locals27.TryUnequipSomething();
					CS_0024_003C_003E8__locals27.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals27.TargetPawn);
				};
				toil2.WithProgressBarToilDelay(TargetIndex.B);
				toil2.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				toil2.defaultCompleteMode = ToilCompleteMode.Delay;
				toil2.defaultDuration = CS_0024_003C_003E8__locals27.duration;
				toil2.handlingFacing = true;
				toil2.PlaySustainerOrSound(CS_0024_003C_003E8__locals27.GetCurrentWearSound);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 8;
				return true;
			}
			case 8:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Do(delegate
				{
					Apparel apparel = CS_0024_003C_003E8__locals27.Apparel;
					CS_0024_003C_003E8__locals27.TargetPawn.apparel.Wear(apparel);
					if (CS_0024_003C_003E8__locals27.TargetPawn.outfits != null && CS_0024_003C_003E8__locals27.job.playerForced)
					{
						CS_0024_003C_003E8__locals27.TargetPawn.outfits.forcedHandler.SetForced(apparel, forced: true);
					}
				});
				toil.AddFinishAction(delegate
				{
					Pawn targetPawn = CS_0024_003C_003E8__locals27.TargetPawn;
					if (targetPawn != null && targetPawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
					{
						targetPawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}
					targetPawn?.Faction?.Notify_MemberStripped(targetPawn, Faction.OfPlayer);
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 9;
				return true;
			}
			case 9:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0118:
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 5;
				return true;
				IL_00b2:
				if (_003CusingApparelSource_003E5__2)
				{
					Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
					toil3.defaultCompleteMode = ToilCompleteMode.Instant;
					toil3.initAction = delegate
					{
						IApparelSource apparelSource = (IApparelSource)CS_0024_003C_003E8__locals27.Apparel.ParentHolder;
						if (apparelSource == null)
						{
							CS_0024_003C_003E8__locals27.EndJobWith(JobCondition.Incompletable);
						}
						else if (!apparelSource.RemoveApparel(CS_0024_003C_003E8__locals27.Apparel))
						{
							CS_0024_003C_003E8__locals27.EndJobWith(JobCondition.Incompletable);
						}
						else if (!CS_0024_003C_003E8__locals27.pawn.carryTracker.TryStartCarry(CS_0024_003C_003E8__locals27.Apparel))
						{
							CS_0024_003C_003E8__locals27.EndJobWith(JobCondition.Incompletable);
						}
						else
						{
							CS_0024_003C_003E8__locals27.job.SetTarget(TargetIndex.B, CS_0024_003C_003E8__locals27.Apparel);
							CS_0024_003C_003E8__locals27.pawn.records.Increment(RecordDefOf.ThingsHauled);
						}
					};
					_003C_003E2__current = toil3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 4;
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
			_003CMakeNewToils_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__16(0)
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

	private const TargetIndex PawnInd = TargetIndex.A;

	private const TargetIndex ApparelInd = TargetIndex.B;

	private const TargetIndex ApparelSourceIndex = TargetIndex.C;

	private Pawn TargetPawn => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	private Apparel Apparel => (Apparel)job.GetTarget(TargetIndex.B).Thing;

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

	private IApparelSource ApparelSource => (IApparelSource)job.GetTarget(TargetIndex.C).Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref duration, "duration", 0);
		Scribe_Values.Look(ref unequipBuffer, "unequipBuffer", 0);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		job.count = 1;
		if (pawn == TargetPawn)
		{
			Log.Error($"Pawn {pawn} tried to do ForceTargetWear with self as target; this should not happen.");
			return false;
		}
		if (!pawn.Reserve(TargetPawn, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
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
			job.targetC = (Thing)Apparel.ParentHolder;
		}
		duration = (int)(Apparel.GetStatValue(StatDefOf.EquipDelay) * 60f);
		Apparel apparel = Apparel;
		List<Apparel> wornApparel = TargetPawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, TargetPawn.RaceProps.body))
			{
				duration += (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__16))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	private SoundDef GetCurrentWearSound()
	{
		Apparel apparel = Apparel;
		List<Apparel> wornApparel = TargetPawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, TargetPawn.RaceProps.body))
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
		List<Apparel> wornApparel = TargetPawn.apparel.WornApparel;
		for (int num = wornApparel.Count - 1; num >= 0; num--)
		{
			if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[num].def, TargetPawn.RaceProps.body))
			{
				int num2 = (int)(wornApparel[num].GetStatValue(StatDefOf.EquipDelay) * 60f);
				if (unequipBuffer >= num2)
				{
					bool forbid = TargetPawn.Faction != null && TargetPawn.Faction.HostileTo(Faction.OfPlayer);
					if (!TargetPawn.apparel.TryDrop(wornApparel[num], out var _, TargetPawn.PositionHeld, forbid))
					{
						Log.Error(TargetPawn?.ToString() + " could not drop " + wornApparel[num].ToStringSafe());
						EndJobWith(JobCondition.Errored);
					}
				}
				break;
			}
		}
	}
}
