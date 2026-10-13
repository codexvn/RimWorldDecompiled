using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002991 RID: 10641
	[StaticConstructorOnStartup]
	public class Building_OutfitStand : Building, IThingHolderEvents<Thing>, IStorageGroupMember, IHaulEnroute, ILoadReferenceable, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, IApparelSource, ISearchableContents, IBeautyContainer
	{
		// Token: 0x0600EA39 RID: 59961 RVA: 0x00441EE8 File Offset: 0x004400E8
		private static void InitGraphics()
		{
			if (Building_OutfitStand.initializedTextures)
			{
				return;
			}
			Building_OutfitStand.initializedTextures = true;
			Building_OutfitStand.baseGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Base", ShaderDatabase.Cutout, Building_OutfitStand.baseDrawSize, Color.white);
			Building_OutfitStand.bodyGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Body", ShaderDatabase.Cutout, Building_OutfitStand.bodyDrawSize, Color.white);
			Building_OutfitStand.bodyGraphicChild = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_BodyChild", ShaderDatabase.Cutout, Building_OutfitStand.bodyChildDrawSize, Color.white);
			Building_OutfitStand.headGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Head", ShaderDatabase.Cutout, Building_OutfitStand.headDrawSize, Color.white);
			Building_OutfitStand.swapOutfitIcon = ContentFinder<Texture2D>.Get("UI/Commands/SwapOutfits", true);
		}

		// Token: 0x170023DD RID: 9181
		// (get) Token: 0x0600EA3A RID: 59962 RVA: 0x00441F9F File Offset: 0x0044019F
		// (set) Token: 0x0600EA3B RID: 59963 RVA: 0x00441FA7 File Offset: 0x004401A7
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

		// Token: 0x170023DE RID: 9182
		// (get) Token: 0x0600EA3C RID: 59964 RVA: 0x00441FB0 File Offset: 0x004401B0
		public StorageSettings StoreSettings
		{
			get
			{
				StorageGroup storageGroup = this.storageGroup;
				return ((storageGroup != null) ? storageGroup.GetStoreSettings() : null) ?? this.settings;
			}
		}

		// Token: 0x170023DF RID: 9183
		// (get) Token: 0x0600EA3D RID: 59965 RVA: 0x00440EF5 File Offset: 0x0043F0F5
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.def.building.fixedStorageSettings;
			}
		}

		// Token: 0x170023E0 RID: 9184
		// (get) Token: 0x0600EA3E RID: 59966 RVA: 0x00441FCE File Offset: 0x004401CE
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x170023E1 RID: 9185
		// (get) Token: 0x0600EA3F RID: 59967 RVA: 0x00440FB7 File Offset: 0x0043F1B7
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x170023E2 RID: 9186
		// (get) Token: 0x0600EA40 RID: 59968 RVA: 0x00440FA7 File Offset: 0x0043F1A7
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x170023E3 RID: 9187
		// (get) Token: 0x0600EA41 RID: 59969 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023E4 RID: 9188
		// (get) Token: 0x0600EA42 RID: 59970 RVA: 0x00438FD7 File Offset: 0x004371D7
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170023E5 RID: 9189
		// (get) Token: 0x0600EA43 RID: 59971 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600EA44 RID: 59972 RVA: 0x00441FB0 File Offset: 0x004401B0
		public StorageSettings GetStoreSettings()
		{
			StorageGroup storageGroup = this.storageGroup;
			return ((storageGroup != null) ? storageGroup.GetStoreSettings() : null) ?? this.settings;
		}

		// Token: 0x0600EA45 RID: 59973 RVA: 0x00440EF5 File Offset: 0x0043F0F5
		StorageSettings IStoreSettingsParent.GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600EA46 RID: 59974 RVA: 0x00440F7A File Offset: 0x0043F17A
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
			if (!base.Spawned)
			{
				return;
			}
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}

		// Token: 0x0600EA47 RID: 59975 RVA: 0x00441FD8 File Offset: 0x004401D8
		int IHaulEnroute.SpaceRemainingFor(ThingDef def)
		{
			if (def.IsApparel)
			{
				if (!this.GetStoreSettings().AllowedToAccept(def))
				{
					return 0;
				}
				if (!this.HasRoomForApparelOfDef(def))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (!def.IsWeapon)
				{
					return 0;
				}
				if (this.holdingWeaponCached)
				{
					return 0;
				}
				if (!this.GetStoreSettings().AllowedToAccept(def))
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x0600EA48 RID: 59976 RVA: 0x00442030 File Offset: 0x00440230
		bool IHaulDestination.Accepts(Thing t)
		{
			if (t is Apparel)
			{
				return this.GetStoreSettings().AllowedToAccept(t) && (this.innerContainer.Contains(t) || this.HasRoomForApparelOfDef(t.def));
			}
			return t.def.IsWeapon && this.GetStoreSettings().AllowedToAccept(t) && (!this.holdingWeaponCached || this.innerContainer.Contains(t));
		}

		// Token: 0x170023E6 RID: 9190
		// (get) Token: 0x0600EA49 RID: 59977 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IHaulDestination.HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023E7 RID: 9191
		// (get) Token: 0x0600EA4A RID: 59978 RVA: 0x004420A7 File Offset: 0x004402A7
		bool IHaulSource.HaulSourceEnabled
		{
			get
			{
				return this.allowRemovingItems;
			}
		}

		// Token: 0x170023E8 RID: 9192
		// (get) Token: 0x0600EA4B RID: 59979 RVA: 0x004420A7 File Offset: 0x004402A7
		bool IApparelSource.ApparelSourceEnabled
		{
			get
			{
				return this.allowRemovingItems;
			}
		}

		// Token: 0x0600EA4C RID: 59980 RVA: 0x004420AF File Offset: 0x004402AF
		void IThingHolder.GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.innerContainer);
		}

		// Token: 0x0600EA4D RID: 59981 RVA: 0x004420BD File Offset: 0x004402BD
		ThingOwner IThingHolder.GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x170023E9 RID: 9193
		// (get) Token: 0x0600EA4E RID: 59982 RVA: 0x004420BD File Offset: 0x004402BD
		ThingOwner ISearchableContents.SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x170023EA RID: 9194
		// (get) Token: 0x0600EA4F RID: 59983 RVA: 0x004420C8 File Offset: 0x004402C8
		public float BeautyOffset
		{
			get
			{
				if (this.cachedBeauty < 0f)
				{
					bool outdoors = this.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;
					this.cachedBeauty = this.HeldItems.Aggregate(0f, (float beauty, Thing apparel) => beauty + apparel.GetBeauty(outdoors));
				}
				return this.cachedBeauty - 0.001f;
			}
		}

		// Token: 0x0600EA50 RID: 59984 RVA: 0x0044212C File Offset: 0x0044032C
		public void Notify_ItemAdded(Thing thing)
		{
			this.cachedBeauty = -1f;
			if (base.Spawned)
			{
				base.MapHeld.listerHaulables.Notify_AddedThing(thing);
				this.DirtyRoomStats();
			}
			if (!(thing is Apparel) && thing.def.IsWeapon)
			{
				this.holdingWeaponCached = true;
			}
			this.RecacheGraphics();
		}

		// Token: 0x0600EA51 RID: 59985 RVA: 0x00442188 File Offset: 0x00440388
		public void Notify_ItemRemoved(Thing thing)
		{
			this.cachedBeauty = -1f;
			if (base.Spawned)
			{
				base.MapHeld.listerHaulables.Notify_DeSpawned(thing);
				this.DirtyRoomStats();
			}
			if (this.holdingWeaponCached && !(thing is Apparel) && thing.def.IsWeapon)
			{
				this.holdingWeaponCached = false;
			}
			this.RecacheGraphics();
		}

		// Token: 0x170023EB RID: 9195
		// (get) Token: 0x0600EA52 RID: 59986 RVA: 0x004421E9 File Offset: 0x004403E9
		string IBeautyContainer.BeautyOffsetExplanation
		{
			get
			{
				return string.Format("{0}: {1}", "ContainedApparelBeauty".Translate(), this.BeautyOffset.ToStringWithSign("0.##"));
			}
		}

		// Token: 0x170023EC RID: 9196
		// (get) Token: 0x0600EA53 RID: 59987 RVA: 0x00442214 File Offset: 0x00440414
		protected virtual BodyTypeDef BodyTypeDefForRendering { get; } = BodyTypeDefOf.Male;

		// Token: 0x170023ED RID: 9197
		// (get) Token: 0x0600EA54 RID: 59988 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float WeaponDrawDistanceFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170023EE RID: 9198
		// (get) Token: 0x0600EA55 RID: 59989 RVA: 0x0044221C File Offset: 0x0044041C
		public ThingWithComps HeldWeapon
		{
			get
			{
				if (!this.holdingWeaponCached)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading.FirstOrDefault((Thing t) => t.def.IsWeapon) as ThingWithComps;
			}
		}

		// Token: 0x0600EA56 RID: 59990 RVA: 0x0044225C File Offset: 0x0044045C
		public Building_OutfitStand()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x170023EF RID: 9199
		// (get) Token: 0x0600EA57 RID: 59991 RVA: 0x0044229C File Offset: 0x0044049C
		public IReadOnlyList<Thing> HeldItems
		{
			get
			{
				return this.innerContainer.InnerListForReading;
			}
		}

		// Token: 0x0600EA58 RID: 59992 RVA: 0x004422A9 File Offset: 0x004404A9
		public bool AddApparel(Apparel apparel)
		{
			return this.innerContainer.TryAdd(apparel, true);
		}

		// Token: 0x0600EA59 RID: 59993 RVA: 0x004422B8 File Offset: 0x004404B8
		public bool RemoveApparel(Apparel apparel)
		{
			return this.innerContainer.Remove(apparel);
		}

		// Token: 0x0600EA5A RID: 59994 RVA: 0x004422C6 File Offset: 0x004404C6
		public bool RemoveHeldWeapon(Thing weapon)
		{
			return this.holdingWeaponCached && this.HeldWeapon == weapon && this.innerContainer.Remove(weapon);
		}

		// Token: 0x0600EA5B RID: 59995 RVA: 0x004422E9 File Offset: 0x004404E9
		public bool TryAddHeldWeapon(Thing weapon)
		{
			if (this.holdingWeaponCached)
			{
				return false;
			}
			if (!this.innerContainer.TryAdd(weapon, true))
			{
				return false;
			}
			this.holdingWeaponCached = true;
			return true;
		}

		// Token: 0x0600EA5C RID: 59996 RVA: 0x00442310 File Offset: 0x00440510
		public bool TryDrop(Thing thing, IntVec3 cell, ThingPlaceMode mode, int count, out Thing dropped)
		{
			if (this.innerContainer.TryDrop(thing, cell, base.Map, mode, count, out dropped, null, null))
			{
				return true;
			}
			dropped = null;
			return false;
		}

		// Token: 0x0600EA5D RID: 59997 RVA: 0x004415AF File Offset: 0x0043F7AF
		private void DirtyRoomStats()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			room.Notify_ContainedThingSpawnedOrDespawned(this);
		}

		// Token: 0x0600EA5E RID: 59998 RVA: 0x00442340 File Offset: 0x00440540
		public bool CanEverStoreThing(Thing t)
		{
			return this.def.building.fixedStorageSettings.AllowedToAccept(t);
		}

		// Token: 0x0600EA5F RID: 59999 RVA: 0x00442358 File Offset: 0x00440558
		public bool HasRoomForApparelOfDef(ThingDef t)
		{
			foreach (Thing thing in this.innerContainer)
			{
				Apparel apparel = thing as Apparel;
				if (apparel != null && !ApparelUtility.CanWearTogether(t, apparel.def, BodyDefOf.Human))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EA60 RID: 60000 RVA: 0x004423C8 File Offset: 0x004405C8
		public bool TryDropThingsToMakeRoomForThingOfDef(ThingDef t)
		{
			if (this.HasRoomForApparelOfDef(t))
			{
				return true;
			}
			List<Apparel> list = (from ap in this.innerContainer.InnerListForReading.OfType<Apparel>()
			where !ApparelUtility.CanWearTogether(t, ap.def, BodyDefOf.Human)
			select ap).ToList<Apparel>();
			if (list.NullOrEmpty<Apparel>())
			{
				return true;
			}
			foreach (Apparel thing in list)
			{
				Thing thing2;
				if (!this.innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EA61 RID: 60001 RVA: 0x0044247C File Offset: 0x0044067C
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600EA62 RID: 60002 RVA: 0x004424C8 File Offset: 0x004406C8
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
			LongEventHandler.ExecuteWhenFinished(new Action(this.RecacheGraphics));
		}

		// Token: 0x0600EA63 RID: 60003 RVA: 0x00442530 File Offset: 0x00440730
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace && mode != DestroyMode.Vanish)
			{
				this.innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600EA64 RID: 60004 RVA: 0x0044255D File Offset: 0x0044075D
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
		}

		// Token: 0x0600EA65 RID: 60005 RVA: 0x00442588 File Offset: 0x00440788
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

		// Token: 0x0600EA66 RID: 60006 RVA: 0x004425C8 File Offset: 0x004407C8
		private void RecacheGraphics()
		{
			this.cachedApparelGraphicsHeadgear.Clear();
			this.cachedApparelGraphicsNonHeadgear.Clear();
			this.cachedApparelRenderInfoSkipHead = false;
			List<Apparel> list = this.innerContainer.InnerListForReading.OfType<Apparel>().ToList<Apparel>();
			list.SortBy((Apparel a) => a.def.apparel.LastLayer.drawOrder);
			Dictionary<ApparelLayerDef, int> dictionary = new Dictionary<ApparelLayerDef, int>();
			foreach (Apparel apparel in list)
			{
				ApparelLayerDef lastLayer = apparel.def.apparel.LastLayer;
				bool flag = lastLayer == ApparelLayerDefOf.Overhead || lastLayer == ApparelLayerDefOf.EyeCover;
				this.cachedApparelRenderInfoSkipHead = (this.cachedApparelRenderInfoSkipHead || apparel.def.apparel.renderSkipFlags.NotNullAndContains(RenderSkipFlagDefOf.Head));
				ApparelGraphicRecord apparelGraphicRecord;
				if (ApparelGraphicRecordGetter.TryGetGraphicApparel(apparel, this.BodyTypeDefForRendering, false, out apparelGraphicRecord))
				{
					int valueOrDefault = dictionary.GetValueOrDefault(lastLayer, 0);
					dictionary[lastLayer] = valueOrDefault + 1;
					int num = lastLayer.drawOrder / 10 + valueOrDefault;
					DrawData drawData = apparel.def.apparel.drawData;
					float? num2 = (drawData != null) ? new float?(drawData.LayerForRot(base.Rotation, -1f)) : null;
					if (num2 != null)
					{
						float valueOrDefault2 = num2.GetValueOrDefault();
						if (valueOrDefault2 > 0f)
						{
							int num3 = flag ? 70 : 20;
							int num4 = (int)valueOrDefault2 - num3;
							num += num4 * 2;
						}
					}
					Vector3 one = Vector3.one;
					Vector3 zero = Vector3.zero;
					if (apparel.RenderAsPack())
					{
						Vector2 vector = apparel.def.apparel.wornGraphicData.BeltScaleAt(base.Rotation, this.BodyTypeDefForRendering);
						one.x *= vector.x;
						one.z *= vector.y;
						Vector2 vector2 = apparel.def.apparel.wornGraphicData.BeltOffsetAt(base.Rotation, this.BodyTypeDefForRendering);
						zero.x += vector2.x;
						zero.z += vector2.y;
						if (base.Rotation == Rot4.North)
						{
							num = 93;
						}
						else if (base.Rotation == Rot4.South)
						{
							num = -3;
						}
					}
					if (flag)
					{
						this.cachedApparelGraphicsHeadgear.Add(new Building_OutfitStand.CachedGraphicRenderInfo(apparelGraphicRecord.graphic, num, one, zero));
					}
					else
					{
						this.cachedApparelGraphicsNonHeadgear.Add(new Building_OutfitStand.CachedGraphicRenderInfo(apparelGraphicRecord.graphic, num, one, zero));
					}
				}
			}
		}

		// Token: 0x0600EA67 RID: 60007 RVA: 0x0044288C File Offset: 0x00440A8C
		private Vector3 HeadOffsetAt(Rot4 rotation)
		{
			Vector2 headOffset = this.BodyTypeDefForRendering.headOffset;
			Vector3 result;
			switch (rotation.AsInt)
			{
			case 0:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 1:
				result = new Vector3(headOffset.x, 0f, headOffset.y);
				break;
			case 2:
				result = new Vector3(0f, 0f, headOffset.y);
				break;
			case 3:
				result = new Vector3(-headOffset.x, 0f, headOffset.y);
				break;
			default:
				result = Vector3.zero;
				break;
			}
			return result;
		}

		// Token: 0x0600EA68 RID: 60008 RVA: 0x00442930 File Offset: 0x00440B30
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!Building_OutfitStand.initializedTextures)
			{
				Building_OutfitStand.InitGraphics();
			}
			bool flag = this.BodyTypeDefForRendering == BodyTypeDefOf.Child;
			Graphic coloredVersion = Building_OutfitStand.baseGraphic.GetColoredVersion(Building_OutfitStand.baseGraphic.Shader, this.DrawColor, this.DrawColorTwo);
			Vector3 loc = flag ? new Vector3(drawLoc.x, drawLoc.y, drawLoc.z + 0.2f) : drawLoc;
			coloredVersion.Draw(loc, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			Graphic_Multi graphic_Multi = flag ? Building_OutfitStand.bodyGraphicChild : Building_OutfitStand.bodyGraphic;
			graphic_Multi.GetColoredVersion(graphic_Multi.Shader, this.DrawColor, this.DrawColorTwo).Draw(drawLoc.WithYOffset(0.05f), flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			Rot4 rot = flip ? base.Rotation.Opposite : base.Rotation;
			Mesh mesh = MeshPool.GetMeshSetForSize(1.5f, 1.5f).MeshAt(rot);
			foreach (Building_OutfitStand.CachedGraphicRenderInfo cachedGraphicRenderInfo in this.cachedApparelGraphicsNonHeadgear)
			{
				Graphic graphic = cachedGraphicRenderInfo.graphic;
				Vector3 a = drawLoc;
				a.y = AltitudeLayer.Item.AltitudeFor() + PawnRenderUtility.AltitudeForLayer((float)cachedGraphicRenderInfo.layer);
				Material material = graphic.MatAt(rot, null);
				Vector3 b = graphic.DrawOffset(rot) + cachedGraphicRenderInfo.positionOffset;
				Quaternion q = graphic.QuatFromRot(rot);
				Matrix4x4 matrix = Matrix4x4.TRS(a + b, q, cachedGraphicRenderInfo.scale);
				Graphics.DrawMesh(mesh, matrix, material, 0);
			}
			if (!this.cachedApparelRenderInfoSkipHead)
			{
				foreach (Building_OutfitStand.CachedGraphicRenderInfo cachedGraphicRenderInfo2 in this.cachedApparelGraphicsHeadgear)
				{
					Vector3 a2 = drawLoc + this.HeadOffsetAt(rot);
					a2.y = AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer((float)cachedGraphicRenderInfo2.layer);
					Graphic graphic2 = cachedGraphicRenderInfo2.graphic;
					Material material2 = graphic2.MatAt(rot, null);
					Vector3 b2 = graphic2.DrawOffset(rot);
					Quaternion q2 = graphic2.QuatFromRot(rot);
					Matrix4x4 matrix2 = Matrix4x4.TRS(a2 + b2, q2, cachedGraphicRenderInfo2.scale);
					Graphics.DrawMesh(mesh, matrix2, material2, 0);
				}
				Graphic coloredVersion2 = Building_OutfitStand.headGraphic.GetColoredVersion(Building_OutfitStand.headGraphic.Shader, this.DrawColor, this.DrawColorTwo);
				Vector3 vector = drawLoc + this.HeadOffsetAt(rot);
				vector = vector.WithY(AltitudeLayer.ItemImportant.AltitudeFor() - 0.05f);
				coloredVersion2.Draw(vector, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			}
			if (this.holdingWeaponCached)
			{
				Vector3 drawPos;
				if ((flip ? base.Rotation.Opposite : base.Rotation) == Rot4.North)
				{
					drawPos = drawLoc.WithYOffset(-0.05f);
				}
				else
				{
					drawPos = drawLoc.WithY(AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer(90f));
				}
				PawnRenderUtility.DrawCarriedWeapon(this.HeldWeapon, drawPos, flip ? base.Rotation.Opposite : base.Rotation, this.WeaponDrawDistanceFactor);
			}
		}

		// Token: 0x0600EA69 RID: 60009 RVA: 0x00442CB0 File Offset: 0x00440EB0
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			if (!selPawn.IsColonistPlayerControlled)
			{
				yield break;
			}
			if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotSwapOutfit".Translate().CapitalizeFirst() + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (!this.innerContainer.Any)
			{
				yield return new FloatMenuOption("CannotSwapOutfit".Translate().CapitalizeFirst() + ": " + "OutfitStandEmpty".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			FloatMenuOption option = new FloatMenuOption("SwapOutfit".Translate().CapitalizeFirst(), delegate()
			{
				this.SetAllowHauling(false);
				selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseOutfitStand, this), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield return FloatMenuUtility.DecoratePrioritizedTask(option, selPawn, this, "ReservedBy", null);
			foreach (FloatMenuOption floatMenuOption in HaulSourceUtility.GetFloatMenuOptions(this, selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (Thing thing in this.innerContainer.InnerListForReading)
			{
				Apparel ap = thing as Apparel;
				if (ap != null)
				{
					FloatMenuOption floatMenuOptionToWear = this.GetFloatMenuOptionToWear(selPawn, ap);
					yield return floatMenuOptionToWear;
					FloatMenuOption floatMenuOptionForForceWear = this.GetFloatMenuOptionForForceWear(selPawn, ap);
					yield return floatMenuOptionForForceWear;
					ap = null;
				}
			}
			List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
			if (this.holdingWeaponCached)
			{
				yield return this.GetFloatMenuOptionToEquipWeapon(selPawn, this.HeldWeapon);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EA6A RID: 60010 RVA: 0x00442CC8 File Offset: 0x00440EC8
		private FloatMenuOption GetFloatMenuOptionToWear(Pawn selPawn, Apparel apparel)
		{
			string key = "CannotWear";
			string key2 = "ForceWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				key = "CannotEquipApparel";
				key2 = "ForceEquipApparel";
			}
			if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.apparel.WouldReplaceLockedApparel(apparel))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.IsMutant && selPawn.mutant.Def.disableApparel)
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + selPawn.mutant.Def.LabelCap, null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!ApparelUtility.HasPartsToWear(selPawn, apparel.def))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string t;
			if (!EquipmentUtility.CanEquip(apparel, selPawn, out t, true))
			{
				return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + t, null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate()
			{
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wear, apparel), new JobTag?(JobTag.Misc), false);
					});
				}
				Action action2 = action;
				Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(selPawn, apparel);
				if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(selPawn, apparelReplacedByNewApparel, action2))
				{
					action2();
				}
			}, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600EA6B RID: 60011 RVA: 0x00443064 File Offset: 0x00441264
		private FloatMenuOption GetFloatMenuOptionForForceWear(Pawn selPawn, Apparel apparel)
		{
			string cannotForceTargetText = "CannotForceTargetToWear";
			string key = "ForceTargetToWear";
			if (apparel.def.apparel.LastLayer.IsUtilityLayer)
			{
				cannotForceTargetText = "CannotForceTargetToEquipApparel";
				key = "ForceTargetToEquipApparel";
			}
			if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (apparel.IsBurning())
			{
				return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key.Translate(apparel.LabelShort, apparel), delegate()
			{
				bool queueOrder = KeyBindingDefOf.QueueOrder.IsDownEvent;
				Find.Targeter.BeginTargeting(TargetingParameters.ForForceWear(selPawn), delegate(LocalTargetInfo target)
				{
					Pawn targetPawn;
					bool queueOrder;
					if (!target.TryGetPawn(out targetPawn))
					{
						if (ModsConfig.OdysseyActive)
						{
							Building_OutfitStand building_OutfitStand = target.Thing as Building_OutfitStand;
							if (building_OutfitStand != null)
							{
								if (!building_OutfitStand.CanEverStoreThing(apparel))
								{
									Messages.Message("CannotStoreThingOnTarget".Translate(apparel.Named("THING"), building_OutfitStand.Named("TARGET")), MessageTypeDefOf.RejectInput, false);
									return;
								}
								Pawn_JobTracker jobs = selPawn.jobs;
								Job job = JobMaker.MakeJob(JobDefOf.PutApparelOnOutfitStand, apparel, building_OutfitStand);
								queueOrder = queueOrder;
								jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), queueOrder);
							}
						}
						return;
					}
					if (targetPawn.apparel.WouldReplaceLockedApparel(apparel))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (targetPawn.IsMutant && targetPawn.mutant.Def.disableApparel)
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + targetPawn.mutant.Def.LabelCap, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (!ApparelUtility.HasPartsToWear(targetPawn, apparel.def))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					string t;
					if (!EquipmentUtility.CanEquip(apparel, targetPawn, out t, true))
					{
						Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + t, targetPawn, MessageTypeDefOf.RejectInput, false);
						return;
					}
					Action action = delegate()
					{
						Pawn_JobTracker jobs2 = selPawn.jobs;
						Job job2 = JobMaker.MakeJob(JobDefOf.ForceTargetWear, targetPawn, apparel);
						bool queueOrder2 = queueOrder;
						jobs2.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), queueOrder2);
					};
					Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(targetPawn, apparel);
					if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(targetPawn, apparelReplacedByNewApparel, action))
					{
						action();
					}
				}, null, null, null, true);
			}, apparel, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600EA6C RID: 60012 RVA: 0x00443220 File Offset: 0x00441420
		private FloatMenuOption GetFloatMenuOptionToEquipWeapon(Pawn selPawn, Thing weapon)
		{
			if (!weapon.HasComp<CompEquippable>())
			{
				return null;
			}
			string labelShort = weapon.LabelShort;
			if (weapon.def.IsWeapon && selPawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(selPawn.LabelShort, selPawn), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (weapon.def.IsRangedWeapon && selPawn.WorkTagIsDisabled(WorkTags.Shooting))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfShootingLower".Translate(selPawn), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!selPawn.CanReach(weapon, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (weapon.IsBurning())
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (selPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(weapon, selPawn))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string str;
			if (!EquipmentUtility.CanEquip(weapon, selPawn, out str, false))
			{
				return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string text = "Equip".Translate(labelShort);
			if (weapon.def.IsRangedWeapon && selPawn.story != null && selPawn.story.traits.HasTrait(TraitDefOf.Brawler))
			{
				text += " " + "EquipWarningBrawler".Translate();
			}
			if (EquipmentUtility.AlreadyBondedToWeapon(weapon, selPawn))
			{
				text += " " + "BladelinkAlreadyBonded".Translate();
				TaggedString dialogText = "BladelinkAlreadyBondedDialog".Translate(selPawn.Named("PAWN"), weapon.Named("WEAPON"), selPawn.equipment.bondedWeapon.Named("BONDEDWEAPON"));
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
				{
					Find.WindowStack.Add(new Dialog_MessageBox(dialogText, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}, weapon, Color.white, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), selPawn, weapon, "ReservedBy", null);
			}
			Action <>9__3;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(weapon, selPawn);
				if (!personaWeaponConfirmationText.NullOrEmpty())
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text2 = personaWeaponConfirmationText;
					string buttonAText = "Yes".Translate();
					Action buttonAAction;
					if ((buttonAAction = <>9__3) == null)
					{
						buttonAAction = (<>9__3 = delegate()
						{
							base.<GetFloatMenuOptionToEquipWeapon>g__Equip|2();
						});
					}
					windowStack.Add(new Dialog_MessageBox(text2, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				base.<GetFloatMenuOptionToEquipWeapon>g__Equip|2();
			}, weapon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
		}

		// Token: 0x0600EA6D RID: 60013 RVA: 0x004436ED File Offset: 0x004418ED
		private void SetAllowHauling(bool allow)
		{
			if (this.allowRemovingItems == allow)
			{
				return;
			}
			this.allowRemovingItems = allow;
			base.Map.listerHaulables.RecalculateAllInHaulSource(this);
		}

		// Token: 0x0600EA6E RID: 60014 RVA: 0x00443714 File Offset: 0x00441914
		private string GetContentsString()
		{
			return "Contents".Translate() + ": " + (from a in this.innerContainer.InnerListForReading
			select a.LabelNoParenthesis).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600EA6F RID: 60015 RVA: 0x0044377C File Offset: 0x0044197C
		public override string GetInspectString()
		{
			string text = base.GetInspectString() ?? "";
			if (this.storageGroup != null)
			{
				text += string.Format("{0}: {1} ", "StorageGroupLabel".Translate(), this.storageGroup.RenamableLabel.CapitalizeFirst());
				if (this.storageGroup.MemberCount > 1)
				{
					text += "(" + "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst() + ")";
				}
				else
				{
					text += "(" + "OneBuilding".Translate() + ")";
				}
			}
			if (!this.innerContainer.Any)
			{
				return text;
			}
			if (text.Length > 0)
			{
				text += "\n";
			}
			return text + this.GetContentsString();
		}

		// Token: 0x0600EA70 RID: 60016 RVA: 0x00443882 File Offset: 0x00441A82
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
			if (base.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					defaultLabel = "SwapOutfitGizmo".Translate().CapitalizeFirst(),
					defaultDesc = "SwapOutfitDesc".Translate() + "\n\n" + this.GetContentsString(),
					icon = Building_OutfitStand.swapOutfitIcon,
					Disabled = !this.innerContainer.Any,
					disabledReason = ((!this.innerContainer.Any) ? "OutfitStandEmpty".Translate().CapitalizeFirst() : null),
					action = delegate()
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo t)
						{
							Pawn pawn;
							if (!t.TryGetPawn(out pawn))
							{
								return;
							}
							if (pawn.Downed)
							{
								Messages.Message("IsIncapped".Translate(pawn.LabelShort, pawn), MessageTypeDefOf.RejectInput, false);
								return;
							}
							if (!pawn.CanReserveAndReach(this, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
							{
								return;
							}
							this.SetAllowHauling(false);
							pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseOutfitStand, this), new JobTag?(JobTag.Misc), false);
						}, null, null, null, true);
					}
				};
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAllowRemovingApparel".Translate(),
					defaultDesc = "CommandAllowRemovingApparelDesc".Translate(),
					hotKey = KeyBindingDefOf.Command_ItemForbid,
					icon = TexCommand.ForbidOff,
					isActive = (() => this.allowRemovingItems),
					toggleAction = delegate()
					{
						this.SetAllowHauling(!this.allowRemovingItems);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EA71 RID: 60017 RVA: 0x00443894 File Offset: 0x00441A94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Values.Look<bool>(ref this.allowRemovingItems, "allowRemovingItems", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (Thing thing in this.innerContainer.InnerListForReading)
				{
					if (!(thing is Apparel) && thing.def.IsWeapon)
					{
						this.holdingWeaponCached = true;
						break;
					}
				}
			}
		}

		// Token: 0x04009BBD RID: 39869
		private static readonly Vector2 baseDrawSize = new Vector2(1.4f, 1.4f);

		// Token: 0x04009BBE RID: 39870
		private static readonly Vector2 bodyDrawSize = new Vector2(1.2f, 1.2f);

		// Token: 0x04009BBF RID: 39871
		private static readonly Vector2 bodyChildDrawSize = new Vector2(1.2f, 1.2f);

		// Token: 0x04009BC0 RID: 39872
		private static readonly Vector2 headDrawSize = new Vector2(1.3f, 1.3f);

		// Token: 0x04009BC1 RID: 39873
		private static bool initializedTextures;

		// Token: 0x04009BC2 RID: 39874
		private static Graphic_Multi baseGraphic;

		// Token: 0x04009BC3 RID: 39875
		private static Graphic_Multi bodyGraphic;

		// Token: 0x04009BC4 RID: 39876
		private static Graphic_Multi bodyGraphicChild;

		// Token: 0x04009BC5 RID: 39877
		private static Graphic_Multi headGraphic;

		// Token: 0x04009BC6 RID: 39878
		private static Texture2D swapOutfitIcon;

		// Token: 0x04009BC7 RID: 39879
		private ThingOwner<Thing> innerContainer;

		// Token: 0x04009BC8 RID: 39880
		private StorageSettings settings;

		// Token: 0x04009BC9 RID: 39881
		private StorageGroup storageGroup;

		// Token: 0x04009BCA RID: 39882
		private bool allowRemovingItems;

		// Token: 0x04009BCB RID: 39883
		private bool cachedApparelRenderInfoSkipHead;

		// Token: 0x04009BCC RID: 39884
		private readonly List<Building_OutfitStand.CachedGraphicRenderInfo> cachedApparelGraphicsHeadgear = new List<Building_OutfitStand.CachedGraphicRenderInfo>();

		// Token: 0x04009BCD RID: 39885
		private readonly List<Building_OutfitStand.CachedGraphicRenderInfo> cachedApparelGraphicsNonHeadgear = new List<Building_OutfitStand.CachedGraphicRenderInfo>();

		// Token: 0x04009BCE RID: 39886
		private Building_OutfitStand.CachedGraphicRenderInfo? cachedHeldWeaponGraphic;

		// Token: 0x04009BCF RID: 39887
		private bool holdingWeaponCached;

		// Token: 0x04009BD0 RID: 39888
		private float cachedBeauty = -1f;

		// Token: 0x02002992 RID: 10642
		private struct CachedGraphicRenderInfo
		{
			// Token: 0x0600EA78 RID: 60024 RVA: 0x00443A85 File Offset: 0x00441C85
			public CachedGraphicRenderInfo(Graphic graphic, int layer, Vector3 scale, Vector3 positionOffset)
			{
				this.graphic = graphic;
				this.layer = layer;
				this.scale = scale;
				this.positionOffset = positionOffset;
			}

			// Token: 0x04009BD2 RID: 39890
			public Graphic graphic;

			// Token: 0x04009BD3 RID: 39891
			public int layer;

			// Token: 0x04009BD4 RID: 39892
			public Vector3 scale;

			// Token: 0x04009BD5 RID: 39893
			public Vector3 positionOffset;
		}
	}
}
