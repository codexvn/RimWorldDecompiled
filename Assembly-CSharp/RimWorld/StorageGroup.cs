using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029B5 RID: 10677
	public class StorageGroup : IExposable, ILoadReferenceable, IStoreSettingsParent, ISlotGroup, IRenameable
	{
		// Token: 0x1700243C RID: 9276
		// (get) Token: 0x0600EB59 RID: 60249 RVA: 0x00446223 File Offset: 0x00444423
		// (set) Token: 0x0600EB5A RID: 60250 RVA: 0x00446235 File Offset: 0x00444435
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.BaseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x1700243D RID: 9277
		// (get) Token: 0x0600EB5B RID: 60251 RVA: 0x0044623E File Offset: 0x0044443E
		public string BaseLabel
		{
			get
			{
				return "StorageGroup".Translate();
			}
		}

		// Token: 0x1700243E RID: 9278
		// (get) Token: 0x0600EB5C RID: 60252 RVA: 0x0044624F File Offset: 0x0044444F
		public string GroupingLabel
		{
			get
			{
				return "GroupPlural".Translate();
			}
		}

		// Token: 0x1700243F RID: 9279
		// (get) Token: 0x0600EB5D RID: 60253 RVA: 0x00446260 File Offset: 0x00444460
		public int GroupingOrder
		{
			get
			{
				return -100;
			}
		}

		// Token: 0x17002440 RID: 9280
		// (get) Token: 0x0600EB5E RID: 60254 RVA: 0x00446264 File Offset: 0x00444464
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17002441 RID: 9281
		// (get) Token: 0x0600EB5F RID: 60255 RVA: 0x0044626C File Offset: 0x0044446C
		public Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17002442 RID: 9282
		// (get) Token: 0x0600EB60 RID: 60256 RVA: 0x00446274 File Offset: 0x00444474
		public int MemberCount
		{
			get
			{
				return this.members.Count;
			}
		}

		// Token: 0x17002443 RID: 9283
		// (get) Token: 0x0600EB61 RID: 60257 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600EB62 RID: 60258 RVA: 0x00446281 File Offset: 0x00444481
		public StorageGroup()
		{
		}

		// Token: 0x0600EB63 RID: 60259 RVA: 0x00446294 File Offset: 0x00444494
		public StorageGroup(Map map, string label = null)
		{
			this.map = map;
			this.settings = new StorageSettings(this);
			this.RenamableLabel = (label ?? StorageGroupManager.NewStorageName(this.BaseLabel));
		}

		// Token: 0x17002444 RID: 9284
		// (get) Token: 0x0600EB64 RID: 60260 RVA: 0x004462D0 File Offset: 0x004444D0
		StorageSettings ISlotGroup.Settings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17002445 RID: 9285
		// (get) Token: 0x0600EB65 RID: 60261 RVA: 0x000FD030 File Offset: 0x000FB230
		StorageGroup ISlotGroup.StorageGroup
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17002446 RID: 9286
		// (get) Token: 0x0600EB66 RID: 60262 RVA: 0x004462D8 File Offset: 0x004444D8
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				foreach (IStorageGroupMember storageGroupMember in this.members)
				{
					ISlotGroupParent slotGroupParent = storageGroupMember as ISlotGroupParent;
					if (slotGroupParent != null)
					{
						foreach (Thing thing in slotGroupParent.GetSlotGroup().HeldThings)
						{
							yield return thing;
						}
						IEnumerator<Thing> enumerator2 = null;
					}
				}
				List<IStorageGroupMember>.Enumerator enumerator = default(List<IStorageGroupMember>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17002447 RID: 9287
		// (get) Token: 0x0600EB67 RID: 60263 RVA: 0x004462E8 File Offset: 0x004444E8
		public List<IntVec3> CellsList
		{
			get
			{
				StorageGroup.tmpCellsList.Clear();
				foreach (IStorageGroupMember storageGroupMember in this.members)
				{
					ISlotGroupParent slotGroupParent = storageGroupMember as ISlotGroupParent;
					if (slotGroupParent != null)
					{
						StorageGroup.tmpCellsList.AddRange(slotGroupParent.GetSlotGroup().CellsList);
					}
				}
				return StorageGroup.tmpCellsList;
			}
		}

		// Token: 0x17002448 RID: 9288
		// (get) Token: 0x0600EB68 RID: 60264 RVA: 0x00446360 File Offset: 0x00444560
		public List<IHaulSource> HaulSourcesList
		{
			get
			{
				StorageGroup.tmpHaulSourceList.Clear();
				foreach (IStorageGroupMember storageGroupMember in this.members)
				{
					IHaulSource haulSource = storageGroupMember as IHaulSource;
					if (haulSource != null)
					{
						StorageGroup.tmpHaulSourceList.Add(haulSource);
					}
				}
				return StorageGroup.tmpHaulSourceList;
			}
		}

		// Token: 0x0600EB69 RID: 60265 RVA: 0x004463D0 File Offset: 0x004445D0
		public void InitFrom(IStorageGroupMember member)
		{
			this.settings.CopyFrom(member.StoreSettings);
		}

		// Token: 0x0600EB6A RID: 60266 RVA: 0x004463E4 File Offset: 0x004445E4
		public void RemoveMember(IStorageGroupMember member, bool removeIfEmpty = true)
		{
			if (this.members.Remove(member))
			{
				IStoreSettingsParent storeSettingsParent = member as IStoreSettingsParent;
				if (storeSettingsParent != null)
				{
					storeSettingsParent.Notify_SettingsChanged();
				}
				if (removeIfEmpty)
				{
					this.map.storageGroups.Notify_MemberRemoved(this);
				}
				BillUtility.Notify_ISlotGroupRemoved(this);
			}
		}

		// Token: 0x0600EB6B RID: 60267 RVA: 0x00446429 File Offset: 0x00444629
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x0600EB6C RID: 60268 RVA: 0x00446431 File Offset: 0x00444631
		public StorageSettings GetParentStoreSettings()
		{
			if (this.members.Any<IStorageGroupMember>())
			{
				return this.members[0].ParentStoreSettings;
			}
			return null;
		}

		// Token: 0x0600EB6D RID: 60269 RVA: 0x00446454 File Offset: 0x00444654
		public void Notify_SettingsChanged()
		{
			foreach (IStorageGroupMember storageGroupMember in this.members)
			{
				ISlotGroupParent slotGroupParent = storageGroupMember as ISlotGroupParent;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_SettingsChanged();
				}
			}
		}

		// Token: 0x0600EB6E RID: 60270 RVA: 0x004464B0 File Offset: 0x004446B0
		public string GetUniqueLoadID()
		{
			return "StorageGroup_" + this.loadID.ToString();
		}

		// Token: 0x0600EB6F RID: 60271 RVA: 0x004464C8 File Offset: 0x004446C8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Collections.Look<IStorageGroupMember>(ref this.members, "members", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.members.RemoveAll((IStorageGroupMember x) => x == null);
			}
		}

		// Token: 0x04009C35 RID: 39989
		public int loadID;

		// Token: 0x04009C36 RID: 39990
		private Map map;

		// Token: 0x04009C37 RID: 39991
		[LoadAlias("buildings")]
		public List<IStorageGroupMember> members = new List<IStorageGroupMember>();

		// Token: 0x04009C38 RID: 39992
		private StorageSettings settings;

		// Token: 0x04009C39 RID: 39993
		private string label;

		// Token: 0x04009C3A RID: 39994
		private static readonly List<IntVec3> tmpCellsList = new List<IntVec3>(128);

		// Token: 0x04009C3B RID: 39995
		private static readonly List<IHaulSource> tmpHaulSourceList = new List<IHaulSource>(64);
	}
}
