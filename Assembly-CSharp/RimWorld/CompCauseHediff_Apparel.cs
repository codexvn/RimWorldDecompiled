using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C40 RID: 11328
	public class CompCauseHediff_Apparel : ThingComp
	{
		// Token: 0x170027F8 RID: 10232
		// (get) Token: 0x0600FDFC RID: 65020 RVA: 0x0049AAF8 File Offset: 0x00498CF8
		private CompProperties_CauseHediff_Apparel Props
		{
			get
			{
				return (CompProperties_CauseHediff_Apparel)this.props;
			}
		}

		// Token: 0x0600FDFD RID: 65021 RVA: 0x0049AB08 File Offset: 0x00498D08
		public override void Notify_Equipped(Pawn pawn)
		{
			if (pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false) == null)
			{
				HediffComp_RemoveIfApparelDropped hediffComp_RemoveIfApparelDropped = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrFallback((BodyPartRecord p) => p.def == this.Props.part, null), null, null).TryGetComp<HediffComp_RemoveIfApparelDropped>();
				if (hediffComp_RemoveIfApparelDropped != null)
				{
					hediffComp_RemoveIfApparelDropped.wornApparel = (Apparel)this.parent;
				}
			}
		}
	}
}
