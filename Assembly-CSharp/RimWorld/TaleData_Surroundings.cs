using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002917 RID: 10519
	public class TaleData_Surroundings : TaleData
	{
		// Token: 0x170022F4 RID: 8948
		// (get) Token: 0x0600E61C RID: 58908 RVA: 0x00431F5D File Offset: 0x0043015D
		public bool Outdoors
		{
			get
			{
				return this.weather != null;
			}
		}

		// Token: 0x0600E61D RID: 58909 RVA: 0x00431F68 File Offset: 0x00430168
		public override void ExposeData()
		{
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.snowDepth, "snowDepth", 0f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
			Scribe_Defs.Look<RoomRoleDef>(ref this.roomRole, "roomRole");
			Scribe_Defs.Look<BiomeDef>(ref this.biome, "biome");
			Scribe_Values.Look<float>(ref this.roomImpressiveness, "roomImpressiveness", 0f, false);
			Scribe_Values.Look<float>(ref this.roomBeauty, "roomBeauty", 0f, false);
			Scribe_Values.Look<float>(ref this.roomCleanliness, "roomCleanliness", 0f, false);
		}

		// Token: 0x0600E61E RID: 58910 RVA: 0x0043202D File Offset: 0x0043022D
		public override IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			if (this.biome == null)
			{
				yield return new Rule_String("BIOME", Find.WorldGrid[this.tile].PrimaryBiome.label);
			}
			else
			{
				yield return new Rule_String("BIOME", this.biome.label);
			}
			if (this.roomRole != null && this.roomRole != RoomRoleDefOf.None)
			{
				yield return new Rule_String("ROOM_role", this.roomRole.label);
				yield return new Rule_String("ROOM_roleDefinite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.roomRole.label, false, false));
				yield return new Rule_String("ROOM_roleIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.roomRole.label, false, false));
				RoomStatScoreStage impressiveness = RoomStatDefOf.Impressiveness.GetScoreStage(this.roomImpressiveness);
				RoomStatScoreStage beauty = RoomStatDefOf.Beauty.GetScoreStage(this.roomBeauty);
				RoomStatScoreStage cleanliness = RoomStatDefOf.Cleanliness.GetScoreStage(this.roomCleanliness);
				yield return new Rule_String("ROOM_impressiveness", impressiveness.label);
				yield return new Rule_String("ROOM_impressivenessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(impressiveness.label, false, false));
				yield return new Rule_String("ROOM_beauty", beauty.label);
				yield return new Rule_String("ROOM_beautyIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(beauty.label, false, false));
				yield return new Rule_String("ROOM_cleanliness", cleanliness.label);
				yield return new Rule_String("ROOM_cleanlinessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(cleanliness.label, false, false));
				impressiveness = null;
				beauty = null;
				cleanliness = null;
			}
			yield break;
		}

		// Token: 0x0600E61F RID: 58911 RVA: 0x00432040 File Offset: 0x00430240
		public static TaleData_Surroundings GenerateFrom(IntVec3 c, Map map)
		{
			TaleData_Surroundings taleData_Surroundings = new TaleData_Surroundings
			{
				tile = map.Tile,
				biome = map.Biome
			};
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				if (roomOrAdjacent.PsychologicallyOutdoors)
				{
					taleData_Surroundings.weather = map.weatherManager.CurWeatherPerceived;
				}
				taleData_Surroundings.roomRole = roomOrAdjacent.Role;
				taleData_Surroundings.roomImpressiveness = roomOrAdjacent.GetStat(RoomStatDefOf.Impressiveness);
				taleData_Surroundings.roomBeauty = roomOrAdjacent.GetStat(RoomStatDefOf.Beauty);
				taleData_Surroundings.roomCleanliness = roomOrAdjacent.GetStat(RoomStatDefOf.Cleanliness);
			}
			if (!GenTemperature.TryGetTemperatureForCell(c, map, out taleData_Surroundings.temperature))
			{
				taleData_Surroundings.temperature = 21f;
			}
			taleData_Surroundings.snowDepth = map.snowGrid.GetDepth(c);
			return taleData_Surroundings;
		}

		// Token: 0x0600E620 RID: 58912 RVA: 0x004320FD File Offset: 0x004302FD
		public static TaleData_Surroundings GenerateRandom(Map map)
		{
			return TaleData_Surroundings.GenerateFrom(CellFinder.RandomCell(map), map);
		}

		// Token: 0x040099E7 RID: 39399
		public PlanetTile tile;

		// Token: 0x040099E8 RID: 39400
		public BiomeDef biome;

		// Token: 0x040099E9 RID: 39401
		public float temperature;

		// Token: 0x040099EA RID: 39402
		public float snowDepth;

		// Token: 0x040099EB RID: 39403
		public WeatherDef weather;

		// Token: 0x040099EC RID: 39404
		public RoomRoleDef roomRole;

		// Token: 0x040099ED RID: 39405
		public float roomImpressiveness;

		// Token: 0x040099EE RID: 39406
		public float roomBeauty;

		// Token: 0x040099EF RID: 39407
		public float roomCleanliness;
	}
}
