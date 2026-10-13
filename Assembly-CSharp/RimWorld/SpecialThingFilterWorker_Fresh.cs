using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C0 RID: 14016
	public class SpecialThingFilterWorker_Fresh : SpecialThingFilterWorker
	{
		// Token: 0x06013766 RID: 79718 RVA: 0x005C8D04 File Offset: 0x005C6F04
		public override bool Matches(Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return t.def.IsIngestible && !t.def.IsDrug;
			}
			return compRottable.Stage == RotStage.Fresh;
		}

		// Token: 0x06013767 RID: 79719 RVA: 0x005C8D42 File Offset: 0x005C6F42
		public override bool CanEverMatch(ThingDef def)
		{
			return def.GetCompProperties<CompProperties_Rottable>() != null || (def.IsIngestible && !def.IsDrug);
		}

		// Token: 0x06013768 RID: 79720 RVA: 0x005C8D64 File Offset: 0x005C6F64
		public override bool AlwaysMatches(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return (compProperties != null && compProperties.rotDestroys) || (compProperties == null && def.IsIngestible && !def.IsDrug);
		}
	}
}
