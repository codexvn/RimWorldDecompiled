using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003193 RID: 12691
	[StaticConstructorOnStartup]
	public class Dialog_MapSearch : Dialog_Search<Thing>
	{
		// Token: 0x17002E68 RID: 11880
		// (get) Token: 0x06011DA3 RID: 73123 RVA: 0x0052ECB4 File Offset: 0x0052CEB4
		protected override List<Thing> SearchSet
		{
			get
			{
				return this.map.listerThings.AllThings;
			}
		}

		// Token: 0x17002E69 RID: 11881
		// (get) Token: 0x06011DA4 RID: 73124 RVA: 0x0052ECC6 File Offset: 0x0052CEC6
		protected override bool ShouldClose
		{
			get
			{
				return WorldRendererUtility.WorldSelected || Find.CurrentMap != this.map;
			}
		}

		// Token: 0x17002E6A RID: 11882
		// (get) Token: 0x06011DA5 RID: 73125 RVA: 0x0052ECE1 File Offset: 0x0052CEE1
		protected override TaggedString SearchLabel
		{
			get
			{
				return "SearchTheMap".Translate();
			}
		}

		// Token: 0x06011DA6 RID: 73126 RVA: 0x0052ECED File Offset: 0x0052CEED
		public Dialog_MapSearch(Map map)
		{
			this.map = map;
		}

		// Token: 0x06011DA7 RID: 73127 RVA: 0x0052ED08 File Offset: 0x0052CF08
		public override void PostOpen()
		{
			base.PostOpen();
			ThingListChangedCallbacks thingListChangedCallbacks = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks.onThingAdded = (Action<Thing>)Delegate.Combine(thingListChangedCallbacks.onThingAdded, new Action<Thing>(this.TryAddElement));
			ThingListChangedCallbacks thingListChangedCallbacks2 = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks2.onThingRemoved = (Action<Thing>)Delegate.Combine(thingListChangedCallbacks2.onThingRemoved, new Action<Thing>(this.TryRemoveElement));
		}

		// Token: 0x06011DA8 RID: 73128 RVA: 0x0052ED78 File Offset: 0x0052CF78
		public override void PostClose()
		{
			base.PostClose();
			ThingListChangedCallbacks thingListChangedCallbacks = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks.onThingAdded = (Action<Thing>)Delegate.Remove(thingListChangedCallbacks.onThingAdded, new Action<Thing>(this.TryAddElement));
			ThingListChangedCallbacks thingListChangedCallbacks2 = this.map.thingListChangedCallbacks;
			thingListChangedCallbacks2.onThingRemoved = (Action<Thing>)Delegate.Remove(thingListChangedCallbacks2.onThingRemoved, new Action<Thing>(this.TryRemoveElement));
		}

		// Token: 0x06011DA9 RID: 73129 RVA: 0x0052EDE8 File Offset: 0x0052CFE8
		protected override void CheckAnyElementRemoved()
		{
			bool flag = false;
			for (int i = this.searchResults.Count - 1; i >= 0; i--)
			{
				Thing thing = this.searchResults.Values[i];
				if (thing == null || thing.Destroyed || thing.MapHeld != this.map || !this.ThingIsVisibleToPlayer(thing))
				{
					this.searchResults.RemoveAt(i);
					this.searchResultsSet.Remove(thing);
					flag = true;
				}
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x06011DAA RID: 73130 RVA: 0x0052EE68 File Offset: 0x0052D068
		protected override void DoIcon(Thing element, Rect iconRect)
		{
			Widgets.ThingIcon(iconRect, element, 1f, null, false, 1f, false);
		}

		// Token: 0x06011DAB RID: 73131 RVA: 0x0052EE91 File Offset: 0x0052D091
		protected override void DoLabel(Thing element, Rect labelRect)
		{
			Widgets.Label(labelRect, element.LabelCap);
		}

		// Token: 0x06011DAC RID: 73132 RVA: 0x0052EEA0 File Offset: 0x0052D0A0
		protected override void DoExtraIcon(Thing element, Rect iconRect)
		{
			IThingHolder parentHolder = element.ParentHolder;
			if (((parentHolder != null) ? parentHolder.ParentHolder : null) is Pawn)
			{
				GUI.DrawTexture(iconRect.ContractedBy(2f), Dialog_MapSearch.InventoryIcon);
				return;
			}
			if (!(element.ParentHolder is Map))
			{
				GUI.DrawTexture(iconRect.ContractedBy(2f), Dialog_MapSearch.ContainerIcon);
			}
		}

		// Token: 0x06011DAD RID: 73133 RVA: 0x0052EF00 File Offset: 0x0052D100
		protected override void ClikedOnElement(Thing element)
		{
			if (element == null || element.Destroyed || element.MapHeld != this.map)
			{
				return;
			}
			CameraJumper.TryJump(element, CameraJumper.MovementMode.Pan);
			Find.Selector.ClearSelection();
			Thing obj = element;
			IThingHolder parentHolder = element.ParentHolder;
			if (parentHolder != null)
			{
				Pawn pawn = parentHolder.ParentHolder as Pawn;
				if (pawn != null)
				{
					obj = pawn;
				}
				else
				{
					Thing thing = parentHolder as Thing;
					if (thing != null)
					{
						obj = thing;
					}
				}
			}
			Find.Selector.Select(obj, true, true);
		}

		// Token: 0x06011DAE RID: 73134 RVA: 0x0052EF78 File Offset: 0x0052D178
		protected override bool ShouldSkipElement(Thing element)
		{
			if (element == null)
			{
				return true;
			}
			Corpse corpse = element as Corpse;
			if (corpse != null && corpse.Bugged)
			{
				return true;
			}
			MinifiedThing minifiedThing = element as MinifiedThing;
			return minifiedThing != null && minifiedThing.InnerThing == null;
		}

		// Token: 0x06011DAF RID: 73135 RVA: 0x0052EFB4 File Offset: 0x0052D1B4
		protected override void OnHighlightUpdate(Thing element)
		{
			GenDraw.DrawArrowPointingAt(element.PositionHeld.ToVector3Shifted(), false);
		}

		// Token: 0x06011DB0 RID: 73136 RVA: 0x0052EFD8 File Offset: 0x0052D1D8
		protected override void SetInitialSizeAndPosition()
		{
			this.scrollHeight = (float)this.searchResults.Count * 26f;
			Vector2 initialSize = this.InitialSize;
			initialSize.y = Mathf.Clamp(initialSize.y + this.scrollHeight, this.InitialSize.y, (float)UI.screenHeight / 2f);
			this.windowRect = new Rect((float)UI.screenWidth - initialSize.x, (float)UI.screenHeight - initialSize.y - 35f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x06011DB1 RID: 73137 RVA: 0x0052F070 File Offset: 0x0052D270
		private bool CanAddThing(Thing thing)
		{
			if (!this.quickSearchWidget.filter.Text.NullOrEmpty() && thing != null && thing.def.selectable && !thing.Destroyed && thing.def.showInSearch && thing.MapHeld == this.map && (DebugSettings.searchIgnoresRestrictions || !thing.PositionHeld.Fogged(thing.MapHeld)) && !this.searchResultsSet.Contains(thing))
			{
				Corpse corpse = thing as Corpse;
				if ((corpse == null || !corpse.Bugged) && (base.TextMatch(thing.LabelNoCount.StripTags()) || base.TextMatch(thing.def.label.StripTags())))
				{
					return this.ThingIsVisibleToPlayer(thing);
				}
			}
			return false;
		}

		// Token: 0x06011DB2 RID: 73138 RVA: 0x0052F144 File Offset: 0x0052D344
		private bool ThingIsVisibleToPlayer(Thing thing)
		{
			if (DebugSettings.searchIgnoresRestrictions)
			{
				return true;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsHiddenFromPlayer())
			{
				return false;
			}
			IThingHolder parentHolder = thing.ParentHolder;
			if (parentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = parentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null && pawn_InventoryTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_EquipmentTracker pawn_EquipmentTracker = parentHolder as Pawn_EquipmentTracker;
				if (pawn_EquipmentTracker != null && pawn_EquipmentTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_ApparelTracker pawn_ApparelTracker = parentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker != null && pawn_ApparelTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null && pawn_CarryTracker.pawn.IsHiddenFromPlayer())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06011DB3 RID: 73139 RVA: 0x0052F1E4 File Offset: 0x0052D3E4
		protected override void TryAddElement(Thing element)
		{
			bool flag = false;
			foreach (Thing thing in this.ContentsFromThing(element))
			{
				this.searchResults.Add(thing.LabelNoParenthesis.ToLower(), thing);
				this.searchResultsSet.Add(thing);
				flag = true;
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x06011DB4 RID: 73140 RVA: 0x0052F264 File Offset: 0x0052D464
		protected override void TryRemoveElement(Thing thing)
		{
			bool flag = false;
			foreach (Thing thing2 in this.ContentsFromThing(thing))
			{
				int num = this.searchResults.IndexOfValue(thing2);
				if (num >= 0)
				{
					this.searchResults.RemoveAt(num);
					this.searchResultsSet.Remove(thing2);
					flag = true;
				}
			}
			if (flag)
			{
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x06011DB5 RID: 73141 RVA: 0x0052F2E8 File Offset: 0x0052D4E8
		private List<Thing> ContentsFromThing(Thing thing)
		{
			this.tmpContents.Clear();
			if (this.CanAddThing(thing))
			{
				this.tmpContents.Add(thing);
			}
			if (!thing.Faction.IsPlayerSafe())
			{
				return this.tmpContents;
			}
			ISearchableContents searchableContents = thing as ISearchableContents;
			if (searchableContents != null)
			{
				ThingOwner searchableContents2 = searchableContents.SearchableContents;
				if (searchableContents2 != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)searchableContents2))
					{
						if (this.CanAddThing(thing2))
						{
							this.tmpContents.Add(thing2);
						}
					}
				}
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsAnimal))
			{
				if (pawn.equipment != null)
				{
					foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
					{
						if (this.CanAddThing(thingWithComps))
						{
							this.tmpContents.Add(thingWithComps);
						}
					}
				}
				if (pawn.apparel != null)
				{
					foreach (Apparel apparel in pawn.apparel.WornApparel)
					{
						if (this.CanAddThing(apparel))
						{
							this.tmpContents.Add(apparel);
						}
					}
				}
				if (pawn.inventory != null)
				{
					foreach (Thing thing3 in pawn.inventory.innerContainer)
					{
						if (this.CanAddThing(thing3))
						{
							this.tmpContents.Add(thing3);
						}
					}
				}
			}
			ThingWithComps thingWithComps2 = thing as ThingWithComps;
			if (thingWithComps2 != null)
			{
				foreach (ThingComp thingComp in thingWithComps2.AllComps)
				{
					ISearchableContents searchableContents3 = thingComp as ISearchableContents;
					if (searchableContents3 != null)
					{
						ThingOwner searchableContents4 = searchableContents3.SearchableContents;
						if (searchableContents4 != null)
						{
							foreach (Thing thing4 in ((IEnumerable<Thing>)searchableContents4))
							{
								if (this.CanAddThing(thing4))
								{
									this.tmpContents.Add(thing4);
								}
							}
						}
					}
				}
			}
			return this.tmpContents;
		}

		// Token: 0x0400B8AF RID: 47279
		private Map map;

		// Token: 0x0400B8B0 RID: 47280
		private static readonly Texture2D InventoryIcon = ContentFinder<Texture2D>.Get("UI/Widgets/SearchBar_Inventory", true);

		// Token: 0x0400B8B1 RID: 47281
		private static readonly Texture2D ContainerIcon = ContentFinder<Texture2D>.Get("UI/Widgets/SearchBar_Container", true);

		// Token: 0x0400B8B2 RID: 47282
		private List<Thing> tmpContents = new List<Thing>();
	}
}
