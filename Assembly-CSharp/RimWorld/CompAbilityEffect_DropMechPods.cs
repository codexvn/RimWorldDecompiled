using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002212 RID: 8722
	public class CompAbilityEffect_DropMechPods : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x0600BED2 RID: 48850 RVA: 0x003757F5 File Offset: 0x003739F5
		public new CompProperties_DropMechPods Props
		{
			get
			{
				return (CompProperties_DropMechPods)this.props;
			}
		}

		// Token: 0x0600BED3 RID: 48851 RVA: 0x00375804 File Offset: 0x00373A04
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			List<Thing> list = new List<Thing>();
			int randomInRange = this.Props.numPods.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.MechanoidDropPod, null);
				thing.SetFaction(Faction.OfPlayer, null);
				list.Add(thing);
			}
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.MapHeld, list, 140, false, false, true, true, true, Faction.OfMechanoids);
		}

		// Token: 0x0600BED4 RID: 48852 RVA: 0x00375887 File Offset: 0x00373A87
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.OdysseyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}

		// Token: 0x04008302 RID: 33538
		private const int PodOpenDelay = 140;
	}
}
