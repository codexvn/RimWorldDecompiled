using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A0 RID: 10656
	[StaticConstructorOnStartup]
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination, IStorageGroupMember, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x0600EAAB RID: 60075 RVA: 0x00444A02 File Offset: 0x00442C02
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170023F6 RID: 9206
		// (get) Token: 0x0600EAAC RID: 60076 RVA: 0x00444A16 File Offset: 0x00442C16
		// (set) Token: 0x0600EAAD RID: 60077 RVA: 0x00444A1E File Offset: 0x00442C1E
		StorageGroup IStorageGroupMember.Group
		{
			get
			{
				return this.storageGroup;
			}
			set
			{
				this.storageGroup = value;
			}
		}

		// Token: 0x170023F7 RID: 9207
		// (get) Token: 0x0600EAAE RID: 60078 RVA: 0x00440FA7 File Offset: 0x0043F1A7
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x170023F8 RID: 9208
		// (get) Token: 0x0600EAAF RID: 60079 RVA: 0x00440FAF File Offset: 0x0043F1AF
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.MapHeld;
			}
		}

		// Token: 0x170023F9 RID: 9209
		// (get) Token: 0x0600EAB0 RID: 60080 RVA: 0x00440FB7 File Offset: 0x0043F1B7
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x170023FA RID: 9210
		// (get) Token: 0x0600EAB1 RID: 60081 RVA: 0x00444A27 File Offset: 0x00442C27
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x170023FB RID: 9211
		// (get) Token: 0x0600EAB2 RID: 60082 RVA: 0x00444A2F File Offset: 0x00442C2F
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x170023FC RID: 9212
		// (get) Token: 0x0600EAB3 RID: 60083 RVA: 0x00444A37 File Offset: 0x00442C37
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x170023FD RID: 9213
		// (get) Token: 0x0600EAB4 RID: 60084 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023FE RID: 9214
		// (get) Token: 0x0600EAB5 RID: 60085 RVA: 0x00438FD7 File Offset: 0x004371D7
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170023FF RID: 9215
		// (get) Token: 0x0600EAB6 RID: 60086 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002400 RID: 9216
		// (get) Token: 0x0600EAB7 RID: 60087 RVA: 0x00444A3F File Offset: 0x00442C3F
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x0600EAB8 RID: 60088 RVA: 0x00444A51 File Offset: 0x00442C51
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600EAB9 RID: 60089 RVA: 0x00444A59 File Offset: 0x00442C59
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600EABA RID: 60090 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x0600EABB RID: 60091 RVA: 0x00444A86 File Offset: 0x00442C86
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			if (!base.Spawned)
			{
				yield break;
			}
			foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(this))
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600EABC RID: 60092 RVA: 0x00444A98 File Offset: 0x00442C98
		public List<IntVec3> AllSlotCellsList()
		{
			List<IntVec3> result;
			if ((result = this.cachedOccupiedCells) == null)
			{
				result = (this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>());
			}
			return result;
		}

		// Token: 0x0600EABD RID: 60093 RVA: 0x00444AC3 File Offset: 0x00442CC3
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600EABE RID: 60094 RVA: 0x00444AE0 File Offset: 0x00442CE0
		public StorageSettings GetParentStoreSettings()
		{
			StorageSettings fixedStorageSettings = this.def.building.fixedStorageSettings;
			if (fixedStorageSettings != null)
			{
				return fixedStorageSettings;
			}
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x0600EABF RID: 60095 RVA: 0x00444B08 File Offset: 0x00442D08
		public void Notify_SettingsChanged()
		{
			if (base.Spawned && this.slotGroup != null)
			{
				base.Map.listerHaulables.Notify_SlotGroupChanged(this.slotGroup);
			}
		}

		// Token: 0x0600EAC0 RID: 60096 RVA: 0x00066AFC File Offset: 0x00064CFC
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x17002401 RID: 9217
		// (get) Token: 0x0600EAC1 RID: 60097 RVA: 0x00444B30 File Offset: 0x00442D30
		public string GroupingLabel
		{
			get
			{
				return this.def.building.groupingLabel;
			}
		}

		// Token: 0x17002402 RID: 9218
		// (get) Token: 0x0600EAC2 RID: 60098 RVA: 0x00444B42 File Offset: 0x00442D42
		public int GroupingOrder
		{
			get
			{
				return this.def.building.groupingOrder;
			}
		}

		// Token: 0x17002403 RID: 9219
		// (get) Token: 0x0600EAC3 RID: 60099 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600EAC4 RID: 60100 RVA: 0x00444B54 File Offset: 0x00442D54
		public bool Accepts(Thing t)
		{
			return this.GetStoreSettings().AllowedToAccept(t);
		}

		// Token: 0x0600EAC5 RID: 60101 RVA: 0x00444B64 File Offset: 0x00442D64
		public int SpaceRemainingFor(ThingDef _)
		{
			return this.slotGroup.HeldThingsCount - this.def.building.maxItemsInCell * this.def.Size.Area;
		}

		// Token: 0x0600EAC6 RID: 60102 RVA: 0x00444BA4 File Offset: 0x00442DA4
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600EAC7 RID: 60103 RVA: 0x00444BF0 File Offset: 0x00442DF0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.storageGroup != null && map != this.storageGroup.Map)
			{
				StorageSettings storeSettings = this.storageGroup.GetStoreSettings();
				this.storageGroup.RemoveMember(this, true);
				this.storageGroup = null;
				this.settings.CopyFrom(storeSettings);
			}
		}

		// Token: 0x0600EAC8 RID: 60104 RVA: 0x00444C4E File Offset: 0x00442E4E
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cachedOccupiedCells = null;
		}

		// Token: 0x0600EAC9 RID: 60105 RVA: 0x00444C5E File Offset: 0x00442E5E
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.storageGroup != null)
			{
				StorageGroup storageGroup = this.storageGroup;
				if (storageGroup != null)
				{
					storageGroup.RemoveMember(this, true);
				}
				this.storageGroup = null;
			}
			BillUtility.Notify_ISlotGroupRemoved(this.slotGroup);
		}

		// Token: 0x0600EACA RID: 60106 RVA: 0x00444C94 File Offset: 0x00442E94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
		}

		// Token: 0x0600EACB RID: 60107 RVA: 0x00444CE4 File Offset: 0x00442EE4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600EACC RID: 60108 RVA: 0x00444CF4 File Offset: 0x00442EF4
		public override string GetInspectString()
		{
			Building_Storage.sb.Clear();
			Building_Storage.sb.Append(base.GetInspectString());
			if (base.Spawned)
			{
				if (this.storageGroup != null)
				{
					Building_Storage.sb.AppendLineIfNotEmpty();
					Building_Storage.sb.Append(string.Format("{0}: {1} ", "StorageGroupLabel".Translate(), this.storageGroup.RenamableLabel.CapitalizeFirst()));
					if (this.storageGroup.MemberCount > 1)
					{
						Building_Storage.sb.Append(string.Format("({0})", "NumBuildings".Translate(this.storageGroup.MemberCount)));
					}
					else
					{
						Building_Storage.sb.Append(string.Format("({0})", "OneBuilding".Translate()));
					}
				}
				if (this.slotGroup.HeldThings.Any<Thing>())
				{
					Building_Storage.sb.AppendLineIfNotEmpty();
					Building_Storage.sb.Append("StoresThings".Translate());
					Building_Storage.sb.Append(": ");
					Building_Storage.sb.Append((from x in this.slotGroup.HeldThings
					select x.LabelShortCap).Distinct<string>().ToCommaList(false, false));
					Building_Storage.sb.Append(".");
				}
			}
			return Building_Storage.sb.ToString();
		}

		// Token: 0x0600EACD RID: 60109 RVA: 0x00444E84 File Offset: 0x00443084
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.GetStoreSettings()))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (this.StorageTabVisible && base.MapHeld != null)
			{
				foreach (Gizmo gizmo3 in StorageGroupUtility.StorageGroupMemberGizmos(this))
				{
					yield return gizmo3;
				}
				enumerator = null;
				if (Find.Selector.NumSelected == 1)
				{
					foreach (Thing thing in this.slotGroup.HeldThings)
					{
						yield return ContainingSelectionUtility.CreateSelectStorageGizmo("CommandSelectStoredThing".Translate(thing), ("CommandSelectStoredThingDesc".Translate() + "\n\n" + thing.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + thing.GetInspectString()).Resolve(), thing, thing, false);
					}
					IEnumerator<Thing> enumerator2 = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04009BFC RID: 39932
		public StorageSettings settings;

		// Token: 0x04009BFD RID: 39933
		public StorageGroup storageGroup;

		// Token: 0x04009BFE RID: 39934
		public string label;

		// Token: 0x04009BFF RID: 39935
		public SlotGroup slotGroup;

		// Token: 0x04009C00 RID: 39936
		private List<IntVec3> cachedOccupiedCells;

		// Token: 0x04009C01 RID: 39937
		private static StringBuilder sb = new StringBuilder();
	}
}
