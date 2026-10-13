using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038AA RID: 14506
	public class SketchResolver_AncientMechGeneratorRoom : SketchResolver
	{
		// Token: 0x06014099 RID: 82073 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0601409A RID: 82074 RVA: 0x005F3BF8 File Offset: 0x005F1DF8
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient mechanitor generator room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ThingDefOf.AncientToxifierGenerator;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
		}
	}
}
