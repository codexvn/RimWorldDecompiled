using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D41 RID: 15681
	public static class CaravanBedUtility
	{
		// Token: 0x060155D6 RID: 87510 RVA: 0x00663708 File Offset: 0x00661908
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		// Token: 0x060155D7 RID: 87511 RVA: 0x00663714 File Offset: 0x00661914
		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			if (caravan == null)
			{
				return null;
			}
			return caravan.beds.GetBedUsedBy(p);
		}

		// Token: 0x060155D8 RID: 87512 RVA: 0x00663739 File Offset: 0x00661939
		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x060155D9 RID: 87513 RVA: 0x00663758 File Offset: 0x00661958
		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount == 1) ? "UsingBedroll".Translate() : "UsingBedrolls".Translate(bedCount);
			return str + " (" + str2 + ")";
		}
	}
}
