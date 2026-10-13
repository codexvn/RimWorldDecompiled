using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002269 RID: 8809
	public class CompAbilityEffect_ReleaseGas : CompAbilityEffect
	{
		// Token: 0x17001D55 RID: 7509
		// (get) Token: 0x0600C007 RID: 49159 RVA: 0x0037A557 File Offset: 0x00378757
		private new CompProperties_AbilityReleaseGas Props
		{
			get
			{
				return (CompProperties_AbilityReleaseGas)this.props;
			}
		}

		// Token: 0x17001D56 RID: 7510
		// (get) Token: 0x0600C008 RID: 49160 RVA: 0x0037A564 File Offset: 0x00378764
		private int TotalGas
		{
			get
			{
				return Mathf.CeilToInt((float)(this.Props.cellsToFill * 255));
			}
		}

		// Token: 0x0600C009 RID: 49161 RVA: 0x0037A580 File Offset: 0x00378780
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			if (this.Props.gasType == GasType.DeadlifeDust)
			{
				GasUtility.AddDeadifeGas(pawn.Position, pawn.Map, pawn.Faction, this.TotalGas);
			}
			else
			{
				GasUtility.AddGas(pawn.Position, pawn.Map, this.Props.gasType, this.TotalGas);
			}
			base.Apply(target, dest);
		}
	}
}
