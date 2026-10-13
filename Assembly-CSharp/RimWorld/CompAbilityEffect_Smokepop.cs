using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200227B RID: 8827
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		// Token: 0x17001D64 RID: 7524
		// (get) Token: 0x0600C057 RID: 49239 RVA: 0x0037B5A6 File Offset: 0x003797A6
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		// Token: 0x17001D65 RID: 7525
		// (get) Token: 0x0600C058 RID: 49240 RVA: 0x0037B5B3 File Offset: 0x003797B3
		public bool ShouldHaveInspectString
		{
			get
			{
				return ModsConfig.BiotechActive && this.parent.pawn.RaceProps.IsMechanoid;
			}
		}

		// Token: 0x0600C059 RID: 49241 RVA: 0x0037B5D4 File Offset: 0x003797D4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.BlindSmoke), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0600C05A RID: 49242 RVA: 0x0037B672 File Offset: 0x00379872
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}

		// Token: 0x0600C05B RID: 49243 RVA: 0x0037B68C File Offset: 0x0037988C
		public override string CompInspectStringExtra()
		{
			if (!this.ShouldHaveInspectString)
			{
				return null;
			}
			if (this.parent.CanCast)
			{
				return "AbilityMechSmokepopCharged".Translate();
			}
			return "AbilityMechSmokepopRecharging".Translate(this.parent.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false));
		}
	}
}
