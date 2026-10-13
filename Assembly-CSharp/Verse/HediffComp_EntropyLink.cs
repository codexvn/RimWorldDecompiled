using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006B3 RID: 1715
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000E116F File Offset: 0x000DF36F
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000E117C File Offset: 0x000DF37C
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, source);
			HediffComp_Link hediffComp_Link = this.parent.TryGetComp<HediffComp_Link>();
			if (hediffComp_Link != null && hediffComp_Link.other != source && hediffComp_Link.OtherPawn.psychicEntropy != null)
			{
				hediffComp_Link.OtherPawn.psychicEntropy.TryAddEntropy(baseAmount * this.Props.entropyTransferAmount, this.parent.pawn, false, false);
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, hediffComp_Link.other);
			}
		}
	}
}
