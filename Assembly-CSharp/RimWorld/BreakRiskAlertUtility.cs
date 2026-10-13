using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003063 RID: 12387
	public static class BreakRiskAlertUtility
	{
		// Token: 0x17002D01 RID: 11521
		// (get) Token: 0x06011640 RID: 71232 RVA: 0x004FD15C File Offset: 0x004FB35C
		public static List<Pawn> PawnsAtRiskExtreme
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskExtremeResult;
			}
		}

		// Token: 0x17002D02 RID: 11522
		// (get) Token: 0x06011641 RID: 71233 RVA: 0x004FD1DC File Offset: 0x004FB3DC
		public static List<Pawn> PawnsAtRiskMajor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMajorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMajorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMajorResult;
			}
		}

		// Token: 0x17002D03 RID: 11523
		// (get) Token: 0x06011642 RID: 71234 RVA: 0x004FD25C File Offset: 0x004FB45C
		public static List<Pawn> PawnsAtRiskMinor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMinorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMinorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMinorResult;
			}
		}

		// Token: 0x17002D04 RID: 11524
		// (get) Token: 0x06011643 RID: 71235 RVA: 0x004FD2DC File Offset: 0x004FB4DC
		public static string AlertLabel
		{
			get
			{
				int num = BreakRiskAlertUtility.PawnsAtRiskExtreme.Count<Pawn>();
				string text;
				if (num > 0)
				{
					text = "BreakRiskExtreme".Translate();
				}
				else
				{
					num = BreakRiskAlertUtility.PawnsAtRiskMajor.Count<Pawn>();
					if (num > 0)
					{
						text = "BreakRiskMajor".Translate();
					}
					else
					{
						num = BreakRiskAlertUtility.PawnsAtRiskMinor.Count<Pawn>();
						text = "BreakRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x17002D05 RID: 11525
		// (get) Token: 0x06011644 RID: 71236 RVA: 0x004FD360 File Offset: 0x004FB560
		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn pawn in BreakRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("  - " + pawn.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskExtremeDesc".Translate(stringBuilder2).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn pawn2 in BreakRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("  - " + pawn2.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMajorDesc".Translate(stringBuilder3).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMinor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn pawn3 in BreakRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("  - " + pawn3.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMinorDesc".Translate(stringBuilder4).Resolve());
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("BreakRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06011645 RID: 71237 RVA: 0x004FD56C File Offset: 0x004FB76C
		public static void Clear()
		{
			BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
		}

		// Token: 0x0400B3F9 RID: 46073
		private static List<Pawn> pawnsAtRiskExtremeResult = new List<Pawn>();

		// Token: 0x0400B3FA RID: 46074
		private static List<Pawn> pawnsAtRiskMajorResult = new List<Pawn>();

		// Token: 0x0400B3FB RID: 46075
		private static List<Pawn> pawnsAtRiskMinorResult = new List<Pawn>();
	}
}
