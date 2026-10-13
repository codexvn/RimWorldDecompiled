using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FCB RID: 12235
	public class Alert_DisallowedBuildingInsideMonument : Alert_Critical
	{
		// Token: 0x17002CB5 RID: 11445
		// (get) Token: 0x060113E4 RID: 70628 RVA: 0x004F0CD4 File Offset: 0x004EEED4
		private List<Thing> DisallowedBuildings
		{
			get
			{
				this.disallowedBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone)
						{
							Thing firstDisallowedBuilding = monumentMarker.FirstDisallowedBuilding;
							if (firstDisallowedBuilding != null)
							{
								this.disallowedBuildingsResult.Add(firstDisallowedBuilding);
							}
						}
					}
				}
				return this.disallowedBuildingsResult;
			}
		}

		// Token: 0x17002CB6 RID: 11446
		// (get) Token: 0x060113E5 RID: 70629 RVA: 0x004F0D64 File Offset: 0x004EEF64
		private int MinTicksLeft
		{
			get
			{
				int num = int.MaxValue;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone && monumentMarker.AnyDisallowedBuilding)
						{
							num = Mathf.Min(num, 60000 - monumentMarker.ticksSinceDisallowedBuilding);
						}
					}
				}
				return num;
			}
		}

		// Token: 0x060113E6 RID: 70630 RVA: 0x004F0DF0 File Offset: 0x004EEFF0
		public Alert_DisallowedBuildingInsideMonument()
		{
			this.defaultLabel = "DisallowedBuildingInsideMonument".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x060113E7 RID: 70631 RVA: 0x004F0E1F File Offset: 0x004EF01F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.DisallowedBuildings);
		}

		// Token: 0x060113E8 RID: 70632 RVA: 0x004F0E2C File Offset: 0x004EF02C
		public override TaggedString GetExplanation()
		{
			return "DisallowedBuildingInsideMonumentDesc".Translate(this.MinTicksLeft.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0400B2F6 RID: 45814
		private List<Thing> disallowedBuildingsResult = new List<Thing>();
	}
}
