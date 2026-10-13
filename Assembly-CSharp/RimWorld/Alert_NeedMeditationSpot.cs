using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200302A RID: 12330
	public class Alert_NeedMeditationSpot : Alert
	{
		// Token: 0x17002CF3 RID: 11507
		// (get) Token: 0x06011563 RID: 71011 RVA: 0x004F8DFC File Offset: 0x004F6FFC
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnNames.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					bool flag = false;
					for (int i = 0; i < pawn.timetable.times.Count; i++)
					{
						if (pawn.timetable.times[i] == TimeAssignmentDefOf.Meditate)
						{
							flag = true;
							break;
						}
					}
					if ((pawn.HasPsylink || flag) && pawn.psychicEntropy.IsPsychicallySensitive)
					{
						Building spot;
						if (Alert_NeedMeditationSpot.CachedSpots.TryGetValue(pawn, out spot))
						{
							if (MeditationUtility.IsValidMeditationBuildingForPawn(spot, pawn))
							{
								continue;
							}
							Alert_NeedMeditationSpot.CachedSpots.Remove(pawn);
						}
						LocalTargetInfo localTargetInfo = MeditationUtility.AllMeditationSpotCandidates(pawn, false).FirstOrFallback(LocalTargetInfo.Invalid);
						if (!localTargetInfo.IsValid)
						{
							this.targets.Add(pawn);
							this.pawnNames.Add(pawn.NameShortColored.Resolve());
						}
						else
						{
							Building building = localTargetInfo.Thing as Building;
							if (building != null)
							{
								Alert_NeedMeditationSpot.CachedSpots[pawn] = building;
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06011564 RID: 71012 RVA: 0x004F8F60 File Offset: 0x004F7160
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x06011565 RID: 71013 RVA: 0x004F8F9A File Offset: 0x004F719A
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06011566 RID: 71014 RVA: 0x004F8FBB File Offset: 0x004F71BB
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06011567 RID: 71015 RVA: 0x004F8FC8 File Offset: 0x004F71C8
		public static void ClearCache()
		{
			Alert_NeedMeditationSpot.CachedSpots.Clear();
		}

		// Token: 0x0400B391 RID: 45969
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B392 RID: 45970
		private List<string> pawnNames = new List<string>();

		// Token: 0x0400B393 RID: 45971
		private static Dictionary<Pawn, Building> CachedSpots = new Dictionary<Pawn, Building>();
	}
}
