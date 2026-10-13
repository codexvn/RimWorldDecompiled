using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CB3 RID: 15539
	public class Tile : IExposable
	{
		// Token: 0x170034A7 RID: 13479
		// (get) Token: 0x060151F6 RID: 86518 RVA: 0x00652806 File Offset: 0x00650A06
		public PlanetLayer Layer
		{
			get
			{
				return this.tile.Layer;
			}
		}

		// Token: 0x170034A8 RID: 13480
		// (get) Token: 0x060151F7 RID: 86519 RVA: 0x00652813 File Offset: 0x00650A13
		// (set) Token: 0x060151F8 RID: 86520 RVA: 0x0065281B File Offset: 0x00650A1B
		public BiomeDef PrimaryBiome
		{
			get
			{
				return this.biome;
			}
			set
			{
				this.biome = value;
			}
		}

		// Token: 0x170034A9 RID: 13481
		// (get) Token: 0x060151F9 RID: 86521 RVA: 0x00652824 File Offset: 0x00650A24
		public IEnumerable<BiomeDef> Biomes
		{
			get
			{
				yield return this.biome;
				if (this.tmpHasSecondaryBiome == null)
				{
					TileMutatorDef tileMutatorDef = this.Mutators.FirstOrDefault((TileMutatorDef m) => m.Worker is TileMutatorWorker_MixedBiome);
					if (tileMutatorDef != null)
					{
						this.tmpHasSecondaryBiome = new bool?(true);
						TileMutatorWorker_MixedBiome tileMutatorWorker_MixedBiome = tileMutatorDef.Worker as TileMutatorWorker_MixedBiome;
						this.tmpSecondaryBiome = ((tileMutatorWorker_MixedBiome != null) ? tileMutatorWorker_MixedBiome.SecondaryBiome(this.tile, this.Layer) : null);
					}
					else
					{
						this.tmpHasSecondaryBiome = new bool?(false);
						this.tmpSecondaryBiome = null;
					}
				}
				if (this.tmpHasSecondaryBiome.Value)
				{
					yield return this.tmpSecondaryBiome;
				}
				yield break;
			}
		}

		// Token: 0x170034AA RID: 13482
		// (get) Token: 0x060151FA RID: 86522 RVA: 0x00652834 File Offset: 0x00650A34
		public Landmark Landmark
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return null;
				}
				return Find.World.landmarks[this.tile];
			}
		}

		// Token: 0x170034AB RID: 13483
		// (get) Token: 0x060151FB RID: 86523 RVA: 0x00652854 File Offset: 0x00650A54
		public bool OnSurface
		{
			get
			{
				return this.tile.Layer.IsRootSurface;
			}
		}

		// Token: 0x170034AC RID: 13484
		// (get) Token: 0x060151FC RID: 86524 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool WaterCovered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170034AD RID: 13485
		// (get) Token: 0x060151FD RID: 86525 RVA: 0x00652866 File Offset: 0x00650A66
		public bool IsCoastal
		{
			get
			{
				return Find.World.CoastDirectionAt(this.tile) != Rot4.Invalid;
			}
		}

		// Token: 0x170034AE RID: 13486
		// (get) Token: 0x060151FE RID: 86526 RVA: 0x00652884 File Offset: 0x00650A84
		public IList<TileMutatorDef> Mutators
		{
			get
			{
				IList<TileMutatorDef> list = this.mutatorsNullable;
				return list ?? Array.Empty<TileMutatorDef>();
			}
		}

		// Token: 0x170034AF RID: 13487
		// (get) Token: 0x060151FF RID: 86527 RVA: 0x006528A4 File Offset: 0x00650AA4
		public Hilliness HillinessLabel
		{
			get
			{
				if (this.hillinessLabelCached != null)
				{
					return this.hillinessLabelCached.Value;
				}
				this.hillinessLabelCached = new Hilliness?(this.hilliness);
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessLabel != Hilliness.Undefined)
					{
						this.hillinessLabelCached = new Hilliness?(tileMutatorDef.hillinessLabel);
					}
				}
				return this.hillinessLabelCached.Value;
			}
		}

		// Token: 0x170034B0 RID: 13488
		// (get) Token: 0x06015200 RID: 86528 RVA: 0x00652938 File Offset: 0x00650B38
		public Hilliness HillinessForElevationGen
		{
			get
			{
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessForElevationGen != Hilliness.Undefined)
					{
						return tileMutatorDef.hillinessForElevationGen;
					}
				}
				return this.hilliness;
			}
		}

		// Token: 0x170034B1 RID: 13489
		// (get) Token: 0x06015201 RID: 86529 RVA: 0x00652998 File Offset: 0x00650B98
		public Hilliness HillinessForOreGeneration
		{
			get
			{
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessForOreGeneration != Hilliness.Undefined)
					{
						return tileMutatorDef.hillinessForOreGeneration;
					}
				}
				return this.HillinessForElevationGen;
			}
		}

		// Token: 0x170034B2 RID: 13490
		// (get) Token: 0x06015202 RID: 86530 RVA: 0x006529F8 File Offset: 0x00650BF8
		public float AnimalDensity
		{
			get
			{
				float num = this.biome.animalDensity;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.animalDensityFactor;
				}
				return num;
			}
		}

		// Token: 0x170034B3 RID: 13491
		// (get) Token: 0x06015203 RID: 86531 RVA: 0x00652A54 File Offset: 0x00650C54
		public float PlantDensityFactor
		{
			get
			{
				float num = 1f;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.plantDensityFactor;
				}
				return num;
			}
		}

		// Token: 0x170034B4 RID: 13492
		// (get) Token: 0x06015204 RID: 86532 RVA: 0x00652AAC File Offset: 0x00650CAC
		public float FishPopulationFactor
		{
			get
			{
				float num = 1f;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.fishPopulationFactor;
				}
				return num;
			}
		}

		// Token: 0x170034B5 RID: 13493
		// (get) Token: 0x06015205 RID: 86533 RVA: 0x00652B04 File Offset: 0x00650D04
		public bool AllowRoofedEdgeWalkIn
		{
			get
			{
				using (IEnumerator<TileMutatorDef> enumerator = this.Mutators.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.allowRoofedEdgeWalkIn)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170034B6 RID: 13494
		// (get) Token: 0x06015206 RID: 86534 RVA: 0x00652B58 File Offset: 0x00650D58
		public float MaxTemperature
		{
			get
			{
				if (this.cachedMaxTemp == null)
				{
					this.cachedMaxTemp = new float?(GenTemperature.MaxTemperatureAtTile(this.tile));
				}
				return this.cachedMaxTemp.Value;
			}
		}

		// Token: 0x170034B7 RID: 13495
		// (get) Token: 0x06015207 RID: 86535 RVA: 0x00652B88 File Offset: 0x00650D88
		public float MinTemperature
		{
			get
			{
				if (this.cachedMinTemp == null)
				{
					this.cachedMinTemp = new float?(GenTemperature.MinTemperatureAtTile(this.tile));
				}
				return this.cachedMinTemp.Value;
			}
		}

		// Token: 0x170034B8 RID: 13496
		// (get) Token: 0x06015208 RID: 86536 RVA: 0x00652BB8 File Offset: 0x00650DB8
		public float MaxFishPopulation
		{
			get
			{
				return this.PrimaryBiome.maxFishPopulation * this.FishPopulationFactor;
			}
		}

		// Token: 0x06015209 RID: 86537 RVA: 0x00652BCC File Offset: 0x00650DCC
		public Tile()
		{
		}

		// Token: 0x0601520A RID: 86538 RVA: 0x00652BEA File Offset: 0x00650DEA
		public Tile(PlanetTile tile)
		{
			this.tile = tile;
		}

		// Token: 0x0601520B RID: 86539 RVA: 0x00652C10 File Offset: 0x00650E10
		public void AddMutator(TileMutatorDef mutator)
		{
			if (this.mutatorsNullable == null)
			{
				this.mutatorsNullable = new List<TileMutatorDef>();
			}
			for (int i = this.mutatorsNullable.Count - 1; i >= 0; i--)
			{
				TileMutatorDef tileMutatorDef = this.mutatorsNullable[i];
				foreach (string item in mutator.categories)
				{
					if (tileMutatorDef.categories.Contains(item))
					{
						if (mutator.priority >= tileMutatorDef.priority)
						{
							this.mutatorsNullable.Remove(tileMutatorDef);
						}
						else
						{
							string str = "Detected mutator conflict: ";
							TileMutatorDef tileMutatorDef2 = tileMutatorDef;
							Log.Error(str + ((tileMutatorDef2 != null) ? tileMutatorDef2.ToString() : null) + " and " + ((mutator != null) ? mutator.ToString() : null));
						}
					}
				}
				foreach (string item2 in mutator.overrideCategories)
				{
					if (tileMutatorDef.categories.Contains(item2))
					{
						this.mutatorsNullable.Remove(tileMutatorDef);
					}
				}
			}
			this.mutatorsNullable.Add(mutator);
			this.mutatorsNullable.SortBy((TileMutatorDef m) => m.genOrder);
			TileMutatorWorker worker = mutator.Worker;
			if (worker == null)
			{
				return;
			}
			worker.OnAddedToTile(this.tile);
		}

		// Token: 0x0601520C RID: 86540 RVA: 0x00652D98 File Offset: 0x00650F98
		public void RemoveMutator(TileMutatorDef mutator)
		{
			List<TileMutatorDef> list = this.mutatorsNullable;
			if (list == null)
			{
				return;
			}
			list.Remove(mutator);
		}

		// Token: 0x0601520D RID: 86541 RVA: 0x00652DAC File Offset: 0x00650FAC
		public override string ToString()
		{
			return string.Format("({0} elev={1}m hill={2} temp={3}°C rain={4}mm", new object[]
			{
				this.biome,
				this.elevation,
				this.hilliness,
				this.temperature,
				this.rainfall
			}) + string.Format(" swampiness={0} (allowed={1}) (allowed={2}))", this.swampiness.ToStringPercent(), this.biome.allowRoads, this.biome.allowRivers);
		}

		// Token: 0x0601520E RID: 86542 RVA: 0x00652E44 File Offset: 0x00651044
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "DEV: Generate settlement";
			command_Action.action = delegate()
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
					mapParent.Tile = Find.WorldSelector.SelectedTile;
					mapParent.SetFaction(Faction.OfPlayer);
					Find.WorldObjects.Add(mapParent);
					GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, Find.World.info.initialMapSize, null, null, false);
				}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, delegate()
				{
					MapParent mapParent = Find.WorldObjects.MapParentAt(Find.WorldSelector.SelectedTile);
					if (mapParent != null)
					{
						Current.Game.CurrentMap = mapParent.Map;
						CameraJumper.TryJump(mapParent.Map.Center, mapParent.Map, CameraJumper.MovementMode.Pan);
					}
				});
			};
			yield return command_Action;
			yield return new Command_Action
			{
				defaultLabel = "DEV: Generate map",
				action = delegate()
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (WorldObjectDef worldObjectDef in DefDatabase<WorldObjectDef>.AllDefsListForReading)
					{
						WorldObjectDef local = worldObjectDef;
						Action <>9__5;
						list.Add(new FloatMenuOption(worldObjectDef.label, delegate()
						{
							Action action;
							if ((action = <>9__5) == null)
							{
								action = (<>9__5 = delegate()
								{
									MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(local);
									mapParent.Tile = this.tile;
									mapParent.SetFaction(Faction.OfPlayer);
									Find.WorldObjects.Add(mapParent);
									Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.tile, new IntVec3(100, 1, 100), null, null, false);
									Current.Game.CurrentMap = orGenerateMap;
									CameraJumper.TryJump(orGenerateMap.Center, orGenerateMap, CameraJumper.MovementMode.Pan);
								});
							}
							LongEventHandler.QueueLongEvent(action, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, delegate()
							{
								MapParent mapParent = Find.WorldObjects.MapParentAt(Find.WorldSelector.SelectedTile);
								if (mapParent != null)
								{
									Current.Game.CurrentMap = mapParent.Map;
									CameraJumper.TryJump(mapParent.Map.Center, mapParent.Map, CameraJumper.MovementMode.Pan);
								}
							});
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list, "Select type", false));
				}
			};
			yield break;
		}

		// Token: 0x0601520F RID: 86543 RVA: 0x00652E54 File Offset: 0x00651054
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<BiomeDef>(ref this.biome, "biome");
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.elevation, "elevation", 100f, false);
			Scribe_Values.Look<Hilliness>(ref this.hilliness, "hilliness", Hilliness.Undefined, false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 20f, false);
			Scribe_Values.Look<float>(ref this.rainfall, "rainfall", 0f, false);
			Scribe_Values.Look<float>(ref this.swampiness, "swampiness", 0f, false);
			Scribe_Values.Look<float>(ref this.pollution, "pollution", 0f, false);
			Scribe_Deep.Look<WorldFeature>(ref this.feature, "feature", Array.Empty<object>());
			Scribe_Collections.Look<TileMutatorDef>(ref this.mutatorsNullable, "mutatorDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x0400EBA9 RID: 60329
		private BiomeDef biome;

		// Token: 0x0400EBAA RID: 60330
		public float elevation = 100f;

		// Token: 0x0400EBAB RID: 60331
		public Hilliness hilliness;

		// Token: 0x0400EBAC RID: 60332
		public float temperature = 20f;

		// Token: 0x0400EBAD RID: 60333
		public float rainfall;

		// Token: 0x0400EBAE RID: 60334
		public float swampiness;

		// Token: 0x0400EBAF RID: 60335
		public WorldFeature feature;

		// Token: 0x0400EBB0 RID: 60336
		public float pollution;

		// Token: 0x0400EBB1 RID: 60337
		public PlanetTile tile;

		// Token: 0x0400EBB2 RID: 60338
		public List<TileMutatorDef> mutatorsNullable;

		// Token: 0x0400EBB3 RID: 60339
		[Unsaved(false)]
		private bool? tmpHasSecondaryBiome;

		// Token: 0x0400EBB4 RID: 60340
		[Unsaved(false)]
		private BiomeDef tmpSecondaryBiome;

		// Token: 0x0400EBB5 RID: 60341
		private Hilliness? hillinessLabelCached;

		// Token: 0x0400EBB6 RID: 60342
		private float? cachedMaxTemp;

		// Token: 0x0400EBB7 RID: 60343
		private float? cachedMinTemp;
	}
}
