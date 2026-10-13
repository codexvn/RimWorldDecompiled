using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003DA9 RID: 15785
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x1700357C RID: 13692
		// (get) Token: 0x06015813 RID: 88083 RVA: 0x00671695 File Offset: 0x0066F895
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x1700357D RID: 13693
		// (get) Token: 0x06015814 RID: 88084 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport CanBeSettled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700357E RID: 13694
		// (get) Token: 0x06015815 RID: 88085 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700357F RID: 13695
		// (get) Token: 0x06015816 RID: 88086 RVA: 0x006716A0 File Offset: 0x0066F8A0
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17003580 RID: 13696
		// (get) Token: 0x06015817 RID: 88087 RVA: 0x006716AD File Offset: 0x0066F8AD
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.def.mapGenerator ?? MapGeneratorDefOf.Encounter;
			}
		}

		// Token: 0x17003581 RID: 13697
		// (get) Token: 0x06015818 RID: 88088 RVA: 0x0043BC50 File Offset: 0x00439E50
		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				return Enumerable.Empty<GenStepWithParams>();
			}
		}

		// Token: 0x17003582 RID: 13698
		// (get) Token: 0x06015819 RID: 88089 RVA: 0x006716C3 File Offset: 0x0066F8C3
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x17003583 RID: 13699
		// (get) Token: 0x0601581A RID: 88090 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HandlesConditionCausers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003584 RID: 13700
		// (get) Token: 0x0601581B RID: 88091 RVA: 0x006716D8 File Offset: 0x0066F8D8
		public virtual Vector3 WorldCameraPosition
		{
			get
			{
				return base.Tile.Layer.GetTileCenter(base.Tile);
			}
		}

		// Token: 0x0601581C RID: 88092 RVA: 0x00671700 File Offset: 0x0066F900
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
		}

		// Token: 0x0601581D RID: 88093 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MyMapAboutToBeRemoved()
		{
		}

		// Token: 0x0601581E RID: 88094 RVA: 0x0067174C File Offset: 0x0066F94C
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
		}

		// Token: 0x0601581F RID: 88095 RVA: 0x00671798 File Offset: 0x0066F998
		public virtual void Notify_MyMapSettled(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapSettled();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapSettled", this.Named("SUBJECT"));
		}

		// Token: 0x06015820 RID: 88096 RVA: 0x006717E4 File Offset: 0x0066F9E4
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x06015821 RID: 88097 RVA: 0x00671816 File Offset: 0x0066FA16
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06015822 RID: 88098 RVA: 0x00671816 File Offset: 0x0066FA16
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06015823 RID: 88099 RVA: 0x003D6395 File Offset: 0x003D4595
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06015824 RID: 88100 RVA: 0x0067181E File Offset: 0x0066FA1E
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map, true);
			}
		}

		// Token: 0x06015825 RID: 88101 RVA: 0x00671840 File Offset: 0x0066FA40
		public virtual void Abandon(bool wasGravshipLaunch)
		{
			bool flag = false;
			int count = Find.Maps.Count;
			for (int i = 0; i < count; i++)
			{
				if (Find.Maps[i].IsPlayerHome && Find.Maps[i] != this.Map)
				{
					flag = true;
					break;
				}
			}
			if (this.Map.IsPlayerHome && !flag)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_GeneTracker genes = pawn.genes;
					if (genes != null)
					{
						genes.Notify_NewColony();
					}
					if (!pawn.BeingTransportedOnGravship)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						MemoryThoughtHandler memoryThoughtHandler;
						if (needs == null)
						{
							memoryThoughtHandler = null;
						}
						else
						{
							Need_Mood mood = needs.mood;
							if (mood == null)
							{
								memoryThoughtHandler = null;
							}
							else
							{
								ThoughtHandler thoughts = mood.thoughts;
								memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
							}
						}
						MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
						if (memoryThoughtHandler2 != null)
						{
							memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
							memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
							if (pawn.IsFreeNonSlaveColonist)
							{
								memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
							}
						}
					}
				}
			}
			this.Destroy();
			if (wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.GravshipLaunch);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x06015826 RID: 88102 RVA: 0x006719B8 File Offset: 0x0066FBB8
		public override void Destroy()
		{
			if (this.Map != null)
			{
				for (int i = this.Map.listerThings.AllThings.Count - 1; i >= 0; i--)
				{
					this.Map.listerThings.AllThings[i].Notify_LeftBehind();
				}
			}
			base.Destroy();
		}

		// Token: 0x06015827 RID: 88103 RVA: 0x00671A10 File Offset: 0x0066FC10
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.CheckRemoveMapNow();
		}

		// Token: 0x06015828 RID: 88104 RVA: 0x00671A1F File Offset: 0x0066FC1F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
			Scribe_Values.Look<bool>(ref this.doorsAlwaysOpenForPlayerPawns, "doorsAlwaysOpenForPlayerPawns", false, false);
		}

		// Token: 0x06015829 RID: 88105 RVA: 0x00671A4B File Offset: 0x0066FC4B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate()
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0601582A RID: 88106 RVA: 0x00671A5B File Offset: 0x0066FC5B
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in base.IncidentTargetTags())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.hibernatableIncidentTargets)
				{
					yield return incidentTargetTagDef2;
				}
				HashSet<IncidentTargetTagDef>.Enumerator enumerator2 = default(HashSet<IncidentTargetTagDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601582B RID: 88107 RVA: 0x00671A6B File Offset: 0x0066FC6B
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601582C RID: 88108 RVA: 0x00671A82 File Offset: 0x0066FC82
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportersFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				Action<LocalTargetInfo> <>9__1;
				yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate()
				{
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Targeter targeter = Find.Targeter;
					TargetingParameters targetParams = TargetingParameters.ForDropPodsDestination();
					Action<LocalTargetInfo> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(LocalTargetInfo x)
						{
							launchAction(this.Tile, new TransportersArrivalAction_LandInSpecificCell(this, x.Cell, Rot4.North, false));
						});
					}
					targeter.BeginTargeting(targetParams, action, null, null, CompLaunchable.TargeterMouseAttachment, true);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601582D RID: 88109 RVA: 0x00671AA0 File Offset: 0x0066FCA0
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			MapParent.<>c__DisplayClass40_0 CS$<>8__locals1 = new MapParent.<>c__DisplayClass40_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.launchAction = launchAction;
			MapParent.<>c__DisplayClass40_0 CS$<>8__locals2 = CS$<>8__locals1;
			CompTransporter compTransporter = pods.FirstOrDefault<IThingHolder>() as CompTransporter;
			CS$<>8__locals2.shuttle = ((compTransporter != null) ? compTransporter.parent : null);
			if (TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				yield return new FloatMenuOption("LandInExistingMap".Translate(this.Label), delegate()
				{
					MapParent.<>c__DisplayClass40_1 CS$<>8__locals3 = new MapParent.<>c__DisplayClass40_1();
					CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
					Map map = CS$<>8__locals1.<>4__this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					MapParent.<>c__DisplayClass40_1 CS$<>8__locals4 = CS$<>8__locals3;
					ThingWithComps shuttle = CS$<>8__locals1.shuttle;
					CS$<>8__locals4.shuttleDef = (((shuttle != null) ? shuttle.def : null) ?? ThingDefOf.Shuttle);
					MapParent.shuttleRotation = CS$<>8__locals3.shuttleDef.defaultPlacingRot;
					Targeter targeter = Find.Targeter;
					TargetingParameters targetParams = TargetingParameters.ForCell();
					Action<LocalTargetInfo> action;
					if ((action = CS$<>8__locals1.<>9__1) == null)
					{
						action = (CS$<>8__locals1.<>9__1 = delegate(LocalTargetInfo x)
						{
							CS$<>8__locals1.launchAction(CS$<>8__locals1.<>4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(CS$<>8__locals1.<>4__this, x.Cell, MapParent.shuttleRotation, true));
						});
					}
					targeter.BeginTargeting(targetParams, action, delegate(LocalTargetInfo x)
					{
						RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(x, CS$<>8__locals3.CS$<>8__locals1.<>4__this.Map, CS$<>8__locals3.shuttleDef, MapParent.shuttleRotation);
					}, delegate(LocalTargetInfo x)
					{
						AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(x, CS$<>8__locals3.CS$<>8__locals1.<>4__this.Map, CS$<>8__locals3.shuttleDef, new Rot4?(MapParent.shuttleRotation));
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, new LookTargets(CS$<>8__locals3.CS$<>8__locals1.<>4__this), MessageTypeDefOf.RejectInput, false);
						}
						return acceptanceReport.Accepted;
					}, null, null, CompLaunchable.TargeterMouseAttachment, true, delegate(LocalTargetInfo _)
					{
						if (!CS$<>8__locals3.shuttleDef.rotatable)
						{
							return;
						}
						if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
						{
							MapParent.shuttleRotation = MapParent.shuttleRotation.Rotated(RotationDirection.Clockwise);
						}
						if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
						{
							MapParent.shuttleRotation = MapParent.shuttleRotation.Rotated(RotationDirection.Counterclockwise);
						}
					}, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x0601582E RID: 88110 RVA: 0x00671AC0 File Offset: 0x0066FCC0
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map, true);
				if (!base.Destroyed && (flag || this.forceRemoveWorldObjectWhenMapRemoved))
				{
					this.Destroy();
				}
			}
		}

		// Token: 0x0601582F RID: 88111 RVA: 0x00671B0C File Offset: 0x0066FD0C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string t;
			if (this.HasMap && GravshipUtility.TryGetNameOfGravshipOnMap(this.Map, out t))
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "GravshipOnTileInspectString".Translate().CapitalizeFirst() + ": " + t;
			}
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(this.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					text += "\n" + list[i].LabelShortCap + " (" + "ConditionCauserRadius".Translate(list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange) + ")";
				}
			}
			return text;
		}

		// Token: 0x06015830 RID: 88112 RVA: 0x00671C5C File Offset: 0x0066FE5C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				int num = 0;
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					num = Mathf.Max(num, list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange);
				}
				if (num > 0)
				{
					GenDraw.DrawWorldRadiusRing(base.Tile, num, null);
				}
			}
		}

		// Token: 0x06015831 RID: 88113 RVA: 0x00002C42 File Offset: 0x00000E42
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06015832 RID: 88114 RVA: 0x00671CD3 File Offset: 0x0066FED3
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x06015833 RID: 88115 RVA: 0x00671CF8 File Offset: 0x0066FEF8
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTagDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Token: 0x0400EF3D RID: 61245
		public bool forceRemoveWorldObjectWhenMapRemoved;

		// Token: 0x0400EF3E RID: 61246
		public bool doorsAlwaysOpenForPlayerPawns;

		// Token: 0x0400EF3F RID: 61247
		private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

		// Token: 0x0400EF40 RID: 61248
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

		// Token: 0x0400EF41 RID: 61249
		private static Rot4 shuttleRotation = Rot4.East;
	}
}
