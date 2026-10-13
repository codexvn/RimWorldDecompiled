using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002224 RID: 8740
	public class CompAbilityEffect_FireBurst : CompAbilityEffect
	{
		// Token: 0x17001D29 RID: 7465
		// (get) Token: 0x0600BF13 RID: 48915 RVA: 0x00376B36 File Offset: 0x00374D36
		private new CompProperties_AbilityFireBurst Props
		{
			get
			{
				return (CompProperties_AbilityFireBurst)this.props;
			}
		}

		// Token: 0x17001D2A RID: 7466
		// (get) Token: 0x0600BF14 RID: 48916 RVA: 0x00375CA4 File Offset: 0x00373EA4
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x0600BF15 RID: 48917 RVA: 0x00376B44 File Offset: 0x00374D44
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			GenExplosion.DoExplosion(this.Pawn.Position, this.Pawn.MapHeld, this.Props.radius, DamageDefOf.Flame, this.Pawn, -1, -1f, null, null, null, null, ThingDefOf.Filth_Fuel, 1f, 1, null, null, 255, false, null, 0f, 1, 1f, false, null, null, null, false, 0.6f, 0f, true, null, 1f, null, null, null, null);
			base.Apply(target, dest);
		}

		// Token: 0x0600BF16 RID: 48918 RVA: 0x00376BED File Offset: 0x00374DED
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo a, LocalTargetInfo b)
				{
					this.parent.AddEffecterToMaintain(EffecterDefOf.Fire_Burst.Spawn(this.parent.pawn.Position, this.parent.pawn.Map, 1f), this.parent.pawn.Position, 17, this.parent.pawn.Map);
				},
				ticksAwayFromCast = 17
			};
			yield break;
		}

		// Token: 0x0600BF17 RID: 48919 RVA: 0x00376C00 File Offset: 0x00374E00
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			if (this.Pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					return pawn.TargetCurrentlyAimingAt == this.Pawn;
				}
			}
			return false;
		}

		// Token: 0x0600BF18 RID: 48920 RVA: 0x00376C52 File Offset: 0x00374E52
		public override void CompTickInterval(int delta)
		{
			if (this.parent.Casting)
			{
				FireBurstUtility.ThrowFuelTick(this.Pawn.Position, this.Props.radius, this.Pawn.Map);
			}
		}
	}
}
