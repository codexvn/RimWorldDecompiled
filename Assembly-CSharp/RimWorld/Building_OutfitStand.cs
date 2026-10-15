using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_OutfitStand : Building, IThingHolderEvents<Thing>, IStorageGroupMember, IHaulEnroute, ILoadReferenceable, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, IApparelSource, ISearchableContents, IBeautyContainer
{
	private struct CachedGraphicRenderInfo
	{
		public Graphic graphic;

		public int layer;

		public Vector3 scale;

		public Vector3 positionOffset;

		public CachedGraphicRenderInfo(Graphic graphic, int layer, Vector3 scale, Vector3 positionOffset)
		{
			this.graphic = graphic;
			this.layer = layer;
			this.scale = scale;
			this.positionOffset = positionOffset;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass90_0
	{
		public Building_OutfitStand _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.SetAllowHauling(allow: false);
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseOutfitStand, _003C_003E4__this), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__90 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_OutfitStand _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass90_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		private List<Thing>.Enumerator _003C_003E7__wrap2;

		private Apparel _003Cap_003E5__4;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetFloatMenuOptions_003Ed__90(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 5:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Thing>.Enumerator);
			_003Cap_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_OutfitStand building_OutfitStand = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass90_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					if (!_003C_003E8__1.selPawn.IsColonistPlayerControlled)
					{
						return false;
					}
					if (!_003C_003E8__1.selPawn.CanReach(building_OutfitStand, PathEndMode.InteractionCell, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotSwapOutfit".Translate().CapitalizeFirst() + ": " + "NoPath".Translate(), null);
						_003C_003E1__state = 1;
						return true;
					}
					if (!building_OutfitStand.innerContainer.Any)
					{
						_003C_003E2__current = new FloatMenuOption("CannotSwapOutfit".Translate().CapitalizeFirst() + ": " + "OutfitStandEmpty".Translate(), null);
						_003C_003E1__state = 2;
						return true;
					}
					FloatMenuOption option = new FloatMenuOption("SwapOutfit".Translate().CapitalizeFirst(), delegate
					{
						_003C_003E8__1._003C_003E4__this.SetAllowHauling(allow: false);
						_003C_003E8__1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseOutfitStand, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
					});
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(option, _003C_003E8__1.selPawn, building_OutfitStand);
					_003C_003E1__state = 3;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -1;
					return false;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = HaulSourceUtility.GetFloatMenuOptions(building_OutfitStand, _003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0240;
				case 4:
					_003C_003E1__state = -3;
					goto IL_0240;
				case 5:
				{
					_003C_003E1__state = -4;
					FloatMenuOption floatMenuOptionForForceWear = building_OutfitStand.GetFloatMenuOptionForForceWear(_003C_003E8__1.selPawn, _003Cap_003E5__4);
					_003C_003E2__current = floatMenuOptionForForceWear;
					_003C_003E1__state = 6;
					return true;
				}
				case 6:
					_003C_003E1__state = -4;
					_003Cap_003E5__4 = null;
					goto IL_0311;
				case 7:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0240:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = building_OutfitStand.innerContainer.InnerListForReading.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0311;
					IL_0311:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap2.Current;
						_003Cap_003E5__4 = current2 as Apparel;
						if (_003Cap_003E5__4 != null)
						{
							FloatMenuOption floatMenuOptionToWear = building_OutfitStand.GetFloatMenuOptionToWear(_003C_003E8__1.selPawn, _003Cap_003E5__4);
							_003C_003E2__current = floatMenuOptionToWear;
							_003C_003E1__state = 5;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Thing>.Enumerator);
					if (building_OutfitStand.holdingWeaponCached)
					{
						_003C_003E2__current = building_OutfitStand.GetFloatMenuOptionToEquipWeapon(_003C_003E8__1.selPawn, building_OutfitStand.HeldWeapon);
						_003C_003E1__state = 7;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__90 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__90(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__97 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_OutfitStand _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__97(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_OutfitStand CS_0024_003C_003E8__locals15 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals15).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00e3;
				case 3:
					_003C_003E1__state = -5;
					goto IL_0153;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandAllowRemovingApparel".Translate(),
						defaultDesc = "CommandAllowRemovingApparelDesc".Translate(),
						hotKey = KeyBindingDefOf.Command_ItemForbid,
						icon = TexCommand.ForbidOff,
						isActive = () => CS_0024_003C_003E8__locals15.allowRemovingItems,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals15.SetAllowHauling(!CS_0024_003C_003E8__locals15.allowRemovingItems);
						}
					};
					_003C_003E1__state = 5;
					return true;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(CS_0024_003C_003E8__locals15.GetStoreSettings()).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e3;
					IL_00e3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals15.StorageTabVisible && CS_0024_003C_003E8__locals15.MapHeld != null)
					{
						_003C_003E7__wrap1 = StorageGroupUtility.StorageGroupMemberGizmos(CS_0024_003C_003E8__locals15).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0153;
					}
					goto IL_016d;
					IL_0153:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_016d;
					IL_016d:
					if (CS_0024_003C_003E8__locals15.Faction != Faction.OfPlayer)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "SwapOutfitGizmo".Translate().CapitalizeFirst(),
						defaultDesc = "SwapOutfitDesc".Translate() + "\n\n" + CS_0024_003C_003E8__locals15.GetContentsString(),
						icon = swapOutfitIcon,
						Disabled = !CS_0024_003C_003E8__locals15.innerContainer.Any,
						disabledReason = ((!CS_0024_003C_003E8__locals15.innerContainer.Any) ? "OutfitStandEmpty".Translate().CapitalizeFirst() : ((TaggedString)null)),
						action = delegate
						{
							Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo t)
							{
								if (t.TryGetPawn(out var pawn))
								{
									if (pawn.Downed)
									{
										Messages.Message("IsIncapped".Translate(pawn.LabelShort, pawn), MessageTypeDefOf.RejectInput, historical: false);
									}
									else if (pawn.CanReserveAndReach(CS_0024_003C_003E8__locals15, PathEndMode.InteractionCell, Danger.Deadly))
									{
										CS_0024_003C_003E8__locals15.SetAllowHauling(allow: false);
										pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.UseOutfitStand, CS_0024_003C_003E8__locals15), JobTag.Misc);
									}
								}
							});
						}
					};
					_003C_003E1__state = 4;
					return true;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__97 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__97(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private static readonly Vector2 baseDrawSize = new Vector2(1.4f, 1.4f);

	private static readonly Vector2 bodyDrawSize = new Vector2(1.2f, 1.2f);

	private static readonly Vector2 bodyChildDrawSize = new Vector2(1.2f, 1.2f);

	private static readonly Vector2 headDrawSize = new Vector2(1.3f, 1.3f);

	private static bool initializedTextures;

	private static Graphic_Multi baseGraphic;

	private static Graphic_Multi bodyGraphic;

	private static Graphic_Multi bodyGraphicChild;

	private static Graphic_Multi headGraphic;

	private static Texture2D swapOutfitIcon;

	private ThingOwner<Thing> innerContainer;

	private StorageSettings settings;

	private StorageGroup storageGroup;

	private bool allowRemovingItems;

	private bool cachedApparelRenderInfoSkipHead;

	private readonly List<CachedGraphicRenderInfo> cachedApparelGraphicsHeadgear = new List<CachedGraphicRenderInfo>();

	private readonly List<CachedGraphicRenderInfo> cachedApparelGraphicsNonHeadgear = new List<CachedGraphicRenderInfo>();

	private CachedGraphicRenderInfo? cachedHeldWeaponGraphic;

	private bool holdingWeaponCached;

	private float cachedBeauty = -1f;

	StorageGroup IStorageGroupMember.Group
	{
		get
		{
			return storageGroup;
		}
		set
		{
			storageGroup = value;
		}
	}

	public StorageSettings StoreSettings => storageGroup?.GetStoreSettings() ?? settings;

	StorageSettings IStorageGroupMember.ParentStoreSettings => def.building.fixedStorageSettings;

	StorageSettings IStorageGroupMember.ThingStoreSettings => settings;

	string IStorageGroupMember.StorageGroupTag => def.building.storageGroupTag;

	bool IStorageGroupMember.DrawConnectionOverlay => base.Spawned;

	bool IStorageGroupMember.DrawStorageTab => true;

	bool IStorageGroupMember.ShowRenameButton => base.Faction == Faction.OfPlayer;

	public bool StorageTabVisible => true;

	bool IHaulDestination.HaulDestinationEnabled => true;

	bool IHaulSource.HaulSourceEnabled => allowRemovingItems;

	bool IApparelSource.ApparelSourceEnabled => allowRemovingItems;

	ThingOwner ISearchableContents.SearchableContents => innerContainer;

	public float BeautyOffset
	{
		get
		{
			if (cachedBeauty < 0f)
			{
				bool outdoors = this.GetRoom().PsychologicallyOutdoors;
				cachedBeauty = HeldItems.Aggregate(0f, (float beauty, Thing apparel) => beauty + apparel.GetBeauty(outdoors));
			}
			return cachedBeauty - 0.001f;
		}
	}

	string IBeautyContainer.BeautyOffsetExplanation => $"{"ContainedApparelBeauty".Translate()}: {BeautyOffset.ToStringWithSign()}";

	protected virtual BodyTypeDef BodyTypeDefForRendering { get; } = BodyTypeDefOf.Male;

	protected virtual float WeaponDrawDistanceFactor => 1f;

	public ThingWithComps HeldWeapon
	{
		get
		{
			if (!holdingWeaponCached)
			{
				return null;
			}
			return innerContainer.InnerListForReading.FirstOrDefault((Thing t) => t.def.IsWeapon) as ThingWithComps;
		}
	}

	public IReadOnlyList<Thing> HeldItems => innerContainer.InnerListForReading;

	private static void InitGraphics()
	{
		if (!initializedTextures)
		{
			initializedTextures = true;
			baseGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Base", ShaderDatabase.Cutout, baseDrawSize, Color.white);
			bodyGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Body", ShaderDatabase.Cutout, bodyDrawSize, Color.white);
			bodyGraphicChild = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_BodyChild", ShaderDatabase.Cutout, bodyChildDrawSize, Color.white);
			headGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>("Things/Building/OutfitStand/OutfitStand_Head", ShaderDatabase.Cutout, headDrawSize, Color.white);
			swapOutfitIcon = ContentFinder<Texture2D>.Get("UI/Commands/SwapOutfits");
		}
	}

	public StorageSettings GetStoreSettings()
	{
		return storageGroup?.GetStoreSettings() ?? settings;
	}

	StorageSettings IStoreSettingsParent.GetParentStoreSettings()
	{
		return def.building.fixedStorageSettings;
	}

	void IStoreSettingsParent.Notify_SettingsChanged()
	{
		if (base.Spawned)
		{
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}
	}

	int IHaulEnroute.SpaceRemainingFor(ThingDef def)
	{
		if (def.IsApparel)
		{
			if (!GetStoreSettings().AllowedToAccept(def))
			{
				return 0;
			}
			return HasRoomForApparelOfDef(def) ? 1 : 0;
		}
		if (def.IsWeapon)
		{
			if (holdingWeaponCached)
			{
				return 0;
			}
			return GetStoreSettings().AllowedToAccept(def) ? 1 : 0;
		}
		return 0;
	}

	bool IHaulDestination.Accepts(Thing t)
	{
		if (t is Apparel)
		{
			if (!GetStoreSettings().AllowedToAccept(t))
			{
				return false;
			}
			if (innerContainer.Contains(t))
			{
				return true;
			}
			return HasRoomForApparelOfDef(t.def);
		}
		if (t.def.IsWeapon)
		{
			if (!GetStoreSettings().AllowedToAccept(t))
			{
				return false;
			}
			if (holdingWeaponCached)
			{
				return innerContainer.Contains(t);
			}
			return true;
		}
		return false;
	}

	void IThingHolder.GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, innerContainer);
	}

	ThingOwner IThingHolder.GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void Notify_ItemAdded(Thing thing)
	{
		cachedBeauty = -1f;
		if (base.Spawned)
		{
			base.MapHeld.listerHaulables.Notify_AddedThing(thing);
			DirtyRoomStats();
		}
		if (!(thing is Apparel) && thing.def.IsWeapon)
		{
			holdingWeaponCached = true;
		}
		RecacheGraphics();
	}

	public void Notify_ItemRemoved(Thing thing)
	{
		cachedBeauty = -1f;
		if (base.Spawned)
		{
			base.MapHeld.listerHaulables.Notify_DeSpawned(thing);
			DirtyRoomStats();
		}
		if (holdingWeaponCached && !(thing is Apparel) && thing.def.IsWeapon)
		{
			holdingWeaponCached = false;
		}
		RecacheGraphics();
	}

	public Building_OutfitStand()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public bool AddApparel(Apparel apparel)
	{
		return innerContainer.TryAdd(apparel);
	}

	public bool RemoveApparel(Apparel apparel)
	{
		return innerContainer.Remove(apparel);
	}

	public bool RemoveHeldWeapon(Thing weapon)
	{
		if (!holdingWeaponCached)
		{
			return false;
		}
		if (HeldWeapon != weapon)
		{
			return false;
		}
		return innerContainer.Remove(weapon);
	}

	public bool TryAddHeldWeapon(Thing weapon)
	{
		if (holdingWeaponCached)
		{
			return false;
		}
		if (!innerContainer.TryAdd(weapon))
		{
			return false;
		}
		holdingWeaponCached = true;
		return true;
	}

	public bool TryDrop(Thing thing, IntVec3 cell, ThingPlaceMode mode, int count, out Thing dropped)
	{
		if (innerContainer.TryDrop(thing, cell, base.Map, mode, count, out dropped))
		{
			return true;
		}
		dropped = null;
		return false;
	}

	private void DirtyRoomStats()
	{
		this.GetRoom()?.Notify_ContainedThingSpawnedOrDespawned(this);
	}

	public bool CanEverStoreThing(Thing t)
	{
		return def.building.fixedStorageSettings.AllowedToAccept(t);
	}

	public bool HasRoomForApparelOfDef(ThingDef t)
	{
		foreach (Thing item in innerContainer)
		{
			if (item is Apparel apparel && !ApparelUtility.CanWearTogether(t, apparel.def, BodyDefOf.Human))
			{
				return false;
			}
		}
		return true;
	}

	public bool TryDropThingsToMakeRoomForThingOfDef(ThingDef t)
	{
		if (HasRoomForApparelOfDef(t))
		{
			return true;
		}
		List<Apparel> list = (from ap in innerContainer.InnerListForReading.OfType<Apparel>()
			where !ApparelUtility.CanWearTogether(t, ap.def, BodyDefOf.Human)
			select ap).ToList();
		if (list.NullOrEmpty())
		{
			return true;
		}
		foreach (Apparel item in list)
		{
			if (!innerContainer.TryDrop(item, ThingPlaceMode.Near, out var _))
			{
				return false;
			}
		}
		return true;
	}

	public override void PostMake()
	{
		base.PostMake();
		settings = new StorageSettings(this);
		if (def.building.defaultStorageSettings != null)
		{
			settings.CopyFrom(def.building.defaultStorageSettings);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (storageGroup != null && map != storageGroup.Map)
		{
			StorageSettings storeSettings = storageGroup.GetStoreSettings();
			storageGroup.RemoveMember(this);
			storageGroup = null;
			settings.CopyFrom(storeSettings);
		}
		LongEventHandler.ExecuteWhenFinished(RecacheGraphics);
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode > DestroyMode.WillReplace)
		{
			innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near);
		}
		base.DeSpawn(mode);
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		base.Destroy(mode);
		if (storageGroup != null)
		{
			storageGroup?.RemoveMember(this);
			storageGroup = null;
		}
	}

	public override void Notify_MinifiedThingAboutToBeDestroyed(DestroyMode mode)
	{
		if (mode > DestroyMode.WillReplace)
		{
			innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near);
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (Find.Selector.SingleSelectedThing == this)
		{
			Room room = this.GetRoom();
			if (room != null && room.ProperRoom)
			{
				room.DrawFieldEdges();
			}
		}
		StorageGroupUtility.DrawSelectionOverlaysFor(this);
	}

	private void RecacheGraphics()
	{
		cachedApparelGraphicsHeadgear.Clear();
		cachedApparelGraphicsNonHeadgear.Clear();
		cachedApparelRenderInfoSkipHead = false;
		List<Apparel> list = innerContainer.InnerListForReading.OfType<Apparel>().ToList();
		list.SortBy((Apparel a) => a.def.apparel.LastLayer.drawOrder);
		Dictionary<ApparelLayerDef, int> dictionary = new Dictionary<ApparelLayerDef, int>();
		foreach (Apparel item in list)
		{
			ApparelLayerDef lastLayer = item.def.apparel.LastLayer;
			bool flag = lastLayer == ApparelLayerDefOf.Overhead || lastLayer == ApparelLayerDefOf.EyeCover;
			cachedApparelRenderInfoSkipHead = cachedApparelRenderInfoSkipHead || item.def.apparel.renderSkipFlags.NotNullAndContains(RenderSkipFlagDefOf.Head);
			if (!ApparelGraphicRecordGetter.TryGetGraphicApparel(item, BodyTypeDefForRendering, forStatue: false, out var rec))
			{
				continue;
			}
			int valueOrDefault = dictionary.GetValueOrDefault(lastLayer, 0);
			dictionary[lastLayer] = valueOrDefault + 1;
			int num = lastLayer.drawOrder / 10 + valueOrDefault;
			float? num2 = item.def.apparel.drawData?.LayerForRot(base.Rotation, -1f);
			if (num2.HasValue)
			{
				float valueOrDefault2 = num2.GetValueOrDefault();
				if (valueOrDefault2 > 0f)
				{
					int num3 = (flag ? 70 : 20);
					int num4 = (int)valueOrDefault2 - num3;
					num += num4 * 2;
				}
			}
			Vector3 one = Vector3.one;
			Vector3 zero = Vector3.zero;
			if (item.RenderAsPack())
			{
				Vector2 vector = item.def.apparel.wornGraphicData.BeltScaleAt(base.Rotation, BodyTypeDefForRendering);
				one.x *= vector.x;
				one.z *= vector.y;
				Vector2 vector2 = item.def.apparel.wornGraphicData.BeltOffsetAt(base.Rotation, BodyTypeDefForRendering);
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
				cachedApparelGraphicsHeadgear.Add(new CachedGraphicRenderInfo(rec.graphic, num, one, zero));
			}
			else
			{
				cachedApparelGraphicsNonHeadgear.Add(new CachedGraphicRenderInfo(rec.graphic, num, one, zero));
			}
		}
	}

	private Vector3 HeadOffsetAt(Rot4 rotation)
	{
		Vector2 headOffset = BodyTypeDefForRendering.headOffset;
		return rotation.AsInt switch
		{
			0 => new Vector3(0f, 0f, headOffset.y), 
			1 => new Vector3(headOffset.x, 0f, headOffset.y), 
			2 => new Vector3(0f, 0f, headOffset.y), 
			3 => new Vector3(0f - headOffset.x, 0f, headOffset.y), 
			_ => Vector3.zero, 
		};
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		if (!initializedTextures)
		{
			InitGraphics();
		}
		bool num = BodyTypeDefForRendering == BodyTypeDefOf.Child;
		Graphic coloredVersion = baseGraphic.GetColoredVersion(baseGraphic.Shader, DrawColor, DrawColorTwo);
		Vector3 loc = (num ? new Vector3(drawLoc.x, drawLoc.y, drawLoc.z + 0.2f) : drawLoc);
		coloredVersion.Draw(loc, flip ? base.Rotation.Opposite : base.Rotation, this);
		Graphic_Multi obj = (num ? bodyGraphicChild : bodyGraphic);
		obj.GetColoredVersion(obj.Shader, DrawColor, DrawColorTwo).Draw(drawLoc.WithYOffset(0.05f), flip ? base.Rotation.Opposite : base.Rotation, this);
		Rot4 rot = (flip ? base.Rotation.Opposite : base.Rotation);
		Mesh mesh = MeshPool.GetMeshSetForSize(1.5f, 1.5f).MeshAt(rot);
		foreach (CachedGraphicRenderInfo item in cachedApparelGraphicsNonHeadgear)
		{
			Graphic graphic = item.graphic;
			Vector3 vector = drawLoc;
			vector.y = AltitudeLayer.Item.AltitudeFor() + PawnRenderUtility.AltitudeForLayer(item.layer);
			Material material = graphic.MatAt(rot);
			Vector3 vector2 = graphic.DrawOffset(rot) + item.positionOffset;
			Quaternion q = graphic.QuatFromRot(rot);
			Matrix4x4 matrix = Matrix4x4.TRS(vector + vector2, q, item.scale);
			Graphics.DrawMesh(mesh, matrix, material, 0);
		}
		if (!cachedApparelRenderInfoSkipHead)
		{
			foreach (CachedGraphicRenderInfo item2 in cachedApparelGraphicsHeadgear)
			{
				Vector3 vector3 = drawLoc;
				vector3 += HeadOffsetAt(rot);
				vector3.y = AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer(item2.layer);
				Graphic graphic2 = item2.graphic;
				Material material2 = graphic2.MatAt(rot);
				Vector3 vector4 = graphic2.DrawOffset(rot);
				Quaternion q2 = graphic2.QuatFromRot(rot);
				Matrix4x4 matrix2 = Matrix4x4.TRS(vector3 + vector4, q2, item2.scale);
				Graphics.DrawMesh(mesh, matrix2, material2, 0);
			}
			Graphic coloredVersion2 = headGraphic.GetColoredVersion(headGraphic.Shader, DrawColor, DrawColorTwo);
			Vector3 v = drawLoc + HeadOffsetAt(rot);
			v = v.WithY(AltitudeLayer.ItemImportant.AltitudeFor() - 0.05f);
			coloredVersion2.Draw(v, flip ? base.Rotation.Opposite : base.Rotation, this);
		}
		if (holdingWeaponCached)
		{
			PawnRenderUtility.DrawCarriedWeapon(drawPos: (!((flip ? base.Rotation.Opposite : base.Rotation) == Rot4.North)) ? drawLoc.WithY(AltitudeLayer.ItemImportant.AltitudeFor() + PawnRenderUtility.AltitudeForLayer(90f)) : drawLoc.WithYOffset(-0.05f), weapon: HeldWeapon, facing: flip ? base.Rotation.Opposite : base.Rotation, equipmentDrawDistanceFactor: WeaponDrawDistanceFactor);
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__90))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__90(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	private FloatMenuOption GetFloatMenuOptionToWear(Pawn selPawn, Apparel apparel)
	{
		string key = "CannotWear";
		string key2 = "ForceWear";
		if (apparel.def.apparel.LastLayer.IsUtilityLayer)
		{
			key = "CannotEquipApparel";
			key2 = "ForceEquipApparel";
		}
		if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white);
		}
		if (apparel.IsBurning())
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white);
		}
		if (selPawn.apparel.WouldReplaceLockedApparel(apparel))
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, apparel, Color.white);
		}
		if (selPawn.IsMutant && selPawn.mutant.Def.disableApparel)
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + selPawn.mutant.Def.LabelCap, null, apparel, Color.white);
		}
		if (!ApparelUtility.HasPartsToWear(selPawn, apparel.def))
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), null, apparel, Color.white);
		}
		if (!EquipmentUtility.CanEquip(apparel, selPawn, out var cantReason))
		{
			return new FloatMenuOption(key.Translate(apparel.Label, apparel) + ": " + cantReason, null, apparel, Color.white);
		}
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key2.Translate(apparel.LabelShort, apparel), delegate
		{
			Action action = delegate
			{
				selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wear, apparel), JobTag.Misc);
			};
			Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(selPawn, apparel);
			if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(selPawn, apparelReplacedByNewApparel, action))
			{
				action();
			}
		}, apparel, Color.white), selPawn, this);
	}

	private FloatMenuOption GetFloatMenuOptionForForceWear(Pawn selPawn, Apparel apparel)
	{
		string cannotForceTargetText = "CannotForceTargetToWear";
		string key = "ForceTargetToWear";
		if (apparel.def.apparel.LastLayer.IsUtilityLayer)
		{
			cannotForceTargetText = "CannotForceTargetToEquipApparel";
			key = "ForceTargetToEquipApparel";
		}
		if (!selPawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "NoPath".Translate().CapitalizeFirst(), null, apparel, Color.white);
		}
		if (apparel.IsBurning())
		{
			return new FloatMenuOption(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, apparel, Color.white);
		}
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(key.Translate(apparel.LabelShort, apparel), delegate
		{
			bool queueOrder = KeyBindingDefOf.QueueOrder.IsDownEvent;
			Find.Targeter.BeginTargeting(TargetingParameters.ForForceWear(selPawn), delegate(LocalTargetInfo target)
			{
				string cantReason;
				if (!target.TryGetPawn(out var targetPawn))
				{
					if (ModsConfig.OdysseyActive && target.Thing is Building_OutfitStand building_OutfitStand)
					{
						if (!building_OutfitStand.CanEverStoreThing(apparel))
						{
							Messages.Message("CannotStoreThingOnTarget".Translate(apparel.Named("THING"), building_OutfitStand.Named("TARGET")), MessageTypeDefOf.RejectInput, historical: false);
						}
						else
						{
							selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.PutApparelOnOutfitStand, apparel, building_OutfitStand), requestQueueing: queueOrder, tag: JobTag.Misc);
						}
					}
				}
				else if (targetPawn.apparel.WouldReplaceLockedApparel(apparel))
				{
					Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, historical: false);
				}
				else if (targetPawn.IsMutant && targetPawn.mutant.Def.disableApparel)
				{
					Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + targetPawn.mutant.Def.LabelCap, targetPawn, MessageTypeDefOf.RejectInput, historical: false);
				}
				else if (!ApparelUtility.HasPartsToWear(targetPawn, apparel.def))
				{
					Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate().CapitalizeFirst(), targetPawn, MessageTypeDefOf.RejectInput, historical: false);
				}
				else if (!EquipmentUtility.CanEquip(apparel, targetPawn, out cantReason))
				{
					Messages.Message(cannotForceTargetText.Translate(apparel.Label, apparel) + ": " + cantReason, targetPawn, MessageTypeDefOf.RejectInput, historical: false);
				}
				else
				{
					Action action = delegate
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ForceTargetWear, targetPawn, apparel), requestQueueing: queueOrder, tag: JobTag.Misc);
					};
					Apparel apparelReplacedByNewApparel = ApparelUtility.GetApparelReplacedByNewApparel(targetPawn, apparel);
					if (apparelReplacedByNewApparel == null || !ModsConfig.BiotechActive || !MechanitorUtility.TryConfirmBandwidthLossFromDroppingThing(targetPawn, apparelReplacedByNewApparel, action))
					{
						action();
					}
				}
			});
		}, apparel, Color.white), selPawn, this);
	}

	private FloatMenuOption GetFloatMenuOptionToEquipWeapon(Pawn selPawn, Thing weapon)
	{
		if (!weapon.HasComp<CompEquippable>())
		{
			return null;
		}
		string labelShort = weapon.LabelShort;
		if (weapon.def.IsWeapon && selPawn.WorkTagIsDisabled(WorkTags.Violent))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(selPawn.LabelShort, selPawn), null, weapon, Color.white);
		}
		if (weapon.def.IsRangedWeapon && selPawn.WorkTagIsDisabled(WorkTags.Shooting))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfShootingLower".Translate(selPawn), null, weapon, Color.white);
		}
		if (!selPawn.CanReach(weapon, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null, weapon, Color.white);
		}
		if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate().CapitalizeFirst(), null, weapon, Color.white);
		}
		if (weapon.IsBurning())
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, weapon, Color.white);
		}
		if (selPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(weapon, selPawn))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, weapon, Color.white);
		}
		if (!EquipmentUtility.CanEquip(weapon, selPawn, out var cantReason, checkBonded: false))
		{
			return new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + cantReason.CapitalizeFirst(), null, weapon, Color.white);
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
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
			{
				Find.WindowStack.Add(new Dialog_MessageBox(dialogText));
			}, weapon, Color.white, MenuOptionPriority.High), selPawn, weapon);
		}
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
		{
			string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(weapon, selPawn);
			if (!personaWeaponConfirmationText.NullOrEmpty())
			{
				Find.WindowStack.Add(new Dialog_MessageBox(personaWeaponConfirmationText, "Yes".Translate(), delegate
				{
					Equip();
				}, "No".Translate()));
			}
			else
			{
				Equip();
			}
		}, weapon, Color.white), selPawn, this);
		void Equip()
		{
			weapon.SetForbidden(value: false);
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Equip, weapon), JobTag.Misc);
			FleckMaker.Static(weapon.PositionHeld, weapon.MapHeld, FleckDefOf.FeedbackEquip);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EquippingWeapons, KnowledgeAmount.Total);
		}
	}

	private void SetAllowHauling(bool allow)
	{
		if (allowRemovingItems != allow)
		{
			allowRemovingItems = allow;
			base.Map.listerHaulables.RecalculateAllInHaulSource(this);
		}
	}

	private string GetContentsString()
	{
		return "Contents".Translate() + ": " + innerContainer.InnerListForReading.Select((Thing a) => a.LabelNoParenthesis).ToCommaList().CapitalizeFirst();
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString() ?? "";
		if (storageGroup != null)
		{
			text += $"{"StorageGroupLabel".Translate()}: {storageGroup.RenamableLabel.CapitalizeFirst()} ";
			text = ((storageGroup.MemberCount <= 1) ? ((string)(text + ("(" + "OneBuilding".Translate() + ")"))) : ((string)(text + ("(" + "NumBuildings".Translate(storageGroup.MemberCount).CapitalizeFirst() + ")"))));
		}
		if (!innerContainer.Any)
		{
			return text;
		}
		if (text.Length > 0)
		{
			text += "\n";
		}
		return text + GetContentsString();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__97))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__97(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Deep.Look(ref settings, "settings", this);
		Scribe_References.Look(ref storageGroup, "storageGroup");
		Scribe_Values.Look(ref allowRemovingItems, "allowRemovingItems", defaultValue: false);
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		foreach (Thing item in innerContainer.InnerListForReading)
		{
			if (!(item is Apparel) && item.def.IsWeapon)
			{
				holdingWeaponCached = true;
				break;
			}
		}
	}
}
