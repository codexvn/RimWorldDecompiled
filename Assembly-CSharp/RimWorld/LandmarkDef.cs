using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200282A RID: 10282
	public class LandmarkDef : Def
	{
		// Token: 0x1700224A RID: 8778
		// (get) Token: 0x0600E0CB RID: 57547 RVA: 0x0041F877 File Offset: 0x0041DA77
		public Material Material
		{
			get
			{
				return MaterialPool.MatFrom(this.iconTexturePath, ShaderDatabase.WorldOverlayTransparentLit, 3510);
			}
		}

		// Token: 0x1700224B RID: 8779
		// (get) Token: 0x0600E0CC RID: 57548 RVA: 0x0041F88E File Offset: 0x0041DA8E
		public Texture2D Texture
		{
			get
			{
				return CachedTexture.Get(this.iconTexturePath);
			}
		}

		// Token: 0x0600E0CD RID: 57549 RVA: 0x0041F89C File Offset: 0x0041DA9C
		public bool EverValid()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			foreach (MutatorChance mutatorChance in this.mutatorChances)
			{
				if (mutatorChance.required && !mutatorChance.mutator.EverValid())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600E0CE RID: 57550 RVA: 0x0041F910 File Offset: 0x0041DB10
		public static void ClearCache()
		{
			LandmarkDef.cachedSettlementTiles.Clear();
		}

		// Token: 0x0600E0CF RID: 57551 RVA: 0x0041F91C File Offset: 0x0041DB1C
		public bool IsValidTile(PlanetTile tile, PlanetLayer layer, bool canUseCache = false)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			bool flag = false;
			Tile tile2 = layer[tile];
			if (tile2.PrimaryBiome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				return false;
			}
			if (Find.World.landmarks[tile] != null)
			{
				return false;
			}
			if (tile2.Mutators.Any((TileMutatorDef m) => m.preventsLandmarks))
			{
				return false;
			}
			foreach (MutatorChance mutatorChance in this.mutatorChances)
			{
				if (mutatorChance.required)
				{
					if (!mutatorChance.mutator.IsValidTile(tile, layer))
					{
						return false;
					}
					flag = true;
				}
				if (!flag && mutatorChance.chance >= 1f && mutatorChance.mutator.IsValidTile(tile, layer))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (this.coastRotateMode != LandmarkDef.CoastRotateMode.None && !LandmarkDef.IsValidRotatableTile(tile, layer, this.coastRotateMode))
			{
				return false;
			}
			if (canUseCache)
			{
				if (!LandmarkDef.cachedSettlementTiles.ContainsKey(tile))
				{
					LandmarkDef.cachedSettlementTiles[tile] = Find.World.worldObjects.AnySettlementAt(tile);
				}
				if (LandmarkDef.cachedSettlementTiles[tile])
				{
					return false;
				}
			}
			else if (Find.World.worldObjects.AnySettlementAt(tile))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600E0D0 RID: 57552 RVA: 0x0041FA88 File Offset: 0x0041DC88
		private static bool IsValidRotatableTile(PlanetTile tile, PlanetLayer layer, LandmarkDef.CoastRotateMode mode)
		{
			layer.GetTileNeighbors(tile, LandmarkDef.neighbours);
			bool result = false;
			for (int i = 0; i < LandmarkDef.neighbours.Count; i++)
			{
				if (layer[LandmarkDef.neighbours[i]].Landmark != null)
				{
					return false;
				}
				if (LandmarkDef.IsValidRotatableNeighbour(layer[LandmarkDef.neighbours[i]].PrimaryBiome, mode))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600E0D1 RID: 57553 RVA: 0x0041FAFA File Offset: 0x0041DCFA
		public static bool IsValidRotatableNeighbour(BiomeDef biome, LandmarkDef.CoastRotateMode mode)
		{
			if (mode == LandmarkDef.CoastRotateMode.ToCoast)
			{
				return !biome.isBackgroundBiome;
			}
			return biome.isBackgroundBiome;
		}

		// Token: 0x0600E0D2 RID: 57554 RVA: 0x0041FB10 File Offset: 0x0041DD10
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.mutatorChances.Any((MutatorChance mc) => mc.chance >= 1f))
			{
				yield return "LandmarkDef " + this.defName + " has no mutators with a chance of 1 or higher.";
			}
			yield break;
		}

		// Token: 0x0400970E RID: 38670
		public Type workerClass = typeof(Landmark);

		// Token: 0x0400970F RID: 38671
		public float commonality;

		// Token: 0x04009710 RID: 38672
		public List<MutatorChance> mutatorChances = new List<MutatorChance>();

		// Token: 0x04009711 RID: 38673
		public List<MutatorChance> comboLandmarkMutators = new List<MutatorChance>();

		// Token: 0x04009712 RID: 38674
		public RulePackDef nameMaker;

		// Token: 0x04009713 RID: 38675
		[NoTranslate]
		public string iconTexturePath;

		// Token: 0x04009714 RID: 38676
		public IntVec2 atlasSize = IntVec2.One;

		// Token: 0x04009715 RID: 38677
		public float drawScale = 1f;

		// Token: 0x04009716 RID: 38678
		public LandmarkDef.LandmarkDrawType drawType;

		// Token: 0x04009717 RID: 38679
		public LandmarkDef.CoastRotateMode coastRotateMode;

		// Token: 0x04009718 RID: 38680
		public bool drawAboveRoads;

		// Token: 0x04009719 RID: 38681
		[NoTranslate]
		public string category;

		// Token: 0x0400971A RID: 38682
		public List<ShaderParameter> terrainParameters;

		// Token: 0x0400971B RID: 38683
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

		// Token: 0x0400971C RID: 38684
		private static Dictionary<PlanetTile, bool> cachedSettlementTiles = new Dictionary<PlanetTile, bool>();

		// Token: 0x0200282B RID: 10283
		public enum LandmarkDrawType
		{
			// Token: 0x0400971E RID: 38686
			Standard,
			// Token: 0x0400971F RID: 38687
			TerrainMask
		}

		// Token: 0x0200282C RID: 10284
		public enum CoastRotateMode
		{
			// Token: 0x04009721 RID: 38689
			None,
			// Token: 0x04009722 RID: 38690
			ToCoast,
			// Token: 0x04009723 RID: 38691
			AwayFromCoast
		}
	}
}
