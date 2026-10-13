using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000517 RID: 1303
	public sealed class RoomTempTracker
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x000B0A4F File Offset: 0x000AEC4F
		private Map Map
		{
			get
			{
				return this.room.Map;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x000B0A5C File Offset: 0x000AEC5C
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000B0A71 File Offset: 0x000AEC71
		public List<IntVec3> EqualizeCellsForReading
		{
			get
			{
				return this.equalizeCells;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x000B0A79 File Offset: 0x000AEC79
		// (set) Token: 0x06002201 RID: 8705 RVA: 0x000B0A81 File Offset: 0x000AEC81
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 1000f);
			}
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000B0A99 File Offset: 0x000AEC99
		public RoomTempTracker(Room room, Map map)
		{
			this.room = room;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000B0AC4 File Offset: 0x000AECC4
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000B0ACC File Offset: 0x000AECCC
		public void RoomChanged()
		{
			if (this.Map != null && !GravshipPlacementUtility.placingGravship)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000B0AF4 File Offset: 0x000AECF4
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.room.DistrictCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (this.room.UsesOutdoorTemperature)
			{
				return;
			}
			this.CalculateRoofCovereage(map);
			this.RegenerateEqualizeCells(map);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000B0B54 File Offset: 0x000AED54
		private void CalculateRoofCovereage(Map map)
		{
			int num = 0;
			foreach (IntVec3 c in this.room.Cells)
			{
				RoofDef roof = c.GetRoof(map);
				if (roof == null)
				{
					this.noRoofCoverage += 1f;
				}
				else if (roof.isThickRoof)
				{
					this.thickRoofCoverage += 1f;
				}
				num++;
			}
			this.thickRoofCoverage /= (float)num;
			this.noRoofCoverage /= (float)num;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000B0BFC File Offset: 0x000AEDFC
		private void RegenerateEqualizeCells(Map map)
		{
			foreach (IntVec3 a in this.room.Cells)
			{
				int i = 0;
				while (i < 4)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
					if (!intVec.InBounds(map))
					{
						goto IL_100;
					}
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region == null)
					{
						goto IL_100;
					}
					if (region.type == RegionType.Portal)
					{
						bool flag = false;
						for (int j = 0; j < region.links.Count; j++)
						{
							Region regionA = region.links[j].RegionA;
							Region regionB = region.links[j].RegionB;
							if (regionA.Room != this.room && !regionA.IsDoorway)
							{
								flag = true;
								break;
							}
							if (regionB.Room != this.room && !regionB.IsDoorway)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_100;
						}
					}
					IL_164:
					i++;
					continue;
					IL_100:
					if (!intVec2.InBounds(map) || intVec2.GetRoom(map) == this.room)
					{
						goto IL_164;
					}
					bool flag2 = false;
					for (int k = 0; k < 4; k++)
					{
						if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoom(map) == this.room)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						this.equalizeCells.Add(intVec2);
						goto IL_164;
					}
					goto IL_164;
				}
			}
			this.equalizeCells.Shuffle<IntVec3>();
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000B0DB8 File Offset: 0x000AEFB8
		public void EqualizeTemperature()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.room.IsDoorway)
			{
				bool flag = true;
				IntVec3 anyCell = this.room.Districts[0].Regions[0].AnyCell;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = anyCell + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(this.Map))
					{
						Room room = intVec.GetRoom(this.Map);
						if (room != null && !room.IsDoorway)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.room.Temperature += this.WallEqualizationTempChangePerInterval();
				}
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000B0EC4 File Offset: 0x000AF0C4
		private float WallEqualizationTempChangePerInterval()
		{
			if (this.equalizeCells.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
			for (int i = 0; i < num2; i++)
			{
				this.cycleIndex++;
				int index = this.cycleIndex % this.equalizeCells.Count;
				float num3;
				if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
				{
					if (this.Map.Biome.inVacuum)
					{
						float num4 = num3 - this.Temperature;
						num += num4 * 5E-05f;
					}
					else
					{
						num += num3 - this.Temperature;
					}
				}
				else
				{
					float outdoorTemp = this.Map.mapTemperature.OutdoorTemp;
					if (this.Map.Biome.inVacuum)
					{
						float num5 = outdoorTemp - this.Temperature;
						num += num5 * 5E-05f;
					}
					else
					{
						num += Mathf.Lerp(this.Temperature, outdoorTemp, 0.5f) - this.Temperature;
					}
				}
			}
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.room.CellCount;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000B100C File Offset: 0x000AF20C
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000B1064 File Offset: 0x000AF264
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = this.TempDiffFromOutdoorsAdjusted();
			float num2 = this.Map.Biome.inVacuum ? 5E-05f : 5E-05f;
			return num * this.ThinRoofCoverage * num2 * 120f;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000B10B8 File Offset: 0x000AF2B8
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000B10E8 File Offset: 0x000AF2E8
		private float DeepEqualizationTempChangePerInterval()
		{
			if (this.thickRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = 15f - this.temperatureInt;
			if (num > 0f)
			{
				return 0f;
			}
			float num2 = (this.Map.generatorDef != null && this.Map.generatorDef.isUnderground) ? 0.002f : 5E-05f;
			return num * this.thickRoofCoverage * num2 * 120f;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000B1160 File Offset: 0x000AF360
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000B11B8 File Offset: 0x000AF3B8
		internal string DebugString()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomTempTracker.debugGetFrame + 120)
			{
				RoomTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomTempTracker.debugWallEq /= 40f;
				RoomTempTracker.debugGetFrame = Time.frameCount;
			}
			return string.Concat(new string[]
			{
				"  thick roof coverage: ",
				this.thickRoofCoverage.ToStringPercent("F0"),
				"\n  thin roof coverage: ",
				this.ThinRoofCoverage.ToStringPercent("F0"),
				"\n  no roof coverage: ",
				this.noRoofCoverage.ToStringPercent("F0"),
				"\n\n  wall equalization: ",
				RoomTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, 200f).ToString(),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, -200f).ToString(),
				"\n  equalize interval ticks: ",
				120.ToString(),
				"\n  equalize cells count:",
				this.equalizeCells.Count.ToString()
			});
		}

		// Token: 0x04001CA9 RID: 7337
		private Room room;

		// Token: 0x04001CAA RID: 7338
		private float temperatureInt;

		// Token: 0x04001CAB RID: 7339
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04001CAC RID: 7340
		private float noRoofCoverage;

		// Token: 0x04001CAD RID: 7341
		private float thickRoofCoverage;

		// Token: 0x04001CAE RID: 7342
		public const float FractionWallEqualizeCells = 0.2f;

		// Token: 0x04001CAF RID: 7343
		public const float WallEqualizeFactor = 0.00017f;

		// Token: 0x04001CB0 RID: 7344
		public const float EqualizationPowerOfFilledCells = 0.5f;

		// Token: 0x04001CB1 RID: 7345
		private int cycleIndex;

		// Token: 0x04001CB2 RID: 7346
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x04001CB3 RID: 7347
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04001CB4 RID: 7348
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04001CB5 RID: 7349
		private const float UndergroundEqualizeFractionPerTick = 0.002f;

		// Token: 0x04001CB6 RID: 7350
		private const float VacuumEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04001CB7 RID: 7351
		private static int debugGetFrame = -999;

		// Token: 0x04001CB8 RID: 7352
		private static float debugWallEq;
	}
}
