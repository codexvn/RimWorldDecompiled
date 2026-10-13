using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003994 RID: 14740
	public class SymbolResolver_Interior_BatteryRoom : SymbolResolver
	{
		// Token: 0x060143D2 RID: 82898 RVA: 0x0060A8A0 File Offset: 0x00608AA0
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("chargeBatteries", rp, null);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Battery;
			resolveParams.thingRot = new Rot4?(Rand.Bool ? Rot4.North : Rot4.East);
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? 1);
			BaseGen.symbolStack.Push("fillWithThings", resolveParams, null);
		}
	}
}
