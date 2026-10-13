using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022BE RID: 8894
	public class Verb_CastAbilityTouch : Verb_CastAbility
	{
		// Token: 0x0600C181 RID: 49537 RVA: 0x003806BA File Offset: 0x0037E8BA
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.IsApplicableTo(target, false))
			{
				GenDraw.DrawTargetHighlight(target);
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x0600C182 RID: 49538 RVA: 0x003806E1 File Offset: 0x0037E8E1
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			base.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x0600C183 RID: 49539 RVA: 0x0038070C File Offset: 0x0037E90C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!this.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, showMessages))
				{
					return false;
				}
			}
			return true;
		}
	}
}
