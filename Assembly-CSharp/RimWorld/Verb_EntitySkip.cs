using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200377D RID: 14205
	public class Verb_EntitySkip : Verb_CastAbility
	{
		// Token: 0x06013ADE RID: 80606 RVA: 0x005D8C4A File Offset: 0x005D6E4A
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return ModLister.AnomalyInstalled && base.IsApplicableTo(target, showMessages);
		}

		// Token: 0x06013ADF RID: 80607 RVA: 0x005D8C62 File Offset: 0x005D6E62
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x06013AE0 RID: 80608 RVA: 0x005D8C71 File Offset: 0x005D6E71
		public override void OnGUI(LocalTargetInfo target)
		{
			base.DrawAttachmentExtraLabel(target);
		}
	}
}
