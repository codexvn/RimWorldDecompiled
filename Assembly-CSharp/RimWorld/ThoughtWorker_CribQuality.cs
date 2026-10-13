using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E9 RID: 6377
	public class ThoughtWorker_CribQuality : ThoughtWorker
	{
		// Token: 0x0600911C RID: 37148 RVA: 0x002AAFD4 File Offset: 0x002A91D4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed == null || !building_Bed.def.building.bed_crib)
			{
				return false;
			}
			QualityCategory stageIndex;
			building_Bed.TryGetQuality(out stageIndex);
			return ThoughtState.ActiveAtStage((int)stageIndex);
		}
	}
}
