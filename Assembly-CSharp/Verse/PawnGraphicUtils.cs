using System;

namespace Verse
{
	// Token: 0x020005F2 RID: 1522
	public static class PawnGraphicUtils
	{
		// Token: 0x060026EF RID: 9967 RVA: 0x000CE788 File Offset: 0x000CC988
		public static bool TryGetAlternate(this Pawn pawn, out AlternateGraphic ag, out int index)
		{
			ag = null;
			index = -1;
			Rand.PushState(pawn.thingIDNumber ^ 46101);
			if (Rand.Chance(pawn.kindDef.alternateGraphicChance))
			{
				if (pawn.kindDef.alternateGraphics.TryRandomElementByWeight((AlternateGraphic x) => x.Weight, out ag))
				{
					index = pawn.kindDef.alternateGraphics.IndexOf(ag);
				}
			}
			Rand.PopState();
			return ag != null;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000CE810 File Offset: 0x000CCA10
		public static int GetGraphicIndex(this Pawn pawn)
		{
			AlternateGraphic alternateGraphic;
			int result;
			if (pawn.TryGetAlternate(out alternateGraphic, out result))
			{
				return result;
			}
			return -1;
		}
	}
}
