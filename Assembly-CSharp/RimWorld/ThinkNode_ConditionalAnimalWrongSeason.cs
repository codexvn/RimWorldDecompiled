using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200179F RID: 6047
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x06008D89 RID: 36233 RVA: 0x002A0D04 File Offset: 0x0029EF04
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsAnimal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def, 0f);
		}
	}
}
