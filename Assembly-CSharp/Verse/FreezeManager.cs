using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	public class FreezeManager : IExposable
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x00034EAB File Offset: 0x000330AB
		public FreezeManager()
		{
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00034EBE File Offset: 0x000330BE
		public FreezeManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00034ED8 File Offset: 0x000330D8
		public void ExposeData()
		{
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00034EEB File Offset: 0x000330EB
		public void DoCellSteadyEffects(IntVec3 c)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.DoWaterFreezing(c);
			this.DoIceMelting(c);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00034F04 File Offset: 0x00033104
		private void DoIceMelting(IntVec3 c)
		{
			float temperature = c.GetTemperature(this.map);
			Room room = c.GetRoom(this.map);
			if (temperature < 2f)
			{
				return;
			}
			if (!Rand.Chance(0.001f * ((temperature + 5f) / 5f)))
			{
				return;
			}
			this.tmpCellsToAffect.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(c, 2.9f, true))
			{
				if (intVec.InBounds(this.map) && intVec.GetRoom(this.map) == room && this.CanEverMelt(intVec) && this.CanMelt(intVec))
				{
					this.tmpCellsToAffect.Add(intVec);
				}
			}
			foreach (IntVec3 c2 in this.tmpCellsToAffect)
			{
				this.map.terrainGrid.RemoveTempTerrain(c2, false, false);
			}
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00035028 File Offset: 0x00033228
		private void DoWaterFreezing(IntVec3 c)
		{
			float temperature = c.GetTemperature(this.map);
			Room room = c.GetRoom(this.map);
			if (temperature > -7f)
			{
				return;
			}
			if (!this.CanFreeze(c))
			{
				return;
			}
			float num = 0.003f * (temperature / -5f);
			WaterBody waterBody = c.GetWaterBody(this.map);
			num += (1f - num) * Mathf.Pow((float)waterBody.numCellsFrozen / (float)waterBody.cells.Count, 10f);
			if (this.SurroundedByIce(c))
			{
				num = 1f;
			}
			if (!Rand.Chance(num))
			{
				return;
			}
			this.tmpCellsToAffect.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(c, 2.9f, true))
			{
				if (intVec.InBounds(this.map) && intVec.GetRoom(this.map) == room && this.CanFreeze(intVec))
				{
					this.tmpCellsToAffect.Add(intVec);
				}
			}
			foreach (IntVec3 c2 in this.tmpCellsToAffect)
			{
				this.map.terrainGrid.SetTempTerrain(c2, TerrainDefOf.ThinIce);
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00035198 File Offset: 0x00033398
		public static bool CanEverFreeze(IntVec3 c, Map map)
		{
			return c.GetTerrain(map).canFreeze;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000351AB File Offset: 0x000333AB
		private bool CanFreeze(IntVec3 c)
		{
			return FreezeManager.CanEverFreeze(c, this.map) && !this.SurroundedByWater(c);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x000351C7 File Offset: 0x000333C7
		private bool CanEverMelt(IntVec3 c)
		{
			return c.GetTerrain(this.map) == TerrainDefOf.ThinIce;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x000351DC File Offset: 0x000333DC
		private bool CanMelt(IntVec3 c)
		{
			return this.CanEverMelt(c) && !this.SurroundedByIce(c);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x000351F4 File Offset: 0x000333F4
		private bool SurroundedByIce(IntVec3 c)
		{
			bool result = true;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(this.map) && c2.GetTerrain(this.map) != TerrainDefOf.ThinIce)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00035250 File Offset: 0x00033450
		private bool SurroundedByWater(IntVec3 c)
		{
			bool result = true;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(this.map) && !c2.GetTerrain(this.map).IsWater)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0400064F RID: 1615
		public const float WaterFreezeThreshold = -7f;

		// Token: 0x04000650 RID: 1616
		private const float IceMeltThreshold = 2f;

		// Token: 0x04000651 RID: 1617
		private const float BaseWaterFreezeChance = 0.003f;

		// Token: 0x04000652 RID: 1618
		private const float BaseIceMeltChance = 0.001f;

		// Token: 0x04000653 RID: 1619
		private const float TemperatureFactor = 5f;

		// Token: 0x04000654 RID: 1620
		private const float FreezeRadius = 2.9f;

		// Token: 0x04000655 RID: 1621
		private const int LakeFrozenPctExp = 10;

		// Token: 0x04000656 RID: 1622
		private Map map;

		// Token: 0x04000657 RID: 1623
		[Unsaved(false)]
		private List<IntVec3> tmpCellsToAffect = new List<IntVec3>();
	}
}
