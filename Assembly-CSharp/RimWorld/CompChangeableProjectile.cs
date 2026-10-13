using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C43 RID: 11331
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x170027FC RID: 10236
		// (get) Token: 0x0600FE16 RID: 65046 RVA: 0x0049B3CB File Offset: 0x004995CB
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170027FD RID: 10237
		// (get) Token: 0x0600FE17 RID: 65047 RVA: 0x0049B3D8 File Offset: 0x004995D8
		public ThingDef LoadedShell
		{
			get
			{
				if (this.loadedCount <= 0)
				{
					return null;
				}
				return this.loadedShell;
			}
		}

		// Token: 0x170027FE RID: 10238
		// (get) Token: 0x0600FE18 RID: 65048 RVA: 0x0049B3EB File Offset: 0x004995EB
		public ThingDef Projectile
		{
			get
			{
				if (!this.Loaded)
				{
					return null;
				}
				return this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x170027FF RID: 10239
		// (get) Token: 0x0600FE19 RID: 65049 RVA: 0x0049B402 File Offset: 0x00499602
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x17002800 RID: 10240
		// (get) Token: 0x0600FE1A RID: 65050 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600FE1B RID: 65051 RVA: 0x0049B40D File Offset: 0x0049960D
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		// Token: 0x0600FE1C RID: 65052 RVA: 0x0049B448 File Offset: 0x00499648
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600FE1D RID: 65053 RVA: 0x0049B49F File Offset: 0x0049969F
		public virtual void Notify_ProjectileLaunched()
		{
			if (this.loadedCount > 0)
			{
				this.loadedCount--;
			}
			if (this.loadedCount <= 0)
			{
				this.loadedShell = null;
			}
		}

		// Token: 0x0600FE1E RID: 65054 RVA: 0x0049B4C8 File Offset: 0x004996C8
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		// Token: 0x0600FE1F RID: 65055 RVA: 0x0049B4E5 File Offset: 0x004996E5
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x0600FE20 RID: 65056 RVA: 0x0049B50D File Offset: 0x0049970D
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x0600FE21 RID: 65057 RVA: 0x00496857 File Offset: 0x00494A57
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x0600FE22 RID: 65058 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0400A693 RID: 42643
		private ThingDef loadedShell;

		// Token: 0x0400A694 RID: 42644
		public int loadedCount;

		// Token: 0x0400A695 RID: 42645
		public StorageSettings allowedShellsSettings;
	}
}
