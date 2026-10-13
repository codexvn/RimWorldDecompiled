using System;
using RimWorld.Utility;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003780 RID: 14208
	public class Verb_Jump : Verb
	{
		// Token: 0x17003255 RID: 12885
		// (get) Token: 0x06013AEC RID: 80620 RVA: 0x005D8D78 File Offset: 0x005D6F78
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

		// Token: 0x17003256 RID: 12886
		// (get) Token: 0x06013AED RID: 80621 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06013AEE RID: 80622 RVA: 0x005D8DC8 File Offset: 0x005D6FC8
		protected override bool TryCastShot()
		{
			return JumpUtility.DoJump(this.CasterPawn, this.currentTarget, base.ReloadableCompSource, this.verbProps, null, default(LocalTargetInfo), null);
		}

		// Token: 0x06013AEF RID: 80623 RVA: 0x0038057D File Offset: 0x0037E77D
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			JumpUtility.OrderJump(this.CasterPawn, target, this, this.EffectiveRange);
		}

		// Token: 0x06013AF0 RID: 80624 RVA: 0x005D8E00 File Offset: 0x005D7000
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return this.caster != null && this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06013AF1 RID: 80625 RVA: 0x003805EC File Offset: 0x0037E7EC
		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			return JumpUtility.CanHitTargetFrom(this.CasterPawn, root, targ, this.EffectiveRange);
		}

		// Token: 0x06013AF2 RID: 80626 RVA: 0x005D8E58 File Offset: 0x005D7058
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell))
			{
				base.OnGUI(target);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06013AF3 RID: 80627 RVA: 0x005D8E94 File Offset: 0x005D7094
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (this.caster != null && !this.caster.Spawned)
			{
				return;
			}
			if (target.IsValid && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, target.Cell))
			{
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			GenDraw.DrawRadiusRing(this.caster.Position, this.EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(this.caster.Position, c, this.caster.Map) && JumpUtility.ValidJumpTarget(this.caster, this.caster.Map, c));
		}

		// Token: 0x0400DA04 RID: 55812
		private float cachedEffectiveRange = -1f;
	}
}
