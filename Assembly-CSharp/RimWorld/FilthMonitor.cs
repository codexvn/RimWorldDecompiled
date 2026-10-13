using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003636 RID: 13878
	internal static class FilthMonitor
	{
		// Token: 0x0601344E RID: 78926 RVA: 0x005B78D4 File Offset: 0x005B5AD4
		public static void FilthMonitorTick()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			if (FilthMonitor.lastUpdate + 2500 <= Find.TickManager.TicksAbs)
			{
				int num = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer);
				int num2 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike);
				int num3 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike);
				Log.Message(string.Format("Filth data, per day:\n  {0} filth spawned per pawn\n  {1} filth human-generated per human\n  {2} filth animal-generated per animal\n  {3} filth accumulated per pawn\n  {4} filth dropped per pawn", new object[]
				{
					(float)FilthMonitor.filthSpawned / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthHumanGenerated / (float)num2 / 2500f * 60000f,
					(float)FilthMonitor.filthAnimalGenerated / (float)num3 / 2500f * 60000f,
					(float)FilthMonitor.filthAccumulated / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthDropped / (float)num / 2500f * 60000f
				}));
				FilthMonitor.filthSpawned = 0;
				FilthMonitor.filthAnimalGenerated = 0;
				FilthMonitor.filthHumanGenerated = 0;
				FilthMonitor.filthAccumulated = 0;
				FilthMonitor.filthDropped = 0;
				FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
			}
		}

		// Token: 0x0601344F RID: 78927 RVA: 0x005B7A54 File Offset: 0x005B5C54
		public static void Notify_FilthAccumulated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAccumulated++;
		}

		// Token: 0x06013450 RID: 78928 RVA: 0x005B7A6A File Offset: 0x005B5C6A
		public static void Notify_FilthDropped()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthDropped++;
		}

		// Token: 0x06013451 RID: 78929 RVA: 0x005B7A80 File Offset: 0x005B5C80
		public static void Notify_FilthAnimalGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAnimalGenerated++;
		}

		// Token: 0x06013452 RID: 78930 RVA: 0x005B7A96 File Offset: 0x005B5C96
		public static void Notify_FilthHumanGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthHumanGenerated++;
		}

		// Token: 0x06013453 RID: 78931 RVA: 0x005B7AAC File Offset: 0x005B5CAC
		public static void Notify_FilthSpawned()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthSpawned++;
		}

		// Token: 0x0400D5DD RID: 54749
		private static int lastUpdate;

		// Token: 0x0400D5DE RID: 54750
		private static int filthAccumulated;

		// Token: 0x0400D5DF RID: 54751
		private static int filthDropped;

		// Token: 0x0400D5E0 RID: 54752
		private static int filthAnimalGenerated;

		// Token: 0x0400D5E1 RID: 54753
		private static int filthHumanGenerated;

		// Token: 0x0400D5E2 RID: 54754
		private static int filthSpawned;

		// Token: 0x0400D5E3 RID: 54755
		private const int SampleDuration = 2500;
	}
}
