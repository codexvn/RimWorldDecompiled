using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C1B RID: 15387
	public class WorldReachability
	{
		// Token: 0x06014DEC RID: 85484 RVA: 0x006401E4 File Offset: 0x0063E3E4
		public WorldReachability()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.OnPlanetLayerAdded(layer);
			}
			this.nextFieldID = 1;
			this.InvalidateAllFields();
			Find.WorldGrid.OnPlanetLayerAdded += this.OnPlanetLayerAdded;
			Find.WorldGrid.OnPlanetLayerRemoved += this.OnPlanetLayerRemoved;
		}

		// Token: 0x06014DED RID: 85485 RVA: 0x00640290 File Offset: 0x0063E490
		private void OnPlanetLayerAdded(PlanetLayer layer)
		{
			this.layerFields[layer] = new int[layer.TilesCount];
		}

		// Token: 0x06014DEE RID: 85486 RVA: 0x006402A9 File Offset: 0x0063E4A9
		private void OnPlanetLayerRemoved(PlanetLayer layer)
		{
			if (this.layerFields.ContainsKey(layer))
			{
				this.layerFields.Remove(layer);
			}
		}

		// Token: 0x06014DEF RID: 85487 RVA: 0x006402C6 File Offset: 0x0063E4C6
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x06014DF0 RID: 85488 RVA: 0x006402CE File Offset: 0x0063E4CE
		public bool CanReach(Caravan c, PlanetTile tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x06014DF1 RID: 85489 RVA: 0x006402E0 File Offset: 0x0063E4E0
		public bool CanReach(PlanetTile startTile, PlanetTile destTile)
		{
			int[] array = this.layerFields[startTile.Layer];
			if (startTile.Layer != destTile.Layer)
			{
				return false;
			}
			if (!startTile.Valid || startTile.tileId >= array.Length || !destTile.Valid || destTile.tileId >= array.Length)
			{
				return false;
			}
			if (array[startTile.tileId] == this.impassableFieldID || array[destTile.tileId] == this.impassableFieldID)
			{
				return false;
			}
			if (this.IsValidField(array[startTile.tileId]) || this.IsValidField(array[destTile.tileId]))
			{
				return array[startTile.tileId] == array[destTile.tileId];
			}
			this.FloodFillAt(startTile);
			return array[startTile.tileId] != this.impassableFieldID && array[startTile.tileId] == array[destTile.tileId];
		}

		// Token: 0x06014DF2 RID: 85490 RVA: 0x006403BC File Offset: 0x0063E5BC
		public int GetLocalFieldId(PlanetTile tile)
		{
			return this.layerFields[tile.Layer][tile.tileId] - this.minValidFieldID;
		}

		// Token: 0x06014DF3 RID: 85491 RVA: 0x006403E0 File Offset: 0x0063E5E0
		private void InvalidateAllFields()
		{
			if (this.nextFieldID == 2147483646)
			{
				this.nextFieldID = 1;
			}
			this.minValidFieldID = this.nextFieldID;
			this.impassableFieldID = this.nextFieldID;
			this.nextFieldID++;
			foreach (KeyValuePair<PlanetLayer, int[]> keyValuePair in this.layerFields)
			{
				PlanetLayer planetLayer;
				int[] array;
				keyValuePair.Deconstruct(out planetLayer, out array);
				PlanetLayer layer = planetLayer;
				int[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] < this.minValidFieldID)
					{
						this.FloodFillAt(new PlanetTile(i, layer));
					}
				}
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
			{
				PlanetLayer planetLayer;
				int num;
				keyValuePair2.Deconstruct(out num, out planetLayer);
				planetLayer.FastTileFinder.DirtyCache();
			}
		}

		// Token: 0x06014DF4 RID: 85492 RVA: 0x006404FC File Offset: 0x0063E6FC
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x06014DF5 RID: 85493 RVA: 0x0064050C File Offset: 0x0063E70C
		private void FloodFillAt(PlanetTile tile)
		{
			World world = Find.World;
			int[] fields = this.layerFields[tile.Layer];
			if (world.Impassable(tile))
			{
				fields[tile.tileId] = this.impassableFieldID;
				return;
			}
			tile.Layer.Filler.FloodFill(tile, (PlanetTile x) => !world.Impassable(x), delegate(PlanetTile x)
			{
				fields[x.tileId] = this.nextFieldID;
			}, int.MaxValue, null);
			this.nextFieldID++;
		}

		// Token: 0x0400E89E RID: 59550
		private readonly Dictionary<PlanetLayer, int[]> layerFields = new Dictionary<PlanetLayer, int[]>();

		// Token: 0x0400E89F RID: 59551
		private int nextFieldID;

		// Token: 0x0400E8A0 RID: 59552
		private int impassableFieldID;

		// Token: 0x0400E8A1 RID: 59553
		private int minValidFieldID;
	}
}
