using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003001 RID: 12289
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x17002CD4 RID: 11476
		// (get) Token: 0x060114CA RID: 70858 RVA: 0x004F5934 File Offset: 0x004F3B34
		private List<Thing> BadDispensers
		{
			get
			{
				this.badDispensersResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.FoodDispenser))
					{
						bool flag = false;
						foreach (IntVec3 c in ((Building_NutrientPasteDispenser)thing).AdjCellsCardinalInBounds)
						{
							Thing edifice = c.GetEdifice(thing.Map);
							if (edifice != null && edifice.IsHopper())
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.badDispensersResult.Add(thing);
						}
					}
				}
				return this.badDispensersResult;
			}
		}

		// Token: 0x060114CB RID: 70859 RVA: 0x004F5A30 File Offset: 0x004F3C30
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060114CC RID: 70860 RVA: 0x004F5A7F File Offset: 0x004F3C7F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		// Token: 0x0400B354 RID: 45908
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
