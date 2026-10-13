using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EA1 RID: 11937
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17002B3B RID: 11067
		// (get) Token: 0x06010D04 RID: 68868 RVA: 0x004D6C40 File Offset: 0x004D4E40
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x17002B3C RID: 11068
		// (get) Token: 0x06010D05 RID: 68869 RVA: 0x004D6C4D File Offset: 0x004D4E4D
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x17002B3D RID: 11069
		// (get) Token: 0x06010D06 RID: 68870 RVA: 0x004D6C5A File Offset: 0x004D4E5A
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x17002B3E RID: 11070
		// (get) Token: 0x06010D07 RID: 68871 RVA: 0x004D6C65 File Offset: 0x004D4E65
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0 || this.parent.IsInCaravan();
			}
		}

		// Token: 0x17002B3F RID: 11071
		// (get) Token: 0x06010D08 RID: 68872 RVA: 0x004D6C7D File Offset: 0x004D4E7D
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17002B40 RID: 11072
		// (get) Token: 0x06010D09 RID: 68873 RVA: 0x004D6C88 File Offset: 0x004D4E88
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityOverride > 0f)
				{
					return this.massCapacityOverride;
				}
				return this.Props.massCapacity;
			}
		}

		// Token: 0x17002B41 RID: 11073
		// (get) Token: 0x06010D0A RID: 68874 RVA: 0x004D6CA9 File Offset: 0x004D4EA9
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17002B42 RID: 11074
		// (get) Token: 0x06010D0B RID: 68875 RVA: 0x004D6CB1 File Offset: 0x004D4EB1
		public bool Groupable
		{
			get
			{
				return !this.Props.max1PerGroup;
			}
		}

		// Token: 0x17002B43 RID: 11075
		// (get) Token: 0x06010D0C RID: 68876 RVA: 0x004D6CC1 File Offset: 0x004D4EC1
		public bool OverMassCapacity
		{
			get
			{
				return this.MassUsage > this.MassCapacity;
			}
		}

		// Token: 0x17002B44 RID: 11076
		// (get) Token: 0x06010D0D RID: 68877 RVA: 0x004D6CD1 File Offset: 0x004D4ED1
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsage(this.innerContainer, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17002B45 RID: 11077
		// (get) Token: 0x06010D0E RID: 68878 RVA: 0x004D6CFC File Offset: 0x004D4EFC
		public CompLaunchable Launchable
		{
			get
			{
				CompLaunchable result;
				if ((result = this.cachedCompLaunchable) == null)
				{
					result = (this.cachedCompLaunchable = this.parent.GetComp<CompLaunchable>());
				}
				return result;
			}
		}

		// Token: 0x17002B46 RID: 11078
		// (get) Token: 0x06010D0F RID: 68879 RVA: 0x004D6D28 File Offset: 0x004D4F28
		public CompShuttle Shuttle
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedCompShuttle) == null)
				{
					result = (this.cachedCompShuttle = this.parent.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x17002B47 RID: 11079
		// (get) Token: 0x06010D10 RID: 68880 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool RequiresFuelingPort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002B48 RID: 11080
		// (get) Token: 0x06010D11 RID: 68881 RVA: 0x004D6D54 File Offset: 0x004D4F54
		public Thing FirstThingLeftToLoad
		{
			get
			{
				if (this.leftToLoad == null)
				{
					return null;
				}
				for (int i = 0; i < this.leftToLoad.Count; i++)
				{
					if (this.leftToLoad[i].CountToTransfer != 0 && this.leftToLoad[i].HasAnyThing)
					{
						return this.leftToLoad[i].AnyThing;
					}
				}
				return null;
			}
		}

		// Token: 0x17002B49 RID: 11081
		// (get) Token: 0x06010D12 RID: 68882 RVA: 0x004D6DBC File Offset: 0x004D4FBC
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return null;
				}
				for (int i = 0; i < list.Count; i++)
				{
					Thing firstThingLeftToLoad = list[i].FirstThingLeftToLoad;
					if (firstThingLeftToLoad != null)
					{
						return firstThingLeftToLoad;
					}
				}
				return null;
			}
		}

		// Token: 0x17002B4A RID: 11082
		// (get) Token: 0x06010D13 RID: 68883 RVA: 0x004D6E04 File Offset: 0x004D5004
		public bool AnyInGroupNotifiedCantLoadMore
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return false;
				}
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].notifiedCantLoadMore)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002B4B RID: 11083
		// (get) Token: 0x06010D14 RID: 68884 RVA: 0x004D6E4C File Offset: 0x004D504C
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.AnythingLeftToLoad)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = this.parent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == this.groupID)
						{
							return true;
						}
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterTransporter)
					{
						CompTransporter transporter2 = ((JobDriver_EnterTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter2 != null && transporter2.groupID == this.groupID)
						{
							return true;
						}
					}
				}
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return false;
				}
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].mindState.duty != null && allPawnsSpawned[j].mindState.duty.transportersGroup == this.groupID)
					{
						CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(list, allPawnsSpawned[j]);
						if (compTransporter != null && allPawnsSpawned[j].CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (LoadTransportersJobUtility.HasJobOnTransporter(allPawnsSpawned[k], list[l]))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06010D15 RID: 68885 RVA: 0x004D7010 File Offset: 0x004D5210
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06010D16 RID: 68886 RVA: 0x004D7060 File Offset: 0x004D5260
		public override void PostExposeData()
		{
			base.PostExposeData();
			bool flag = !this.parent.SpawnedOrAnyParentSpawned;
			if (flag && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
						if (!pawn.IsWorldPawn())
						{
							string str = "Trying to save a non-world pawn (";
							Pawn pawn2 = pawn;
							Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null) + ") as a reference in a transporter.");
						}
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.notifiedCantLoadMore, "notifiedCantLoadMore", false, false);
			Scribe_Values.Look<float>(ref this.massCapacityOverride, "massCapacityOverride", 0f, false);
			if (flag && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x06010D17 RID: 68887 RVA: 0x004D6CA9 File Offset: 0x004D4EA9
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06010D18 RID: 68888 RVA: 0x004D71FF File Offset: 0x004D53FF
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06010D19 RID: 68889 RVA: 0x004D7210 File Offset: 0x004D5410
		public override void CompTick()
		{
			if (this.Props.shouldTickContents)
			{
				this.innerContainer.DoTick();
			}
			else if (this.Props.restEffectiveness != 0f)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null && !pawn.Dead && pawn.needs.rest != null)
					{
						pawn.needs.rest.TickResting(this.Props.restEffectiveness);
					}
				}
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.Spawned && this.LoadingInProgressOrReadyToLaunch && this.AnyInGroupHasAnythingLeftToLoad && !this.AnyInGroupNotifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow && (this.Shuttle == null || !this.Shuttle.Autoload))
			{
				this.notifiedCantLoadMore = true;
				if (this.Shuttle != null)
				{
					Messages.Message("MessageCantLoadMoreIntoShuttle".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
					return;
				}
				Messages.Message("MessageCantLoadMoreIntoTransporters".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x06010D1A RID: 68890 RVA: 0x004D73C4 File Offset: 0x004D55C4
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			CompTransporter.tmpTransportersInGroup.Clear();
			if (!this.parent.Spawned)
			{
				CompTransporter.tmpTransportersInGroup.Add(this);
				return CompTransporter.tmpTransportersInGroup;
			}
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		// Token: 0x06010D1B RID: 68891 RVA: 0x004D7419 File Offset: 0x004D5619
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			int num = 0;
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				ThingWithComps thingWithComps = obj as ThingWithComps;
				if (thingWithComps != null && thingWithComps.HasComp<CompTransporter>())
				{
					num++;
				}
			}
			if (this.Shuttle != null && (!this.Shuttle.ShowLoadingGizmos || num > 1))
			{
				yield break;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				CompShuttle shuttle = this.Shuttle;
				if ((shuttle == null || !shuttle.Autoload) && this.innerContainer.Any)
				{
					TaggedString taggedString = this.AnythingLeftToLoad ? "CommandCancelLoad".Translate() : "CommandUnload".Translate();
					TaggedString taggedString2 = this.AnythingLeftToLoad ? "CommandCancelLoadDesc".Translate() : "CommandUnloadDesc".Translate(this.parent.LabelShort);
					Texture2D cancelLoadCommandTex = CompTransporter.CancelLoadCommandTex;
					yield return new Command_Action
					{
						defaultLabel = taggedString,
						defaultDesc = taggedString2,
						icon = cancelLoadCommandTex,
						action = delegate()
						{
							SoundDefOf.Designate_Cancel.PlayOneShotOnCamera(null);
							this.CancelLoad();
						}
					};
				}
				if (this.Groupable)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectPreviousTransporter".Translate(),
						defaultDesc = "CommandSelectPreviousTransporterDesc".Translate(),
						icon = CompTransporter.SelectPreviousInGroupCommandTex,
						action = new Action(this.SelectPreviousInGroup)
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectAllTransporters".Translate(),
						defaultDesc = "CommandSelectAllTransportersDesc".Translate(),
						icon = CompTransporter.SelectAllInGroupCommandTex,
						action = new Action(this.SelectAllInGroup)
					};
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectNextTransporter".Translate(),
						defaultDesc = "CommandSelectNextTransporterDesc".Translate(),
						icon = CompTransporter.SelectNextInGroupCommandTex,
						action = new Action(this.SelectNextInGroup)
					};
				}
				if (this.Props.canChangeAssignedThingsAfterStarting && (this.Shuttle == null || !this.Shuttle.Autoload))
				{
					yield return new Command_LoadToTransporter
					{
						defaultLabel = "CommandSetToLoadTransporter".Translate(),
						defaultDesc = "CommandSetToLoadTransporterDesc".Translate(),
						icon = CompTransporter.LoadCommandTex,
						transComp = this
					};
				}
			}
			else
			{
				Command_LoadToTransporter command_LoadToTransporter = new Command_LoadToTransporter();
				if (!this.Groupable)
				{
					if (this.Props.canChangeAssignedThingsAfterStarting)
					{
						command_LoadToTransporter.defaultLabel = "CommandSetToLoadTransporter".Translate();
						command_LoadToTransporter.defaultDesc = "CommandSetToLoadTransporterDesc".Translate();
					}
					else
					{
						command_LoadToTransporter.defaultLabel = "CommandLoadTransporterSingle".Translate();
						command_LoadToTransporter.defaultDesc = "CommandLoadTransporterSingleDesc".Translate();
					}
				}
				else
				{
					int num2 = 0;
					for (int i = 0; i < Find.Selector.NumSelected; i++)
					{
						Thing thing = Find.Selector.SelectedObjectsListForReading[i] as Thing;
						if (thing != null && thing.def == this.parent.def)
						{
							CompLaunchable compLaunchable = thing.TryGetComp<CompLaunchable>();
							if (compLaunchable != null)
							{
								CompRefuelable refuelable = compLaunchable.Refuelable;
								if (refuelable == null || !refuelable.HasFuel)
								{
									goto IL_4BD;
								}
							}
							num2++;
						}
						IL_4BD:;
					}
					command_LoadToTransporter.defaultLabel = "CommandLoadTransporter".Translate(num2.ToString());
					command_LoadToTransporter.defaultDesc = "CommandLoadTransporterDesc".Translate();
				}
				command_LoadToTransporter.icon = CompTransporter.LoadCommandTex;
				command_LoadToTransporter.transComp = this;
				CompLaunchable launchable = this.Launchable;
				if (launchable != null && launchable.RequiresFuelingPort)
				{
					CompLaunchable_TransportPod compLaunchable_TransportPod = launchable as CompLaunchable_TransportPod;
					if (compLaunchable_TransportPod != null && !compLaunchable_TransportPod.ConnectedToFuelingPort)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNotConnectedToFuelingPort".Translate());
					}
					else if (!launchable.Refuelable.HasFuel && this.Shuttle == null)
					{
						command_LoadToTransporter.Disable("CommandLoadTransporterFailNoFuel".Translate());
					}
				}
				yield return command_LoadToTransporter;
			}
			yield break;
			yield break;
		}

		// Token: 0x06010D1C RID: 68892 RVA: 0x004D742C File Offset: 0x004D562C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.CancelLoad(map) && this.Shuttle == null)
			{
				if (!this.Groupable)
				{
					Messages.Message("MessageTransporterSingleLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
				else
				{
					Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x06010D1D RID: 68893 RVA: 0x004D74C0 File Offset: 0x004D56C0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Contents".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst());
			if (this.Props.showMassInInspectString)
			{
				TaggedString taggedString = "Mass".Translate() + ": " + this.MassUsage.ToString("F0") + " / " + this.MassCapacity.ToString("F0") + " kg";
				stringBuilder.AppendLine().Append((this.MassUsage > this.MassCapacity) ? taggedString.Colorize(ColorLibrary.RedReadable) : taggedString);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010D1E RID: 68894 RVA: 0x004D75A4 File Offset: 0x004D57A4
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || count <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null)
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					if (!transferableOneWay.things.Contains(t.things[i]))
					{
						transferableOneWay.things.Add(t.things[i]);
					}
				}
				if (transferableOneWay.CanAdjustBy(count).Accepted)
				{
					transferableOneWay.AdjustBy(count);
					return;
				}
			}
			else
			{
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				this.leftToLoad.Add(transferableOneWay2);
				transferableOneWay2.things.AddRange(t.things);
				transferableOneWay2.AdjustTo(count);
			}
		}

		// Token: 0x06010D1F RID: 68895 RVA: 0x004D7670 File Offset: 0x004D5870
		public bool LeftToLoadContains(Thing thing)
		{
			if (this.leftToLoad == null)
			{
				return false;
			}
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				for (int j = 0; j < this.leftToLoad[i].things.Count; j++)
				{
					if (this.leftToLoad[i].things[j] == thing)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06010D20 RID: 68896 RVA: 0x004D76DC File Offset: 0x004D58DC
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount, true);
			if (this.parent.Spawned && this.Props.pawnLoadedSound != null && t is Pawn)
			{
				this.Props.pawnLoadedSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.IsFormingCaravan())
			{
				pawn.GetLord().Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "ThingAdded", t.Named("SUBJECT"));
			this.massUsageDirty = true;
		}

		// Token: 0x06010D21 RID: 68897 RVA: 0x004D779C File Offset: 0x004D599C
		public void Notify_ThingRemoved(Thing t)
		{
			if (this.Props.pawnExitSound != null && t is Pawn)
			{
				this.Props.pawnExitSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
			this.massUsageDirty = true;
		}

		// Token: 0x06010D22 RID: 68898 RVA: 0x004D77F6 File Offset: 0x004D59F6
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount, true);
			this.massUsageDirty = true;
		}

		// Token: 0x06010D23 RID: 68899 RVA: 0x004D7809 File Offset: 0x004D5A09
		public bool CancelLoad()
		{
			CompShuttle shuttle = this.Shuttle;
			if (((shuttle != null) ? shuttle.shipParent : null) != null)
			{
				this.Shuttle.shipParent.ForceJob_DelayCurrent(ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload));
			}
			return this.CancelLoad(this.Map);
		}

		// Token: 0x06010D24 RID: 68900 RVA: 0x004D7848 File Offset: 0x004D5A48
		public bool CancelLoad(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return false;
			}
			this.TryRemoveLord(map);
			List<CompTransporter> list = this.TransportersInGroup(map);
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CleanUpLoadingVars(map);
			}
			this.CleanUpLoadingVars(map);
			return true;
		}

		// Token: 0x06010D25 RID: 68901 RVA: 0x004D789C File Offset: 0x004D5A9C
		public void TryRemoveLord(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, map);
			if (lord != null)
			{
				foreach (Pawn pawn in lord.ownedPawns)
				{
					if (pawn.IsColonist && map.IsPlayerHome)
					{
						pawn.inventory.UnloadEverything = true;
					}
				}
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x06010D26 RID: 68902 RVA: 0x004D792C File Offset: 0x004D5B2C
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			List<TransferableOneWay> list = this.leftToLoad;
			if (list != null)
			{
				list.Clear();
			}
			CompShuttle shuttle = this.Shuttle;
			if (shuttle != null)
			{
				shuttle.CleanUpLoadingVars();
			}
			this.notifiedCantLoadMore = false;
			this.massUsageDirty = true;
		}

		// Token: 0x06010D27 RID: 68903 RVA: 0x004D798C File Offset: 0x004D5B8C
		public int SubtractFromToLoadList(Thing t, int count, bool sendMessageOnFinished = true)
		{
			if (this.leftToLoad == null)
			{
				return 0;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				return 0;
			}
			if (transferableOneWay.CountToTransfer <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
			transferableOneWay.AdjustBy(-num);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			if (sendMessageOnFinished && !this.AnyInGroupHasAnythingLeftToLoad)
			{
				CompShuttle comp = this.parent.GetComp<CompShuttle>();
				if (comp == null || comp.AllRequiredThingsLoaded)
				{
					if (comp != null)
					{
						Messages.Message("MessageFinishedLoadingShuttle".Translate(this.parent.Named("SHUTTLE")), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
					else
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
				}
			}
			return num;
		}

		// Token: 0x06010D28 RID: 68904 RVA: 0x004D7A70 File Offset: 0x004D5C70
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list != null)
			{
				int num = list.IndexOf(this);
				CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent, CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x06010D29 RID: 68905 RVA: 0x004D7ABC File Offset: 0x004D5CBC
		private void SelectAllInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list == null)
			{
				return;
			}
			Selector selector = Find.Selector;
			selector.ClearSelection();
			for (int i = 0; i < list.Count; i++)
			{
				selector.Select(list[i].parent, true, true);
			}
		}

		// Token: 0x06010D2A RID: 68906 RVA: 0x004D7B0C File Offset: 0x004D5D0C
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list != null)
			{
				int num = list.IndexOf(this);
				CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent, CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x0400AF0B RID: 44811
		public int groupID = -1;

		// Token: 0x0400AF0C RID: 44812
		public ThingOwner innerContainer;

		// Token: 0x0400AF0D RID: 44813
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x0400AF0E RID: 44814
		private bool notifiedCantLoadMore;

		// Token: 0x0400AF0F RID: 44815
		public float massCapacityOverride = -1f;

		// Token: 0x0400AF10 RID: 44816
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x0400AF11 RID: 44817
		private CompShuttle cachedCompShuttle;

		// Token: 0x0400AF12 RID: 44818
		private bool massUsageDirty = true;

		// Token: 0x0400AF13 RID: 44819
		private float cachedMassUsage;

		// Token: 0x0400AF14 RID: 44820
		public static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400AF15 RID: 44821
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x0400AF16 RID: 44822
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x0400AF17 RID: 44823
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x0400AF18 RID: 44824
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x0400AF19 RID: 44825
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400AF1A RID: 44826
		private List<Pawn> tmpSavedPawns = new List<Pawn>();

		// Token: 0x0400AF1B RID: 44827
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
