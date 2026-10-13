using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA3 RID: 7587
	public class IncidentWorker_HarbingerTreeProvoked : IncidentWorker
	{
		// Token: 0x0600A9A9 RID: 43433 RVA: 0x00308EBC File Offset: 0x003070BC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 loc;
			if (!IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(map, out loc, null))
			{
				return false;
			}
			Plant plant = (Plant)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null);
			plant.Growth = 0.15f;
			GenSpawn.Spawn(plant, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, plant, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
