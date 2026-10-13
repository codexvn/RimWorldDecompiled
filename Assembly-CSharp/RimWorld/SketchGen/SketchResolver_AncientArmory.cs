using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200389F RID: 14495
	public class SketchResolver_AncientArmory : SketchResolver
	{
		// Token: 0x0601406B RID: 82027 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0601406C RID: 82028 RVA: 0x005F30A0 File Offset: 0x005F12A0
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient armory"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.AncientLockerBank;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
			SketchResolveParams parms3 = parms;
			parms3.thingCentral = ThingDefOf.AncientSecurityTurret;
			parms3.requireFloor = new bool?(true);
			SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
		}
	}
}
