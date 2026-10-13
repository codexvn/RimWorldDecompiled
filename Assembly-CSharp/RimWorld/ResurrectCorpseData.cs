using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003793 RID: 14227
	public class ResurrectCorpseData : IExposable
	{
		// Token: 0x06013B42 RID: 80706 RVA: 0x000024DF File Offset: 0x000006DF
		public ResurrectCorpseData()
		{
		}

		// Token: 0x06013B43 RID: 80707 RVA: 0x005DAD43 File Offset: 0x005D8F43
		public ResurrectCorpseData(Corpse corpse, IntVec3 castPosition)
		{
			this.corpse = corpse;
			this.castPosition = castPosition;
		}

		// Token: 0x06013B44 RID: 80708 RVA: 0x005DAD59 File Offset: 0x005D8F59
		public void ExposeData()
		{
			Scribe_References.Look<Corpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<IntVec3>(ref this.castPosition, "castPosition", IntVec3.Invalid, false);
		}

		// Token: 0x0400DA3F RID: 55871
		public Corpse corpse;

		// Token: 0x0400DA40 RID: 55872
		public IntVec3 castPosition;
	}
}
