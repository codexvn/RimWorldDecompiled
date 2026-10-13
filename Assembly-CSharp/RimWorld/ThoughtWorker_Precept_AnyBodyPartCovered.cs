using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EB RID: 6123
	public class ThoughtWorker_Precept_AnyBodyPartCovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E56 RID: 36438 RVA: 0x002A2D14 File Offset: 0x002A0F14
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(p);
		}

		// Token: 0x06008E57 RID: 36439 RVA: 0x002A2D24 File Offset: 0x002A0F24
		public static bool HasUnnecessarilyCoveredBodyParts(Pawn p)
		{
			return p.apparel != null && p.apparel.AnyClothing && PawnUtility.HasClothingNotRequiredByKind(p) && GenTemperature.SafeTemperatureRange(p.def, null).Includes(p.AmbientTemperature);
		}
	}
}
