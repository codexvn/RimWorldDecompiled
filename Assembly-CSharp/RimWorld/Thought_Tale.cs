using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001871 RID: 6257
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x06008FC8 RID: 36808 RVA: 0x002A71EE File Offset: 0x002A53EE
		protected override ThoughtState CurrentStateInternal()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return false;
			}
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x06008FC9 RID: 36809 RVA: 0x002A7228 File Offset: 0x002A5428
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			Tale latestTale = Find.TaleManager.GetLatestTale(this.def.taleDef, this.otherPawn);
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000f);
					num = Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, value);
				}
				return base.CurStage.baseOpinionOffset * num;
			}
			return 0f;
		}
	}
}
