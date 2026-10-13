using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002954 RID: 10580
	public class Blueprint_Storage : Blueprint_Build, IStorageGroupMember, IStoreSettingsParent
	{
		// Token: 0x1700233E RID: 9022
		// (get) Token: 0x0600E7ED RID: 59373 RVA: 0x00438F94 File Offset: 0x00437194
		// (set) Token: 0x0600E7EE RID: 59374 RVA: 0x00438F9C File Offset: 0x0043719C
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

		// Token: 0x1700233F RID: 9023
		// (get) Token: 0x0600E7EF RID: 59375 RVA: 0x00438FA5 File Offset: 0x004371A5
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17002340 RID: 9024
		// (get) Token: 0x0600E7F0 RID: 59376 RVA: 0x00438FAD File Offset: 0x004371AD
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17002341 RID: 9025
		// (get) Token: 0x0600E7F1 RID: 59377 RVA: 0x00438FB5 File Offset: 0x004371B5
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17002342 RID: 9026
		// (get) Token: 0x0600E7F2 RID: 59378 RVA: 0x00438FBD File Offset: 0x004371BD
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17002343 RID: 9027
		// (get) Token: 0x0600E7F3 RID: 59379 RVA: 0x00438FC5 File Offset: 0x004371C5
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return base.BuildDef.building.storageGroupTag;
			}
		}

		// Token: 0x17002344 RID: 9028
		// (get) Token: 0x0600E7F4 RID: 59380 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002345 RID: 9029
		// (get) Token: 0x0600E7F5 RID: 59381 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002346 RID: 9030
		// (get) Token: 0x0600E7F6 RID: 59382 RVA: 0x00438FD7 File Offset: 0x004371D7
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17002347 RID: 9031
		// (get) Token: 0x0600E7F7 RID: 59383 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStoreSettingsParent.StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E7F8 RID: 59384 RVA: 0x00438FE6 File Offset: 0x004371E6
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600E7F9 RID: 59385 RVA: 0x00439002 File Offset: 0x00437202
		public StorageSettings GetParentStoreSettings()
		{
			return base.BuildDef.building.fixedStorageSettings;
		}

		// Token: 0x0600E7FA RID: 59386 RVA: 0x000026EA File Offset: 0x000008EA
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
		}

		// Token: 0x0600E7FB RID: 59387 RVA: 0x00439014 File Offset: 0x00437214
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (base.BuildDef.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(base.BuildDef.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600E7FC RID: 59388 RVA: 0x00439060 File Offset: 0x00437260
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			StorageGroup storageGroup = this.storageGroup;
			if (storageGroup != null)
			{
				storageGroup.RemoveMember(this, true);
			}
			this.storageGroup = null;
			base.Destroy(mode);
		}

		// Token: 0x0600E7FD RID: 59389 RVA: 0x00439083 File Offset: 0x00437283
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			if (base.BuildDef.inspectorTabsResolved != null)
			{
				foreach (InspectTabBase inspectTabBase in base.BuildDef.inspectorTabsResolved)
				{
					if (inspectTabBase.VisibleInBlueprintMode)
					{
						yield return inspectTabBase;
					}
				}
				List<InspectTabBase>.Enumerator enumerator = default(List<InspectTabBase>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E7FE RID: 59390 RVA: 0x00439093 File Offset: 0x00437293
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Frame frame = (Frame)base.MakeSolidThing(out shouldSelect);
			frame.SetStorageGroup(this.storageGroup, true);
			frame.storageSettings = new StorageSettings();
			frame.storageSettings.CopyFrom(this.GetStoreSettings());
			this.SetStorageGroup(null, false);
			return frame;
		}

		// Token: 0x0600E7FF RID: 59391 RVA: 0x004390D2 File Offset: 0x004372D2
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600E800 RID: 59392 RVA: 0x004390E0 File Offset: 0x004372E0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.storageGroup != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "LinkedStorageSettings".Translate() + ": ";
				if (this.storageGroup.MemberCount > 1)
				{
					text += "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst();
				}
				else
				{
					text += "OneBuilding".Translate().CapitalizeFirst();
				}
			}
			return text;
		}

		// Token: 0x0600E801 RID: 59393 RVA: 0x00439190 File Offset: 0x00437390
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
			foreach (Gizmo gizmo3 in StorageGroupUtility.StorageGroupMemberGizmos(this))
			{
				yield return gizmo3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E802 RID: 59394 RVA: 0x004391A0 File Offset: 0x004373A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x04009ABB RID: 39611
		protected StorageGroup storageGroup;

		// Token: 0x04009ABC RID: 39612
		public StorageSettings settings;
	}
}
