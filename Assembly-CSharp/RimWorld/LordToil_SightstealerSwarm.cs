using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001725 RID: 5925
	public class LordToil_SightstealerSwarm : LordToil_EntitySwarm
	{
		// Token: 0x06008B76 RID: 35702 RVA: 0x00297C6F File Offset: 0x00295E6F
		public LordToil_SightstealerSwarm(IntVec3 start, IntVec3 dest) : base(start, dest)
		{
		}

		// Token: 0x06008B77 RID: 35703 RVA: 0x00298CF1 File Offset: 0x00296EF1
		protected override DutyDef GetDutyDef()
		{
			return DutyDefOf.SightstealerSwarm;
		}

		// Token: 0x06008B78 RID: 35704 RVA: 0x00298CF8 File Offset: 0x00296EF8
		public override bool CanAddPawn(Pawn p)
		{
			return p.kindDef == PawnKindDefOf.Sightstealer;
		}
	}
}
