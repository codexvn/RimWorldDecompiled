using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C15 RID: 15381
	public class WorldPathGrid
	{
		// Token: 0x170033A0 RID: 13216
		// (get) Token: 0x06014DCB RID: 85451 RVA: 0x0063F184 File Offset: 0x0063D384
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x06014DCC RID: 85452 RVA: 0x0063F198 File Offset: 0x0063D398
		public WorldPathGrid()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.OnPlanetLayerAdded(layer);
			}
			Find.WorldGrid.OnPlanetLayerAdded += this.OnPlanetLayerAdded;
			Find.WorldGrid.OnPlanetLayerRemoved += this.OnPlanetLayerRemoved;
		}

		// Token: 0x06014DCD RID: 85453 RVA: 0x0063F240 File Offset: 0x0063D440
		public void WorldPathGridTick()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (this.layerAllPathCostsRecalculatedDayOfYear[planetLayer2] != WorldPathGrid.DayOfYearAt0Long)
				{
					this.RecalculateLayerPerceivedPathCosts(planetLayer2);
				}
			}
		}

		// Token: 0x06014DCE RID: 85454 RVA: 0x0063F2B4 File Offset: 0x0063D4B4
		private void OnPlanetLayerAdded(PlanetLayer layer)
		{
			this.layerMovementDifficulty[layer] = new float[layer.TilesCount];
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = -1;
		}

		// Token: 0x06014DCF RID: 85455 RVA: 0x0063F2DA File Offset: 0x0063D4DA
		private void OnPlanetLayerRemoved(PlanetLayer layer)
		{
			this.layerMovementDifficulty.Remove(layer);
			this.layerAllPathCostsRecalculatedDayOfYear.Remove(layer);
		}

		// Token: 0x06014DD0 RID: 85456 RVA: 0x0063F2F6 File Offset: 0x0063D4F6
		public bool Passable(PlanetTile tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.layerMovementDifficulty[tile.Layer][tile.tileId] < 1000f;
		}

		// Token: 0x06014DD1 RID: 85457 RVA: 0x0063F327 File Offset: 0x0063D527
		public bool PassableFast(PlanetTile tile)
		{
			return this.layerMovementDifficulty[tile.Layer][tile.tileId] < 1000f;
		}

		// Token: 0x06014DD2 RID: 85458 RVA: 0x0063F349 File Offset: 0x0063D549
		public float PerceivedMovementDifficultyAt(PlanetTile tile)
		{
			return this.layerMovementDifficulty[tile.Layer][tile.tileId];
		}

		// Token: 0x06014DD3 RID: 85459 RVA: 0x0063F364 File Offset: 0x0063D564
		public void RecalculatePerceivedMovementDifficultyAt(PlanetTile tile, out bool needsRecache, int? ticksAbs = null)
		{
			needsRecache = false;
			if (!Find.WorldGrid.InBounds(tile))
			{
				return;
			}
			bool flag = this.PassableFast(tile);
			this.layerMovementDifficulty[tile.Layer][tile.tileId] = WorldPathGrid.CalculatedMovementDifficultyAt(tile, true, ticksAbs, null);
			if (flag != this.PassableFast(tile))
			{
				needsRecache = true;
			}
		}

		// Token: 0x06014DD4 RID: 85460 RVA: 0x0063F3B8 File Offset: 0x0063D5B8
		public void RecalculateAllLayersPathCosts()
		{
			foreach (KeyValuePair<PlanetLayer, float[]> keyValuePair in this.layerMovementDifficulty)
			{
				PlanetLayer planetLayer;
				float[] array;
				keyValuePair.Deconstruct(out planetLayer, out array);
				PlanetLayer layer = planetLayer;
				this.RecalculateLayerPerceivedPathCosts(layer);
			}
		}

		// Token: 0x06014DD5 RID: 85461 RVA: 0x0063F418 File Offset: 0x0063D618
		public void RecalculateLayerPerceivedPathCosts(PlanetLayer layer)
		{
			this.RecalculateLayerPerceivedPathCosts(layer, null);
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x06014DD6 RID: 85462 RVA: 0x0063F448 File Offset: 0x0063D648
		public void RecalculateLayerPerceivedPathCosts(PlanetLayer layer, int? ticksAbs)
		{
			bool flag = false;
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = -1;
			float[] array = this.layerMovementDifficulty[layer];
			for (int i = 0; i < array.Length; i++)
			{
				bool flag2;
				this.RecalculatePerceivedMovementDifficultyAt(new PlanetTile(i, layer), out flag2, ticksAbs);
				flag = (flag || flag2);
			}
			if (flag)
			{
				Find.WorldReachability.ClearCache();
			}
		}

		// Token: 0x06014DD7 RID: 85463 RVA: 0x0063F4A0 File Offset: 0x0063D6A0
		public static float CalculatedMovementDifficultyAt(PlanetTile tile, bool perceivedStatic, int? ticksAbs = null, StringBuilder explanation = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (explanation != null && explanation.Length > 0)
			{
				explanation.AppendLine();
			}
			if (tile2.PrimaryBiome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				if (explanation != null)
				{
					explanation.Append("Impassable".Translate());
				}
				return 1000f;
			}
			float num = 0f + tile2.PrimaryBiome.movementDifficulty;
			if (explanation != null)
			{
				explanation.Append(tile2.PrimaryBiome.LabelCap + ": " + tile2.PrimaryBiome.movementDifficulty.ToStringWithSign("0.#"));
			}
			float num2 = WorldPathGrid.HillinessMovementDifficultyOffset(tile2.hilliness);
			float num3 = num + num2;
			if (explanation != null && num2 != 0f)
			{
				explanation.AppendLine();
				explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
			}
			return num3 + WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, new int?(ticksAbs ?? GenTicks.TicksAbs), explanation);
		}

		// Token: 0x06014DD8 RID: 85464 RVA: 0x0063F5C0 File Offset: 0x0063D7C0
		public static float GetCurrentWinterMovementDifficultyOffset(PlanetTile tile, int? ticksAbs = null, StringBuilder explanation = null)
		{
			if (ticksAbs == null)
			{
				ticksAbs = new int?(GenTicks.TicksAbs);
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			SeasonUtility.GetSeason(GenDate.YearPercent((long)ticksAbs.Value, vector.x), vector.y, out num, out num2, out num3, out num4, out num5, out num6);
			float num7 = num4 + num6;
			num7 *= Mathf.InverseLerp(5f, 0f, GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs.Value, tile));
			if (num7 > 0.01f)
			{
				float num8 = 2f * num7;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("Winter".Translate());
					if (num7 < 0.999f)
					{
						explanation.Append(" (" + num7.ToStringPercent("F0") + ")");
					}
					explanation.Append(": ");
					explanation.Append(num8.ToStringWithSign("0.#"));
				}
				return num8;
			}
			return 0f;
		}

		// Token: 0x06014DD9 RID: 85465 RVA: 0x0063F6C8 File Offset: 0x0063D8C8
		public static bool WillWinterEverAffectMovementDifficulty(PlanetTile tile)
		{
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 3600000; i += 60000)
			{
				if (GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs + i, tile) < 5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014DDA RID: 85466 RVA: 0x0063F704 File Offset: 0x0063D904
		private static float HillinessMovementDifficultyOffset(Hilliness hilliness)
		{
			switch (hilliness)
			{
			case Hilliness.Flat:
				return 0f;
			case Hilliness.SmallHills:
				return 0.5f;
			case Hilliness.LargeHills:
				return 1.5f;
			case Hilliness.Mountainous:
				return 3f;
			case Hilliness.Impassable:
				return 1000f;
			default:
				return 0f;
			}
		}

		// Token: 0x0400E886 RID: 59526
		public readonly Dictionary<PlanetLayer, float[]> layerMovementDifficulty = new Dictionary<PlanetLayer, float[]>();

		// Token: 0x0400E887 RID: 59527
		private readonly Dictionary<PlanetLayer, int> layerAllPathCostsRecalculatedDayOfYear = new Dictionary<PlanetLayer, int>();

		// Token: 0x0400E888 RID: 59528
		private const float ImpassableMovementDifficulty = 1000f;

		// Token: 0x0400E889 RID: 59529
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x0400E88A RID: 59530
		public const float MaxTempForWinterOffset = 5f;
	}
}
