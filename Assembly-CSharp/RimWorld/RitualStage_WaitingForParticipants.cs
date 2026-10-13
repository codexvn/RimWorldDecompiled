using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026C0 RID: 9920
	public class RitualStage_WaitingForParticipants : RitualStage
	{
		// Token: 0x0600D976 RID: 55670 RVA: 0x00405EF8 File Offset: 0x004040F8
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (!ritual.IsParticipating(p))
				{
					return 0f;
				}
			}
			return base.ProgressPerTick(ritual);
		}
	}
}
