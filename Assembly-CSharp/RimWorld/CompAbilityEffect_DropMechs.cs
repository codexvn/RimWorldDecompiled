using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002214 RID: 8724
	public class CompAbilityEffect_DropMechs : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D1E RID: 7454
		// (get) Token: 0x0600BED7 RID: 48855 RVA: 0x003758C5 File Offset: 0x00373AC5
		public new CompProperties_DropMechs Props
		{
			get
			{
				return (CompProperties_DropMechs)this.props;
			}
		}

		// Token: 0x0600BED8 RID: 48856 RVA: 0x003758D4 File Offset: 0x00373AD4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			PawnGroupMakerParms parms = new PawnGroupMakerParms
			{
				faction = Faction.OfMechanoids,
				groupKind = PawnGroupKindDefOf.Combat,
				points = this.Props.points.RandomInRange,
				tile = this.parent.pawn.MapHeld.Tile
			};
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.MapHeld, PawnGroupMakerUtility.GeneratePawns(parms, true).ToList<Pawn>(), 140, false, false, true, true, true, Faction.OfMechanoids);
		}

		// Token: 0x0600BED9 RID: 48857 RVA: 0x00375887 File Offset: 0x00373A87
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.OdysseyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}

		// Token: 0x04008304 RID: 33540
		private const int PodOpenDelay = 140;
	}
}
