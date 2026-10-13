using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CF7 RID: 15607
	public class WorldGenStep_Mutators : WorldGenStep
	{
		// Token: 0x170034EE RID: 13550
		// (get) Token: 0x060153C6 RID: 86982 RVA: 0x0065A2F2 File Offset: 0x006584F2
		public override int SeedPart
		{
			get
			{
				return 897213645;
			}
		}

		// Token: 0x060153C7 RID: 86983 RVA: 0x0065A2FC File Offset: 0x006584FC
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			using (ProfilerBlock.Scope("Generate Mutators"))
			{
				WorldGenStep_Mutators.AddMutatorsFromTile(layer);
			}
		}

		// Token: 0x060153C8 RID: 86984 RVA: 0x0065A33C File Offset: 0x0065853C
		public static void AddMutatorsFromTile(PlanetLayer layer)
		{
			List<TileMutatorDef> list = (from m in DefDatabase<TileMutatorDef>.AllDefsListForReading
			where m.chanceOnNonLandmarkTile > 0f
			select m).ToList<TileMutatorDef>();
			foreach (Tile tile in layer.Tiles)
			{
				PlanetTile tile2 = tile.tile;
				if (tile.hilliness == Hilliness.Mountainous || tile.hilliness == Hilliness.Impassable)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Mountain);
				}
				if (ModsConfig.OdysseyActive && Find.World.LakeDirectionAt(tile2).IsValid)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Lakeshore);
				}
				else if (Find.World.CoastDirectionAt(tile2).IsValid)
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Coast);
				}
				SurfaceTile surfaceTile = tile as SurfaceTile;
				if (surfaceTile != null && !surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>())
				{
					if (!ModsConfig.OdysseyActive)
					{
						WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.River);
					}
					else if ((!surfaceTile.IsCoastal || surfaceTile.Rivers.Count <= 2 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverDelta)) && (surfaceTile.Rivers.Count != 1 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Headwater)) && (surfaceTile.Rivers.Count <= 2 || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverConfluence)) && (!Rand.Chance(0.05f) || !WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.RiverIsland)))
					{
						WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.River);
					}
				}
				if ((tile.hilliness == Hilliness.Mountainous && Rand.Chance(0.5f)) || (tile.hilliness == Hilliness.LargeHills && Rand.Chance(0.25f)))
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.Caves);
				}
				if (tile.Landmark == null)
				{
					foreach (TileMutatorDef tileMutatorDef in list)
					{
						if (Rand.Chance(tileMutatorDef.chanceOnNonLandmarkTile))
						{
							WorldGenStep_Mutators.TryAddMutator(tile, layer, tileMutatorDef);
						}
					}
				}
				if (ModsConfig.OdysseyActive && Rand.Chance(0.2f))
				{
					WorldGenStep_Mutators.TryAddMutator(tile, layer, TileMutatorDefOf.MixedBiome);
				}
			}
		}

		// Token: 0x060153C9 RID: 86985 RVA: 0x0065A5BC File Offset: 0x006587BC
		private static bool TryAddMutator(Tile tile, PlanetLayer layer, TileMutatorDef mutator)
		{
			if (tile.Mutators.Contains(mutator))
			{
				return false;
			}
			if (!mutator.IsValidTile(tile.tile, layer))
			{
				return false;
			}
			try
			{
				tile.AddMutator(mutator);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Error adding mutator ";
				array[1] = ((mutator != null) ? mutator.ToString() : null);
				array[2] = " to tile ";
				array[3] = ((tile != null) ? tile.ToString() : null);
				array[4] = ": ";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
				return false;
			}
			return true;
		}

		// Token: 0x0400ECC2 RID: 60610
		private const float MixedBiomeChance = 0.2f;

		// Token: 0x0400ECC3 RID: 60611
		private const float CaveChanceMountainous = 0.5f;

		// Token: 0x0400ECC4 RID: 60612
		private const float CaveChanceLargeHills = 0.25f;

		// Token: 0x0400ECC5 RID: 60613
		private const float RiverIslandChance = 0.05f;
	}
}
