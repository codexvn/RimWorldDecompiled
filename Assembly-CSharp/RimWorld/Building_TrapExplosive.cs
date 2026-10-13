using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200297B RID: 10619
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x0600E952 RID: 59730 RVA: 0x0043E847 File Offset: 0x0043CA47
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(p);
		}
	}
}
