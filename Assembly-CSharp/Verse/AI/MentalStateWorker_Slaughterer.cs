using System;

namespace Verse.AI
{
	// Token: 0x02000FA7 RID: 4007
	public class MentalStateWorker_Slaughterer : MentalStateWorker
	{
		// Token: 0x06006298 RID: 25240 RVA: 0x001F1599 File Offset: 0x001EF799
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && SlaughtererMentalStateUtility.FindAnimal(pawn) != null;
		}
	}
}
