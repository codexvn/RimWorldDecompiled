using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9E RID: 8094
	public abstract class GenStep_BaseRuins : GenStep
	{
		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600B2BB RID: 45755 RVA: 0x003331E9 File Offset: 0x003313E9
		protected virtual int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600B2BC RID: 45756 RVA: 0x003331ED File Offset: 0x003313ED
		protected virtual FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.15f, 0.3f);
			}
		}

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x0600B2BD RID: 45757 RVA: 0x003331FE File Offset: 0x003313FE
		protected virtual FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.1f, 0.35f);
			}
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x0600B2BE RID: 45758 RVA: 0x00075162 File Offset: 0x00073362
		protected virtual int MoveRangeLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x0600B2BF RID: 45759 RVA: 0x00075162 File Offset: 0x00073362
		protected virtual int ContractLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x0600B2C0 RID: 45760 RVA: 0x002A9950 File Offset: 0x002A7B50
		protected virtual int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x0600B2C1 RID: 45761 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x0600B2C2 RID: 45762 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool AvoidWaterRoads
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x0600B2C3 RID: 45763 RVA: 0x0033320F File Offset: 0x0033140F
		protected virtual TerrainAffordanceDef MinAffordance
		{
			get
			{
				return TerrainAffordanceDefOf.Heavy;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x0600B2C4 RID: 45764
		protected abstract LayoutDef LayoutDef { get; }

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x0600B2C5 RID: 45765
		protected abstract Faction Faction { get; }

		// Token: 0x0600B2C6 RID: 45766 RVA: 0x00333216 File Offset: 0x00331416
		protected virtual CellRect GetBounds(Map map)
		{
			return map.BoundsRect(16);
		}

		// Token: 0x0600B2C7 RID: 45767 RVA: 0x00333220 File Offset: 0x00331420
		public override void Generate(Map map, GenStepParams parms)
		{
			this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
		}

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x00333230 File Offset: 0x00331430
		public virtual void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			this.structureSketches.Clear();
			CellRect bounds = this.GetBounds(map);
			List<CellRect> list = this.GetRects(bounds, map).ToList<CellRect>();
			if (!list.Any<CellRect>())
			{
				Log.Warning(string.Format("{0} did not find any valid rects to generate ruins in for bounds {1}.", base.GetType().Name, bounds));
				return;
			}
			float num = (float)bounds.Area * mapFillPercentRange.RandomInRange;
			foreach (CellRect rect in this.GetRectOrder(list, map))
			{
				if (num <= 0f)
				{
					break;
				}
				this.GenerateAndSpawn(rect, map, parms, this.LayoutDef);
				num -= (float)rect.Area;
			}
		}

		// Token: 0x0600B2C9 RID: 45769 RVA: 0x003332FC File Offset: 0x003314FC
		protected virtual IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			List<CellRect> list = MapGenUtility.SubdivideRectIntoChunks(area, this.RegionSize, 1);
			MapGenUtility.TryFixInvalidRects(list, map, this.MinRegionSize, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			this.MergeRandomRects(list, map);
			this.OffsetContractRandomRects(list);
			MapGenUtility.RemoveInvalidRects(list, map, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!this.IsValidRect(list[i], map))
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}

		// Token: 0x0600B2CA RID: 45770 RVA: 0x00333389 File Offset: 0x00331589
		protected virtual IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return rects.InRandomOrder(null);
		}

		// Token: 0x0600B2CB RID: 45771 RVA: 0x00333394 File Offset: 0x00331594
		protected virtual bool IsValidRect(CellRect rect, Map map)
		{
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any(new Predicate<CellRect>(rect.Overlaps)))
			{
				return false;
			}
			using (IEnumerator<IntVec3> enumerator = rect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!MapGenUtility.IsCellValid(enumerator.Current, map, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B2CC RID: 45772 RVA: 0x00333424 File Offset: 0x00331624
		private void OffsetContractRandomRects(List<CellRect> rects)
		{
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect value = rects[i];
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = value.Width - this.MinRegionSize;
				int num6 = value.Height - this.MinRegionSize;
				if (num5 > this.MinRegionSize)
				{
					num5 -= (num = Rand.Range(0, Mathf.Min(num5, this.MoveRangeLimit)));
					num3 = Rand.Range(0, Mathf.Min(num5, this.ContractLimit));
				}
				if (num6 > this.MinRegionSize)
				{
					num6 -= (num2 = Rand.Range(0, Mathf.Min(num6, this.MoveRangeLimit)));
					num4 = Rand.Range(0, Mathf.Min(num6, this.ContractLimit));
				}
				value = value.MovedBy(num, num2).ContractedBy(num + num3, num2 + num4);
				rects[i] = value;
			}
		}

		// Token: 0x0600B2CD RID: 45773 RVA: 0x00333510 File Offset: 0x00331710
		private void MergeRandomRects(List<CellRect> rects, Map map)
		{
			int num = Mathf.RoundToInt(this.MergeRange.RandomInRange * (float)rects.Count);
			List<CellRect> merged = new List<CellRect>();
			int num2 = 0;
			Func<CellRect, bool> <>9__0;
			while (num2 < num && rects.Count > 2)
			{
				Func<CellRect, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((CellRect x) => !merged.Contains(x)));
				}
				List<CellRect> list = rects.Where(predicate).ToList<CellRect>();
				if (!list.Any<CellRect>())
				{
					break;
				}
				foreach (CellRect cellRect in list.InRandomOrder(null))
				{
					bool flag = false;
					for (int i = 0; i < rects.Count; i++)
					{
						CellRect cellRect2 = rects[i];
						if (!(cellRect2 == cellRect) && !merged.Contains(cellRect2) && cellRect2.InlineWith(cellRect))
						{
							CellRect cellRect3 = cellRect.Encapsulate(cellRect2);
							bool flag2 = true;
							if (this.IsValidRect(cellRect3, map))
							{
								foreach (CellRect lhs in rects)
								{
									if (!(lhs == cellRect) && !(lhs == cellRect2) && lhs.Overlaps(cellRect3))
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									rects.Remove(cellRect2);
									merged.Add(cellRect3);
									rects[rects.IndexOf(cellRect)] = cellRect3;
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				num2++;
			}
		}

		// Token: 0x0600B2CE RID: 45774 RVA: 0x00333704 File Offset: 0x00331904
		protected virtual StructureGenParams GetStructureGenParams(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			return new StructureGenParams
			{
				size = rect.Size
			};
		}

		// Token: 0x0600B2CF RID: 45775 RVA: 0x00333718 File Offset: 0x00331918
		protected virtual LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			StructureGenParams structureGenParams = this.GetStructureGenParams(rect, map, parms, layoutDef);
			LayoutWorker worker = layoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(structureGenParams);
			LayoutStructureSketch result;
			using (new RandBlock(layoutStructureSketch.id))
			{
				map.layoutStructureSketches.Add(layoutStructureSketch);
				this.structureSketches.Add(layoutStructureSketch);
				float? threatPoints = null;
				if (parms.sitePart != null)
				{
					threatPoints = new float?(parms.sitePart.parms.points);
				}
				if (threatPoints == null)
				{
					Site site = map.Parent as Site;
					if (site != null)
					{
						threatPoints = new float?(site.ActualThreatPoints);
					}
				}
				worker.Spawn(layoutStructureSketch, map, rect.Min, threatPoints, null, true, false, this.Faction);
				if (this.UseUsedRects)
				{
					MapGenerator.UsedRects.Add(rect.ExpandedBy(1));
				}
				result = layoutStructureSketch;
			}
			return result;
		}

		// Token: 0x04007A71 RID: 31345
		public readonly List<LayoutStructureSketch> structureSketches = new List<LayoutStructureSketch>();
	}
}
