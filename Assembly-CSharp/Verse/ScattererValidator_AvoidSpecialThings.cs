using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004B4 RID: 1204
	public class ScattererValidator_AvoidSpecialThings : ScattererValidator
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x000A46F2 File Offset: 0x000A28F2
		public override bool Allows(IntVec3 c, Map map)
		{
			return ScattererValidator_AvoidSpecialThings.IsValid(c, map);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000A46FC File Offset: 0x000A28FC
		public static bool IsValid(IntVec3 c, Map map)
		{
			if (ScattererValidator_AvoidSpecialThings.thingsToAvoid == null)
			{
				ScattererValidator_AvoidSpecialThings.thingsToAvoid = new Dictionary<ThingDef, float>
				{
					{
						ThingDefOf.SteamGeyser,
						3f
					},
					{
						ThingDefOf.AncientCryptosleepCasket,
						30f
					}
				};
				if (ModsConfig.RoyaltyActive)
				{
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.Plant_TreeAnima, 5f);
				}
				if (ModsConfig.IdeologyActive)
				{
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.ArchonexusCore, 20f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.GrandArchotechStructure, 20f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.MajorArchotechStructure, 20f);
				}
				if (ModsConfig.BiotechActive)
				{
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.AncientExostriderRemains, 6f);
				}
				if (ModsConfig.AnomalyActive)
				{
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.VoidMonolith, 10f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.VoidStructure, 5f);
				}
				if (ModsConfig.OdysseyActive)
				{
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.GeothermalVent, 10f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.AncientSmokeVent, 19f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.AncientToxVent, 19f);
					ScattererValidator_AvoidSpecialThings.thingsToAvoid.Add(ThingDefOf.AncientHeatVent, 19f);
				}
			}
			foreach (KeyValuePair<ThingDef, float> keyValuePair in ScattererValidator_AvoidSpecialThings.thingsToAvoid)
			{
				if (keyValuePair.Key != null)
				{
					foreach (Thing thing in map.listerThings.ThingsOfDef(keyValuePair.Key))
					{
						if (c.InHorDistOf(thing.Position, keyValuePair.Value + thing.def.size.Magnitude / 2f + keyValuePair.Key.size.Magnitude / 2f))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04001AFD RID: 6909
		private static Dictionary<ThingDef, float> thingsToAvoid;
	}
}
