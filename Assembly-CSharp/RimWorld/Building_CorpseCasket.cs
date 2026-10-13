using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029EB RID: 10731
	public abstract class Building_CorpseCasket : Building_Casket, IHaulDestination, IStoreSettingsParent, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x1700248B RID: 9355
		// (get) Token: 0x0600ECD3 RID: 60627 RVA: 0x0044C001 File Offset: 0x0044A201
		public virtual bool StorageTabVisible
		{
			get
			{
				return !this.HasCorpse;
			}
		}

		// Token: 0x1700248C RID: 9356
		// (get) Token: 0x0600ECD4 RID: 60628 RVA: 0x0044C00C File Offset: 0x0044A20C
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		// Token: 0x1700248D RID: 9357
		// (get) Token: 0x0600ECD5 RID: 60629 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700248E RID: 9358
		// (get) Token: 0x0600ECD6 RID: 60630 RVA: 0x0044C018 File Offset: 0x0044A218
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		// Token: 0x0600ECD7 RID: 60631 RVA: 0x0044C053 File Offset: 0x0044A253
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		// Token: 0x0600ECD8 RID: 60632 RVA: 0x00440EF5 File Offset: 0x0043F0F5
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600ECD9 RID: 60633 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0600ECDA RID: 60634 RVA: 0x0044C05B File Offset: 0x0044A25B
		public int SpaceRemainingFor(ThingDef _)
		{
			if (!this.HasCorpse)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600ECDB RID: 60635 RVA: 0x0044C068 File Offset: 0x0044A268
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600ECDC RID: 60636 RVA: 0x0044C0B4 File Offset: 0x0044A2B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		// Token: 0x04009CE8 RID: 40168
		protected StorageSettings storageSettings;
	}
}
