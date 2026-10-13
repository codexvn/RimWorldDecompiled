using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F9 RID: 8697
	public class CompAbilityEffect_Bombardment : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D0F RID: 7439
		// (get) Token: 0x0600BE78 RID: 48760 RVA: 0x00373AB0 File Offset: 0x00371CB0
		public new CompProperties_Bombardment Props
		{
			get
			{
				return (CompProperties_Bombardment)this.props;
			}
		}

		// Token: 0x0600BE79 RID: 48761 RVA: 0x00373AC0 File Offset: 0x00371CC0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			bombardment.duration = this.Props.durationTicks;
			bombardment.instigator = this.parent.pawn;
			bombardment.StartStrike();
		}
	}
}
