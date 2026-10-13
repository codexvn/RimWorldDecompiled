using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B9 RID: 9913
	public class RitualStage_InteractWithAnimal : RitualStage
	{
		// Token: 0x0600D95C RID: 55644 RVA: 0x00405B24 File Offset: 0x00403D24
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.RaceProps.Animal);
		}
	}
}
