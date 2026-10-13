using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EB RID: 6379
	public class ThoughtWorker_SleepSuppressor : ThoughtWorker
	{
		// Token: 0x06009124 RID: 37156 RVA: 0x002AB190 File Offset: 0x002A9390
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
			foreach (Thing thing in p.MapHeld.listerThings.ThingsOfDef(ThingDefOf.SleepSuppressor))
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
