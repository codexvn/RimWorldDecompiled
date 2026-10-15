using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ContainingSelectionUtility
{
	[CompilerGenerated]
	private sealed class _003CSelectableContainedThings_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing container;

		public Thing _003C_003E3__container;

		private IEnumerator<Thing> _003C_003E7__wrap1;

		private List<ThingComp>.Enumerator _003C_003E7__wrap2;

		Thing IEnumerator<Thing>.Current
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
		public _003CSelectableContainedThings_003Ed__2(int _003C_003E1__state)
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
			case 2:
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
			case 3:
				try
				{
					if (num == -5 || num == 3)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<ThingComp>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				IThingHolder thingHolder2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (container is Pawn pawn)
					{
						Thing thing = pawn.carryTracker?.CarriedThing;
						if (thing != null && CanSelect(thing, container))
						{
							_003C_003E2__current = thing;
							_003C_003E1__state = 1;
							return true;
						}
						break;
					}
					if (container is MinifiedThing || !container.Faction.IsPlayerSafe())
					{
						break;
					}
					if (container is IThingHolder thingHolder)
					{
						_003C_003E7__wrap1 = ((IEnumerable<Thing>)thingHolder.GetDirectlyHeldThings()).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0111;
					}
					if (!(container is ThingWithComps thingWithComps))
					{
						break;
					}
					_003C_003E7__wrap2 = thingWithComps.AllComps.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01e6;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0111;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_01cc;
					}
					IL_0179:
					_003C_003E7__wrap1 = ((IEnumerable<Thing>)thingHolder2.GetDirectlyHeldThings()).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01cc;
					IL_01e6:
					while (_003C_003E7__wrap2.MoveNext())
					{
						thingHolder2 = _003C_003E7__wrap2.Current as IThingHolder;
						if (thingHolder2 == null)
						{
							continue;
						}
						goto IL_0179;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<ThingComp>.Enumerator);
					break;
					IL_0111:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Thing current = _003C_003E7__wrap1.Current;
						if (CanSelect(current, container))
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_01cc:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap1.Current;
						if (CanSelect(current2, container))
						{
							_003C_003E2__current = current2;
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_01e6;
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

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CSelectableContainedThings_003Ed__2 _003CSelectableContainedThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSelectableContainedThings_003Ed__ = this;
			}
			else
			{
				_003CSelectableContainedThings_003Ed__ = new _003CSelectableContainedThings_003Ed__2(0);
			}
			_003CSelectableContainedThings_003Ed__.container = _003C_003E3__container;
			return _003CSelectableContainedThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private static readonly Dictionary<string, TaggedString> labelCache = new Dictionary<string, TaggedString>();

	public static Gizmo SelectCarriedThingGizmo(Thing container, Thing carriedThing)
	{
		if (container == null || carriedThing == null)
		{
			return null;
		}
		if (!CanSelect(carriedThing, container))
		{
			return null;
		}
		string label = GizmoLabel(container, carriedThing);
		string description = GizmoDescription(container, carriedThing);
		return CreateSelectGizmo(label, description, carriedThing, carriedThing);
	}

	public static Gizmo SelectContainingThingGizmo(Thing containedThing)
	{
		if (containedThing.ParentHolder is Corpse)
		{
			return null;
		}
		if (containedThing.ParentHolder is Pawn_CarryTracker pawn_CarryTracker)
		{
			return CreateSelectGizmo("CommandSelectCarryingPawn", "CommandSelectCarryingPawnDesc", pawn_CarryTracker.pawn);
		}
		if (containedThing.ParentHolder is Thing thingToSelect)
		{
			return CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thingToSelect);
		}
		if (containedThing.ParentHolder is ThingComp thingComp)
		{
			return CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thingComp.parent);
		}
		if (containedThing.Spawned && containedThing.def.category == ThingCategory.Item)
		{
			List<Thing> thingList = containedThing.Position.GetThingList(containedThing.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Building_Storage building_Storage)
				{
					return CreateSelectStorageGizmo("CommandSelectContainerThing".Translate(building_Storage), "CommandSelectContainerThingDesc".Translate(), building_Storage, building_Storage);
				}
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CSelectableContainedThings_003Ed__2))]
	public static IEnumerable<Thing> SelectableContainedThings(Thing container)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSelectableContainedThings_003Ed__2(-2)
		{
			_003C_003E3__container = container
		};
	}

	private static bool CanSelect(Thing carriedThing, Thing container)
	{
		if (!carriedThing.def.selectable)
		{
			return false;
		}
		if (carriedThing is Pawn)
		{
			return container.def.containedPawnsSelectable;
		}
		return container.def.containedItemsSelectable;
	}

	private static Gizmo CreateSelectGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null)
	{
		if (!thingToSelect.def.selectable)
		{
			return null;
		}
		float scale = 1f;
		float angle = 0f;
		Vector2 iconProportions = Vector2.one;
		Color color = Color.white;
		Texture icon;
		if (!(iconThing is Pawn pawn) || !(pawn.def.uiIcon == null))
		{
			icon = ((iconThing == null) ? ((thingToSelect is Pawn) ? TexCommand.SelectCarriedPawn : TexCommand.SelectCarriedThing) : Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), iconThing.def.defaultPlacingRot, stackOfOne: true, out scale, out angle, out iconProportions, out color, out var _));
		}
		else
		{
			Vector2 size = new Vector2(75f, 75f);
			Rot4 south = Rot4.South;
			PawnHealthState? healthStateOverride = PawnHealthState.Mobile;
			icon = PortraitsCache.Get(pawn, size, south, default(Vector3), 1f, supersample: true, compensateForUIScale: true, renderHeadgear: true, renderClothes: true, null, null, stylingStation: false, healthStateOverride);
		}
		return new Command_SelectStorage
		{
			defaultLabel = label.Translate(thingToSelect).TruncateHeight(75f, 37.5f, labelCache),
			defaultDesc = description.Translate(),
			icon = icon,
			iconDrawScale = scale * 0.85f,
			iconAngle = angle,
			defaultIconColor = color,
			iconProportions = iconProportions,
			action = delegate
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thingToSelect);
			}
		};
	}

	public static Gizmo CreateSelectStorageGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null, bool groupable = true)
	{
		if (!thingToSelect.def.selectable)
		{
			return null;
		}
		float scale;
		float angle;
		Vector2 iconProportions;
		Color color;
		Material material;
		Texture iconFor = Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), iconThing.def.defaultPlacingRot, stackOfOne: false, out scale, out angle, out iconProportions, out color, out material);
		Text.Font = GameFont.Tiny;
		Command_SelectStorage result = new Command_SelectStorage
		{
			defaultLabel = label.TruncateHeight(75f, 37.5f),
			defaultDesc = description,
			icon = iconFor,
			iconDrawScale = scale * 0.85f,
			iconAngle = angle,
			defaultIconColor = color,
			iconProportions = iconProportions,
			groupable = groupable,
			groupKey = thingToSelect.thingIDNumber,
			action = delegate
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thingToSelect);
			}
		};
		Text.Font = GameFont.Small;
		return result;
	}

	private static string GizmoLabel(Thing container, Thing carriedThing)
	{
		if (container is Pawn)
		{
			if (carriedThing is Pawn)
			{
				return "CommandSelectCarriedPawn";
			}
			return "CommandSelectCarriedThing";
		}
		if (carriedThing is Pawn)
		{
			return "CommandSelectContainedPawn";
		}
		return "CommandSelectContainedThing";
	}

	private static string GizmoDescription(Thing container, Thing carriedThing)
	{
		if (container is Pawn)
		{
			if (carriedThing is Pawn)
			{
				return "CommandSelectCarriedPawnDesc";
			}
			return "CommandSelectCarriedThingDesc";
		}
		if (carriedThing is Pawn)
		{
			return "CommandSelectContainedPawnDesc";
		}
		return "CommandSelectContainedThingDesc";
	}
}
