using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002807 RID: 10247
	public class CompRitualEffect_SpawnOnRole : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x17002233 RID: 8755
		// (get) Token: 0x0600E00F RID: 57359 RVA: 0x0041C974 File Offset: 0x0041AB74
		protected new CompProperties_RitualEffectSpawnOnRole Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnRole)this.props;
			}
		}

		// Token: 0x0600E010 RID: 57360 RVA: 0x0041C981 File Offset: 0x0041AB81
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.PawnWithRole(this.Props.roleId);
		}
	}
}
