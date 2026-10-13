using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F2 RID: 8690
	public class CompAbilityEffect_ConnectingFleckLine : CompAbilityEffect
	{
		// Token: 0x17001D0A RID: 7434
		// (get) Token: 0x0600BE5D RID: 48733 RVA: 0x003733F7 File Offset: 0x003715F7
		public new CompProperties_AbilityConnectingFleckLine Props
		{
			get
			{
				return (CompProperties_AbilityConnectingFleckLine)this.props;
			}
		}

		// Token: 0x0600BE5E RID: 48734 RVA: 0x00373404 File Offset: 0x00371604
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.ConnectingLine(this.parent.pawn.DrawPos, target.CenterVector3, this.Props.fleckDef, this.parent.pawn.Map, 1f);
			}
		}
	}
}
