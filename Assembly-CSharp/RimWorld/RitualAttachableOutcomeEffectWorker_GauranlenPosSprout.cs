using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200268C RID: 9868
	public class RitualAttachableOutcomeEffectWorker_GauranlenPosSprout : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8D6 RID: 55510 RVA: 0x0040419C File Offset: 0x0040239C
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (this.CanApplyNow(jobRitual.Ritual, jobRitual.Map))
			{
				IncidentParms parms = new IncidentParms
				{
					target = jobRitual.Map,
					customLetterText = IncidentDefOf.GauranlenPodSpawn.letterText + "\n\n" + "RitualAttachedOutcome_GauranlenTreePod_ExtraDesc".Translate(jobRitual.RitualLabel)
				};
				if (IncidentDefOf.GauranlenPodSpawn.Worker.TryExecute(parms))
				{
					extraOutcomeDesc = this.def.letterInfoText;
				}
			}
		}

		// Token: 0x0600D8D7 RID: 55511 RVA: 0x00404234 File Offset: 0x00402434
		public override AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			if (!IncidentWorker_GauranlenPodSpawn.IsGoodBiome(map.Biome))
			{
				return "RitualAttachedOutcomeCantApply_ExtremeBiome".Translate();
			}
			IntVec3 intVec;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out intVec))
			{
				return "RitualAttachedOutcomeCantApply_NoValidSpot".Translate();
			}
			return base.CanApplyNow(ritual, map);
		}
	}
}
