using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACA RID: 6858
	public class RaidAgeRestrictionWorker
	{
		// Token: 0x0600998A RID: 39306 RVA: 0x002CB788 File Offset: 0x002C9988
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return GenDate.DaysPassedSinceSettle >= this.def.earliestDay && parms.faction != null && parms.faction.def.humanlikeFaction && !parms.faction.def.disallowedRaidAgeRestrictions.NotNullAndContains(this.def);
		}

		// Token: 0x0600998B RID: 39307 RVA: 0x0014A519 File Offset: 0x00148719
		public bool ShouldApplyToKind(PawnKindDef kind)
		{
			return kind.RaceProps.Humanlike;
		}

		// Token: 0x0600998C RID: 39308 RVA: 0x002CB7E4 File Offset: 0x002C99E4
		public bool CanUseKind(PawnKindDef kind)
		{
			if (this.def.ageRange != null)
			{
				FloatRange value = this.def.ageRange.Value;
				if ((float)kind.minGenerationAge > value.max)
				{
					return false;
				}
				if ((float)kind.maxGenerationAge < value.min)
				{
					return false;
				}
			}
			if (kind.apparelRequired != null)
			{
				for (int i = 0; i < kind.apparelRequired.Count; i++)
				{
					if (!kind.apparelRequired[i].apparel.developmentalStageFilter.Has(this.def.developmentStage))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400690A RID: 26890
		public RaidAgeRestrictionDef def;
	}
}
