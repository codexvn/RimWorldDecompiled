using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BF RID: 6335
	public class ThoughtWorker_MissingTongue : ThoughtWorker
	{
		// Token: 0x0600909E RID: 37022 RVA: 0x002A9A3C File Offset: 0x002A7C3C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<BodyPartRecord> partsWithTag = p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.Tongue);
			if (!partsWithTag.Any<BodyPartRecord>())
			{
				return ThoughtState.Inactive;
			}
			foreach (BodyPartRecord part in partsWithTag)
			{
				if (!p.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part) && p.health.hediffSet.PartIsMissing(part))
				{
					return true;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
