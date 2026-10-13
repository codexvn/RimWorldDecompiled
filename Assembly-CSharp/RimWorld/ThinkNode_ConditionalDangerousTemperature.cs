using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A6 RID: 6054
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x06008D97 RID: 36247 RVA: 0x002A0DA8 File Offset: 0x0029EFA8
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
