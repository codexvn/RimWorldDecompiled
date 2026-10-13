using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001897 RID: 6295
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x0600901F RID: 36895 RVA: 0x002A8290 File Offset: 0x002A6490
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = GeneUtility.AddedAndImplantedPartsWithXenogenesCount(p);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
