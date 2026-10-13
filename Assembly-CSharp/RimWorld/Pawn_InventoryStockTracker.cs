using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002454 RID: 9300
	public class Pawn_InventoryStockTracker : IExposable
	{
		// Token: 0x0600C968 RID: 51560 RVA: 0x003AF181 File Offset: 0x003AD381
		public Pawn_InventoryStockTracker()
		{
		}

		// Token: 0x0600C969 RID: 51561 RVA: 0x003AF194 File Offset: 0x003AD394
		public Pawn_InventoryStockTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C96A RID: 51562 RVA: 0x003AF1AE File Offset: 0x003AD3AE
		public InventoryStockEntry GetCurrentEntryFor(InventoryStockGroupDef group)
		{
			if (!this.stockEntries.ContainsKey(group))
			{
				this.stockEntries[group] = this.CreateDefaultEntryFor(group);
			}
			return this.stockEntries[group];
		}

		// Token: 0x0600C96B RID: 51563 RVA: 0x003AF1DD File Offset: 0x003AD3DD
		public int GetDesiredCountForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).count;
		}

		// Token: 0x0600C96C RID: 51564 RVA: 0x003AF1EB File Offset: 0x003AD3EB
		public ThingDef GetDesiredThingForGroup(InventoryStockGroupDef group)
		{
			return this.GetCurrentEntryFor(group).thingDef;
		}

		// Token: 0x0600C96D RID: 51565 RVA: 0x003AF1FC File Offset: 0x003AD3FC
		public void SetCountForGroup(InventoryStockGroupDef group, int count)
		{
			InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(group);
			int count2 = Mathf.Clamp(count, group.min, group.max);
			currentEntryFor.count = count2;
		}

		// Token: 0x0600C96E RID: 51566 RVA: 0x003AF22C File Offset: 0x003AD42C
		public void SetThingForGroup(InventoryStockGroupDef group, ThingDef thing)
		{
			if (!group.thingDefs.Contains(thing))
			{
				Log.Error(string.Concat(new string[]
				{
					"Inventory stock group ",
					group.defName,
					" does not contain ",
					thing.defName,
					"."
				}));
				return;
			}
			this.GetCurrentEntryFor(group).thingDef = thing;
		}

		// Token: 0x0600C96F RID: 51567 RVA: 0x003AF28F File Offset: 0x003AD48F
		private InventoryStockEntry CreateDefaultEntryFor(InventoryStockGroupDef group)
		{
			return new InventoryStockEntry
			{
				count = group.min,
				thingDef = group.DefaultThingDef
			};
		}

		// Token: 0x0600C970 RID: 51568 RVA: 0x003AF2B0 File Offset: 0x003AD4B0
		public bool AnyThingsRequiredNow()
		{
			foreach (KeyValuePair<InventoryStockGroupDef, InventoryStockEntry> keyValuePair in this.stockEntries)
			{
				if (this.pawn.inventory.Count(keyValuePair.Value.thingDef) < keyValuePair.Value.count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C971 RID: 51569 RVA: 0x003AF330 File Offset: 0x003AD530
		public void ExposeData()
		{
			Scribe_Collections.Look<InventoryStockGroupDef, InventoryStockEntry>(ref this.stockEntries, "stockEntries", LookMode.Def, LookMode.Deep, ref this.tmpInventoryStockGroups, ref this.tmpInventoryStock, true, false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (InventoryStockGroupDef inventoryStockGroupDef in DefDatabase<InventoryStockGroupDef>.AllDefs)
				{
					InventoryStockEntry currentEntryFor = this.GetCurrentEntryFor(inventoryStockGroupDef);
					if (!inventoryStockGroupDef.thingDefs.Contains(currentEntryFor.thingDef))
					{
						currentEntryFor.thingDef = inventoryStockGroupDef.DefaultThingDef;
					}
				}
			}
		}

		// Token: 0x04008927 RID: 35111
		public Pawn pawn;

		// Token: 0x04008928 RID: 35112
		public Dictionary<InventoryStockGroupDef, InventoryStockEntry> stockEntries = new Dictionary<InventoryStockGroupDef, InventoryStockEntry>();

		// Token: 0x04008929 RID: 35113
		private List<InventoryStockGroupDef> tmpInventoryStockGroups;

		// Token: 0x0400892A RID: 35114
		private List<InventoryStockEntry> tmpInventoryStock;
	}
}
