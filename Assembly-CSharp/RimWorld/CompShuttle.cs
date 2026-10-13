using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002E14 RID: 11796
	[StaticConstructorOnStartup]
	public class CompShuttle : ThingComp
	{
		// Token: 0x17002A80 RID: 10880
		// (get) Token: 0x060109AA RID: 68010 RVA: 0x004C93CC File Offset: 0x004C75CC
		public bool IsPlayerShuttle
		{
			get
			{
				if (ModLister.OdysseyInstalled)
				{
					TransportShipDef shipDef = this.Props.shipDef;
					return shipDef != null && shipDef.playerShuttle;
				}
				return false;
			}
		}

		// Token: 0x17002A81 RID: 10881
		// (get) Token: 0x060109AB RID: 68011 RVA: 0x004C93ED File Offset: 0x004C75ED
		public bool Autoload
		{
			get
			{
				return this.autoload;
			}
		}

		// Token: 0x17002A82 RID: 10882
		// (get) Token: 0x060109AC RID: 68012 RVA: 0x004C93F5 File Offset: 0x004C75F5
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x17002A83 RID: 10883
		// (get) Token: 0x060109AD RID: 68013 RVA: 0x004C940D File Offset: 0x004C760D
		public CompProperties_Shuttle Props
		{
			get
			{
				return (CompProperties_Shuttle)this.props;
			}
		}

		// Token: 0x17002A84 RID: 10884
		// (get) Token: 0x060109AE RID: 68014 RVA: 0x004C941C File Offset: 0x004C761C
		public bool ShowLoadingGizmos
		{
			get
			{
				return this.IsPlayerShuttle || ((this.shipParent == null || this.shipParent.ShowGizmos) && (this.parent.Faction == null || this.parent.Faction == Faction.OfPlayer));
			}
		}

		// Token: 0x17002A85 RID: 10885
		// (get) Token: 0x060109AF RID: 68015 RVA: 0x004C946C File Offset: 0x004C766C
		public CompTransporter Transporter
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedCompTransporter) == null)
				{
					result = (this.cachedCompTransporter = this.parent.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x17002A86 RID: 10886
		// (get) Token: 0x060109B0 RID: 68016 RVA: 0x004C9498 File Offset: 0x004C7698
		private bool Autoloadable
		{
			get
			{
				if (this.IsPlayerShuttle)
				{
					return false;
				}
				if (this.cachedTransporterList == null)
				{
					this.cachedTransporterList = new List<CompTransporter>
					{
						this.Transporter
					};
				}
				foreach (Pawn thing in TransporterUtility.AllSendablePawns(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing))
					{
						return false;
					}
				}
				foreach (Thing thing2 in TransporterUtility.AllSendableItems(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing2))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17002A87 RID: 10887
		// (get) Token: 0x060109B1 RID: 68017 RVA: 0x004C957C File Offset: 0x004C777C
		private int ContainedColonistCount
		{
			get
			{
				int num = 0;
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsFreeColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002A88 RID: 10888
		// (get) Token: 0x060109B2 RID: 68018 RVA: 0x004C95C5 File Offset: 0x004C77C5
		private IEnumerable<Pawn> ContainedPawns
		{
			get
			{
				ThingOwner things = this.Transporter.innerContainer;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Pawn pawn = things[i] as Pawn;
					if (pawn != null)
					{
						yield return pawn;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17002A89 RID: 10889
		// (get) Token: 0x060109B3 RID: 68019 RVA: 0x004C95D8 File Offset: 0x004C77D8
		public bool AllRequiredThingsLoaded
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				foreach (Pawn item in this.RequiredPawns)
				{
					if (!innerContainer.Contains(item))
					{
						return false;
					}
				}
				if (this.requireAllColonistsOnMap || this.requiredColonistCount > 0)
				{
					int containedColonistCount = this.ContainedColonistCount;
					if (this.requireAllColonistsOnMap && containedColonistCount < this.parent.Map.mapPawns.FreeColonistsCount)
					{
						return false;
					}
					if (this.requiredColonistCount > 0 && containedColonistCount < this.requiredColonistCount)
					{
						return false;
					}
				}
				CompShuttle.tmpRequiredItemsWithoutDuplicates.Clear();
				for (int i = 0; i < this.requiredItems.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; j++)
					{
						if (CompShuttle.tmpRequiredItemsWithoutDuplicates[j].ThingDef == this.requiredItems[i].ThingDef)
						{
							CompShuttle.tmpRequiredItemsWithoutDuplicates[j] = CompShuttle.tmpRequiredItemsWithoutDuplicates[j].WithCount(CompShuttle.tmpRequiredItemsWithoutDuplicates[j].Count + this.requiredItems[i].Count);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						CompShuttle.tmpRequiredItemsWithoutDuplicates.Add(this.requiredItems[i]);
					}
				}
				for (int k = 0; k < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; k++)
				{
					int num = 0;
					for (int l = 0; l < innerContainer.Count; l++)
					{
						if (innerContainer[l].def == CompShuttle.tmpRequiredItemsWithoutDuplicates[k].ThingDef)
						{
							num += innerContainer[l].stackCount;
						}
					}
					if (num < CompShuttle.tmpRequiredItemsWithoutDuplicates[k].Count)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17002A8A RID: 10890
		// (get) Token: 0x060109B4 RID: 68020 RVA: 0x004C97F8 File Offset: 0x004C79F8
		public IEnumerable<Pawn> RequiredPawns
		{
			get
			{
				foreach (Pawn pawn in this.requiredPawns)
				{
					if (!pawn.Dead && ((!pawn.Downed && pawn.MapHeld == this.parent.MapHeld) || !this.pawnsToIgnoreIfDownedOfNotOnTheMap.Contains(pawn)))
					{
						yield return pawn;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17002A8B RID: 10891
		// (get) Token: 0x060109B5 RID: 68021 RVA: 0x004C9808 File Offset: 0x004C7A08
		public TaggedString RequiredThingsLabel
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Pawn pawn in this.RequiredPawns)
				{
					stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				}
				for (int i = 0; i < this.requiredItems.Count; i++)
				{
					stringBuilder.AppendLine("  - " + this.requiredItems[i].LabelCap);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x17002A8C RID: 10892
		// (get) Token: 0x060109B6 RID: 68022 RVA: 0x004C98C4 File Offset: 0x004C7AC4
		public bool HasPilot
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsFreeColonist && !StatDefOf.PilotingAbility.Worker.IsDisabledFor(pawn) && pawn.GetStatValue(StatDefOf.PilotingAbility, true, -1) > 0.1f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002A8D RID: 10893
		// (get) Token: 0x060109B7 RID: 68023 RVA: 0x004C9930 File Offset: 0x004C7B30
		public AcceptanceReport CanLaunch
		{
			get
			{
				TransportShipDef shipDef = this.Props.shipDef;
				if (shipDef != null && !shipDef.playerShuttle)
				{
					if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
					{
						return "CommandLaunchGroupFailNotLoaded".Translate();
					}
					return true;
				}
				else
				{
					if (this.parent.Spawned && !this.HasPilot)
					{
						return "CommandShuttleNoPilot".Translate();
					}
					if (this.parent.Spawned)
					{
						WeatherDef curWeatherPerceived = this.parent.Map.weatherManager.CurWeatherPerceived;
						if (curWeatherPerceived.preventsShuttleLaunch)
						{
							return curWeatherPerceived.LabelCap;
						}
						foreach (GameCondition gameCondition in this.parent.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition.def.preventShuttleLaunch)
							{
								return gameCondition.LabelCap;
							}
						}
					}
					return true;
				}
			}
		}

		// Token: 0x060109B8 RID: 68024 RVA: 0x004C9A50 File Offset: 0x004C7C50
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnyExpansion("Shuttle"))
			{
				return;
			}
			if (!respawningAfterLoad && this.Props.shipDef != null)
			{
				TransportShipDef shipDef = this.Props.shipDef;
				if (shipDef != null && shipDef.playerShuttle)
				{
					this.acceptColonists = true;
					this.acceptChildren = true;
					this.acceptColonyPrisoners = true;
					this.allowSlaves = true;
				}
				if (this.shipParent == null)
				{
					this.shipParent = TransportShipMaker.MakeTransportShip(this.Props.shipDef, null, this.parent);
				}
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x060109B9 RID: 68025 RVA: 0x004C9ADC File Offset: 0x004C7CDC
		public void SetPawnToLeaveBehind(Func<Pawn, bool> predicate)
		{
			foreach (Pawn pawn in this.requiredPawns)
			{
				if (predicate(pawn))
				{
					this.pawnsToIgnoreIfDownedOfNotOnTheMap.AddUnique(pawn);
				}
			}
		}

		// Token: 0x060109BA RID: 68026 RVA: 0x004C9B40 File Offset: 0x004C7D40
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.shipParent != null && this.shipParent.ShowGizmos)
			{
				IEnumerable<Gizmo> jobGizmos = this.shipParent.curJob.GetJobGizmos();
				if (jobGizmos != null)
				{
					foreach (Gizmo gizmo2 in jobGizmos)
					{
						yield return gizmo2;
					}
					enumerator = null;
				}
			}
			if (this.ShowLoadingGizmos && this.Autoloadable)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoloadTransporters".Translate(),
					defaultDesc = "CommandAutoloadTransportersDesc".Translate(),
					icon = CompShuttle.AutoloadToggleTex,
					isActive = (() => this.autoload),
					toggleAction = delegate()
					{
						this.autoload = !this.autoload;
						if (this.autoload && !this.Transporter.LoadingInProgressOrReadyToLaunch)
						{
							TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
						}
						this.CheckAutoload();
					}
				};
			}
			foreach (Gizmo gizmo3 in QuestUtility.GetQuestRelatedGizmos(this.parent))
			{
				yield return gizmo3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060109BB RID: 68027 RVA: 0x004C9B50 File Offset: 0x004C7D50
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!selPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield break;
			}
			string text = "EnterShuttle".Translate();
			if (!this.IsAllowedNow(selPawn))
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
				selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x060109BC RID: 68028 RVA: 0x004C9B67 File Offset: 0x004C7D67
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			CompShuttle.tmpAllowedPawns.Clear();
			string text = "EnterShuttle".Translate();
			foreach (Pawn pawn in selPawns)
			{
				if (pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompShuttle.tmpAllowedPawns.Add(pawn);
				}
			}
			if (!CompShuttle.tmpAllowedPawns.Any<Pawn>())
			{
				yield return new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			for (int i = CompShuttle.tmpAllowedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.IsAllowedNow(CompShuttle.tmpAllowedPawns[i]))
				{
					CompShuttle.tmpAllowedPawns.RemoveAt(i);
				}
			}
			if (!CompShuttle.tmpAllowedPawns.Any<Pawn>())
			{
				yield return new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
				}
				for (int j = 0; j < CompShuttle.tmpAllowedPawns.Count; j++)
				{
					Pawn pawn2 = CompShuttle.tmpAllowedPawns[j];
					if (pawn2.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn2.Downed && !pawn2.Dead && pawn2.Spawned)
					{
						Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, this.parent);
						CompShuttle.tmpAllowedPawns[j].jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x060109BD RID: 68029 RVA: 0x004C9B80 File Offset: 0x004C7D80
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(120))
			{
				this.CheckAutoload();
			}
			if (this.parent.IsHashIntervalTick(600))
			{
				foreach (Pawn pawn in this.ContainedPawns)
				{
					if (pawn.TickDeSpawned > 0 && Find.TickManager.TicksGame - pawn.TickDeSpawned > 60000)
					{
						ShipJob_Unload.UnloadThingFromShuttle(this.shipParent, pawn, null, false);
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
						}
					}
				}
			}
			TransportShipDef shipDef = this.Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle && !this.Transporter.LoadingInProgressOrReadyToLaunch && this.ContainedColonistCount > 0)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
			}
		}

		// Token: 0x060109BE RID: 68030 RVA: 0x004C9C88 File Offset: 0x004C7E88
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.shipParent != null && this.shipParent.curJob != null)
			{
				string text = this.shipParent.curJob.ShipThingExtraInspectString();
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text);
				}
			}
			CompShuttle.tmpRequiredLabels.Clear();
			if (this.requireAllColonistsOnMap)
			{
				int freeColonistsCount = this.parent.Map.mapPawns.FreeColonistsCount;
				CompShuttle.tmpRequiredLabels.Add(freeColonistsCount.ToString() + " " + ((freeColonistsCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			else if (this.requiredColonistCount > 0)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredColonistCount.ToString() + " " + ((this.requiredColonistCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			foreach (Pawn pawn in this.RequiredPawns)
			{
				if (!this.Transporter.innerContainer.Contains(pawn))
				{
					CompShuttle.tmpRequiredLabels.Add(pawn.LabelShort);
				}
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.Transporter.innerContainer.TotalStackCountOfDef(this.requiredItems[i].ThingDef) < this.requiredItems[i].Count)
				{
					CompShuttle.tmpRequiredLabels.Add(this.requiredItems[i].Label);
				}
			}
			if (CompShuttle.tmpRequiredLabels.Any<string>())
			{
				stringBuilder.AppendInNewLine("Required".Translate() + ": " + CompShuttle.tmpRequiredLabels.ToCommaList(false, false).CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060109BF RID: 68031 RVA: 0x004C9EAC File Offset: 0x004C80AC
		public void SendLaunchedSignals()
		{
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				for (int j = 0; j < transportersInGroup[i].innerContainer.Count; j++)
				{
					Pawn pawn = transportersInGroup[i].innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsColonist && !this.requiredPawns.Contains(pawn))
					{
						list.Add(pawn);
					}
				}
			}
			if (list.Count != 0)
			{
				for (int k = 0; k < transportersInGroup.Count; k++)
				{
					QuestUtility.SendQuestTargetSignals(transportersInGroup[k].parent.questTags, "SentWithExtraColonists", transportersInGroup[k].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
				}
			}
			string signalPart = this.AllRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied";
			for (int l = 0; l < transportersInGroup.Count; l++)
			{
				QuestUtility.SendQuestTargetSignals(transportersInGroup[l].parent.questTags, signalPart, transportersInGroup[l].parent.Named("SUBJECT"), transportersInGroup[l].innerContainer.ToList<Thing>().Named("SENT"));
			}
		}

		// Token: 0x060109C0 RID: 68032 RVA: 0x004CA008 File Offset: 0x004C8208
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDefCount>(ref this.requiredItems, "requiredItems", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsToIgnoreIfDownedOfNotOnTheMap, "pawnsToIgnoreIfDownedOfNotOnTheMap", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<bool>(ref this.acceptColonists, "acceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.acceptChildren, "acceptChildren", true, false);
			Scribe_Values.Look<bool>(ref this.onlyAcceptColonists, "onlyAcceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaves, "allowSlaves", false, false);
			Scribe_Values.Look<bool>(ref this.autoload, "autoload", false, false);
			Scribe_Values.Look<bool>(ref this.permitShuttle, "permitShuttle", false, false);
			Scribe_Values.Look<bool>(ref this.acceptColonyPrisoners, "acceptColonyPrisoners", false, false);
			Scribe_Values.Look<int>(ref this.maxColonistCount, "maxColonistCount", -1, false);
			Scribe_References.Look<TransportShip>(ref this.shipParent, "shipParent", false);
			Scribe_Values.Look<float>(ref this.minAge, "minAge", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.requiredPawns.RemoveAll((Pawn x) => x == null);
			}
			if (this.maxColonistCount == 0)
			{
				this.maxColonistCount = -1;
			}
		}

		// Token: 0x060109C1 RID: 68033 RVA: 0x004CA17C File Offset: 0x004C837C
		private void CheckAutoload()
		{
			if (!this.autoload || !this.Transporter.LoadingInProgressOrReadyToLaunch || !this.parent.Spawned)
			{
				return;
			}
			CompShuttle.tmpRequiredItems.Clear();
			CompShuttle.tmpRequiredItems.AddRange(this.requiredItems);
			CompShuttle.tmpRequiredPawns.Clear();
			CompShuttle.tmpRequiredPawns.AddRange(this.requiredPawns);
			ThingOwner innerContainer = this.Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null)
				{
					CompShuttle.tmpRequiredPawns.Remove(pawn);
				}
				else
				{
					int num = innerContainer[i].stackCount;
					for (int j = 0; j < CompShuttle.tmpRequiredItems.Count; j++)
					{
						if (CompShuttle.tmpRequiredItems[j].ThingDef == innerContainer[i].def)
						{
							int num2 = Mathf.Min(CompShuttle.tmpRequiredItems[j].Count, num);
							if (num2 > 0)
							{
								CompShuttle.tmpRequiredItems[j] = CompShuttle.tmpRequiredItems[j].WithCount(CompShuttle.tmpRequiredItems[j].Count - num2);
								num -= num2;
							}
						}
					}
				}
			}
			for (int k = CompShuttle.tmpRequiredItems.Count - 1; k >= 0; k--)
			{
				if (CompShuttle.tmpRequiredItems[k].Count <= 0)
				{
					CompShuttle.tmpRequiredItems.RemoveAt(k);
				}
			}
			if (CompShuttle.tmpRequiredItems.Any<ThingDefCount>() || CompShuttle.tmpRequiredPawns.Any<Pawn>())
			{
				if (this.Transporter.leftToLoad != null)
				{
					this.Transporter.leftToLoad.Clear();
				}
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.ThingsBeingHauledTo(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				for (int l = 0; l < CompShuttle.tmpRequiredPawns.Count; l++)
				{
					if (CompShuttle.tmpAllSendablePawns.Contains(CompShuttle.tmpRequiredPawns[l]))
					{
						TransferableOneWay transferableOneWay = new TransferableOneWay();
						transferableOneWay.things.Add(CompShuttle.tmpRequiredPawns[l]);
						this.Transporter.AddToTheToLoadList(transferableOneWay, 1);
						CompShuttle.tmpRequiredPawnsPossibleToSend.Add(CompShuttle.tmpRequiredPawns[l]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItems.Count; m++)
				{
					if (CompShuttle.tmpRequiredItems[m].Count > 0)
					{
						int num3 = 0;
						for (int n = 0; n < CompShuttle.tmpAllSendableItems.Count; n++)
						{
							if (CompShuttle.tmpAllSendableItems[n].def == CompShuttle.tmpRequiredItems[m].ThingDef)
							{
								num3 += CompShuttle.tmpAllSendableItems[n].stackCount;
							}
						}
						if (num3 > 0)
						{
							TransferableOneWay transferableOneWay2 = new TransferableOneWay();
							for (int num4 = 0; num4 < CompShuttle.tmpAllSendableItems.Count; num4++)
							{
								if (CompShuttle.tmpAllSendableItems[num4].def == CompShuttle.tmpRequiredItems[m].ThingDef)
								{
									transferableOneWay2.things.Add(CompShuttle.tmpAllSendableItems[num4]);
								}
							}
							int count = Mathf.Min(CompShuttle.tmpRequiredItems[m].Count, num3);
							this.Transporter.AddToTheToLoadList(transferableOneWay2, count);
						}
					}
				}
				TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpRequiredItems.Clear();
				CompShuttle.tmpRequiredPawns.Clear();
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				return;
			}
			if (this.Transporter.leftToLoad != null)
			{
				this.Transporter.leftToLoad.Clear();
			}
			TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
		}

		// Token: 0x060109C2 RID: 68034 RVA: 0x004CA5EC File Offset: 0x004C87EC
		public virtual bool IsRequired(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return this.requiredPawns.Contains(pawn);
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.requiredItems[i].ThingDef == thing.def && this.requiredItems[i].Count != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060109C3 RID: 68035 RVA: 0x004CA65C File Offset: 0x004C885C
		public virtual bool IsAllowed(Thing t)
		{
			TransportShipDef shipDef = this.Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle)
			{
				return true;
			}
			if (this.IsRequired(t))
			{
				return true;
			}
			if (this.acceptColonists)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && (pawn.IsColonist || (pawn.IsPrisonerOfColony && this.acceptColonyPrisoners) || (!this.onlyAcceptColonists && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer)) && (!pawn.IsSlave || this.allowSlaves) && (!this.onlyAcceptColonists || !pawn.IsQuestLodger()) && (!this.onlyAcceptHealthy || this.PawnIsHealthyEnoughForShuttle(pawn)) && pawn.ageTracker.AgeBiologicalYearsFloat >= this.minAge)
				{
					return true;
				}
			}
			if (!this.acceptChildren)
			{
				Pawn pawn2 = t as Pawn;
				if (pawn2 != null && pawn2.RaceProps.Humanlike && !pawn2.DevelopmentalStage.Adult())
				{
					return false;
				}
			}
			if (this.permitShuttle)
			{
				Pawn pawn3 = t as Pawn;
				if (pawn3 == null)
				{
					return true;
				}
				if ((pawn3.Faction == Faction.OfPlayer && !pawn3.IsQuestLodger()) || pawn3.IsPrisonerOfColony)
				{
					return true;
				}
			}
			return !(t is Pawn) && !this.parent.Map.IsPlayerHome && !GenHostility.AnyHostileActiveThreatToPlayer(this.parent.Map, true, false);
		}

		// Token: 0x060109C4 RID: 68036 RVA: 0x004CA7B4 File Offset: 0x004C89B4
		public virtual bool IsAllowedNow(Thing t)
		{
			if (!this.IsAllowed(t))
			{
				return false;
			}
			if (this.maxColonistCount == -1)
			{
				return true;
			}
			int num = 0;
			foreach (Thing thing in ((IEnumerable<Thing>)this.Transporter.innerContainer))
			{
				if (thing != t)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						num++;
					}
				}
			}
			foreach (Pawn pawn2 in this.parent.Map.mapPawns.AllPawns)
			{
				if (pawn2.jobs != null && pawn2.jobs.curDriver != null && pawn2 != t)
				{
					foreach (QueuedJob queuedJob in pawn2.jobs.jobQueue)
					{
						if (this.<IsAllowedNow>g__CheckJob|71_0(queuedJob.job))
						{
							num++;
						}
					}
					if (this.<IsAllowedNow>g__CheckJob|71_0(pawn2.jobs.curJob))
					{
						num++;
					}
				}
			}
			return num < this.maxColonistCount;
		}

		// Token: 0x060109C5 RID: 68037 RVA: 0x004CA914 File Offset: 0x004C8B14
		private bool PawnIsHealthyEnoughForShuttle(Pawn p)
		{
			return !p.Downed && !p.InMentalState && p.health.capacities.CanBeAwake && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x060109C6 RID: 68038 RVA: 0x004CA974 File Offset: 0x004C8B74
		public virtual void CleanUpLoadingVars()
		{
			this.autoload = false;
		}

		// Token: 0x060109CD RID: 68045 RVA: 0x004CAB10 File Offset: 0x004C8D10
		[CompilerGenerated]
		private bool <IsAllowedNow>g__CheckJob|71_0(Job job)
		{
			return typeof(JobDriver_EnterTransporter).IsAssignableFrom(job.def.driverClass) && job.GetTarget(TargetIndex.A).Thing == this.parent;
		}

		// Token: 0x0400ACFE RID: 44286
		public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

		// Token: 0x0400ACFF RID: 44287
		public List<Pawn> requiredPawns = new List<Pawn>();

		// Token: 0x0400AD00 RID: 44288
		public List<Pawn> pawnsToIgnoreIfDownedOfNotOnTheMap = new List<Pawn>();

		// Token: 0x0400AD01 RID: 44289
		public int requiredColonistCount;

		// Token: 0x0400AD02 RID: 44290
		public bool requireAllColonistsOnMap;

		// Token: 0x0400AD03 RID: 44291
		public int maxColonistCount = -1;

		// Token: 0x0400AD04 RID: 44292
		public bool acceptColonists;

		// Token: 0x0400AD05 RID: 44293
		public bool acceptChildren;

		// Token: 0x0400AD06 RID: 44294
		public bool acceptColonyPrisoners;

		// Token: 0x0400AD07 RID: 44295
		public bool onlyAcceptColonists;

		// Token: 0x0400AD08 RID: 44296
		public bool onlyAcceptHealthy;

		// Token: 0x0400AD09 RID: 44297
		public bool permitShuttle;

		// Token: 0x0400AD0A RID: 44298
		public bool allowSlaves;

		// Token: 0x0400AD0B RID: 44299
		public float minAge;

		// Token: 0x0400AD0C RID: 44300
		private bool autoload;

		// Token: 0x0400AD0D RID: 44301
		public TransportShip shipParent;

		// Token: 0x0400AD0E RID: 44302
		private CompRefuelable cachedCompRefuelable;

		// Token: 0x0400AD0F RID: 44303
		private CompTransporter cachedCompTransporter;

		// Token: 0x0400AD10 RID: 44304
		private List<CompTransporter> cachedTransporterList;

		// Token: 0x0400AD11 RID: 44305
		private const int CheckAutoloadIntervalTicks = 120;

		// Token: 0x0400AD12 RID: 44306
		private const int CheckUnloadPawnIntervalTicks = 600;

		// Token: 0x0400AD13 RID: 44307
		private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload", true);

		// Token: 0x0400AD14 RID: 44308
		private const float MinPilotingSkill = 0.1f;

		// Token: 0x0400AD15 RID: 44309
		private static readonly List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

		// Token: 0x0400AD16 RID: 44310
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

		// Token: 0x0400AD17 RID: 44311
		private static List<string> tmpRequiredLabels = new List<string>();

		// Token: 0x0400AD18 RID: 44312
		private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

		// Token: 0x0400AD19 RID: 44313
		private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

		// Token: 0x0400AD1A RID: 44314
		private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

		// Token: 0x0400AD1B RID: 44315
		private static List<Thing> tmpAllSendableItems = new List<Thing>();

		// Token: 0x0400AD1C RID: 44316
		private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();
	}
}
