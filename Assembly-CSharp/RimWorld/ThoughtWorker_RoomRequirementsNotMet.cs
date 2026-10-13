using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A2 RID: 6306
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		// Token: 0x06009045 RID: 36933
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		// Token: 0x06009046 RID: 36934 RVA: 0x002A8E3F File Offset: 0x002A703F
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		// Token: 0x06009047 RID: 36935 RVA: 0x002A8E6C File Offset: 0x002A706C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
