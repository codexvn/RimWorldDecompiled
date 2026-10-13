using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C4 RID: 10180
	public abstract class RitualStageAction : IExposable
	{
		// Token: 0x0600DEF1 RID: 57073 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Apply(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600DEF2 RID: 57074 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
		}

		// Token: 0x0600DEF3 RID: 57075
		public abstract void ExposeData();
	}
}
