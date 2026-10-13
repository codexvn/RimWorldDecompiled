using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EC RID: 6380
	public class ThoughtWorker_BioferriteGenerator : ThoughtWorker
	{
		// Token: 0x06009126 RID: 37158 RVA: 0x002AB26C File Offset: 0x002A946C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (p.MapHeld == null)
			{
				return false;
			}
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			foreach (Thing thing in p.MapHeld.listerThings.ThingsOfDef(ThingDefOf.BioferriteGenerator))
			{
				CompNoiseSource compNoiseSource = thing.TryGetComp<CompNoiseSource>();
				if (compNoiseSource.Active && p.PositionHeld.InHorDistOf(thing.Position, compNoiseSource.Props.radius))
				{
					return true;
				}
			}
			return false;
		}
	}
}
