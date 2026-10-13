using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FB RID: 6139
	public class ThoughtWorker_Precept_HasProsthetic_Count : ThoughtWorker_Precept
	{
		// Token: 0x06008E7E RID: 36478 RVA: 0x002A345C File Offset: 0x002A165C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int num = ThoughtWorker_Precept_HasProsthetic_Count.ProstheticsCount(p);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}

		// Token: 0x06008E7F RID: 36479 RVA: 0x002A3483 File Offset: 0x002A1683
		public static int ProstheticsCount(Pawn p)
		{
			return GeneUtility.AddedAndImplantedPartsWithXenogenesCount(p);
		}
	}
}
