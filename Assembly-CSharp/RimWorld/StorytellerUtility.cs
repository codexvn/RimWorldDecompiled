using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E7A RID: 7802
	public static class StorytellerUtility
	{
		// Token: 0x0600AD3C RID: 44348 RVA: 0x003168AB File Offset: 0x00314AAB
		public static float GlobalPointsMin()
		{
			return Rand.RangeSeeded(35f, Find.Storyteller.difficulty.MinThreatPointsCeiling, Find.TickManager.TicksGame / 2500);
		}

		// Token: 0x0600AD3D RID: 44349 RVA: 0x003168D8 File Offset: 0x00314AD8
		public static IncidentParms DefaultParmsNow(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			if (incCat == null)
			{
				Log.Warning("Trying to get default parms for null incident category.");
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = target;
			if (incCat.needsParmsPoints)
			{
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(target);
			}
			return incidentParms;
		}

		// Token: 0x0600AD3E RID: 44350 RVA: 0x00316914 File Offset: 0x00314B14
		public static float GetProgressScore(IIncidentTarget target)
		{
			int num = 0;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				if (!pawn.IsQuestLodger() && pawn.IsFreeColonist)
				{
					num++;
				}
			}
			return (float)num * 1f + target.PlayerWealthForStoryteller * 0.0001f;
		}

		// Token: 0x0600AD3F RID: 44351 RVA: 0x00316988 File Offset: 0x00314B88
		public static float DefaultThreatPointsNow(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null && map.IsPocketMap)
			{
				target = map.PocketMapParent.sourceMap;
			}
			float playerWealthForStoryteller = target.PlayerWealthForStoryteller;
			float num = StorytellerUtility.PointsPerWealthCurve.Evaluate(playerWealthForStoryteller);
			float num2 = 0f;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				if (!pawn.IsQuestLodger())
				{
					float num3 = 0f;
					if (pawn.IsFreeColonist)
					{
						num3 = StorytellerUtility.PointsPerColonistByWealthCurve.Evaluate(playerWealthForStoryteller);
					}
					else if (pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && !pawn.Downed && pawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
					{
						num3 = 0.08f * pawn.kindDef.combatPower;
						if (target is Caravan)
						{
							num3 *= 0.7f;
						}
					}
					else if (pawn.IsColonyMech && !pawn.Downed)
					{
						num3 = pawn.kindDef.combatPower * StorytellerUtility.PointsFactorForColonyMechsCurve.Evaluate(playerWealthForStoryteller);
					}
					else if (pawn.IsSubhuman)
					{
						num3 = pawn.kindDef.combatPower * StorytellerUtility.PointsFactorForColonySubhumanCurve.Evaluate(playerWealthForStoryteller);
					}
					if (num3 > 0f)
					{
						if (pawn.ParentHolder != null && pawn.ParentHolder is Building_CryptosleepCasket)
						{
							num3 *= 0.3f;
						}
						num3 = Mathf.Lerp(num3, num3 * pawn.health.summaryHealth.SummaryHealthPercent, 0.65f);
						if (pawn.IsSlaveOfColony)
						{
							num3 *= 0.75f;
						}
						if (ModsConfig.BiotechActive && pawn.RaceProps.Humanlike)
						{
							num3 *= StorytellerUtility.PointsFactorForPawnAgeYearsCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
						}
						num2 += num3;
					}
				}
			}
			float num4 = (num + num2) * target.IncidentPointsRandomFactorRange.RandomInRange;
			float totalThreatPointsFactor = Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor;
			float num5 = Mathf.Lerp(1f, totalThreatPointsFactor, Find.Storyteller.difficulty.adaptationEffectFactor);
			return Mathf.Clamp(num4 * num5 * Find.Storyteller.difficulty.threatScale * Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassedSinceSettle), StorytellerUtility.GlobalPointsMin(), 10000f);
		}

		// Token: 0x0600AD40 RID: 44352 RVA: 0x00316C18 File Offset: 0x00314E18
		public static float DefaultSiteThreatPointsNow()
		{
			return SiteTuning.ThreatPointsToSiteThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.World)) * SiteTuning.SitePointRandomFactorRange.RandomInRange;
		}

		// Token: 0x0600AD41 RID: 44353 RVA: 0x00316C48 File Offset: 0x00314E48
		public static float AllyIncidentFraction(bool fullAlliesOnly)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!allFactionsListForReading[i].Hidden && !allFactionsListForReading[i].IsPlayer && !allFactionsListForReading[i].temporary)
				{
					if (allFactionsListForReading[i].def.CanEverBeNonHostile)
					{
						num2++;
					}
					if (allFactionsListForReading[i].PlayerRelationKind == FactionRelationKind.Ally || (!fullAlliesOnly && !allFactionsListForReading[i].HostileTo(Faction.OfPlayer)))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				return -1f;
			}
			float x = (float)num / Mathf.Max((float)num2, 1f);
			return StorytellerUtility.AllyIncidentFractionFromAllyFraction.Evaluate(x);
		}

		// Token: 0x0600AD42 RID: 44354 RVA: 0x00316D10 File Offset: 0x00314F10
		public static void ShowFutureIncidentsDebugLogFloatMenu(bool currentMapOnly)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("-All comps-", delegate()
			{
				StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, null, null, 100);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp comp = storytellerComps[i];
				string text = comp.ToString();
				if (!text.NullOrEmpty())
				{
					list.Add(new FloatMenuOption(text, delegate()
					{
						StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, comp, null, 100);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600AD43 RID: 44355 RVA: 0x00316DE0 File Offset: 0x00314FE0
		public static void DebugLogTestFutureIncidents(bool currentMapOnly, StorytellerComp onlyThisComp = null, QuestPart onlyThisQuestPart = null, int numTestDays = 100)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<IIncidentTarget, int> incCountsForTarget;
			int[] incCountsForComp;
			List<Pair<IncidentDef, IncidentParms>> allIncidents;
			int threatBigCount;
			StorytellerUtility.DebugGetFutureIncidents(numTestDays, currentMapOnly, out incCountsForTarget, out incCountsForComp, out allIncidents, out threatBigCount, stringBuilder, onlyThisComp, null, onlyThisQuestPart);
			new StringBuilder();
			string str = "Test future incidents for ";
			StorytellerDef def = Find.Storyteller.def;
			string text = str + ((def != null) ? def.ToString() : null);
			if (onlyThisComp != null)
			{
				text = text + " (" + ((onlyThisComp != null) ? onlyThisComp.ToString() : null) + ")";
			}
			text = string.Concat(new string[]
			{
				text,
				" (",
				Find.TickManager.TicksGame.TicksToDays().ToString("F1"),
				"d - ",
				(Find.TickManager.TicksGame + numTestDays * 60000).TicksToDays().ToString("F1"),
				"d)"
			});
			StorytellerUtility.DebugLogIncidentsInternal(allIncidents, threatBigCount, incCountsForTarget, incCountsForComp, numTestDays, stringBuilder.ToString(), text);
		}

		// Token: 0x0600AD44 RID: 44356 RVA: 0x00316ED8 File Offset: 0x003150D8
		public static void DebugLogTestFutureIncidents(ThreatsGeneratorParams parms)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<IIncidentTarget, int> incCountsForTarget;
			int[] incCountsForComp;
			List<Pair<IncidentDef, IncidentParms>> allIncidents;
			int threatBigCount;
			StorytellerUtility.DebugGetFutureIncidents(20, true, out incCountsForTarget, out incCountsForComp, out allIncidents, out threatBigCount, stringBuilder, null, parms, null);
			new StringBuilder();
			string[] array = new string[7];
			array[0] = "Test future incidents for ThreatsGenerator ";
			array[1] = ((parms != null) ? parms.ToString() : null);
			array[2] = " (";
			array[3] = 20.ToString();
			array[4] = " days, difficulty ";
			int num = 5;
			DifficultyDef difficultyDef = Find.Storyteller.difficultyDef;
			array[num] = ((difficultyDef != null) ? difficultyDef.ToString() : null);
			array[6] = ")";
			string header = string.Concat(array);
			StorytellerUtility.DebugLogIncidentsInternal(allIncidents, threatBigCount, incCountsForTarget, incCountsForComp, 20, stringBuilder.ToString(), header);
		}

		// Token: 0x0600AD45 RID: 44357 RVA: 0x00316F80 File Offset: 0x00315180
		private static void DebugLogIncidentsInternal(List<Pair<IncidentDef, IncidentParms>> allIncidents, int threatBigCount, Dictionary<IIncidentTarget, int> incCountsForTarget, int[] incCountsForComp, int numTestDays, string incidentList, string header)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(header);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Points guess:            " + StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap).ToString());
			stringBuilder.AppendLine("Incident count:          " + incCountsForTarget.Sum((KeyValuePair<IIncidentTarget, int> x) => x.Value).ToString());
			stringBuilder.AppendLine("Incident count per day:  " + ((float)incCountsForTarget.Sum((KeyValuePair<IIncidentTarget, int> x) => x.Value) / (float)numTestDays).ToString("F2"));
			stringBuilder.AppendLine("ThreatBig count:         " + threatBigCount.ToString());
			stringBuilder.AppendLine("ThreatBig count per day: " + ((float)threatBigCount / (float)numTestDays).ToString("F2"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident count per def:");
			using (IEnumerator<IncidentDef> enumerator = (from x in (from x in allIncidents
			select x.First).Distinct<IncidentDef>()
			orderby x.category.defName, x.defName
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IncidentDef inc = enumerator.Current;
					int num = (from i in allIncidents
					where i.First == inc
					select i).Count<Pair<IncidentDef, IncidentParms>>();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"  ",
						inc.category.defName.PadRight(20),
						" ",
						inc.defName.PadRight(35),
						" ",
						num.ToString()
					}));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident count per target:");
			foreach (KeyValuePair<IIncidentTarget, int> keyValuePair in from kvp in incCountsForTarget
			orderby kvp.Value
			select kvp)
			{
				stringBuilder.AppendLine("  " + keyValuePair.Key.ToString().PadRight(30) + " " + keyValuePair.Value.ToString());
			}
			if (ModsConfig.AnomalyActive)
			{
				List<IncidentCategoryDef> allDefsListForReading = DefDatabase<IncidentCategoryDef>.AllDefsListForReading;
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Anomaly % by category:");
				using (List<IncidentCategoryDef>.Enumerator enumerator3 = allDefsListForReading.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						IncidentCategoryDef cat = enumerator3.Current;
						if (Storyteller.AnomalyIncidents.Count((IncidentDef incident) => incident.category == cat) > 0)
						{
							IEnumerable<Pair<IncidentDef, IncidentParms>> enumerable = from i in allIncidents
							where i.First.category == cat
							select i;
							if (!enumerable.EnumerableNullOrEmpty<Pair<IncidentDef, IncidentParms>>())
							{
								float num2 = enumerable.Percent((Pair<IncidentDef, IncidentParms> i) => i.First.IsAnomalyIncident);
								num2 = Mathf.Max(num2, 0f);
								stringBuilder.AppendLine(cat.defName + ": " + num2.ToStringPercent());
							}
						}
					}
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incidents per StorytellerComp:");
			for (int j = 0; j < incCountsForComp.Length; j++)
			{
				stringBuilder.AppendLine("  M" + j.ToString().PadRight(5) + incCountsForComp[j].ToString());
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Full incident record:");
			stringBuilder.Append(incidentList);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600AD46 RID: 44358 RVA: 0x003173EC File Offset: 0x003155EC
		public static void DebugGetFutureIncidents(int numTestDays, bool currentMapOnly, out Dictionary<IIncidentTarget, int> incCountsForTarget, out int[] incCountsForComp, out List<Pair<IncidentDef, IncidentParms>> allIncidents, out int threatBigCount, StringBuilder outputSb = null, StorytellerComp onlyThisComp = null, ThreatsGeneratorParams onlyThisThreatsGenerator = null, QuestPart onlyThisQuestPart = null)
		{
			int ticksGame = Find.TickManager.TicksGame;
			IncidentQueue incidentQueue = Find.Storyteller.incidentQueue;
			List<IIncidentTarget> allIncidentTargets = Find.Storyteller.AllIncidentTargets;
			StorytellerUtility.tmpOldStoryStates.Clear();
			for (int i = 0; i < allIncidentTargets.Count; i++)
			{
				IIncidentTarget incidentTarget = allIncidentTargets[i];
				StorytellerUtility.tmpOldStoryStates.Add(incidentTarget, incidentTarget.StoryState);
				new StoryState(incidentTarget).CopyTo(incidentTarget.StoryState);
			}
			Find.Storyteller.incidentQueue = new IncidentQueue();
			int num = numTestDays * 60;
			incCountsForComp = new int[Find.Storyteller.storytellerComps.Count];
			incCountsForTarget = new Dictionary<IIncidentTarget, int>();
			allIncidents = new List<Pair<IncidentDef, IncidentParms>>();
			threatBigCount = 0;
			Func<FiringIncident, bool> <>9__0;
			for (int j = 0; j < num; j++)
			{
				IEnumerable<FiringIncident> enumerable;
				if (onlyThisThreatsGenerator != null)
				{
					enumerable = ThreatsGenerator.MakeIntervalIncidents(onlyThisThreatsGenerator, Find.CurrentMap, ticksGame);
				}
				else if (onlyThisComp != null)
				{
					enumerable = Find.Storyteller.MakeIncidentsForInterval(onlyThisComp, Find.Storyteller.AllIncidentTargets);
				}
				else if (onlyThisQuestPart != null)
				{
					IEnumerable<FiringIncident> source = Find.Storyteller.MakeIncidentsForInterval();
					Func<FiringIncident, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((FiringIncident x) => x.sourceQuestPart == onlyThisQuestPart));
					}
					enumerable = source.Where(predicate);
				}
				else
				{
					enumerable = Find.Storyteller.MakeIncidentsForInterval();
				}
				foreach (FiringIncident firingIncident in enumerable)
				{
					if (firingIncident == null)
					{
						Log.Error("Null incident generated.");
					}
					if (!currentMapOnly || firingIncident.parms.target == Find.CurrentMap)
					{
						firingIncident.parms.target.StoryState.Notify_IncidentFired(firingIncident);
						Find.Storyteller.RecordIncidentFired(firingIncident.def);
						allIncidents.Add(new Pair<IncidentDef, IncidentParms>(firingIncident.def, firingIncident.parms));
						if (!incCountsForTarget.ContainsKey(firingIncident.parms.target))
						{
							incCountsForTarget[firingIncident.parms.target] = 0;
						}
						Dictionary<IIncidentTarget, int> dictionary = incCountsForTarget;
						IIncidentTarget target = firingIncident.parms.target;
						int num2 = dictionary[target];
						dictionary[target] = num2 + 1;
						string text;
						if (firingIncident.def.category == IncidentCategoryDefOf.ThreatBig)
						{
							threatBigCount++;
							text = "T ";
						}
						else if (firingIncident.def.category == IncidentCategoryDefOf.ThreatSmall)
						{
							text = "S ";
						}
						else
						{
							text = "  ";
						}
						string text2;
						if (onlyThisThreatsGenerator != null)
						{
							text2 = "";
						}
						else
						{
							int num3 = Find.Storyteller.storytellerComps.IndexOf(firingIncident.source);
							if (num3 >= 0)
							{
								incCountsForComp[num3]++;
								text2 = "M" + num3.ToString() + " ";
							}
							else
							{
								text2 = "";
							}
						}
						text2 = text2.PadRight(4);
						if (outputSb != null)
						{
							string[] array = new string[5];
							array[0] = text2;
							array[1] = text;
							array[2] = (Find.TickManager.TicksGame.TicksToDays().ToString("F1") + "d").PadRight(6);
							array[3] = " ";
							int num4 = 4;
							FiringIncident firingIncident2 = firingIncident;
							array[num4] = ((firingIncident2 != null) ? firingIncident2.ToString() : null);
							outputSb.AppendLine(string.Concat(array));
						}
					}
				}
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1000);
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.Storyteller.incidentQueue = incidentQueue;
			for (int k = 0; k < allIncidentTargets.Count; k++)
			{
				StorytellerUtility.tmpOldStoryStates[allIncidentTargets[k]].CopyTo(allIncidentTargets[k].StoryState);
			}
			StorytellerUtility.tmpOldStoryStates.Clear();
		}

		// Token: 0x0600AD47 RID: 44359 RVA: 0x003177D4 File Offset: 0x003159D4
		public static void DebugLogTestIncidentTargets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Available incident targets:\n");
			foreach (IIncidentTarget incidentTarget in Find.Storyteller.AllIncidentTargets)
			{
				stringBuilder.AppendLine(incidentTarget.ToString());
				foreach (IncidentTargetTagDef incidentTargetTagDef in incidentTarget.IncidentTargetTags())
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string str = "  ";
					IncidentTargetTagDef incidentTargetTagDef2 = incidentTargetTagDef;
					stringBuilder2.AppendLine(str + ((incidentTargetTagDef2 != null) ? incidentTargetTagDef2.ToString() : null));
				}
				stringBuilder.AppendLine("");
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04007725 RID: 30501
		public const float GlobalPointsMinRangeFloor = 35f;

		// Token: 0x04007726 RID: 30502
		public const float GlobalPointsMax = 10000f;

		// Token: 0x04007727 RID: 30503
		public const float BuildingWealthFactor = 0.5f;

		// Token: 0x04007728 RID: 30504
		private static readonly SimpleCurve PointsPerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(14000f, 0f),
				true
			},
			{
				new CurvePoint(400000f, 2400f),
				true
			},
			{
				new CurvePoint(700000f, 3600f),
				true
			},
			{
				new CurvePoint(1000000f, 4200f),
				true
			}
		};

		// Token: 0x04007729 RID: 30505
		public const float FixedWeathModeMaxThreatLevelInYears = 12f;

		// Token: 0x0400772A RID: 30506
		public static readonly SimpleCurve FixedWealthModeMapWealthFromTimeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 10000f),
				true
			},
			{
				new CurvePoint(180f, 180000f),
				true
			},
			{
				new CurvePoint(720f, 1000000f),
				true
			},
			{
				new CurvePoint(1800f, 2500000f),
				true
			}
		};

		// Token: 0x0400772B RID: 30507
		private const float PointsPerTameNonDownedCombatTrainableAnimalCombatPower = 0.08f;

		// Token: 0x0400772C RID: 30508
		private const float PointsPerPlayerPawnFactorInContainer = 0.3f;

		// Token: 0x0400772D RID: 30509
		private const float PointsPerPlayerPawnHealthSummaryLerpAmount = 0.65f;

		// Token: 0x0400772E RID: 30510
		private static readonly SimpleCurve PointsPerColonistByWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 15f),
				true
			},
			{
				new CurvePoint(10000f, 15f),
				true
			},
			{
				new CurvePoint(400000f, 140f),
				true
			},
			{
				new CurvePoint(1000000f, 200f),
				true
			}
		};

		// Token: 0x0400772F RID: 30511
		private static readonly SimpleCurve PointsFactorForPawnAgeYearsCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(13f, 0.5f),
				true
			},
			{
				new CurvePoint(18f, 1f),
				true
			}
		};

		// Token: 0x04007730 RID: 30512
		private static readonly SimpleCurve PointsFactorForColonyMechsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(10000f, 0.2f),
				true
			},
			{
				new CurvePoint(400000f, 0.3f),
				true
			},
			{
				new CurvePoint(1000000f, 0.4f),
				true
			}
		};

		// Token: 0x04007731 RID: 30513
		private static readonly SimpleCurve PointsFactorForColonySubhumanCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(10000f, 0.2f),
				true
			},
			{
				new CurvePoint(400000f, 0.3f),
				true
			},
			{
				new CurvePoint(1000000f, 0.4f),
				true
			}
		};

		// Token: 0x04007732 RID: 30514
		private const float PointsPerPlayerSlaveFactor = 0.75f;

		// Token: 0x04007733 RID: 30515
		public const float CaravanWealthPointsFactor = 0.7f;

		// Token: 0x04007734 RID: 30516
		public const float CaravanAnimalPointsFactor = 0.7f;

		// Token: 0x04007735 RID: 30517
		public const float GravshipWealthPointsFactor = 0.7f;

		// Token: 0x04007736 RID: 30518
		public static readonly FloatRange CaravanPointsRandomFactorRange = new FloatRange(0.7f, 0.9f);

		// Token: 0x04007737 RID: 30519
		public static readonly FloatRange GravshipPointsRandomFactorRange = new FloatRange(0.7f, 0.9f);

		// Token: 0x04007738 RID: 30520
		private static readonly SimpleCurve AllyIncidentFractionFromAllyFraction = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(0.25f, 0.6f),
				true
			}
		};

		// Token: 0x04007739 RID: 30521
		public const float ProgressScorePerWealth = 0.0001f;

		// Token: 0x0400773A RID: 30522
		public const float ProgressScorePerFreeColonist = 1f;

		// Token: 0x0400773B RID: 30523
		private static Dictionary<IIncidentTarget, StoryState> tmpOldStoryStates = new Dictionary<IIncidentTarget, StoryState>();
	}
}
