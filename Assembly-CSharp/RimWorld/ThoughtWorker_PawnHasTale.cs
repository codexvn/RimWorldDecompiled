using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186F RID: 6255
	public class ThoughtWorker_PawnHasTale : ThoughtWorker
	{
		// Token: 0x06008FC4 RID: 36804 RVA: 0x002A7116 File Offset: 0x002A5316
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, p) == null)
			{
				return false;
			}
			return true;
		}
	}
}
