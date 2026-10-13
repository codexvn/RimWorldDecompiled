using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B1D RID: 11037
	public interface ICompAbilityEffectOnJumpCompleted
	{
		// Token: 0x0600F69E RID: 63134
		void OnJumpCompleted(IntVec3 origin, LocalTargetInfo target);
	}
}
