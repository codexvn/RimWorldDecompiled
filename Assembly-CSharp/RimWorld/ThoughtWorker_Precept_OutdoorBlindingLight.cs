using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001821 RID: 6177
	public class ThoughtWorker_Precept_OutdoorBlindingLight : ThoughtWorker_Precept
	{
		// Token: 0x06008F03 RID: 36611 RVA: 0x002A4974 File Offset: 0x002A2B74
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Overlit && !p.Position.Roofed(p.Map);
		}
	}
}
