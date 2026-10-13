using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A4 RID: 6308
	public class ThoughtWorker_BedroomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x0600904C RID: 36940 RVA: 0x002A8ED3 File Offset: 0x002A70D3
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetBedroomRequirements(false, false);
		}

		// Token: 0x0600904D RID: 36941 RVA: 0x002A8EE4 File Offset: 0x002A70E4
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
