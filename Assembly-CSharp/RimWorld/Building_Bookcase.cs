using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200298D RID: 10637
	public class Building_Bookcase : Building, IThingHolderEvents<Book>, IHaulEnroute, ILoadReferenceable, IStorageGroupMember, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, ISearchableContents, IBeautyContainer
	{
		// Token: 0x170023C1 RID: 9153
		// (get) Token: 0x0600E9E9 RID: 59881 RVA: 0x00440DF0 File Offset: 0x0043EFF0
		private Graphic BookendGraphicEast
		{
			get
			{
				Graphic result;
				if ((result = this.bookendGraphicEastInt) == null)
				{
					result = (this.bookendGraphicEastInt = this.def.building.bookendGraphicEast.GraphicColoredFor(this));
				}
				return result;
			}
		}

		// Token: 0x170023C2 RID: 9154
		// (get) Token: 0x0600E9EA RID: 59882 RVA: 0x00440E28 File Offset: 0x0043F028
		private Graphic BookendGraphicNorth
		{
			get
			{
				Graphic result;
				if ((result = this.bookendGraphicNorthInt) == null)
				{
					result = (this.bookendGraphicNorthInt = this.def.building.bookendGraphicNorth.GraphicColoredFor(this));
				}
				return result;
			}
		}

		// Token: 0x170023C3 RID: 9155
		// (get) Token: 0x0600E9EB RID: 59883 RVA: 0x00440E5E File Offset: 0x0043F05E
		public int MaximumBooks
		{
			get
			{
				return this.def.building.maxItemsInCell * this.def.size.Area;
			}
		}

		// Token: 0x170023C4 RID: 9156
		// (get) Token: 0x0600E9EC RID: 59884 RVA: 0x00440E81 File Offset: 0x0043F081
		public IReadOnlyList<Book> HeldBooks
		{
			get
			{
				return this.innerContainer.InnerListForReading;
			}
		}

		// Token: 0x170023C5 RID: 9157
		// (get) Token: 0x0600E9ED RID: 59885 RVA: 0x00440E8E File Offset: 0x0043F08E
		public float ReadingBonus
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room == null)
				{
					return 0f;
				}
				return room.GetStat(RoomStatDefOf.ReadingBonus);
			}
		}

		// Token: 0x170023C6 RID: 9158
		// (get) Token: 0x0600E9EE RID: 59886 RVA: 0x00440EAC File Offset: 0x0043F0AC
		protected virtual Vector3[] RotOffsets
		{
			get
			{
				return Building_Bookcase.StandardBookshelfRotOffsets;
			}
		}

		// Token: 0x170023C7 RID: 9159
		// (get) Token: 0x0600E9EF RID: 59887 RVA: 0x00440EB3 File Offset: 0x0043F0B3
		public IEnumerable<float> CellsFilledPercentage
		{
			get
			{
				int books = this.HeldBooks.Count;
				int num2;
				for (int i = 0; i < this.def.size.Area; i = num2 + 1)
				{
					int num = Mathf.Min(books, this.def.building.maxItemsInCell);
					books -= num;
					yield return Mathf.Clamp01((float)num / (float)this.def.building.maxItemsInCell);
					num2 = i;
				}
				yield break;
			}
		}

		// Token: 0x170023C8 RID: 9160
		// (get) Token: 0x0600E9F0 RID: 59888 RVA: 0x00440EC3 File Offset: 0x0043F0C3
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x0600E9F1 RID: 59889 RVA: 0x00440ECB File Offset: 0x0043F0CB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E9F2 RID: 59890 RVA: 0x00440EC3 File Offset: 0x0043F0C3
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x170023C9 RID: 9161
		// (get) Token: 0x0600E9F3 RID: 59891 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E9F4 RID: 59892 RVA: 0x00440ED9 File Offset: 0x0043F0D9
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600E9F5 RID: 59893 RVA: 0x00440EF5 File Offset: 0x0043F0F5
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x170023CA RID: 9162
		// (get) Token: 0x0600E9F6 RID: 59894 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HaulSourceEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023CB RID: 9163
		// (get) Token: 0x0600E9F7 RID: 59895 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E9F8 RID: 59896 RVA: 0x00440F08 File Offset: 0x0043F108
		public bool Accepts(Thing t)
		{
			if (this.HeldBooks.Count >= this.MaximumBooks)
			{
				Book book = t as Book;
				if (book == null)
				{
					return false;
				}
				if (!this.innerContainer.InnerListForReading.Contains(book))
				{
					return false;
				}
			}
			return this.GetStoreSettings().AllowedToAccept(t) && this.innerContainer.CanAcceptAnyOf(t, true);
		}

		// Token: 0x0600E9F9 RID: 59897 RVA: 0x00440F66 File Offset: 0x0043F166
		public int SpaceRemainingFor(ThingDef _)
		{
			return this.MaximumBooks - this.HeldBooks.Count;
		}

		// Token: 0x0600E9FA RID: 59898 RVA: 0x00440F7A File Offset: 0x0043F17A
		public void Notify_SettingsChanged()
		{
			if (!base.Spawned)
			{
				return;
			}
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}

		// Token: 0x170023CC RID: 9164
		// (get) Token: 0x0600E9FB RID: 59899 RVA: 0x00440F96 File Offset: 0x0043F196
		// (set) Token: 0x0600E9FC RID: 59900 RVA: 0x00440F9E File Offset: 0x0043F19E
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

		// Token: 0x170023CD RID: 9165
		// (get) Token: 0x0600E9FD RID: 59901 RVA: 0x00440FA7 File Offset: 0x0043F1A7
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x170023CE RID: 9166
		// (get) Token: 0x0600E9FE RID: 59902 RVA: 0x00440FAF File Offset: 0x0043F1AF
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.MapHeld;
			}
		}

		// Token: 0x170023CF RID: 9167
		// (get) Token: 0x0600E9FF RID: 59903 RVA: 0x00440FB7 File Offset: 0x0043F1B7
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x170023D0 RID: 9168
		// (get) Token: 0x0600EA00 RID: 59904 RVA: 0x00440FC9 File Offset: 0x0043F1C9
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x170023D1 RID: 9169
		// (get) Token: 0x0600EA01 RID: 59905 RVA: 0x00440FD1 File Offset: 0x0043F1D1
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x170023D2 RID: 9170
		// (get) Token: 0x0600EA02 RID: 59906 RVA: 0x00440FD9 File Offset: 0x0043F1D9
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x170023D3 RID: 9171
		// (get) Token: 0x0600EA03 RID: 59907 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023D4 RID: 9172
		// (get) Token: 0x0600EA04 RID: 59908 RVA: 0x00438FD7 File Offset: 0x004371D7
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170023D5 RID: 9173
		// (get) Token: 0x0600EA05 RID: 59909 RVA: 0x00440FE1 File Offset: 0x0043F1E1
		public float BeautyOffset
		{
			get
			{
				if (this.cachedBeauty < 0f)
				{
					this.cachedBeauty = this.HeldBooks.Aggregate(0f, (float beauty, Book book) => beauty + book.GetBeauty(this.GetRoom(RegionType.Set_All).PsychologicallyOutdoors));
				}
				return this.cachedBeauty - 0.001f;
			}
		}

		// Token: 0x170023D6 RID: 9174
		// (get) Token: 0x0600EA06 RID: 59910 RVA: 0x0044101E File Offset: 0x0043F21E
		string IBeautyContainer.BeautyOffsetExplanation
		{
			get
			{
				return string.Format("{0}: {1}", "ContainedBookBeauty".Translate(), this.BeautyOffset.ToStringWithSign("0.##"));
			}
		}

		// Token: 0x0600EA07 RID: 59911 RVA: 0x00441049 File Offset: 0x0043F249
		public void Notify_ItemAdded(Book item)
		{
			this.DirtyRoomStats();
			this.cachedBeauty = -1f;
			base.MapHeld.listerHaulables.Notify_AddedThing(item);
		}

		// Token: 0x0600EA08 RID: 59912 RVA: 0x0044106D File Offset: 0x0043F26D
		public void Notify_ItemRemoved(Book item)
		{
			this.DirtyRoomStats();
			this.cachedBeauty = -1f;
		}

		// Token: 0x0600EA09 RID: 59913 RVA: 0x00441080 File Offset: 0x0043F280
		public Building_Bookcase()
		{
			this.innerContainer = new ThingOwner<Book>(this, false, LookMode.Deep, true);
		}

		// Token: 0x0600EA0A RID: 59914 RVA: 0x004410A4 File Offset: 0x0043F2A4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.storageGroup != null && map != this.storageGroup.Map)
			{
				StorageSettings storeSettings = this.storageGroup.GetStoreSettings();
				this.storageGroup.RemoveMember(this, true);
				this.storageGroup = null;
				this.settings.CopyFrom(storeSettings);
			}
		}

		// Token: 0x0600EA0B RID: 59915 RVA: 0x004410FC File Offset: 0x0043F2FC
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
			this.DirtyRoomStats();
		}

		// Token: 0x0600EA0C RID: 59916 RVA: 0x00441150 File Offset: 0x0043F350
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			drawLoc -= Altitudes.AltIncVect * 2f;
			base.DrawAt(drawLoc, flip);
			Rot4 rot = base.Rotation.Rotated(RotationDirection.Counterclockwise);
			float num = (base.Rotation == Rot4.North || base.Rotation == Rot4.South) ? 0.155f : 0.16f;
			Vector3 a = rot.FacingCell.ToVector3() * num;
			Vector3 b = rot.FacingCell.ToVector3() * ((float)(-(float)this.MaximumBooks) * num * 0.5f);
			Vector3 b2 = this.RotOffsets[base.Rotation.AsInt];
			for (int i = 0; i < this.HeldBooks.Count; i++)
			{
				Book book = this.HeldBooks[i];
				Rot4 opposite = base.Rotation.Opposite;
				if (opposite == Rot4.East || opposite == Rot4.West)
				{
					opposite = opposite.Opposite;
				}
				Vector3 loc = drawLoc + b + b2 + Building_Bookcase.DrawOffset + a * (float)i;
				book.VerticalGraphic.Draw(loc, opposite, this, 0f);
			}
			if (base.Rotation != Rot4.South)
			{
				if (base.Rotation != Rot4.North && this.def.building.bookendGraphicEast != null)
				{
					this.BookendGraphicEast.Draw(drawLoc + Building_Bookcase.DrawOffsetBookcaseEnd, Rot4.North, this, 0f);
					return;
				}
				if (base.Rotation == Rot4.North && this.def.building.bookendGraphicNorth != null)
				{
					this.BookendGraphicNorth.Draw(drawLoc + Building_Bookcase.DrawOffsetBookcaseEnd, Rot4.North, this, 0f);
				}
			}
		}

		// Token: 0x0600EA0D RID: 59917 RVA: 0x00441358 File Offset: 0x0043F558
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.storageGroup != null)
			{
				StorageGroup storageGroup = this.storageGroup;
				if (storageGroup != null)
				{
					storageGroup.RemoveMember(this, true);
				}
				this.storageGroup = null;
			}
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600EA0E RID: 59918 RVA: 0x004413B0 File Offset: 0x0043F5B0
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (Find.Selector.SingleSelectedThing == this)
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room != null && room.ProperRoom)
				{
					room.DrawFieldEdges();
				}
			}
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600EA0F RID: 59919 RVA: 0x004413F0 File Offset: 0x0043F5F0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				if (this.storageGroup != null)
				{
					text += string.Format("{0}: {1} ", "StorageGroupLabel".Translate(), this.storageGroup.RenamableLabel.CapitalizeFirst());
					if (this.storageGroup.MemberCount > 1)
					{
						text += "(" + "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst() + ")\n";
					}
					else
					{
						text += "(" + "OneBuilding".Translate() + ")\n";
					}
				}
				text += string.Format("{0}: {1} / {2}", "BookshelfStoredInspect".Translate(), this.HeldBooks.Count, this.MaximumBooks);
				text += string.Format("\n{0}: {1}", "BookshelfReadingBonusInspect".Translate(), Mathf.Max(this.ReadingBonus - 1f, 0f).ToStringPercent());
				if (this.IsOutside())
				{
					text += string.Format(" ({0})", "Outdoors".Translate()).Colorize(Color.gray).ResolveTags();
				}
			}
			return text;
		}

		// Token: 0x0600EA10 RID: 59920 RVA: 0x00441588 File Offset: 0x0043F788
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
			}
			if (!DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Fill",
				action = delegate()
				{
					for (int i = this.HeldBooks.Count; i < this.MaximumBooks; i++)
					{
						Book item = BookUtility.MakeBook(ThingDefOf.TextBook, ArtGenerationContext.Outsider, null);
						this.innerContainer.TryAdd(item, true);
					}
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600EA11 RID: 59921 RVA: 0x00441598 File Offset: 0x0043F798
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in HaulSourceUtility.GetFloatMenuOptions(this, selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (Book book in this.HeldBooks)
			{
				foreach (FloatMenuOption floatMenuOption2 in book.GetFloatMenuOptions(selPawn))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			IEnumerator<Book> enumerator2 = null;
			foreach (FloatMenuOption floatMenuOption3 in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600EA12 RID: 59922 RVA: 0x004415AF File Offset: 0x0043F7AF
		private void DirtyRoomStats()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			room.Notify_ContainedThingSpawnedOrDespawned(this);
		}

		// Token: 0x0600EA13 RID: 59923 RVA: 0x004415C4 File Offset: 0x0043F7C4
		public override void Notify_ColorChanged()
		{
			this.bookendGraphicEastInt = null;
			this.bookendGraphicNorthInt = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x0600EA14 RID: 59924 RVA: 0x004415DC File Offset: 0x0043F7DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner<Book>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
		}

		// Token: 0x04009B9F RID: 39839
		private ThingOwner<Book> innerContainer;

		// Token: 0x04009BA0 RID: 39840
		private StorageSettings settings;

		// Token: 0x04009BA1 RID: 39841
		private StorageGroup storageGroup;

		// Token: 0x04009BA2 RID: 39842
		private Graphic bookendGraphicEastInt;

		// Token: 0x04009BA3 RID: 39843
		private Graphic bookendGraphicNorthInt;

		// Token: 0x04009BA4 RID: 39844
		private static readonly Vector3 DrawOffset = new Vector3(0f, 0.018292684f, 0f);

		// Token: 0x04009BA5 RID: 39845
		private static readonly Vector3 DrawOffsetBookcaseEnd = new Vector3(0f, 0.07317074f, 0f);

		// Token: 0x04009BA6 RID: 39846
		private const float BookWidthEastWest = 0.16f;

		// Token: 0x04009BA7 RID: 39847
		private const float BookWidthNorthSouth = 0.155f;

		// Token: 0x04009BA8 RID: 39848
		private static readonly Vector3[] StandardBookshelfRotOffsets = new Vector3[]
		{
			new Vector3(-0.081f, 0f, 0f),
			new Vector3(-0.082f, 0f, 0.02f),
			new Vector3(0.08f, 0f, 0.08f),
			new Vector3(0.082f, 0f, -0.15f)
		};

		// Token: 0x04009BA9 RID: 39849
		private float cachedBeauty = -1f;
	}
}
