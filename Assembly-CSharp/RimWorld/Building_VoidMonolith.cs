using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A5A RID: 10842
	public class Building_VoidMonolith : Building, ITargetingSource, IThingGlower
	{
		// Token: 0x17002589 RID: 9609
		// (get) Token: 0x0600F0DE RID: 61662 RVA: 0x0045DA57 File Offset: 0x0045BC57
		public override int? OverrideGraphicIndex
		{
			get
			{
				return new int?(Find.Anomaly.LevelDef.graphicIndex);
			}
		}

		// Token: 0x1700258A RID: 9610
		// (get) Token: 0x0600F0DF RID: 61663 RVA: 0x0045DA6D File Offset: 0x0045BC6D
		public bool IsAutoActivating
		{
			get
			{
				return this.autoActivateTick > 0 && Find.TickManager.TicksGame > this.autoActivateTick - 300000;
			}
		}

		// Token: 0x1700258B RID: 9611
		// (get) Token: 0x0600F0E0 RID: 61664 RVA: 0x0045DA92 File Offset: 0x0045BC92
		public int TicksUntilAutoActivate
		{
			get
			{
				return this.autoActivateTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700258C RID: 9612
		// (get) Token: 0x0600F0E1 RID: 61665 RVA: 0x0043DE46 File Offset: 0x0043C046
		private TargetInfo EffecterInfo
		{
			get
			{
				return new TargetInfo(base.Position, base.Map, false);
			}
		}

		// Token: 0x1700258D RID: 9613
		// (get) Token: 0x0600F0E2 RID: 61666 RVA: 0x0045DAA8 File Offset: 0x0045BCA8
		public override CellRect? CustomRectForSelector
		{
			get
			{
				return new CellRect?(GenAdj.OccupiedRect(base.Position, Rot4.North, Find.Anomaly.LevelDef.sizeIncludingAttachments ?? this.def.Size));
			}
		}

		// Token: 0x1700258E RID: 9614
		// (get) Token: 0x0600F0E3 RID: 61667 RVA: 0x0045DAF7 File Offset: 0x0045BCF7
		public override Texture UIIconOverride
		{
			get
			{
				return Find.Anomaly.LevelDef.UIIcon;
			}
		}

		// Token: 0x1700258F RID: 9615
		// (get) Token: 0x0600F0E4 RID: 61668 RVA: 0x0045DB08 File Offset: 0x0045BD08
		public override string LabelNoCount
		{
			get
			{
				if (Find.Anomaly.LevelDef.monolithLabel != null)
				{
					return Find.Anomaly.LevelDef.monolithLabel;
				}
				return base.LabelNoCount;
			}
		}

		// Token: 0x17002590 RID: 9616
		// (get) Token: 0x0600F0E5 RID: 61669 RVA: 0x0045DB31 File Offset: 0x0045BD31
		public override string DescriptionFlavor
		{
			get
			{
				if (Find.Anomaly.LevelDef.monolithDescription != null)
				{
					return Find.Anomaly.LevelDef.monolithDescription;
				}
				return base.DescriptionFlavor;
			}
		}

		// Token: 0x0600F0E6 RID: 61670 RVA: 0x0045DB5A File Offset: 0x0045BD5A
		public bool ShouldBeLitNow()
		{
			return Find.Anomaly.LevelDef.monolithGlows;
		}

		// Token: 0x17002591 RID: 9617
		// (get) Token: 0x0600F0E7 RID: 61671 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002592 RID: 9618
		// (get) Token: 0x0600F0E8 RID: 61672 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002593 RID: 9619
		// (get) Token: 0x0600F0E9 RID: 61673 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002594 RID: 9620
		// (get) Token: 0x0600F0EA RID: 61674 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002595 RID: 9621
		// (get) Token: 0x0600F0EB RID: 61675 RVA: 0x00002501 File Offset: 0x00000701
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002596 RID: 9622
		// (get) Token: 0x0600F0EC RID: 61676 RVA: 0x000FD030 File Offset: 0x000FB230
		public Thing Caster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17002597 RID: 9623
		// (get) Token: 0x0600F0ED RID: 61677 RVA: 0x00002C42 File Offset: 0x00000E42
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002598 RID: 9624
		// (get) Token: 0x0600F0EE RID: 61678 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002599 RID: 9625
		// (get) Token: 0x0600F0EF RID: 61679 RVA: 0x0045DB6B File Offset: 0x0045BD6B
		public TargetingParameters targetParams
		{
			get
			{
				return Building_VoidMonolith.targetParmsInt;
			}
		}

		// Token: 0x1700259A RID: 9626
		// (get) Token: 0x0600F0F0 RID: 61680 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700259B RID: 9627
		// (get) Token: 0x0600F0F1 RID: 61681 RVA: 0x0045DB72 File Offset: 0x0045BD72
		public Texture2D UIIcon
		{
			get
			{
				if (Find.Anomaly.Level != 0)
				{
					return ContentFinder<Texture2D>.Get("UI/Commands/ActivateMonolith", true);
				}
				return ContentFinder<Texture2D>.Get("UI/Commands/ActivateMonolithInitial", true);
			}
		}

		// Token: 0x0600F0F2 RID: 61682 RVA: 0x0045DB98 File Offset: 0x0045BD98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<int>(ref this.disturbingVisionTick, "disturbingVisionTick", 0, false);
			Scribe_Values.Look<int>(ref this.autoActivateTick, "autoActivateTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.monolithAttachments, "monolithAttachments", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.activatedDialogTick, "activatedDialogTick", 0, false);
			Scribe_References.Look<Pawn>(ref this.activatorPawn, "activatorPawn", false);
			Scribe_References.Look<Thing>(ref this.pyramidThing, "pyramidThing", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.monolithAttachments == null)
			{
				this.monolithAttachments = new List<Thing>();
			}
		}

		// Token: 0x0600F0F3 RID: 61683 RVA: 0x0045DC48 File Offset: 0x0045BE48
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Void monolith"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!respawningAfterLoad)
			{
				if (Find.Anomaly.Level > 0)
				{
					this.TryGetComp<CompProximityLetter>().letterSent = true;
				}
				if (Find.Anomaly.monolith == null)
				{
					Find.Anomaly.monolith = this;
				}
				this.disturbingVisionTick = Find.TickManager.TicksGame + Mathf.RoundToInt(Building_VoidMonolith.DisturbingVisionRangeDaysRange.RandomInRange * 60000f);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.UpdateAttachments();
		}

		// Token: 0x0600F0F4 RID: 61684 RVA: 0x0045DCD4 File Offset: 0x0045BED4
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(30))
			{
				if (Find.Anomaly.Level == 0 && !base.Map.reservationManager.IsReserved(this))
				{
					if (this.level0HintEffecter == null)
					{
						this.level0HintEffecter = EffecterDefOf.MonolithL0Glow.Spawn(this.EffecterInfo, this.EffecterInfo, 1f);
					}
				}
				else
				{
					Effecter effecter = this.level0HintEffecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.level0HintEffecter = null;
				}
			}
			Effecter effecter2 = this.level0HintEffecter;
			if (effecter2 != null)
			{
				effecter2.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			if (Find.Anomaly.Level == 0 && this.disturbingVisionTick > 0 && Find.TickManager.TicksGame > this.disturbingVisionTick)
			{
				Pawn arg;
				if (base.Map.mapPawns.FreeColonistsSpawned.TryRandomElement(out arg))
				{
					Find.LetterStack.ReceiveLetter("VoidMonolithVisionLabel".Translate(), "VoidMonolithVisionText".Translate(arg.Named("PAWN")), LetterDefOf.NeutralEvent, this, null, null, null, null, 0, true);
					this.disturbingVisionTick = -99999;
				}
				else
				{
					this.disturbingVisionTick += 15000;
				}
			}
			if (this.autoActivateTick > 0)
			{
				if (Find.TickManager.TicksGame == this.autoActivateTick - 300000)
				{
					Find.LetterStack.ReceiveLetter("MonolithAutoActivatingLabel".Translate(), "MonolithAutoActivatingText".Translate(), LetterDefOf.NegativeEvent, this, null, null, null, null, 0, true);
				}
				if (Find.TickManager.TicksGame == this.autoActivateTick - 60)
				{
					Find.LetterStack.ReceiveLetter("MonolithAutoActivatedLabel".Translate(), "MonolithAutoActivatedText".Translate(), LetterDefOf.ThreatBig, this, null, null, null, null, 0, true);
				}
				Pawn pawn;
				if (Find.TickManager.TicksGame > this.autoActivateTick && base.Map.mapPawns.FreeColonists.TryRandomElement(out pawn))
				{
					this.Activate(pawn);
				}
			}
			if (this.IsAutoActivating)
			{
				if (this.autoActivateEffecter == null)
				{
					this.autoActivateEffecter = EffecterDefOf.MonolithAutoActivating.Spawn();
				}
				this.autoActivateEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			else if (this.autoActivateEffecter != null)
			{
				this.autoActivateEffecter.Cleanup();
				this.autoActivateEffecter = null;
			}
			if (Find.Anomaly.Level == MonolithLevelDefOf.Gleaming.level && Find.CurrentMap == base.MapHeld)
			{
				if (this.gleamingEffecter == null)
				{
					this.gleamingEffecter = EffecterDefOf.MonolithGleaming_Sustained.Spawn();
				}
				this.gleamingEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
				if (this.gleamingVoidNodeEffecter == null)
				{
					this.gleamingVoidNodeEffecter = EffecterDefOf.MonolithGleamingVoidNode.Spawn(this, base.Map, 1f);
				}
				this.gleamingVoidNodeEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			else if (this.gleamingEffecter != null || Find.CurrentMap != base.MapHeld)
			{
				Effecter effecter3 = this.gleamingEffecter;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.gleamingEffecter = null;
				Effecter effecter4 = this.gleamingVoidNodeEffecter;
				if (effecter4 != null)
				{
					effecter4.Cleanup();
				}
				this.gleamingVoidNodeEffecter = null;
			}
			if (this.activatedDialogTick > 0 && Find.TickManager.TicksGame > this.activatedDialogTick)
			{
				this.OpenActivatedDialog();
			}
		}

		// Token: 0x0600F0F5 RID: 61685 RVA: 0x0045E028 File Offset: 0x0045C228
		public bool CanActivate(out string reason, out string reasonShort)
		{
			reason = "";
			reasonShort = "";
			if (!Find.Anomaly.LevelDef.advanceThroughActivation)
			{
				return false;
			}
			MonolithLevelDef nextLevelDef = Find.Anomaly.NextLevelDef;
			if (nextLevelDef == null)
			{
				return false;
			}
			if (nextLevelDef.entityCatagoryCompletionRequired != null && Find.EntityCodex.DiscoveredCount(nextLevelDef.entityCatagoryCompletionRequired) < nextLevelDef.entityCountCompletionRequired)
			{
				int value = nextLevelDef.entityCountCompletionRequired - Find.EntityCodex.DiscoveredCount(nextLevelDef.entityCatagoryCompletionRequired);
				reason = string.Format("{0}:\n  - {1}", "VoidMonolithRequiresDiscovery".Translate(), "VoidMonolithRequiresCategory".Translate(value, nextLevelDef.entityCatagoryCompletionRequired.label));
				reasonShort = "VoidMonolithRequiresDiscoveryShort".Translate();
				return false;
			}
			foreach (GameCondition gameCondition in base.Map.GameConditionManager.ActiveConditions)
			{
				List<GameConditionDef> unreachableDuringConditions = nextLevelDef.unreachableDuringConditions;
				if (unreachableDuringConditions != null && unreachableDuringConditions.Contains(gameCondition.def))
				{
					reason = gameCondition.def.LabelCap;
					reasonShort = gameCondition.def.LabelCap;
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F0F6 RID: 61686 RVA: 0x0045E184 File Offset: 0x0045C384
		public void Activate(Pawn pawn)
		{
			this.CheckAndGenerateQuest();
			Find.Anomaly.IncrementLevel();
			EffecterDefOf.MonolithLevelChanged.Spawn().Trigger(this.EffecterInfo, this.EffecterInfo, -1);
			this.activatedDialogTick = Find.TickManager.TicksGame + 360;
			this.activatorPawn = pawn;
			if (Find.Anomaly.Level == 1)
			{
				CompStudiable comp = base.GetComp<CompStudiable>();
				if (comp != null)
				{
					comp.SetStudyEnabled(true);
				}
			}
			this.autoActivateTick = -99999;
		}

		// Token: 0x0600F0F7 RID: 61687 RVA: 0x0045E208 File Offset: 0x0045C408
		public void CheckAndGenerateQuest()
		{
			if (Find.Anomaly.monolith == null)
			{
				Find.Anomaly.monolith = this;
			}
			if (this.quest == null && !Find.Anomaly.LevelDef.postEndgame && Find.Anomaly.GenerateMonolith)
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", base.Map, false);
				slate.Set<Building_VoidMonolith>("monolith", this, false);
				this.quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.EndGame_VoidMonolith, slate);
				Find.SignalManager.SendSignal(new Signal("MonolithLevelChanged", true));
			}
		}

		// Token: 0x0600F0F8 RID: 61688 RVA: 0x0045E29C File Offset: 0x0045C49C
		private void OpenActivatedDialog()
		{
			DiaNode diaNode = new DiaNode(Find.Anomaly.LevelDef.activatedLetterText.Formatted(this.activatorPawn.Named("PAWN")));
			diaNode.options.Add(new DiaOption("VoidMonolithViewQuest".Translate())
			{
				action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				},
				resolveTree = true
			});
			if (Find.Anomaly.LevelDef != MonolithLevelDefOf.VoidAwakened)
			{
				List<DiaOption> options = diaNode.options;
				DiaOption diaOption = new DiaOption("VoidMonolithViewResearch".Translate());
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Anomaly;
				};
				diaOption.resolveTree = true;
				options.Add(diaOption);
			}
			if (Find.Anomaly.Level == 1)
			{
				List<DiaOption> options2 = diaNode.options;
				DiaOption diaOption2 = new DiaOption("ViewEntityCodex".Translate());
				diaOption2.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_EntityCodex(null));
				};
				diaOption2.resolveTree = true;
				options2.Add(diaOption2);
			}
			diaNode.options.Add(new DiaOption("Close".Translate())
			{
				resolveTree = true
			});
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
			this.activatedDialogTick = -99999;
			this.activatorPawn = null;
		}

		// Token: 0x0600F0F9 RID: 61689 RVA: 0x0045E414 File Offset: 0x0045C614
		public void Investigate(Pawn pawn)
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("VoidMonolithInvestigatedText".Translate(pawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("VoidMonolithInvestigate".Translate())
					{
						action = delegate()
						{
							if (pawn != null)
							{
								Job job = JobMaker.MakeJob(JobDefOf.ActivateMonolith, this);
								pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}
						},
						resolveTree = true
					},
					new DiaOption("VoidMonolithWalkAway".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x0600F0FA RID: 61690 RVA: 0x0045E4D0 File Offset: 0x0045C6D0
		public void SetLevel(MonolithLevelDef levelDef)
		{
			CompGlower comp = base.GetComp<CompGlower>();
			int monolithGlowRadiusOverride = levelDef.monolithGlowRadiusOverride;
			if (monolithGlowRadiusOverride != -1)
			{
				comp.GlowRadius = (float)monolithGlowRadiusOverride;
			}
			comp.UpdateLit(base.Map);
			if (comp.Glows)
			{
				comp.ForceRegister(base.Map);
			}
			if (base.Spawned)
			{
				this.UpdateAttachments();
				base.DirtyMapMesh(base.Map);
			}
			if (Find.Anomaly.LevelDef.activatedSound != null)
			{
				Find.Anomaly.LevelDef.activatedSound.PlayOneShot(this);
			}
			if (levelDef.level == MonolithLevelDefOf.Gleaming.level)
			{
				EffecterDefOf.MonolithGleaming_Transition.SpawnMaintained(base.PositionHeld, base.Map, 1f);
			}
		}

		// Token: 0x0600F0FB RID: 61691 RVA: 0x0045E58C File Offset: 0x0045C78C
		private void UpdateAttachments()
		{
			TerrainDef newTerr = base.Map.Biome.TerrainForAffordance(this.def.terrainAffordanceNeeded);
			Thing.allowDestroyNonDestroyable = true;
			foreach (Thing thing in this.monolithAttachments)
			{
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			Thing.allowDestroyNonDestroyable = false;
			this.monolithAttachments.Clear();
			if (Find.Anomaly.LevelDef.attachments == null)
			{
				return;
			}
			foreach (MonolithAttachment monolithAttachment in Find.Anomaly.LevelDef.attachments)
			{
				IntVec3 intVec = base.Position + monolithAttachment.offset.ToIntVec3;
				foreach (IntVec3 c in GenAdj.OccupiedRect(intVec, Rot4.North, monolithAttachment.def.Size))
				{
					if (!c.GetAffordances(base.Map).Contains(this.def.terrainAffordanceNeeded))
					{
						base.Map.terrainGrid.RemoveTopLayer(c, false);
						base.Map.terrainGrid.SetTerrain(c, newTerr);
					}
				}
				Thing thing2 = ThingMaker.MakeThing(monolithAttachment.def, null);
				thing2.TryGetComp<CompSelectProxy>().thingToSelect = this;
				GenSpawn.Spawn(thing2, intVec, base.Map, Rot4.North, WipeMode.FullRefund, false, true);
				thing2.overrideGraphicIndex = new int?(monolithAttachment.graphicIndex);
				thing2.DirtyMapMesh(base.Map);
				this.monolithAttachments.Add(thing2);
			}
		}

		// Token: 0x0600F0FC RID: 61692 RVA: 0x0045E7B0 File Offset: 0x0045C9B0
		public void Collapse()
		{
			Map map = Find.Anomaly.monolith.Map;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IntVec3 position = Find.Anomaly.monolith.Position;
				Map map3 = map;
				int squareRadius = 5;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc;
				if (CellFinder.TryFindRandomCellNear(position, map3, squareRadius, validator, out loc, -1))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MonolithFragment, null), loc, Find.Anomaly.monolith.Map, WipeMode.Vanish);
				}
			}
			int j = 10;
			Predicate<IntVec3> <>9__1;
			while (j > 0)
			{
				IntVec3 position2 = Find.Anomaly.monolith.Position;
				Map map2 = map;
				int squareRadius2 = 5;
				Predicate<IntVec3> validator2;
				if ((validator2 = <>9__1) == null)
				{
					validator2 = (<>9__1 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc2;
				if (CellFinder.TryFindRandomCellNear(position2, map2, squareRadius2, validator2, out loc2, -1))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Shard, null);
					thing.stackCount = Mathf.Min(Rand.RangeInclusive(1, 2), j);
					j -= thing.stackCount;
					GenSpawn.Spawn(thing, loc2, Find.Anomaly.monolith.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600F0FD RID: 61693 RVA: 0x0045E8DC File Offset: 0x0045CADC
		public void Reset()
		{
			this.quest = null;
			base.GetComp<CompVoidStructure>().Reset();
			this.SetLevel(Find.Anomaly.LevelDef);
		}

		// Token: 0x0600F0FE RID: 61694 RVA: 0x0045E900 File Offset: 0x0045CB00
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x0600F0FF RID: 61695 RVA: 0x0045E90C File Offset: 0x0045CB0C
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			if (target.Pawn.Downed)
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorDowned".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (target.Pawn.InMentalState)
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorMentalState".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.CanCasuallyInteractNow(false, false, false, true))
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorBusy".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				if (showMessages)
				{
					Messages.Message("NoPath".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600F100 RID: 61696 RVA: 0x0045EA21 File Offset: 0x0045CC21
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600F101 RID: 61697 RVA: 0x0045EA34 File Offset: 0x0045CC34
		public void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				Pawn pawn = target.Pawn;
				if (Find.Anomaly.Level == 0)
				{
					Job job = JobMaker.MakeJob(JobDefOf.InvestigateMonolith, this);
					pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					Messages.Message(Find.Anomaly.LevelDef.pawnSentToActivateMessage.Formatted(pawn.Named("PAWN")), this, MessageTypeDefOf.NeutralEvent, true);
					this.TryGetComp<CompProximityLetter>().letterSent = true;
					return;
				}
				this.OrderActivation(pawn, true);
			}
		}

		// Token: 0x0600F102 RID: 61698 RVA: 0x0045EAD4 File Offset: 0x0045CCD4
		private void OrderActivation(Pawn pawn, bool sendMessage)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ActivateMonolith, this);
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			if (sendMessage)
			{
				Messages.Message(Find.Anomaly.LevelDef.pawnSentToActivateMessage.Formatted(pawn.Named("PAWN")), this, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600F103 RID: 61699 RVA: 0x0045EB3E File Offset: 0x0045CD3E
		public void AutoActivate(int tick)
		{
			this.autoActivateTick = tick;
			this.TryGetComp<CompProximityLetter>().letterSent = true;
			this.disturbingVisionTick = -99999;
		}

		// Token: 0x0600F104 RID: 61700 RVA: 0x0045EB60 File Offset: 0x0045CD60
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("VoidMonolithChooseActivator".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false) && this.targetParams.CanTarget(target.Pawn, this))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600F105 RID: 61701 RVA: 0x0045EBCC File Offset: 0x0045CDCC
		public override string GetInspectString()
		{
			string inspectString = base.GetInspectString();
			StringBuilder stringBuilder = new StringBuilder();
			if (!Find.Anomaly.LevelDef.levelInspectText.NullOrEmpty())
			{
				stringBuilder.Append(Find.Anomaly.LevelDef.levelInspectText);
			}
			if (Find.Anomaly.Level > 0)
			{
				if (Find.Anomaly.LevelDef.advanceThroughActivation)
				{
					string text;
					string text2;
					if (this.CanActivate(out text, out text2))
					{
						stringBuilder.AppendLineIfNotEmpty().Append(Find.Anomaly.LevelDef.monolithCanBeActivatedText);
					}
					else if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLineIfNotEmpty().Append(text);
					}
				}
			}
			else
			{
				stringBuilder.AppendLineIfNotEmpty().Append("VoidMonolithUndiscovered".Translate());
			}
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLineIfNotEmpty().Append(inspectString);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F106 RID: 61702 RVA: 0x0045ECA6 File Offset: 0x0045CEA6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (Find.Anomaly.LevelDef.advanceThroughActivation)
			{
				string text = null;
				string text2;
				string text3;
				if (!this.CanActivate(out text2, out text3))
				{
					text = text2;
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = Find.Anomaly.LevelDef.activateGizmoText.CapitalizeFirst() + "...",
					defaultDesc = Find.Anomaly.LevelDef.activateGizmoDescription,
					icon = this.UIIcon,
					Disabled = !text.NullOrEmpty(),
					disabledReason = text,
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						Find.Targeter.BeginTargeting(this, null, false, null, null, true);
					}
				};
				yield return command_Action;
				if (DebugSettings.ShowDevGizmos)
				{
					if (base.Map.mapPawns.AnyFreeColonistSpawned)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Activate",
							action = delegate()
							{
								Pawn pawn;
								if (base.Map.mapPawns.FreeColonists.TryRandomElement(out pawn))
								{
									this.Activate(pawn);
								}
							}
						};
					}
					if (!Find.Anomaly.MonolithSpawned)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Relink Monolith",
							action = delegate()
							{
								Find.Anomaly.monolith = this;
							}
						};
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F107 RID: 61703 RVA: 0x0045ECB6 File Offset: 0x0045CEB6
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (Find.Anomaly.Level == 0)
			{
				TaggedString value = Find.Anomaly.LevelDef.activateFloatMenuText.Formatted(this.Label);
				if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CannotGenericWorkCustom".Translate(value).CapitalizeFirst() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return new FloatMenuOption(value.CapitalizeFirst(), delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.InvestigateMonolith, this);
						selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						this.TryGetComp<CompProximityLetter>().letterSent = true;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
					{
						tutorTag = "Investigate-" + this.def.defName
					};
				}
				yield break;
			}
			string text;
			string t;
			if (this.CanActivate(out text, out t))
			{
				if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CantActivateMonolith".Translate(Find.Anomaly.LevelDef.activateGizmoText) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return new FloatMenuOption(Find.Anomaly.LevelDef.activateFloatMenuText.Formatted(this.Label).CapitalizeFirst(), delegate()
					{
						this.OrderActivation(selPawn, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			else if (Find.Anomaly.LevelDef.advanceThroughActivation)
			{
				yield return new FloatMenuOption("CantActivateMonolith".Translate(Find.Anomaly.LevelDef.activateGizmoText) + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x04009EE9 RID: 40681
		private static readonly FloatRange DisturbingVisionRangeDaysRange = new FloatRange(13f, 16f);

		// Token: 0x04009EEA RID: 40682
		private const int DisturbingVisionRetryTicks = 15000;

		// Token: 0x04009EEB RID: 40683
		public const int ActivateLetterDelayTicks = 360;

		// Token: 0x04009EEC RID: 40684
		private const int AutoActivateAlertTicks = 300000;

		// Token: 0x04009EED RID: 40685
		private const int MonolithFragmentCount = 3;

		// Token: 0x04009EEE RID: 40686
		private const int MonolithShardCount = 10;

		// Token: 0x04009EEF RID: 40687
		private const int CollapseScatterRadius = 5;

		// Token: 0x04009EF0 RID: 40688
		private const int HintEffecterInterval = 30;

		// Token: 0x04009EF1 RID: 40689
		public Quest quest;

		// Token: 0x04009EF2 RID: 40690
		private int disturbingVisionTick = -99999;

		// Token: 0x04009EF3 RID: 40691
		private int autoActivateTick = -99999;

		// Token: 0x04009EF4 RID: 40692
		private List<Thing> monolithAttachments = new List<Thing>();

		// Token: 0x04009EF5 RID: 40693
		private Thing pyramidThing;

		// Token: 0x04009EF6 RID: 40694
		private int activatedDialogTick = -99999;

		// Token: 0x04009EF7 RID: 40695
		private Pawn activatorPawn;

		// Token: 0x04009EF8 RID: 40696
		private Effecter gleamingEffecter;

		// Token: 0x04009EF9 RID: 40697
		private Effecter gleamingVoidNodeEffecter;

		// Token: 0x04009EFA RID: 40698
		private Effecter autoActivateEffecter;

		// Token: 0x04009EFB RID: 40699
		private Effecter level0HintEffecter;

		// Token: 0x04009EFC RID: 40700
		private static readonly TargetingParameters targetParmsInt = new TargetingParameters
		{
			canTargetBuildings = false,
			canTargetAnimals = false,
			canTargetMechs = false,
			onlyTargetColonists = true
		};
	}
}
