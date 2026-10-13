using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038A0 RID: 14496
	public class SketchResolver_AncientBandNodeRoom : SketchResolver
	{
		// Token: 0x0601406E RID: 82030 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0601406F RID: 82031 RVA: 0x005F3110 File Offset: 0x005F1310
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient band node room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.AncientBandNode;
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
		}
	}
}
