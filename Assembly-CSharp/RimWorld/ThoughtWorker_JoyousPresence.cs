using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018ED RID: 6381
	public class ThoughtWorker_JoyousPresence : ThoughtWorker
	{
		// Token: 0x06009128 RID: 37160 RVA: 0x002AB348 File Offset: 0x002A9548
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
			if (!p.IsPlayerControlled && !p.IsPrisonerOfColony)
			{
				return false;
			}
			foreach (Pawn pawn in p.MapHeld.mapPawns.FreeColonistsSpawned)
			{
				if (pawn != p && pawn.story.traits.HasTrait(TraitDefOf.Joyous) && !pawn.HostileTo(p))
				{
					return ThoughtState.ActiveWithReason(this.def.stages[0].description.Formatted(pawn.Named("PAWN")));
				}
			}
			return false;
		}
	}
}
