using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200356F RID: 13679
	public static class GenLocalDate
	{
		// Token: 0x170031C0 RID: 12736
		// (get) Token: 0x06013310 RID: 78608 RVA: 0x005B446F File Offset: 0x005B266F
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x06013311 RID: 78609 RVA: 0x005B4476 File Offset: 0x005B2676
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x06013312 RID: 78610 RVA: 0x005B4483 File Offset: 0x005B2683
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x06013313 RID: 78611 RVA: 0x005B4490 File Offset: 0x005B2690
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x06013314 RID: 78612 RVA: 0x005B449D File Offset: 0x005B269D
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x06013315 RID: 78613 RVA: 0x005B44AA File Offset: 0x005B26AA
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x06013316 RID: 78614 RVA: 0x005B44B7 File Offset: 0x005B26B7
		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		// Token: 0x06013317 RID: 78615 RVA: 0x005B44D2 File Offset: 0x005B26D2
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x06013318 RID: 78616 RVA: 0x005B44DF File Offset: 0x005B26DF
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x06013319 RID: 78617 RVA: 0x005B44EC File Offset: 0x005B26EC
		public static int DayTick(Map map)
		{
			return GenLocalDate.DayTick(map.Tile);
		}

		// Token: 0x0601331A RID: 78618 RVA: 0x005B44F9 File Offset: 0x005B26F9
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x0601331B RID: 78619 RVA: 0x005B4506 File Offset: 0x005B2706
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x0601331C RID: 78620 RVA: 0x005B4513 File Offset: 0x005B2713
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x0601331D RID: 78621 RVA: 0x005B4520 File Offset: 0x005B2720
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x0601331E RID: 78622 RVA: 0x005B452D File Offset: 0x005B272D
		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		// Token: 0x0601331F RID: 78623 RVA: 0x005B454A File Offset: 0x005B274A
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013320 RID: 78624 RVA: 0x005B455D File Offset: 0x005B275D
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013321 RID: 78625 RVA: 0x005B4570 File Offset: 0x005B2770
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013322 RID: 78626 RVA: 0x005B4583 File Offset: 0x005B2783
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x06013323 RID: 78627 RVA: 0x005B4596 File Offset: 0x005B2796
		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013324 RID: 78628 RVA: 0x005B45B7 File Offset: 0x005B27B7
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013325 RID: 78629 RVA: 0x005B45CA File Offset: 0x005B27CA
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013326 RID: 78630 RVA: 0x005B45DD File Offset: 0x005B27DD
		public static int DayTick(Thing thing)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013327 RID: 78631 RVA: 0x005B45F0 File Offset: 0x005B27F0
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013328 RID: 78632 RVA: 0x005B4603 File Offset: 0x005B2803
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06013329 RID: 78633 RVA: 0x005B4616 File Offset: 0x005B2816
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0601332A RID: 78634 RVA: 0x005B4629 File Offset: 0x005B2829
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x0601332B RID: 78635 RVA: 0x005B463C File Offset: 0x005B283C
		public static int DayOfYear(PlanetTile tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return 0;
		}

		// Token: 0x0601332C RID: 78636 RVA: 0x005B4663 File Offset: 0x005B2863
		public static int HourOfDay(PlanetTile tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0601332D RID: 78637 RVA: 0x005B4680 File Offset: 0x005B2880
		public static int DayOfTwelfth(PlanetTile tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0601332E RID: 78638 RVA: 0x005B469D File Offset: 0x005B289D
		public static Twelfth Twelfth(PlanetTile tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x0601332F RID: 78639 RVA: 0x005B46BA File Offset: 0x005B28BA
		public static Season Season(PlanetTile tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x06013330 RID: 78640 RVA: 0x005B46D2 File Offset: 0x005B28D2
		public static int Year(PlanetTile tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013331 RID: 78641 RVA: 0x005B46FD File Offset: 0x005B28FD
		public static int DayOfSeason(PlanetTile tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013332 RID: 78642 RVA: 0x005B471A File Offset: 0x005B291A
		public static int DayOfQuadrum(PlanetTile tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013333 RID: 78643 RVA: 0x005B4737 File Offset: 0x005B2937
		public static int DayTick(PlanetTile tile)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013334 RID: 78644 RVA: 0x005B4754 File Offset: 0x005B2954
		public static float DayPercent(PlanetTile tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013335 RID: 78645 RVA: 0x005B4771 File Offset: 0x005B2971
		public static float YearPercent(PlanetTile tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013336 RID: 78646 RVA: 0x005B478E File Offset: 0x005B298E
		public static int HourInteger(PlanetTile tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013337 RID: 78647 RVA: 0x005B47AB File Offset: 0x005B29AB
		public static float HourFloat(PlanetTile tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06013338 RID: 78648 RVA: 0x005B47C8 File Offset: 0x005B29C8
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x06013339 RID: 78649 RVA: 0x005B47D8 File Offset: 0x005B29D8
		private static Vector2 LocationForDate(Thing thing)
		{
			PlanetTile tile = thing.Tile;
			if (tile.Valid)
			{
				return Find.WorldGrid.LongLatOf(tile);
			}
			return Vector2.zero;
		}
	}
}
