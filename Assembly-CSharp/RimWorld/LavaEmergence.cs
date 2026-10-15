using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class LavaEmergence : Thing
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public Map map;

		public HashSet<IntVec3> lavaDeepCells;

		internal bool _003CGetInitialCells_003Eb__0(IntVec3 c)
		{
			if (c.GetTerrain(map) == TerrainDefOf.LavaDeep)
			{
				return !c.Fogged(map);
			}
			return false;
		}

		internal bool _003CGetInitialCells_003Eb__1(IntVec3 c)
		{
			return c.GetTerrain(map) == TerrainDefOf.LavaDeep;
		}

		internal void _003CGetInitialCells_003Eb__2(IntVec3 c)
		{
			lavaDeepCells.Add(c);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetInitialCells_003Ed__16 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CGetInitialCells_003Ed__16(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0();
				_003C_003E8__1.map = map;
				if (!CellFinder.TryFindRandomCell(_003C_003E8__1.map, (IntVec3 c) => c.GetTerrain(_003C_003E8__1.map) == TerrainDefOf.LavaDeep && !c.Fogged(_003C_003E8__1.map), out var result))
				{
					return false;
				}
				_003C_003E8__1.lavaDeepCells = new HashSet<IntVec3>();
				_003C_003E8__1.map.floodFiller.FloodFill(result, (IntVec3 c) => c.GetTerrain(_003C_003E8__1.map) == TerrainDefOf.LavaDeep, delegate(IntVec3 c)
				{
					_003C_003E8__1.lavaDeepCells.Add(c);
				});
				foreach (IntVec3 item in _003C_003E8__1.lavaDeepCells.ToList())
				{
					bool flag = false;
					IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
					foreach (IntVec3 intVec in cardinalDirections)
					{
						IntVec3 c2 = item + intVec;
						if (c2.InBounds(_003C_003E8__1.map) && c2.GetEdifice(_003C_003E8__1.map) == null && c2.GetTerrain(_003C_003E8__1.map) != TerrainDefOf.LavaDeep)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						_003C_003E8__1.lavaDeepCells.Remove(item);
					}
				}
				if (_003C_003E8__1.lavaDeepCells.Count == 0)
				{
					return false;
				}
				_003Ccount_003E5__2 = EmergencePointsCountRange.RandomInRange;
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003Ccount_003E5__2)
			{
				IntVec3 intVec2 = _003C_003E8__1.lavaDeepCells.RandomElement();
				_003C_003E8__1.lavaDeepCells.Remove(intVec2);
				IntVec3 intVec3 = intVec2;
				IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
				foreach (IntVec3 intVec4 in cardinalDirections)
				{
					IntVec3 intVec5 = intVec2 + intVec4;
					if (intVec5.InBounds(_003C_003E8__1.map) && intVec5.GetTerrain(_003C_003E8__1.map) != TerrainDefOf.LavaDeep)
					{
						intVec3 = intVec5;
						break;
					}
				}
				_003C_003E2__current = intVec3;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetInitialCells_003Ed__16 _003CGetInitialCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInitialCells_003Ed__ = this;
			}
			else
			{
				_003CGetInitialCells_003Ed__ = new _003CGetInitialCells_003Ed__16(0);
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

	private static readonly IntRange EmergencePointsCountRange = new IntRange(2, 4);

	private static readonly IntRange LavaBeginCoolingDelay = new IntRange(300000, 360000);

	private const int LavaCoolInterval = 400;

	private const int ExpandIntervalTicks = 480;

	[Unsaved(false)]
	public int forcePoolSize = -1;

	[Unsaved(false)]
	public int forceCoolDelay = -1;

	protected HashSet<IntVec3> openCells = new HashSet<IntVec3>();

	protected HashSet<IntVec3> lavaCells = new HashSet<IntVec3>();

	protected int poolSize;

	private int coolDelay;

	protected virtual IntRange PoolSizeRange => new IntRange(450, 600);

	protected virtual bool FireLetter => true;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref openCells, "openCells", LookMode.Value);
		Scribe_Collections.Look(ref lavaCells, "lavaCells", LookMode.Value);
		Scribe_Values.Look(ref poolSize, "poolSize", 0);
		Scribe_Values.Look(ref coolDelay, "coolDelay", 0);
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (respawningAfterLoad)
		{
			return;
		}
		openCells.AddRange(GetInitialCells(map));
		if (openCells.Count == 0)
		{
			Destroy();
			return;
		}
		poolSize = ((forcePoolSize > 0) ? forcePoolSize : PoolSizeRange.RandomInRange);
		coolDelay = ((forceCoolDelay > 0) ? forceCoolDelay : LavaBeginCoolingDelay.RandomInRange);
		if (FireLetter)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelLavaEmergence".Translate(), "LetterLavaEmergence".Translate(), LetterDefOf.ThreatSmall, new LookTargets(openCells.First(), base.Map));
		}
	}

	[IteratorStateMachine(typeof(_003CGetInitialCells_003Ed__16))]
	protected virtual IEnumerable<IntVec3> GetInitialCells(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInitialCells_003Ed__16(-2)
		{
			_003C_003E3__map = map
		};
	}

	protected override void Tick()
	{
		if (this.IsHashIntervalTick(480))
		{
			SpreadLava();
			if (lavaCells.Count >= poolSize || openCells.Count == 0)
			{
				BeginCooling();
			}
		}
	}

	protected void SpreadLava()
	{
		if (openCells.Count == 0)
		{
			return;
		}
		if (!openCells.TryRandomElementByWeight((IntVec3 c) => Mathf.Max(AdjacentLavaCells(c), 1) * 4, out var result))
		{
			result = openCells.RandomElement();
		}
		openCells.Remove(result);
		IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
		foreach (IntVec3 intVec in cardinalDirections)
		{
			IntVec3 intVec2 = result + intVec;
			if (!openCells.Contains(intVec2) && !lavaCells.Contains(intVec2) && CanLavaSpreadInto(intVec2))
			{
				openCells.Add(intVec2);
			}
		}
		base.Map.terrainGrid.SetTempTerrain(result, TerrainDefOf.LavaShallow);
		lavaCells.Add(result);
	}

	protected void BeginCooling()
	{
		List<IntVec3> cells = lavaCells.ToList();
		int num = 0;
		while (!cells.Empty())
		{
			if (!cells.TryRandomElementByWeight(delegate(IntVec3 c)
			{
				int num2 = 0;
				IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
				foreach (IntVec3 intVec in cardinalDirections)
				{
					IntVec3 intVec2 = c + intVec;
					if (intVec2.InBounds(base.Map) && (cells.Contains(intVec2) || intVec2.GetTerrain(base.Map) == TerrainDefOf.LavaDeep))
					{
						num2++;
					}
				}
				return 16 - num2 * 4;
			}, out var result))
			{
				result = cells.RandomElement();
			}
			base.Map.tempTerrain.QueueRemoveTerrain(result, Find.TickManager.TicksGame + coolDelay + 400 * num);
			cells.Remove(result);
			num++;
		}
		Destroy();
	}

	private bool CanLavaSpreadInto(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		Building edifice = c.GetEdifice(base.Map);
		if (edifice != null && !edifice.IsClearableFreeBuilding)
		{
			return false;
		}
		TerrainDef terrain = c.GetTerrain(base.Map);
		if (!terrain.natural)
		{
			return false;
		}
		if (!terrain.affordances.Contains(TerrainAffordanceDefOf.Walkable))
		{
			return false;
		}
		if (base.Map.terrainGrid.FoundationAt(c) != null)
		{
			return false;
		}
		if (terrain == TerrainDefOf.LavaDeep)
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
			if (c2.InBounds(base.Map) && c2.GetTerrain(base.Map) == TerrainDefOf.LavaShallow)
			{
				num++;
			}
		}
		return num;
	}
}
