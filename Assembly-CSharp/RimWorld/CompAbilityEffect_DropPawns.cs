using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002216 RID: 8726
	public class CompAbilityEffect_DropPawns : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D1F RID: 7455
		// (get) Token: 0x0600BEDC RID: 48860 RVA: 0x0037596D File Offset: 0x00373B6D
		public new CompProperties_DropPawns Props
		{
			get
			{
				return (CompProperties_DropPawns)this.props;
			}
		}

		// Token: 0x0600BEDD RID: 48861 RVA: 0x0037597C File Offset: 0x00373B7C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < this.Props.amount; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.Props.pawnKindDef, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.Map, list, 110, false, false, true, true, true, null);
		}
	}
}
