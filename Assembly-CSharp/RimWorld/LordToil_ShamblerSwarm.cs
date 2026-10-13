using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200171C RID: 5916
	public class LordToil_ShamblerSwarm : LordToil_EntitySwarm
	{
		// Token: 0x06008B48 RID: 35656 RVA: 0x00297C6F File Offset: 0x00295E6F
		public LordToil_ShamblerSwarm(IntVec3 start, IntVec3 dest) : base(start, dest)
		{
		}

		// Token: 0x06008B49 RID: 35657 RVA: 0x00297C79 File Offset: 0x00295E79
		protected override DutyDef GetDutyDef()
		{
			return DutyDefOf.ShamblerSwarm;
		}

		// Token: 0x06008B4A RID: 35658 RVA: 0x00297C40 File Offset: 0x00295E40
		public override bool CanAddPawn(Pawn p)
		{
			return p.IsShambler;
		}
	}
}
