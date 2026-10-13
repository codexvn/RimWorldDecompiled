using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CD6 RID: 15574
	public class WorldComponent_LocationGenerator : WorldComponent
	{
		// Token: 0x060152CD RID: 86733 RVA: 0x00656218 File Offset: 0x00654418
		public WorldComponent_LocationGenerator(World world) : base(world)
		{
			float planetCoverage = world.PlanetCoverage;
			int num;
			if (planetCoverage < 0.301f)
			{
				if (planetCoverage >= 0.051f)
				{
					num = 8;
				}
				else
				{
					num = 3;
				}
			}
			else if (planetCoverage >= 0.501f)
			{
				num = 20;
			}
			else
			{
				num = 12;
			}
			this.worldLocationsTarget = num;
			foreach (GeneratedLocationDef generatedLocationDef in DefDatabase<GeneratedLocationDef>.AllDefsListForReading)
			{
				foreach (PlanetLayerDef key in generatedLocationDef.LayerDefs)
				{
					List<GeneratedLocationDef> list;
					if (!WorldComponent_LocationGenerator.LayerList.TryGetValue(key, out list))
					{
						list = (WorldComponent_LocationGenerator.LayerList[key] = new List<GeneratedLocationDef>());
					}
					list.Add(generatedLocationDef);
				}
			}
		}

		// Token: 0x060152CE RID: 86734 RVA: 0x00656310 File Offset: 0x00654510
		public override void WorldComponentTick()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (GenTicks.IsTickInterval(planetLayer2.GetHashCode().HashOffset(), 90000))
				{
					int num2 = Mathf.RoundToInt(planetLayer2.Def.generatedLocationFactor * (float)this.worldLocationsTarget);
					if (planetLayer2.Tiles.Count((Tile tile) => this.world.worldObjects.AnyGeneratedWorldLocationAt(tile.tile)) < num2)
					{
						this.GenerateUntilTarget(planetLayer2);
					}
				}
			}
		}

		// Token: 0x060152CF RID: 86735 RVA: 0x006563BC File Offset: 0x006545BC
		private void GenerateUntilTarget()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.GenerateUntilTarget(layer);
			}
		}

		// Token: 0x060152D0 RID: 86736 RVA: 0x0065641C File Offset: 0x0065461C
		private void GenerateUntilTarget(PlanetLayer layer)
		{
			int num = Mathf.RoundToInt(layer.Def.generatedLocationFactor * (float)this.worldLocationsTarget);
			for (int i = 0; i < num; i++)
			{
				this.GenerateLocationForLayer(layer);
			}
		}

		// Token: 0x060152D1 RID: 86737 RVA: 0x00656458 File Offset: 0x00654658
		private void GenerateLocationForLayer(PlanetLayer layer)
		{
			if (!WorldComponent_LocationGenerator.LayerList.ContainsKey(layer.Def) || WorldComponent_LocationGenerator.LayerList[layer.Def].Empty<GeneratedLocationDef>())
			{
				return;
			}
			PlanetTile tile;
			if (!WorldComponent_LocationGenerator.TryFindSiteTile(layer, out tile))
			{
				return;
			}
			WorldComponent_LocationGenerator.tmpOptions.Clear();
			WorldComponent_LocationGenerator.tmpOptions.AddRange(WorldComponent_LocationGenerator.LayerList[layer.Def]);
			using (List<GeneratedLocationDef>.Enumerator enumerator = WorldComponent_LocationGenerator.LayerList[layer.Def].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GeneratedLocationDef location = enumerator.Current;
					if (location.layerMaximum >= 0 && layer.Tiles.Count((Tile t) => this.world.worldObjects.WorldObjectOfDefAt(location.worldObjectDef, t.tile) != null) >= location.layerMaximum)
					{
						WorldComponent_LocationGenerator.tmpOptions.Remove(location);
					}
				}
			}
			if (WorldComponent_LocationGenerator.tmpOptions.Empty<GeneratedLocationDef>())
			{
				return;
			}
			GeneratedLocationDef generatedLocationDef = WorldComponent_LocationGenerator.tmpOptions.RandomElementByWeight((GeneratedLocationDef def) => def.weight);
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(generatedLocationDef.worldObjectDef);
			INameableWorldObject nameableWorldObject = worldObject as INameableWorldObject;
			if (nameableWorldObject != null)
			{
				nameableWorldObject.Name = NameGenerator.GenerateName(worldObject.def.nameMaker, null, false, "r_name", null, null);
			}
			IResourceWorldObject resourceWorldObject = worldObject as IResourceWorldObject;
			if (resourceWorldObject != null && !generatedLocationDef.preciousResources.NullOrEmpty<ThingDef>())
			{
				resourceWorldObject.PreciousResource = generatedLocationDef.preciousResources.RandomElement<ThingDef>();
			}
			IExpirableWorldObject expirableWorldObject = worldObject as IExpirableWorldObject;
			if (expirableWorldObject != null && !generatedLocationDef.TimeoutRangeDays.IsZeros)
			{
				expirableWorldObject.ExpireAtTicks = GenTicks.TicksGame + (int)(generatedLocationDef.TimeoutRangeDays.RandomInRange * 60000f);
			}
			worldObject.isGeneratedLocation = true;
			worldObject.Tile = tile;
			this.world.worldObjects.Add(worldObject);
		}

		// Token: 0x060152D2 RID: 86738 RVA: 0x00656654 File Offset: 0x00654854
		public static bool TryFindSiteTile(PlanetLayer layer, out PlanetTile tile)
		{
			tile = PlanetTile.Invalid;
			PlanetTile other = new PlanetTile(0, Find.WorldGrid.Surface);
			PlanetTile planetTile;
			if (TileFinder.TryFindRandomPlayerTile(out planetTile, false, null, false, null))
			{
				other = planetTile;
			}
			return TileFinder.TryFindTileWithDistance(layer.GetClosestTile(other), 0, int.MaxValue, out tile, new Predicate<PlanetTile>(WorldComponent_LocationGenerator.<TryFindSiteTile>g__Validator|9_0), TileFinderMode.Random, false);
		}

		// Token: 0x060152D3 RID: 86739 RVA: 0x006566AE File Offset: 0x006548AE
		public override void FinalizeInit(bool fromLoad)
		{
			if (fromLoad)
			{
				return;
			}
			this.GenerateUntilTarget();
		}

		// Token: 0x060152D6 RID: 86742 RVA: 0x006566E8 File Offset: 0x006548E8
		[CompilerGenerated]
		internal static bool <TryFindSiteTile>g__Validator|9_0(PlanetTile x)
		{
			return !Find.WorldObjects.AnyWorldObjectAt(x);
		}

		// Token: 0x0400EC48 RID: 60488
		private const int UpdateInterval = 90000;

		// Token: 0x0400EC49 RID: 60489
		private readonly int worldLocationsTarget;

		// Token: 0x0400EC4A RID: 60490
		private static readonly Dictionary<PlanetLayerDef, List<GeneratedLocationDef>> LayerList = new Dictionary<PlanetLayerDef, List<GeneratedLocationDef>>();

		// Token: 0x0400EC4B RID: 60491
		private static readonly List<GeneratedLocationDef> tmpOptions = new List<GeneratedLocationDef>();
	}
}
