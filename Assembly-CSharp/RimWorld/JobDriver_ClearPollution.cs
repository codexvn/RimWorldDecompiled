using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ClearPollution : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ClearPollution _003C_003E4__this;

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
			JobDriver_ClearPollution CS_0024_003C_003E8__locals18 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals18.FailOn(() => !CS_0024_003C_003E8__locals18.TargetLocA.IsPolluted(CS_0024_003C_003E8__locals18.Map));
				CS_0024_003C_003E8__locals18.FailOn(() => !CS_0024_003C_003E8__locals18.Map.areaManager.PollutionClear[CS_0024_003C_003E8__locals18.TargetLocA]);
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals18.workDone += CS_0024_003C_003E8__locals18.pawn.GetStatValue(StatDefOf.GeneralLaborSpeed) * (float)delta;
					if (CS_0024_003C_003E8__locals18.workDone >= 5600f)
					{
						CS_0024_003C_003E8__locals18.ClearPollutionAt(CS_0024_003C_003E8__locals18.TargetLocA, CS_0024_003C_003E8__locals18.pawn.Map);
						IntVec3 intVec = CellFinder.FindNoWipeSpawnLocNear(CS_0024_003C_003E8__locals18.TargetLocA, CS_0024_003C_003E8__locals18.pawn.Map, ThingDefOf.Wastepack, Rot4.North);
						GenSpawn.Spawn(ThingDefOf.Wastepack, intVec.IsValid ? intVec : CS_0024_003C_003E8__locals18.TargetLocA, CS_0024_003C_003E8__locals18.pawn.Map);
						CS_0024_003C_003E8__locals18.ReadyForNextToil();
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals18.workDone / 5600f, interpolateBetweenActorAndTarget: true);
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				toil.WithEffect(EffecterDefOf.Clean, TargetIndex.A, null);
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

	private const float WorkAmount = 5600f;

	private const int MaxDistanceToClear = 10;

	private const int MaxDistanceToClearSquared = 100;

	private const int PollutionCellsToClearPerJob = 6;

	private const TargetIndex CleanCellIndex = TargetIndex.A;

	private float workDone;

	private static List<IntVec3> tmpCellsToClear = new List<IntVec3>();

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workDone, "workDone", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
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

	private void ClearPollutionAt(IntVec3 root, Map map)
	{
		tmpCellsToClear.Clear();
		tmpCellsToClear.AddRange(map.areaManager.PollutionClear.ActiveCells.Where((IntVec3 c) => c.DistanceToSquared(root) < 100));
		tmpCellsToClear.SortBy((IntVec3 c) => c.DistanceToSquared(root));
		int num = 6;
		for (int i = 0; i < tmpCellsToClear.Count; i++)
		{
			if (CanUnpollute(root, map, tmpCellsToClear[i]))
			{
				tmpCellsToClear[i].Unpollute(map);
				num--;
				if (num <= 0)
				{
					break;
				}
			}
		}
		tmpCellsToClear.Clear();
		if (num > 0)
		{
			num -= UnpolluteRadially(root, map, num);
			if (num > 0)
			{
				UnpolluteRadially(root, map, num, ignoreOtherPawnsCleaningCell: true);
			}
		}
	}

	private int UnpolluteRadially(IntVec3 root, Map map, int maxToUnpollute = 5, bool ignoreOtherPawnsCleaningCell = false)
	{
		int num = 0;
		foreach (IntVec3 item in GenRadial.RadialCellsAround(root, 10f, useCenter: true))
		{
			if (CanUnpollute(root, map, item, ignoreOtherPawnsCleaningCell))
			{
				item.Unpollute(map);
				num++;
				if (num >= maxToUnpollute)
				{
					break;
				}
			}
		}
		return num;
	}

	private bool CanUnpollute(IntVec3 root, Map map, IntVec3 c, bool ignoreOtherPawnsCleaningCell = false)
	{
		if (!c.IsPolluted(map))
		{
			return false;
		}
		if (!ignoreOtherPawnsCleaningCell && AnyOtherPawnCleaning(pawn, c))
		{
			return false;
		}
		if (c.GetRoom(map) != null && root.GetRoom(map) != c.GetRoom(map))
		{
			return false;
		}
		if (c.DistanceToSquared(root) > 100)
		{
			return false;
		}
		return true;
	}

	private bool AnyOtherPawnCleaning(Pawn pawn, IntVec3 cell)
	{
		List<Pawn> freeColonistsSpawned = pawn.Map.mapPawns.FreeColonistsSpawned;
		for (int i = 0; i < freeColonistsSpawned.Count; i++)
		{
			if (freeColonistsSpawned[i] != pawn && freeColonistsSpawned[i].CurJobDef == JobDefOf.ClearPollution)
			{
				LocalTargetInfo target = freeColonistsSpawned[i].CurJob.GetTarget(TargetIndex.A);
				if (target.IsValid && target.Cell == cell)
				{
					return true;
				}
			}
		}
		return false;
	}
}
