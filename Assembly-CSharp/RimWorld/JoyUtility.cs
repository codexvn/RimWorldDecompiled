using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001504 RID: 5380
	public static class JoyUtility
	{
		// Token: 0x06008021 RID: 32801 RVA: 0x0026935C File Offset: 0x0026755C
		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			if (map.weatherManager.RainRate >= 0.25f)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				return false;
			}
			GameConditionDef gameConditionDef;
			if (!map.gameConditionManager.AllowEnjoyableOutsideNow(map, out gameConditionDef))
			{
				if (outFailReason != null)
				{
					outFailReason.Append(gameConditionDef.label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x002693BC File Offset: 0x002675BC
		public static bool EnjoyableOutsideNow(Pawn pawn, StringBuilder outFailReason = null)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return true;
			}
			if (!JoyUtility.EnjoyableOutsideNow(mapHeld, outFailReason))
			{
				return false;
			}
			if (!pawn.ComfortableTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("NotEnjoyableOutsideTemperature".Translate());
				}
				return false;
			}
			if (mapHeld.Biome.inVacuum && pawn.ConcernedByVacuum)
			{
				if (outFailReason != null)
				{
					outFailReason.Append("Vacuum".Translate());
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x00269448 File Offset: 0x00267648
		public static bool JoyTickCheckEnd(Pawn pawn, int delta, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f, Building joySource = null)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.");
				return false;
			}
			if (joySource != null)
			{
				if (joySource.def.building.joyKind != null && pawn.CurJob.def.joyKind != joySource.def.building.joyKind)
				{
					Log.ErrorOnce("Joy source joyKind and jobDef.joyKind are not the same. building=" + joySource.ToStringSafe<Building>() + ", jobDef=" + pawn.CurJob.def.ToStringSafe<JobDef>(), joySource.thingIDNumber ^ 876598732);
				}
				extraJoyGainFactor *= joySource.GetStatValue(StatDefOf.JoyGainFactor, true, -1);
			}
			if (pawn.needs.joy == null && !curJob.doUntilGatheringEnded)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return false;
			}
			Need_Joy joy = pawn.needs.joy;
			if (joy != null)
			{
				joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f * (float)delta, curJob.def.joyKind);
			}
			if (curJob.def.joySkill != null)
			{
				pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick * (float)delta, false, false);
			}
			if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy && !curJob.doUntilGatheringEnded)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return true;
			}
			Need_Joy joy2 = pawn.needs.joy;
			if (joy2 != null && joy2.CurLevel > 0.9999f && !curJob.doUntilGatheringEnded)
			{
				if (fullJoyAction == JoyTickFullJoyAction.EndJob)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					return true;
				}
				if (fullJoyAction == JoyTickFullJoyAction.GoToNextToil)
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x00269620 File Offset: 0x00267820
		public static void TryGainRecRoomThought(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
				if (pawn.needs.mood != null && ThoughtDefOf.JoyActivityInImpressiveRecRoom.stages[scoreStageIndex] != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.JoyActivityInImpressiveRecRoom, scoreStageIndex), null);
				}
			}
		}

		// Token: 0x06008025 RID: 32805 RVA: 0x00269694 File Offset: 0x00267894
		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds;
		}

		// Token: 0x06008026 RID: 32806 RVA: 0x002696BB File Offset: 0x002678BB
		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			return !((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment).allowJoy;
		}

		// Token: 0x06008027 RID: 32807 RVA: 0x002696E4 File Offset: 0x002678E4
		public static int JoyKindsOnMapCount(Map map)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			int count = list.Count;
			list.Clear();
			return count;
		}

		// Token: 0x06008028 RID: 32808 RVA: 0x00269704 File Offset: 0x00267904
		public static List<JoyKindDef> JoyKindsOnMapTempList(Map map)
		{
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.tempKindList.Add(joyKindDef);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null && !JoyUtility.tempKindList.Contains(building.def.building.joyKind))
				{
					JoyUtility.tempKindList.Add(building.def.building.joyKind);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing.def.ingestible.joyKind) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing.def.ingestible.joyKind);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing2.def.ingestible.joyKind) && !thing2.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing2.def.ingestible.joyKind);
				}
			}
			JoyUtility.CheckForReadingRecreation(map);
			return JoyUtility.tempKindList;
		}

		// Token: 0x06008029 RID: 32809 RVA: 0x00269934 File Offset: 0x00267B34
		private static void CheckForReadingRecreation(Map map)
		{
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Book))
			{
				if (!JoyUtility.tempKindList.Contains(JoyKindDefOf.Reading) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					return;
				}
			}
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				if (((carryTracker != null) ? carryTracker.CarriedThing : null) is Book)
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					return;
				}
			}
			foreach (Building_Bookcase building_Bookcase in map.listerThings.GetThingsOfType<Building_Bookcase>())
			{
				if (building_Bookcase.HeldBooks.Any<Book>() && !JoyUtility.tempKindList.Contains(JoyKindDefOf.Reading) && !building_Bookcase.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(JoyKindDefOf.Reading);
					break;
				}
			}
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x00269AA0 File Offset: 0x00267CA0
		public static string JoyKindsOnMapString(Map map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, joyKindDef, map);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, building, building.def.building.joyKind, map);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing, thing.def.ingestible.joyKind, map);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing2, thing2.def.ingestible.joyKind, map);
				}
			}
			bool flag = false;
			using (List<Thing>.Enumerator enumerator2 = map.listerThings.ThingsInGroup(ThingRequestGroup.Book).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Thing thing3 = enumerator2.Current;
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
					flag = true;
				}
			}
			if (!flag)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					Pawn_CarryTracker carryTracker = pawn.carryTracker;
					if (((carryTracker != null) ? carryTracker.CarriedThing : null) is Book)
					{
						JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				using (IEnumerator<Building_Bookcase> enumerator4 = map.listerBuildings.AllColonistBuildingsOfType<Building_Bookcase>().GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (enumerator4.Current.HeldBooks.Any<Book>())
						{
							JoyUtility.CheckAppendJoyKind(stringBuilder, null, JoyKindDefOf.Reading, map);
							break;
						}
					}
				}
			}
			JoyUtility.listedJoyKinds.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x00269D88 File Offset: 0x00267F88
		private static void CheckAppendJoyKind(StringBuilder sb, Thing t, JoyKindDef kind, Map map)
		{
			if (JoyUtility.listedJoyKinds.Contains(kind))
			{
				return;
			}
			sb.Append("  - ");
			if (t == null)
			{
				sb.AppendLine(kind.LabelCap);
			}
			else
			{
				if (t.def.category == ThingCategory.Item && t.Position.Fogged(map))
				{
					return;
				}
				sb.AppendLine(kind.LabelCap + " (" + t.def.label + ")");
			}
			JoyUtility.listedJoyKinds.Add(kind);
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x00269E24 File Offset: 0x00268024
		public static string JoyKindsNotOnMapString(Map map)
		{
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			if (allDefsListForReading.Count == list.Count)
			{
				return "(" + "None".Translate() + ")";
			}
			string text = "";
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				if (!list.Contains(joyKindDef))
				{
					text += "  - " + joyKindDef.LabelCap + "\n";
				}
			}
			list.Clear();
			return text.TrimEndNewlines();
		}

		// Token: 0x040055AA RID: 21930
		private static readonly List<JoyKindDef> tempKindList = new List<JoyKindDef>();

		// Token: 0x040055AB RID: 21931
		private static readonly List<JoyKindDef> listedJoyKinds = new List<JoyKindDef>();
	}
}
