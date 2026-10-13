using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018A7 RID: 6311
	public abstract class ThoughtWorker_MusicalInstrumentListeningBase : ThoughtWorker
	{
		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06009054 RID: 36948
		protected abstract ThingDef InstrumentDef { get; }

		// Token: 0x06009055 RID: 36949 RVA: 0x002A9028 File Offset: 0x002A7228
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			ThingDef def = this.InstrumentDef;
			return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(def), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), def.building.instrumentRange, delegate(Thing thing)
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				return building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed && Building_MusicalInstrument.IsAffectedByInstrument(def, building_MusicalInstrument.Position, p.Position, p.Map);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false) != null;
		}
	}
}
