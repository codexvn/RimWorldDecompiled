using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001D27 RID: 7463
	public class GameCondition : IExposable, ILoadReferenceable
	{
		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x0600A6F5 RID: 42741 RVA: 0x002FE03C File Offset: 0x002FC23C
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x0600A6F6 RID: 42742 RVA: 0x002FE049 File Offset: 0x002FC249
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x0600A6F7 RID: 42743 RVA: 0x002FE056 File Offset: 0x002FC256
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x0600A6F8 RID: 42744 RVA: 0x002FE069 File Offset: 0x002FC269
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600A6F9 RID: 42745 RVA: 0x002FE076 File Offset: 0x002FC276
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x0600A6FA RID: 42746 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x0600A6FB RID: 42747 RVA: 0x002FE09B File Offset: 0x002FC29B
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600A6FC RID: 42748 RVA: 0x002FE0AE File Offset: 0x002FC2AE
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600A6FD RID: 42749 RVA: 0x00222DEB File Offset: 0x00220FEB
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600A6FE RID: 42750 RVA: 0x002FE0BB File Offset: 0x002FC2BB
		// (set) Token: 0x0600A6FF RID: 42751 RVA: 0x002FE0E7 File Offset: 0x002FC2E7
		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600A700 RID: 42752 RVA: 0x002FE0F7 File Offset: 0x002FC2F7
		// (set) Token: 0x0600A701 RID: 42753 RVA: 0x002FE0FF File Offset: 0x002FC2FF
		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600A702 RID: 42754 RVA: 0x002FE112 File Offset: 0x002FC312
		// (set) Token: 0x0600A703 RID: 42755 RVA: 0x002FE137 File Offset: 0x002FC337
		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867);
					return 360000000;
				}
				return this.duration;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600A704 RID: 42756 RVA: 0x002FE148 File Offset: 0x002FC348
		public virtual string TooltipString
		{
			get
			{
				string text = this.def.LabelCap.ToString();
				Map map;
				if (this.SingleMap != null)
				{
					map = this.SingleMap;
				}
				else if (Find.CurrentMap != null)
				{
					map = Find.CurrentMap;
				}
				else
				{
					map = Find.AnyPlayerHomeMap;
				}
				if (this.Permanent && !this.forceDisplayAsDuration && this.def.showPermanentInTooltip)
				{
					text += "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile) : Vector2.zero;
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"Started".Translate(),
						": ",
						GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick), location).Colorize(ColoredText.DateTimeColor)
					});
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"Lasted".Translate(),
						": ",
						this.TicksPassed.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
					});
				}
				text += "\n";
				text = text + "\n" + this.Description.ResolveTags();
				if (this.conditionCauser != null && !this.hideSource && CameraJumper.CanJump(this.conditionCauser))
				{
					text = text + "\n\n" + this.def.jumpToSourceKey.Translate().Resolve();
				}
				else
				{
					Quest quest = this.quest;
					if (quest != null && !quest.hidden)
					{
						text = text + "\n\n" + "CausedByQuest".Translate(this.quest.name).Resolve();
					}
					else if (this.psychicRitualDef != null)
					{
						text += string.Format("\n\n{0}: {1}", "CausedByPsychicRitual".Translate(), this.psychicRitualDef.label.CapitalizeFirst());
					}
					else if (!this.def.natural)
					{
						text += string.Format("\n\n{0}", "SourceUnknown".Translate());
					}
				}
				if (map != null && GameCondition.MapExcludedByFilter(this.def, map))
				{
					text += string.Format("\n\n{0}", "ThisWillNotAffectLayer".Translate(map.Tile.LayerDef.gerundLabel.Named("GERUND"), map.Tile.LayerDef.label.Named("LAYER")));
				}
				return text;
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600A705 RID: 42757 RVA: 0x002FE424 File Offset: 0x002FC624
		public List<Map> AffectedMaps
		{
			get
			{
				if (!GenCollection.ListsEqual<Map>(this.cachedAffectedMapsForMaps, Find.Maps))
				{
					this.cachedAffectedMapsForMaps.Clear();
					this.cachedAffectedMapsForMaps.AddRange(Find.Maps);
					this.cachedAffectedMaps.Clear();
					if (this.CanApplyOnMap(this.gameConditionManager.ownerMap))
					{
						this.cachedAffectedMaps.Add(this.gameConditionManager.ownerMap);
					}
					GameCondition.tmpGameConditionManagers.Clear();
					this.gameConditionManager.GetChildren(GameCondition.tmpGameConditionManagers);
					for (int i = 0; i < GameCondition.tmpGameConditionManagers.Count; i++)
					{
						if (this.CanApplyOnMap(GameCondition.tmpGameConditionManagers[i].ownerMap))
						{
							this.cachedAffectedMaps.Add(GameCondition.tmpGameConditionManagers[i].ownerMap);
						}
					}
					GameCondition.tmpGameConditionManagers.Clear();
				}
				return this.cachedAffectedMaps;
			}
		}

		// Token: 0x0600A706 RID: 42758 RVA: 0x002FE508 File Offset: 0x002FC708
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<bool>(ref this.suppressEndMessage, "suppressEndMessage", false, false);
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<bool>(ref this.forceDisplayAsDuration, "forceDisplayAsDuration", false, false);
			Scribe_Values.Look<bool>(ref this.hideSource, "hideSource", false, false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.psychicRitualDef, "psychicRitualDef");
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600A707 RID: 42759 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x0600A708 RID: 42760 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x0600A709 RID: 42761 RVA: 0x002FE5CA File Offset: 0x002FC7CA
		public virtual void Init()
		{
			if (!this.def.startMessage.NullOrEmpty())
			{
				Messages.Message(this.def.startMessage, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600A70A RID: 42762 RVA: 0x002FE5F4 File Offset: 0x002FC7F4
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null && !this.cachedAffectedMaps.Any(new Predicate<Map>(this.HiddenByOtherCondition)))
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.OnConditionEnd(this);
		}

		// Token: 0x0600A70B RID: 42763 RVA: 0x002FE654 File Offset: 0x002FC854
		public bool CanApplyOnMap(Map map)
		{
			if (map == null)
			{
				return false;
			}
			if (map.generatorDef.isUnderground && !this.def.allowUnderground)
			{
				return false;
			}
			if (GameCondition.MapExcludedByFilter(this.def, map))
			{
				return false;
			}
			if (ModsConfig.OdysseyActive && this.def.requireFish)
			{
				WaterBodyTracker waterBodyTracker = map.waterBodyTracker;
				if (waterBodyTracker == null || !waterBodyTracker.AnyBodyContainsFish)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A70C RID: 42764 RVA: 0x002FE6BC File Offset: 0x002FC8BC
		public static bool MapExcludedByFilter(GameConditionDef def, Map map)
		{
			return map.Tile.Valid && ((!def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !def.layerWhitelist.Contains(map.Tile.LayerDef)) || (!def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && def.layerBlacklist.Contains(map.Tile.LayerDef)) || (!def.canAffectAllPlanetLayers && map.Tile.LayerDef.onlyAllowWhitelistedGameConditions && (def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || !def.layerWhitelist.Contains(map.Tile.LayerDef))));
		}

		// Token: 0x0600A70D RID: 42765 RVA: 0x002FE778 File Offset: 0x002FC978
		public bool HiddenByOtherCondition(Map map)
		{
			if (this.def.silencedByConditions.NullOrEmpty<GameConditionDef>())
			{
				return false;
			}
			for (int i = 0; i < this.def.silencedByConditions.Count; i++)
			{
				if (map.gameConditionManager.ConditionIsActive(this.def.silencedByConditions[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A70E RID: 42766 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600A70F RID: 42767 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600A710 RID: 42768 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x0600A711 RID: 42769 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x0600A712 RID: 42770 RVA: 0x002FE7D8 File Offset: 0x002FC9D8
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x0600A713 RID: 42771 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600A714 RID: 42772 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x0600A715 RID: 42773 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x0600A716 RID: 42774 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x0600A717 RID: 42775 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x0600A718 RID: 42776 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		// Token: 0x0600A719 RID: 42777 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float MinWindSpeed()
		{
			return 0f;
		}

		// Token: 0x0600A71A RID: 42778 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float WeatherCommonalityFactor(WeatherDef weather, Map map)
		{
			return 1f;
		}

		// Token: 0x0600A71B RID: 42779 RVA: 0x002FE7EE File Offset: 0x002FC9EE
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		// Token: 0x0600A71C RID: 42780 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		// Token: 0x0600A71D RID: 42781 RVA: 0x002FE800 File Offset: 0x002FCA00
		public string GetUniqueLoadID()
		{
			return base.GetType().Name + "_" + this.uniqueID.ToString();
		}

		// Token: 0x040073CB RID: 29643
		public GameConditionManager gameConditionManager;

		// Token: 0x040073CC RID: 29644
		public Thing conditionCauser;

		// Token: 0x040073CD RID: 29645
		public bool hideSource;

		// Token: 0x040073CE RID: 29646
		public GameConditionDef def;

		// Token: 0x040073CF RID: 29647
		public int uniqueID = -1;

		// Token: 0x040073D0 RID: 29648
		public int startTick;

		// Token: 0x040073D1 RID: 29649
		public bool suppressEndMessage;

		// Token: 0x040073D2 RID: 29650
		private int duration = -1;

		// Token: 0x040073D3 RID: 29651
		private bool permanent;

		// Token: 0x040073D4 RID: 29652
		public bool forceDisplayAsDuration;

		// Token: 0x040073D5 RID: 29653
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x040073D6 RID: 29654
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x040073D7 RID: 29655
		public Quest quest;

		// Token: 0x040073D8 RID: 29656
		public PsychicRitualDef psychicRitualDef;

		// Token: 0x040073D9 RID: 29657
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
