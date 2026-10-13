using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039A0 RID: 14752
	public class SymbolResolver_Interior_Storage : SymbolResolver
	{
		// Token: 0x060143F2 RID: 82930 RVA: 0x0060AFEC File Offset: 0x006091EC
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			BaseGen.symbolStack.Push("stockpile", rp, null);
			if (map.mapTemperature.OutdoorTemp > 15f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
		}

		// Token: 0x0400DFB6 RID: 57270
		private const float SpawnPassiveCoolerIfTemperatureAbove = 15f;
	}
}
