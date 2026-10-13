using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E23 RID: 7715
	public class Storyteller : IExposable
	{
		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600AB97 RID: 43927 RVA: 0x003112E6 File Offset: 0x0030F4E6
		[Obsolete("Use \"difficulty\" instead.")]
		public Difficulty difficultyValues
		{
			get
			{
				return this.difficulty;
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600AB98 RID: 43928 RVA: 0x003112EE File Offset: 0x0030F4EE
		public int LastIncidentTick
		{
			get
			{
				return this.lastIncidentTick;
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x0600AB99 RID: 43929 RVA: 0x003112F8 File Offset: 0x0030F4F8
		public List<IIncidentTarget> AllIncidentTargets
		{
			get
			{
				Storyteller.tmpAllIncidentTargets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Storyteller.tmpAllIncidentTargets.Add(maps[i]);
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled)
					{
						Storyteller.tmpAllIncidentTargets.Add(caravans[j]);
					}
				}
				Storyteller.tmpAllIncidentTargets.Add(Find.World);
				return Storyteller.tmpAllIncidentTargets;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600AB9A RID: 43930 RVA: 0x00311386 File Offset: 0x0030F586
		public static List<IncidentDef> AnomalyIncidents
		{
			get
			{
				List<IncidentDef> result;
				if ((result = Storyteller.anomalyIncidents) == null)
				{
					result = (Storyteller.anomalyIncidents = (from incident in DefDatabase<IncidentDef>.AllDefsListForReading
					where incident.IsAnomalyIncident
					select incident).ToList<IncidentDef>());
				}
				return result;
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x0600AB9B RID: 43931 RVA: 0x003113C5 File Offset: 0x0030F5C5
		public static List<IncidentDef> NonAnomalyIncidents
		{
			get
			{
				List<IncidentDef> result;
				if ((result = Storyteller.nonAnomalyIncidents) == null)
				{
					result = (Storyteller.nonAnomalyIncidents = (from incident in DefDatabase<IncidentDef>.AllDefsListForReading
					where !incident.IsAnomalyIncident
					select incident).ToList<IncidentDef>());
				}
				return result;
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x0600AB9C RID: 43932 RVA: 0x00311404 File Offset: 0x0030F604
		public float AnomalyIncidentChanceNow
		{
			get
			{
				return Mathf.Clamp01(Find.Anomaly.AnomalyThreatFractionNow);
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x0600AB9D RID: 43933 RVA: 0x00311418 File Offset: 0x0030F618
		public float QuestGiverTraderChanceNow
		{
			get
			{
				if (Find.TickManager.TicksGame - this.lastQuestGiverTraderTick < 600000)
				{
					return 0f;
				}
				if (Find.TickManager.TicksGame - this.lastQuestGiverTraderTick > 3600000)
				{
					return 1f;
				}
				return 0.15f;
			}
		}

		// Token: 0x0600AB9E RID: 43934 RVA: 0x00311466 File Offset: 0x0030F666
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x0600AB9F RID: 43935 RVA: 0x00311474 File Offset: 0x0030F674
		public Storyteller()
		{
		}

		// Token: 0x0600ABA0 RID: 43936 RVA: 0x003114DF File Offset: 0x0030F6DF
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef) : this(def, difficultyDef, new Difficulty(difficultyDef))
		{
		}

		// Token: 0x0600ABA1 RID: 43937 RVA: 0x003114F0 File Offset: 0x0030F6F0
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef, Difficulty difficulty)
		{
			this.def = def;
			this.difficultyDef = difficultyDef;
			this.difficulty = difficulty;
			this.InitializeStorytellerComps();
		}

		// Token: 0x0600ABA2 RID: 43938 RVA: 0x00311578 File Offset: 0x0030F778
		private void InitializeStorytellerComps()
		{
			this.storytellerComps = new List<StorytellerComp>();
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				if (this.def.comps[i].Enabled)
				{
					StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(this.def.comps[i].compClass);
					storytellerComp.props = this.def.comps[i];
					storytellerComp.Initialize();
					this.storytellerComps.Add(storytellerComp);
				}
			}
		}

		// Token: 0x0600ABA3 RID: 43939 RVA: 0x00311610 File Offset: 0x0030F810
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficultyDef, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.recentIncidentsAnomaly, "recentIncidentsAnomaly", LookMode.Value);
			Scribe_Values.Look<float>(ref this.recentAnomalyIncidentFactor, "recentAnomalyIncidentFactor", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastQuestGiverTraderTick, "lastQuestGiverTraderTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastIncidentTick, "lastIncidentTick", 0, false);
			if (this.difficultyDef == null)
			{
				Log.Error("Loaded storyteller without difficulty");
				this.difficultyDef = DifficultyDefOf.Rough;
			}
			if (this.difficultyDef.isCustom)
			{
				Scribe_Deep.Look<Difficulty>(ref this.difficulty, "customDifficulty", new object[]
				{
					this.difficultyDef
				});
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.difficulty = new Difficulty(this.difficultyDef);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.Saving)
			{
				this.SetupNonCustomDifficultyAnomalySettings();
			}
		}

		// Token: 0x0600ABA4 RID: 43940 RVA: 0x00311728 File Offset: 0x0030F928
		private void SetupNonCustomDifficultyAnomalySettings()
		{
			if (!ModsConfig.AnomalyActive || this.difficultyDef.isCustom)
			{
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Scribe_Values.Look<float>(ref this.difficulty.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
				Scribe_Values.Look<float>(ref this.difficulty.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
				Scribe_Values.Look<float?>(ref this.difficulty.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
				Scribe_Values.Look<float>(ref this.difficulty.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
				Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
				this.difficulty.AnomalyPlaystyleDef = this.anomalyPlaystyleDef;
				return;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.anomalyThreatsInactiveFraction = this.difficulty.anomalyThreatsInactiveFraction;
				this.anomalyThreatsActiveFraction = this.difficulty.anomalyThreatsActiveFraction;
				this.overrideAnomalyThreatsFraction = this.difficulty.overrideAnomalyThreatsFraction;
				this.studyEfficiencyFactor = this.difficulty.studyEfficiencyFactor;
				this.anomalyPlaystyleDef = this.difficulty.AnomalyPlaystyleDef;
				Scribe_Values.Look<float>(ref this.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
				Scribe_Values.Look<float>(ref this.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
				Scribe_Values.Look<float?>(ref this.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
				Scribe_Values.Look<float>(ref this.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
				Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
			}
		}

		// Token: 0x0600ABA5 RID: 43941 RVA: 0x003118B4 File Offset: 0x0030FAB4
		public void StorytellerTick()
		{
			this.incidentQueue.IncidentQueueTick();
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				if (!DebugSettings.enableStoryteller)
				{
					return;
				}
				foreach (FiringIncident fi in this.MakeIncidentsForInterval())
				{
					this.TryFire(fi, false);
				}
			}
		}

		// Token: 0x0600ABA6 RID: 43942 RVA: 0x00311928 File Offset: 0x0030FB28
		public bool TryFire(FiringIncident fi, bool queued = false)
		{
			if (fi.def.Worker.CanFireNow(fi.parms) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				this.lastIncidentTick = GenTicks.TicksGame;
				return true;
			}
			return false;
		}

		// Token: 0x0600ABA7 RID: 43943 RVA: 0x00311989 File Offset: 0x0030FB89
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			List<IIncidentTarget> targets = this.AllIncidentTargets;
			int num;
			for (int i = 0; i < this.storytellerComps.Count; i = num + 1)
			{
				foreach (FiringIncident firingIncident in this.MakeIncidentsForInterval(this.storytellerComps[i], targets))
				{
					yield return firingIncident;
				}
				IEnumerator<FiringIncident> enumerator = null;
				num = i;
			}
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> parts = quests[i].PartsListForReading;
					for (int j = 0; j < parts.Count; j = num + 1)
					{
						IIncidentMakerQuestPart incidentMakerQuestPart = parts[j] as IIncidentMakerQuestPart;
						if (incidentMakerQuestPart != null && ((QuestPartActivable)parts[j]).State == QuestPartState.Enabled)
						{
							foreach (FiringIncident firingIncident2 in incidentMakerQuestPart.MakeIntervalIncidents())
							{
								firingIncident2.sourceQuestPart = parts[j];
								firingIncident2.parms.quest = quests[i];
								yield return firingIncident2;
							}
							IEnumerator<FiringIncident> enumerator = null;
						}
						num = j;
					}
					parts = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ABA8 RID: 43944 RVA: 0x00311999 File Offset: 0x0030FB99
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
			if (GenDate.DaysPassedSinceSettleFloat <= comp.props.minDaysPassed)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < targets.Count; i = num + 1)
			{
				IIncidentTarget incidentTarget = targets[i];
				bool flag = false;
				bool flag2 = comp.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>();
				foreach (IncidentTargetTagDef item in incidentTarget.IncidentTargetTags())
				{
					if (!comp.props.disallowedTargetTags.NullOrEmpty<IncidentTargetTagDef>() && comp.props.disallowedTargetTags.Contains(item))
					{
						flag = true;
						break;
					}
					if (!flag2 && comp.props.allowedTargetTags.Contains(item))
					{
						flag2 = true;
					}
				}
				if (!flag && flag2)
				{
					foreach (FiringIncident firingIncident in comp.MakeIntervalIncidents(incidentTarget))
					{
						if ((Find.Storyteller.difficulty.allowBigThreats || firingIncident.def.category != IncidentCategoryDefOf.ThreatBig) && (!ModsConfig.AnomalyActive || Find.TickManager.TicksGame - Find.Anomaly.metalHellClosedTick >= 300000 || firingIncident.def.category != IncidentCategoryDefOf.ThreatBig))
						{
							yield return firingIncident;
						}
					}
					IEnumerator<FiringIncident> enumerator2 = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ABA9 RID: 43945 RVA: 0x003119B0 File Offset: 0x0030FBB0
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		// Token: 0x0600ABAA RID: 43946 RVA: 0x003119FC File Offset: 0x0030FBFC
		public void Notify_DissolutionEvent(Thing thing)
		{
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_DissolutionEvent(thing);
			}
		}

		// Token: 0x0600ABAB RID: 43947 RVA: 0x00311A31 File Offset: 0x0030FC31
		public void Notify_QuestGiverTraderSpawned()
		{
			this.lastQuestGiverTraderTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600ABAC RID: 43948 RVA: 0x00311A43 File Offset: 0x0030FC43
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x0600ABAD RID: 43949 RVA: 0x00311A4C File Offset: 0x0030FC4C
		public void RecordIncidentFired(IncidentDef incident)
		{
			if (!ModsConfig.AnomalyActive || !incident.category.canUseAnomalyChance)
			{
				return;
			}
			if (this.recentIncidentsAnomaly == null)
			{
				this.recentIncidentsAnomaly = new Queue<bool>();
			}
			if (this.recentIncidentsAnomaly.Count >= 4)
			{
				this.recentIncidentsAnomaly.Dequeue();
			}
			this.recentIncidentsAnomaly.Enqueue(incident.IsAnomalyIncident);
			this.recentAnomalyIncidentFactor = this.recentIncidentsAnomaly.Average(delegate(bool b)
			{
				if (!b)
				{
					return 0f;
				}
				return 1f;
			});
		}

		// Token: 0x0600ABAE RID: 43950 RVA: 0x00311ADC File Offset: 0x0030FCDC
		public static void ResetStaticData()
		{
			Storyteller.anomalyIncidents = null;
			Storyteller.nonAnomalyIncidents = null;
		}

		// Token: 0x0600ABAF RID: 43951 RVA: 0x00311AEC File Offset: 0x0030FCEC
		public string DebugString()
		{
			if (Time.frameCount % 60 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("GLOBAL STORYTELLER STATS");
				stringBuilder.AppendLine("------------------------");
				stringBuilder.AppendLine("Storyteller: ".PadRight(40) + this.def.label);
				stringBuilder.AppendLine("Adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.AdaptDays.ToString("F1"));
				stringBuilder.AppendLine("Adapt points factor: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor.ToString("F2"));
				stringBuilder.AppendLine("Time points factor: ".PadRight(40) + Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassedSinceSettle).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Ally incident fraction (neutral or ally): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(false).ToString("F2"));
				stringBuilder.AppendLine("Ally incident fraction (ally only): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(true).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(StorytellerUtilityPopulation.DebugReadout().TrimEndNewlines());
				IIncidentTarget incidentTarget = (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) ?? Find.CurrentMap;
				if (incidentTarget != null)
				{
					Map map = incidentTarget as Map;
					stringBuilder.AppendLine();
					StringBuilder stringBuilder2 = stringBuilder;
					string str = "STATS FOR INCIDENT TARGET: ";
					IIncidentTarget incidentTarget2 = incidentTarget;
					stringBuilder2.AppendLine(str + ((incidentTarget2 != null) ? incidentTarget2.ToString() : null));
					stringBuilder.AppendLine("------------------------");
					stringBuilder.AppendLine("Progress score: ".PadRight(40) + StorytellerUtility.GetProgressScore(incidentTarget).ToString("F2"));
					stringBuilder.AppendLine("Base points: ".PadRight(40) + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
					stringBuilder.AppendLine("Points factor random range: ".PadRight(40) + incidentTarget.IncidentPointsRandomFactorRange.ToString());
					stringBuilder.AppendLine("Wealth: ".PadRight(40) + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
					if (map != null)
					{
						if (Find.Storyteller.difficulty.fixedWealthMode)
						{
							stringBuilder.AppendLine(string.Format("- Wealth calculated using fixed model curve, time factor: {0:F1}", Find.Storyteller.difficulty.fixedWealthTimeFactor));
							stringBuilder.AppendLine("- Map age: ".PadRight(40) + map.AgeInDays.ToString("F1"));
						}
						stringBuilder.AppendLine("- Items: ".PadRight(40) + map.wealthWatcher.WealthItems.ToString("F0"));
						stringBuilder.AppendLine("- Buildings: ".PadRight(40) + map.wealthWatcher.WealthBuildings.ToString("F0"));
						stringBuilder.AppendLine("- Floors: ".PadRight(40) + map.wealthWatcher.WealthFloorsOnly.ToString("F0"));
						stringBuilder.AppendLine("- Pawns: ".PadRight(40) + map.wealthWatcher.WealthPawns.ToString("F0"));
					}
					stringBuilder.AppendLine("Pawn count human: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.def.race.Humanlike).ToString());
					stringBuilder.AppendLine("Pawn count animal: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.IsAnimal).ToString());
					if (map != null)
					{
						stringBuilder.AppendLine("StoryDanger: ".PadRight(40) + map.dangerWatcher.DangerRating.ToString());
						stringBuilder.AppendLine("FireDanger: ".PadRight(40) + map.fireWatcher.FireDanger.ToString("F2"));
						stringBuilder.AppendLine("LastThreatBigTick days ago: ".PadRight(40) + (Find.TickManager.TicksGame - map.storyState.LastThreatBigTick).ToStringTicksToDays("F1"));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("LIST OF ALL INCIDENT TARGETS");
				stringBuilder.AppendLine("------------------------");
				for (int i = 0; i < this.AllIncidentTargets.Count; i++)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					string str2 = i.ToString();
					string str3 = ". ";
					IIncidentTarget incidentTarget3 = this.AllIncidentTargets[i];
					stringBuilder3.AppendLine(str2 + str3 + ((incidentTarget3 != null) ? incidentTarget3.ToString() : null));
				}
				this.debugStringCached = stringBuilder.ToString();
			}
			return this.debugStringCached;
		}

		// Token: 0x040075F6 RID: 30198
		public StorytellerDef def;

		// Token: 0x040075F7 RID: 30199
		public DifficultyDef difficultyDef;

		// Token: 0x040075F8 RID: 30200
		public Difficulty difficulty = new Difficulty();

		// Token: 0x040075F9 RID: 30201
		public List<StorytellerComp> storytellerComps;

		// Token: 0x040075FA RID: 30202
		private static List<IncidentDef> anomalyIncidents;

		// Token: 0x040075FB RID: 30203
		private static List<IncidentDef> nonAnomalyIncidents;

		// Token: 0x040075FC RID: 30204
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x040075FD RID: 30205
		private Queue<bool> recentIncidentsAnomaly = new Queue<bool>();

		// Token: 0x040075FE RID: 30206
		private float recentAnomalyIncidentFactor = 0.5f;

		// Token: 0x040075FF RID: 30207
		private int lastQuestGiverTraderTick;

		// Token: 0x04007600 RID: 30208
		private int lastIncidentTick;

		// Token: 0x04007601 RID: 30209
		private float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x04007602 RID: 30210
		private float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x04007603 RID: 30211
		private float? overrideAnomalyThreatsFraction;

		// Token: 0x04007604 RID: 30212
		private float studyEfficiencyFactor = 1f;

		// Token: 0x04007605 RID: 30213
		private AnomalyPlaystyleDef anomalyPlaystyleDef;

		// Token: 0x04007606 RID: 30214
		public static readonly Vector2 PortraitSizeTiny = new Vector2(122f, 130f);

		// Token: 0x04007607 RID: 30215
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x04007608 RID: 30216
		public const int IntervalsPerDay = 60;

		// Token: 0x04007609 RID: 30217
		public const int CheckInterval = 1000;

		// Token: 0x0400760A RID: 30218
		private const int RecentAnomalyIncidentsToRecord = 4;

		// Token: 0x0400760B RID: 30219
		private const float QuestGiverTraderChance = 0.15f;

		// Token: 0x0400760C RID: 30220
		private const int QuestGiverTraderMinIntervalTicks = 600000;

		// Token: 0x0400760D RID: 30221
		private const int QuestGiverTraderMaxIntervalTicks = 3600000;

		// Token: 0x0400760E RID: 30222
		private static readonly List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x0400760F RID: 30223
		private string debugStringCached = "Generating data...";
	}
}
