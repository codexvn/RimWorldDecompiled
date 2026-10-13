using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A3 RID: 6307
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06009049 RID: 36937 RVA: 0x002A8E83 File Offset: 0x002A7083
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		// Token: 0x0600904A RID: 36938 RVA: 0x002A8E92 File Offset: 0x002A7092
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().Named("TITLE"));
		}
	}
}
