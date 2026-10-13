using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A40 RID: 6720
	public class GauranlenTreeModeDef : Def
	{
		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x06009759 RID: 38745 RVA: 0x002C34C4 File Offset: 0x002C16C4
		public string Description
		{
			get
			{
				if (this.cachedDescription == null)
				{
					this.cachedDescription = this.description;
					PawnKindDef pawnKindDef = this.pawnKindDef;
					CompProperties_Spawner compProperties_Spawner = (pawnKindDef != null) ? pawnKindDef.race.GetCompProperties<CompProperties_Spawner>() : null;
					if (compProperties_Spawner != null)
					{
						this.cachedDescription = this.cachedDescription + "\n\n" + "DryadProducesResourcesDesc".Translate(this.pawnKindDef.Named("DRYAD"), GenLabel.ThingLabel(compProperties_Spawner.thingToSpawn, null, compProperties_Spawner.spawnCount).Named("RESOURCES"), compProperties_Spawner.spawnIntervalRange.max.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve().CapitalizeFirst();
					}
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x04006455 RID: 25685
		public GauranlenTreeModeDef previousStage;

		// Token: 0x04006456 RID: 25686
		public PawnKindDef pawnKindDef;

		// Token: 0x04006457 RID: 25687
		public List<MemeDef> requiredMemes;

		// Token: 0x04006458 RID: 25688
		public Vector2 drawPosition;

		// Token: 0x04006459 RID: 25689
		public List<StatDef> displayedStats;

		// Token: 0x0400645A RID: 25690
		public List<DefHyperlink> hyperlinks;

		// Token: 0x0400645B RID: 25691
		private string cachedDescription;
	}
}
