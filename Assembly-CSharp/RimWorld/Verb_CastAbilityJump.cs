using System;
using RimWorld.Utility;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022BD RID: 8893
	public class Verb_CastAbilityJump : Verb_CastAbility
	{
		// Token: 0x17001DA3 RID: 7587
		// (get) Token: 0x0600C176 RID: 49526 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DA4 RID: 7588
		// (get) Token: 0x0600C177 RID: 49527 RVA: 0x003804AE File Offset: 0x0037E6AE
		public virtual ThingDef JumpFlyerDef
		{
			get
			{
				return ThingDefOf.PawnFlyer;
			}
		}

		// Token: 0x17001DA5 RID: 7589
		// (get) Token: 0x0600C178 RID: 49528 RVA: 0x003804B8 File Offset: 0x0037E6B8
		public override float EffectiveRange
		{
			get
			{
				if (this.cachedEffectiveRange < 0f)
				{
					if (base.EquipmentSource != null)
					{
						this.cachedEffectiveRange = base.EquipmentSource.GetStatValue(StatDefOf.JumpRange, true, -1);
					}
					else
					{
						this.cachedEffectiveRange = base.EffectiveRange;
					}
				}
				return this.cachedEffectiveRange;
			}
		}

		// Token: 0x0600C179 RID: 49529 RVA: 0x00380506 File Offset: 0x0037E706
		protected override bool TryCastShot()
		{
			return base.TryCastShot() && JumpUtility.DoJump(this.CasterPawn, this.currentTarget, base.ReloadableCompSource, this.verbProps, this.ability, base.CurrentTarget, this.JumpFlyerDef);
		}

		// Token: 0x0600C17A RID: 49530 RVA: 0x00380541 File Offset: 0x0037E741
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600C17B RID: 49531 RVA: 0x0038057D File Offset: 0x0037E77D
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			JumpUtility.OrderJump(this.CasterPawn, target, this, this.EffectiveRange);
		}

		// Token: 0x0600C17C RID: 49532 RVA: 0x00380594 File Offset: 0x0037E794
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.caster != null && this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x0600C17D RID: 49533 RVA: 0x003805EC File Offset: 0x0037E7EC
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			return JumpUtility.CanHitTargetFrom(this.CasterPawn, root, targ, this.EffectiveRange);
		}

		// Token: 0x0600C17E RID: 49534 RVA: 0x00380604 File Offset: 0x0037E804
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && JumpUtility.ValidJumpTarget(this.CasterPawn, this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, c));
		}

		// Token: 0x0400843D RID: 33853
		private float cachedEffectiveRange = -1f;
	}
}
