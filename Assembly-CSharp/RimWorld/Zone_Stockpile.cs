using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021D5 RID: 8661
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x0600BD3F RID: 48447 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x0600BD40 RID: 48448 RVA: 0x00002501 File Offset: 0x00000701
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x0600BD41 RID: 48449 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x0600BD42 RID: 48450 RVA: 0x0036E675 File Offset: 0x0036C875
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x0600BD43 RID: 48451 RVA: 0x0036E67C File Offset: 0x0036C87C
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x0600BD44 RID: 48452 RVA: 0x0036E690 File Offset: 0x0036C890
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x0600BD45 RID: 48453 RVA: 0x0036E6C3 File Offset: 0x0036C8C3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x0600BD46 RID: 48454 RVA: 0x0036E6E5 File Offset: 0x0036C8E5
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			SlotGroup slotGroup = this.slotGroup;
			if (slotGroup != null)
			{
				slotGroup.Notify_AddedCell(sq);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Shelves, OpportunityType.GoodToKnow);
		}

		// Token: 0x0600BD47 RID: 48455 RVA: 0x0036E70B File Offset: 0x0036C90B
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x0600BD48 RID: 48456 RVA: 0x0036E720 File Offset: 0x0036C920
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ISlotGroupRemoved(this.slotGroup);
		}

		// Token: 0x0600BD49 RID: 48457 RVA: 0x0036E733 File Offset: 0x0036C933
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		// Token: 0x0600BD4A RID: 48458 RVA: 0x0036E73A File Offset: 0x0036C93A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Hide_ZoneStockpile(this);
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BD4B RID: 48459 RVA: 0x0036E74A File Offset: 0x0036C94A
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x0600BD4C RID: 48460 RVA: 0x0036E753 File Offset: 0x0036C953
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600BD4D RID: 48461 RVA: 0x0036E75B File Offset: 0x0036C95B
		public IEnumerable<IntVec3> AllSlotCells()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BD4E RID: 48462 RVA: 0x0036E76B File Offset: 0x0036C96B
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x0600BD4F RID: 48463 RVA: 0x0036E773 File Offset: 0x0036C973
		public StorageSettings GetParentStoreSettings()
		{
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x0600BD50 RID: 48464 RVA: 0x0036E77A File Offset: 0x0036C97A
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x0600BD51 RID: 48465 RVA: 0x0036E782 File Offset: 0x0036C982
		public void Notify_SettingsChanged()
		{
			if (base.Map != null && this.slotGroup != null)
			{
				base.Map.listerHaulables.Notify_SlotGroupChanged(this.slotGroup);
			}
		}

		// Token: 0x0600BD52 RID: 48466 RVA: 0x0036E7AA File Offset: 0x0036C9AA
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x17001CD9 RID: 7385
		// (get) Token: 0x0600BD53 RID: 48467 RVA: 0x0036E7B8 File Offset: 0x0036C9B8
		public int SpaceRemaining
		{
			get
			{
				return base.CellCount - base.HeldThingsCount;
			}
		}

		// Token: 0x0600BD54 RID: 48468 RVA: 0x000B7A10 File Offset: 0x000B5C10
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x17001CDA RID: 7386
		// (get) Token: 0x0600BD55 RID: 48469 RVA: 0x0036E7C7 File Offset: 0x0036C9C7
		public string GroupingLabel
		{
			get
			{
				return "StockpilePlural".Translate();
			}
		}

		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x0600BD56 RID: 48470 RVA: 0x0036E7D8 File Offset: 0x0036C9D8
		public int GroupingOrder
		{
			get
			{
				return -50;
			}
		}

		// Token: 0x0600BD57 RID: 48471 RVA: 0x0036E7DC File Offset: 0x0036C9DC
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600BD58 RID: 48472 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x04008257 RID: 33367
		public StorageSettings settings;

		// Token: 0x04008258 RID: 33368
		public SlotGroup slotGroup;

		// Token: 0x04008259 RID: 33369
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
