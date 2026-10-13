using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004B3 RID: 1203
	public class ScattererValidator_AvoidThingsOfDef : ScattererValidator
	{
		// Token: 0x06001F49 RID: 8009 RVA: 0x000A462C File Offset: 0x000A282C
		public override bool Allows(IntVec3 c, Map map)
		{
			foreach (ThingDef def in this.thingsToAvoid)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(def))
				{
					if (c.InHorDistOf(thing.Position, (float)this.radius))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001AFB RID: 6907
		public int radius = 1;

		// Token: 0x04001AFC RID: 6908
		public List<ThingDef> thingsToAvoid = new List<ThingDef>();
	}
}
