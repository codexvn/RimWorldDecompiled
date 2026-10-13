using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002933 RID: 10547
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x0600E6E7 RID: 59111 RVA: 0x004349FF File Offset: 0x00432BFF
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x0600E6E8 RID: 59112 RVA: 0x00434F9F File Offset: 0x0043319F
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
