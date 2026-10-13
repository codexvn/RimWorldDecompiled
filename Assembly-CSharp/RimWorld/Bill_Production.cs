using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A68 RID: 10856
	public class Bill_Production : Bill, IRenameable
	{
		// Token: 0x170025B8 RID: 9656
		// (get) Token: 0x0600F17A RID: 61818 RVA: 0x004609C8 File Offset: 0x0045EBC8
		protected override string StatusString
		{
			get
			{
				if (this.paused)
				{
					return " " + "Paused".Translate().CapitalizeFirst();
				}
				return "";
			}
		}

		// Token: 0x170025B9 RID: 9657
		// (get) Token: 0x0600F17B RID: 61819 RVA: 0x00460A04 File Offset: 0x0045EC04
		protected override float StatusLineMinHeight
		{
			get
			{
				if (!this.CanUnpause())
				{
					return 0f;
				}
				return 24f;
			}
		}

		// Token: 0x170025BA RID: 9658
		// (get) Token: 0x0600F17C RID: 61820 RVA: 0x00460A1C File Offset: 0x0045EC1C
		public string RepeatInfoText
		{
			get
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					return "Forever".Translate();
				}
				if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					return this.repeatCount.ToString() + "x";
				}
				if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					return this.recipe.WorkerCounter.CountProducts(this).ToString() + "/" + this.targetCount.ToString();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170025BB RID: 9659
		// (get) Token: 0x0600F17D RID: 61821 RVA: 0x00460AAA File Offset: 0x0045ECAA
		// (set) Token: 0x0600F17E RID: 61822 RVA: 0x00460ABC File Offset: 0x0045ECBC
		public string RenamableLabel
		{
			get
			{
				return this.playerCustomName ?? this.BaseLabel;
			}
			set
			{
				this.playerCustomName = value;
			}
		}

		// Token: 0x170025BC RID: 9660
		// (get) Token: 0x0600F17F RID: 61823 RVA: 0x00460AC5 File Offset: 0x0045ECC5
		public string BaseLabel
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x170025BD RID: 9661
		// (get) Token: 0x0600F180 RID: 61824 RVA: 0x00460ACD File Offset: 0x0045ECCD
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x170025BE RID: 9662
		// (get) Token: 0x0600F181 RID: 61825 RVA: 0x00460ACD File Offset: 0x0045ECCD
		public override string LabelCap
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x0600F182 RID: 61826 RVA: 0x00460AD8 File Offset: 0x0045ECD8
		public Bill_Production()
		{
		}

		// Token: 0x0600F183 RID: 61827 RVA: 0x00460B30 File Offset: 0x0045ED30
		public Bill_Production(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F184 RID: 61828 RVA: 0x00460B88 File Offset: 0x0045ED88
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<BillRepeatModeDef>(ref this.repeatMode, "repeatMode");
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 0, false);
			Scribe_Defs.Look<BillStoreModeDef>(ref this.storeMode, "storeMode");
			Scribe_Values.Look<string>(ref this.playerCustomName, "playerCustomName", null, false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 0, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseWhenYouHave, "unpauseWhenYouHave", 0, false);
			Scribe_Values.Look<bool>(ref this.includeEquipped, "includeEquipped", false, false);
			Scribe_Values.Look<bool>(ref this.includeTainted, "includeTainted", false, false);
			Scribe_Values.Look<FloatRange>(ref this.hpRange, "hpRange", FloatRange.ZeroToOne, false);
			Scribe_Values.Look<QualityRange>(ref this.qualityRange, "qualityRange", QualityRange.All, false);
			Scribe_Values.Look<bool>(ref this.limitToAllowedStuff, "limitToAllowedStuff", false, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Bill_Production.SaveSlotReferencable(this.storeGroup, "storeGroup");
				Bill_Production.SaveSlotReferencable(this.includeGroup, "includeGroup");
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Bill_Production.LoadSlotReferencable(ref this.storeGroup, "storeGroup");
				Bill_Production.LoadSlotReferencable(ref this.includeGroup, "includeGroup");
			}
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			if (this.repeatMode == null)
			{
				this.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}
			if (this.storeMode == null)
			{
				this.storeMode = BillStoreModeDefOf.BestStockpile;
			}
		}

		// Token: 0x0600F185 RID: 61829 RVA: 0x00460D0C File Offset: 0x0045EF0C
		private static void SaveSlotReferencable(ISlotGroup slot, string key)
		{
			ILoadReferenceable loadReferenceable = null;
			ILoadReferenceable loadReferenceable2 = slot as ILoadReferenceable;
			if (loadReferenceable2 != null)
			{
				loadReferenceable = loadReferenceable2;
			}
			else
			{
				SlotGroup slotGroup = slot as SlotGroup;
				if (slotGroup != null)
				{
					ILoadReferenceable loadReferenceable3 = slotGroup.parent as ILoadReferenceable;
					if (loadReferenceable3 != null)
					{
						loadReferenceable = loadReferenceable3;
					}
				}
			}
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, key, false);
		}

		// Token: 0x0600F186 RID: 61830 RVA: 0x00460D50 File Offset: 0x0045EF50
		private static void LoadSlotReferencable(ref ISlotGroup slot, string key)
		{
			ILoadReferenceable loadReferenceable = null;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, key, false);
			ISlotGroup slotGroup = loadReferenceable as ISlotGroup;
			if (slotGroup != null)
			{
				slot = slotGroup;
				return;
			}
			ISlotGroupParent slotGroupParent = loadReferenceable as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				slot = slotGroupParent.GetSlotGroup();
			}
		}

		// Token: 0x0600F187 RID: 61831 RVA: 0x00460D88 File Offset: 0x0045EF88
		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		// Token: 0x0600F188 RID: 61832 RVA: 0x00460D90 File Offset: 0x0045EF90
		public override ISlotGroup GetSlotGroup()
		{
			return this.storeGroup;
		}

		// Token: 0x0600F189 RID: 61833 RVA: 0x00460D98 File Offset: 0x0045EF98
		public void SetIncludeGroup(ISlotGroup group)
		{
			this.includeGroup = group;
		}

		// Token: 0x0600F18A RID: 61834 RVA: 0x00460DA1 File Offset: 0x0045EFA1
		public ISlotGroup GetIncludeSlotGroup()
		{
			return this.includeGroup;
		}

		// Token: 0x0600F18B RID: 61835 RVA: 0x00460DA9 File Offset: 0x0045EFA9
		public override void SetStoreMode(BillStoreModeDef mode, ISlotGroup group = null)
		{
			this.storeGroup = group;
			this.storeMode = mode;
			if (this.storeMode == BillStoreModeDefOf.SpecificStockpile != (group != null))
			{
				Log.ErrorOnce("Inconsistent bill StoreMode data set", 75645354);
			}
		}

		// Token: 0x0600F18C RID: 61836 RVA: 0x00460DDC File Offset: 0x0045EFDC
		public override bool ShouldDoNow()
		{
			if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
			{
				this.paused = false;
			}
			if (this.suspended)
			{
				return false;
			}
			if (this.repeatMode == BillRepeatModeDefOf.Forever)
			{
				return true;
			}
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				return this.repeatCount > 0;
			}
			if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				int num = this.recipe.WorkerCounter.CountProducts(this);
				if (this.pauseWhenSatisfied && num >= this.targetCount)
				{
					this.paused = true;
				}
				if (num <= this.unpauseWhenYouHave || !this.pauseWhenSatisfied)
				{
					this.paused = false;
				}
				return !this.paused && num < this.targetCount;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600F18D RID: 61837 RVA: 0x00460E98 File Offset: 0x0045F098
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				if (this.repeatCount > 0)
				{
					this.repeatCount--;
				}
				if (this.repeatCount == 0)
				{
					Messages.Message("MessageBillComplete".Translate(this.LabelCap), (Thing)this.billStack.billGiver, MessageTypeDefOf.TaskCompletion, true);
				}
			}
			this.recipe.Worker.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x0600F18E RID: 61838 RVA: 0x00460F1D File Offset: 0x0045F11D
		protected virtual Window GetBillDialog()
		{
			return new Dialog_BillConfig(this, ((Thing)this.billStack.billGiver).Position);
		}

		// Token: 0x0600F18F RID: 61839 RVA: 0x00460F3C File Offset: 0x0045F13C
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, true, true, null))
			{
				Find.WindowStack.Add(this.GetBillDialog());
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.Resolve().PadRight(20), null, true, true, true, null))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, null, null, null, null, true, -1f))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount += num;
					this.unpauseWhenYouHave += num;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount += GenUI.CurrentAdjustmentMultiplier();
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount.ToString());
				}
			}
			if (widgetRow.ButtonIcon(TexButton.Minus, null, null, null, null, true, -1f))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num2 = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount = Mathf.Max(0, this.targetCount - num2);
					this.unpauseWhenYouHave = Mathf.Max(0, this.unpauseWhenYouHave - num2);
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - GenUI.CurrentAdjustmentMultiplier());
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount.ToString());
				}
			}
		}

		// Token: 0x0600F190 RID: 61840 RVA: 0x0046122C File Offset: 0x0045F42C
		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && this.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		// Token: 0x0600F191 RID: 61841 RVA: 0x00461268 File Offset: 0x0045F468
		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused && new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f).ButtonText("Unpause".Translate(), null, true, true, true, null))
			{
				this.paused = false;
			}
		}

		// Token: 0x0600F192 RID: 61842 RVA: 0x004612C8 File Offset: 0x0045F4C8
		public override void ValidateSettings()
		{
			base.ValidateSettings();
			this.ValidateGroup(ref this.storeGroup);
			this.ValidateGroup(ref this.includeGroup);
			if (this.storeGroup == null && this.storeMode == BillStoreModeDefOf.SpecificStockpile)
			{
				this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
			}
		}

		// Token: 0x0600F193 RID: 61843 RVA: 0x00461314 File Offset: 0x0045F514
		private void ValidateGroup(ref ISlotGroup slot)
		{
			if (slot == null)
			{
				return;
			}
			SlotGroup slotGroup = slot as SlotGroup;
			if (slotGroup != null)
			{
				Zone_Stockpile zone_Stockpile = slotGroup.parent as Zone_Stockpile;
				if (zone_Stockpile != null && !this.IsZoneValid(zone_Stockpile))
				{
					slot = null;
					return;
				}
				Building_Storage building_Storage = slotGroup.parent as Building_Storage;
				if (building_Storage != null && !this.IsBuildingValid(building_Storage))
				{
					slot = null;
					return;
				}
			}
			else
			{
				StorageGroup storageGroup = slot as StorageGroup;
				if (storageGroup != null && !this.IsStorageGroupValid(storageGroup))
				{
					slot = null;
				}
			}
		}

		// Token: 0x0600F194 RID: 61844 RVA: 0x00461380 File Offset: 0x0045F580
		private bool IsZoneValid(Zone_Stockpile zone)
		{
			if (zone == null)
			{
				return false;
			}
			int id = zone.ID;
			if (!base.Map.zoneManager.AllZones.Contains(zone))
			{
				zone = (base.Map.zoneManager.AllZones.FirstOrDefault((Zone x) => x.ID == id) as Zone_Stockpile);
			}
			if (zone == null)
			{
				return false;
			}
			if (!zone.zoneManager.AllZones.Contains(zone))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeZoneDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), zone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(zone))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeZoneUnavailable".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), zone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600F195 RID: 61845 RVA: 0x004614F4 File Offset: 0x0045F6F4
		private bool IsBuildingValid(Building_Storage storage)
		{
			if (storage == null)
			{
				return true;
			}
			if (base.Map != null && !base.Map.haulDestinationManager.AllGroups.Contains(storage.slotGroup))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeBuildingDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), storage.LabelCap), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600F196 RID: 61846 RVA: 0x00461598 File Offset: 0x0045F798
		private bool IsStorageGroupValid(StorageGroup group)
		{
			if (group == null)
			{
				return true;
			}
			if (base.Map != null && !base.Map.storageGroups.HasStorageGroup(group))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationIncludeStorageGroupDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), group.RenamableLabel), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600F197 RID: 61847 RVA: 0x00461630 File Offset: 0x0045F830
		public override Bill Clone()
		{
			Bill_Production bill_Production = (Bill_Production)base.Clone();
			bill_Production.repeatMode = this.repeatMode;
			bill_Production.repeatCount = this.repeatCount;
			bill_Production.storeMode = this.storeMode;
			bill_Production.storeGroup = this.storeGroup;
			bill_Production.playerCustomName = this.playerCustomName;
			bill_Production.targetCount = this.targetCount;
			bill_Production.pauseWhenSatisfied = this.pauseWhenSatisfied;
			bill_Production.unpauseWhenYouHave = this.unpauseWhenYouHave;
			bill_Production.includeEquipped = this.includeEquipped;
			bill_Production.includeTainted = this.includeTainted;
			bill_Production.includeGroup = this.includeGroup;
			bill_Production.hpRange = this.hpRange;
			bill_Production.qualityRange = this.qualityRange;
			bill_Production.limitToAllowedStuff = this.limitToAllowedStuff;
			bill_Production.paused = this.paused;
			return bill_Production;
		}

		// Token: 0x04009F3A RID: 40762
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		// Token: 0x04009F3B RID: 40763
		public int repeatCount = 1;

		// Token: 0x04009F3C RID: 40764
		private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		// Token: 0x04009F3D RID: 40765
		private ISlotGroup storeGroup;

		// Token: 0x04009F3E RID: 40766
		private string playerCustomName;

		// Token: 0x04009F3F RID: 40767
		public int targetCount = 10;

		// Token: 0x04009F40 RID: 40768
		public bool pauseWhenSatisfied;

		// Token: 0x04009F41 RID: 40769
		public int unpauseWhenYouHave = 5;

		// Token: 0x04009F42 RID: 40770
		public bool includeEquipped;

		// Token: 0x04009F43 RID: 40771
		public bool includeTainted;

		// Token: 0x04009F44 RID: 40772
		public FloatRange hpRange = FloatRange.ZeroToOne;

		// Token: 0x04009F45 RID: 40773
		public QualityRange qualityRange = QualityRange.All;

		// Token: 0x04009F46 RID: 40774
		public bool limitToAllowedStuff;

		// Token: 0x04009F47 RID: 40775
		private ISlotGroup includeGroup;

		// Token: 0x04009F48 RID: 40776
		public bool paused;
	}
}
