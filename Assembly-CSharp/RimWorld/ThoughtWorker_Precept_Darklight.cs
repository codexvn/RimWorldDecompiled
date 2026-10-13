using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001823 RID: 6179
	public class ThoughtWorker_Precept_Darklight : ThoughtWorker_Precept
	{
		// Token: 0x06008F07 RID: 36615 RVA: 0x002A4A3B File Offset: 0x002A2C3B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
