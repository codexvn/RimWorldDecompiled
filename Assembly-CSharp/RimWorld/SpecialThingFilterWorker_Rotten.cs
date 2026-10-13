using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BF RID: 14015
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		// Token: 0x06013763 RID: 79715 RVA: 0x005C8CB0 File Offset: 0x005C6EB0
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && !compRottable.PropsRot.rotDestroys && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x06013764 RID: 79716 RVA: 0x005C8CE0 File Offset: 0x005C6EE0
		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
