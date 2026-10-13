using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200397C RID: 14716
	public class SymbolResolver_Street : SymbolResolver
	{
		// Token: 0x06014389 RID: 82825 RVA: 0x006089C4 File Offset: 0x00606BC4
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.streetHorizontal ?? (rp.rect.Width >= rp.rect.Height);
			int width = flag ? rp.rect.Height : rp.rect.Width;
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			this.CalculateStreet(rp.rect, flag, floorDef);
			this.FillStreetGaps(flag, width);
			this.RemoveShortStreetParts(flag, width);
			this.SpawnFloor(rp.rect, flag, floorDef);
		}

		// Token: 0x0601438A RID: 82826 RVA: 0x00608A6C File Offset: 0x00606C6C
		private void CalculateStreet(CellRect rect, bool horizontal, TerrainDef floorDef)
		{
			SymbolResolver_Street.street.Clear();
			int num = horizontal ? rect.Width : rect.Height;
			for (int i = 0; i < num; i++)
			{
				if (horizontal)
				{
					SymbolResolver_Street.street.Add(this.CausesStreet(new IntVec3(rect.minX + i, 0, rect.minZ - 1), floorDef) && this.CausesStreet(new IntVec3(rect.minX + i, 0, rect.maxZ + 1), floorDef));
				}
				else
				{
					SymbolResolver_Street.street.Add(this.CausesStreet(new IntVec3(rect.minX - 1, 0, rect.minZ + i), floorDef) && this.CausesStreet(new IntVec3(rect.maxX + 1, 0, rect.minZ + i), floorDef));
				}
			}
		}

		// Token: 0x0601438B RID: 82827 RVA: 0x00608B40 File Offset: 0x00606D40
		private void FillStreetGaps(bool horizontal, int width)
		{
			int num = -1;
			for (int i = 0; i < SymbolResolver_Street.street.Count; i++)
			{
				if (SymbolResolver_Street.street[i])
				{
					num = i;
				}
				else if (num != -1 && i - num <= width)
				{
					int num2 = i + 1;
					while (num2 < i + width + 1 && num2 < SymbolResolver_Street.street.Count)
					{
						if (SymbolResolver_Street.street[num2])
						{
							SymbolResolver_Street.street[i] = true;
							break;
						}
						num2++;
					}
				}
			}
		}

		// Token: 0x0601438C RID: 82828 RVA: 0x00608BBC File Offset: 0x00606DBC
		private void RemoveShortStreetParts(bool horizontal, int width)
		{
			for (int i = 0; i < SymbolResolver_Street.street.Count; i++)
			{
				if (SymbolResolver_Street.street[i])
				{
					int num = 0;
					int num2 = i;
					while (num2 < SymbolResolver_Street.street.Count && SymbolResolver_Street.street[num2])
					{
						num++;
						num2++;
					}
					int num3 = 0;
					int num4 = i;
					while (num4 >= 0 && SymbolResolver_Street.street[num4])
					{
						num3++;
						num4--;
					}
					if (num3 + num - 1 < width)
					{
						SymbolResolver_Street.street[i] = false;
					}
				}
			}
		}

		// Token: 0x0601438D RID: 82829 RVA: 0x00608C4C File Offset: 0x00606E4C
		private void SpawnFloor(CellRect rect, bool horizontal, TerrainDef floorDef)
		{
			TerrainGrid terrainGrid = BaseGen.globalSettings.map.terrainGrid;
			foreach (IntVec3 intVec in rect)
			{
				if ((horizontal && SymbolResolver_Street.street[intVec.x - rect.minX]) || (!horizontal && SymbolResolver_Street.street[intVec.z - rect.minZ]))
				{
					terrainGrid.SetTerrain(intVec, floorDef);
				}
			}
		}

		// Token: 0x0601438E RID: 82830 RVA: 0x00608CE4 File Offset: 0x00606EE4
		private bool CausesStreet(IntVec3 c, TerrainDef floorDef)
		{
			Map map = BaseGen.globalSettings.map;
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return (edifice != null && edifice.def == ThingDefOf.Wall) || c.GetDoor(map) != null || c.GetTerrain(map) == floorDef;
		}

		// Token: 0x0400DF70 RID: 57200
		private static List<bool> street = new List<bool>();
	}
}
