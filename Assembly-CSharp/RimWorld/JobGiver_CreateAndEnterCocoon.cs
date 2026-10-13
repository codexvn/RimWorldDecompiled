using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001576 RID: 5494
	public class JobGiver_CreateAndEnterCocoon : JobGiver_CreateAndEnterDryadHolder
	{
		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x0600824D RID: 33357 RVA: 0x00272D65 File Offset: 0x00270F65
		public override JobDef JobDef
		{
			get
			{
				return JobDefOf.CreateAndEnterCocoon;
			}
		}

		// Token: 0x0600824E RID: 33358 RVA: 0x00272D6C File Offset: 0x00270F6C
		public override bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			return connectionComp.DryadKind != pawn.kindDef || base.ExtraValidator(pawn, connectionComp);
		}
	}
}
