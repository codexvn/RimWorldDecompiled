using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002254 RID: 8788
	public class CompAbilityEffect_OnlyTargetHostiles : CompAbilityEffect
	{
		// Token: 0x0600BFC3 RID: 49091 RVA: 0x0037972A File Offset: 0x0037792A
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}

		// Token: 0x0600BFC4 RID: 49092 RVA: 0x00379752 File Offset: 0x00377952
		public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}

		// Token: 0x0600BFC5 RID: 49093 RVA: 0x0037972A File Offset: 0x0037792A
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.parent.pawn != null && this.parent.pawn.HostileTo(target.Thing);
		}
	}
}
