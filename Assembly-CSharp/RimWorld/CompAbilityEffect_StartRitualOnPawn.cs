using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200228A RID: 8842
	public class CompAbilityEffect_StartRitualOnPawn : CompAbilityEffect_StartRitual
	{
		// Token: 0x17001D6E RID: 7534
		// (get) Token: 0x0600C086 RID: 49286 RVA: 0x0037C29A File Offset: 0x0037A49A
		public new CompProperties_AbilityStartRitualOnPawn Props
		{
			get
			{
				return (CompProperties_AbilityStartRitualOnPawn)this.props;
			}
		}

		// Token: 0x0600C087 RID: 49287 RVA: 0x0037C2A7 File Offset: 0x0037A4A7
		protected virtual Precept_Ritual RitualForTarget(Pawn pawn)
		{
			return base.Ritual;
		}

		// Token: 0x0600C088 RID: 49288 RVA: 0x0037C2B0 File Offset: 0x0037A4B0
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = target.Pawn;
			Precept_Ritual precept_Ritual = this.RitualForTarget(pawn);
			TargetInfo targetInfo = TargetInfo.Invalid;
			if (precept_Ritual.targetFilter != null)
			{
				targetInfo = precept_Ritual.targetFilter.BestTarget(this.parent.pawn, target.ToTargetInfo(this.parent.pawn.MapHeld));
			}
			return precept_Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, new Dictionary<string, Pawn>
			{
				{
					this.Props.targetRoleId,
					pawn
				}
			}, null);
		}
	}
}
