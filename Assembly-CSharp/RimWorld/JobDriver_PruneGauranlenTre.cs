using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PruneGauranlenTre : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PruneGauranlenTre _003C_003E4__this;

		private Toil _003CfindAdjacentCell_003E5__2;

		private Toil _003CgoToAdjacentCell_003E5__3;

		private Toil _003Cprune_003E5__4;

		private int _003Ci_003E5__5;

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
			_003CfindAdjacentCell_003E5__2 = null;
			_003CgoToAdjacentCell_003E5__3 = null;
			_003Cprune_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PruneGauranlenTre CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals13.FailOnDestroyedOrNull(TargetIndex.A);
				int ticks = Mathf.RoundToInt(2500f / CS_0024_003C_003E8__locals13.pawn.GetStatValue(StatDefOf.PruningSpeed));
				_003CfindAdjacentCell_003E5__2 = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals13.job.targetB = CS_0024_003C_003E8__locals13.GetAdjacentCell(CS_0024_003C_003E8__locals13.job.GetTarget(TargetIndex.A).Thing);
				});
				_003CgoToAdjacentCell_003E5__3 = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell).FailOn(() => CS_0024_003C_003E8__locals13.TreeConnection.ConnectionStrength >= CS_0024_003C_003E8__locals13.TreeConnection.DesiredConnectionStrength);
				_003Cprune_003E5__4 = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: true).WithEffect(EffecterDefOf.Harvest_MetaOnly, TargetIndex.A, null).WithEffect(EffecterDefOf.GauranlenDebris, TargetIndex.A, null)
					.PlaySustainerOrSound(SoundDefOf.Interact_Prune);
				_003Cprune_003E5__4.AddPreTickIntervalAction(delegate(int delta)
				{
					CS_0024_003C_003E8__locals13.TreeConnection.Prune(delta);
					CS_0024_003C_003E8__locals13.pawn.skills?.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
					if (CS_0024_003C_003E8__locals13.TreeConnection.ConnectionStrength >= CS_0024_003C_003E8__locals13.TreeConnection.DesiredConnectionStrength)
					{
						CS_0024_003C_003E8__locals13.ReadyForNextToil();
					}
				});
				_003Cprune_003E5__4.activeSkill = () => SkillDefOf.Plants;
				_003Ci_003E5__5 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToAdjacentCell_003E5__3;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cprune_003E5__4;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003Ci_003E5__5++;
				break;
			}
			if (_003Ci_003E5__5 < CS_0024_003C_003E8__locals13.numPositions)
			{
				_003C_003E2__current = _003CfindAdjacentCell_003E5__2;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
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

	private int numPositions = 1;

	private const TargetIndex TreeIndex = TargetIndex.A;

	private const TargetIndex AdjacentCellIndex = TargetIndex.B;

	private const int DurationTicks = 2500;

	private const int MaxPositions = 8;

	private CompTreeConnection TreeConnection => job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTreeConnection>();

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		float num = TreeConnection.DesiredConnectionStrength - TreeConnection.ConnectionStrength;
		numPositions = Mathf.Min(8, Mathf.CeilToInt(num / TreeConnection.ConnectionStrengthGainPerHourOfPruning) + 1);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
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

	private IntVec3 GetAdjacentCell(Thing treeThing)
	{
		if ((from x in GenAdj.CellsAdjacent8Way(treeThing)
			where x.InBounds(pawn.Map) && !x.Fogged(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some)
			select x).TryRandomElement(out var result))
		{
			return result;
		}
		return treeThing.Position;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref numPositions, "numPositions", 1);
	}
}
