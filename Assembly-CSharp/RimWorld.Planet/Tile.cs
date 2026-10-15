using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class Tile : IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass57_0
	{
		public WorldObjectDef local;

		public Tile _003C_003E4__this;

		public Action _003C_003E9__5;

		internal void _003CGetGizmos_003Eb__4()
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(local);
				mapParent.Tile = _003C_003E4__this.tile;
				mapParent.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(mapParent);
				Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(_003C_003E4__this.tile, new IntVec3(100, 1, 100), null);
				Current.Game.CurrentMap = orGenerateMap;
				CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap);
			}, "GeneratingMap", doAsynchronously: true, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap, showExtraUIInfo: true, forceHideUI: false, delegate
			{
				MapParent mapParent2 = Find.WorldObjects.MapParentAt(Find.WorldSelector.SelectedTile);
				if (mapParent2 != null)
				{
					Current.Game.CurrentMap = mapParent2.Map;
					CameraJumper.TryJump(mapParent2.Map.Center, mapParent2.Map);
				}
			});
		}

		internal void _003CGetGizmos_003Eb__5()
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(local);
			mapParent.Tile = _003C_003E4__this.tile;
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(_003C_003E4__this.tile, new IntVec3(100, 1, 100), null);
			Current.Game.CurrentMap = orGenerateMap;
			CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__57 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tile _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__57(int _003C_003E1__state)
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
			Tile CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Generate settlement",
					action = delegate
					{
						LongEventHandler.QueueLongEvent(delegate
						{
							MapParent mapParent3 = (MapParent)WorldObjectMaker.MakeWorldObject(Find.WorldSelector.SelectedTile.LayerDef.SettlementWorldObjectDef);
							mapParent3.Tile = Find.WorldSelector.SelectedTile;
							mapParent3.SetFaction(Faction.OfPlayer);
							Find.WorldObjects.Add(mapParent3);
							GetOrGenerateMapUtility.GetOrGenerateMap(mapParent3.Tile, Find.World.info.initialMapSize, null);
						}, "GeneratingMap", doAsynchronously: true, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap, showExtraUIInfo: true, forceHideUI: false, delegate
						{
							MapParent mapParent4 = Find.WorldObjects.MapParentAt(Find.WorldSelector.SelectedTile);
							if (mapParent4 != null)
							{
								Current.Game.CurrentMap = mapParent4.Map;
								CameraJumper.TryJump(mapParent4.Map.Center, mapParent4.Map);
							}
						});
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Generate map",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						foreach (WorldObjectDef item in DefDatabase<WorldObjectDef>.AllDefsListForReading)
						{
							_003C_003Ec__DisplayClass57_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass57_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals1,
								local = item
							};
							list.Add(new FloatMenuOption(item.label, delegate
							{
								LongEventHandler.QueueLongEvent(delegate
								{
									MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(CS_0024_003C_003E8__locals5.local);
									mapParent.Tile = CS_0024_003C_003E8__locals5._003C_003E4__this.tile;
									mapParent.SetFaction(Faction.OfPlayer);
									Find.WorldObjects.Add(mapParent);
									Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(CS_0024_003C_003E8__locals5._003C_003E4__this.tile, new IntVec3(100, 1, 100), null);
									Current.Game.CurrentMap = orGenerateMap;
									CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap);
								}, "GeneratingMap", doAsynchronously: true, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap, showExtraUIInfo: true, forceHideUI: false, delegate
								{
									MapParent mapParent2 = Find.WorldObjects.MapParentAt(Find.WorldSelector.SelectedTile);
									if (mapParent2 != null)
									{
										Current.Game.CurrentMap = mapParent2.Map;
										CameraJumper.TryJump(mapParent2.Map.Center, mapParent2.Map);
									}
								});
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list, "Select type"));
					}
				};
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__57 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__57(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Biomes_003Ed__18 : IEnumerable<BiomeDef>, IEnumerable, IEnumerator<BiomeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BiomeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tile _003C_003E4__this;

		BiomeDef IEnumerator<BiomeDef>.Current
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
		public _003Cget_Biomes_003Ed__18(int _003C_003E1__state)
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
			Tile tile = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = tile.biome;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (!tile.tmpHasSecondaryBiome.HasValue)
				{
					TileMutatorDef tileMutatorDef = tile.Mutators.FirstOrDefault((TileMutatorDef m) => m.Worker is TileMutatorWorker_MixedBiome);
					if (tileMutatorDef != null)
					{
						tile.tmpHasSecondaryBiome = true;
						tile.tmpSecondaryBiome = (tileMutatorDef.Worker as TileMutatorWorker_MixedBiome)?.SecondaryBiome(tile.tile, tile.Layer);
					}
					else
					{
						tile.tmpHasSecondaryBiome = false;
						tile.tmpSecondaryBiome = null;
					}
				}
				if (tile.tmpHasSecondaryBiome.Value)
				{
					_003C_003E2__current = tile.tmpSecondaryBiome;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<BiomeDef> IEnumerable<BiomeDef>.GetEnumerator()
		{
			_003Cget_Biomes_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Biomes_003Ed__18(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BiomeDef>)this).GetEnumerator();
		}
	}

	private BiomeDef biome;

	public float elevation = 100f;

	public Hilliness hilliness;

	public float temperature = 20f;

	public float rainfall;

	public float swampiness;

	public WorldFeature feature;

	public float pollution;

	public PlanetTile tile;

	public List<TileMutatorDef> mutatorsNullable;

	[Unsaved(false)]
	private bool? tmpHasSecondaryBiome;

	[Unsaved(false)]
	private BiomeDef tmpSecondaryBiome;

	private Hilliness? hillinessLabelCached;

	private float? cachedMaxTemp;

	private float? cachedMinTemp;

	public PlanetLayer Layer => tile.Layer;

	public BiomeDef PrimaryBiome
	{
		get
		{
			return biome;
		}
		set
		{
			biome = value;
		}
	}

	public IEnumerable<BiomeDef> Biomes
	{
		[IteratorStateMachine(typeof(_003Cget_Biomes_003Ed__18))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Biomes_003Ed__18(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Landmark Landmark
	{
		get
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			return Find.World.landmarks[tile];
		}
	}

	public bool OnSurface => tile.Layer.IsRootSurface;

	public virtual bool WaterCovered => false;

	public bool IsCoastal => Find.World.CoastDirectionAt(tile) != Rot4.Invalid;

	public IList<TileMutatorDef> Mutators
	{
		get
		{
			IList<TileMutatorDef> list = mutatorsNullable;
			return list ?? Array.Empty<TileMutatorDef>();
		}
	}

	public Hilliness HillinessLabel
	{
		get
		{
			if (hillinessLabelCached.HasValue)
			{
				return hillinessLabelCached.Value;
			}
			hillinessLabelCached = hilliness;
			foreach (TileMutatorDef mutator in Mutators)
			{
				if (mutator.hillinessLabel != 0)
				{
					hillinessLabelCached = mutator.hillinessLabel;
				}
			}
			return hillinessLabelCached.Value;
		}
	}

	public Hilliness HillinessForElevationGen
	{
		get
		{
			foreach (TileMutatorDef mutator in Mutators)
			{
				if (mutator.hillinessForElevationGen != 0)
				{
					return mutator.hillinessForElevationGen;
				}
			}
			return hilliness;
		}
	}

	public Hilliness HillinessForOreGeneration
	{
		get
		{
			foreach (TileMutatorDef mutator in Mutators)
			{
				if (mutator.hillinessForOreGeneration != 0)
				{
					return mutator.hillinessForOreGeneration;
				}
			}
			return HillinessForElevationGen;
		}
	}

	public float AnimalDensity
	{
		get
		{
			float num = biome.animalDensity;
			foreach (TileMutatorDef mutator in Mutators)
			{
				num *= mutator.animalDensityFactor;
			}
			return num;
		}
	}

	public float PlantDensityFactor
	{
		get
		{
			float num = 1f;
			foreach (TileMutatorDef mutator in Mutators)
			{
				num *= mutator.plantDensityFactor;
			}
			return num;
		}
	}

	public float FishPopulationFactor
	{
		get
		{
			float num = 1f;
			foreach (TileMutatorDef mutator in Mutators)
			{
				num *= mutator.fishPopulationFactor;
			}
			return num;
		}
	}

	public bool AllowRoofedEdgeWalkIn
	{
		get
		{
			foreach (TileMutatorDef mutator in Mutators)
			{
				if (mutator.allowRoofedEdgeWalkIn)
				{
					return true;
				}
			}
			return false;
		}
	}

	public float MaxTemperature
	{
		get
		{
			if (!cachedMaxTemp.HasValue)
			{
				cachedMaxTemp = GenTemperature.MaxTemperatureAtTile(tile);
			}
			return cachedMaxTemp.Value;
		}
	}

	public float MinTemperature
	{
		get
		{
			if (!cachedMinTemp.HasValue)
			{
				cachedMinTemp = GenTemperature.MinTemperatureAtTile(tile);
			}
			return cachedMinTemp.Value;
		}
	}

	public float MaxFishPopulation => PrimaryBiome.maxFishPopulation * FishPopulationFactor;

	public Tile()
	{
	}

	public Tile(PlanetTile tile)
	{
		this.tile = tile;
	}

	public void AddMutator(TileMutatorDef mutator)
	{
		if (mutatorsNullable == null)
		{
			mutatorsNullable = new List<TileMutatorDef>();
		}
		for (int num = mutatorsNullable.Count - 1; num >= 0; num--)
		{
			TileMutatorDef tileMutatorDef = mutatorsNullable[num];
			foreach (string category in mutator.categories)
			{
				if (tileMutatorDef.categories.Contains(category))
				{
					if (mutator.priority >= tileMutatorDef.priority)
					{
						mutatorsNullable.Remove(tileMutatorDef);
					}
					else
					{
						Log.Error("Detected mutator conflict: " + tileMutatorDef?.ToString() + " and " + mutator);
					}
				}
			}
			foreach (string overrideCategory in mutator.overrideCategories)
			{
				if (tileMutatorDef.categories.Contains(overrideCategory))
				{
					mutatorsNullable.Remove(tileMutatorDef);
				}
			}
		}
		mutatorsNullable.Add(mutator);
		mutatorsNullable.SortBy((TileMutatorDef m) => m.genOrder);
		mutator.Worker?.OnAddedToTile(tile);
	}

	public void RemoveMutator(TileMutatorDef mutator)
	{
		mutatorsNullable?.Remove(mutator);
	}

	public override string ToString()
	{
		return $"({biome} elev={elevation}m hill={hilliness} temp={temperature}°C rain={rainfall}mm swampiness={swampiness.ToStringPercent()} (allowed={biome.allowRoads}) (allowed={biome.allowRivers}))";
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__57))]
	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__57(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref biome, "biome");
		Scribe_Values.Look(ref tile, "tile");
		Scribe_Values.Look(ref elevation, "elevation", 100f);
		Scribe_Values.Look(ref hilliness, "hilliness", Hilliness.Undefined);
		Scribe_Values.Look(ref temperature, "temperature", 20f);
		Scribe_Values.Look(ref rainfall, "rainfall", 0f);
		Scribe_Values.Look(ref swampiness, "swampiness", 0f);
		Scribe_Values.Look(ref pollution, "pollution", 0f);
		Scribe_Deep.Look(ref feature, "feature");
		Scribe_Collections.Look(ref mutatorsNullable, "mutatorDefs", LookMode.Def);
	}
}
