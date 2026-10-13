using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002158 RID: 8536
	public sealed class ResourceCounter
	{
		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x0600BA84 RID: 47748 RVA: 0x0035D87E File Offset: 0x0035BA7E
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x0600BA85 RID: 47749 RVA: 0x0035D88B File Offset: 0x0035BA8B
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x0600BA86 RID: 47750 RVA: 0x0035D894 File Offset: 0x0035BA94
		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					if (keyValuePair.Key.IsNutritionGivingIngestible && keyValuePair.Key.ingestible.HumanEdible)
					{
						num += keyValuePair.Key.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)keyValuePair.Value;
					}
				}
				return num;
			}
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x0600BA87 RID: 47751 RVA: 0x0035D928 File Offset: 0x0035BB28
		public float TotalHumanBabyEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					ThingDef thingDef;
					int num2;
					keyValuePair.Deconstruct(out thingDef, out num2);
					ThingDef thingDef2 = thingDef;
					int num3 = num2;
					if (thingDef2.IsNutritionGivingIngestibleForHumanlikeBabies)
					{
						num += thingDef2.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)num3;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BA88 RID: 47752 RVA: 0x0035D9A8 File Offset: 0x0035BBA8
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x0600BA89 RID: 47753 RVA: 0x0035DA16 File Offset: 0x0035BC16
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x0600BA8A RID: 47754 RVA: 0x0035DA38 File Offset: 0x0035BC38
		private void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x0600BA8B RID: 47755 RVA: 0x0035DA7C File Offset: 0x0035BC7C
		public int GetCount(ThingDef rDef)
		{
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				return 0;
			}
			int result;
			if (this.countedAmounts.TryGetValue(rDef, out result))
			{
				return result;
			}
			Log.Error("Looked for nonexistent key " + ((rDef != null) ? rDef.ToString() : null) + " in counted resources.");
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x0035DAD4 File Offset: 0x0035BCD4
		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
			{
				if (group.Includes(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x0600BA8D RID: 47757 RVA: 0x0035DB3C File Offset: 0x0035BD3C
		public int GetCountIn(ThingCategoryDef cat)
		{
			int num = 0;
			for (int i = 0; i < cat.childThingDefs.Count; i++)
			{
				num += this.GetCount(cat.childThingDefs[i]);
			}
			for (int j = 0; j < cat.childCategories.Count; j++)
			{
				if (!cat.childCategories[j].resourceReadoutRoot)
				{
					num += this.GetCountIn(cat.childCategories[j]);
				}
			}
			return num;
		}

		// Token: 0x0600BA8E RID: 47758 RVA: 0x0035DBB5 File Offset: 0x0035BDB5
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x0600BA8F RID: 47759 RVA: 0x0035DBD0 File Offset: 0x0035BDD0
		public void UpdateResourceCounts()
		{
			this.ResetResourceCounts();
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing outerThing in allGroupsListForReading[i].HeldThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (innerIfMinified.def.CountAsResource && this.ShouldCount(innerIfMinified))
					{
						Dictionary<ThingDef, int> dictionary = this.countedAmounts;
						ThingDef def = innerIfMinified.def;
						dictionary[def] += innerIfMinified.stackCount;
					}
				}
			}
		}

		// Token: 0x0600BA90 RID: 47760 RVA: 0x0035DC88 File Offset: 0x0035BE88
		public void CheckUpdateResource(Thing t)
		{
			if (t == null)
			{
				return;
			}
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (!innerIfMinified.def.CountAsResource || !this.ShouldCount(innerIfMinified))
			{
				return;
			}
			this.UpdateResourceCounts();
		}

		// Token: 0x0600BA91 RID: 47761 RVA: 0x0035DCBD File Offset: 0x0035BEBD
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh() && (!t.SpawnedOrAnyParentSpawned || !t.PositionHeld.Fogged(t.MapHeld));
		}

		// Token: 0x0400809C RID: 32924
		private Map map;

		// Token: 0x0400809D RID: 32925
		private readonly Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x0400809E RID: 32926
		private static readonly List<ThingDef> resources = new List<ThingDef>();
	}
}
