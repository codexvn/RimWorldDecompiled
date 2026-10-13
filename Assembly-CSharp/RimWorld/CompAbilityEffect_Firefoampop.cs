using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002228 RID: 8744
	public class CompAbilityEffect_Firefoampop : CompAbilityEffect
	{
		// Token: 0x17001D2D RID: 7469
		// (get) Token: 0x0600BF25 RID: 48933 RVA: 0x00376E68 File Offset: 0x00375068
		public new CompProperties_AbilityFirefoampop Props
		{
			get
			{
				return (CompProperties_AbilityFirefoampop)this.props;
			}
		}

		// Token: 0x17001D2E RID: 7470
		// (get) Token: 0x0600BF26 RID: 48934 RVA: 0x00376E75 File Offset: 0x00375075
		public bool ShouldHaveInspectString
		{
			get
			{
				return ModsConfig.BiotechActive && this.parent.pawn.RaceProps.IsMechanoid && this.parent.pawn.IsColonyMech;
			}
		}

		// Token: 0x0600BF27 RID: 48935 RVA: 0x00376EA8 File Offset: 0x003750A8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Effecter effecter = EffecterDefOf.ExtinguisherExplosion.Spawn();
			effecter.Trigger(new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), new TargetInfo(target.Cell, this.parent.pawn.MapHeld, false), -1);
			effecter.Cleanup();
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.firefoamRadius, DamageDefOf.Extinguish, null, -1, -1f, SoundDefOf.Explosion_FirefoamPopper, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 1, null, null, 255, true, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0600BF28 RID: 48936 RVA: 0x00376FA2 File Offset: 0x003751A2
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.firefoamRadius);
		}

		// Token: 0x0600BF29 RID: 48937 RVA: 0x00376FBC File Offset: 0x003751BC
		public override string CompInspectStringExtra()
		{
			if (!this.ShouldHaveInspectString)
			{
				return null;
			}
			if (this.parent.CanCast)
			{
				return "AbilityMechFirefoampopCharged".Translate();
			}
			return "AbilityMechFirefoampopRecharging".Translate(this.parent.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false));
		}
	}
}
