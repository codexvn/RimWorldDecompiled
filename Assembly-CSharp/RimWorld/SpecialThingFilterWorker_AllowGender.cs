using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D1 RID: 14033
	public abstract class SpecialThingFilterWorker_AllowGender : SpecialThingFilterWorker
	{
		// Token: 0x060137A3 RID: 79779 RVA: 0x005C90CF File Offset: 0x005C72CF
		protected SpecialThingFilterWorker_AllowGender(Gender targetGender)
		{
			this.targetGender = targetGender;
		}

		// Token: 0x060137A4 RID: 79780 RVA: 0x005C90E0 File Offset: 0x005C72E0
		public override bool Matches(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.gender == this.targetGender;
		}

		// Token: 0x060137A5 RID: 79781 RVA: 0x000DA193 File Offset: 0x000D8393
		public override bool CanEverMatch(ThingDef def)
		{
			return def.category == ThingCategory.Pawn;
		}

		// Token: 0x0400D7AC RID: 55212
		private Gender targetGender;
	}
}
