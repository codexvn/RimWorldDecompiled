using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Building_Bookcase : Building, IThingHolderEvents<Book>, IHaulEnroute, ILoadReferenceable, IStorageGroupMember, IHaulDestination, IStoreSettingsParent, IHaulSource, IThingHolder, ISearchableContents, IBeautyContainer
{
	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__73 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Bookcase _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		private IEnumerator<Book> _003C_003E7__wrap2;

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
		public _003CGetFloatMenuOptions_003Ed__73(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
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
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -6:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_Bookcase building_Bookcase = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = HaulSourceUtility.GetFloatMenuOptions(building_Bookcase, selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -5;
					goto IL_010b;
				case 3:
					{
						_003C_003E1__state = -6;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = building_Bookcase.HeldBooks.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0125;
					IL_0125:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Book current2 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap1 = current2.GetFloatMenuOptions(selPawn).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_010b;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					_003C_003E7__wrap1 = ((ThingWithComps)building_Bookcase).GetFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -6;
					break;
					IL_010b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_0125;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current4 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current4;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally4();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__73 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__73(0)
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
	private sealed class _003CGetGizmos_003Ed__72 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Bookcase _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__72(int _003C_003E1__state)
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
				Building_Bookcase CS_0024_003C_003E8__locals8 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals8).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00df;
				case 3:
					_003C_003E1__state = -5;
					goto IL_014c;
				case 4:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(CS_0024_003C_003E8__locals8.GetStoreSettings()).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00df;
					IL_014c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_0166;
					IL_00df:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals8.StorageTabVisible && CS_0024_003C_003E8__locals8.MapHeld != null)
					{
						_003C_003E7__wrap1 = StorageGroupUtility.StorageGroupMemberGizmos(CS_0024_003C_003E8__locals8).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_014c;
					}
					goto IL_0166;
					IL_0166:
					if (!DebugSettings.godMode)
					{
						return false;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Fill",
						action = delegate
						{
							for (int i = CS_0024_003C_003E8__locals8.HeldBooks.Count; i < CS_0024_003C_003E8__locals8.MaximumBooks; i++)
							{
								Book item = BookUtility.MakeBook(ThingDefOf.TextBook, ArtGenerationContext.Outsider, null);
								CS_0024_003C_003E8__locals8.innerContainer.TryAdd(item);
							}
						}
					};
					_003C_003E1__state = 4;
					return true;
				}
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
			_003CGetGizmos_003Ed__72 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__72(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_CellsFilledPercentage_003Ed__23 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Bookcase _003C_003E4__this;

		private int _003Cbooks_003E5__2;

		private int _003Ci_003E5__3;

		float IEnumerator<float>.Current
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
		public _003Cget_CellsFilledPercentage_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Building_Bookcase building_Bookcase = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cbooks_003E5__2 = building_Bookcase.HeldBooks.Count;
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < building_Bookcase.def.size.Area)
			{
				int num2 = Mathf.Min(_003Cbooks_003E5__2, building_Bookcase.def.building.maxItemsInCell);
				_003Cbooks_003E5__2 -= num2;
				_003C_003E2__current = Mathf.Clamp01((float)num2 / (float)building_Bookcase.def.building.maxItemsInCell);
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			_003Cget_CellsFilledPercentage_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_CellsFilledPercentage_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	private ThingOwner<Book> innerContainer;

	private StorageSettings settings;

	private StorageGroup storageGroup;

	private Graphic bookendGraphicEastInt;

	private Graphic bookendGraphicNorthInt;

	private static readonly Vector3 DrawOffset = new Vector3(0f, 0.018292684f, 0f);

	private static readonly Vector3 DrawOffsetBookcaseEnd = new Vector3(0f, 0.07317074f, 0f);

	private const float BookWidthEastWest = 0.16f;

	private const float BookWidthNorthSouth = 0.155f;

	private static readonly Vector3[] StandardBookshelfRotOffsets = new Vector3[4]
	{
		new Vector3(-0.081f, 0f, 0f),
		new Vector3(-0.082f, 0f, 0.02f),
		new Vector3(0.08f, 0f, 0.08f),
		new Vector3(0.082f, 0f, -0.15f)
	};

	private float cachedBeauty = -1f;

	private Graphic BookendGraphicEast => bookendGraphicEastInt ?? (bookendGraphicEastInt = def.building.bookendGraphicEast.GraphicColoredFor(this));

	private Graphic BookendGraphicNorth => bookendGraphicNorthInt ?? (bookendGraphicNorthInt = def.building.bookendGraphicNorth.GraphicColoredFor(this));

	public int MaximumBooks => def.building.maxItemsInCell * def.size.Area;

	public IReadOnlyList<Book> HeldBooks => innerContainer.InnerListForReading;

	public float ReadingBonus => this.GetRoom()?.GetStat(RoomStatDefOf.ReadingBonus) ?? 0f;

	protected virtual Vector3[] RotOffsets => StandardBookshelfRotOffsets;

	public IEnumerable<float> CellsFilledPercentage
	{
		[IteratorStateMachine(typeof(_003Cget_CellsFilledPercentage_003Ed__23))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_CellsFilledPercentage_003Ed__23(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public ThingOwner SearchableContents => innerContainer;

	public bool StorageTabVisible => true;

	public bool HaulSourceEnabled => true;

	public bool HaulDestinationEnabled => true;

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

	bool IStorageGroupMember.DrawConnectionOverlay => base.Spawned;

	Map IStorageGroupMember.Map => base.MapHeld;

	string IStorageGroupMember.StorageGroupTag => def.building.storageGroupTag;

	StorageSettings IStorageGroupMember.StoreSettings => GetStoreSettings();

	StorageSettings IStorageGroupMember.ParentStoreSettings => GetParentStoreSettings();

	StorageSettings IStorageGroupMember.ThingStoreSettings => settings;

	bool IStorageGroupMember.DrawStorageTab => true;

	bool IStorageGroupMember.ShowRenameButton => base.Faction == Faction.OfPlayer;

	public float BeautyOffset
	{
		get
		{
			if (cachedBeauty < 0f)
			{
				cachedBeauty = HeldBooks.Aggregate(0f, (float beauty, Book book) => beauty + book.GetBeauty(this.GetRoom().PsychologicallyOutdoors));
			}
			return cachedBeauty - 0.001f;
		}
	}

	string IBeautyContainer.BeautyOffsetExplanation => $"{"ContainedBookBeauty".Translate()}: {BeautyOffset.ToStringWithSign()}";

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public StorageSettings GetStoreSettings()
	{
		if (storageGroup != null)
		{
			return storageGroup.GetStoreSettings();
		}
		return settings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		return def.building.fixedStorageSettings;
	}

	public bool Accepts(Thing t)
	{
		if (HeldBooks.Count >= MaximumBooks)
		{
			if (!(t is Book item))
			{
				return false;
			}
			if (!innerContainer.InnerListForReading.Contains(item))
			{
				return false;
			}
		}
		if (GetStoreSettings().AllowedToAccept(t))
		{
			return innerContainer.CanAcceptAnyOf(t);
		}
		return false;
	}

	public int SpaceRemainingFor(ThingDef _)
	{
		return MaximumBooks - HeldBooks.Count;
	}

	public void Notify_SettingsChanged()
	{
		if (base.Spawned)
		{
			base.MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
		}
	}

	public void Notify_ItemAdded(Book item)
	{
		DirtyRoomStats();
		cachedBeauty = -1f;
		base.MapHeld.listerHaulables.Notify_AddedThing(item);
	}

	public void Notify_ItemRemoved(Book item)
	{
		DirtyRoomStats();
		cachedBeauty = -1f;
	}

	public Building_Bookcase()
	{
		innerContainer = new ThingOwner<Book>(this, oneStackOnly: false);
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
	}

	public override void PostMake()
	{
		base.PostMake();
		settings = new StorageSettings(this);
		if (def.building.defaultStorageSettings != null)
		{
			settings.CopyFrom(def.building.defaultStorageSettings);
		}
		DirtyRoomStats();
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		drawLoc -= Altitudes.AltIncVect * 2f;
		base.DrawAt(drawLoc, flip);
		Rot4 rot = base.Rotation.Rotated(RotationDirection.Counterclockwise);
		float num = ((base.Rotation == Rot4.North || base.Rotation == Rot4.South) ? 0.155f : 0.16f);
		Vector3 vector = rot.FacingCell.ToVector3() * num;
		Vector3 vector2 = rot.FacingCell.ToVector3() * ((float)(-MaximumBooks) * num * 0.5f);
		Vector3 vector3 = RotOffsets[base.Rotation.AsInt];
		for (int i = 0; i < HeldBooks.Count; i++)
		{
			Book book = HeldBooks[i];
			Rot4 opposite = base.Rotation.Opposite;
			if (opposite == Rot4.East || opposite == Rot4.West)
			{
				opposite = opposite.Opposite;
			}
			Vector3 loc = drawLoc + vector2 + vector3 + DrawOffset + vector * i;
			book.VerticalGraphic.Draw(loc, opposite, this);
		}
		if (base.Rotation != Rot4.South)
		{
			if (base.Rotation != Rot4.North && def.building.bookendGraphicEast != null)
			{
				BookendGraphicEast.Draw(drawLoc + DrawOffsetBookcaseEnd, Rot4.North, this);
			}
			else if (base.Rotation == Rot4.North && def.building.bookendGraphicNorth != null)
			{
				BookendGraphicNorth.Draw(drawLoc + DrawOffsetBookcaseEnd, Rot4.North, this);
			}
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (storageGroup != null)
		{
			storageGroup?.RemoveMember(this);
			storageGroup = null;
		}
		if (mode != DestroyMode.WillReplace)
		{
			innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
		}
		base.DeSpawn(mode);
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

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (base.Spawned)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			if (storageGroup != null)
			{
				text += $"{"StorageGroupLabel".Translate()}: {storageGroup.RenamableLabel.CapitalizeFirst()} ";
				text = ((storageGroup.MemberCount <= 1) ? ((string)(text + ("(" + "OneBuilding".Translate() + ")\n"))) : ((string)(text + ("(" + "NumBuildings".Translate(storageGroup.MemberCount).CapitalizeFirst() + ")\n"))));
			}
			text += $"{"BookshelfStoredInspect".Translate()}: {HeldBooks.Count} / {MaximumBooks}";
			text += $"\n{"BookshelfReadingBonusInspect".Translate()}: {Mathf.Max(ReadingBonus - 1f, 0f).ToStringPercent()}";
			if (this.IsOutside())
			{
				text += $" ({"Outdoors".Translate()})".Colorize(Color.gray).ResolveTags();
			}
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__72))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__72(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__73))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__73(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	private void DirtyRoomStats()
	{
		this.GetRoom()?.Notify_ContainedThingSpawnedOrDespawned(this);
	}

	public override void Notify_ColorChanged()
	{
		bookendGraphicEastInt = null;
		bookendGraphicNorthInt = null;
		base.Notify_ColorChanged();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Deep.Look(ref settings, "settings", this);
		Scribe_References.Look(ref storageGroup, "storageGroup");
	}
}
