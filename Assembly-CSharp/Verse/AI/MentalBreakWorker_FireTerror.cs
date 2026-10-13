using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F65 RID: 3941
	public class MentalBreakWorker_FireTerror : MentalBreakWorker
	{
		// Token: 0x0600617B RID: 24955 RVA: 0x001EDC0D File Offset: 0x001EBE0D
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.Spawned && base.BreakCanOccur(pawn) && ThoughtWorker_Pyrophobia.NearFire(pawn);
		}
	}
}
