using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200224F RID: 8783
	public class CompAbilityEffect_MustBeCapableOf : CompAbilityEffect
	{
		// Token: 0x17001D46 RID: 7494
		// (get) Token: 0x0600BFB1 RID: 49073 RVA: 0x00378EA3 File Offset: 0x003770A3
		public new CompProperties_AbilityMustBeCapableOf Props
		{
			get
			{
				return (CompProperties_AbilityMustBeCapableOf)this.props;
			}
		}

		// Token: 0x0600BFB2 RID: 49074 RVA: 0x00378EB0 File Offset: 0x003770B0
		public override bool GizmoDisabled(out string reason)
		{
			foreach (WorkTags workTags in this.Props.workTags.GetAllSelectedItems<WorkTags>())
			{
				if (this.parent.pawn.WorkTagIsDisabled(workTags))
				{
					reason = "AbilityDisabled_IncapableOfWorkTag".Translate(this.parent.pawn.Named("PAWN"), workTags.LabelTranslated());
					return true;
				}
			}
			reason = null;
			return false;
		}
	}
}
