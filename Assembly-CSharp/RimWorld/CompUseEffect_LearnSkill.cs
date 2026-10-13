using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F66 RID: 12134
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17002C36 RID: 11318
		// (get) Token: 0x0601118D RID: 70029 RVA: 0x004E8CCE File Offset: 0x004E6ECE
		public CompProperties_UseEffect_LearnSkill Props
		{
			get
			{
				return (CompProperties_UseEffect_LearnSkill)this.props;
			}
		}

		// Token: 0x0601118E RID: 70030 RVA: 0x004E8CDC File Offset: 0x004E6EDC
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			int level = user.skills.GetSkill(this.Props.skill).GetLevel(true);
			user.skills.Learn(this.Props.skill, this.Props.xpGainAmount, true, false);
			int level2 = user.skills.GetSkill(this.Props.skill).GetLevel(true);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("SkillNeurotrainerUsed".Translate(user.LabelShort, this.Props.skill.LabelCap, level, level2, user.Named("USER")), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0601118F RID: 70031 RVA: 0x004E8DAC File Offset: 0x004E6FAC
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (p.skills == null)
			{
				return false;
			}
			if (p.skills.GetSkill(this.Props.skill).TotallyDisabled)
			{
				return "SkillDisabled".Translate();
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x06011190 RID: 70032 RVA: 0x004E8DFC File Offset: 0x004E6FFC
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompUseEffect_LearnSkill compUseEffect_LearnSkill = other.TryGetComp<CompUseEffect_LearnSkill>();
			return compUseEffect_LearnSkill != null && compUseEffect_LearnSkill.Props.skill == this.Props.skill;
		}
	}
}
