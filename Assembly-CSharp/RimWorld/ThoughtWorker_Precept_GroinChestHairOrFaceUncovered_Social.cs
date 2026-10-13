using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FA RID: 6138
	public class ThoughtWorker_Precept_GroinChestHairOrFaceUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E7C RID: 36476 RVA: 0x002A344C File Offset: 0x002A164C
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestHairOrFaceUncovered.HasUncoveredGroinChestHairOrFace(otherPawn);
		}
	}
}
