using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001801 RID: 6145
	public class ThoughtWorker_Precept_IdeoDiversity_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008E8C RID: 36492 RVA: 0x002A35E9 File Offset: 0x002A17E9
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return p.Faction == otherPawn.Faction && p.Ideo != otherPawn.Ideo && !otherPawn.DevelopmentalStage.Baby();
		}
	}
}
