using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E7 RID: 14055
	public class StatPart_LifeStageMaxFood : StatPart
	{
		// Token: 0x060137FB RID: 79867 RVA: 0x005C9B68 File Offset: 0x005C7D68
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= pawn.ageTracker.CurLifeStage.foodMaxFactor;
				}
			}
		}

		// Token: 0x060137FC RID: 79868 RVA: 0x005C9BA4 File Offset: 0x005C7DA4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "LifeStageMaxFood".Translate() + ": x" + pawn.ageTracker.CurLifeStage.foodMaxFactor.ToStringPercent();
				}
			}
			return null;
		}
	}
}
