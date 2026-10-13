using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D16 RID: 15638
	public class AbandonedArchotechStructures : WorldObject
	{
		// Token: 0x06015493 RID: 87187 RVA: 0x0065E234 File Offset: 0x0065C434
		public Settlement GenerateSettlementAndDestroy()
		{
			this.Destroy();
			ArchotechSettlement archotechSettlement;
			if (this.worldObjectDef != null)
			{
				archotechSettlement = (ArchotechSettlement)WorldObjectMaker.MakeWorldObject(this.worldObjectDef);
			}
			else
			{
				archotechSettlement = (ArchotechSettlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement_SecondArchonexusCycle);
				Log.Warning("No archotech settlement configured for abandoned archotech structures, using default settlement def.");
			}
			archotechSettlement.existingBuildings.AddRange(this.archotechStructures);
			return archotechSettlement;
		}

		// Token: 0x06015494 RID: 87188 RVA: 0x0065E28E File Offset: 0x0065C48E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjectDef");
			Scribe_Collections.Look<Building>(ref this.archotechStructures, "archotechStructures", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400ED56 RID: 60758
		public WorldObjectDef worldObjectDef;

		// Token: 0x0400ED57 RID: 60759
		public List<Building> archotechStructures = new List<Building>();
	}
}
