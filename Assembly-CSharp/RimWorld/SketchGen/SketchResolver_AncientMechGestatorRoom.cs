using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038AB RID: 14507
	public class SketchResolver_AncientMechGestatorRoom : SketchResolver
	{
		// Token: 0x17003318 RID: 13080
		// (get) Token: 0x0601409C RID: 82076 RVA: 0x005F3C2B File Offset: 0x005F1E2B
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				yield return ThingDefOf.AncientLargeMechGestator;
				yield break;
			}
		}

		// Token: 0x0601409D RID: 82077 RVA: 0x005F3083 File Offset: 0x005F1283
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0601409E RID: 82078 RVA: 0x005F3C34 File Offset: 0x005F1E34
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient mech gestator room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientMechGestatorRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			if (ModsConfig.IdeologyActive)
			{
				foreach (IntVec3 pos in parms.rect.Value.Cells)
				{
					if (Rand.Chance(0.05f) && !parms.sketch.ThingsAt(pos).Any<SketchThing>())
					{
						parms.sketch.AddThing(ThingDefOf.ChunkMechanoidSlag, pos, ThingDefOf.ChunkMechanoidSlag.defaultPlacingRot, null, 1, null, null, true, 2f);
					}
				}
			}
		}

		// Token: 0x0400DD5F RID: 56671
		private const float MechanoidDebrisChance = 0.05f;
	}
}
