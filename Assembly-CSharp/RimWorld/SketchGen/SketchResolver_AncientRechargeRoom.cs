using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038AE RID: 14510
	public class SketchResolver_AncientRechargeRoom : SketchResolver
	{
		// Token: 0x1700331B RID: 13083
		// (get) Token: 0x060140AB RID: 82091 RVA: 0x005F3F84 File Offset: 0x005F2184
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				yield return ThingDefOf.AncientBasicRecharger;
				yield return ThingDefOf.AncientStandardRecharger;
				yield break;
			}
		}

		// Token: 0x060140AC RID: 82092 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x060140AD RID: 82093 RVA: 0x005F3F90 File Offset: 0x005F2190
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient recharger room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientRechargeRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			SketchResolveParams parms4 = parms;
			parms4.wallEdgeThing = ThingDefOf.Table1x2c;
			parms4.allowWood = new bool?(false);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
		}
	}
}
