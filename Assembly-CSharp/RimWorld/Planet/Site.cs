using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DD7 RID: 15831
	[StaticConstructorOnStartup]
	public class Site : MapParent
	{
		// Token: 0x170035C9 RID: 13769
		// (get) Token: 0x0601597D RID: 88445 RVA: 0x00676EBC File Offset: 0x006750BC
		public override string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				if (this.MainSitePartDef == SitePartDefOf.PreciousLump && this.MainSitePart.parms.preciousLumpResources != null)
				{
					return "PreciousLumpLabel".Translate(this.MainSitePart.parms.preciousLumpResources.label);
				}
				return this.MainSitePartDef.label;
			}
		}

		// Token: 0x170035CA RID: 13770
		// (get) Token: 0x0601597E RID: 88446 RVA: 0x00676F31 File Offset: 0x00675131
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSitePartDef.ExpandingIconTexture;
			}
		}

		// Token: 0x170035CB RID: 13771
		// (get) Token: 0x0601597F RID: 88447 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HandlesConditionCausers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170035CC RID: 13772
		// (get) Token: 0x06015980 RID: 88448 RVA: 0x00676F40 File Offset: 0x00675140
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSitePartDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSitePartDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x170035CD RID: 13773
		// (get) Token: 0x06015981 RID: 88449 RVA: 0x00676FAB File Offset: 0x006751AB
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;
			}
		}

		// Token: 0x170035CE RID: 13774
		// (get) Token: 0x06015982 RID: 88450 RVA: 0x00676FC8 File Offset: 0x006751C8
		private SitePart MainSitePart
		{
			get
			{
				if (!this.parts.Any<SitePart>())
				{
					Log.ErrorOnce("Site without any SitePart at " + base.Tile.ToString(), this.ID ^ 93890909);
					return null;
				}
				if (this.parts[0].hidden)
				{
					Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile.ToString(), this.ID ^ 48471239);
					return this.parts[0];
				}
				return this.parts[0];
			}
		}

		// Token: 0x170035CF RID: 13775
		// (get) Token: 0x06015983 RID: 88451 RVA: 0x0067706E File Offset: 0x0067526E
		public SitePartDef MainSitePartDef
		{
			get
			{
				return this.MainSitePart.def;
			}
		}

		// Token: 0x170035D0 RID: 13776
		// (get) Token: 0x06015984 RID: 88452 RVA: 0x0067707B File Offset: 0x0067527B
		public override bool GravShipCanLandOn
		{
			get
			{
				return this.MainSitePartDef.gravShipsCanLandOn && !base.HasMap;
			}
		}

		// Token: 0x170035D1 RID: 13777
		// (get) Token: 0x06015985 RID: 88453 RVA: 0x00677095 File Offset: 0x00675295
		public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepWithParams genStepWithParams in base.ExtraGenStepDefs)
				{
					yield return genStepWithParams;
				}
				IEnumerator<GenStepWithParams> enumerator = null;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					GenStepParams partGenStepParams = default(GenStepParams);
					partGenStepParams.sitePart = this.parts[i];
					List<GenStepDef> partGenStepDefs = this.parts[i].def.ExtraGenSteps;
					for (int j = 0; j < partGenStepDefs.Count; j = num + 1)
					{
						yield return new GenStepWithParams(partGenStepDefs[j], partGenStepParams);
						num = j;
					}
					partGenStepParams = default(GenStepParams);
					partGenStepDefs = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170035D2 RID: 13778
		// (get) Token: 0x06015986 RID: 88454 RVA: 0x006770A8 File Offset: 0x006752A8
		public string ApproachOrderString
		{
			get
			{
				return this.MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(this.Label) : this.MainSitePartDef.approachOrderString.Formatted(this.Label);
			}
		}

		// Token: 0x170035D3 RID: 13779
		// (get) Token: 0x06015987 RID: 88455 RVA: 0x00677100 File Offset: 0x00675300
		public string ApproachingReportString
		{
			get
			{
				return this.MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(this.Label) : this.MainSitePartDef.approachingReportString.Formatted(this.Label);
			}
		}

		// Token: 0x170035D4 RID: 13780
		// (get) Token: 0x06015988 RID: 88456 RVA: 0x00677158 File Offset: 0x00675358
		public float ActualThreatPoints
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.parts.Count; i++)
				{
					num += this.parts[i].parms.threatPoints;
				}
				return num;
			}
		}

		// Token: 0x170035D5 RID: 13781
		// (get) Token: 0x06015989 RID: 88457 RVA: 0x0067719C File Offset: 0x0067539C
		public bool IncreasesPopulation
		{
			get
			{
				if (base.HasMap)
				{
					return false;
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.Worker.IncreasesPopulation(this.parts[i].parms))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170035D6 RID: 13782
		// (get) Token: 0x0601598A RID: 88458 RVA: 0x006771FC File Offset: 0x006753FC
		public bool BadEvenIfNoMap
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.badEvenIfNoMap)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170035D7 RID: 13783
		// (get) Token: 0x0601598B RID: 88459 RVA: 0x0067723A File Offset: 0x0067543A
		public bool HasWorldObjectTimeout
		{
			get
			{
				return this.WorldObjectTimeoutTicksLeft != -1;
			}
		}

		// Token: 0x170035D8 RID: 13784
		// (get) Token: 0x0601598C RID: 88460 RVA: 0x00677248 File Offset: 0x00675448
		public int WorldObjectTimeoutTicksLeft
		{
			get
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					Quest quest = questsListForReading[i];
					if (quest.State == QuestState.Ongoing)
					{
						for (int j = 0; j < quest.PartsListForReading.Count; j++)
						{
							QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = quest.PartsListForReading[j] as QuestPart_WorldObjectTimeout;
							if (questPart_WorldObjectTimeout != null && questPart_WorldObjectTimeout.State == QuestPartState.Enabled && questPart_WorldObjectTimeout.worldObject == this)
							{
								return questPart_WorldObjectTimeout.TicksLeft;
							}
						}
					}
				}
				return -1;
			}
		}

		// Token: 0x170035D9 RID: 13785
		// (get) Token: 0x0601598D RID: 88461 RVA: 0x006772CC File Offset: 0x006754CC
		public IntVec3 PreferredMapSize
		{
			get
			{
				IntVec3 defaultMapSize = Site.DefaultMapSize;
				for (int i = 0; i < this.parts.Count; i++)
				{
					SitePart sitePart = this.parts[i];
					if (sitePart.def.minMapSize != null)
					{
						IntVec3 value = sitePart.def.minMapSize.Value;
						defaultMapSize.x = Mathf.Max(value.x, defaultMapSize.x);
						defaultMapSize.y = Mathf.Max(value.y, defaultMapSize.y);
						defaultMapSize.z = Mathf.Max(value.z, defaultMapSize.z);
					}
				}
				return defaultMapSize;
			}
		}

		// Token: 0x170035DA RID: 13786
		// (get) Token: 0x0601598E RID: 88462 RVA: 0x00677374 File Offset: 0x00675574
		public override AcceptanceReport CanBeSettled
		{
			get
			{
				foreach (Quest quest in Find.QuestManager.QuestsListForReading)
				{
					if (quest.State == QuestState.Ongoing)
					{
						foreach (QuestPart questPart in quest.PartsListForReading)
						{
							QuestPart_SpawnWorldObject questPart_SpawnWorldObject = questPart as QuestPart_SpawnWorldObject;
							if (questPart_SpawnWorldObject != null && questPart_SpawnWorldObject.worldObject == this)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0601598F RID: 88463 RVA: 0x0067742C File Offset: 0x0067562C
		public override void Destroy()
		{
			base.Destroy();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDestroy(this.parts[i]);
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].PostDestroy();
			}
		}

		// Token: 0x06015990 RID: 88464 RVA: 0x006774A4 File Offset: 0x006756A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Deep.Look<SiteCoreBackCompat>(ref this.coreBackCompat, "core", Array.Empty<object>());
			Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.caravanAssaultSuccessfulTaleRecorded, "caravanAssaultSuccessfulTaleRecorded", false, false);
			Scribe_Values.Look<bool>(ref this.allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
			Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.preventGravshipLanding, "preventGravshipLanding", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.coreBackCompat != null && this.coreBackCompat.def != null)
				{
					this.parts.Insert(0, new SitePart(this, this.coreBackCompat.def, this.coreBackCompat.parms));
					this.coreBackCompat = null;
				}
				if (this.parts.RemoveAll((SitePart x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some site parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].site = this;
				}
				BackCompatibility.PostExposeData(this);
			}
		}

		// Token: 0x06015991 RID: 88465 RVA: 0x00677618 File Offset: 0x00675818
		public void AddPart(SitePart part)
		{
			if (!part.def.forceMutators.NullOrEmpty<TileMutatorDef>())
			{
				foreach (TileMutatorDef mutator in part.def.forceMutators)
				{
					base.Tile.Tile.AddMutator(mutator);
				}
			}
			this.parts.Add(part);
			part.def.Worker.Init(this, part);
		}

		// Token: 0x06015992 RID: 88466 RVA: 0x006776B0 File Offset: 0x006758B0
		protected override void Tick()
		{
			base.Tick();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.SitePartWorkerTick(this.parts[i]);
			}
		}

		// Token: 0x06015993 RID: 88467 RVA: 0x00677700 File Offset: 0x00675900
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].SitePartTickInterval(delta);
			}
			if (base.HasMap)
			{
				this.CheckRecordAssaultSuccessfulTale();
				this.CheckAllEnemiesDefeated();
			}
		}

		// Token: 0x06015994 RID: 88468 RVA: 0x00677750 File Offset: 0x00675950
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostMapGenerate(map);
			}
			float num = 0f;
			for (int j = 0; j < this.parts.Count; j++)
			{
				num = Mathf.Max(num, this.parts[j].def.forceExitAndRemoveMapCountdownDurationDays);
			}
			num *= MapParentTuning.SiteDetectionCountdownMultiplier.RandomInRange;
			if (!this.parts.Any((SitePart p) => p.def.disallowsAutomaticDetectionTimerStart) && !map.wasSpawnedViaGravShipLanding)
			{
				int ticks = Mathf.RoundToInt(num * 60000f);
				base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks, -1);
			}
			this.allEnemiesDefeatedSignalSent = false;
		}

		// Token: 0x06015995 RID: 88469 RVA: 0x00677840 File Offset: 0x00675A40
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDrawExtraSelectionOverlays(this.parts[i]);
			}
		}

		// Token: 0x06015996 RID: 88470 RVA: 0x00677890 File Offset: 0x00675A90
		public override void Notify_MyMapAboutToBeRemoved()
		{
			base.Notify_MyMapAboutToBeRemoved();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[i]);
			}
		}

		// Token: 0x06015997 RID: 88471 RVA: 0x006778E0 File Offset: 0x00675AE0
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == base.Map && pocketMapParent.Map.mapPawns.AnyPawnBlockingMapRemoval)
				{
					return false;
				}
			}
			if (ModsConfig.OdysseyActive && base.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
			{
				return false;
			}
			alsoRemoveWorldObject = !this.parts.Any((SitePart x) => (x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed) || (x.def.Worker is SitePartWorker_RaidSource && GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, false)));
			if (this.parts.Any(delegate(SitePart x)
			{
				SitePartWorker_AncientAltar sitePartWorker_AncientAltar = x.def.Worker as SitePartWorker_AncientAltar;
				return sitePartWorker_AncientAltar != null && sitePartWorker_AncientAltar.ShouldKeepMapForRelic(x);
			}))
			{
				alsoRemoveWorldObject = false;
			}
			return !TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map);
		}

		// Token: 0x06015998 RID: 88472 RVA: 0x006779F0 File Offset: 0x00675BF0
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			for (int i = 0; i < this.parts.Count; i++)
			{
				outChildren.Add(this.parts[i]);
			}
		}

		// Token: 0x06015999 RID: 88473 RVA: 0x00677A2C File Offset: 0x00675C2C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!base.HasMap)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601599A RID: 88474 RVA: 0x00677A43 File Offset: 0x00675C43
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportersFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_VisitSite.GetFloatMenuOptions(launchAction, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601599B RID: 88475 RVA: 0x00677A61 File Offset: 0x00675C61
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetShuttleFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.def.mapGenerator == MapGeneratorDefOf.Space)
			{
				foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			else
			{
				foreach (FloatMenuOption floatMenuOption3 in TransportersArrivalAction_VisitSite.GetFloatMenuOptions(launchAction, pods, this))
				{
					yield return floatMenuOption3;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601599C RID: 88476 RVA: 0x00677A7F File Offset: 0x00675C7F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601599D RID: 88477 RVA: 0x00677A90 File Offset: 0x00675C90
		private void CheckRecordAssaultSuccessfulTale()
		{
			if (this.anyEnemiesInitially && !this.caravanAssaultSuccessfulTaleRecorded && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, false))
			{
				this.caravanAssaultSuccessfulTaleRecorded = true;
				if (base.Map.mapPawns.FreeColonists.Any<Pawn>())
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		// Token: 0x0601599E RID: 88478 RVA: 0x00677B04 File Offset: 0x00675D04
		private void CheckAllEnemiesDefeated()
		{
			if (this.allEnemiesDefeatedSignalSent || !base.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, true))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			this.allEnemiesDefeatedSignalSent = true;
		}

		// Token: 0x0601599F RID: 88479 RVA: 0x00677B54 File Offset: 0x00675D54
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSitePartDef == this.MainSitePartDef;
		}

		// Token: 0x060159A0 RID: 88480 RVA: 0x00677B7C File Offset: 0x00675D7C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			Site.tmpSitePartsLabels.Clear();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (!this.parts[i].hidden && this.parts[i].def.displayOnInspectPane)
				{
					if (this.MainSitePart == this.parts[i] && !this.parts[i].def.mainPartAllThreatsLabel.NullOrEmpty() && this.ActualThreatPoints > 0f)
					{
						stringBuilder.Length = 0;
						stringBuilder.Append(this.parts[i].def.mainPartAllThreatsLabel.CapitalizeFirst());
						break;
					}
					string postProcessedThreatLabel = this.parts[i].def.Worker.GetPostProcessedThreatLabel(this, this.parts[i]);
					if (!postProcessedThreatLabel.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(postProcessedThreatLabel.CapitalizeFirst());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060159A1 RID: 88481 RVA: 0x00677CB0 File Offset: 0x00675EB0
		public override string GetDescription()
		{
			string text = this.MainSitePartDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}

		// Token: 0x0400EFF5 RID: 61429
		public string customLabel;

		// Token: 0x0400EFF6 RID: 61430
		public List<SitePart> parts = new List<SitePart>();

		// Token: 0x0400EFF7 RID: 61431
		public bool sitePartsKnown = true;

		// Token: 0x0400EFF8 RID: 61432
		public bool factionMustRemainHostile;

		// Token: 0x0400EFF9 RID: 61433
		public float desiredThreatPoints;

		// Token: 0x0400EFFA RID: 61434
		private SiteCoreBackCompat coreBackCompat;

		// Token: 0x0400EFFB RID: 61435
		private bool anyEnemiesInitially;

		// Token: 0x0400EFFC RID: 61436
		private bool caravanAssaultSuccessfulTaleRecorded;

		// Token: 0x0400EFFD RID: 61437
		private bool allEnemiesDefeatedSignalSent;

		// Token: 0x0400EFFE RID: 61438
		public bool preventGravshipLanding;

		// Token: 0x0400EFFF RID: 61439
		private Material cachedMat;

		// Token: 0x0400F000 RID: 61440
		private static readonly IntVec3 DefaultMapSize = new IntVec3(200, 1, 200);

		// Token: 0x0400F001 RID: 61441
		private static List<string> tmpSitePartsLabels = new List<string>();
	}
}
