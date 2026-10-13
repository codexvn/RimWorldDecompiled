using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF4 RID: 15092
	public class QuestNode_SetItemStashContents : QuestNode
	{
		// Token: 0x060148D5 RID: 84181 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148D6 RID: 84182 RVA: 0x006216AC File Offset: 0x0061F8AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		// Token: 0x060148D7 RID: 84183 RVA: 0x006216D2 File Offset: 0x0061F8D2
		private IEnumerable<ThingDef> GetContents(Slate slate)
		{
			IEnumerable<ThingDef> value = this.items.GetValue(slate);
			if (value != null)
			{
				foreach (ThingDef thingDef in value)
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
			}
			List<QuestNode_SetItemStashContents.ThingCategoryCount> value2 = this.categories.GetValue(slate);
			if (value2 != null)
			{
				using (List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = value2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						QuestNode_SetItemStashContents.<>c__DisplayClass6_0 CS$<>8__locals1 = new QuestNode_SetItemStashContents.<>c__DisplayClass6_0();
						CS$<>8__locals1.c = enumerator2.Current;
						try
						{
							int amt = Mathf.Max(CS$<>8__locals1.c.amount.RandomInRange, 1);
							int num;
							for (int i = 0; i < amt; i = num + 1)
							{
								IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
								Func<ThingDef, bool> predicate;
								if ((predicate = CS$<>8__locals1.<>9__0) == null)
								{
									predicate = (CS$<>8__locals1.<>9__0 = ((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(CS$<>8__locals1.c.category) && (CS$<>8__locals1.c.allowDuplicates || !QuestNode_SetItemStashContents.tmpItems.Contains(x))));
								}
								ThingDef thingDef2;
								if (allDefs.Where(predicate).TryRandomElement(out thingDef2))
								{
									QuestNode_SetItemStashContents.tmpItems.Add(thingDef2);
									yield return thingDef2;
								}
								num = i;
							}
						}
						finally
						{
							QuestNode_SetItemStashContents.tmpItems.Clear();
						}
						CS$<>8__locals1 = null;
					}
				}
				List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = default(List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400E414 RID: 58388
		public SlateRef<IEnumerable<ThingDef>> items;

		// Token: 0x0400E415 RID: 58389
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		// Token: 0x0400E416 RID: 58390
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		// Token: 0x02003AF5 RID: 15093
		public class ThingCategoryCount
		{
			// Token: 0x0400E417 RID: 58391
			public ThingCategoryDef category;

			// Token: 0x0400E418 RID: 58392
			public IntRange amount;

			// Token: 0x0400E419 RID: 58393
			public bool allowDuplicates = true;
		}
	}
}
