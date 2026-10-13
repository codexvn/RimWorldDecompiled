using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038A4 RID: 14500
	public class SketchResolver_AncientEquipmentRoom : SketchResolver
	{
		// Token: 0x17003312 RID: 13074
		// (get) Token: 0x0601407B RID: 82043 RVA: 0x005F35D2 File Offset: 0x005F17D2
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				yield return ThingDefOf.AncientMachine;
				yield return ThingDefOf.AncientStorageCylinder;
				yield break;
			}
		}

		// Token: 0x17003313 RID: 13075
		// (get) Token: 0x0601407C RID: 82044 RVA: 0x005F35DB File Offset: 0x005F17DB
		private static IEnumerable<ThingDef> EdgeThings
		{
			get
			{
				yield return ThingDefOf.AncientSystemRack;
				yield return ThingDefOf.AncientEquipmentBlocks;
				yield break;
			}
		}

		// Token: 0x0601407D RID: 82045 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0601407E RID: 82046 RVA: 0x005F35E4 File Offset: 0x005F17E4
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient equipment room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientEquipmentRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			foreach (ThingDef wallEdgeThing in SketchResolver_AncientEquipmentRoom.EdgeThings)
			{
				SketchResolveParams parms4 = parms;
				parms4.wallEdgeThing = wallEdgeThing;
				parms4.requireFloor = new bool?(true);
				SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
			}
		}
	}
}
