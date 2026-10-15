using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class GameCondition_LavaFlow : GameCondition
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Map map;

		public Predicate<IntVec3> _003C_003E9__0;

		internal bool _003CGetInitialCells_003Eb__0(IntVec3 c)
		{
			if (c.GetTerrain(map) != TerrainDefOf.VolcanicRock && c.GetTerrain(map) != TerrainDefOf.CooledLava)
			{
				return false;
			}
			if (c.Fogged(map))
			{
				return false;
			}
			if (c.GetEdifice(map) != null)
			{
				return false;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetInitialCells_003Ed__10 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private Rot4 _003Cedge_003E5__2;

		private int _003Ccount_003E5__3;

		private int _003Ci_003E5__4;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGetInitialCells_003Ed__10(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00c1;
			}
			_003C_003E1__state = -1;
			_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
			_003C_003E8__1.map = map;
			_003Cedge_003E5__2 = Rot4.Random;
			_003Ccount_003E5__3 = StartPositionCountRange.RandomInRange;
			_003Ci_003E5__4 = 0;
			goto IL_00d3;
			IL_00c1:
			_003Ci_003E5__4++;
			goto IL_00d3;
			IL_00d3:
			if (_003Ci_003E5__4 < _003Ccount_003E5__3)
			{
				if (CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 c)
				{
					if (c.GetTerrain(_003C_003E8__1.map) != TerrainDefOf.VolcanicRock && c.GetTerrain(_003C_003E8__1.map) != TerrainDefOf.CooledLava)
					{
						return false;
					}
					if (c.Fogged(_003C_003E8__1.map))
					{
						return false;
					}
					return c.GetEdifice(_003C_003E8__1.map) == null;
				}, _003C_003E8__1.map, _003Cedge_003E5__2, 0f, out var result))
				{
					_003C_003E2__current = result;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00c1;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetInitialCells_003Ed__10 _003CGetInitialCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInitialCells_003Ed__ = this;
			}
			else
			{
				_003CGetInitialCells_003Ed__ = new _003CGetInitialCells_003Ed__10(0);
			}
			_003CGetInitialCells_003Ed__.map = _003C_003E3__map;
			return _003CGetInitialCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private HashSet<IntVec3> openCells = new HashSet<IntVec3>();

	private int maxFlow;

	private const int FlowRateTicks = 5;

	private const int CoolDurationTicks = 60000;

	private static readonly IntRange StartPositionCountRange = new IntRange(3, 6);

	private static readonly IntRange MaxFlowRange = new IntRange(20000, 32000);

	private List<IntVec3> tempAdjCells;

	private int FlowDurationTicks => maxFlow * 5;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref openCells, "openCells", LookMode.Value);
		Scribe_Values.Look(ref maxFlow, "maxFlow", 0);
	}

	public override void Init()
	{
		base.Init();
		maxFlow = MaxFlowRange.RandomInRange;
		foreach (IntVec3 initialCell in GetInitialCells(base.SingleMap))
		{
			base.SingleMap.terrainGrid.SetTempTerrain(initialCell, TerrainDefOf.LavaShallow);
			base.SingleMap.tempTerrain.QueueRemoveTerrain(initialCell, Find.TickManager.TicksGame + FlowDurationTicks + 60000);
			openCells.Add(initialCell);
		}
		if (openCells.Count == 0)
		{
			End();
		}
		else
		{
			Find.LetterStack.ReceiveLetter("LetterLabelLavaFlow".Translate(), "LetterLavaFlow".Translate(), LetterDefOf.ThreatSmall, new LookTargets(openCells.First(), base.SingleMap));
		}
	}

	[IteratorStateMachine(typeof(_003CGetInitialCells_003Ed__10))]
	private IEnumerable<IntVec3> GetInitialCells(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInitialCells_003Ed__10(-2)
		{
			_003C_003E3__map = map
		};
	}

	public override void GameConditionTick()
	{
		if (base.SingleMap.IsHashIntervalTick(5))
		{
			SpreadLava();
			if (Find.TickManager.TicksGame > startTick + FlowDurationTicks || openCells.Count == 0)
			{
				End();
			}
		}
	}

	private void SpreadLava()
	{
		if (!openCells.TryRandomElementByWeight((IntVec3 c) => Mathf.Max(AdjacentLavaCells(c), 1) * 4, out var result))
		{
			result = openCells.RandomElement();
		}
		if (tempAdjCells == null)
		{
			tempAdjCells = GenAdj.CardinalDirections.ToList();
		}
		tempAdjCells.Shuffle();
		bool flag = true;
		foreach (IntVec3 tempAdjCell in tempAdjCells)
		{
			IntVec3 intVec = result + tempAdjCell;
			if (CanLavaPotentiallySpreadInto(intVec))
			{
				flag = false;
			}
			if (CanLavaSpreadInto(intVec))
			{
				base.SingleMap.terrainGrid.SetTempTerrain(intVec, TerrainDefOf.LavaShallow);
				base.SingleMap.tempTerrain.QueueRemoveTerrain(intVec, Find.TickManager.TicksGame + FlowDurationTicks + 60000);
				openCells.Add(intVec);
				break;
			}
		}
		if (flag)
		{
			openCells.Remove(result);
		}
	}

	private bool CanLavaPotentiallySpreadInto(IntVec3 c)
	{
		if (!c.InBounds(base.SingleMap))
		{
			return false;
		}
		if (openCells.Contains(c))
		{
			return false;
		}
		if (base.SingleMap.terrainGrid.FoundationAt(c) != null)
		{
			return false;
		}
		if ((base.SingleMap.terrainGrid.UnderTerrainAt(c) ?? base.SingleMap.terrainGrid.TopTerrainAt(c)) != TerrainDefOf.VolcanicRock)
		{
			return false;
		}
		if (base.SingleMap.terrainGrid.TerrainAt(c) == TerrainDefOf.LavaShallow)
		{
			return false;
		}
		return true;
	}

	private bool CanLavaSpreadInto(IntVec3 c)
	{
		if (!CanLavaPotentiallySpreadInto(c))
		{
			return false;
		}
		Building edifice = c.GetEdifice(base.SingleMap);
		if (edifice != null && !edifice.IsClearableFreeBuilding)
		{
			return false;
		}
		return true;
	}

	private int AdjacentLavaCells(IntVec3 c)
	{
		int num = 0;
		IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
		foreach (IntVec3 intVec in cardinalDirections)
		{
			IntVec3 c2 = c + intVec;
			if (c2.InBounds(base.SingleMap) && c2.GetTerrain(base.SingleMap) == TerrainDefOf.LavaShallow)
			{
				num++;
			}
		}
		return num;
	}
}
