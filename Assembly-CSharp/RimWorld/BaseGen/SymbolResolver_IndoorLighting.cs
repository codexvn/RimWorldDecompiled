using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003962 RID: 14690
	public class SymbolResolver_IndoorLighting : SymbolResolver
	{
		// Token: 0x06014339 RID: 82745 RVA: 0x006069AC File Offset: 0x00604BAC
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else if (map.mapTemperature.OutdoorTemp > 18f)
			{
				thingDef = null;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			if (thingDef != null)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = thingDef;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x0400DF47 RID: 57159
		private const float NeverSpawnTorchesIfTemperatureAbove = 18f;
	}
}
