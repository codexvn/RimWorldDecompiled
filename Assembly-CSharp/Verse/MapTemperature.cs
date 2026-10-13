using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000537 RID: 1335
	public class MapTemperature : ICellBoolGiver
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x000B438C File Offset: 0x000B258C
		public float OutdoorTemp
		{
			get
			{
				if (this.map.Biome.constantOutdoorTemperature != null)
				{
					return this.map.Biome.constantOutdoorTemperature.Value;
				}
				if (this.map.IsPocketMap)
				{
					return this.map.generatorDef.pocketMapProperties.temperature;
				}
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000B4404 File Offset: 0x000B2604
		public float SeasonalTemp
		{
			get
			{
				if (this.map.Biome.constantOutdoorTemperature != null)
				{
					return this.map.Biome.constantOutdoorTemperature.Value;
				}
				if (this.map.IsPocketMap)
				{
					return this.map.generatorDef.pocketMapProperties.temperature;
				}
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x000B447C File Offset: 0x000B267C
		public CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3600, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x000440FD File Offset: 0x000422FD
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000B44CC File Offset: 0x000B26CC
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000B44DC File Offset: 0x000B26DC
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				IReadOnlyList<Room> allRooms = this.map.regionGrid.AllRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					allRooms[i].TempTracker.EqualizeTemperature();
				}
			}
			if (Find.TickManager.TicksGame % 60 == 0)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000B454D File Offset: 0x000B274D
		public void TemperatureUpdate()
		{
			if (Find.PlaySettings.showTemperatureOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000B4580 File Offset: 0x000B2780
		public bool SeasonAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			float seasonalTemp = this.SeasonalTemp;
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - buffer && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + buffer;
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000B45B8 File Offset: 0x000B27B8
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			float outdoorTemp = this.OutdoorTemp;
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - buffer && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + buffer;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000B45F0 File Offset: 0x000B27F0
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace, float buffer = 0f)
		{
			return this.SeasonAcceptableFor(animalRace, buffer) && this.OutdoorTemperatureAcceptableFor(animalRace, buffer);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000B4608 File Offset: 0x000B2808
		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)i);
				if (num > 0f)
				{
					flag2 = true;
				}
				if (num < 0f)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000B4658 File Offset: 0x000B2858
		public bool GetCellBool(int index)
		{
			IntVec3 intVec = this.map.cellIndices.IndexToCell(index);
			return !intVec.Fogged(this.map) && (!ModsConfig.OdysseyActive || intVec.GetTerrain(this.map) != TerrainDefOf.Space) && intVec.GetRoom(this.map) != null;
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000B46B4 File Offset: 0x000B28B4
		public Color GetCellExtraColor(int index)
		{
			float temperature = this.map.cellIndices.IndexToCell(index).GetTemperature(this.map);
			return this.GetColorForTemperature(temperature);
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000B46E8 File Offset: 0x000B28E8
		private Color GetColorForTemperature(float temperature)
		{
			List<ValueTuple<float, Color>> temperatureColorMap = MapTemperature.TemperatureColorMap;
			if (temperature <= temperatureColorMap[0].Item1)
			{
				return temperatureColorMap[0].Item2;
			}
			if (temperature >= temperatureColorMap[temperatureColorMap.Count - 1].Item1)
			{
				return temperatureColorMap[temperatureColorMap.Count - 1].Item2;
			}
			int i = 1;
			while (i < temperatureColorMap.Count)
			{
				if (temperatureColorMap[i].Item1 > temperature)
				{
					if (i >= temperatureColorMap.Count - 1)
					{
						return temperatureColorMap[i].Item2;
					}
					int index = i - 1;
					int index2 = i;
					float item = temperatureColorMap[index].Item1;
					float item2 = temperatureColorMap[index2].Item1;
					float t = (temperature - item) / (item2 - item);
					return Color.Lerp(temperatureColorMap[index].Item2, temperatureColorMap[index2].Item2, t);
				}
				else
				{
					i++;
				}
			}
			Log.Error("Error when trying to determine correct color for temperature grid.");
			return Color.white;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000B47D9 File Offset: 0x000B29D9
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.def.AffectsRegions)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000B47F4 File Offset: 0x000B29F4
		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float latitude = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).y : 0f;
			stringBuilder.AppendLine("Latitude " + latitude.ToString());
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)j;
				float num2 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					twelfth.GetQuadrum().ToString(),
					"/",
					SeasonUtility.GetReportedSeason(twelfth.GetMiddleYearPct(), latitude).ToString(),
					" - ",
					twelfth.ToString(),
					" ",
					num2.ToString("F2")
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each day this year------");
			stringBuilder.AppendLine("Tile avg: " + this.map.TileInfo.temperature.ToString() + "°C");
			stringBuilder.AppendLine("Seasonal shift: " + GenTemperature.SeasonalShiftAmplitudeAt(this.map.Tile).ToString());
			stringBuilder.AppendLine("Equatorial distance: " + Find.WorldGrid.DistanceFromEquatorNormalized(this.map.Tile).ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Day  Lo   Hi   OffsetFromSeason RandomDailyVariation");
			for (int k = 0; k < 60; k++)
			{
				int absTick2 = (int)((float)(k * 60000) + 15000f);
				int absTick3 = (int)((float)(k * 60000) + 45000f);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000B4BBC File Offset: 0x000B2DBC
		public void DebugLogTemperatureOverlayColors()
		{
			int num = -50;
			int num2 = 150;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("-----Temperature overlay colors by temperature------");
			for (int i = num; i <= num2; i++)
			{
				stringBuilder.AppendLine(i.ToString() + ": " + this.GetColorForTemperature((float)i).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001D02 RID: 7426
		private static readonly List<ValueTuple<float, Color>> TemperatureColorMap = new List<ValueTuple<float, Color>>
		{
			new ValueTuple<float, Color>(-25f, ColorLibrary.DarkBlue),
			new ValueTuple<float, Color>(0f, ColorLibrary.Blue),
			new ValueTuple<float, Color>(25f, ColorLibrary.Green),
			new ValueTuple<float, Color>(50f, ColorLibrary.Yellow),
			new ValueTuple<float, Color>(100f, ColorLibrary.Red)
		};

		// Token: 0x04001D03 RID: 7427
		private const int TemperatureOverlayUpdateInterval = 60;

		// Token: 0x04001D04 RID: 7428
		private Map map;

		// Token: 0x04001D05 RID: 7429
		private CellBoolDrawer drawerInt;
	}
}
