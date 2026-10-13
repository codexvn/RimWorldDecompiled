using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D91 RID: 7569
	public class IncidentWorker_DiseaseHuman : IncidentWorker_Disease
	{
		// Token: 0x0600A95F RID: 43359 RVA: 0x00307BC0 File Offset: 0x00305DC0
		protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map != null)
			{
				return from x in map.mapPawns.FreeColonistsAndPrisoners
				where x.ParentHolder == null || !(x.ParentHolder is CompBiosculpterPod)
				select x;
			}
			return from x in ((Caravan)target).PawnsListForReading
			where x.IsFreeColonist || x.IsPrisonerOfColony
			select x;
		}

		// Token: 0x0600A960 RID: 43360 RVA: 0x00307C38 File Offset: 0x00305E38
		protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
		{
			int num = this.PotentialVictimCandidates(parms.target).Count<Pawn>();
			IntRange intRange = new IntRange(Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * this.def.diseaseVictimFractionRange.max));
			int num2 = intRange.RandomInRange;
			num2 = Mathf.Clamp(num2, 1, this.def.diseaseMaxVictims);
			return base.PotentialVictims(parms.target).InRandomOrder(null).Take(num2);
		}
	}
}
