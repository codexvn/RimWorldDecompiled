using System.Collections.Generic;
using Verse;

namespace RimWorld;

public class ThoughtWorker_JoyousPresence : ThoughtWorker
{
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
		List<Pawn> freeColonistsSpawned = p.MapHeld.mapPawns.FreeColonistsSpawned;
		for (int i = 0; i < freeColonistsSpawned.Count; i++)
		{
			Pawn pawn = freeColonistsSpawned[i];
			if (pawn != p && pawn.story.traits.HasTrait(TraitDefOf.Joyous) && !pawn.HostileTo(p))
			{
				return ThoughtState.ActiveWithReason(def.stages[0].description.Formatted(pawn.Named("PAWN")));
			}
		}
		return false;
	}
}
