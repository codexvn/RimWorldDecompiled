using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D59 RID: 3417
	public static class CoverUtility
	{
		// Token: 0x060056F9 RID: 22265 RVA: 0x001BF594 File Offset: 0x001BD794
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return list;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo item;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item) && item.BlockChance > 0f)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x001BF618 File Offset: 0x001BD818
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return num;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo;
				if (intVec.InBounds(map) && !(shooterLoc == intVec) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
				{
					num += (1f - num) * coverInfo.BlockChance;
				}
			}
			return num;
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001BF6A4 File Offset: 0x001BD8A4
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				return false;
			}
			float angleFlat = (shooterLoc - cell).AngleFlat;
			float num = GenGeo.AngleDifferenceBetween((adjCell - cell).AngleFlat, angleFlat);
			if (!cell.AdjacentToCardinal(adjCell))
			{
				num *= 1.75f;
			}
			float num2 = cover.BaseBlockChance();
			if (num < 15f)
			{
				num2 *= 1f;
			}
			else if (num < 27f)
			{
				num2 *= 0.8f;
			}
			else if (num < 40f)
			{
				num2 *= 0.6f;
			}
			else if (num < 52f)
			{
				num2 *= 0.4f;
			}
			else
			{
				if (num >= 65f)
				{
					result = CoverInfo.Invalid;
					return false;
				}
				num2 *= 0.2f;
			}
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.9f)
			{
				num2 *= 0.3333f;
			}
			else if (lengthHorizontal < 2.9f)
			{
				num2 *= 0.66666f;
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x001BF7E3 File Offset: 0x001BD9E3
		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x001BF7FC File Offset: 0x001BD9FC
		public static float BaseBlockChance(this Thing thing)
		{
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null && building_Door.Open)
			{
				return 0f;
			}
			return thing.def.BaseBlockChance();
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x001BF82C File Offset: 0x001BDA2C
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x001BF87C File Offset: 0x001BDA7C
		public static bool ThingCovered(Thing thing, Map map)
		{
			foreach (IntVec3 c in thing.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thing && thingList[i].def.Fillage == FillCategory.Full && thingList[i].def.Altitude >= thing.def.Altitude)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003F50 RID: 16208
		public const float CoverPercent_Corner = 0.75f;
	}
}
