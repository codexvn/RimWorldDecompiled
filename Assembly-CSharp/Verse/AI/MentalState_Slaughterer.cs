using System;

namespace Verse.AI
{
	// Token: 0x02000F90 RID: 3984
	public class MentalState_Slaughterer : MentalState_SlaughterThing
	{
		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x0600624B RID: 25163 RVA: 0x001F0A78 File Offset: 0x001EEC78
		protected override bool SlaughterTargetAvailable
		{
			get
			{
				return SlaughtererMentalStateUtility.FindAnimal(this.pawn) != null;
			}
		}
	}
}
