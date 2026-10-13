using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F2F RID: 12079
	public class ComTargetEffect_FleckConnecting : CompTargetEffect
	{
		// Token: 0x17002C0F RID: 11279
		// (get) Token: 0x060110CD RID: 69837 RVA: 0x004E6861 File Offset: 0x004E4A61
		private CompProperties_TargetEffect_FleckConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckConnecting)this.props;
			}
		}

		// Token: 0x060110CE RID: 69838 RVA: 0x004E686E File Offset: 0x004E4A6E
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.ConnectingLine(user.DrawPos, target.DrawPos, this.Props.fleckDef, user.Map, 1f);
			}
		}
	}
}
