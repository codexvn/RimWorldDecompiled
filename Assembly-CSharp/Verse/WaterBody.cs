using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class WaterBody : IExposable
{
	[CompilerGenerated]
	private sealed class _003Cget_CommonFishIncludingExtras_003Ed__39 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WaterBody _003C_003E4__this;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_CommonFishIncludingExtras_003Ed__39(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WaterBody waterBody = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (waterBody.commonFish.NullOrEmpty())
					{
						return false;
					}
					_003C_003E7__wrap1 = waterBody.commonFish.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0083;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0083;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0083:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
					if (ModsConfig.BiotechActive && FishingUtility.PollutionToxfishChanceCurve.Evaluate(waterBody.PollutionPct) > 0f)
					{
						_003C_003E2__current = ThingDefOf.Fish_Toxfish;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003Cget_CommonFishIncludingExtras_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_CommonFishIncludingExtras_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public Map map;

	public IntVec3 rootCell;

	public WaterBodyType waterBodyType;

	[Unsaved(false)]
	public HashSet<IntVec3> cells = new HashSet<IntVec3>();

	[Unsaved(false)]
	public CellRect? cachedBounds;

	private bool shouldHaveFish;

	private List<ThingDef> commonFish = new List<ThingDef>();

	private List<ThingDef> uncommonFish = new List<ThingDef>();

	private int cellCount;

	private float population;

	[Unsaved(false)]
	private float cachedPopulationFactor = -1f;

	public int numCellsFrozen;

	public int numCellsPolluted;

	public int numCellsCanFreeze;

	public bool HasFish
	{
		get
		{
			if (commonFish.Count > 0 || uncommonFish.Count > 0)
			{
				return population > 0f;
			}
			return false;
		}
	}

	public float PollutionPct => (float)numCellsPolluted / (float)cellCount;

	public bool TotallyFrozen => numCellsFrozen == cells.Count;

	public int Size => cellCount;

	public float Population
	{
		get
		{
			return population;
		}
		set
		{
			population = value;
			population = Mathf.Clamp(population, 0f, MaxPopulation);
		}
	}

	public float MaxPopulation => Mathf.Max(map.TileInfo.MaxFishPopulation * PopulationFactor, 20f);

	public float PopulationFactor
	{
		get
		{
			if (cachedPopulationFactor < 0f)
			{
				cachedPopulationFactor = FishingUtility.FishPopulationFactorPerBodySizeCurve.Evaluate(Size);
				if (cachedPopulationFactor < 0f)
				{
					cachedPopulationFactor = 0f;
				}
			}
			return cachedPopulationFactor;
		}
	}

	public int CellCount
	{
		get
		{
			return cellCount;
		}
		set
		{
			cellCount = value;
			cachedPopulationFactor = -1f;
			cachedBounds = null;
		}
	}

	public CellRect Bounds
	{
		get
		{
			CellRect valueOrDefault = cachedBounds.GetValueOrDefault();
			if (!cachedBounds.HasValue)
			{
				valueOrDefault = CellRect.FromCellList(cells);
				cachedBounds = valueOrDefault;
			}
			return cachedBounds.Value;
		}
	}

	public IEnumerable<ThingDef> CommonFish => commonFish;

	public IEnumerable<ThingDef> UncommonFish => uncommonFish;

	public IEnumerable<ThingDef> CommonFishIncludingExtras
	{
		[IteratorStateMachine(typeof(_003Cget_CommonFishIncludingExtras_003Ed__39))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_CommonFishIncludingExtras_003Ed__39(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public WaterBody()
	{
	}

	public WaterBody(Map map, IntVec3 rootCell)
	{
		this.map = map;
		this.rootCell = rootCell;
		cellCount = 1;
		waterBodyType = rootCell.GetWaterBodyType(map);
	}

	public void Initialize()
	{
		InitializePopulation();
		SetFishTypes();
		RecacheState();
	}

	public void InitializePopulation()
	{
		population = MaxPopulation;
	}

	public void SetFishTypes()
	{
		if (map.Biome.fishTypes == null)
		{
			return;
		}
		commonFish.Clear();
		uncommonFish.Clear();
		shouldHaveFish = Rand.Chance(FishingUtility.ChanceForCommonFishFromWaterBodySizeCurve.Evaluate(Size));
		if (!shouldHaveFish)
		{
			return;
		}
		bool flag = Rand.Chance(FishingUtility.ChanceForUncommonFishFromWaterBodySizeCurve.Evaluate(Size));
		switch (waterBodyType)
		{
		case WaterBodyType.Freshwater:
		{
			if (map.Biome.fishTypes.freshwater_Common.TryRandomElement(out var result3))
			{
				commonFish.Add(result3.fishDef);
			}
			if (flag && map.Biome.fishTypes.freshwater_Uncommon.TryRandomElement(out var result4))
			{
				uncommonFish.Add(result4.fishDef);
			}
			break;
		}
		case WaterBodyType.Saltwater:
		{
			if (map.Biome.fishTypes.saltwater_Common.TryRandomElement(out var result))
			{
				commonFish.Add(result.fishDef);
			}
			if (flag && map.Biome.fishTypes.saltwater_Uncommon.TryRandomElement(out var result2))
			{
				uncommonFish.Add(result2.fishDef);
			}
			break;
		}
		}
	}

	public void RecacheState()
	{
		numCellsPolluted = 0;
		numCellsFrozen = 0;
		numCellsCanFreeze = 0;
		foreach (IntVec3 cell in cells)
		{
			if (cell.GetTerrain(map) == TerrainDefOf.ThinIce)
			{
				numCellsFrozen++;
			}
			if (map.terrainGrid.BaseTerrainAt(cell).canFreeze)
			{
				numCellsCanFreeze++;
			}
			if (cell.IsPolluted(map))
			{
				numCellsPolluted++;
			}
		}
		if (shouldHaveFish && commonFish.NullOrEmpty())
		{
			SetFishTypes();
		}
	}

	public void Notify_TerrainChanged(IntVec3 cell, TerrainDef oldTerr, TerrainDef newTerr)
	{
		if (cells.Contains(cell))
		{
			if (newTerr == TerrainDefOf.ThinIce && oldTerr != TerrainDefOf.ThinIce)
			{
				numCellsFrozen++;
			}
			if (newTerr != TerrainDefOf.ThinIce && oldTerr == TerrainDefOf.ThinIce)
			{
				numCellsFrozen--;
			}
		}
	}

	public void Notify_PollutionChanged(IntVec3 cell, bool isPolluted)
	{
		if (cells.Contains(cell))
		{
			if (isPolluted)
			{
				numCellsPolluted++;
			}
			else
			{
				numCellsPolluted--;
			}
		}
	}

	public override int GetHashCode()
	{
		return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(base.GetHashCode(), rootCell.GetHashCode()), cellCount), waterBodyType.GetHashCode());
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref rootCell, "rootCell");
		Scribe_Values.Look(ref cellCount, "cellCount", 0);
		Scribe_Values.Look(ref waterBodyType, "fishType", WaterBodyType.None);
		Scribe_Values.Look(ref population, "population", 0f);
		Scribe_Values.Look(ref shouldHaveFish, "shouldHaveFish", defaultValue: false);
		Scribe_Collections.Look(ref commonFish, "commonFish", LookMode.Def);
		Scribe_Collections.Look(ref uncommonFish, "uncommonFish", LookMode.Def);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (!commonFish.NullOrEmpty())
			{
				shouldHaveFish = true;
			}
			if (shouldHaveFish && commonFish == null)
			{
				commonFish = new List<ThingDef>();
				uncommonFish = new List<ThingDef>();
				SetFishTypes();
			}
			if (shouldHaveFish && Population > 0f && commonFish.Count == 0)
			{
				SetFishTypes();
			}
			commonFish.RemoveAll((ThingDef x) => x == null);
			uncommonFish.RemoveAll((ThingDef x) => x == null);
		}
	}
}
