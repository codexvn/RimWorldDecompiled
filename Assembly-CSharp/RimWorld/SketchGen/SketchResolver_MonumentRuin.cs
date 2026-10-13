using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038DB RID: 14555
	public class SketchResolver_MonumentRuin : SketchResolver
	{
		// Token: 0x0601415F RID: 82271 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x06014160 RID: 82272 RVA: 0x005F9C94 File Offset: 0x005F7E94
		protected override void ResolveInt(SketchResolveParams parms)
		{
			SketchResolveParams sketchResolveParams = parms;
			sketchResolveParams.allowWood = new bool?(parms.allowWood.GetValueOrDefault());
			if (sketchResolveParams.allowedMonumentThings == null)
			{
				sketchResolveParams.allowedMonumentThings = new ThingFilter();
				sketchResolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			if (ModsConfig.RoyaltyActive)
			{
				sketchResolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			}
			SketchResolverDefOf.Monument.Resolve(sketchResolveParams);
			SketchResolverDefOf.DamageBuildings.Resolve(parms);
		}
	}
}
