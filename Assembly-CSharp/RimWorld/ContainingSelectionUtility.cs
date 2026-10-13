using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003563 RID: 13667
	public static class ContainingSelectionUtility
	{
		// Token: 0x060132A2 RID: 78498 RVA: 0x005B2644 File Offset: 0x005B0844
		public static Gizmo SelectCarriedThingGizmo(Thing container, Thing carriedThing)
		{
			if (container == null || carriedThing == null)
			{
				return null;
			}
			if (!ContainingSelectionUtility.CanSelect(carriedThing, container))
			{
				return null;
			}
			string label = ContainingSelectionUtility.GizmoLabel(container, carriedThing);
			string description = ContainingSelectionUtility.GizmoDescription(container, carriedThing);
			return ContainingSelectionUtility.CreateSelectGizmo(label, description, carriedThing, carriedThing);
		}

		// Token: 0x060132A3 RID: 78499 RVA: 0x005B267C File Offset: 0x005B087C
		public static Gizmo SelectContainingThingGizmo(Thing containedThing)
		{
			if (containedThing.ParentHolder is Corpse)
			{
				return null;
			}
			Pawn_CarryTracker pawn_CarryTracker = containedThing.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectCarryingPawn", "CommandSelectCarryingPawnDesc", pawn_CarryTracker.pawn, null);
			}
			Thing thing = containedThing.ParentHolder as Thing;
			if (thing != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thing, null);
			}
			ThingComp thingComp = containedThing.ParentHolder as ThingComp;
			if (thingComp != null)
			{
				return ContainingSelectionUtility.CreateSelectGizmo("CommandSelectContainerThing", "CommandSelectContainerThingDesc", thingComp.parent, null);
			}
			if (containedThing.Spawned && containedThing.def.category == ThingCategory.Item)
			{
				List<Thing> thingList = containedThing.Position.GetThingList(containedThing.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Building_Storage building_Storage = thingList[i] as Building_Storage;
					if (building_Storage != null)
					{
						return ContainingSelectionUtility.CreateSelectStorageGizmo("CommandSelectContainerThing".Translate(building_Storage), "CommandSelectContainerThingDesc".Translate(), building_Storage, building_Storage, true);
					}
				}
			}
			return null;
		}

		// Token: 0x060132A4 RID: 78500 RVA: 0x005B2786 File Offset: 0x005B0986
		public static IEnumerable<Thing> SelectableContainedThings(Thing container)
		{
			Pawn pawn = container as Pawn;
			if (pawn != null)
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null && ContainingSelectionUtility.CanSelect(thing, container))
				{
					yield return thing;
				}
			}
			else if (!(container is MinifiedThing) && container.Faction.IsPlayerSafe())
			{
				IThingHolder thingHolder = container as IThingHolder;
				if (thingHolder != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)thingHolder.GetDirectlyHeldThings()))
					{
						if (ContainingSelectionUtility.CanSelect(thing2, container))
						{
							yield return thing2;
						}
					}
					IEnumerator<Thing> enumerator = null;
				}
				else
				{
					ThingWithComps thingWithComps = container as ThingWithComps;
					if (thingWithComps != null)
					{
						foreach (ThingComp thingComp in thingWithComps.AllComps)
						{
							IThingHolder thingHolder2 = thingComp as IThingHolder;
							if (thingHolder2 != null)
							{
								foreach (Thing thing3 in ((IEnumerable<Thing>)thingHolder2.GetDirectlyHeldThings()))
								{
									if (ContainingSelectionUtility.CanSelect(thing3, container))
									{
										yield return thing3;
									}
								}
								IEnumerator<Thing> enumerator = null;
							}
						}
						List<ThingComp>.Enumerator enumerator2 = default(List<ThingComp>.Enumerator);
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060132A5 RID: 78501 RVA: 0x005B2796 File Offset: 0x005B0996
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

		// Token: 0x060132A6 RID: 78502 RVA: 0x005B27C8 File Offset: 0x005B09C8
		private static Gizmo CreateSelectGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null)
		{
			if (!thingToSelect.def.selectable)
			{
				return null;
			}
			float num = 1f;
			float iconAngle = 0f;
			Vector2 one = Vector2.one;
			Color white = Color.white;
			Pawn pawn = iconThing as Pawn;
			Texture icon;
			if (pawn != null && pawn.def.uiIcon == null)
			{
				Pawn pawn2 = pawn;
				Vector2 size = new Vector2(75f, 75f);
				Rot4 south = Rot4.South;
				PawnHealthState? healthStateOverride = new PawnHealthState?(PawnHealthState.Mobile);
				icon = PortraitsCache.Get(pawn2, size, south, default(Vector3), 1f, true, true, true, true, null, null, false, healthStateOverride);
			}
			else if (iconThing != null)
			{
				Material material;
				icon = Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), new Rot4?(iconThing.def.defaultPlacingRot), true, out num, out iconAngle, out one, out white, out material);
			}
			else
			{
				icon = ((thingToSelect is Pawn) ? TexCommand.SelectCarriedPawn : TexCommand.SelectCarriedThing);
			}
			return new Command_SelectStorage
			{
				defaultLabel = label.Translate(thingToSelect).TruncateHeight(75f, 37.5f, ContainingSelectionUtility.labelCache),
				defaultDesc = description.Translate(),
				icon = icon,
				iconDrawScale = num * 0.85f,
				iconAngle = iconAngle,
				defaultIconColor = white,
				iconProportions = one,
				action = delegate()
				{
					Find.Selector.ClearSelection();
					Find.Selector.Select(thingToSelect, true, true);
				}
			};
		}

		// Token: 0x060132A7 RID: 78503 RVA: 0x005B294C File Offset: 0x005B0B4C
		public static Gizmo CreateSelectStorageGizmo(string label, string description, Thing thingToSelect, Thing iconThing = null, bool groupable = true)
		{
			if (!thingToSelect.def.selectable)
			{
				return null;
			}
			float num;
			float iconAngle;
			Vector2 iconProportions;
			Color defaultIconColor;
			Material material;
			Texture iconFor = Widgets.GetIconFor(iconThing, new Vector2(75f, 75f), new Rot4?(iconThing.def.defaultPlacingRot), false, out num, out iconAngle, out iconProportions, out defaultIconColor, out material);
			Text.Font = GameFont.Tiny;
			Command_SelectStorage command_SelectStorage = new Command_SelectStorage();
			command_SelectStorage.defaultLabel = label.TruncateHeight(75f, 37.5f, null);
			command_SelectStorage.defaultDesc = description;
			command_SelectStorage.icon = iconFor;
			command_SelectStorage.iconDrawScale = num * 0.85f;
			command_SelectStorage.iconAngle = iconAngle;
			command_SelectStorage.defaultIconColor = defaultIconColor;
			command_SelectStorage.iconProportions = iconProportions;
			command_SelectStorage.groupable = groupable;
			command_SelectStorage.groupKey = thingToSelect.thingIDNumber;
			command_SelectStorage.action = delegate()
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thingToSelect, true, true);
			};
			Text.Font = GameFont.Small;
			return command_SelectStorage;
		}

		// Token: 0x060132A8 RID: 78504 RVA: 0x005B2A30 File Offset: 0x005B0C30
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
			else
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectContainedPawn";
				}
				return "CommandSelectContainedThing";
			}
		}

		// Token: 0x060132A9 RID: 78505 RVA: 0x005B2A61 File Offset: 0x005B0C61
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
			else
			{
				if (carriedThing is Pawn)
				{
					return "CommandSelectContainedPawnDesc";
				}
				return "CommandSelectContainedThingDesc";
			}
		}

		// Token: 0x0400C6ED RID: 50925
		private static readonly Dictionary<string, TaggedString> labelCache = new Dictionary<string, TaggedString>();
	}
}
