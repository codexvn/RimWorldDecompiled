using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003990 RID: 14736
	public static class InteriorSymbolResolverUtility
	{
		// Token: 0x060143C9 RID: 82889 RVA: 0x0060A50C File Offset: 0x0060870C
		public static void PushBedroomHeatersCoolersAndLightSourcesSymbols(ResolveParams rp, bool hasToSpawnLightSource = true)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > 22f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			bool flag = false;
			if (map.mapTemperature.OutdoorTemp < 3f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ((map.mapTemperature.OutdoorTemp < -20f) ? ThingDefOf.Campfire : ThingDefOf.TorchLamp);
					flag = true;
				}
				int num = (map.mapTemperature.OutdoorTemp < -45f) ? 2 : 1;
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = singleThingDef;
					BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
				}
			}
			if (!flag && hasToSpawnLightSource)
			{
				BaseGen.symbolStack.Push("indoorLighting", rp, null);
			}
		}

		// Token: 0x0400DFA0 RID: 57248
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		// Token: 0x0400DFA1 RID: 57249
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		// Token: 0x0400DFA2 RID: 57250
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		// Token: 0x0400DFA3 RID: 57251
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;
	}
}
