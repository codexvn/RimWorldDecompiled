using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class SeasonalFlood : Flood
{
	[CompilerGenerated]
	private sealed class _003CGetInitialCells_003Ed__8 : IEnumerable<(IntVec3, int)>, IEnumerable, IEnumerator<(IntVec3, int)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (IntVec3, int) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		public SeasonalFlood _003C_003E4__this;

		private List<IntVec3>.Enumerator _003C_003E7__wrap1;

		(IntVec3, int) IEnumerator<(IntVec3, int)>.Current
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
		public _003CGetInitialCells_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<IntVec3>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SeasonalFlood seasonalFlood = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!ModsConfig.OdysseyActive)
					{
						return false;
					}
					List<IntVec3> list = new List<IntVec3>();
					foreach (IntVec3 allCell in map.AllCells)
					{
						if (allCell.GetTerrain(map).IsRiver)
						{
							list.Add(allCell);
						}
					}
					foreach (IntVec3 item in list.ToList())
					{
						bool flag = false;
						IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
						foreach (IntVec3 intVec in cardinalDirections)
						{
							IntVec3 c = item + intVec;
							if (c.InBounds(seasonalFlood.Map) && !c.GetTerrain(map).IsRiver)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Remove(item);
						}
					}
					list.Shuffle();
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = (current3, Flood.FloodWidthRange.RandomInRange);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<IntVec3>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(IntVec3, int)> IEnumerable<(IntVec3, int)>.GetEnumerator()
		{
			_003CGetInitialCells_003Ed__8 _003CGetInitialCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInitialCells_003Ed__ = this;
			}
			else
			{
				_003CGetInitialCells_003Ed__ = new _003CGetInitialCells_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetInitialCells_003Ed__.map = _003C_003E3__map;
			return _003CGetInitialCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(IntVec3, int)>)this).GetEnumerator();
		}
	}

	private const int RecedeIntervalTicks = 1;

	private static readonly IntRange FloodedTicksRange = new IntRange(240000, 360000);

	private int remainFloodedTicks;

	protected override int MaxFloodDurationTicks => 240000;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref remainFloodedTicks, "floodedTicks", 0);
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return;
		}
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			remainFloodedTicks = FloodedTicksRange.RandomInRange;
			CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.GetTerrain(map).IsRiver && !c.Fogged(map), out var result);
			Find.LetterStack.ReceiveLetter("LetterLabelSeasonalFlooding".Translate(), "LetterSeasonalFlooding".Translate(), LetterDefOf.ThreatSmall, new LookTargets(result, map));
		}
	}

	protected override void Tick()
	{
		if (base.Map.mapTemperature.OutdoorTemp < -7f)
		{
			Messages.Message("MessageSeasonalFloodingFrozen".Translate(), MessageTypeDefOf.NeutralEvent);
			Destroy();
			return;
		}
		base.Tick();
		if (!base.Destroyed && remainFloodedTicks > 0 && Find.TickManager.TicksGame > spawnedTick + base.FloodingTicks + remainFloodedTicks)
		{
			Messages.Message("MessageFloodingSubsiding".Translate(), MessageTypeDefOf.NeutralEvent);
			Destroy();
		}
	}

	[IteratorStateMachine(typeof(_003CGetInitialCells_003Ed__8))]
	protected override IEnumerable<(IntVec3, int)> GetInitialCells(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInitialCells_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map
		};
	}

	protected override void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain)
	{
		int num = estimatedFloodedTiles - floodedTileCount;
		base.Map.terrainGrid.SetTempTerrain(cell, TerrainDefOf.ShallowFloodwater);
		base.Map.tempTerrain.QueueRemoveTerrain(cell, spawnedTick + base.FloodingTicks + remainFloodedTicks + num);
	}
}
