using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188C RID: 6284
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x06009007 RID: 36871 RVA: 0x002A7E90 File Offset: 0x002A6090
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 0.0001f)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.4f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.8f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x06009008 RID: 36872 RVA: 0x002A7EEF File Offset: 0x002A60EF
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
