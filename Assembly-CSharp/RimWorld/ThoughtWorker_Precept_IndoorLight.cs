using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001822 RID: 6178
	public class ThoughtWorker_Precept_IndoorLight : ThoughtWorker_Precept
	{
		// Token: 0x06008F05 RID: 36613 RVA: 0x002A49D0 File Offset: 0x002A2BD0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) != PsychGlow.Dark && p.Position.Roofed(p.Map) && !DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
