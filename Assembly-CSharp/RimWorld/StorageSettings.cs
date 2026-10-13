using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C1 RID: 10689
	public class StorageSettings : IExposable
	{
		// Token: 0x17002450 RID: 9296
		// (get) Token: 0x0600EBA3 RID: 60323 RVA: 0x00447247 File Offset: 0x00445447
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17002451 RID: 9297
		// (get) Token: 0x0600EBA4 RID: 60324 RVA: 0x00447254 File Offset: 0x00445454
		private IHaulSource HaulSourceOwner
		{
			get
			{
				return this.owner as IHaulSource;
			}
		}

		// Token: 0x17002452 RID: 9298
		// (get) Token: 0x0600EBA5 RID: 60325 RVA: 0x00447261 File Offset: 0x00445461
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17002453 RID: 9299
		// (get) Token: 0x0600EBA6 RID: 60326 RVA: 0x0044726E File Offset: 0x0044546E
		// (set) Token: 0x0600EBA7 RID: 60327 RVA: 0x00447278 File Offset: 0x00445478
		public StoragePriority Priority
		{
			get
			{
				return this.priorityInt;
			}
			set
			{
				this.priorityInt = value;
				if (Current.ProgramState != ProgramState.Playing)
				{
					return;
				}
				StorageGroup storageGroup = this.owner as StorageGroup;
				if (storageGroup == null)
				{
					if (this.HaulDestinationOwner != null && this.HaulDestinationOwner.Map != null)
					{
						this.HaulDestinationOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
					}
					if (this.HaulSourceOwner != null && this.HaulSourceOwner.Map != null)
					{
						this.HaulSourceOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
						this.HaulSourceOwner.Map.listerHaulables.RecalculateAllInHaulSource(this.HaulSourceOwner);
					}
					if (this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.Map != null)
					{
						this.SlotGroupParentOwner.Map.listerHaulables.RecalcAllInCells(this.SlotGroupParentOwner.AllSlotCells());
					}
					return;
				}
				Map map = storageGroup.Map;
				if (map != null)
				{
					map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
				}
				Map map2 = storageGroup.Map;
				if (map2 != null)
				{
					map2.listerHaulables.RecalcAllInCells(storageGroup.CellsList);
				}
				Map map3 = storageGroup.Map;
				if (map3 == null)
				{
					return;
				}
				map3.listerHaulables.RecalculateAllInHaulSources(storageGroup.HaulSourcesList);
			}
		}

		// Token: 0x0600EBA8 RID: 60328 RVA: 0x00447396 File Offset: 0x00445596
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x0600EBA9 RID: 60329 RVA: 0x004473BC File Offset: 0x004455BC
		public StorageSettings(IStoreSettingsParent owner) : this()
		{
			this.owner = owner;
			if (owner != null)
			{
				StorageSettings parentStoreSettings = owner.GetParentStoreSettings();
				if (parentStoreSettings != null)
				{
					this.priorityInt = parentStoreSettings.priorityInt;
				}
			}
		}

		// Token: 0x0600EBAA RID: 60330 RVA: 0x004473EF File Offset: 0x004455EF
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[]
			{
				new Action(this.TryNotifyChanged)
			});
		}

		// Token: 0x0600EBAB RID: 60331 RVA: 0x00447428 File Offset: 0x00445628
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x0600EBAC RID: 60332 RVA: 0x0044743C File Offset: 0x0044563C
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x0600EBAD RID: 60333 RVA: 0x00447464 File Offset: 0x00445664
		public bool AllowedToAccept(Thing t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EBAE RID: 60334 RVA: 0x004474A4 File Offset: 0x004456A4
		public bool AllowedToAccept(ThingDef t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EBAF RID: 60335 RVA: 0x004474E4 File Offset: 0x004456E4
		private void TryNotifyChanged()
		{
			IStoreSettingsParent storeSettingsParent = this.owner;
			if (storeSettingsParent == null)
			{
				return;
			}
			storeSettingsParent.Notify_SettingsChanged();
		}

		// Token: 0x0600EBB0 RID: 60336 RVA: 0x004474F6 File Offset: 0x004456F6
		public static StorageSettings EverStorableFixedSettings()
		{
			if (StorageSettings.cachedEverStorableFixedSettings == null)
			{
				StorageSettings.cachedEverStorableFixedSettings = new StorageSettings(null)
				{
					filter = ThingFilter.CreateOnlyEverStorableThingFilter()
				};
			}
			return StorageSettings.cachedEverStorableFixedSettings;
		}

		// Token: 0x0600EBB1 RID: 60337 RVA: 0x0044751A File Offset: 0x0044571A
		public static void ResetStaticData()
		{
			StorageSettings.cachedEverStorableFixedSettings = null;
		}

		// Token: 0x04009C62 RID: 40034
		private static StorageSettings cachedEverStorableFixedSettings;

		// Token: 0x04009C63 RID: 40035
		public IStoreSettingsParent owner;

		// Token: 0x04009C64 RID: 40036
		public ThingFilter filter;

		// Token: 0x04009C65 RID: 40037
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;
	}
}
