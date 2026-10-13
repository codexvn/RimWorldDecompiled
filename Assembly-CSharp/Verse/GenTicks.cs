using System;
using RimWorld;
using RimWorld.Planet;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D12 RID: 3346
	public static class GenTicks
	{
		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06005585 RID: 21893 RVA: 0x001B9538 File Offset: 0x001B7738
		public static int TicksAbs
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06005586 RID: 21894 RVA: 0x001B9585 File Offset: 0x001B7785
		public static int TicksGame
		{
			get
			{
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksGame;
				}
				return 0;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x001B95A4 File Offset: 0x001B77A4
		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out result))
				{
					return result;
				}
				Vector2 vector;
				if (gameInitData.startingTile.Valid)
				{
					vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				}
				else
				{
					vector = Vector2.zero;
				}
				Twelfth twelfth;
				if (gameInitData.startingSeason != Season.Undefined)
				{
					twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				}
				else if (gameInitData.startingTile.Valid)
				{
					twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
				}
				else
				{
					twelfth = Season.Summer.GetFirstTwelfth(0f);
				}
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				int num2 = 300000 * (int)twelfth + 2500 * (6 + num);
				ticksAbsCache.Cache(num2, gameInitData);
				return num2;
			}
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x001B966E File Offset: 0x001B786E
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x001B9678 File Offset: 0x001B7878
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x0600558A RID: 21898 RVA: 0x001B9688 File Offset: 0x001B7888
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x001B96C8 File Offset: 0x001B78C8
		public static string ToStringSecondsFromTicks(this int numTicks, string format)
		{
			return numTicks.TicksToSeconds().ToString(format) + " " + "SecondsLower".Translate();
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x001B9704 File Offset: 0x001B7904
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x001B971F File Offset: 0x001B791F
		[BurstCompile]
		public static int GetTickIntervalOffset(int index, int count, int period)
		{
			return Mathf.CeilToInt((float)period / (float)count * (float)index % (float)period);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x001B9731 File Offset: 0x001B7931
		[BurstCompile]
		public static bool IsTickIntervalDelta(int offset, int period, int delta)
		{
			return Mathf.Abs(GenTicks.TicksGame + offset) % period < delta;
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x001B9744 File Offset: 0x001B7944
		[BurstCompile]
		public static bool IsTickIntervalDelta(int period, int delta)
		{
			return GenTicks.TicksGame % period < delta;
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x001B9750 File Offset: 0x001B7950
		[BurstCompile]
		public static bool IsTickInterval(int offset, int period)
		{
			return (GenTicks.TicksGame + offset) % period == 0;
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x001B975E File Offset: 0x001B795E
		[BurstCompile]
		public static bool IsTickInterval(int period)
		{
			return GenTicks.TicksGame % period == 0;
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001B976C File Offset: 0x001B796C
		public static int GetCameraUpdateRate(Thing thing)
		{
			if (!WorldRendererUtility.DrawingMap || Find.CurrentMap == null || thing.MapHeld != Find.CurrentMap)
			{
				return 15;
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			if (!cameraDriver.InViewOf(thing))
			{
				return 15;
			}
			return (int)(cameraDriver.CurrentZoom + 1);
		}

		// Token: 0x04003DDC RID: 15836
		public const int TicksPerRealSecond = 60;

		// Token: 0x04003DDD RID: 15837
		public const int TickRareInterval = 250;

		// Token: 0x04003DDE RID: 15838
		public const int TickLongInterval = 2000;

		// Token: 0x04003DDF RID: 15839
		public const int MaxTickInterval = 15;

		// Token: 0x04003DE0 RID: 15840
		public const float SecondsPerTick = 0.016666668f;
	}
}
