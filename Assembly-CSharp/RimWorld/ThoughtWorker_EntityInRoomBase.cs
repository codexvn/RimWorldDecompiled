using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F3 RID: 6387
	public class ThoughtWorker_EntityInRoomBase : ThoughtWorker
	{
		// Token: 0x06009136 RID: 37174 RVA: 0x002AB5C4 File Offset: 0x002A97C4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			Room bedroom = p.ownership.Bedroom;
			if (bedroom == null || bedroom.TouchesMapEdge)
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn pawn in bedroom.ContainedThings<Pawn>())
			{
				if (pawn.Faction != p.Faction && (pawn.RaceProps.IsAnomalyEntity || pawn.IsSubhuman))
				{
					return ThoughtState.ActiveAtStage(0);
				}
			}
			foreach (Building_HoldingPlatform building_HoldingPlatform in bedroom.ContainedThings<Building_HoldingPlatform>())
			{
				if (building_HoldingPlatform.HeldPawn != null && building_HoldingPlatform.HeldPawn.Faction != p.Faction && (building_HoldingPlatform.HeldPawn.RaceProps.IsAnomalyEntity || building_HoldingPlatform.HeldPawn.IsSubhuman))
				{
					return ThoughtState.ActiveAtStage(0);
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
